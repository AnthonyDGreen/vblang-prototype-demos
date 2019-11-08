Imports WebViews

Public Class Timer
    Inherits ControlWebElement

    Property Interval As Integer

    Property Tick As Expressions.Expression(Of Action)

    Protected Overrides Sub RegisterToWriteHeaderContent(perTypeHeaderContentWriters As Dictionary(Of Type, WebElement),
                                                         perInstanceHeaderContentWriters As List(Of WebElement))

        perInstanceHeaderContentWriters.Add(Me)

        MyBase.RegisterToWriteHeaderContent(perTypeHeaderContentWriters, perInstanceHeaderContentWriters)
    End Sub

    Protected Overrides Sub WritePerInstanceHeaderContent(writer As HtmlTextWriter)

        If Tick Is Nothing Then Return

        ' writer.WriteLine("setInterval(function(){ alert(""Hello""); }, 3000);")

        writer.ForceNewLine()

        writer.WriteIndent()
        writer.WriteLine("<script>")

        writer.WriteIndent()
        writer.Write("setInterval(function(){ ")
        writer.Write(ExpressionTreeConverter.ConvertToJavaScript(Tick))
        writer.Write("; }, ")
        writer.Write(Interval)
        writer.WriteLine(");")

        writer.WriteIndent()
        writer.WriteLine("</script>")

    End Sub

    Public Overrides Sub WriteToHtml(writer As HtmlTextWriter)
        Exit Sub
    End Sub

End Class
