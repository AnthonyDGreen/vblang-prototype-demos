'```
' # Chapter 1: Input & Output
'
' ## The `WriteLine` method
'
' When writing a program it's not uncommon to want to say stuff to your users. This is known as **output**.
' You can use the `WriteLine` method of the `Console` type to display a line of text.
'```
Console.WriteLine("Enter your name: ")
Stop

'```
' ## The `ReadLine` method
'
' You can read **input** from the user with the `ReadLine` method and store the user's response in a _variable_.
'```
Dim response = Console.ReadLine()

'```
' You can then display the value you read using a feature called _String Interpolation_. In an interpolation
' whatever code you write between the `{` and `}` characters is substituted in.
'```
Console.WriteLine($"Hello, {response}.")

'```
' You should see a screen like this now:
'
' ![Screenshot](https://ibin.co/4eqdIaVPGPJo.png)
'
'
'
'
'
'
'```
Stop