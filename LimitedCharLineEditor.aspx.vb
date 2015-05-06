Imports System.Text

Partial Class LimitedCharLineEditor
    Inherits System.Web.UI.Page


#Region "Variables"
    Private disallowedChars(-1) As String
    Private preOpInsertPoint As Integer
#End Region ' Variables


#Region "Initialization"

    Private Sub frmMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        SetInitialTextboxValues()
    End Sub

#End Region 'Initialization


#Region "Controls"

    'Private Sub rtbText_TextChanged(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles rtbText.KeyPress
    '    If disallowedChars.Contains(e.KeyChar) Then
    '        ' Do not allow chars from the disallowed chars box
    '        e.Handled = True
    '        Exit Sub
    '    End If
    '    preOpInsertPoint = rtbText.selectionstart
    '    If e.KeyChar <> ChrW(Keys.Back) Then
    '        ' We probably want the user to be able to backspace without interference (otherwise, backspacing at the start of a line simply adds a new line again).
    '        If (rtbText.SelectionStart + 1) - rtbText.GetFirstCharIndexOfCurrentLine > Val(nudCharsPerLine.Text) Then
    '            Dim insertPoint As Integer = rtbText.SelectionStart
    '            Dim newEndPoint As Integer = InStrRev(rtbText.Text, " ")
    '            If InStrRev(rtbText.Text, "-") > newEndPoint Then newEndPoint = InStrRev(rtbText.Text, "-")
    '            If newEndPoint = 0 Or newEndPoint < rtbText.GetFirstCharIndexOfCurrentLine Then
    '                newEndPoint = rtbText.GetFirstCharIndexOfCurrentLine + Val(nudCharsPerLine.Text)
    '            End If
    '            Dim fullBuilder As New StringBuilder(rtbText.Text)
    '            fullBuilder.Insert(newEndPoint, Chr(11)) ' Insert soft return
    '            rtbText.Text = fullBuilder.ToString
    '            rtbText.SelectionStart = insertPoint + 1
    '            'Dim currentLine As String = Mid(rtbText.Text, rtbText.GetFirstCharIndexOfCurrentLine, newEndPoint)
    '        End If
    '    End If
    'End Sub

    'Private Sub rtbText_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles rtbText.KeyUp
    '    If rtbText.SelectionStart < rtbText.Text.Length Then
    '        Dim insertPoint As Integer = rtbText.SelectionStart
    '        ' Select the rest of the text
    '        rtbText.SelectionLength = rtbText.Text.Length - insertPoint
    '        ' Then cut it out and paste it in again to run the formatter
    '        rtbText.Cut()
    '        rtbText.SelectionStart = insertPoint
    '        rtbText.Paste()
    '    End If
    'End Sub

    Private Sub btnFormat_Click(sender As System.Object, e As System.EventArgs) Handles btnFormatNoBreaks.Click
        ManualFormat2(False)
    End Sub

    Private Sub btnFormatWBreaks_Click(sender As System.Object, e As System.EventArgs) Handles btnFormatWBreaks.Click
        ManualFormat2(True)
    End Sub

    Private Sub txtDisallowChars_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDisallowChars.TextChanged
        ReDim disallowedChars(txtDisallowChars.Text.Length - 1)
        For i = 0 To txtDisallowChars.Text.Length - 1
            disallowedChars(i) = Mid(txtDisallowChars.Text, i + 1, 1)
        Next i
    End Sub

    Protected Sub nudCharsPerLine_TextChanged(sender As Object, e As System.EventArgs) Handles nudCharsPerLine.TextChanged
        If Not IsNumeric(nudCharsPerLine.Text) Then
            'MsgBox doesn't work for ASP .Net (it pops up on the server)
            'MsgBox("Please enter a value between 1 and 150")
            nudCharsPerLine.Text = "80"
        ElseIf Val(nudCharsPerLine.Text) < 1 Then
            nudCharsPerLine.Text = "1"
        ElseIf Val(nudCharsPerLine.Text) > 150 Then
            nudCharsPerLine.Text = "150"
        Else
            nudCharsPerLine.Text = Math.Round(Val(nudCharsPerLine.Text), 0)
        End If
    End Sub

    Protected Sub nudMaxLength_TextChanged(sender As Object, e As System.EventArgs) Handles nudMaxLength.TextChanged
        If Not IsNumeric(nudMaxLength.Text) Then
            'MsgBox doesn't work for ASP .Net (it pops up on the server)
            'MsgBox("Please enter a value between 1 and 2147483647")
            nudMaxLength.Text = "2000"
        ElseIf Val(nudMaxLength.Text) < 1 Then
            nudMaxLength.Text = "1"
        ElseIf Val(nudMaxLength.Text) > 2147483647 Then
            nudMaxLength.Text = "2147483647"
        Else
            nudMaxLength.Text = Math.Round(Val(nudMaxLength.Text), 0)
            ' MaxLength cannot be used in ASP.NET for multi-line textboxes
            'rtbText.MaxLength = Val(nudMaxLength.Text)
        End If
    End Sub

#End Region ' Controls


#Region "Functions"

    Private Sub SetInitialTextboxValues()
        ' Apparently these don't fire on form load in ASP .NET, so we have to call them explicitly
        txtDisallowChars_TextChanged(txtDisallowChars, Nothing)
        nudCharsPerLine_TextChanged(nudCharsPerLine, Nothing)
        nudMaxLength_TextChanged(nudMaxLength, Nothing)
    End Sub

    'Private Sub AddChar(ByVal newChar As Char, ByRef currentString As StringBuilder)
    '    If disallowedChars.Contains(newChar) Then
    '        ' Do not allow chars from the disallowed chars box
    '        Exit Sub
    '    End If
    '    If (rtbText.SelectionStart + 1) - rtbText.GetFirstCharIndexOfCurrentLine > Val(nudCharsPerLine.Text) Then
    '        Dim insertPoint As Integer = rtbText.SelectionStart
    '        Dim newEndPoint As Integer = InStrRev(rtbText.Text, " ")
    '        If InStrRev(rtbText.Text, "-") > newEndPoint Then newEndPoint = InStrRev(rtbText.Text, "-")
    '        If newEndPoint = 0 Or newEndPoint < rtbText.GetFirstCharIndexOfCurrentLine Then
    '            newEndPoint = rtbText.GetFirstCharIndexOfCurrentLine + Val(nudCharsPerLine.Text)
    '        End If
    '        Dim fullBuilder As New StringBuilder(rtbText.Text)
    '        fullBuilder.Insert(newEndPoint, vbCrLf)
    '        rtbText.Text = fullBuilder.ToString
    '        rtbText.SelectionStart = insertPoint + 1
    '    End If
    'End Sub

    Private Sub ManualFormat(ByVal leaveGaps As Boolean)
        Dim currentText As New StringBuilder(rtbText.Text)
        Dim outputText As New StringBuilder
        Dim sampleLine As String = ""
        Dim nextChar As Char = ""
        Dim thisLineLength As Integer = 0

        If leaveGaps = False Then
            ' Remove all (soft) new lines added by this program, then refactor
            currentText.Replace(Environment.NewLine, " ")
        End If

        Do
            If currentText.ToString.Length > Val(nudCharsPerLine.Text) Then
                sampleLine = Microsoft.VisualBasic.Left(currentText.ToString, Val(nudCharsPerLine.Text))
                nextChar = Mid(currentText.ToString, Val(nudCharsPerLine.Text) + 1, 1)

                ' First, if there is a user entered new line in the alloted chars, apply it
                If InStr(sampleLine, vbCrLf) > 0 Or InStr(sampleLine, vbCr) > 0 Then
                    thisLineLength = InStr(sampleLine, vbCrLf)
                    If InStr(sampleLine, vbCr) < thisLineLength Then
                        thisLineLength = InStr(sampleLine, vbCr)
                    End If
                ElseIf InStrRev(sampleLine, " ") > 0 Or InStrRev(sampleLine, "-") > 0 Then
                    If nextChar <> "" And nextChar <> " " And nextChar <> "-" Then
                        thisLineLength = InStrRev(sampleLine, " ")
                        If InStrRev(sampleLine, "-") > thisLineLength Then
                            thisLineLength = InStrRev(sampleLine, "-")
                        End If
                    Else
                        ' Natural line break at full line length
                        thisLineLength = Val(nudCharsPerLine.Text)
                    End If
                Else
                    thisLineLength = Val(nudCharsPerLine.Text)
                End If
            Else
                thisLineLength = currentText.ToString.Length
            End If
            Dim breakNeeded As Boolean = True
            If leaveGaps = True Then
                If Mid(currentText.ToString, thisLineLength, 1) = vbCrLf Then
                    breakNeeded = False
                End If
            End If
            outputText.Append(Microsoft.VisualBasic.Left(currentText.ToString, thisLineLength) & IIf(breakNeeded, Environment.NewLine, ""))
            currentText.Remove(0, thisLineLength)
        Loop While currentText.ToString.Length > 0

        rtbText.Text = outputText.ToString

    End Sub

    Private Sub ManualFormat2(ByVal leaveGaps As Boolean)
        Dim currentText As New StringBuilder(rtbText.Text)
        Dim outputText As New StringBuilder
        Dim sampleLine As String = ""
        Dim finalLine As String = ""
        Dim nextChar As Char = ""
        Dim thisLineLength As Integer = 0
        Dim lineBreaks() As String = {vbCrLf, vbCr, vbLf, Chr(11), Chr(3), Environment.NewLine} ' Chr(11) = soft line break, Chr(3) = end of text

        'currentText.Replace(Chr(11), " ")
        For i = 1 To currentText.ToString.Length - 1
            If i <= currentText.ToString.Length - 1 Then
                ' We can break our for loop by removing items at the end, need to check inside
                If leaveGaps = False Then
                    ' Remove all new lines, then refactor
                    If lineBreaks.Contains(currentText(i)) Then
                        If currentText(i - 1) = "-" Then
                            currentText.Remove(i, 1)
                        Else
                            currentText(i) = " "
                        End If
                    End If
                Else
                    ' Remove all (soft) new lines added by this program, then refactor (this doesn't work as hoped, the Chr(11) gets turned into vbLf when reading back in, so we have to remove both, carefully)
                    If i < currentText.ToString.Length - 1 AndAlso (currentText(i) = vbLf And (currentText(i + 1) = vbLf Or (i > 0 And currentText(i - 1) = vbLf))) Then
                        ' Do nothing (if there are two line feeds in a row)
                    ElseIf i < currentText.ToString.Length - 3 AndAlso (currentText(i) = vbLf And (currentText(i + 1) = " " And currentText(i + 2) = " " And currentText(i + 3) = " ")) Then
                        ' Do nothing (if the next line is clearly intended as a newline [ie is indented])
                    ElseIf currentText(i) = Environment.NewLine Or currentText(i) = vbLf Then
                        If currentText(i - 1) = "-" Or currentText(i - 1) = " " Then
                            currentText.Remove(i, 1)
                        Else
                            currentText(i) = " "
                        End If
                    End If
                End If
            End If
        Next i

        Do
            If currentText.ToString.Length > Val(nudCharsPerLine.Text) Then
                sampleLine = Microsoft.VisualBasic.Left(currentText.ToString, Val(nudCharsPerLine.Text))
                nextChar = currentText(Val(nudCharsPerLine.Text))

                ' First, if there is a user entered new line in the alloted chars, apply it
                If InStr(sampleLine, vbCrLf) > 0 Or InStr(sampleLine, vbCr) > 0 Or InStr(sampleLine, vbLf) > 0 Or InStr(sampleLine, Chr(11)) > 0 Or InStr(sampleLine, Environment.NewLine) > 0 Then
                    thisLineLength = Val(nudCharsPerLine.Text)
                    If InStr(sampleLine, vbCrLf) > 0 Then
                        thisLineLength = InStr(sampleLine, vbCrLf)
                    End If
                    If InStr(sampleLine, vbCr) > 0 And InStr(sampleLine, vbCr) < thisLineLength Then
                        thisLineLength = InStr(sampleLine, vbCr)
                    End If
                    If InStr(sampleLine, vbLf) > 0 And InStr(sampleLine, vbLf) < thisLineLength Then
                        thisLineLength = InStr(sampleLine, vbLf)
                    End If
                    If InStr(sampleLine, Chr(11)) > 0 And InStr(sampleLine, Chr(11)) < thisLineLength Then
                        thisLineLength = InStr(sampleLine, Chr(11))
                    End If
                    If InStr(sampleLine, Environment.NewLine) > 0 And InStr(sampleLine, Environment.NewLine) < thisLineLength Then
                        thisLineLength = InStr(sampleLine, Environment.NewLine)
                    End If
                    'ElseIf lineBreaks.Contains(nextChar) Then
                    '    thisLineLength = nudCharsPerLine.Value + 1
                ElseIf InStrRev(sampleLine, " ") > 0 Or InStrRev(sampleLine, "-") > 0 Then
                    If nextChar <> "" And nextChar <> " " And nextChar <> "-" Then
                        thisLineLength = InStrRev(sampleLine, " ") ' If 0, will be overwritten after
                        If InStrRev(sampleLine, "-") > thisLineLength Then
                            thisLineLength = InStrRev(sampleLine, "-")
                        End If
                    Else
                        ' Natural line break at full line length
                        thisLineLength = Val(nudCharsPerLine.Text)
                    End If
                Else
                    thisLineLength = Val(nudCharsPerLine.Text)
                End If
            Else
                thisLineLength = currentText.ToString.Length
            End If

            Dim breakNeeded As Boolean = True
            If leaveGaps = True Then
                If lineBreaks.Contains(currentText(thisLineLength - 1)) Then
                    breakNeeded = False
                End If
            End If
            ' If there is a single space on the end of the line, get rid of it
            Dim removeSpace As Integer = 0
            If currentText(thisLineLength - 1) = " " Then
                If thisLineLength > 1 Then
                    'If currentText(thisLineLength - 2) <> " " Then
                    removeSpace = 1
                    'Else
                    '    removeSpace = removeSpace ' Error catch - why does ending a line with a space cause more crlf to be added??
                    'End If
                End If
            End If

            finalLine = currentText.ToString(0, thisLineLength - removeSpace) 'Microsoft.VisualBasic.Left(currentText.ToString, thisLineLength - removeSpace)
            For i = 0 To disallowedChars.Length - 1
                If InStr(finalLine, disallowedChars(i)) > 0 Then
                    finalLine = finalLine.Replace(disallowedChars(i), " ")
                End If
            Next i

            outputText.Append(finalLine & IIf(breakNeeded, Environment.NewLine, ""))
            currentText.Remove(0, thisLineLength)
            ' If there is a single or double space starting the next line, get rid of it
            If currentText.ToString.Length >= 2 Then
                If currentText(0) = " " Then
                    If currentText(1) <> " " Then
                        currentText.Remove(0, 1)
                    ElseIf currentText.ToString.Length >= 2 AndAlso (currentText(2) = " " And currentText(3) <> " ") Then
                        currentText.Remove(0, 2)
                    End If
                End If
            End If
        Loop While currentText.ToString.Length > 0

        Dim finalText As String = outputText.ToString
        If finalText.Length > Val(nudMaxLength.Text) Then
            finalText = Microsoft.VisualBasic.Left(finalText, Val(nudMaxLength.Text))
        End If

        rtbText.Text = finalText

    End Sub

#End Region ' Functions

End Class
