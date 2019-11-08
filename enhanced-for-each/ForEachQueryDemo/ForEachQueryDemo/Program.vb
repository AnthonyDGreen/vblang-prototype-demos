Imports System.Console

Module Program

    Sub Main()

        Dim letters As Char() = "Anthony Diante Green"

        For Each l In letters
            Where Not Char.IsWhiteSpace(l)
            Group By l Into count = Count()
            Order By count Descending

            WriteLine($"The letter '{l}' appears {count} times(s) in my name.")
        Next

        WriteLine()

    End Sub

End Module