Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Module JObjectExtensions

    <Runtime.CompilerServices.Extension>
    Sub AddEx(obj As JObject, content As Object)
        Dim asJObject = TryCast(content, JObject)

        If asJObject IsNot Nothing Then
            For Each pair In asJObject
                obj.Add(pair.Key, pair.Value)
            Next

            Return
        End If

        Dim asJObjects = TryCast(content, IEnumerable(Of JObject))

        If asJObjects IsNot Nothing Then
            For Each child In asJObjects
                For Each pair In child
                    obj.Add(pair.Key, pair.Value)
                Next
            Next

            Return
        End If

        obj.Add(content)

    End Sub

    <Runtime.CompilerServices.Extension>
    Sub AddEx(obj As JObject, propertyName As String, value As JToken)

        obj.Add(propertyName, value)

    End Sub

End Module