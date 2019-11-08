Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports Newtonsoft.Json.Linq

Module HttpClientExtensions

    <Extension>
    Sub Add(headers As HttpHeaderValueCollection(Of ProductInfoHeaderValue), productName As String, productVersion As String)
        headers.Add(New ProductInfoHeaderValue(productName, productVersion))
    End Sub

    <Extension>
    Sub Add(headers As HttpHeaderValueCollection(Of MediaTypeWithQualityHeaderValue), mediaType As String)
        headers.Add(New MediaTypeWithQualityHeaderValue(mediaType))
    End Sub

    <Extension>
    Sub [Set](ByRef header As AuthenticationHeaderValue, scheme As String, Optional parameter As String = Nothing)
        header = New AuthenticationHeaderValue(scheme, parameter)
    End Sub

    ''' <summary>
    ''' Send a GET request to the specified URI and parses the response as JSON.
    ''' </summary>
    <Extension>
    Async Function GetJsonAsync(client As HttpClient, requestUri As String, Optional cancellationToken As CancellationToken = Nothing) As Task(Of JContainer)
        Dim response = Await client.GetAsync(requestUri, cancellationToken)

        If Not response.IsSuccessStatusCode Then Return Nothing

        Return JToken.Parse(Await response.Content.ReadAsStringAsync())
    End Function

    ''' <summary>
    ''' Send a POST request to the specified URI and parses the response as JSON.
    ''' </summary>
    <Extension>
    Async Function PostJsonAsync(
                       client As HttpClient,
                       requestUri As String,
                       content As JContainer,
                       Optional cancellationToken As CancellationToken = Nothing
                   ) As Task(Of JContainer)

        Dim response = Await client.PostAsync(requestUri, New StringContent(content.ToString()), cancellationToken)

        If Not response.IsSuccessStatusCode Then Return Nothing

        Return JToken.Parse(Await response.Content.ReadAsStringAsync())
    End Function

End Module
