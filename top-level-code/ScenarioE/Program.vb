Imports System.IO
Imports System.Text
Imports Microsoft.AspNetCore.Hosting

Module Program
    Sub Main(args As String())
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)

        Dim host = New WebHostBuilder().
                       UseKestrel().
                       UseContentRoot(Directory.GetCurrentDirectory()).
                       UseIISIntegration().
                       UseStartup(Of Startup).
                       UseApplicationInsights().
                       Build()
        host.Run()
    End Sub
End Module