Imports System.Net.Http
Imports Newtonsoft.Json.Linq

Module RestDemo
    Async Function RunAsync() As Task

        Dim client As New HttpClient With {.BaseAddress = New Uri("https://api.github.com")}
#Region " Initialize HttpClient headers "
        With client.DefaultRequestHeaders
            .UserAgent.Add("JsonDemo", "0.1")
            .Accept.Add("application/json")
            .Authorization.Set("Token", My.Settings.AccessToken)
        End With
#End Region

        Dim gist =
            {
              "description": My.Application.Info.AssemblyName,
              "public": true,
              "files": {
                "RestDemo.vb": {
                  "content": IO.File.ReadAllText("..\..\RestDemo.vb")
                }
              }
            }

        Dim response = Await client.PostJsonAsync("/gists", gist)

        Console.WriteLine(response)

    End Function
End Module