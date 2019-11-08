Partial Public Class AllInOneController
    Inherits SingleVBViewController

    Public Overrides Function Index() As IActionResult
        ViewData!Title = "Revisitation - Part II"
        ViewData!Message = "Welcome back!"

        Return MyBase.Index()
    End Function

    Public Function CalculatePower(base As Integer, exponent As Integer) As Integer
        Return base ^ exponent
    End Function

End Class