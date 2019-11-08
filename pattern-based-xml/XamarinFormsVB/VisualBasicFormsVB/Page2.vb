Imports Xamarin.Forms
Imports <xmlns="clr-namespace:Xamarin.Forms">

Public Class Page2
    Inherits ContentPage

    Private WithEvents FirstNumberEntry, SecondNumberEntry As Entry,
                       OperationPicker As Picker,
                       CalculateButton As Button,
                       ResultLabel As Label

    Private Operations As New List(Of Operation) From {
                            New Operation("+", Function(left, right) left + right),
                            New Operation("-", Function(left, right) left - right),
                            New Operation("*", Function(left, right) left * right),
                            New Operation("/", Function(left, right) left / right),
                            New Operation("^", Function(left, right) left ^ right)
                          }

    Public Sub New()

        Content = <StackLayout VerticalOptions="Center">
                      <Label HorizontalTextAlignment="Start" FontSize="Micro">
                          <Label.FormattedText>VB.NET <Bold>2nd</> Page</Label.FormattedText>
                      </Label>
                      <Entry Name="FirstNumberEntry" Placeholder="1st number" Keyboard="Numeric"/>
                      <Picker Name="OperationPicker" ItemsSource=<%= Operations %> SelectedIndex="0"/>
                      <Entry Name="SecondNumberEntry" Placeholder="2nd number"/>
                      <Button Name="CalculateButton" Text="="/>
                      <Label Name="ResultLabel"/>
                  </StackLayout>

    End Sub

    Private Sub CalculateButton_Clicked() Handles CalculateButton.Clicked
        Dim left, right As Decimal

        If Decimal.TryParse(FirstNumberEntry.Text, left) AndAlso Decimal.TryParse(SecondNumberEntry.Text, right) Then
            ResultLabel.Text = CType(OperationPicker.SelectedItem, Operation).Apply(left, right)
        Else
            ResultLabel.Text = "Error!"
        End If
    End Sub

End Class

Class Operation
    ReadOnly Property Symbol As String
    ReadOnly Implementation As Func(Of Decimal, Decimal, Decimal)

    Sub New(symbol As String, implementation As Func(Of Decimal, Decimal, Decimal))
        Me.Symbol = symbol
        Me.Implementation = implementation
    End Sub

    Function Apply(left As Decimal, right As Decimal) As Decimal
        Return Implementation(left, right)
    End Function

    Overrides Function ToString() As String
        Return Symbol
    End Function
End Class

Class Bold
    Inherits Span

    Sub New()
        FontAttributes = FontAttributes.Bold
    End Sub
End Class