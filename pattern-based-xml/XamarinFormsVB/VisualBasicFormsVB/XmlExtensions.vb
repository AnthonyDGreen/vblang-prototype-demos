Imports System.Runtime.CompilerServices
Imports Xamarin.Forms

Module XmlExtensions

#Region " Properties with binding and/or type converters "

    <Extension>
    Sub SetFontSize(instance As Label,
                    value As Double,
                    valueText As String,
                    otherValue As Object)

        If otherValue IsNot Nothing Then
            Throw New NotImplementedException()
        End If

        ' TODO: Consider passing a flag indicating whether the value is 0.0 because the developer
        ' wrote "0.0" in such cases to simplify tests for bridge method authors.
        ' Don't use Boolean? because like WPF CheckBox.IsChecked are naturally that type.
        If value <> 0.0 OrElse valueText = "0" OrElse valueText = "0.0" Then
            instance.FontSize = value
            Return
        End If

        instance.FontSize = DirectCast(New FontSizeConverter().ConvertFromInvariantString(valueText), Double)
        Return

        'Dim namedSize As NamedSize
        'Select Case valueText
        '    Case "micro"
        '        namedSize = NamedSize.Micro
        '    Case "small"
        '        namedSize = NamedSize.Small
        '    Case "medium"
        '        namedSize = NamedSize.Medium
        '    Case "large"
        '        namedSize = NamedSize.Large
        '    Case "default"
        '        namedSize = NamedSize.Default
        '    Case Else
        '        Throw New FormatException(valueText)
        'End Select

        'instance.FontSize = Device.GetNamedSize(namedSize, GetType(Label))

    End Sub

    <Extension>
    Sub SetText(instance As Entry,
                value As String,
                valueText As String,
                otherValue As Object)

        If otherValue IsNot Nothing Then
            Throw New NotSupportedException()
        End If

        If valueText?.StartsWith("{") Then
            If valueText.StartsWith("{Binding ") AndAlso valueText.EndsWith("}") Then
                Dim propertyName = valueText.Substring(9, valueText.Length - 9 - 1)

                instance.SetBinding(Entry.TextProperty, New Binding(propertyName))
            Else
                Throw New FormatException(valueText)
            End If
        Else
            instance.Text = value
        End If
    End Sub

    <Extension>
    Sub SetKeyboard(instance As Entry,
                value As Keyboard,
                valueText As String,
                otherValue As Object)

        If otherValue IsNot Nothing Then
            Throw New NotSupportedException()
        End If

        If value IsNot Nothing Then
            instance.Keyboard = value
            Return
        End If

        If valueText?.StartsWith("{") Then
            If valueText.StartsWith("{Binding ") AndAlso valueText.EndsWith("}") Then
                Dim propertyName = valueText.Substring(9, valueText.Length - 9 - 1)

                instance.SetBinding(Entry.KeyboardProperty, New Binding(propertyName))
            Else
                Throw New FormatException(valueText)
            End If
        Else
            instance.Keyboard = New KeyboardTypeConverter().ConvertFromInvariantString(valueText)
        End If
    End Sub

    <Extension>
    Sub SetVerticalOptions(instance As View,
                           value As LayoutOptions,
                           valueText As String,
                           otherValue As Object)

        SetPropertyIfNotDefaultWithSupportForTypeConvertersAndBinding(Of LayoutOptions, LayoutOptionsConverter)(
            instance.VerticalOptions,
            value,
            valueText,
            otherValue,
            Function(left, right) left.Alignment = right.Alignment AndAlso left.Expands = right.Expands,
            Nothing,
            instance,
            View.VerticalOptionsProperty)

    End Sub

    Private Sub SetPropertyIfNotDefaultWithSupportForTypeConvertersAndBinding(Of TValue As Structure, TConverter As {New, TypeConverter})(
                    ByRef backingStore As TValue,
                    value As TValue,
                    valueText As String,
                    otherValue As Object,
                    equals As Func(Of TValue, TValue, Boolean),
                    defaultValueText As String,
                    instance As BindableObject,
                    bindableProperty As BindableProperty
                )

        If Not equals(value, Nothing) OrElse valueText = defaultValueText Then
            backingStore = value
            Return
        ElseIf otherValue IsNot Nothing Then
            If TypeOf otherValue Is TValue Then
                backingStore = otherValue
                Return
            End If

            Dim asBinding = TryCast(otherValue, Binding)
            If asBinding IsNot Nothing Then
                instance.SetBinding(bindableProperty, asBinding)
                Return
            End If

            Throw New InvalidOperationException()
        ElseIf valueText.StartsWith("{") Then
            If valueText.StartsWith("{Binding ") AndAlso valueText.EndsWith("}") Then
                Dim propertyName = valueText.Substring(9, valueText.Length - 9 - 1)

                instance.SetBinding(bindableProperty, New Binding(propertyName))
            Else
                Throw New FormatException(valueText)
            End If
        Else
            backingStore = New TConverter().ConvertFromInvariantString(valueText)
        End If
    End Sub

    Private Sub SetPropertyIfNotDefaultWithSupportForTypeConvertersAndBinding(Of TValue As Class, TConverter As {New, TypeConverter})(
                    ByRef backingStore As TValue,
                    value As TValue,
                    valueText As String,
                    otherValue As Object,
                    instance As BindableObject,
                    bindableProperty As BindableProperty
                )

#If DEBUG Then
        If GetType(TValue) Is GetType(String) Then Throw New InvalidOperationException("Wrong method for strings.")
#End If

        If value IsNot Nothing Then
            backingStore = value
            Return

        ElseIf otherValue Is Nothing AndAlso valueText Is Nothing Then
            backingStore = Nothing
            Return

        ElseIf otherValue IsNot Nothing Then

            Dim asTValue = TryCast(otherValue, TValue)
            If asTValue IsNot Nothing Then
                backingStore = otherValue
                Return
            End If

            Dim asBinding = TryCast(otherValue, Binding)
            If asBinding IsNot Nothing Then
                instance.SetBinding(bindableProperty, asBinding)
                Return
            End If

            Throw New InvalidOperationException()

        ElseIf valueText.StartsWith("{") Then

            If valueText.StartsWith("{Binding ") AndAlso valueText.EndsWith("}") Then
                Dim propertyName = valueText.Substring(9, valueText.Length - 9 - 1)

                instance.SetBinding(bindableProperty, New Binding(propertyName))
            Else
                Throw New FormatException(valueText)
            End If

        Else
            backingStore = New TConverter().ConvertFromInvariantString(valueText)
        End If
    End Sub

    Private Sub SetPropertyIfNotDefaultWithSupportForTypeConvertersAndBinding(
                    ByRef backingStore As String,
                    value As String,
                    valueText As String,
                    otherValue As Object,
                    instance As BindableObject,
                    bindableProperty As BindableProperty
                )

        If value IsNot Nothing AndAlso value <> valueText Then
            backingStore = value
            Return

        ElseIf otherValue Is Nothing AndAlso valueText Is Nothing Then
            backingStore = Nothing
            Return

        ElseIf otherValue IsNot Nothing Then

            Dim asString = TryCast(otherValue, String)
            If asString IsNot Nothing Then
                backingStore = otherValue
                Return
            End If

            Dim asBinding = TryCast(otherValue, Binding)
            If asBinding IsNot Nothing Then
                instance.SetBinding(bindableProperty, asBinding)
                Return
            End If

            Throw New InvalidOperationException()

        ElseIf valueText.StartsWith("{") Then

            If valueText.StartsWith("{Binding ") AndAlso valueText.EndsWith("}") Then
                Dim propertyName = valueText.Substring(9, valueText.Length - 9 - 1)

                instance.SetBinding(bindableProperty, New Binding(propertyName))
            Else
                Throw New FormatException(valueText)
            End If

        Else
            backingStore = valueText
        End If
    End Sub

    Private Sub SetPropertyIfNotDefaultWithSupportForBinding(Of TValue As Structure)(
                    ByRef backingStore As TValue,
                    value As TValue,
                    valueText As String,
                    otherValue As Object,
                    equals As Func(Of TValue, TValue, Boolean),
                    defaultValueText As String,
                    instance As BindableObject,
                    bindableProperty As BindableProperty
                )

        If Not equals(value, Nothing) OrElse (valueText Is Nothing OrElse valueText = defaultValueText) Then
            backingStore = value
            Return
        ElseIf otherValue IsNot Nothing Then
            If TypeOf otherValue Is TValue Then
                backingStore = otherValue
                Return
            End If

            Dim asBinding = TryCast(otherValue, Binding)
            If asBinding IsNot Nothing Then
                instance.SetBinding(bindableProperty, asBinding)
                Return
            End If

            Throw New InvalidOperationException()
        ElseIf valueText.StartsWith("{") Then
            If valueText.StartsWith("{Binding ") AndAlso valueText.EndsWith("}") Then
                Dim propertyName = valueText.Substring(9, valueText.Length - 9 - 1)

                instance.SetBinding(bindableProperty, New Binding(propertyName))
            Else
                Throw New FormatException(valueText)
            End If
        Else
            Throw New InvalidOperationException()
        End If
    End Sub

#End Region

#Region " Content properties "

    <Extension>
    Sub SetChildContent(instance As ContentPage, content As View)
        instance.Content = content
    End Sub

    <Extension>
    Sub SetChildContent(instance As Label, content As String)
        instance.Text = content
    End Sub

    <Extension>
    Sub SetChildContent(instance As Span, content As String)
        instance.Text = content
    End Sub

    <Extension>
    Sub AddChildContent(instance As Layout(Of View), content As View)
        instance.Children.Add(content)
    End Sub

    <Extension>
    Sub AddFormattedTextChildContent(instance As Label, content As FormattedString)
        instance.FormattedText = content
    End Sub

    <Extension>
    Sub AddFormattedTextChildContent(instance As Label, content As Span)
        Dim formattedString = instance.FormattedText
        If formattedString Is Nothing Then
            formattedString = New FormattedString
            instance.FormattedText = formattedString
        End If

        formattedString.AddChildContent(content)
    End Sub

    <Extension>
    Sub AddFormattedTextChildContent(instance As Label, content As String)
        Dim formattedString = instance.FormattedText
        If formattedString Is Nothing Then
            formattedString = New FormattedString
            instance.FormattedText = formattedString
        End If

        formattedString.AddChildContent(content)
    End Sub

    <Extension>
    Sub AddChildContent(instance As FormattedString, content As Span)
        instance.Spans.Add(content)
    End Sub

    <Extension>
    Sub AddChildContent(instance As FormattedString, content As String)
        instance.Spans.Add(New Span With {.Text = content})
    End Sub

#End Region

#Region " Experimental "

    <Extension>
    Sub SetName(Of T As BindableObject)(instance As T, ByRef value As T, valueText As String, otherValue As Object)
        value = instance
    End Sub

    <Extension>
    Sub SetSelf(ByRef instance As ContentPage,
                value As ContentPage,
                valueText As String,
                otherValue As Object)
        instance = value
    End Sub

#End Region

End Module