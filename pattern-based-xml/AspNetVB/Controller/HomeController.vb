Public Class HomeController
    Inherits VBController

    Public Function Index() As IActionResult
        ViewData!Title = "ASP.NET Core View Engine Revisited"
        ViewData!Message = "Welcome!"
        Return View()
    End Function

    Public Function Square(id As Integer) As Integer
        Return id ^ 2
    End Function

    Public Function Add(left As Integer, right As Integer) As Integer
        Return left + right
    End Function

End Class