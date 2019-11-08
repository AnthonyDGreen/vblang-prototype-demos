
<Route("api/[controller]")>
Public MustInherit Class VBJsonApiController
    Inherits Controller

    <HttpGet>
    Public Function [Get]() As String
        Return OnGet()?.ToString()
    End Function

    <CompilerServices.DefaultOverrideMethod>
    Protected Overridable Function OnGet() As JContainer
        Return Nothing
    End Function

End Class

Namespace Global.Microsoft.VisualBasic.CompilerServices
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)>
    Public Class DefaultOverrideMethodAttribute
        Inherits Attribute
    End Class
End Namespace