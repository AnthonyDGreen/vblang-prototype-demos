Module Program
    Sub Main()
        ' Imagine all that other code happened already.
        Dim reader As IDataReader = New MockDataReader()

        Dim output =
            [
              From rs In reader.ResultSets(),
                   row In rs.Records()
              Let status = row.GetString(0)
              Take While Not String.IsNullOrEmpty(status)
              Select {
                "Status": status,
                "Title": row.GetString(1),
                "Host": row.GetString(6),
                "Guest": row.GetString(7),
                "Episode": CInt(row.GetDouble(2)),
                "Live": row.GetDateTime(5),
                "Url": row.GetString(11),
                "embedUrl": $"{row.GetString(11)}player"
              }
            ]
        Console.WriteLine(output)
    End Sub

    <Runtime.CompilerServices.Extension>
    Iterator Function ResultSets(reader As IDataReader) As IEnumerable(Of IDataReader)
        Yield reader

        Do While reader.NextResult()
            Yield reader
        Loop
    End Function

    <Runtime.CompilerServices.Extension>
    Iterator Function Records(reader As IDataReader) As IEnumerable(Of IDataReader)
        Do While reader.Read()
            Yield reader
        Loop
    End Function

End Module

Class MockDataReader
    Inherits Common.DbDataReader

    Public Overrides Function GetDataTypeName(ordinal As Integer) As String
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetEnumerator() As IEnumerator
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetFieldType(ordinal As Integer) As Type
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetName(ordinal As Integer) As String
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetOrdinal(name As String) As Integer
        Return -1
    End Function

    Public Overrides Function GetBoolean(ordinal As Integer) As Boolean
        Return Me(ordinal)
    End Function

    Public Overrides Function GetByte(ordinal As Integer) As Byte
        Return Me(ordinal)
    End Function

    Public Overrides Function GetBytes(ordinal As Integer, dataOffset As Long, buffer() As Byte, bufferOffset As Integer, length As Integer) As Long
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetChar(ordinal As Integer) As Char
        Return Me(ordinal)
    End Function

    Public Overrides Function GetChars(ordinal As Integer, dataOffset As Long, buffer() As Char, bufferOffset As Integer, length As Integer) As Long
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetDateTime(ordinal As Integer) As Date
        Return Me(ordinal)
    End Function

    Public Overrides Function GetDecimal(ordinal As Integer) As Decimal
        Return Me(ordinal)
    End Function

    Public Overrides Function GetDouble(ordinal As Integer) As Double
        Return Me(ordinal)
    End Function

    Public Overrides Function GetFloat(ordinal As Integer) As Single
        Return Me(ordinal)
    End Function

    Public Overrides Function GetGuid(ordinal As Integer) As Guid
        Return Me(ordinal)
    End Function

    Public Overrides Function GetInt16(ordinal As Integer) As Short
        Return Me(ordinal)
    End Function

    Public Overrides Function GetInt32(ordinal As Integer) As Integer
        Return Me(ordinal)
    End Function

    Public Overrides Function GetInt64(ordinal As Integer) As Long
        Return Me(ordinal)
    End Function

    Public Overrides Function GetString(ordinal As Integer) As String
        Return Me(ordinal)
    End Function

    Public Overrides Function GetValue(ordinal As Integer) As Object
        Return Me(ordinal)
    End Function

    Public Overrides Function GetValues(values() As Object) As Integer
        Throw New NotImplementedException()
    End Function

    Public Overrides Function IsDBNull(ordinal As Integer) As Boolean
        Throw New NotImplementedException()
    End Function

    Public Overrides Function NextResult() As Boolean
        Static setCount = 0

        setCount += 1

        Return setCount <= 2
    End Function

    Public Overrides Function Read() As Boolean
        Static rowCount = 0

        rowCount += 1

        Return rowCount <= 3 OrElse (rowCount > 4 AndAlso rowCount <= 6)
    End Function

    Public Overrides ReadOnly Property Depth As Integer
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Overrides ReadOnly Property FieldCount As Integer
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Overrides ReadOnly Property HasRows As Boolean
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Overrides ReadOnly Property IsClosed As Boolean
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Overrides ReadOnly Property RecordsAffected As Integer
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Default Public Overrides ReadOnly Property Item(ordinal As Integer) As Object
        Get
            Static values As Object() =
                    {
                        "Live",
                        "Introduction to Azure Integration Service Environment for Logic Apps",
                        528.0,
                        Nothing, Nothing,
                        CDate("2019-02-26T00:00:00"),
                        "Scott Hanselman",
                        "Kevin Lam",
                        Nothing, Nothing, Nothing,
                        "https://azure.microsoft.com/en-us/resources/videos/azure-friday-introduction-to-azure-integration-service-environment-for-logic-apps"
                    }

            Return values(ordinal)
        End Get
    End Property

    Default Public Overrides ReadOnly Property Item(name As String) As Object
        Get
            Throw New NotImplementedException()
        End Get
    End Property
End Class