Imports System.IO
Imports System.Text

Public Class HtmlTextWriter
    Inherits StreamWriter

    Public ReadOnly Property IndentLevel As Integer = 0

    Private ReadOnly IndentCache As New List(Of String)

    Private IsOnNewLine As Boolean

    Public Sub New(stream As Stream, Optional spacesPerIndent As Integer = 2)
        MyBase.New(stream)

        If spacesPerIndent < 0 Then Throw New ArgumentException(NameOf(spacesPerIndent))

        IndentCache.Add("")

        For i = 1 To 8
            IndentCache.Add(New String(" "c, i * spacesPerIndent))
        Next
    End Sub

    Public ReadOnly Property SpacesPerIndent As Integer
        Get
            Return IndentCache(1).Length
        End Get
    End Property

    Public Overrides ReadOnly Property Encoding As Encoding
        Get
            Return Encoding.UTF8
        End Get
    End Property

    Public Sub Indent()
        _IndentLevel += 1
    End Sub

    Public Sub Unindent()
        If IndentLevel = 0 Then Return

        _IndentLevel -= 1
    End Sub

    Public Sub WriteIndent()
        Write(GetIndentText())
    End Sub

    Public Sub WriteIndentIfOnNewLine()
        If IsOnNewLine Then
            WriteIndent()
        End If
    End Sub

    Private Function GetIndentText() As String
        If IndentLevel <= IndentCache.Count - 1 Then
            Return IndentCache(IndentLevel)
        End If

        Debug.Assert(IndentLevel = IndentCache.Count)

        Dim spacesPerIndent = IndentCache(1).Length

        If IndentCache.Count < 32 Then
            For i = IndentCache.Count To IndentLevel
                IndentCache.Add(New String(" "c, i * spacesPerIndent))
            Next

            Return IndentCache(IndentLevel)
        End If

        Return New String(" "c, IndentLevel * spacesPerIndent)
    End Function

    Public Overrides Sub WriteLine()
        MyBase.WriteLine()

        IsOnNewLine = True
    End Sub

    Public Overrides Sub Write(value As String)
        MyBase.Write(value)

        IsOnNewLine = False
    End Sub

    Public Sub WriteHtmlEncoded(value As String)
        For Each c In value
            Select Case c
                Case """"c
                    MyBase.Write("&quot;")
                Case "&"c
                    MyBase.Write("&amp;")
                Case "<"c
                    MyBase.Write("&lt;")
                Case ">"c
                    MyBase.Write("&gt;")
                Case Else
                    MyBase.Write(c)
            End Select
        Next

        IsOnNewLine = False
    End Sub

    Public Sub WriteHtmlEncodedAttributeValue(value As String)
        For Each c In value
            Select Case c
                Case """"c
                    MyBase.Write("&quot;")
                Case Else
                    MyBase.Write(c)
            End Select
        Next

        IsOnNewLine = False
    End Sub

    Public Sub ForceNewLine()
        If Not IsOnNewLine Then
            WriteLine()
        End If
    End Sub
End Class
