Public Class Expander
    Inherits ControlWebElement

    Public Property Header As Object

    Protected ReadOnly Property Children As New List(Of WebElement)

    Public Sub AddChildContent(content As WebElement)
        Children.Add(content)
    End Sub

    Public Sub AddChildContent(content As String)
        Children.Add(New Text(content))
    End Sub

    Protected Overrides Function GetChildren() As IEnumerable(Of WebElement)
        Return Children
    End Function

    Public Overrides Sub WriteToHtml(writer As HtmlTextWriter)
        ApplyBindings()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("<div class=""expander"">")
        writer.WriteLine()

        writer.Indent()

        writer.WriteIndent()
        writer.Write("<button class=""expander-header"" id=""TEMPLATE_THE_BUTTON"" onclick=""Expander_Clicked(this, event);"">")

        writer.Indent()

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

        writer.Unindent()

        writer.WriteIndentIfOnNewLine()
        writer.Write("</button>")
        writer.WriteLine()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("<div class=""expander-content"" id=""TEMPLATE_THE_CONTENT"">")
        writer.WriteLine()

        writer.Indent()

        For Each child In GetChildren()
            WriteToHtml(child, writer)
        Next

        writer.Unindent()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("</div>")
        writer.WriteLine()

        writer.Unindent()

        writer.WriteIndent()
        writer.Write("</div>")
        writer.WriteLine()

    End Sub

    Protected Overrides Sub RegisterToWriteHeaderContent(perTypeHeaderContentWriters As Dictionary(Of Type, WebElement),
                                                         perInstanceHeaderContentWriters As List(Of WebElement))

        perTypeHeaderContentWriters(GetType(Expander)) = Me

        MyBase.RegisterToWriteHeaderContent(perTypeHeaderContentWriters, perInstanceHeaderContentWriters)
    End Sub

    Protected Overrides Sub WritePerTypeHeaderContent(writer As HtmlTextWriter)

        Dim headerContent = <![CDATA[<style>
.expander-header {
  background-color: #eee;
  color: #444;
  cursor: pointer;
  padding: 18px;
  width: 100%;
  border: none;
  text-align: left;
  outline: none;
  font-size: 15px;
  transition: 0.4s;
}

.expander-header:hover, .expander-header.active {
  background-color: #ccc;
}

.expander-header:after {
  content: '\002B';
  color: #777;
  font-weight: bold;
  float: right;
  margin-left: 5px;
}

.expander-header.active:after {
  content: "\2212";
}

.expander-content {
  padding: 0 18px;
  background-color: white;
  max-height: 0;
  overflow: hidden;
  transition: max-height 0.2s ease-out;
}
</style>
<script type="text/javascript">
function Expander_Clicked(sender, e)
{
    var header = sender;

    // Find "The Content"
    var content = null;
    if (header.nextElementSibling != null && header.nextElementSibling.id == "TEMPLATE_THE_CONTENT")
    {
        content = header.nextElementSibling
    }
    else
    {
        var parent = header.parentElement;
        
        // Ideally this would search descendents, I think.
        for (i = 0; i < parent.children.length; i++)
        {
            if (parent.children[i].id == "TEMPLATE_THE_CONTENT")
            {
                content = parent.children[i];
                break;
            }
        }

        if (content == null) return;
    }

    header.classList.toggle("active");
    if (content.style.maxHeight)
    {
        content.style.maxHeight = null;
    }
    else
    {
        content.style.maxHeight = content.scrollHeight + "px";
    }
}
</script>        
        ]]>.Value

        writer.Write(headerContent)

    End Sub

End Class