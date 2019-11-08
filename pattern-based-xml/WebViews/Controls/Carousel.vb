Public Class Carousel
    Inherits ControlWebElement

    Public Property Id As String = "myCarousel"

    Public Property Interval As Integer = TimeSpan.FromSeconds(6).TotalMilliseconds

    Public Property ItemTemplate As WebElement

    Public Property ItemsSource As IEnumerable

    Protected Overrides Sub RegisterToWriteHeaderContent(perTypeHeaderContentWriters As Dictionary(Of Type, WebElement), perInstanceHeaderContentWriters As List(Of WebElement))
        perTypeHeaderContentWriters(GetType(Carousel)) = Me

        MyBase.RegisterToWriteHeaderContent(perTypeHeaderContentWriters, perInstanceHeaderContentWriters)
    End Sub

    Protected Overrides Sub WritePerTypeHeaderContent(writer As HtmlTextWriter)

        writer.ForceNewLine()

        Dim content = <![CDATA[    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <style>
        /* Make the image fully responsive 
        .carousel-inner img {
          width: 100%;
          height: 100%;
        }*/
    </style>]]>.Value

        writer.Write(content)

        'writer.WriteIndent()
        'writer.Write("<script src=""https: //ajax.aspnetcdn.com/ajax/jquery/jquery-2.2.0.min.js"" integrity=""sha384-K+ctZQ+LL8q6tP7I94W+qzQsfRV2a+AfHIi9k8z8l9ggpc8X+Ytst4yBo/hH+8Fk"">")
        'writer.WriteLine()
        'writer.WriteIndent()
        'writer.WriteLine("</script>")
        'writer.WriteLine()

        'writer.WriteIndent()
        'writer.Write("<script src=""https://ajax.aspnetcdn.com/ajax/bootstrap/3.3.7/bootstrap.min.js"" integrity=""sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa"">")
        'writer.WriteLine()
        'writer.WriteIndent()
        'writer.WriteLine("</script>")
        writer.WriteLine()

    End Sub

    Public Overrides Sub WriteToHtml(writer As HtmlTextWriter)
        'ApplyBindings()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write($"<div id=""{Id}"" class=""carousel slide"" data-ride=""carousel"" data-interval=""{Interval}"">")
        writer.WriteLine()

        writer.Indent()

        Dim items As List(Of Object)
        If ItemsSource Is Nothing Then
            items = New List(Of Object)
        Else
            items = ItemsSource.Cast(Of Object).ToList()
        End If

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("<ol class=""carousel-indicators"">")
        writer.WriteLine()

        writer.Indent()

        If items.Count > 0 Then
            writer.ForceNewLine()
            writer.WriteIndent()
            writer.Write($"<li data-target=""#{Id}"" data-slide-to=""0"" class=""active""></li>")

            For i = 1 To items.Count - 1
                writer.ForceNewLine()
                writer.WriteIndent()
                writer.Write($"<li data-target=""#{Id}"" data-slide-to=""")
                writer.Write(i)
                writer.Write("""></li>")
            Next
        End If

        writer.Unindent()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("</ol>")
        writer.WriteLine()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("<div class=""carousel-inner"" role=""listbox"">")
        writer.WriteLine()

        writer.Indent()

        If items.Count > 0 Then

            Dim itemTemplate = Me.ItemTemplate

            writer.ForceNewLine()
            writer.WriteIndent()
            writer.Write("<div class=""carousel-item active"">")

            writer.Indent()

            If itemTemplate IsNot Nothing Then
                itemTemplate.DataContext = items(0)
                WriteToHtml(itemTemplate, writer)
            Else
                WriteToHtml(New Text(items(0).ToString()), writer)
            End If

            writer.Unindent()

            writer.ForceNewLine()
            writer.WriteIndent()
            writer.Write("</div>")
            writer.WriteLine()

            For i = 1 To items.Count - 1
                writer.ForceNewLine()
                writer.WriteIndent()
                writer.Write("<div class=""carousel-item"">")

                writer.Indent()

                If itemTemplate IsNot Nothing Then
                    itemTemplate.DataContext = items(i)
                    WriteToHtml(itemTemplate, writer)
                Else
                    WriteToHtml(New Text(items(i).ToString()), writer)
                End If

                writer.Unindent()

                writer.ForceNewLine()
                writer.WriteIndent()
                writer.Write("</div>")
                writer.WriteLine()

            Next
        End If

        writer.Unindent()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("</div>")
        writer.WriteLine()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write($"<a class=""carousel-control-prev"" href=""#{Id}"" role=""button"" data-slide=""prev"">")
        writer.WriteLine()

        writer.Indent()

        writer.ForceNewLine()
        writer.WriteIndent()
        ' ⟨
        writer.Write("<span class=""carousel-control-prev-icon""></span>")
        writer.WriteLine()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("<span class=""sr-only"">Previous</span>")
        writer.WriteLine()

        writer.Unindent()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("</a>")
        writer.WriteLine()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write($"<a class=""carousel-control-next"" href=""#{Id}"" role=""button"" data-slide=""next"">")
        writer.WriteLine()

        writer.Indent()

        writer.ForceNewLine()
        writer.WriteIndent()
        ' ⟩
        writer.Write("<span class=""carousel-control-next-icon""></span>")
        writer.WriteLine()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("<span class=""sr-only"">Next</span>")
        writer.WriteLine()

        writer.Unindent()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("</a>")
        writer.WriteLine()

        writer.Unindent()

        writer.ForceNewLine()
        writer.WriteIndent()
        writer.Write("</div>")
        writer.WriteLine()

    End Sub

End Class

#If False Then

#End If