Imports WebViews

Public MustInherit Class WebElement

    Protected Overridable ReadOnly Property IsEmpty As Boolean
        Get
            Return False
        End Get
    End Property

    Protected Overridable ReadOnly Property NewLineBeforeStartTag As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overridable ReadOnly Property NewLineAfterStartTag As Boolean
        Get
            Return NewLineBeforeStartTag
        End Get
    End Property

    Protected Overridable ReadOnly Property NewLineBeforeEndTag As Boolean
        Get
            Return NewLineAfterStartTag
        End Get
    End Property

    Protected Overridable ReadOnly Property NewLineAfterEndTag As Boolean
        Get
            Return NewLineBeforeStartTag
        End Get
    End Property

    Public Overridable Property DataContext As Object

    Protected ReadOnly Property Bindings As New Dictionary(Of String, Action(Of Object))(StringComparer.OrdinalIgnoreCase)

    Protected Overridable Sub ApplyBindings()
        For Each binding In Bindings.Values
            binding(DataContext)
        Next

        For Each child In GetChildren()
            child.DataContext = DataContext
        Next
    End Sub

    Protected Function TryParseBinding(text As String, ByRef propertyPath As String) As Boolean
        If Not text.StartsWith("{Binding", StringComparison.OrdinalIgnoreCase) Then
            propertyPath = Nothing
            Return False
        End If

        If text.Equals("{Binding}", StringComparison.OrdinalIgnoreCase) Then
            propertyPath = ""
            Return True
        End If

        If text(8) = " "c AndAlso text.EndsWith("}") Then
            propertyPath = text.Substring(9, text.Length - 9 - 1).Trim()
            Return True
        Else
            propertyPath = Nothing
            Return False
        End If
    End Function

    Protected Shared Function GetValueByPath(receiver As Object, propertyPath As String) As Object
        If propertyPath = "" Then Return receiver

        Dim properties = propertyPath.Split("."c)

        Debug.Assert(properties.Length > 0)

        Dim value = receiver

        For i = 0 To properties.Length - 1
            value = ReadPropertyOrAccessDictionaryByName(value, properties(i))
        Next

        Return value
    End Function

    Private Shared Function ReadPropertyOrAccessDictionaryByName(receiver As Object, propertyName As String) As Object
        Dim receiverType = receiver.GetType()

        Dim propertyInfo = receiverType.GetProperty(propertyName)

        If propertyInfo IsNot Nothing Then
            Return propertyInfo.GetValue(receiver, index:={})
        Else
            Throw New MissingMemberException(receiverType.Name, propertyName)
        End If
    End Function

    Public Overridable Sub WriteToHtml(writer As HtmlTextWriter)
        ApplyBindings()

        If NewLineBeforeStartTag Then
            writer.ForceNewLine()
        End If

        writer.WriteIndentIfOnNewLine()

        writer.Write("<")
        writer.Write(GetStartTagName())

        For Each pair In GetAttributes()
            If String.IsNullOrEmpty(pair.Value) Then Continue For

            writer.Write(" "c)
            writer.Write(pair.Key)
            writer.Write("=")
            writer.Write(""""c)
            writer.WriteHtmlEncodedAttributeValue(pair.Value)
            writer.Write(""""c)
        Next

        If IsEmpty Then
            writer.Write("/>")

            If NewLineAfterEndTag Then
                writer.ForceNewLine()
            End If

            Return
        Else
            writer.Write(">")

            If NewLineAfterStartTag Then
                writer.ForceNewLine()
            End If
        End If

        writer.Indent()
        For Each child In GetChildren()
            child.WriteToHtml(writer)
        Next
        writer.Unindent()

        If NewLineBeforeEndTag Then
            writer.ForceNewLine()
        End If

        writer.WriteIndentIfOnNewLine()

        writer.Write("</")
        writer.Write(GetStartTagName())
        writer.Write(">")

        If NewLineAfterEndTag Then
            writer.ForceNewLine()
        End If
    End Sub

    Protected Overridable Sub RegisterToWriteHeaderContent(perTypeHeaderContentWriters As Dictionary(Of Type, WebElement),
                                                           perInstanceHeaderContentWriters As List(Of WebElement))
        For Each child In GetChildren()
            child.RegisterToWriteHeaderContent(perTypeHeaderContentWriters, perInstanceHeaderContentWriters)
        Next
    End Sub

    Protected Shared Sub WritePerTypeHeaderContent(element As WebElement, writer As HtmlTextWriter)
        element.WritePerTypeHeaderContent(writer)
    End Sub

    Protected Overridable Sub WritePerTypeHeaderContent(writer As HtmlTextWriter)
        Return
    End Sub

    Protected Shared Sub WritePerInstanceHeaderContent(element As WebElement, writer As HtmlTextWriter)
        element.WritePerInstanceHeaderContent(writer)
    End Sub

    Protected Overridable Sub WritePerInstanceHeaderContent(writer As HtmlTextWriter)
        Return
    End Sub

    Protected Shared Sub WriteToHtml(child As WebElement, writer As HtmlTextWriter)
        child.WriteToHtml(writer)
    End Sub

    Protected Overridable Function GetStartTagName() As String
        Return Me.GetType().Name.ToLower()
    End Function

    Protected Overridable Function GetAttributes() As IEnumerable(Of KeyValuePair(Of String, String))
        Return {}
    End Function

    Protected Overridable Function GetChildren() As IEnumerable(Of WebElement)
        Return {}
    End Function

    Public Overrides Function ToString() As String
        Using stream = New IO.MemoryStream(),
              writer = New HtmlTextWriter(stream),
              reader = New IO.StreamReader(stream)

            WriteToHtml(writer)
            writer.Flush()

            stream.Seek(0, IO.SeekOrigin.Begin)

            Return reader.ReadToEnd()
        End Using
    End Function
End Class

Public Class AttributeBag
    Inherits Dictionary(Of String, String)

    Default Public Shadows Property Item(name As String) As String
        Get
            Dim value As String = Nothing

            If TryGetValue(name, value) Then
                Return value
            Else
                Return String.Empty
            End If
        End Get
        Set(value As String)
            MyBase.Item(name) = value
        End Set
    End Property
End Class

Public MustInherit Class HtmlWebElement
    Inherits WebElement

    Public ReadOnly Property Attributes As New AttributeBag

    Public Property [Class] As String
        Get
            Return Attributes!class
        End Get
        Set(value As String)
            Attributes!class = value
        End Set
    End Property

    Public Property Style As String
        Get
            Return Attributes!style
        End Get
        Set(value As String)
            Attributes!style = value
        End Set
    End Property

    Public Property Id As String
        Get
            Return Attributes!id
        End Get
        Set(value As String)
            Attributes!id = value
        End Set
    End Property

    Protected Overrides Function GetAttributes() As IEnumerable(Of KeyValuePair(Of String, String))
        Return Attributes
    End Function
End Class

Public MustInherit Class MultiContentWebElement
    Inherits HtmlWebElement

    Protected ReadOnly Property Children As New List(Of WebElement)

    Public Sub AddChildContent(content As WebElement)
        Children.Add(content)
    End Sub

    Public Overridable Sub AddChildContent(content As String)
        Children.Add(New Text(content))
    End Sub

    Protected Overrides Function GetChildren() As IEnumerable(Of WebElement)
        Return Children
    End Function
End Class

Public MustInherit Class MultiContentWebElement(Of T)
    Inherits MultiContentWebElement

End Class

Public MustInherit Class EmptyWebElement
    Inherits HtmlWebElement

    Protected Overrides ReadOnly Property IsEmpty As Boolean
        Get
            Return True
        End Get
    End Property
End Class

Public MustInherit Class ControlWebElement
    Inherits WebElement
End Class

Public Class Html
    Inherits MultiContentWebElement

    Public Property Head As Head

    Public Property Body As Body

    Public Shadows Sub AddChildContent(head As Head)
        If Me.Head IsNot Nothing Then Throw New InvalidOperationException()

        Me.Head = head
    End Sub

    Public Shadows Sub AddChildContent(body As Body)
        If Me.Body IsNot Nothing Then Throw New InvalidOperationException()

        Me.Body = body
    End Sub

    Protected Overrides Iterator Function GetChildren() As IEnumerable(Of WebElement)

        If Head IsNot Nothing Then
            Yield Head
        End If

        If Body IsNot Nothing Then
            Yield Body
        End If

    End Function

    Public Overrides Sub WriteToHtml(writer As HtmlTextWriter)
        ApplyBindings()

        writer.Write("<html>")
        writer.WriteLine()

        writer.Indent()

        writer.WriteIndent()
        writer.Write("<head>")
        writer.WriteLine()

        writer.Indent()

        Dim perTypeHeaderContentWriters = New Dictionary(Of Type, WebElement),
            perInstanceHeaderContentWriters = New List(Of WebElement)

        RegisterToWriteHeaderContent(perTypeHeaderContentWriters, perInstanceHeaderContentWriters)

        For Each contentWriter In perTypeHeaderContentWriters.Values
            WritePerTypeHeaderContent(contentWriter, writer)
        Next

        For Each contentwriter In perInstanceHeaderContentWriters
            WritePerInstanceHeaderContent(contentwriter, writer)
        Next

        If Head IsNot Nothing Then
            Head.WriteToHtml(writer, childrenOnly:=True)
        End If

        writer.Unindent()

        writer.WriteLine()
        writer.WriteIndent()
        writer.Write("</head>")
        writer.WriteLine()

        If Body IsNot Nothing Then
            WriteToHtml(Body, writer)
        End If

        writer.Unindent()

        writer.ForceNewLine()
        writer.Write("</html>")
        writer.WriteLine()
    End Sub

End Class

Public Class Head
    Inherits MultiContentWebElement

    Friend Overloads Sub WriteToHtml(writer As HtmlTextWriter, childrenOnly As Boolean)
        If childrenOnly Then
            For Each child In GetChildren()
                WriteToHtml(child, writer)
            Next
        Else
            MyBase.WriteToHtml(writer)
        End If
    End Sub

End Class

Public Class Title
    Inherits MultiContentWebElement

    Public Sub New()
    End Sub

    Public Sub New(content As String)
        AddChildContent(content)
    End Sub

    Protected Overrides ReadOnly Property NewLineAfterStartTag As Boolean
        Get
            Return False
        End Get
    End Property

End Class

Public Class Body
    Inherits MultiContentWebElement
End Class

Public Class P
    Inherits MultiContentWebElement

    Public Sub New()
    End Sub

    Public Sub New(content As String)
        AddChildContent(content)
    End Sub

    Public Sub SetContent(value As WebElement, valueText As String, otherValue As Object)

        If value IsNot Nothing Then GoTo SetValue

        Dim asWebElement = TryCast(otherValue, WebElement)
        If asWebElement IsNot Nothing Then
            value = asWebElement
        End If

        If value IsNot Nothing Then GoTo SetValue

        Dim asString = TryCast(otherValue, String)
        If asString IsNot Nothing Then
            value = New Text(asString)
        End If

        If value IsNot Nothing Then GoTo SetValue

        Dim propertyPath As String
        If TryParseBinding(valueText, propertyPath) Then
            Bindings!content = Sub(context)
                                   Children.Clear()
                                   Children.Add(New Text(GetValueByPath(context, propertyPath)))
                               End Sub
            Return
        End If

        If valueText IsNot Nothing Then
            value = New Text(valueText)
        End If

SetValue:
        Bindings.Remove("content")
        Children.Clear()

        If value IsNot Nothing Then
            Children.Add(value)
        End If
    End Sub

    Protected Overrides ReadOnly Property NewLineAfterStartTag As Boolean
        Get
            Return False
        End Get
    End Property
End Class

Public Class Span
    Inherits MultiContentWebElement

    Public Sub New()
    End Sub

    Public Sub New(content As String)
        AddChildContent(content)
    End Sub

    Public Sub SetContent(value As WebElement, valueText As String, otherValue As Object)

        If value IsNot Nothing Then GoTo SetValue

        Dim asWebElement = TryCast(otherValue, WebElement)
        If asWebElement IsNot Nothing Then
            value = asWebElement
        End If

        If value IsNot Nothing Then GoTo SetValue

        Dim asString = TryCast(otherValue, String)
        If asString IsNot Nothing Then
            value = New Text(asString)
        End If

        If value IsNot Nothing Then GoTo SetValue

        Dim propertyPath As String
        If TryParseBinding(valueText, propertyPath) Then
            Bindings!content = Sub(context)
                                   Children.Clear()
                                   Children.Add(New Text(GetValueByPath(context, propertyPath)))
                               End Sub
            Return
        End If

        If valueText IsNot Nothing Then
            value = New Text(valueText)
        End If

SetValue:
        Bindings.Remove("content")
        Children.Clear()

        If value IsNot Nothing Then
            Children.Add(value)
        End If
    End Sub

    Protected Overrides ReadOnly Property NewLineBeforeStartTag As Boolean
        Get
            Return False
        End Get
    End Property

    Public Property innerText As String

End Class

Public Class Div
    Inherits MultiContentWebElement
End Class

Public MustInherit Class Header
    Inherits MultiContentWebElement

    Public Sub SetContent(value As WebElement, valueText As String, otherValue As Object)

        If value IsNot Nothing Then GoTo SetValue

        Dim asWebElement = TryCast(otherValue, WebElement)
        If asWebElement IsNot Nothing Then
            value = asWebElement
        End If

        If value IsNot Nothing Then GoTo SetValue

        Dim asString = TryCast(otherValue, String)
        If asString IsNot Nothing Then
            value = New Text(asString)
        End If

        If value IsNot Nothing Then GoTo SetValue

        Dim propertyPath As String
        If TryParseBinding(valueText, propertyPath) Then
            Bindings!content = Sub(context)
                                   Children.Clear()
                                   Children.Add(New Text(GetValueByPath(context, propertyPath)))
                               End Sub
            Return
        End If

        If valueText IsNot Nothing Then
            value = New Text(valueText)
        End If

SetValue:
        Bindings.Remove("content")
        Children.Clear()

        If value IsNot Nothing Then
            Children.Add(value)
        End If
    End Sub

    Protected Overrides ReadOnly Property NewLineBeforeStartTag As Boolean
        Get
            Return True
        End Get
    End Property

    Protected Overrides ReadOnly Property NewLineAfterStartTag As Boolean
        Get
            Return False
        End Get
    End Property
End Class

Public Class H1
    Inherits Header

    Public Sub New()
    End Sub

    Public Sub New(content As String)
        AddChildContent(content)
    End Sub
End Class

Public Class H2
    Inherits Header

    Public Sub New()
    End Sub

    Public Sub New(content As String)
        AddChildContent(content)
    End Sub
End Class

Public Class H3
    Inherits Header

    Public Sub New()
    End Sub

    Public Sub New(content As String)
        AddChildContent(content)
    End Sub
End Class

Public Class Hr
    Inherits EmptyWebElement

    Protected Overrides ReadOnly Property NewLineBeforeStartTag As Boolean
        Get
            Return True
        End Get
    End Property
End Class

Public Class Img
    Inherits EmptyWebElement

    Public Property Src As String
        Get
            Return Attributes!src
        End Get
        Set(value As String)
            Dim propertyPath As String
            If TryParseBinding(value, propertyPath) Then
                Bindings!src = Sub(context) Attributes!src = GetValueByPath(context, propertyPath)
            Else
                Bindings.Remove("src")
                Attributes!src = value
            End If
        End Set
    End Property

    Public Property Alt As String
        Get
            Return Attributes!alt
        End Get
        Set(value As String)
            Dim propertyPath As String
            If TryParseBinding(value, propertyPath) Then
                Bindings!alt = Sub(context) Attributes!alt = GetValueByPath(context, propertyPath)
            Else
                Bindings.Remove("alt")
                Attributes!alt = value
            End If
        End Set
    End Property

End Class

Public Class Input
    Inherits EmptyWebElement

    Public Property Type As InputType
        Get
            Return [Enum].Parse(GetType(InputType), Attributes!type)
        End Get
        Set(value As InputType)
            Attributes!type = value.ToString()
        End Set
    End Property

    Public Property Value As String
        Get
            Return Attributes!value
        End Get
        Set(value As String)
            Attributes!value = value
        End Set
    End Property

End Class

Public Enum InputType
    Number
End Enum

Public Class Button
    Inherits MultiContentWebElement

    Public Sub New()
        Type = "button"
    End Sub

    Public Property Type As String
        Get
            Return Attributes!Type
        End Get
        Set(value As String)
            Attributes!Type = value
        End Set
    End Property

    Public Property OnClick As String
        Get
            Return Attributes!onclick
        End Get
        Set(value As String)
            Attributes!onclick = value
        End Set
    End Property

    Public Sub SetOnClick(value As String, valueText As String, otherValue As Expressions.Expression(Of Action))
        If otherValue IsNot Nothing Then
            Clicked = otherValue
        Else
            OnClick = value
        End If
    End Sub

    Private _Clicked As Expressions.Expression(Of Action)

    Public Property Clicked As Expressions.Expression(Of Action)
        Get
            Return _Clicked
        End Get
        Set(value As Expressions.Expression(Of Action))
            _Clicked = value

            If value Is Nothing Then
                Attributes!onclick = Nothing
            Else
                Attributes!onclick = ExpressionTreeConverter.ConvertToJavaScript(value)
            End If
        End Set
    End Property

    Protected Overrides Sub RegisterToWriteHeaderContent(perTypeHeaderContentWriters As Dictionary(Of Type, WebElement), perInstanceHeaderContentWriters As List(Of WebElement))
        perTypeHeaderContentWriters(GetType(Button)) = Me
    End Sub

    Protected Overrides Sub WritePerTypeHeaderContent(writer As HtmlTextWriter)

        Dim content = <![CDATA[    <script>
    function postRequest(url, data)
    {
        var query = ""
        var separator = "?"
    
        for (name in data) {
            query += separator + name + "=" + data[name]
            separator = "&"
        }
    
    	var request = new XMLHttpRequest()
        request.open("POST", location.pathname + "/" + url + query, false)
        request.send()
        return request.responseText
    }
    </script>]]>.Value

        writer.ForceNewLine()
        writer.Write(content)
        writer.WriteLine()

    End Sub

    'Public Sub SetClicked(value As String, valueText As String, otherValue As Expressions.Expression(Of Action))

    '    If otherValue IsNot Nothing Then
    '        Attributes!onclick = ConvertingVisitor.Convert(otherValue)
    '    Else
    '        Attributes!onclick = value
    '    End If

    'End Sub

End Class

Public Class Script
    Inherits MultiContentWebElement

    Public Overrides Sub AddChildContent(content As String)
        AddChildContent(New RawText(content))
    End Sub
End Class

Public Class Nav
    Inherits MultiContentWebElement
End Class

Public Class Ul
    Inherits MultiContentWebElement
End Class

Public Class Ol
    Inherits MultiContentWebElement
End Class

Public Class Li
    Inherits MultiContentWebElement
End Class

Public Class Br
    Inherits EmptyWebElement

    Protected Overrides ReadOnly Property NewLineBeforeStartTag As Boolean
        Get
            Return False
        End Get
    End Property

    Protected Overrides ReadOnly Property NewLineAfterEndTag As Boolean
        Get
            Return True
        End Get
    End Property
End Class

Public Class A
    Inherits MultiContentWebElement

    Public Sub New()
    End Sub

    Public Sub New(href As String, content As String)
        Me.Href = href

        AddChildContent(content)
    End Sub

    Protected Overrides ReadOnly Property NewLineBeforeStartTag As Boolean
        Get
            Return False
        End Get
    End Property

    Public Property Href As String
        Get
            Return Attributes!href
        End Get
        Set(value As String)
            Attributes!href = value
        End Set
    End Property

    Public Sub SetHref(value As String, valueText As String, otherValue As Object)

        Dim asString = TryCast(otherValue, String)
        If asString IsNot Nothing Then
            value = asString
            GoTo SetValue
        End If

        Dim propertyPath As String
        If TryParseBinding(valueText, propertyPath) Then
            Bindings!href = Sub(context) Href = GetValueByPath(context, propertyPath)
            Return
        End If

SetValue:
        Bindings.Remove("href")

        Href = value
    End Sub

End Class

Public Class Meta
    Inherits EmptyWebElement

    Public Property Charset As String
        Get
            Return Attributes!charset
        End Get
        Set(value As String)
            Attributes!charset = value
        End Set
    End Property

    Public Property Name As String
        Get
            Return Attributes!name
        End Get
        Set(value As String)
            Attributes!name = value
        End Set
    End Property

    Public Property Content As String
        Get
            Return Attributes!content
        End Get
        Set(value As String)
            Attributes!content = value
        End Set
    End Property

End Class

Public Class Style
    Inherits MultiContentWebElement

    Public Overrides Sub AddChildContent(content As String)
        AddChildContent(New RawText(content))
    End Sub
End Class

Public Class Text
    Inherits WebElement

    Public ReadOnly Property Value As String

    Public Sub New(value As String)
        Me.Value = value
    End Sub

    Public Overrides Sub WriteToHtml(writer As HtmlTextWriter)
        writer.WriteHtmlEncoded(Value)
    End Sub
End Class

Public Class RawText
    Inherits WebElement

    Public ReadOnly Property Value As String

    Public Sub New(value As String)
        Me.Value = value
    End Sub

    Public Overrides Sub WriteToHtml(writer As HtmlTextWriter)
        writer.Write(Value)
    End Sub
End Class