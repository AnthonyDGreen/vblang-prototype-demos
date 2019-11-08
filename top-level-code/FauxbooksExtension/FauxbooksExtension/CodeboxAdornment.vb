Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports Microsoft.VisualStudio.Text
Imports Microsoft.VisualStudio.Text.Editor
Imports Microsoft.VisualStudio.Text.Formatting
Imports System.Math
Imports System.Text

''' <summary>
''' CodeboxAdornment places red boxes behind all the "a"s in the editor window
''' </summary>
Friend NotInheritable Class CodeboxAdornment

    ''' <summary>
    ''' Text view where the adornment is created.
    ''' </summary>
    Private WithEvents View As IWpfTextView

    ''' <summary>
    ''' The layer of the adornment.
    ''' </summary>
    Private ReadOnly CodeboxLayer As IAdornmentLayer

    ''' <summary>
    ''' The layer of the adornment.
    ''' </summary>
    Private ReadOnly MarkdownLayer As IAdornmentLayer

    ''' <summary>
    ''' Adornment brush.
    ''' </summary>
    Private ReadOnly Brush As Brush

    ''' <summary>
    ''' Adornment pen.
    ''' </summary>
    Private ReadOnly Pen As Pen

    ''' <summary>
    ''' Initializes a new instance of the <see cref="CodeboxAdornment"/> class.
    ''' </summary>
    ''' <param name="textView">Text view to create the adornment for</param>
    Public Sub New(textView As IWpfTextView)

        Me.View = textView
        Me.CodeboxLayer = View.GetAdornmentLayer("CodeboxAdornment")
        Me.MarkdownLayer = View.GetAdornmentLayer("RenderedMarkdownAdornment")

        ' Create the pen and brush to color the box behind the a's
        Me.Brush = Brushes.LightGray

        Me.Pen = New Pen(Brushes.Red, 0)
        Me.Pen.Freeze()

    End Sub

    Private Enum LineClassification
        Code
        Comment
        Delimiter
        Whitespace
    End Enum

    ''' <summary>
    ''' Handles whenever the text displayed in the view changes by adding the adornment to any reformatted lines
    ''' </summary>
    ''' <remarks><para>This event Is raised whenever the rendered text displayed in the <see cref="ITextView"/> changes.</para>
    ''' <para>It Is raised whenever the view does a layout (which happens when DisplayTextLineContainingBufferPosition Is called Or in response to text Or classification changes).</para>
    ''' <para>It is also raised whenever the view scrolls horizontally Or when its size changes.</para>
    ''' </remarks>
    ''' <param name="sender">The event sender.</param>
    ''' <param name="e">The event arguments.</param>
    Private Sub OnLayoutChanged(sender As Object, e As TextViewLayoutChangedEventArgs) Handles View.LayoutChanged
        If e.NewOrReformattedLines.Count = 0 AndAlso Not e.HorizontalTranslation Then Return
        If View.TextViewLines Is Nothing Then Return

        Static markdownVisualCache As New System.Collections.Generic.Dictionary(Of String, WebBrowser)

        CodeboxLayer.RemoveAllAdornments()
        MarkdownLayer.RemoveAllAdornments()

        Dim getLineClassification = Function(line As ITextViewLine) As LineClassification
                                        For i = line.Start.Position To line.End.Position - 1
                                            If Char.IsWhiteSpace(line.Snapshot(i)) Then Continue For

                                            If line.Snapshot.GetText(i, 4) = "'```" Then
                                                Return LineClassification.Delimiter

                                            ElseIf line.Snapshot(i) = "'"c Then
                                                Return LineClassification.Comment

                                            Else
                                                Return LineClassification.Code
                                            End If
                                        Next

                                        Return LineClassification.Whitespace
                                    End Function

        Dim createCodeboxVisual = Sub(startLine As ITextViewLine, endLine As ITextViewLine)
                                      Const MARGIN = 5

                                      Dim bounds = New Rect(New Point(startLine.Left, startLine.Top - MARGIN),
                                                      New Point(Max(View.ViewportRight - 5, endLine.Right), endLine.Bottom + MARGIN))
                                      Dim box = New RectangleGeometry(bounds, MARGIN, MARGIN)
                                      Dim drawing = New GeometryDrawing(Me.Brush, Me.Pen, box)
                                      drawing.Freeze()

                                      Dim drawingImage = New DrawingImage(drawing)
                                      drawingImage.Freeze()

                                      Dim image = New Image()
                                      image.Source = drawingImage
                                      Canvas.SetLeft(image, bounds.Left)
                                      Canvas.SetTop(image, bounds.Top)

                                      CodeboxLayer.AddAdornment(AdornmentPositioningBehavior.TextRelative, New SnapshotSpan(startLine.Start, endLine.End), Nothing, image, removedCallback:=Nothing)
                                  End Sub

        Dim createMarkdownVisual = Sub(first As Integer, last As Integer)
                                       If last <= first Then Return

                                       Dim contentBuilder = New StringBuilder()

                                       Dim firstLine = View.TextViewLines(first)

                                       For i = first + 1 To last - 1
                                           Dim line = View.TextViewLines(i)
                                           Dim lineText = line.Snapshot.GetText(line.Start.Position, line.Length)
                                           contentBuilder.AppendLine(lineText.Trim().Substring(1).Trim())
                                       Next

                                       Dim lastLine = View.TextViewLines(last)

                                       Dim content = contentBuilder.ToString()

                                       Dim browser As WebBrowser = Nothing

                                       If Not markdownVisualCache.TryGetValue(content, browser) Then
                                           browser = New WebBrowser()
                                           browser.HorizontalAlignment = HorizontalAlignment.Stretch
                                           browser.Height = lastLine.Bottom - firstLine.Top
                                           browser.NavigateToString(<html>
                                                                        <head>
                                                                            <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
                                                                            <meta charset="utf-8"/>
                                                                            <title></title>
                                                                            <script src="https://cdn.jsdelivr.net/npm/marked/marked.min.js"></script>
                                                                        </head>
                                                                        <body>
                                                                            <div id="content">
                                                                                <%= content %>
                                                                            </div>
                                                                            <script>
                                                                        document.getElementById('content').innerHTML =
                                                                            marked(document.getElementById('content').innerHTML);
                                                                        </script>
                                                                        </body>
                                                                    </html>.ToString())
                                           markdownVisualCache.Add(content, browser)
                                       End If

                                       Canvas.SetLeft(browser, firstLine.Left)
                                       Canvas.SetTop(browser, firstLine.Top)
                                       browser.Width = View.ViewportWidth  'lastLine.Right - firstLine.Left

                                       MarkdownLayer.AddAdornment(AdornmentPositioningBehavior.TextRelative, New SnapshotSpan(firstLine.Start, lastLine.End), Nothing, browser, removedCallback:=Nothing)
                                   End Sub

        For l = 0 To View.TextViewLines.Count - 1
            Dim line = View.TextViewLines(l)

            Select Case getLineClassification(line)
                Case LineClassification.Code,
                     LineClassification.Comment

                    Dim lastCodeLine = l

                    For peek = l + 1 To View.TextViewLines.Count - 1

                        Dim peekedLine = View.TextViewLines(peek)

                        Select Case getLineClassification(peekedLine)
                            Case LineClassification.Code,
                                 LineClassification.Comment

                                lastCodeLine = peek

                            Case LineClassification.Delimiter
                                Exit For

                            Case LineClassification.Whitespace
                                Continue For
                        End Select
                    Next

                    createCodeboxVisual(line, View.TextViewLines(lastCodeLine))

                    l = lastCodeLine
                    Continue For

                Case LineClassification.Delimiter

                    Dim lastRichLine = l

                    For peek = l + 1 To View.TextViewLines.Count - 1

                        Dim peekedLine = View.TextViewLines(peek)

                        Select Case getLineClassification(peekedLine)
                            Case LineClassification.Code
                                Exit For
                            Case LineClassification.Comment
                                Continue For
                            Case LineClassification.Delimiter
                                lastRichLine = peek
                                Exit For
                            Case LineClassification.Whitespace
                                Exit For
                        End Select
                    Next

                    createMarkdownVisual(l, lastRichLine)

                    l = lastRichLine
                    Continue For

                Case LineClassification.Whitespace

                    Continue For

            End Select
        Next
    End Sub

End Class