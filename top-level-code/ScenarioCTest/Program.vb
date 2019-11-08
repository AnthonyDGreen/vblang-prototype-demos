Inherits ScenarioCLibrary.GameLoop

buffer.Clear(Color.Black)

For i = 1 To 10
    buffer.FillRectangle(Brushes.Yellow, Rnd() * 600, Rnd() * 400, 5, 5)
Next

' Draw ball.
BallLeft += XAcceleration * elapsedSeconds
BallTop += YAcceleration * elapsedSeconds

' Detect collisions.
If BallRight >= RightEdge Then
    XAcceleration *= -1

    BallRight = RightEdge - (BallRight - RightEdge)

ElseIf BallLeft <= LeftEdge Then
    XAcceleration *= -1

    BallLeft = LeftEdge + (LeftEdge - BallLeft)

End If

If BallBottom >= BottomEdge Then
    YAcceleration *= -1

    BallBottom = BottomEdge - (BallBottom - BottomEdge)

ElseIf BallTop <= TopEdge Then
    YAcceleration *= -1

    BallTop = TopEdge + (TopEdge - BallTop)

End If

buffer.FillRectangle(Brushes.Blue, BallLeft, BallTop, BallWidth, BallHeight)

Private BallLeft As Integer = 500
Private Property BallRight As Integer
    Get
        Return BallLeft + BallWidth
    End Get
    Set(value As Integer)
        BallLeft = value - BallWidth
    End Set
End Property
Private BallTop As Integer = 300
Private Property BallBottom As Integer
    Get
        Return BallTop + BallHeight
    End Get
    Set(value As Integer)
        BallTop = value - BallHeight
    End Set
End Property
Private BallWidth As Integer = 15
Private BallHeight As Integer = 15
Private XAcceleration As Integer = 400
Private YAcceleration As Integer = 200