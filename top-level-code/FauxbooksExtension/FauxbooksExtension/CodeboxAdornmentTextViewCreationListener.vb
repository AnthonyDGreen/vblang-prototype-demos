Imports System.ComponentModel.Composition
Imports Microsoft.VisualStudio.Text.Editor
Imports Microsoft.VisualStudio.Utilities

''' <summary>
''' Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
''' that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
''' </summary>
<Export(GetType(IWpfTextViewCreationListener))>
<ContentType("text")>
<TextViewRole(PredefinedTextViewRoles.Document)>
Friend NotInheritable Class CodeboxAdornmentTextViewCreationListener
    Implements IWpfTextViewCreationListener

    ''' <summary>
    ''' Defines the adornment layer for the scarlet adornment. This layer is ordered
    ''' after the selection layer in the Z-order
    ''' </summary>
    <Export(GetType(AdornmentLayerDefinition))>
    <Name("CodeboxAdornment")>
    <Order(Before:=PredefinedAdornmentLayers.Selection)>
    Private CodeboxAdornmentLayer As AdornmentLayerDefinition

    ''' <summary>
    ''' Defines the adornment layer for the scarlet adornment. This layer is ordered
    ''' after the selection layer in the Z-order
    ''' </summary>
    <Export(GetType(AdornmentLayerDefinition))>
    <Name("RenderedMarkdownAdornment")>
    <Order(After:=PredefinedAdornmentLayers.Text)>
    Private RenderedMarkdownAdornmentLayer As AdornmentLayerDefinition

#Region "IWpfTextViewCreationListener"

    ''' <summary>
    ''' Called when a text view having matching roles is created over a text data model having a matching content type.
    ''' Creates a CodeboxAdornment manager when a textview is created
    ''' </summary>
    ''' <param name="textView">The <see cref="IWpfTextView"/> upon which the adornment should be placed</param>
    Public Sub TextViewCreated(ByVal textView As IWpfTextView) Implements IWpfTextViewCreationListener.TextViewCreated

        ' The adornment will listen to any event that changes the layout (text changes, scrolling, etc)
        Dim tempCodeboxAdornment = New CodeboxAdornment(textView)

    End Sub

#End Region

End Class
