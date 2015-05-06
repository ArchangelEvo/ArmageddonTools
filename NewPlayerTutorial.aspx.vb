Imports System.IO

Partial Class NewPlayerTutorial
    Inherits System.Web.UI.Page

#Region "Variables"

    Private tutorialData(Col.Video, -1) As String
    Private chapterNames(-1) As String
    'Private currentChapter As Integer = 0
    Private quoteCharacters() As Char = {ChrW(&H22), ChrW(&H27), ChrW(&H2018), ChrW(&H2019), ChrW(&H201C), ChrW(&H201D), ChrW(&HAB), ChrW(&HBB), ChrW(&H201A), ChrW(&H201B), ChrW(&H201E)} ' Various US and int'l quote characters
    Private chapterAdvancedNormally As Boolean = False
    'Private classYouTube As YouTubeScriptCS

    ' Variables for video player
    Private _W As Integer = 720
    Private _H As Integer = 454
    Private auto As Boolean = True

    Private enterKeyPressed As Boolean = False

#End Region 'Variables


#Region "Enums"

    Private Enum Col As Integer
        Title
        Input1
        Input2
        Result
        SubNote
        Links
        Video
    End Enum

#End Region 'Enums


#Region "Initialization"

    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        ' This code taken from videoplayer.codeplex.com
        If Not Page.IsPostBack Then
            Dim idx As Integer = 0
            Dim qry As String = ""

            Try
                qry = "auto"
                qry = IIf(Request.QueryString(qry) = vbNull, "", Request.QueryString(qry))
                If qry <> "" Then auto = Boolean.Parse(qry)
            Catch ex As Exception
            End Try

            Try
                qry = "item"
                qry = IIf(Request.QueryString(qry) = vbNull, "", Request.QueryString(qry))
                If qry <> "" Then idx = Integer.Parse(qry)
            Catch ex As Exception
            End Try

            classGlobalVariables.currentTutChapter = idx
        End If

        BuildTutorialFlow()
        LoadChapter(True)
        txtInputBox.Attributes.Add("autocomplete", "off")
    End Sub

#End Region 'Initialization


#Region "Controls"

    Protected Sub btnEnterKey_Click(sender As Object, e As System.EventArgs) Handles btnEnterKey.Click
        If enterKeyPressed = False Then
            enterKeyPressed = True

            Dim showQuoteMessage As Boolean = False

            'If quoteCharacters.Contains(CChar(Microsoft.VisualBasic.Left(txtInputBox.Text, 1))) Then
            If txtInputBox.Text <> "" AndAlso txtInputBox.Text.Length > 0 Then
                If quoteCharacters.Contains(txtInputBox.Text.Chars(0)) Then
                    txtInputBox.Text.TrimStart(quoteCharacters)
                    showQuoteMessage = True
                End If
                If quoteCharacters.Contains(txtInputBox.Text.Chars(txtInputBox.Text.Length - 1)) Then
                    txtInputBox.Text.TrimEnd(quoteCharacters)
                    showQuoteMessage = True
                End If
                If showQuoteMessage = True Then
                    txtReturnMsgBox.Text = "You don't need to type the quotes in the input box.  They are just there to show you what text to type."
                    'MsgBox("You don't need to type the quotes in the input box.  They are just there to show you what text to type.", MsgBoxStyle.OkOnly, "Incorrect Text")
                End If
            End If

            If txtInputBox.Text = tutorialData(Col.Input1, classGlobalVariables.currentTutChapter) Or txtInputBox.Text = tutorialData(Col.Input2, classGlobalVariables.currentTutChapter) Then
                ' This line is for debug purposes
                'MsgBox doesn't work in ASP .NET (it pops up on the server)
                'MsgBox("Correct! [" & txtInputBox.Text & "] vs [" & tutorialData(Col.Input1, classGlobalVariables.currentTutChapter) & "]", MsgBoxStyle.OkOnly, "Correct Text")
                txtInputBox.Text = ""
                classGlobalVariables.currentTutChapter += 1
                chapterAdvancedNormally = True
                txtReturnMsgBox.Text = ""
                LoadChapter(True)
            Else
                txtReturnMsgBox.Text = "Try again!  Make sure you enter the text as accurately as possible.  Things like parentheses and tildes are important for the game syntax, and capitalization and punctuation are a big help to other players! [" & txtInputBox.Text & "] vs [" & tutorialData(Col.Input1, classGlobalVariables.currentTutChapter) & "]"
                'MsgBox doesn't work in ASP .NET (it pops up on the server)
                'MsgBox("Try again!  Make sure you enter the text as accurately as possible.  Things like parentheses and tildes are important for the game syntax, and capitalization and punctuation are a big help to other players! [" & txtInputBox.Text & "] vs [" & tutorialData(Col.Input1, classGlobalVariables.currentTutChapter) & "]", MsgBoxStyle.OkOnly, "Incorrect Text")
                ' We might want to go back to just falsifying enterKeyPressed here, and not not load the same chapter again (total page reload may have been due to short test data, and not due to problems with reload after text input failure)
                classGlobalVariables.currentTutChapter = classGlobalVariables.currentTutChapter
                chapterAdvancedNormally = False
                LoadChapter(False)
            End If
        End If

    End Sub

    Protected Sub btnSkipToNext_Click(sender As Object, e As System.EventArgs) Handles btnSkipToNext.Click
        classGlobalVariables.currentTutChapter += 1
        chapterAdvancedNormally = True
        txtReturnMsgBox.Text = ""
        LoadChapter(True)
    End Sub

    Protected Sub btnSkipTo_Click(sender As Object, e As System.EventArgs) Handles btnSkipTo.Click
        classGlobalVariables.currentTutChapter = dbxSelectSkipTo.SelectedIndex
        chapterAdvancedNormally = False
        txtReturnMsgBox.Text = ""
        LoadChapter(True)
    End Sub

    'Protected Sub dbxSelectSkipTo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles dbxSelectSkipTo.SelectedIndexChanged
    '    ' we might just need to have this empty sub in here to update the selected index when it changes
    'End Sub

#End Region 'Controls


#Region "Functions"

    Private Sub SetChapter(ByVal chapterToSet As Integer)
        classGlobalVariables.currentTutChapter = chapterToSet
        classGlobalVariables.currentTutChapter = chapterToSet
    End Sub

    Private Sub BuildTutorialFlow()

        Dim categoryRow As String
        Dim categoryFields() As String
        Dim thisRow As Integer = 1 'First line of labels is discarded
        Dim inChoiceSection As Boolean = True
        Dim chapterNumber As Integer = 1

        Try
            Dim csvReader As StreamReader = New StreamReader(Server.MapPath("~/App_Data/TutorialReference.csv"))
            'categoryRows = app_CategoryConfig.Split(vbCrLf)
            ' Get first two lines, to discard line of labels
            categoryRow = csvReader.ReadLine
            categoryRow = csvReader.ReadLine

            Do While categoryRow <> Nothing And categoryRow <> ""
                categoryFields = categoryRow.Split("$")
                ReDim Preserve tutorialData(Col.Video, tutorialData.GetUpperBound(1) + 1)
                ReDim Preserve chapterNames(chapterNames.Length)
                For i = 0 To categoryFields.Length - 1
                    categoryFields(i) = categoryFields(i).TrimStart(",")
                    If i <= Col.Video Then
                        ' Any new columns will simply be ignored
                        tutorialData(i, tutorialData.GetUpperBound(1)) = categoryFields(i)
                        If i = Col.Title Then
                            chapterNames(chapterNames.Length - 1) = chapterNumber & ": " & categoryFields(i)
                            chapterNumber += 1
                        End If
                    End If
                Next i
                categoryRow = csvReader.ReadLine
            Loop

            If Not Page.IsPostBack And Not IsPostBack Then
                dbxSelectSkipTo.DataSource = chapterNames

                ' We don't need this stepped call for now, but we'll keep it, in case
                'Dim choiceBoxes() As System.Web.UI.WebControls.DropDownList = {dbxSelectSkipTo}
                'For i = 0 To choiceBoxes.Length - 1
                '    choiceBoxes(i).DataBind()
                'Next i
                dbxSelectSkipTo.DataBind()
            End If

        Catch ex As Exception
            txtReturnMsgBox.Text = "Tutorial Config file could not be read:" & vbCrLf & ex.Message
            'MsgBox doesn't work in ASP .NET (it pops up on the server)
            'MsgBox("Tutorial Config file could not be read:" & vbCrLf & ex.Message, MsgBoxStyle.Exclamation, "File Read Error")
        End Try

    End Sub

    Private Sub LoadChapter(ByVal IsNewChapter As Boolean)
        If classGlobalVariables.currentTutChapter <= tutorialData.GetUpperBound(1) Then
            'currentChapter = classGlobalVariables.currentTutChapter
            Dim expectedEntryMessage As String = ""
            Dim entryText As String = ""
            Dim linkInfo() As String = tutorialData(Col.Links, classGlobalVariables.currentTutChapter).Split(";")

            ' Load Video
            Literal1.Text = YouTubeScriptCS.Get(tutorialData(Col.Video, classGlobalVariables.currentTutChapter), IsNewChapter, _W, _H, False)
            ' Might also be able to use
            ' Literal1.Text = "<iframe width = """ & _W & """ height = """ & _H & """ src = ""http://www.youtube.com/embed/" & tutorialData(Col.Video, classGlobalVariables.currentTutChapter) & """ frameborder = ""0""></iframe>"
            ' Some how figure out when the video ends

            ' Populate instruction box
            If tutorialData(Col.Input2, classGlobalVariables.currentTutChapter) <> "" Then
                entryText = tutorialData(Col.Input1, classGlobalVariables.currentTutChapter) & """ or just """ & tutorialData(Col.Input2, classGlobalVariables.currentTutChapter)
            Else
                entryText = tutorialData(Col.Input1, classGlobalVariables.currentTutChapter)
            End If
            expectedEntryMessage = "Type """ & entryText & """ in the box above, then hit enter to " & tutorialData(Col.Result, classGlobalVariables.currentTutChapter) & Environment.NewLine & IIf(tutorialData(Col.SubNote, classGlobalVariables.currentTutChapter) <> "", "Note: " & tutorialData(Col.SubNote, classGlobalVariables.currentTutChapter), "")
            txtInstructionsBox.Text = expectedEntryMessage

            ' Populate chapter list
            If (Not Page.IsPostBack And Not IsPostBack) Or (chapterAdvancedNormally = True) Then
                dbxSelectSkipTo.SelectedIndex = classGlobalVariables.currentTutChapter
            End If

            ' Populate links column
            pnlLinks.Controls.Clear()
            Dim linkLabel As New Label
            linkLabel.Text = "Links:"
            linkLabel.Width = 195
            pnlLinks.Controls.Add(linkLabel)
            For i = 0 To linkInfo.Length - 1 Step 2
                If linkInfo.Length - 1 >= i + 1 Then
                    If linkInfo(i) <> "" And linkInfo(i + 1) <> "" Then
                        Dim newLink As New HyperLink
                        newLink.Text = linkInfo(i).Trim
                        newLink.NavigateUrl = linkInfo(i + 1).Trim
                        newLink.Style.Add(HtmlTextWriterStyle.Display, "block")
                        newLink.Target = "_blank"
                        pnlLinks.Controls.Add(newLink)
                    End If
                End If
            Next i

            enterKeyPressed = False
        ElseIf classGlobalVariables.currentTutChapter >= tutorialData.GetUpperBound(1) + 1 Then
            txtReturnMsgBox.Text = "Congratulations! You have finished the ArmageddonMUD new player tutorial.  Don't forget to have a look over some of the other new player documentation available on the site, as it is your best bet for getting the hang of the game!  We hope you enjoy it!"
            'MsgBox doesn't work in ASP .NET (it pops up on the server)
            ' MsgBox("Congratulations! You have finished the ArmageddonMUD new player tutorial.  Don't forget to have a look over some of the other new player documentation available on the site, as it is your best bet for getting the hang of the game!  We hope you enjoy it!", MsgBoxStyle.Information, "Tutorial Complete")
            ' Restart from the beginning (The 'if' is just so we don't get stuck in an endless loop of loading chapter zero if there are no chapters for some reason)
            If classGlobalVariables.currentTutChapter > 0 Then
                classGlobalVariables.currentTutChapter = 0
                chapterAdvancedNormally = True
                LoadChapter(True)
            End If
        End If
    End Sub

#End Region 'Functions

End Class