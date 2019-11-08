Imports Microsoft.AspNetCore.Mvc.ViewFeatures

Namespace Global.Microsoft.VisualBasic.CompilerServices
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)>
    Public Class DefaultOverrideMethodAttribute
        Inherits Attribute
    End Class
End Namespace

Namespace Global.Microsoft.AspNetCore.Mvc
    Public Class VBController
        Inherits Microsoft.AspNetCore.Mvc.Controller

        Public Overloads Function View(Of TView As {VBView, New})() As IActionResult
            Return View(New TView)
        End Function

        Private Overloads Function View(definition As VBView) As IActionResult
            definition.SetViewData(Me.ViewData)
            Return View(definition.Render())
        End Function

        Public Overloads Function View(body As XNode) As IActionResult
            Return New RenderXmlLiteralActionResult(body)
        End Function

        Public Overrides Function View(viewName As String, model As Object) As ViewResult
            Static views As New Lazy(Of Dictionary(Of String, Type))(Function() Aggregate t In Me.GetType().Assembly.GetTypes()
                                                                                Where GetType(VBView).IsAssignableFrom(t)
                                                                                Into ToDictionary(t.Name.ToLower()))

            Return View(CType(Activator.CreateInstance(views.Value(RouteData.Values!action.ToString().ToLower())), VBView))
        End Function

        Private Class RenderXmlLiteralActionResult
            Inherits ViewResult

            Private Body As XNode

            Public Sub New(body As XNode)
                Me.Body = body
            End Sub

            Public Overrides Async Function ExecuteResultAsync(context As ActionContext) As Task
                context.HttpContext.Response.ContentType = "text/html"
                Using writer = New IO.StreamWriter(context.HttpContext.Response.Body)
                    Await writer.WriteAsync(Body.ToString())
                End Using
            End Function

        End Class

    End Class

    Public Class VBView

        Friend Overridable Sub SetViewData(data As ViewFeatures.ViewDataDictionary)

        End Sub

        Friend Overridable Function GetViewData() As ViewDataDictionary
            Return Nothing
        End Function

        <CompilerServices.DefaultOverrideMethod>
        Public Overridable Function Render() As XNode
            Return <?xml version="1.0" encoding="UTF-8"?><html/>
        End Function

        Protected Function RenderSection(Of TView As {VBView, New})() As XNode
            Dim newView = New TView()
            newView.SetViewData(Me.GetViewData())
            Dim section = newView.Render()

            Return If(TryCast(section, XElement), TryCast(section, XDocument)?.Root)
        End Function

    End Class

    Public Class VBView(Of TModel)
        Inherits VBView

        Public Property ViewData As Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(Of TModel)

        Friend Overrides Sub SetViewData(data As ViewDataDictionary)
            ViewData = New ViewDataDictionary(Of TModel)(data)
        End Sub

        Friend Overrides Function GetViewData() As ViewDataDictionary
            Return ViewData
        End Function

    End Class
End Namespace