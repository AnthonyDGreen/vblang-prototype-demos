Public Class GameLoop
    Inherits Form

    Private Timer As New Stopwatch()
    'Private OtherTimer As New Threading.Timer(Sub() Invalidate(ClientRectangle), Nothing, 1000, 15)

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim elapsed = Timer.ElapsedMilliseconds

        If elapsed >= 16 Then
            DrawFrame(e.Graphics, elapsed / 1000)
            Timer.Restart()
        ElseIf Not Timer.IsRunning Then
            DrawFrame(e.Graphics, elapsed / 1000)
            Timer.Start()
        End If

        Invalidate(ClientRectangle)
    End Sub

    Private Const HorizontalMargin = 20
    Private Const VerticalMargin = 20
    Public ReadOnly Property LeftEdge As Integer = HorizontalMargin
    Public ReadOnly Property TopEdge As Integer = VerticalMargin
    Public ReadOnly Property RightEdge As Integer = ClientSize.Width - HorizontalMargin
    Public ReadOnly Property BottomEdge As Integer = ClientSize.Height - VerticalMargin

    <CompilerServices.DefaultOverrideMethod>
    Public Overridable Sub DrawFrame(buffer As Graphics, elapsedSeconds As Single)

    End Sub

    Sub New()


        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.SetStyle(ControlStyles.Opaque Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, False)
        Me.UpdateStyles()

    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        Return
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)

        _RightEdge = ClientSize.Width - HorizontalMargin
        _BottomEdge = ClientSize.Height - VerticalMargin

    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'GameLoop
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(600, 400)
        Me.Name = Me.GetType().Name
        Me.Text = Name
        Me.ResumeLayout(False)

    End Sub
End Class

Namespace Global.Microsoft.VisualBasic.CompilerServices

    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False, Inherited:=True)>
    Public Class DefaultOverrideMethodAttribute
        Inherits Attribute
    End Class

End Namespace