'```
' # Chapter 2: `For` loops
'
' There are 5 basic parts to a `For` loop;
'
' * The counter **variable** (`i`)
' * The initial **value** (`1`)
' * The final **value** (`10`)
' * The increment, which is _optional_ and is specified in the `Step` clause
' * The code you would like to repeat:
' ```
' Console.WriteLine(i)
' ```
'```

For i = 1 To 10
    Console.WriteLine(i)
Next

'```
' ### Iterating backward
'
' You can loop backward by swapping the _initial_ and _final_ values using a **negative** `Step` value.
'```

Stop
For i = 10 To 1 Step -1
    Console.WriteLine(i)
Next

'```
' `For` loops are useful in combination with **arrays**. We'll look at arrays in the next chapter.
'```