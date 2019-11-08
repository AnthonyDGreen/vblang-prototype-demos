Public Class Database

    Public Shared Function GetCarouselData() As CarouselInfo()
        Return {
                New CarouselInfo With {
                      .BackgroundImageUrl = "https://www.w3schools.com/bootstrap4/ny.jpg",
                      .BackgroundImageAltText = "New York",
                      .Description = "The Big Apple. Home of super heroes like Spider-man and the Avengers!",
                      .DestinationUrl = "https://en.wikipedia.org/wiki/New_York_City"
                    },
                New CarouselInfo With {
                      .BackgroundImageUrl = "https://www.w3schools.com/bootstrap4/chicago.jpg",
                      .BackgroundImageAltText = "Chicago",
                      .Description = "The Windy City. Home of Great Pizza and Hot Dogs!",
                      .DestinationUrl = "https://en.wikipedia.org/wiki/Chicago"
                    },
                New CarouselInfo With {
                      .BackgroundImageUrl = "https://www.w3schools.com/bootstrap4/la.jpg",
                      .BackgroundImageAltText = "Los Angeles",
                      .Description = "The City of Angels. Home of Hollywood!",
                      .DestinationUrl = "https://en.wikipedia.org/wiki/Los_Angeles"
                    }
               }

    End Function

End Class

Public Class CarouselInfo
    Public Property BackgroundImageUrl As String
    Public Property BackgroundImageAltText As String
    Public Property Description As String
    Public Property DestinationUrl As String
End Class
