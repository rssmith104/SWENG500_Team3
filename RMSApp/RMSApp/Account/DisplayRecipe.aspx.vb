Imports System
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Owin

' ADDED RSS 9-21-2015
Imports System.Data
Imports System.Data.SqlClient
' END ADD

Partial Public Class DisplayRecipe
    Inherits Page

    Public strLoggedInUser As String
    Public strAuthenticated As String
    Public strFunction As String

    Public strFirstName As String
    Public strLastName As String
    Friend WithEvents EventLog1 As EventLog
    Public strMessage As String

    Private m_strRecipeID As String
    Private m_intRecipeID As Int16


    Protected Property NumberOfControls() As Integer
        Get
            Return m_NumberOfControls
        End Get
        Private Set(value As Integer)
            m_NumberOfControls = value
        End Set
    End Property
    Protected m_NumberOfControls As Integer

    Protected Property IngredientsNumberOfControls() As Integer
        Get
            Return m_IngredientsNumberOfControls
        End Get
        Private Set(value As Integer)
            m_IngredientsNumberOfControls = value
        End Set
    End Property
    Protected m_IngredientsNumberOfControls As Integer

    ''' <summary>
    ''' Register_Load - Page Load Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DisplayRecipe_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' On Page Load of the Registration Page, we want to populate the SecurityQuestion dropdown listbox.
        ' Only perform is it is new.  If Postback, we should already be good to go.

        strLoggedInUser = Session("RMS_LoggedInUser")
        strAuthenticated = Session("RMS_Authenticated")
        strFunction = Session("RMS_Function")

        If Not IsPostBack Then
            ' On initial load, we need to first extract the target recipe from the querystring indicator
            m_strRecipeID = Request.QueryString("RecipeID")

            ' We then transalate it into an integer
            m_intRecipeID = ExtractRecipeID(m_strRecipeID)
            Me.hdnRecipeID.Value = m_intRecipeID.ToString

            ' Bind the data to the form controls on the page.
            BindRecipeContent(m_intRecipeID)
        Else
            m_intRecipeID = CInt(Me.hdnRecipeID.Value)
        End If
    End Sub

    Private Function BindRecipeContent(ByVal intRecipeID As Int16) As String
        Dim strRetVal As String = ""

        If intRecipeID = 0 Then
            strRetVal = BindDemoRecipeData()
        Else
            strRetVal = BindRecipeData(intRecipeID)
        End If

        Return strRetVal
    End Function

    Private Function BindRecipeData(ByVal intRecipeID As Int16) As String
        Dim strRetVal As String = ""

        BindRecipeHeaderData(intRecipeID)
        BindRecipeImageData(intRecipeID)
        BindIngredientData(intRecipeID)
        BindInstructionData(intRecipeID)

        Return strRetVal
    End Function

    Private Sub BindRecipeHeaderData(ByVal intRecipeID As Int16)
        '
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader
        Dim strConnectionString As String
        Dim strRating As String
        Dim strReviewCount As Integer

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@RecipeID", SqlDbType.Int, 1, intRecipeID)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_Recipe_Select_byRecipeID", objParams)

        ' Do we have rows returned?
        If objDR.HasRows Then

            objDR.Read()

            'Add Header Data
            ' Bind the Recipe Name
            Me.ltRecipeName.Text = objDR("RecipeName").ToString

            ' Bind the Owner Name
            Me.ltOwner.Text = objDR("Owner_Email").ToString
            Me.ltSubmissionDate.Text = objDR("SubmissionDate").ToString

            '<TODO retrieve rating from database on bind to Page Data
            ' Bind the Rating
            strRating = GetRating(intRecipeID)
            strReviewCount = GetReviewCount(intRecipeID)

            If strRating <> "No Rate" Then
                Me.ltRating.Text = FormatNumber(CDbl(strRating), 1).ToString & " of 5"
                Me.hypReadReviews.Enabled = True
                Me.hypReadReviews.Text = strReviewCount & " Reviews Found"
            Else
                Me.ltRating.Text = "No Rating"
                Me.hypReadReviews.Enabled = False
                Me.hypReadReviews.Text = "0 Reveiws Found"
            End If
            Me.imgRating.ImageUrl = Me.RatingImageSelection(strRating)

            'Bind the Recipe Description
            Me.ltRecipeDescription.Text = objDR("RecipeDescription").ToString

            ' Serving Size 
            Me.ltServingSize.Text = objDR("ServingSize").ToString & " Portion(s)"
        Else
            '<TODO>  Assign Invalid Recipe
        End If

        ' CleanUp on our way out.  Make sure that we kill the connection so that we do not run our limit on concurrent 
        ' database connections.
        If Not IsNothing(objDR) Then
            objDR.Close()
            objDR = Nothing
        End If

        If Not IsNothing(objData_DB) Then
            objData_DB.Close()
            objData_DB = Nothing
        End If

    End Sub

    Private Function GetRating(ByVal intRecipeID As Integer) As String
        Dim strRetVal As String = ""
        Dim objData_DB As clsData_DB
        Dim strConnectionString As String
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@RecipeID ", SqlDbType.Int, 4, CInt(intRecipeID))

        objDR = objData_DB.RunStoredProc("usp_Review_Select_Total_byRecipeID", objParams)

        If objDR.HasRows Then
            objDR.Read()
            strRetVal = objDR("AverageScore".ToString)
        End If

        ' CleanUp on our way out.  Make sure that we kill the connection so that we do not run our limit on concurrent 
        ' database connections.
        If Not IsNothing(objDR) Then
            objDR.Close()
            objDR = Nothing
        End If

        If Not IsNothing(objData_DB) Then
            objData_DB.Close()
            objData_DB = Nothing
        End If

        Return strRetVal
    End Function

    Private Function GetReviewCount(ByVal intRecipeID As Integer) As String
        Dim strRetVal As String = ""
        Dim objData_DB As clsData_DB
        Dim strConnectionString As String
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@RecipeID ", SqlDbType.Int, 4, CInt(intRecipeID))

        objDR = objData_DB.RunStoredProc("usp_Review_Select_Total_byRecipeID", objParams)

        If objDR.HasRows Then
            objDR.Read()
            strRetVal = objDR("QtyReviews").ToString
        End If

        ' CleanUp on our way out.  Make sure that we kill the connection so that we do not run our limit on concurrent 
        ' database connections.
        If Not IsNothing(objDR) Then
            objDR.Close()
            objDR = Nothing
        End If

        If Not IsNothing(objData_DB) Then
            objData_DB.Close()
            objData_DB = Nothing
        End If

        Return strRetVal
    End Function

    Private Sub BindRecipeImageData(ByVal intRecipeID As Int16)
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader
        Dim strConnectionString As String

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@RecipeID", SqlDbType.Int, 1, intRecipeID)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_RecipeImage_Select", objParams)

        ' Do we have rows returned?
        If objDR.HasRows Then

            objDR.Read()

            'Add Image
            Me.imgRecipeImage.ImageUrl = objDR("ImageText").ToString
        Else
            '<TODO>  Assign No Image Image
        End If

        ' CleanUp on our way out.  Make sure that we kill the connection so that we do not run our limit on concurrent 
        ' database connections.
        If Not IsNothing(objDR) Then
            objDR.Close()
            objDR = Nothing
        End If

        If Not IsNothing(objData_DB) Then
            objData_DB.Close()
            objData_DB = Nothing
        End If

    End Sub

    Private Sub BindInstructionData(ByVal intRecipeID As Int16)
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader
        Dim strConnectionString As String

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@RecipeID", SqlDbType.Int, 1, intRecipeID)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_InstructionStep_SelectByRecipe", objParams)

        ' Do we have rows returned?
        If objDR.HasRows Then
            AddInstructionHeader()
            Dim iCounter As Integer = 1
            While objDR.Read()

                'Add Instructions
                AddInstruction(objDR("StepText").ToString, iCounter)
                iCounter += 1
            End While
        End If

        ' CleanUp on our way out.  Make sure that we kill the connection so that we do not run our limit on concurrent 
        ' database connections.
        If Not IsNothing(objDR) Then
            objDR.Close()
            objDR = Nothing
        End If

        If Not IsNothing(objData_DB) Then
            objData_DB.Close()
            objData_DB = Nothing
        End If

    End Sub

    Private Sub BindIngredientData(ByVal intRecipeID As Int16)
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader
        Dim strConnectionString As String

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@RecipeID", SqlDbType.Int, 1, intRecipeID)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_RecipeIngredient_SelectByRecipe", objParams)

        ' Do we have rows returned?
        If objDR.HasRows Then
            AddIngredientHeader()
            Dim iCounter As Integer = 1
            While objDR.Read()

                'Add Ingredient
                AddIngredient(objDR("IngredientName").ToString, objDR("Qty").ToString, objDR("UOMType").ToString, objDR("PreparationText").ToString, iCounter)
                iCounter += 1
            End While
        End If

        ' CleanUp on our way out.  Make sure that we kill the connection so that we do not run our limit on concurrent 
        ' database connections.
        If Not IsNothing(objDR) Then
            objDR.Close()
            objDR = Nothing
        End If

        If Not IsNothing(objData_DB) Then
            objData_DB.Close()
            objData_DB = Nothing
        End If
    End Sub

    Private Function BindDemoRecipeData() As String
        Dim strRetVal As String = ""

        ' Bind the Recipe Name
        Me.ltRecipeName.Text = "Grandms's Famous Old Country Meat Lasagna"

        ' Bind the Owner Name
        Me.ltOwner.Text = "rss261@psu.edu"
        Me.ltSubmissionDate.Text = "February 3, 2014"

        ' Bind the Rating
        Me.ltRating.Text = "4.5 of 5"
        Me.imgRating.ImageUrl = RatingImageSelection("4.5")

        'Bind the Image
        Me.imgRecipeImage.ImageUrl = "~/images/Lasagna_Meat.jpg"

        'Bind the Recipe Description
        Me.ltRecipeDescription.Text = "Everyone loves a good lasagna, right? It's a great way to feed a crowd and a perfect dish " &
            "to bring to a potluck. It freezes well. It reheats well. Leftovers will keep you happy for days. " &
            "Simply Recipes reader Alton Hoover sent me his favorite recipe for lasagna which he has been " &
            "cooking up since college days. His original recipe created enough lasagna for a small army so " &
            "I halved it. What Is posted here will easily serve 8 people"

        ' Serving Size 
        Me.ltServingSize.Text = "8 Portions"

        ' Add Ingredients
        AddIngredientHeader()
        AddIngredient("fresh ginger", "1", "Piece(s)", "peeled, 1"" piece", 1)
        AddIngredient("Fresh Parsley", "1", "Piece(s)", "", 2)
        AddIngredient("Black Peppercorns", "1", "Teaspoon(s)", "Whole ", 3)
        AddIngredient("Chicken", "3 1/2 - 4", "Pound(s)", "Whole", 4)
        AddIngredient("Kosher Salt", "1", "Teaspoon(s)", "", 5)
        AddIngredient("dried, extra wide Egg Noodles", "8", "Ounce(s)", "(4 cups cooked)", 6)
        AddIngredient("Lemon juice", "1/4", "Cup(s)", "", 7)
        AddIngredient("Fresh Dill", "1/4", "Cup(s)", "Snipped", 8)

        'Add Instructions
        AddInstructionHeader()
        AddInstruction("Cut the stems from the bunch of parsley, reserving the leaves for another use. Place the parsley stems, ginger, garlic, cloves, peppercorns, chicken, carrots, celery, parsnips, onion, and salt in a la", 1)
        AddInstruction("Skim the surface with a slotted spoon. Cover; reduce the heat to medium-low and simmer for 40 to 50 minutes or until the chicken is cooked through and is falling away from the bones.", 2)
        AddInstruction("Turn off the heat. Carefully remove the chicken and set aside. Place a large strainer over a very large bowl. Pour the broth through the strainer. Remove the carrots and parsnips to a cutting board an", 3)
        AddInstruction("Carefully pour the broth back into the pot and bring to boiling over high heat. Add the noodles and cook for 8 to 10 minutes or until tender. Remove pot from heat. Add the chicken, carrots and parsnip", 4)

        strRetVal = "SUCCESS"

        Return strRetVal

    End Function

    Private Sub AddIngredient(ByVal strIngredName As String,
                              ByVal strQty As String,
                              ByVal strUOM As String,
                              ByVal strPrepText As String,
                              ByVal intIngredNo As Int16)

        Dim strIngredText As String
        Dim objBtnCtrl As Button = New Button()

        objBtnCtrl.Text = "Purchase Online"
        objBtnCtrl.ID = "btnIngred_" & intIngredNo.ToString
        objBtnCtrl.Attributes.Add("Class", "btn btn-primary btn-sm")

        strIngredText = "<p style=""font-family: 'Courier New'"">" & PadRightSpace(CInt(intIngredNo), 4) & PadRightSpace(strIngredName, 32) & PadRightSpace(strQty, 12) & PadRightSpace(strUOM, 12) & PadRightSpace(strPrepText, 20)

        Me.pnlIngredients.Controls.Add(New LiteralControl(strIngredText))
        Me.pnlIngredients.Controls.Add(objBtnCtrl)
        Me.pnlIngredients.Controls.Add(New LiteralControl("</p>"))  '<br />

    End Sub

    Private Sub AddIngredientHeader()
        Dim strIngredHdr As String

        strIngredHdr = "<p style=""font-family: 'Courier New'""><b>" &
                       PadRightSpace("NO", 4) &
                       PadRightSpace("INGREDIENT", 32) &
                       PadRightSpace("QUANTITY", 12) &
                       PadRightSpace("MEASUREMENT", 12) &
                       PadRightSpace("PREPARATION", 20) &
                       "</b></p>"
        Me.pnlIngredients.Controls.Add(New LiteralControl(strIngredHdr))

    End Sub

    Private Function RatingImageSelection(ByVal strRating As String) As String
        Dim dblRating As Double
        Dim strReturnImageRef As String

        If strRating = "No Rate" Then
            strReturnImageRef = "~/images/RMSRating_NoRating.png"
        Else
            dblRating = CDbl(strRating)

            If dblRating > 4.75 Then
                strReturnImageRef = "~/images/RMSRating_5.0.png"
            ElseIf dblRating > 4.25 Then
                strReturnImageRef = "~/images/RMSRating_4.5.png"
            ElseIf dblRating > 3.75 Then
                strReturnImageRef = "~/images/RMSRating_4.0.png"
            ElseIf dblRating > 3.25 Then
                strReturnImageRef = "~/images/RMSRating_3.5.png"
            ElseIf dblRating > 2.75 Then
                strReturnImageRef = "~/images/RMSRating_3.0.png"
            ElseIf dblRating > 2.25 Then
                strReturnImageRef = "~/images/RMSRating_2.5.png"
            ElseIf dblRating > 1.75 Then
                strReturnImageRef = "~/images/RMSRating_2.0.png"
            ElseIf dblRating > 1.25 Then
                strReturnImageRef = "~/images/RMSRating_1.5.png"
            ElseIf dblRating > 0.75 Then
                strReturnImageRef = "~/images/RMSRating_1.0.png"
            ElseIf dblRating > 0.25 Then
                strReturnImageRef = "~/images/RMSRating_0.5.png"
            Else
                strReturnImageRef = "~/images/RMSRating_0.5.png"
            End If

        End If

        Return strReturnImageRef

    End Function
    Private Function PadRightSpace(ByVal strText As String, ByVal intPadNo As Int16) As String
        Dim strRet As String
        Dim iCounter As Int16 = 0

        strRet = Trim(strText)

        Do While iCounter < intPadNo - Len(Trim(strText))
            strRet &= "&nbsp;"
            iCounter += 1
        Loop

        Return strRet

    End Function

    Private Sub AddInstruction(ByVal strInstruction As String, ByVal intStepNo As Int16)
        Dim strInstructionText As String
        Dim objLabelCtrl As Label = New Label()

        ' Setup Textbox Attributes
        strInstructionText = "<p style=""font-family: 'Courier New'"">&nbsp;&nbsp;&nbsp;"

        objLabelCtrl.ID = "lblInstruction_" & intStepNo.ToString
        'objTxtCtrl.Attributes.Add("style", "width:80%")
        objLabelCtrl.Text = strInstruction

        Me.pnlInstructions.Controls.Add(objLabelCtrl)
        Me.pnlInstructions.Controls.Add(New LiteralControl("</p>"))  '<br />

    End Sub

    Private Sub AddInstructionHeader()
        Dim strInstructionHdr As String

        strInstructionHdr = "<p style=""font-family: 'Courier New'""><b>INSTRUCTIONS</b></p>"

        Me.pnlInstructions.Controls.Add(New LiteralControl(strInstructionHdr))


    End Sub

    Private Function ExtractRecipeID(ByVal strRecipeID As String) As Int16
        Dim intReturnID As Int16
        Dim strTokens() As String

        strTokens = Split(strRecipeID, "_")

        Try
            intReturnID = CInt(strTokens(1))
        Catch ex As Exception
            intReturnID = -1
        End Try

        Return intReturnID
    End Function

    Private Sub InitializeComponent()
        Me.EventLog1 = New System.Diagnostics.EventLog()
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'EventLog1
        '
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Protected Sub ReviewRecipe_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/Account/SubmitRating?RecipeID=" & Me.m_intRecipeID.ToString)
    End Sub

    Protected Sub SearchRecipe_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/Search" & Me.m_intRecipeID.ToString)
    End Sub
End Class

