Public Class HomeController
    Inherits VBController

    Public Function Index() As IActionResult
        ViewData!Message = "Welcome!"
        Return View()
    End Function

    Public Function About() As IActionResult
        ViewData!Title = "About"
        ViewData!Message = "Your application description page (in VB)."

        Return View(Of About)()
    End Function

    Public Function Contact() As IActionResult
        ViewData!Title = "Contact"
        ViewData!Message = "We're experiencing high call volume. Try again later."

        Return View()
    End Function

End Class