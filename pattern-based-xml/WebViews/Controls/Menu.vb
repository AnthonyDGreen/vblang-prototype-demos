Public Class Menu
    Inherits ControlWebElement

    Public Property AlwaysOnTop As Boolean = True

    Private ReadOnly Children As New List(Of MenuItem)

    Public Sub AddChildContent(item As MenuItem)
        Children.Add(item)
    End Sub

    Protected Overrides Function GetChildren() As IEnumerable(Of WebElement)
        Return Children
    End Function

    Public Overrides Sub WriteToHtml(writer As HtmlTextWriter)
        ApplyBindings()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("<nav class=""menu")

        If AlwaysOnTop Then
            writer.Write(" floating")
        End If

        writer.Write(""">")

        writer.WriteLine()

        If Children.Count > 0 Then

            writer.Indent()

            writer.ForceNewLine()
            writer.WriteIndent()
            writer.Write("<ul class=""menu-content"">")
            writer.WriteLine()

            writer.Indent()

            For Each item In Children
                WriteToHtml(item, writer)
            Next

            writer.Unindent()

            writer.ForceNewLine()
            writer.WriteIndent()
            writer.Write("</ul>")
            writer.WriteLine()

            writer.Unindent()

        End If

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("</nav>")
        writer.WriteLine()

    End Sub

    Protected Overrides Sub RegisterToWriteHeaderContent(perTypeHeaderContentWriters As Dictionary(Of Type, WebElement),
                                                         perInstanceHeaderContentWriters As List(Of WebElement))

        perTypeHeaderContentWriters(GetType(Menu)) = Me

        MyBase.RegisterToWriteHeaderContent(perTypeHeaderContentWriters, perInstanceHeaderContentWriters)
    End Sub

    Protected Overrides Sub WritePerTypeHeaderContent(writer As HtmlTextWriter)

        Dim headerContent = <![CDATA[<style>
body {
  font-family: Arial, Helvetica, sans-serif;
}

.menu {
  top: 0;
  left: 0;
  width: 100%;
}

.menu.floating {
  position: fixed;
}

.menu:not(.floating)
{
  position: absolute;
}

.menu ul.menu-content, .menu ul.menu-item-content {
  list-style: none;
}

.menu * {
  margin: 0;
  padding: 0;
}

.menu {
  color: white;
  background-color: #333;
  font-size: 16px;
}

.menu a {
  font-size: inherit;
  color: inherit;
  text-decoration: none;
}

.menu a:hover {
  color: white;
  text-decoration: none;
}

.menu-content {
  color: inherit;
  background-color: inherit;
  font-size: inherit;
}

.menu-item {
  color: inherit;
  background-color: inherit;
  float: left;
  position: relative;
  font-size: inherit;
}

.menu-item-content {
  color: inherit;
  background-color: inherit;
  font-size: inherit;
  box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
  display: none;
}

.menu-item-header {
  font-size: inherit;
  text-align: left;
  display: block;
  width: 160px;
  padding: 14px 16px;
}

.menu > .menu-content > .menu-item > .menu-item-content {
  position: absolute;
  left: 0;
  top: 100%;
}
  
.menu-item-content > .menu-item > .menu-item-content {
  position: absolute;
  left: 100%;
  top: 0;
}

.menu-item-header:hover {
  background-color: red;
}
  
.menu:focus-within .menu-item:hover > .menu-item-content {
  display: block;
}
        
.menu > .menu-content > .menu-item > .menu-item-header:not(:only-child):after {
  content: "⯆";
  float: right;
}
        
.menu-item-content > .menu-item > .menu-item-header:not(:only-child):after {
  content: "⯈";
  position: absolute;
  top: 14px;
  left: 85%;
}

.menu + * {
  margin-top: 60px
}

</style>
]]>.Value

        writer.Write(headerContent)

    End Sub

End Class

Public Class MenuItem
    Inherits ControlWebElement

    Public Property Header As Object

    Public Property Url As String

    Private ReadOnly Children As New List(Of MenuItem)

    Public Sub AddChildContent(item As MenuItem)
        Children.Add(item)
    End Sub

    Protected Overrides Iterator Function GetChildren() As IEnumerable(Of WebElement)

        Dim asWebElement = TryCast(Header, WebElement)

        If asWebElement IsNot Nothing Then
            Yield asWebElement
        End If

        For Each child In Children
            Yield child
        Next
    End Function

    Public Overrides Sub WriteToHtml(writer As HtmlTextWriter)
        ApplyBindings()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("<li class=""menu-item"">")
        writer.WriteLine()

        writer.Indent()

        writer.WriteIndent()
        writer.Write("<a class=""menu-item-header""")
        writer.Write(" href=""")
        If Url IsNot Nothing Then
            writer.WriteHtmlEncodedAttributeValue(Url)
        Else
            writer.Write("javascript:void(0);")
        End If
        writer.Write("""")

        writer.Write(">")

        Dim headerContent = TryCast(Header, WebElement)

        If headerContent Is Nothing Then
            If Header Is Nothing Then
                headerContent = New Text("")
            Else
                Dim asString = TryCast(Header, String)

                If asString IsNot Nothing Then
                    headerContent = New Text(asString)
                Else
                    headerContent = New Text(Header.ToString())
                End If
            End If
        End If

        WriteToHtml(headerContent, writer)

        writer.WriteIndentIfOnNewLine()
        writer.Write("</a>")

        writer.WriteLine()

        If Children.Count > 0 Then

            writer.ForceNewLine()
            writer.WriteIndent()
            writer.Write("<ul class=""menu-item-content"">")
            writer.WriteLine()

            writer.Indent()

            For Each item In Children
                WriteToHtml(item, writer)
            Next

            writer.Unindent()

            writer.ForceNewLine()
            writer.WriteIndent()
            writer.Write("</ul>")
            writer.WriteLine()

        End If

        writer.Unindent()

        writer.WriteIndent()
        writer.Write("</li>")
        writer.WriteLine()
    End Sub

End Class