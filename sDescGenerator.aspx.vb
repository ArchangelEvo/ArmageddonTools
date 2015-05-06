Imports System.IO

Partial Class sDescGenerator

    Inherits System.Web.UI.Page

#Region "Consts"
    Private Const dnu As String = "[Do Not Use]"
    Private Const randomChoice As String = "[Random]"
    Private Const maxDescLength As Integer = 35 ' Characters
    Private Const punctSpace As Integer = 3 ' 2 for comma & space, one for pre-race space
    Private Const minPairedWordLen As Integer = 5 ' 1 for hyphen, 4 for paired word
    Private Const maxRejectionTimeout As Integer = 50000 ' Timeout if we reject more than this number of terms in the space of one run
#End Region 'Consts


#Region "Variables"
    Public descChoices(-1) As DescList
    Public descWords(-1) As DescList
#End Region 'Variables


#Region "Enums"

    Private Enum configCategory As Integer
        gender
        race
        height
        build
        weight
        hairstyle
        overall
        idealized
        color
        lastunused
    End Enum

    Private Enum Col As Integer
        Category            ' The primary category of this row (some, such as color, are not valid choices by themselves, they must be combined)
        BaseChoices         ' Represents the choice in the category that this row is associated with
        SubCat              ' Represents the second category that this row references
        SubChoiceOrDiv      ' Represents the choice in the second category that this row is associated with
        Qualifier           ' A variable representing the word part (first word of compound, second word of compound, stand alone, or any word part)
        Start               ' The column where the descriptive words actually start
    End Enum

#End Region 'Enums


#Region "Initialization"

    Private Sub frmMain_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        BuildDataArrays()
    End Sub

#End Region 'Initialization


#Region "Controls"

    Private Sub NumericUpDown1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudResults.SelectedIndexChanged
        'Dim formSize As New System.Drawing.Size(594, 426)

        'formSize.Height += (31 + (22 * Val(nudResults.SelectedValue) - 1))

        txtDesc.Height = (19 + (16.7 * Val(nudResults.SelectedValue) - 1))
        'txtDesc.Rows = Val(nudResults.SelectedValue)

        'Me.size = formSize

        'Me.Form.
    End Sub

    Private Sub cmbGender_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbGender.SelectedIndexChanged, cmbRace.SelectedIndexChanged, cmbHeight.SelectedIndexChanged, cmbBuild.SelectedIndexChanged, cmbWeight.SelectedIndexChanged, cmbHairColor.SelectedIndexChanged, cmbHairstyle.SelectedIndexChanged, cmbEyeColor.SelectedIndexChanged, cmbSkinColor.SelectedIndexChanged, cmbOverall.SelectedIndexChanged
        Dim thisCMB As System.Web.UI.WebControls.DropDownList = CType(sender, System.Web.UI.WebControls.DropDownList)
        If thisCMB.SelectedValue = dnu Then
            thisCMB.BackColor = Drawing.Color.LavenderBlush
        ElseIf thisCMB.SelectedValue = randomChoice Then
            thisCMB.BackColor = Drawing.Color.MintCream
        Else
            thisCMB.BackColor = Drawing.Color.White
        End If
        If sender Is cmbRace Then
            ' Setting the selected value here does not seem to trigger this sub, so we have to set the colors manually
            If thisCMB.SelectedValue = "dwarf" Or thisCMB.SelectedValue = "mul" Then
                cmbHairColor.SelectedValue = dnu
                cmbHairstyle.SelectedValue = dnu
                cmbHairColor.Enabled = False
                cmbHairstyle.Enabled = False
                cmbHairColor.BackColor = Drawing.Color.LavenderBlush
                cmbHairstyle.BackColor = Drawing.Color.LavenderBlush
            ElseIf cmbHairColor.Enabled = False And cmbHairstyle.Enabled = False Then
                ' Set once when returning to race with hair
                cmbHairColor.SelectedValue = randomChoice
                cmbHairstyle.SelectedValue = randomChoice
                cmbHairColor.Enabled = True
                cmbHairstyle.Enabled = True
                cmbHairColor.BackColor = Drawing.Color.MintCream
                cmbHairstyle.BackColor = Drawing.Color.MintCream
            Else
                cmbHairColor.Enabled = True
                cmbHairstyle.Enabled = True
            End If
        End If

    End Sub

    Private Sub btnGenerate_Click(sender As System.Object, e As System.EventArgs) Handles btnGenerate.Click
        'Me.Enabled = False
        'Me.Cursor = Cursors.WaitCursor

        Dim choiceBoxes() As System.Web.UI.WebControls.DropDownList = {cmbHeight, cmbBuild, cmbWeight, cmbHairColor, cmbHairstyle, cmbEyeColor, cmbSkinColor, cmbOverall, cmbGender, cmbRace}
        Dim fullDescList(-1) As generatedAdjective
        Dim primaryDescList(-1) As generatedAdjective
        Dim currentSeedRandom As New Random

        ' We need to use something from gender or race, can't have dnu for both
        If cmbGender.SelectedValue = dnu Then
            If cmbRace.SelectedValue = dnu Or cmbRace.SelectedValue = "human" Then
                AddOptions(primaryDescList, "master", "human", randomChoice)
            Else
                AddOptions(primaryDescList, "master", cmbRace.SelectedValue, "")
            End If
        Else
            If cmbRace.SelectedValue = dnu Or cmbRace.SelectedValue = "human" Then
                AddOptions(primaryDescList, "master", "human", cmbGender.SelectedValue)
            Else
                AddOptions(primaryDescList, "master", cmbRace.SelectedValue, cmbGender.SelectedValue)
            End If
        End If

        For i = 0 To choiceBoxes.Length - 1
            If choiceBoxes(i).SelectedValue <> dnu Then
                If choiceBoxes(i) Is cmbHeight Then
                    AddOptions(fullDescList, "height", choiceBoxes(i).SelectedValue, randomChoice)
                ElseIf choiceBoxes(i) Is cmbBuild Then
                    If cmbWeight.SelectedValue <> dnu Then
                        AddOptions(fullDescList, "weight", choiceBoxes(i).SelectedValue, cmbWeight.SelectedValue)
                    End If
                ElseIf choiceBoxes(i) Is cmbHairColor Then
                    AddOptions(fullDescList, "color", choiceBoxes(i).SelectedValue, "hair", "haircolor")
                ElseIf choiceBoxes(i) Is cmbHairstyle Then
                    AddOptions(fullDescList, "hairstyle", choiceBoxes(i).SelectedValue, "hair", "haircolor") ' Intentional - we make the style the second word of the color pair
                ElseIf choiceBoxes(i) Is cmbEyeColor Then
                    AddOptions(fullDescList, "color", choiceBoxes(i).SelectedValue, "eyes", "eyecolor")
                ElseIf choiceBoxes(i) Is cmbSkinColor Then
                    AddOptions(fullDescList, "color", choiceBoxes(i).SelectedValue, "skin", "skincolor")
                ElseIf choiceBoxes(i) Is cmbOverall Then
                    AddOptions(fullDescList, "overall", choiceBoxes(i).SelectedValue, "", "overall", False)
                    AddOptions(fullDescList, "overall", choiceBoxes(i).SelectedValue, "hair", "haircolor", False)
                    AddOptions(fullDescList, "overall", choiceBoxes(i).SelectedValue, "eyes", "eyecolor", False)
                    If cmbSkinColor.SelectedValue = "brown" Or cmbSkinColor.SelectedValue = "tan" Then
                        ' This isn't as flexible as I'd like it to be
                        AddOptions(fullDescList, "overall", choiceBoxes(i).SelectedValue, "skin", "skincolor", False)
                    End If
                End If
            End If
        Next i

        If cbxIdeal.Checked = True Then
            AddOptions(fullDescList, "idealized", cmbGender.SelectedValue, cmbBuild.SelectedValue, "overall", False) ' Intentional - we make the idealized words just like the rest of the overall words (even though they are gender dependent)
        End If

        txtDesc.Text = GenerateSDesc(fullDescList, primaryDescList, currentSeedRandom)
        For i = 0 To Val(nudResults.SelectedValue) - 2
            txtDesc.Text &= vbCrLf & GenerateSDesc(fullDescList, primaryDescList, currentSeedRandom)
        Next i

        'Me.Enabled = True
        'Me.Cursor = Cursors.Arrow
    End Sub

#End Region


#Region "Functions"

    Private Function GetPairingWord(ByVal descArray() As generatedAdjective, ByVal adjToPair As Integer, ByVal currentRandom As Random, ByRef rejectCount As Integer, Optional ByVal maxLength As Integer = -1) As String
        Dim subDescList(-1) As generatedAdjective

        For i = 0 To descArray.Length - 1
            If descArray(i).Category = descArray(adjToPair).Category Then
                If (descArray(i).WordPart = 0 And (descArray(adjToPair).WordPart = 1 Or descArray(adjToPair).WordPart = 3)) Or _
                    ((descArray(i).WordPart = 1 Or descArray(i).WordPart = 3) And descArray(adjToPair).WordPart = 0) And _
                    (maxLength = -1 Or ((descArray(i).DescWord.Length + descArray(adjToPair).DescWord.Length + 1) <= maxLength)) Then
                    ' Always need a 0 and 1/3 together
                    ReDim Preserve subDescList(subDescList.Length)
                    subDescList(subDescList.Length - 1) = descArray(i)
                Else
                    AddToRejections(rejectCount)
                End If
            Else
                AddToRejections(rejectCount)
            End If
        Next i

        If subDescList.Length <= 0 Then
            ' We might accidentally plug this value into an sDesc, as it isn't always being checked for
            Return "No Match"
        Else
            Return subDescList(currentRandom.Next(0, subDescList.Length)).DescWord
        End If

    End Function

    ''' <summary>
    ''' This sub adds to a sub array of descriptive terms that are valid FOR THIS SELECTION SET.
    ''' </summary>
    ''' <param name="descArray">The new sub array to be populated.</param>
    ''' <param name="category">The original category of the descriptive words being passed.</param>
    ''' <param name="choice">The user's trait choice that matches the description words (or random).</param>
    ''' <param name="subchoice">The user's second trait choice that matches the descriptive words (or random, or forced, to add a row that is specific to one trait to the general).</param>
    ''' <param name="newCatName">The new category for the descriptive words (this changes from the original category where the original distinction from another cateogry is no longer necessary, or where it has become more granular).</param>
    ''' <param name="noSubCatOkay">True if it is okay to have general terms ("" - no second choice) as well as whatever is passed for the subchoice.  False if you ONLY want to add the words associated with the subchoice.</param>
    ''' <remarks></remarks>
    Private Sub AddOptions(ByRef descArray() As generatedAdjective, ByVal category As String, ByVal choice As String, ByVal subchoice As String, Optional ByVal newCatName As String = "", Optional ByVal noSubCatOkay As Boolean = True)
        For i = 0 To descWords.Length - 1
            If descWords(i).Category = category Then
                If (choice = randomChoice Or descWords(i).Choice = choice Or descWords(i).Choice = "") And (subchoice = randomChoice Or descWords(i).SubChoice = subchoice Or (noSubCatOkay And descWords(i).SubChoice = "")) Then
                    For j = 0 To descWords(i).Contents.Length - 1
                        ReDim Preserve descArray(descArray.Length)
                        descArray(descArray.Length - 1) = New generatedAdjective
                        descArray(descArray.Length - 1).Category = IIf(newCatName = "", category, newCatName)
                        descArray(descArray.Length - 1).SubCategory = descWords(i).SubCategory
                        descArray(descArray.Length - 1).DescWord = descWords(i).Contents(j)
                        descArray(descArray.Length - 1).WordPart = descWords(i).Qualifier
                        descArray(descArray.Length - 1).Choice = descWords(i).Choice
                        descArray(descArray.Length - 1).SubChoice = descWords(i).SubChoice
                    Next j
                End If
            End If
        Next i
    End Sub

    Private Function GenerateSDesc(ByVal fullDescList() As generatedAdjective, ByVal primaryDescList() As generatedAdjective, ByVal currentRandom As Random) As String
        Dim randomizedDesc As String = "the "
        Dim firstAdj As Integer ' The index of the first descriptive word
        Dim secondAdj As Integer ' The index of the second descriptive word
        Dim primaryAdj As Integer ' The index of the primary descriptive word (usually race)
        Dim buildAdj As Integer = -1 ' The index of the build for term agreement
        Dim enoughForOneWord As Boolean = False ' Whether we have enough descriptive words available (and appropriate) to get one term
        Dim enoughForTwoWords As Boolean = False ' Whether we have enough descriptive words available (and appropriate, and short enough) to get a second term
        Dim totalTermRejections As Integer = 0 ' For debug purposes only

        primaryAdj = currentRandom.Next(0, primaryDescList.Length)
        If cmbBuild.SelectedValue <> dnu Then
            ' we need to determine the chosen, or a random build, then use it to check all terms in the generated sdesc that have that as a subchoice
            ' We don't have to worry about weight, because it only is used as a subchoice/choice once (build is used twice, and can therefore contradict itself)
            If cmbBuild.SelectedValue = randomChoice Then
                buildAdj = currentRandom.Next(2, cmbBuild.Items.Count)
            Else
                buildAdj = cmbBuild.SelectedIndex
            End If
        End If

        For i = 0 To fullDescList.Length - 1
            ' Check to make sure we won't get stuck in an infinite loop
            If NoContradictionsPresent(primaryDescList(primaryAdj), fullDescList(i), buildAdj) Then
                enoughForOneWord = True
                Exit For
            Else
                AddToRejections(totalTermRejections)
            End If
        Next i

        If enoughForOneWord Then
            ' Build the Randomized Desc
            If fullDescList.Length <> 0 Then
                Do
                    AddToRejections(totalTermRejections) 'The first count is spurious, but that's okay, we just need a general idea
                    firstAdj = currentRandom.Next(0, fullDescList.Length)
                Loop Until NoContradictionsPresent(primaryDescList(primaryAdj), fullDescList(firstAdj), buildAdj)
                Select Case fullDescList(firstAdj).WordPart
                    Case 0
                        randomizedDesc &= fullDescList(firstAdj).DescWord & "-" & GetPairingWord(fullDescList, firstAdj, currentRandom, totalTermRejections)
                    Case 1
                        randomizedDesc &= GetPairingWord(fullDescList, firstAdj, currentRandom, totalTermRejections) & "-" & fullDescList(firstAdj).DescWord
                    Case 2
                        randomizedDesc &= fullDescList(firstAdj).DescWord
                    Case 3
                        Dim oneOrTwo As Integer = currentRandom.Next(1, 3)
                        If oneOrTwo = 1 Then
                            randomizedDesc &= GetPairingWord(fullDescList, firstAdj, currentRandom, totalTermRejections) & "-" & fullDescList(firstAdj).DescWord
                        Else
                            randomizedDesc &= fullDescList(firstAdj).DescWord
                        End If
                End Select

                Dim maxLengthOf2ndWord As Integer = maxDescLength - (randomizedDesc.Length + primaryDescList(primaryAdj).DescWord.Length + 3) ' 2 is for the comma and following space, 1 for pre-race space
                Dim lenTrimmedDescList(-1) As generatedAdjective
                If (currentRandom.Next(1, 11) Mod 10) <> 0 Then
                    ' When Mod 10 = 0, we want to skip finding a second term (about 10% of the time), to make one description sDescs

                    For i = 0 To fullDescList.Length - 1
                        ' Check to make sure we won't get stuck in an infinite loop
                        If (NoContradictionsPresent(primaryDescList(primaryAdj), fullDescList(i), buildAdj)) And (fullDescList(i).Category <> fullDescList(firstAdj).Category) Then
                            If (fullDescList(i).DescWord.Length <= maxLengthOf2ndWord) Then
                                If (fullDescList(i).WordPart = 2 Or fullDescList(i).WordPart = 3) Or (fullDescList(i).DescWord.Length + minPairedWordLen <= maxLengthOf2ndWord) Then
                                    ' Even if this word fits, if it needs a pair, that has to fit also
                                    ReDim Preserve lenTrimmedDescList(lenTrimmedDescList.Length)
                                    lenTrimmedDescList(lenTrimmedDescList.Length - 1) = fullDescList(i)
                                    ' This only truly works for words with no pair (wordpart 2), the others could be rendered too long by the pair
                                    enoughForTwoWords = True
                                    ' We can't exit this For/Loop, because we need to build the sub array
                                End If
                            Else
                                AddToRejections(totalTermRejections)
                            End If
                        Else
                            AddToRejections(totalTermRejections)
                        End If
                    Next i
                End If

                If randomizedDesc = "the " Then
                    Console.WriteLine("Strange result")
                End If

                If enoughForTwoWords = True Then
                    For i = 0 To fullDescList.Length - 1
                    Next i
                    Dim secondDescWord As String = ""
                    Do
                        Do
                            AddToRejections(totalTermRejections) 'The first count is spurious, but that's okay, we just need a general idea
                            secondAdj = currentRandom.Next(0, lenTrimmedDescList.Length)
                        Loop Until (lenTrimmedDescList(secondAdj).Category <> fullDescList(firstAdj).Category) And (NoContradictionsPresent(primaryDescList(primaryAdj), lenTrimmedDescList(secondAdj), buildAdj))
                        Dim pairedWord As String = ""
                        Select Case lenTrimmedDescList(secondAdj).WordPart
                            Case 0, 1
                                pairedWord = GetPairingWord(lenTrimmedDescList, secondAdj, currentRandom, totalTermRejections, maxLengthOf2ndWord)
                            Case 3
                                Dim oneOrTwo As Integer = currentRandom.Next(1, 3)
                                If oneOrTwo = 1 And (lenTrimmedDescList(secondAdj).DescWord.Length + minPairedWordLen <= maxLengthOf2ndWord) Then
                                    ' Default to stand alone word if making it compound will be too long
                                    pairedWord = GetPairingWord(lenTrimmedDescList, secondAdj, currentRandom, totalTermRejections, maxLengthOf2ndWord)
                                End If
                        End Select
                        If pairedWord <> "No Match" Then
                            ' If there is no matching paired word for this word, then we need a new seed base word (if it doesn't need a paired word, the string will be "" as dimensioned)
                            Select Case lenTrimmedDescList(secondAdj).WordPart
                                Case 0
                                    secondDescWord = lenTrimmedDescList(secondAdj).DescWord & "-" & pairedWord
                                Case 1
                                    secondDescWord = pairedWord & "-" & lenTrimmedDescList(secondAdj).DescWord
                                Case 2
                                    secondDescWord = lenTrimmedDescList(secondAdj).DescWord
                                Case 3
                                    If pairedWord = "" Then
                                        ' Was randomized to be a single word above
                                        secondDescWord = lenTrimmedDescList(secondAdj).DescWord
                                    Else
                                        secondDescWord = pairedWord & "-" & lenTrimmedDescList(secondAdj).DescWord
                                    End If
                            End Select
                        End If
                        If totalTermRejections > maxRejectionTimeout Then
                            ' We're spending too long trying to find a match for this word, just forget it
                            enoughForTwoWords = False
                        End If
                    Loop Until secondDescWord <> "" Or enoughForTwoWords = False
                    If secondDescWord <> "" Then
                        randomizedDesc &= ", " & secondDescWord
                    End If
                End If
            End If

        End If

        If Microsoft.VisualBasic.Right(randomizedDesc, 1) = "," Then
            Console.WriteLine("Strange result")
        End If

        randomizedDesc &= " " & primaryDescList(primaryAdj).DescWord

        Console.WriteLine(totalTermRejections)

        Return randomizedDesc

    End Function

    Private Function NoContradictionsPresent(ByVal masterAdj As generatedAdjective, ByVal descAdj As generatedAdjective, ByVal buildAdj As Integer) As Boolean
        Dim ContradictionsPresent As Boolean = False

        If ((masterAdj.Choice = "dwarf" Or masterAdj.Choice = "mul") And descAdj.Category = "haircolor") Then
            ContradictionsPresent = True
        End If
        If cbxTermAgree.Checked = True Then
            ' Sub-categories only get compared when the list is made - if the box choice is random, they won't be compared again after to ensure agreement, once a value is randomly chosen, so we need to compare them at generate time
            If (descAdj.Choice.ToLower = "female" Or descAdj.Choice.ToLower = "male") AndAlso (masterAdj.SubChoice.ToLower <> descAdj.Choice.ToLower And masterAdj.SubChoice.ToLower <> "") Then
                ContradictionsPresent = True
            End If
            If buildAdj > -1 And descAdj.SubCategory.ToLower = "build" AndAlso cmbBuild.Items(buildAdj).Text.ToLower <> descAdj.SubChoice.ToLower Then
                ContradictionsPresent = True
            End If
            If buildAdj > -1 And descAdj.Category.ToLower = "build" AndAlso cmbBuild.Items(buildAdj).Text.ToLower <> descAdj.Choice.ToLower Then
                ContradictionsPresent = True
            End If
        End If

        Return Not ContradictionsPresent
    End Function

    Private Sub AddToRejections(ByRef rejectCount As Integer)
        rejectCount += 1
        If rejectCount > 100000 Then
            ' Debug catch
            rejectCount = rejectCount
        End If
    End Sub

    Private Sub BuildDataArrays()

        Dim categoryRow As String
        Dim categoryFields() As String
        Dim thisRow As Integer = 1 'First line of labels is discarded
        Dim inChoiceSection As Boolean = True
        Dim currentCat As Integer
        Dim catRegister(configCategory.lastunused - 1) As Integer
        Dim eyeSource(), skinSource() As String

        Try
            Dim csvReader As StreamReader = New StreamReader(Server.MapPath("~/App_Data/CategoryConfig.csv"))
            'categoryRows = app_CategoryConfig.Split(vbCrLf)
            ' Get first two lines, to discard line of labels
            categoryRow = csvReader.ReadLine
            categoryRow = csvReader.ReadLine

            Do While categoryRow <> Nothing And categoryRow <> ""
                categoryFields = categoryRow.Split(",")
                If categoryFields(Col.Category).Trim = "" Then
                    inChoiceSection = False
                    categoryFields = categoryRow.Split(",")
                End If
                If inChoiceSection = True Then
                    ReDim Preserve descChoices(descChoices.Length)
                    descChoices(descChoices.Length - 1) = New DescList
                    currentCat = Array.IndexOf([Enum].GetNames(GetType(configCategory)), categoryFields(Col.Category).Trim.ToLower)
                    If currentCat >= 0 Then
                        ' Make the class array navigable by the enum
                        catRegister(currentCat) = descChoices.Length - 1
                    End If
                    descChoices(descChoices.Length - 1).Category = categoryFields(Col.Category).Trim.ToLower
                    descChoices(descChoices.Length - 1).Choice = categoryFields(Col.BaseChoices).Trim.ToLower
                    descChoices(descChoices.Length - 1).SubCategory = categoryFields(Col.SubCat).Trim.ToLower
                    descChoices(descChoices.Length - 1).SubChoice = categoryFields(Col.SubChoiceOrDiv).Trim.ToLower
                    descChoices(descChoices.Length - 1).Qualifier = ParseQualifier(categoryFields(Col.Qualifier).Trim.ToLower)
                    ReDim Preserve descChoices(descChoices.Length - 1).Contents(descChoices(descChoices.Length - 1).Contents.Length + 1)
                    descChoices(descChoices.Length - 1).Contents(0) = randomChoice
                    descChoices(descChoices.Length - 1).Contents(1) = dnu
                    For i = Col.Start To categoryFields.Length - 1
                        If categoryFields(i).Trim <> "" Then
                            ReDim Preserve descChoices(descChoices.Length - 1).Contents(descChoices(descChoices.Length - 1).Contents.Length)
                            descChoices(descChoices.Length - 1).Contents(i - Col.Start + 2) = categoryFields(i).Trim.ToLower
                        End If
                    Next i
                Else
                    ReDim Preserve descWords(descWords.Length)
                    descWords(descWords.Length - 1) = New DescList
                    descWords(descWords.Length - 1).Category = categoryFields(Col.Category).Trim.ToLower
                    descWords(descWords.Length - 1).Choice = categoryFields(Col.BaseChoices).Trim.ToLower
                    descWords(descWords.Length - 1).SubCategory = categoryFields(Col.SubCat).Trim.ToLower
                    descWords(descWords.Length - 1).SubChoice = categoryFields(Col.SubChoiceOrDiv).Trim.ToLower
                    descWords(descWords.Length - 1).Qualifier = ParseQualifier(categoryFields(Col.Qualifier).Trim.ToLower)
                    For i = Col.Start To categoryFields.Length - 1
                        If categoryFields(i).Trim <> "" Then
                            ReDim Preserve descWords(descWords.Length - 1).Contents(descWords(descWords.Length - 1).Contents.Length)
                            descWords(descWords.Length - 1).Contents(i - Col.Start) = categoryFields(i).Trim.ToLower
                        End If
                    Next i
                End If
                categoryRow = csvReader.ReadLine
            Loop

            ' Necessary to separate them so they don't all change together (bug?)
            ReDim eyeSource(descChoices(catRegister(configCategory.color)).Contents.Length - 1)
            ReDim skinSource(descChoices(catRegister(configCategory.color)).Contents.Length - 1)
            Array.Copy(descChoices(catRegister(configCategory.color)).Contents, eyeSource, descChoices(catRegister(configCategory.color)).Contents.Length)
            Array.Copy(descChoices(catRegister(configCategory.color)).Contents, skinSource, descChoices(catRegister(configCategory.color)).Contents.Length)

            If Not Page.IsPostBack Then
                cmbGender.DataSource = descChoices(catRegister(configCategory.gender)).Contents
                cmbRace.DataSource = descChoices(catRegister(configCategory.race)).Contents
                cmbHeight.DataSource = descChoices(catRegister(configCategory.height)).Contents
                cmbBuild.DataSource = descChoices(catRegister(configCategory.build)).Contents
                cmbWeight.DataSource = descChoices(catRegister(configCategory.weight)).Contents

                cmbHairColor.DataSource = descChoices(catRegister(configCategory.color)).Contents
                cmbHairstyle.DataSource = descChoices(catRegister(configCategory.hairstyle)).Contents

                cmbEyeColor.DataSource = eyeSource

                cmbSkinColor.DataSource = skinSource

                cmbOverall.DataSource = descChoices(catRegister(configCategory.overall)).Contents

                Dim choiceBoxes() As System.Web.UI.WebControls.DropDownList = {cmbHeight, cmbBuild, cmbWeight, cmbHairColor, cmbHairstyle, cmbEyeColor, cmbSkinColor, cmbOverall, cmbGender, cmbRace}
                For i = 0 To choiceBoxes.Length - 1
                    choiceBoxes(i).DataBind()
                    choiceBoxes(i).BackColor = Drawing.Color.MintCream
                    ' All are set to random to start, so all are set to green
                Next i

            End If

        Catch ex As Exception
            txtDesc.Text = "ERROR: Category Config file could not be read:" & vbCrLf & ex.Message
            'MsgBox doesn't work in ASP .NET (it pops up on the server)
            'MsgBox("Category Config file could not be read:" & vbCrLf & ex.Message, MsgBoxStyle.Exclamation, "File Read Error")
        End Try

    End Sub

    Private Function ParseQualifier(ByVal Qualifier As String) As Integer
        Select Case Qualifier
            Case "0"
                Return 0
            Case "1"
                Return 1
            Case "2"
                Return 2
            Case "3"
                Return 3
            Case Else
                Return -1
        End Select
    End Function

#End Region 'Fucntions


#Region "Data Classes"

    Public Class DescList

        Public Category As String
        Public Choice As String
        Public SubCategory As String
        Public SubChoice As String
        Public Qualifier As Integer
        Public Contents(-1) As String

    End Class

    Public Class generatedAdjective

        Public Category As String
        Public Choice As String
        Public SubCategory As String
        Public SubChoice As String
        Public WordPart As Integer ' 0 = prefix, 1 = suffix, 2 = whole word
        Public DescWord As String

    End Class

#End Region 'Data Classes

End Class
