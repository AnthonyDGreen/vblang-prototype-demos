Imports Xamarin.Forms
Imports <xmlns="clr-namespace:Xamarin.Forms">

Public Class App
    Inherits Application

    Public Class PersonModel
        Property Name As String = "John Doe"
    End Class

    Private Model As New PersonModel

    Public Sub New()

        'Dim label = New Label With {.HorizontalTextAlignment = TextAlignment.Center,
        '                            .FontSize = Device.GetNamedSize(NamedSize.Medium, GetType(Label)),
        '                            .Text = "Welcome to Xamarin.Forms with Visual Basic.NET"}

        'Dim stack = New StackLayout With {
        '    .VerticalOptions = LayoutOptions.Center
        '}
        'stack.Children.Add(label)

        'Dim page = New ContentPage
        'page.Content = stack

        'MainPage = page

        Dim page = <ContentPage Title="My First Android App!" BindingContext=<%= Model %>>
                       <StackLayout VerticalOptions="Center">
                           <Label
                               HorizontalTextAlignment="Center"
                               FontSize="Medium"
                               Text="Welcome to Xamarin.Forms with Visual Basic .NET!"
                           />
                           <Entry Text="{Binding Name}"/>
                           <Button Text="Next" Clicked="NextButton_Click"/>
                       </StackLayout>
                   </ContentPage>

        MainPage = New NavigationPage(page)

    End Sub

    Async Sub NextButton_Click(sender As Button, e As EventArgs)
        Await sender.Navigation.PushAsync(New Page2 With {.Title = Model.Name})
    End Sub

End Class