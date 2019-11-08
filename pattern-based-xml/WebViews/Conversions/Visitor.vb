Imports System.Linq.Expressions
Imports System.Reflection

#Disable Warning BC42105 ' Function doesn't return a value on all code paths

Public Module ExpressionTreeConverter

    Public Function ConvertToJavaScript(expression As Expression) As String
        Dim rewritten = New ExpressionTreeCleanupRewriter().Visit(expression)

        rewritten = New DotNetObjectsToJavaScriptObjectsRewriter().Visit(rewritten)

        Return JavaScriptWritingVisitor.Convert(rewritten)
    End Function

    Public Sub WriteToJavaScript(expression As Expression, writer As HtmlTextWriter, Optional escapeQuotes As Boolean = False)
        JavaScriptWritingVisitor.WriteToJavaScript(expression, writer, escapeQuotes)
    End Sub

End Module


Public Class ExpressionVisitorBase
    Inherits ExpressionVisitor

    Protected Shared Function GetSubInfo(overload As Action) As MethodInfo
        Return overload.Method
    End Function

    Protected Shared Function GetSubInfo(Of T1)(overload As Action(Of T1)) As MethodInfo
        Return overload.Method
    End Function

    Protected Shared Function GetSubInfo(Of T1, T2)(overload As Action(Of T1, T2)) As MethodInfo
        Return overload.Method
    End Function

    Protected Shared Function GetFunctionInfo(Of T1)(overload As Func(Of T1)) As MethodInfo
        Return overload.Method
    End Function

    Protected Shared Function GetFunctionInfo(Of T1, T2)(overload As Func(Of T1, T2)) As MethodInfo
        Return overload.Method
    End Function

    Protected Shared Function GetFunctionInfo(Of T1, T2, T3, T4)(overload As Func(Of T1, T2, T3, T4)) As MethodInfo
        Return overload.Method
    End Function

    Protected Shared Function GetPropertyInfo(Of T)(expression As Expression(Of Func(Of T))) As PropertyInfo
        Return CType(expression.Body, MemberExpression).Member
    End Function

    Protected Shared Function IsInstanceParameter(expression As Expression) As Boolean
        Return TryCast(expression, ParameterExpression)?.Name = "$instance"
    End Function

    Protected Shared Function IsVBBuiltInType(type As Type) As Boolean

        Select Case type
            Case GetType(String),
                 GetType(Decimal),
                 GetType(Date)

                Return True

            Case Else
                Return type.IsPrimitive

        End Select

    End Function

    Protected Shared Function ErasedStringConversion(operand As Object) As String
        Throw New InvalidOperationException()
    End Function

End Class

Class ExpressionTreeCleanupRewriter
    Inherits ExpressionVisitorBase

    ' Property assignment is translated as P.set(value) rather than P = value.
    Protected Overrides Function VisitMethodCall(node As MethodCallExpression) As Expression
        If node.Method.Name.StartsWith("set_") Then
            Return Expression.Assign(Expression.Property(Visit(node.Object), node.Method.Name.Substring(4)), Visit(node.Arguments(0)))
        End If

        Return MyBase.VisitMethodCall(node)
    End Function

    ' `Me` access is translated as ConstantValue(Me) rather than Parameter(Me).
    Protected Overrides Function VisitConstant(node As ConstantExpression) As Expression

        If Not IsVBBuiltInType(node.Type) Then
            Return Expression.Parameter(node.Type, "$instance")
        End If

        Return MyBase.VisitConstant(node)

    End Function

End Class

Class DotNetObjectsToJavaScriptObjectsRewriter
    Inherits ExpressionVisitorBase

    Private System_DateTime_NowProperty As PropertyInfo = GetPropertyInfo(Function() Date.Now)

    Protected Overrides Function VisitMember(node As MemberExpression) As Expression

        If node.Member = System_DateTime_NowProperty Then
            Return Expression.[New](GetType(JS.Date))
        End If

        If node.Expression Is Nothing OrElse TryCast(node.Expression, ParameterExpression)?.Name = "$instance" Then
            Return Expression.Call(HtmlDom.Document.DocumentObject, NameOf(HtmlDom.Document.getElementById), {}, Expression.Constant(node.Member.Name))
        End If

        Select Case node.Member.MemberType
            Case MemberTypes.Property, MemberTypes.Field

                Return Expression.PropertyOrField(Visit(node.Expression), node.Member.Name)

        End Select

        Return MyBase.VisitMember(node)
    End Function

    Shared ReadOnly Microsoft_VisualBasic_Interaction_MsgBoxMethod As MethodInfo =
        GetSubInfo(Of String)(AddressOf Microsoft.VisualBasic.MsgBox)

    Protected Overrides Function VisitMethodCall(node As MethodCallExpression) As Expression

        If node.Method = Microsoft_VisualBasic_Interaction_MsgBoxMethod Then
            Return VisitMsgBoxCall(node)
        End If

        If node.Object IsNot Nothing Then

            Dim newObject = Visit(node.Object)

            If IsInstanceParameter(newObject) Then

                Return ConvertToWebApiCall(node)

            ElseIf newObject.Type = GetType(JS.Date) AndAlso node.Method.Name = "ToString" Then

                Return Expression.Call(newObject, GetFunctionInfo(AddressOf New JS.Date().toString), Visit(node.Arguments).ToArray())

            ElseIf IsVBBuiltInType(newObject.Type) AndAlso node.Method.Name = "ToString" AndAlso node.Arguments.Count = 0 Then

                Return Expression.Convert(newObject, GetType(String), GetFunctionInfo(Of Object, String)(AddressOf ErasedStringConversion))

            Else
                Return Expression.Call(newObject, node.Method.Name, {}, Visit(node.Arguments).ToArray())
            End If

        End If

        Return MyBase.VisitMethodCall(node)
    End Function

    Private Function ConvertToWebApiCall(node As MethodCallExpression) As Expression

        Return Expression.Call(HtmlDom.Window.WindowObject,
                               NameOf(HtmlDom.Window.postRequest),
                               {},
                               Expression.Constant(node.Method.Name),
                               ConvertArgumentListToJSObject(node.Method, node.Arguments))

    End Function

    Protected Overrides Function VisitUnary(node As UnaryExpression) As Expression
        If node.NodeType = ExpressionType.Convert Then
            Return VisitConvert(node)
        End If

        Return MyBase.VisitUnary(node)
    End Function

    Protected Function VisitConvert(node As UnaryExpression) As Expression

        Dim rewrittenOperand = Visit(node.Operand)

        If rewrittenOperand.Type = GetType(JS.JSObject) AndAlso node.Type = GetType(String) Then
            Return Expression.Convert(rewrittenOperand, GetType(String), GetFunctionInfo(Of Object, String)(AddressOf ErasedStringConversion))
        End If

        Return MyBase.VisitUnary(node)
    End Function

    Private Function ConvertArgumentListToJSObject(method As MethodInfo, arguments As ObjectModel.ReadOnlyCollection(Of Expression)) As Expression

        Dim parameters = method.GetParameters()

        Dim initializers = New List(Of ElementInit)(parameters.Length)

        Dim addMethod = GetSubInfo(Of String, JS.JSObject)(AddressOf New JS.JSObject().Add)

        For i = 0 To parameters.Length - 1
            initializers.Add(Expression.ElementInit(addMethod, Expression.Constant(parameters(i).Name), Expression.Convert(Visit(arguments(i)), GetType(Object))))
        Next

        Return Expression.ListInit(Expression.[New](GetType(JS.JSObject)), initializers)

    End Function

    Protected Function VisitMsgBoxCall(node As MethodCallExpression) As Expression

        Return Expression.Call(HtmlDom.Window.WindowObject, NameOf(HtmlDom.Window.alert), {}, Visit(node.Arguments(0)))

    End Function


End Class

Public Class JavaScriptWritingVisitor
    Inherits ExpressionVisitorBase

    Private Writer As HtmlTextWriter
    Private EscapeQuotes As Boolean

    Private Sub New(writer As HtmlTextWriter, escapeQuotes As Boolean)
        Me.Writer = writer
        Me.EscapeQuotes = escapeQuotes
    End Sub

    Public Shared Function Convert(expression As Expression) As String
        Using stream As New IO.MemoryStream,
              writer As New HtmlTextWriter(stream, 4),
              reader As New IO.StreamReader(stream)

            If expression.NodeType = ExpressionType.Lambda Then
                expression = CType(expression, LambdaExpression).Body
            End If

            Call New JavaScriptWritingVisitor(writer, escapeQuotes:=False).Visit(expression)
            writer.Flush()

            stream.Seek(0, IO.SeekOrigin.Begin)

            Return reader.ReadToEnd()
        End Using
    End Function

    Public Shared Sub WriteToJavaScript(expression As Expression, writer As HtmlTextWriter, Optional escapeQuotes As Boolean = False)
        Call New JavaScriptWritingVisitor(writer, escapeQuotes).Visit(expression)
    End Sub

    Protected Overrides Function VisitMethodCall(node As MethodCallExpression) As Expression

        If node.Object IsNot Nothing AndAlso node.Object IsNot HtmlDom.Window.WindowObject AndAlso Not IsInstanceParameter(node.Object) Then
            Visit(node.Object)

            Writer.Write(".")
        End If

        Writer.Write(node.Method.Name)

        VisitArgumentList(node.Arguments)
    End Function

    Protected Overrides Function VisitParameter(node As ParameterExpression) As Expression

        Writer.Write(node.Name)

    End Function

    Protected Overrides Function VisitNew(node As NewExpression) As Expression

        Writer.Write("new ")

        Writer.Write(node.Type.Name)

        VisitArgumentList(node.Arguments)

    End Function

    Protected Overrides Function VisitUnary(node As UnaryExpression) As Expression
        Select Case node.NodeType
            Case ExpressionType.Convert,
                 ExpressionType.ConvertChecked

                Dim toType = node.Type,
                    fromType = node.Operand.Type

                If toType.IsAssignableFrom(fromType) Then
                    Return Visit(node.Operand)
                ElseIf IsVBBuiltInType(fromType) AndAlso IsVBBuiltInType(toType) Then
                    Return Visit(node.Operand)
                ElseIf fromType = GetType(JS.JSObject) AndAlso IsVBBuiltInType(toType) Then
                    Return Visit(node.Operand)
                End If
        End Select

        Return MyBase.VisitUnary(node)
    End Function

    Protected Overrides Function VisitLambda(Of T)(node As Expression(Of T)) As Expression
        Writer.WriteLine("function() { ")

        'Writer.Indent()

        If node.ReturnType <> GetType(Void) Then
            Writer.Write("return ")
        End If

        Visit(node.Body)

        Writer.WriteLine(";")

        'Writer.Unindent()

        Writer.WriteLine(" }")
    End Function

    Protected Sub VisitArgumentList(list As ObjectModel.ReadOnlyCollection(Of Expression))
        Writer.Write("(")

        If list.Count > 0 Then
            Visit(list(0))
        End If

        For i = 1 To list.Count - 1
            Writer.Write(", ")

            Visit(list(i))
        Next

        Writer.Write(")")
    End Sub

    Protected Overrides Function VisitMember(node As MemberExpression) As Expression
        If node.Expression Is Nothing Then
            Writer.Write(node.Member.DeclaringType.Name)
            Writer.Write(".")
            Writer.Write(node.Member.Name)
        Else
            Visit(node.Expression)
            Writer.Write(".")
            Writer.Write(node.Member.Name)
        End If
    End Function

    Protected Overrides Function VisitConstant(node As ConstantExpression) As Expression

        Select Case node.Type
            Case GetType(String)
                VisitStringConstant(node.Value)
            Case GetType(Integer)
                Writer.Write(CInt(node.Value))
            Case Else
                Throw New NotImplementedException()
        End Select

    End Function

    Protected Sub VisitStringConstant(value As String)
        WriteDoubleQuote()

        For Each c In value
            Select Case c
                Case """"c
                    Writer.Write("/")
                    WriteDoubleQuote()
                Case Else
                    Writer.Write(c)
            End Select
        Next

        WriteDoubleQuote()
    End Sub

    Protected Sub WriteDoubleQuote()
        If EscapeQuotes Then
            Writer.Write("&quot;")
        Else
            Writer.Write("""")
        End If
    End Sub

    Protected Overrides Function VisitBinary(node As BinaryExpression) As Expression
        Select Case node.NodeType
            Case ExpressionType.Assign

                Visit(node.Left)

                Writer.Write(" = ")

                Visit(node.Right)

            Case Else
                Return MyBase.VisitBinary(node)
        End Select
    End Function

    Protected Overrides Function VisitListInit(node As ListInitExpression) As Expression

        If node.NewExpression.Type = GetType(JS.JSObject) Then
            VisitJsonInitializer(node)
        End If

    End Function

    Protected Function VisitJsonInitializer(node As ListInitExpression) As Expression

        Writer.Write("{")

        Dim initializers = node.Initializers

        Dim i = 0

        If initializers.Count > 0 Then
            GoTo AfterComma
        End If

        Do While i < initializers.Count
            Writer.Write(", ")

AfterComma:
            Dim initializerArguments = initializers(i).Arguments

            VisitStringConstant(CType(initializerArguments(0), ConstantExpression).Value)

            Writer.Write(": ")

            Visit(initializerArguments(1))

            i += 1
        Loop

        Writer.Write("}")

    End Function

End Class

Namespace JS

    Class [Date]

        Public Shadows Function toString() As String
            Throw New InvalidOperationException()
        End Function

    End Class

    Class JSObject
        Implements IEnumerable

        Default Property Item(key As String) As JSObject
            Get
                Throw New InvalidOperationException()
            End Get
            Set(value As JSObject)
                Throw New InvalidOperationException()
            End Set
        End Property

        Sub Add(name As String, value As Object)
            Throw New InvalidOperationException()
        End Sub

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Throw New InvalidOperationException()
        End Function
    End Class

End Namespace

Namespace HtmlDom

    Interface IMarshalByName

    End Interface

    MustInherit Class DomObject

    End Class

    Class Window
        Inherits DomObject

        Public Shared ReadOnly WindowObject As Expression = Expression.Parameter(GetType(Window))

        Public Sub alert(message As Object)
            Throw New InvalidOperationException()
        End Sub

        Public Function postRequest(url As String, params As JS.JSObject) As JS.JSObject
            Throw New InvalidOperationException()
        End Function

    End Class

    Class Document
        Inherits DomObject

        Public Shared ReadOnly DocumentObject As Expression = Expression.Parameter(GetType(Document), "document")

        Public Function getElementById(elementID As String) As Element
            Throw New InvalidOperationException()
        End Function

    End Class

    Class Element
        Inherits DomObject

        Public Property innerText As String
            Get
                Throw New InvalidOperationException()
            End Get
            Set(value As String)
                Throw New InvalidOperationException()
            End Set
        End Property

        Property value As String
            Get
                Throw New InvalidOperationException()
            End Get
            Set(value As String)
                Throw New InvalidOperationException()
            End Set
        End Property

    End Class

End Namespace

Namespace Global.Microsoft.VisualBasic
    Public Module Interaction
        Public Sub MsgBox(prompt As String)
            Throw New NotImplementedException()
        End Sub
    End Module
End Namespace