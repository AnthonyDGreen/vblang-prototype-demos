Imports WebViews

Module Program

    Private TimeLabel As Label

    Sub Main()

        Dim expr As Expressions.Expression(Of Action)

        expr = Sub()
                   TimeLabel.InnerText = (New Dictionary(Of String, Object) from {{"key", "value"}}).ToString()
               End Sub

        expr = Sub() MsgBox("Hello")

        Dim rewritten = New ExpressionTreeCleanupRewriter().Visit(expr)

        rewritten = New DotNetObjectsToJavaScriptObjectsRewriter().Visit(rewritten)

        Dim js = JavaScriptWritingVisitor.Convert(rewritten)

        Console.WriteLine(js)

        Dim inInstanceExpr = New TestView().GetCallAddMethodExpression()

        Dim instanceRewritten = New ExpressionTreeCleanupRewriter().Visit(inInstanceExpr)

        instanceRewritten = New DotNetObjectsToJavaScriptObjectsRewriter().Visit(instanceRewritten)

        Dim instanceJs = JavaScriptWritingVisitor.Convert(instanceRewritten)

        Console.WriteLine(instanceJs)

    End Sub

    Sub TestUI()
        Dim dataFromDatabase =
            New List(Of CarouselInfo) From {
                New CarouselInfo With {
                      .BackgroundImageUrl = "/bootstrap4/ny.jpg",
                      .BackgroundImageAltText = "ASP.NET",
                      .Description = "Learn how to build ASP.NET apps that can run anywhere.",
                      .DestinationUrl = "https://go.microsoft.com/fwlink/?LinkID=525028&clcid=0x409"
                    },
                New CarouselInfo With {
                      .BackgroundImageUrl = "/bootstrap4/chicago.jpg",
                      .BackgroundImageAltText = "Visual Studio",
                      .Description = "There are powerful new features in Visual Studio for building modern web apps.",
                      .DestinationUrl = "https://go.microsoft.com/fwlink/?LinkID=525030&clcid=0x409"
                    },
                New CarouselInfo With {
                      .BackgroundImageUrl = "/bootstrap4/la.jpg",
                      .BackgroundImageAltText = "Package Management",
                      .Description = "Bring in libraries from NuGet and npm, and automate tasks using Grunt or Gulp.",
                      .DestinationUrl = "https://go.microsoft.com/fwlink/?LinkID=525029&clcid=0x409"
                    },
                New CarouselInfo With {
                      .BackgroundImageUrl = "/bootstrap4/ny.jpg",
                      .BackgroundImageAltText = "Microsoft Azure",
                      .Description = "Learn how Microsoft's Azure cloud platform allows you to build, deploy, and scale web apps.",
                      .DestinationUrl = "https://go.microsoft.com/fwlink/?LinkID=525027&clcid=0x409"
                    },
                New CarouselInfo With {
                      .BackgroundImageUrl = "/bootstrap4/chicago.jpg",
                      .BackgroundImageAltText = "Visual Basic .NET",
                      .Description = "There are powerful new features that can be added to VB .NET for building modern web apps.",
                      .DestinationUrl = "https://anthonydgreen.net/"
                    }
            }

        Dim doc = New Html

        Dim head = New Head

        Dim title = New Title("Testing")

        head.AddChildContent(title)

        doc.AddChildContent(head)

        Dim body = New Body

        Dim menu = New Menu

        Dim menu1 = New MenuItem With {.Header = "I"}

        Dim menu1_1 = New MenuItem With {.Header = "I.A"}

        Dim menu1_1_1 = New MenuItem With {.Header = "I.A.1", .Url = "/IA1"}
        menu1_1.AddChildContent(menu1_1_1)

        Dim menu1_1_2 = New MenuItem With {.Header = "I.A.2", .Url = "/IA2"}
        menu1_1.AddChildContent(menu1_1_2)

        menu1.AddChildContent(menu1_1)

        Dim menu1_2 = New MenuItem With {.Header = "I.B", .Url = "/IIB"}
        menu1.AddChildContent(menu1_2)

        menu.AddChildContent(menu1)

        Dim menu2 = New MenuItem With {.Header = "II"}

        Dim menu2_1 = New MenuItem With {.Header = "II.A", .Url = "/IIA"}
        menu2.AddChildContent(menu2_1)

        Dim menu2_2 = New MenuItem With {.Header = "II.B", .Url = "/IIB"}
        menu2.AddChildContent(menu2_2)

        menu.AddChildContent(menu2)

        Dim menu3 = New MenuItem With {.Header = "III", .Url = "/III"}
        menu.AddChildContent(menu3)

        body.AddChildContent(menu)

        Dim h1 = New H1("Title of document")
        body.AddChildContent(h1)

        Dim p1 = New P("Lorem ipsum.")
        body.AddChildContent(p1)

        Dim expander1 = New Expander With {.Header = "Section 1"}

        Dim expanderP = New P("Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.")
        expander1.AddChildContent(expanderP)
        expander1.AddChildContent(expanderP)

        body.AddChildContent(expander1)

        Dim expander2 = New Expander With {.Header = New H1("Section 2")}

        expander2.AddChildContent(expanderP)

        body.AddChildContent(New Br)

        body.AddChildContent(expander2)

        body.AddChildContent(New Br)

        Dim expander3 = New Expander With {.Header = "Section 3"}

        Dim carousel = New Carousel

        carousel.ItemsSource = dataFromDatabase

#If False Then
            <img src="~/images/banner1.svg" alt="ASP.NET" class="img-responsive" />
            <div class="carousel-caption" role="option">
                <p>
                    Learn how to build ASP.NET apps that can run anywhere.
                    <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkID=525028&clcid=0x409">
                        Learn More
                    </a>
                </p>
            </div>
#End If

        carousel.Id = "Carousel1"
        carousel.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds

        Dim templateOuterDiv = New Div

        Dim templateImage = New Img
        templateImage.Src = "{Binding BackgroundImageUrl}"
        templateImage.Alt = "{Binding BackgroundImageAltText}"
        templateImage.Style = "min-width: 780px; min-height: 460px;"

        templateOuterDiv.AddChildContent(templateImage)

        Dim templateInnerDiv = New Div
        templateInnerDiv.Class = "carousel-caption"
        templateInnerDiv.Attributes!role = "option"

        Dim templateP = New P

        Dim templateDescription = New Span
        templateDescription.SetContent(Nothing, "{Binding Description}", Nothing)

        templateP.AddChildContent(templateDescription)

        Dim templateLink = New A
        templateLink.Class = "btn btn-default"
        templateLink.SetHref(Nothing, "{Binding DestinationUrl}", Nothing)
        templateLink.AddChildContent(New Text("Learn More"))

        templateP.AddChildContent(templateLink)

        templateInnerDiv.AddChildContent(templateP)

        templateOuterDiv.AddChildContent(templateInnerDiv)

        carousel.ItemTemplate = templateOuterDiv

        expander3.AddChildContent(carousel)

        body.AddChildContent(expander3)

        Dim footer = New Div

        Dim h2 = New H2("Footer header")
        footer.AddChildContent(h2)

        Dim footerLink = New A("about:blank?p1=v1&p2=v2", "Click <here> for ""more""!")
        footer.AddChildContent(footerLink)

        Dim footerImage = New Img With {.Src = "{Binding PhotoUrl}"}
        footer.AddChildContent(footerImage)

        Dim footerHeader = New H1
        footerHeader.SetContent(Nothing, "{Binding Title}", Nothing)
        footer.AddChildContent(footerHeader)

        Dim footerP = New P
        footerP.SetContent(Nothing, "{Binding Description}", Nothing)
        footer.AddChildContent(footerP)

        body.AddChildContent(footer)

        doc.AddChildContent(body)

        doc.DataContext = New With {.Title = "The Walking Dead",
                                    .Description = "Rick and the group overcome adversity while trying to avoid the dead.",
                                    .PhotoUrl = "https://imgrul.com/11223344.png"}

        Dim src = doc.ToString()

        My.Computer.Clipboard.SetText(src)

        Console.WriteLine(src)

    End Sub

End Module

Class TestView

    Private TimeLabel As Label

    Function GetSetToCurrentDateExpression() As Expressions.Expression(Of Action)
        Return Sub() Me.TimeLabel.InnerText = Date.Now.ToString()
    End Function

    Private FirstNumber, SecondNumber, Result As Label

    Function GetCallAddMethodExpression() As Expressions.Expression(Of Action)
        Return Sub() Result.InnerText = Add(FirstNumber.InnerText, SecondNumber.InnerText)
    End Function

    Function Add(left As Integer, right As Integer) As Integer
        Throw New InvalidOperationException()
    End Function

End Class

Class Label
    Property InnerText As String
End Class

Friend Class CarouselInfo
    Public Property BackgroundImageUrl As String
    Public Property BackgroundImageAltText As String
    Public Property Description As String
    Public Property DestinationUrl As String
End Class
