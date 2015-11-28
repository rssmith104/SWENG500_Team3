Imports System
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Owin
Imports System.IO

' ADDED RSS 9-21-2015
Imports System.Data
Imports System.Data.SqlClient
' END ADD

Partial Public Class ManageMyRecipe
    Inherits Page

    Public strLoggedInUser As String
    Public strAuthenticated As String
    Public strFunction As String

    Public strFirstName As String
    Public strLastName As String
    Friend WithEvents EventLog1 As EventLog
    Public strMessage As String
    Public strRecipeImage As String
    Public arrayUomList As Array
    Public strRecipeId As String

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

    Private m_strRecipeID As String
    Private m_intRecipeID As Int16

    ''' <summary>
    ''' InsertRecipe 
    ''' </summary>
    ''' <returns></returns>
    Private Function InsertRecipe() As String
        Dim strErr As String
        Dim strRecipeTitle As String = Me.txtRecipeTitle.Text
        Dim strRecipeDescription As String = Me.txtRecipeDescription.Text
        Dim strCookingTime As String = Me.txtRecipeCookingTime.Text
        Dim intServingSize As Int16 = Me.ddRecipeServingSize.SelectedValue.ToString
        Dim strCategory As String = Me.ddRecipeCategory.Text
        Dim strShareRecipe As String = Me.ddRecipeSharing.Text
        Dim strSearchKeyWord As String = Me.txtRecipeSearch.Text
        Dim strMeasurementSystem As String = Me.rbRecipeMeasurement.Text
        Dim strLogInUser As String = Me.strLoggedInUser

        Dim objData_DB As clsData_DB
        Dim objParams(8) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader

        Dim strDebugMsg As String = ""

        strErr = Me.ValidateInput()
        If strErr = "NOERROR" Then
            Try

                ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
                Dim objWebConfig As New clsWebConfig()
                strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

                ' Use the Connection string to instantiate a new Database class object.
                objData_DB = New clsData_DB(strConnectionString)

                objParams(0) = objData_DB.MakeInParam("@RecipeName", SqlDbType.VarChar, 150, strRecipeTitle)
                objParams(1) = objData_DB.MakeInParam("@RecipeDescription", SqlDbType.VarChar, 150, strRecipeDescription)
                objParams(2) = objData_DB.MakeInParam("@ServingSize", SqlDbType.SmallInt, 2, intServingSize)
                objParams(3) = objData_DB.MakeInParam("@ShareLevelType", SqlDbType.VarChar, 30, strShareRecipe)
                objParams(4) = objData_DB.MakeInParam("@SearchTerm", SqlDbType.VarChar, 500, strSearchKeyWord)
                objParams(5) = objData_DB.MakeInParam("@CookingTime", SqlDbType.VarChar, 50, strCookingTime)
                objParams(6) = objData_DB.MakeInParam("@MeasurementSystem", SqlDbType.VarChar, 30, strMeasurementSystem)
                objParams(7) = objData_DB.MakeInParam("@Email", SqlDbType.VarChar, 50, strLogInUser)
                objParams(8) = objData_DB.MakeInParam("@CuisineCategory", SqlDbType.VarChar, 50, strCategory)


                ' Run the stored procedure by name.  Pass with it the parameter list.
                objDR = objData_DB.RunStoredProc("usp_Recipe_Insert_Full", objParams)

                If objDR.HasRows Then
                    objDR.Read()

                    If objDR("Status").ToString = "FAIL" Then
                        strErr = "ERROR"
                    Else
                        strErr = ""
                        strRecipeId = objDR("RecipeID").ToString
                    End If
                End If
            Catch ex As Exception
                Response.Write(ex.Message)
                Response.End()
            End Try
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

        Return strErr
    End Function

    Private Function InsertIngredients(ByVal strIngredientName As String, ByVal strIngredientQuantity As String, ByVal strIngredientUom As String) As String

        Dim strErr As String = ""
        Dim objData_DB As clsData_DB
        Dim objParams(3) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        objParams(0) = objData_DB.MakeInParam("@RecipeStr", SqlDbType.VarChar, 6, strRecipeId)
        objParams(1) = objData_DB.MakeInParam("@IngredientName", SqlDbType.VarChar, 100, strIngredientName)
        objParams(2) = objData_DB.MakeInParam("@UnitOfMeasureType", SqlDbType.VarChar, 50, strIngredientUom)
        objParams(3) = objData_DB.MakeInParam("@Qty", SqlDbType.VarChar, 10, strIngredientQuantity)


        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_RecipeIngredientList_Insert", objParams)

        'Response.Write("RecipeId " + objParams(0).Value + "Name " + objParams(1).Value + "UOM " + objParams(2).Value + "Qty " + objParams(3).Value)
        'Response.End()

        If objDR.HasRows Then
            objDR.Read()

            If objDR("Status").ToString = "FAIL" Then
                strErr = "ERROR"
            Else
                strErr = ""
            End If
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
        Return strErr
    End Function

    Private Function GetIngredients() As String

        Dim strIngredientName As String = ""
        Dim strIngredientQuantity As String = ""
        Dim strIngredientUom As String = ""
        Dim strErr As String = ""

        For iCount As Integer = 1 To IngredientsNumberOfControls()
            strIngredientName = CType(pnlIngredients.FindControl("TextBox1_" & iCount.ToString), TextBox).Text
            strIngredientUom = CType(pnlIngredients.FindControl("DropDown3_" & iCount.ToString), DropDownList).SelectedValue.ToString
            strIngredientQuantity = CType(pnlIngredients.FindControl("TextBox3_" & iCount.ToString), TextBox).Text

            Me.InsertIngredients(strIngredientName, strIngredientQuantity, strIngredientUom)

        Next
        Dim strDebugMsg As String = ""

        strErr = Me.ValidateInput()
        If strErr = "NOERROR" Then
        End If
        Return strErr
    End Function

    Private Function InsertDirections(ByVal strDirectionsText As String, ByVal i As Integer) As String

        Dim objData_DB As clsData_DB
        Dim objParams(2) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader
        Dim strErr As String = ""

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        objParams(0) = objData_DB.MakeInParam("@RecipeStr", SqlDbType.VarChar, 6, strRecipeId)
        objParams(1) = objData_DB.MakeInParam("@StepNumber", SqlDbType.Int, 2, i)
        objParams(2) = objData_DB.MakeInParam("@StepText", SqlDbType.VarChar, 200, strDirectionsText)


        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_InstructionStep_Insert", objParams)

        If objDR.HasRows Then
            objDR.Read()

            If objDR("Status").ToString = "FAIL" Then
                strErr = "ERROR"
            Else
                strErr = ""
            End If
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

    End Function

    Private Function GetDirections() As String

        Dim strErr As String = ""
        Dim strDirectionsText As String = ""
        For i As Integer = 1 To NumberOfControls()
            strDirectionsText = CType(pnlDirections.FindControl("Control_" & i.ToString), TextBox).Text
            Me.InsertDirections(strDirectionsText, i)
        Next
        Dim strDebugMsg As String = ""
        strErr = Me.ValidateInput()
        If strErr = "NOERROR" Then
        End If
        Return strErr
    End Function

    Private Function ValidateInput() As String
        Dim strRet As String = "NOERROR"

        ' If Me.Email.Text = "" Then
        'strRet = "Email Address is Required"
        'End If

        Return strRet

    End Function

    Private Sub CreateIngredientsControls()
        Me.IngredientsNumberOfControls = Session("RMS_IngredientsNumberOfControls")

        For iCounter As Integer = 1 To IngredientsNumberOfControls()

            Dim ingredientCtrl1 As TextBox = New TextBox()
            Dim ingredientCtrl3 As TextBox = New TextBox()
            Dim ingredientLabel1 As Label = New Label()
            Dim ingredientLabel2 As Label = New Label()
            Dim ingredientLabel3 As Label = New Label()
            Dim ingredientDropDown3 As DropDownList = New DropDownList()

            ingredientLabel1.ID = "Label1_" & iCounter.ToString
            ingredientLabel1.Text = "Name " & iCounter.ToString
            ingredientCtrl1.ID = "TextBox1_" & iCounter.ToString
            ingredientLabel2.ID = "Label2_" & iCounter.ToString
            ingredientLabel2.Text = " Quantity "
            ingredientCtrl3.ID = "TextBox3_" & iCounter.ToString
            ingredientLabel3.ID = "Label3_" & iCounter.ToString
            ingredientLabel3.Text = " UOM "
            ingredientDropDown3.ID = "DropDown3_" & iCounter.ToString

            Me.pnlIngredients.Controls.Add(ingredientLabel1)
            Me.pnlIngredients.Controls.Add(ingredientCtrl1)
            Me.pnlIngredients.Controls.Add(ingredientLabel2)
            Me.pnlIngredients.Controls.Add(ingredientCtrl3)
            Me.pnlIngredients.Controls.Add(ingredientLabel3)
            Me.pnlIngredients.Controls.Add(ingredientDropDown3)
            Me.pnlIngredients.Controls.Add(New LiteralControl("<br />"))
            Me.Populate_Uom_DropDown(ingredientDropDown3)

        Next
    End Sub

    Protected Sub AddIngredients(sender As Object, e As EventArgs)

        Dim ingredientCtrl1 As TextBox = New TextBox()
        Dim ingredientCtrl3 As TextBox = New TextBox()
        Dim ingredientLabel1 As Label = New Label()
        Dim ingredientLabel2 As Label = New Label()
        Dim ingredientLabel3 As Label = New Label()
        Dim ingredientDropDown3 As DropDownList = New DropDownList()

        Me.IngredientsNumberOfControls += 1
        Session("RMS_IngredientsNumberOfControls") = Me.IngredientsNumberOfControls.ToString

        ingredientLabel1.ID = "Label1_" & IngredientsNumberOfControls.ToString
        ingredientLabel1.Text = "Name " & IngredientsNumberOfControls.ToString
        ingredientCtrl1.ID = "TextBox1_" & IngredientsNumberOfControls.ToString
        ingredientLabel2.ID = "Label2_" & IngredientsNumberOfControls.ToString
        ingredientLabel2.Text = " Quantity "
        ingredientCtrl3.ID = "TextBox3_" & IngredientsNumberOfControls.ToString
        ingredientLabel3.ID = "Label3_" & IngredientsNumberOfControls.ToString
        ingredientLabel3.Text = " UOM "
        ingredientDropDown3.ID = "DropDown3_" & IngredientsNumberOfControls.ToString
        Me.pnlIngredients.Controls.Add(ingredientLabel1)
        Me.pnlIngredients.Controls.Add(ingredientCtrl1)
        Me.pnlIngredients.Controls.Add(ingredientLabel2)
        Me.pnlIngredients.Controls.Add(ingredientCtrl3)
        Me.pnlIngredients.Controls.Add(ingredientLabel3)
        Me.pnlIngredients.Controls.Add(ingredientDropDown3)
        Me.pnlIngredients.Controls.Add(New LiteralControl("<br />"))
        Me.Populate_Uom_DropDown(ingredientDropDown3)

        Me.SetFocus(ingredientCtrl1)

    End Sub

    Private Sub CreateControls()
        Me.NumberOfControls = Session("RMS_NumberOfControls")

        For iCounter As Integer = 1 To NumberOfControls()
            Dim objCtrl As TextBox = New TextBox()
            Dim objLabel As Label = New Label()

            objCtrl.ID = "Control_" & iCounter.ToString
            objCtrl.TextMode = TextBoxMode.MultiLine
            objCtrl.Attributes.Add("style", "width:800px")
            objLabel.ID = "Label_" & iCounter.ToString
            objLabel.Text = "Step " & iCounter.ToString & ":  "

            Me.pnlDirections.Controls.Add(objLabel)
            Me.pnlDirections.Controls.Add(objCtrl)
            Me.pnlDirections.Controls.Add(New LiteralControl("<br />"))

        Next
    End Sub

    Protected Sub AddAStep(sender As Object, e As EventArgs)

        Dim objCtrl As TextBox = New TextBox()
        Dim objLabel As Label = New Label()


        Me.NumberOfControls += 1
        Session("RMS_NumberOfControls") = Me.NumberOfControls.ToString

        objCtrl.ID = "Control_" & NumberOfControls.ToString
        objCtrl.TextMode = TextBoxMode.MultiLine
        objCtrl.Attributes.Add("style", "width:800px")
        objLabel.ID = "Label_" & NumberOfControls.ToString
        objLabel.Text = "Step " & NumberOfControls.ToString & ":  "

        Me.pnlDirections.Controls.Add(objLabel)
        Me.pnlDirections.Controls.Add(objCtrl)
        Me.pnlDirections.Controls.Add(New LiteralControl("<br />"))
        'Me.SetFocus(btnAddStep)
        'Me.SetFocus(pnlDirections)
        Me.SetFocus(objCtrl)

    End Sub

    Private Sub populate_RecipeCategory_DropDown()
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@TranCode", SqlDbType.VarChar, 10, "ALL")

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_CuisineCategoryList_Select", objParams)

        ' Do we have rows returned?
        If objDR.HasRows Then
            ' Clear the List before we populate
            Me.ddRecipeCategory.Items.Clear()

            ' Iterate Through the DataReader and Populate the Listbox
            Me.ddRecipeCategory.Items.Add(New ListItem(""))
            While objDR.Read()
                Me.ddRecipeCategory.Items.Add(New ListItem(objDR("CuisineCategory").ToString))
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

    Private Sub Populate_ServingSize_DropDown()
        Dim iServingSize As Integer

        ' Clear the List before we populate
        Me.ddRecipeServingSize.Items.Clear()

        ' Iterate Through the DataReader and Populate the Listbox
        Me.ddRecipeServingSize.Items.Add(New ListItem(""))
        For iServingSize = 1 To 20
            Me.ddRecipeServingSize.Items.Add(New ListItem(iServingSize.ToString))
        Next
    End Sub

    Protected Sub SaveRecipeChanges_Click(sender As Object, e As EventArgs)
        Dim strErr As String = ""

        ErrorMessage.Text = Me.InsertRecipe()
        If ErrorMessage.Text = "" Then
            Me.InsertRecipeImage()
            Me.GetIngredients()
            Me.GetDirections()
            ErrorMessage.Text = "Record Successfully Updated"

            Response.Redirect("~/Account/AddRecipe?err=Success")
        End If

    End Sub

    Protected Sub CancelRecipe_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/Authenticated_Default")
    End Sub

    Private Sub Populate_Sharing_DropDown()
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@TranCode", SqlDbType.VarChar, 10, "ALL")

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_ShareLevelList_Select", objParams)

        ' Do we have rows returned?
        If objDR.HasRows Then
            ' Clear the List before we populate
            Me.ddRecipeSharing.Items.Clear()

            ' Iterate Through the DataReader and Populate the Listbox
            Me.ddRecipeSharing.Items.Add(New ListItem(""))
            While objDR.Read()
                Me.ddRecipeSharing.Items.Add(New ListItem(objDR("ShareLevelType").ToString))
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

    Private Sub Populate_Uom_DropDown(ByVal ddCtrl As DropDownList)
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@TranCode", SqlDbType.VarChar, 10, "ALL")

        ' Run the stored procedure by name.  Pass with it the parameter list.
        If Me.rbRecipeMeasurement.SelectedIndex = 0 Then
            objDR = objData_DB.RunStoredProc("usp_UOM_Select_US", objParams)
        Else
            objDR = objData_DB.RunStoredProc("usp_UOM_Select_NonUs", objParams)
        End If

        ' Do we have rows returned?
        If objDR.HasRows Then
            ' Clear the List before we populate
            Dim arrCounter = 0
            Dim arrayUomList(0)
            ddCtrl.Items.Clear()
            ' Iterate Through the DataReader and Populate the Listbox
            'Me.arrayUomList.Items.Add(New ListItem(""))
            While objDR.Read()
                'Me.arrayUomList.Items.Add(New ListItem(objDR("ShareLevelType").ToString)
                ddCtrl.Items.Add(New ListItem(objDR("UOMType").ToString))
                ReDim arrayUomList(arrCounter)
                arrayUomList(arrCounter) = objDR("UOMType")
                arrCounter = arrCounter + 1
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

    Private Sub InsertRecipeImage()

        Dim strImageURL As String = Me.imRecipeImage.ImageUrl.ToString()

        Dim objData_DB As clsData_DB
        Dim objParams(1) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader
        Dim objByte As Byte = &H20

        If strImageURL <> "" Then
            ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
            Dim objWebConfig As New clsWebConfig()
            strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

            ' Use the Connection string to instantiate a new Database class object.
            objData_DB = New clsData_DB(strConnectionString)

            objParams(0) = objData_DB.MakeInParam("@RecipeId", SqlDbType.VarChar, 4, strRecipeId)
            objParams(1) = objData_DB.MakeInParam("@ImageText", SqlDbType.VarChar, 200, strImageURL)

            ' Run the stored procedure by name.  Pass with it the parameter list.
            objDR = objData_DB.RunStoredProc("usp_RecipeImage_InsertUpdate", objParams)

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

        End If
    End Sub

    Protected Sub Recipe_Image_Upload(sender As Object, e As EventArgs)
        If Me.fuRecipePicUpload.HasFile Then
            Dim strFileName As String = Path.GetFileName(Me.fuRecipePicUpload.PostedFile.FileName)
            fuRecipePicUpload.PostedFile.SaveAs(Server.MapPath("~/Images/") + strFileName)
            Me.imRecipeImage.ImageUrl = "~/images/" & strFileName
            'Me.imRecipeImage.ImageUrl = Server.MapPath("~/Images/") + strFileName
            'Me.imRecipeImage.Attributes.Add("ImageUrl", "Server.MapPath(""~/Images/"") + strFileName")
            Session("RMS_RecipeImage") = "~/Images/" & strFileName
            Me.SetFocus(Me.btnSave)
            'Response.Redirect(Request.Url.AbsoluteUri)

        End If
    End Sub


    Private Sub InitializeComponent()
        Me.EventLog1 = New System.Diagnostics.EventLog()
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'EventLog1
        '
        CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub

    Private Sub EventLog1_EntryWritten(sender As Object, e As EntryWrittenEventArgs) Handles EventLog1.EntryWritten

    End Sub

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
            Me.txtRecipeTitle.Text = objDR("RecipeName").ToString

            ' Bind the Owner Name
            ''Me.ltOwner.Text = objDR("Owner_Email").ToString
            ''Me.ltSubmissionDate.Text = objDR("SubmissionDate").ToString

            '<TODO retrieve rating from database on bind to Page Data
            ' Bind the Rating
            'strRating = GetRating(intRecipeID)
            'strReviewCount = GetReviewCount(intRecipeID)

            If strRating <> "No Rate" Then
                ''Me.ltRating.Text = FormatNumber(CDbl(strRating), 1).ToString & " of 5"
                ''Me.hypReadReviews.Enabled = True
                ''Me.hypReadReviews.Text = strReviewCount & " Reviews Found"
                ''Me.hypReadReviews.NavigateUrl = "~/Account/DisplayReview?RecipeID=" & intRecipeID.ToString
            Else
                ''Me.ltRating.Text = "No Rating"
                ''Me.hypReadReviews.Enabled = False
                ''Me.hypReadReviews.Text = "0 Reveiws Found"
                ''Me.hypReadReviews.NavigateUrl = ""
            End If
            ''Me.imgRating.ImageUrl = Me.RatingImageSelection(strRating)

            'Bind the Recipe Description
            Me.txtRecipeDescription.Text = objDR("RecipeDescription").ToString

            'Cooking Time
            Me.txtRecipeCookingTime.Text = objDR("CookingTime").ToString

            ' Serving Size 
            Me.ddRecipeServingSize.SelectedIndex = ddRecipeServingSize.Items.IndexOf(ddRecipeServingSize.Items.FindByText(objDR("ServingSize").ToString))

            'Cuisine Category
            Me.ddRecipeCategory.SelectedIndex = ddRecipeCategory.Items.IndexOf(ddRecipeCategory.Items.FindByText(objDR("CuisineCategory").ToString))

            ' Share Level
            Me.ddRecipeSharing.SelectedIndex = ddRecipeSharing.Items.IndexOf(ddRecipeSharing.Items.FindByText(objDR("ShareLevelType").ToString))

            ' Search Keywords
            Me.txtRecipeSearch.Text = objDR("SearchTerm").ToString

            ' Measurement System
            If objDR("MeasurementSystem").ToString = "Metric" Then
                Me.rbRecipeMeasurement.Items(1).Selected = True
            Else
                Me.rbRecipeMeasurement.Items(0).Selected = True
            End If

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

    Private Function BindRecipeData(ByVal intRecipeID As Int16) As String
        Dim strRetVal As String = ""

        BindRecipeHeaderData(intRecipeID)
        'BindRecipeImageData(intRecipeID)
        'BindIngredientData(intRecipeID)
        'BindInstructionData(intRecipeID)

        Return strRetVal
    End Function
    Private Function BindRecipeContent(ByVal intRecipeID As Int16) As String
        Dim strRetVal As String = ""

        strRetVal = BindRecipeData(intRecipeID)

        Return strRetVal
    End Function

    ''' <summary>
    ''' Register_Load - Page Load Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' 
    Private Sub ManageMyRecipe_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' On Page Load of the Registration Page, we want to populate the SecurityQuestion dropdown listbox.
        ' Only perform is it is new.  If Postback, we should already be good to go.
        strLoggedInUser = Session("RMS_LoggedInUser")
        strAuthenticated = Session("RMS_Authenticated")
        strRecipeImage = Session("RMS_RecipeImage")
        strFunction = Session("RMS_Function")

        If Not IsPostBack Then
            ' On initial load, we need to first extract the target recipe from the querystring indicator
            m_strRecipeID = Request.QueryString("RecipeID")

            ' We then transalate it into an integer
            m_intRecipeID = ExtractRecipeID(m_strRecipeID)
            Me.hdnRecipeID.Value = m_intRecipeID.ToString

            Me.Populate_ServingSize_DropDown()
            Me.populate_RecipeCategory_DropDown()
            Me.Populate_Sharing_DropDown()
            Me.rbRecipeMeasurement.SelectedIndex = 0
            NumberOfControls = 0
            Session("RMS_NumberOfControls") = Me.NumberOfControls.ToString
            Session("RMS_IngredientsNumberOfControls") = Me.NumberOfControls.ToString

            Dim strErr As String
            strErr = Request.QueryString("err")

            If strErr = "Success" Then
                ErrorMessage.Text = "Record Successfully Updated"
            End If

            BindRecipeContent(m_intRecipeID)

        Else
            Me.CreateIngredientsControls()
            Me.CreateControls()
            If Convert.ToInt16(Session("RMS_IngredientsNumberOfControls")) > 0 Then
                Me.rbRecipeMeasurement.Enabled = False
            End If

        End If
        If strRecipeImage <> "" Then
            Me.imRecipeImage.ImageUrl = strRecipeImage
        End If
    End Sub


End Class

