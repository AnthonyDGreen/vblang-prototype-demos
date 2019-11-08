Imports ExcelDataReader

Inherits VBJsonApiController

Dim getRows = Iterator Function()
                  Using inFile = IO.File.Open("test-data.xlsx", IO.FileMode.Open, IO.FileAccess.Read),
                        reader = ExcelDataReader.ExcelReaderFactory.CreateReader(inFile)
                      Do
                          reader.Read() ' Skip header row.
                          Do While reader.Read()
                              If String.IsNullOrEmpty(reader.GetString(0)) Then Exit Do

                              Yield reader
                          Loop
                      Loop While reader.NextResult()
                  End Using
              End Function

[
  From row In getRows()
  Select {
    "Status": row.GetString(0),
    "Title": row.GetString(1),
    "Host": row.GetString(6),
    "Guest": row.GetString(7),
    "Episode": CInt(row.GetDouble(2)),
    "Live": row.GetDateTime(5),
    "Url": row.GetString(11),
    "embedUrl": $"{row.GetString(11)}player"
  }
]