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

Partial Public Class AddRecipe
    Inherits Page

    Public strLoggedInUser As String
    Public strAuthenticated As String
    Public strFunction As String

    Public strFirstName As String
    Public strLastName As String
    Friend WithEvents EventLog1 As EventLog
    Public strMessage As String
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

    Protected Sub SaveChanges_Click(sender As Object, e As EventArgs)
        Dim strErr As String = ""

        ' ErrorMessage.Text = Me.UpdateUserAccount()
        If ErrorMessage.Text = "" Then
            '    ErrorMessage.Text = Me.UpdateSecurityQuestion()
        End If

        If ErrorMessage.Text = "" Then
            ErrorMessage.Text = "Record Successfully Updated"
        End If

    End Sub

    Protected Sub ResetPassword_Click(sender As Object, e As EventArgs)
        Dim strEncryptString As String

        Session("RMS_LoggedInUser") = strLoggedInUser
        Session("RMS_Authenticated") = strAuthenticated
        Session("RMS_Function") = "PASSWORD_RESET"
        Session("RMS_ReturnPage") = "~/Account/ManageProfile"

        Dim objDes_Codec As DES_Codec = New DES_Codec()
        strEncryptString = objDes_Codec.EncodeString(strLoggedInUser)
        objDes_Codec = Nothing

        Response.Redirect("~/Account/ManagePassword.aspx?et=" & strEncryptString)
    End Sub



    Private Function SaveProfileUpdate(ByVal strCon As String, ByVal strEmail As String,
                                  ByVal strFN As String, ByVal strLN As String,
                                  ByVal strPhone As String, ByVal strAddr As String,
                                  ByVal strCity As String, ByVal strState As String,
                                  ByVal strZip As String, ByVal strCountry As String) As String
        Dim objData_DB As clsData_DB
        Dim objParams(9) As SqlParameter
        Dim objDR As SqlDataReader

        Dim strAccountID As String = ""

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strCon)

        objParams(0) = objData_DB.MakeInParam("@Email", SqlDbType.VarChar, 50, strEmail)
        objParams(1) = objData_DB.MakeInParam("@FirstName", SqlDbType.VarChar, 50, strFN)
        objParams(2) = objData_DB.MakeInParam("@LastName", SqlDbType.VarChar, 50, strLN)
        objParams(3) = objData_DB.MakeInParam("@PhoneNumber", SqlDbType.VarChar, 12, strPhone)
        objParams(4) = objData_DB.MakeInParam("@StreetAddressLine1", SqlDbType.VarChar, 60, strAddr)
        objParams(5) = objData_DB.MakeInParam("@StreetAddressLine2", SqlDbType.VarChar, 60, "Not Used")
        objParams(6) = objData_DB.MakeInParam("@City ", SqlDbType.VarChar, 50, strCity)
        objParams(7) = objData_DB.MakeInParam("@StateProvName ", SqlDbType.VarChar, 50, strState)
        objParams(8) = objData_DB.MakeInParam("@PostalCode ", SqlDbType.NChar, 10, strZip)
        objParams(9) = objData_DB.MakeInParam("@CountryName ", SqlDbType.VarChar, 50, strCountry)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_UserAccount_Insert_AltOutput", objParams)

        If objDR.HasRows Then
            objDR.Read()
            strAccountID = objDR("UserAccountID").ToString
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

        Return strAccountID
    End Function
    Private Function ValidateInput() As String
        Dim strRet As String = "NOERROR"

        ' If Me.Email.Text = "" Then
        'strRet = "Email Address is Required"
        'End If

        ' Return strRet

    End Function

    Private Sub CreateIngredientsControls()
        Me.IngredientsNumberOfControls = Session("RMS_IngredientsNumberOfControls")

        For iCounter As Integer = 1 To IngredientsNumberOfControls()

            Dim ingredientCtrl1 As TextBox = New TextBox()
            Dim ingredientCtrl2 As TextBox = New TextBox()
            Dim ingredientLabel1 As Label = New Label()
            Dim ingredientLabel2 As Label = New Label()
            Dim ingredientLabel3 As Label = New Label()
            Dim ingredientDropDown1 As DropDownList = New DropDownList()
            Dim ingredientDropDown2 As DropDownList = New DropDownList()
            Dim ingredientDropDown3 As DropDownList = New DropDownList()

            ingredientLabel1.ID = "Label1_" & iCounter.ToString
            ingredientLabel1.Text = "Name " & iCounter.ToString
            ingredientCtrl1.ID = "TextBox1_" & iCounter.ToString
            ingredientLabel2.ID = "Label2_" & iCounter.ToString
            ingredientLabel2.Text = " Quantity "
            ingredientDropDown1.ID = "DropDown1_" & iCounter.ToString
            ingredientDropDown2.ID = "DropDown2_" & iCounter.ToString
            ingredientLabel3.ID = "Label3_" & iCounter.ToString
            ingredientLabel3.Text = " UOM "
            ingredientDropDown3.ID = "DropDown3_" & iCounter.ToString
            ingredientCtrl2.ID = "TextBox2_" & iCounter.ToString

            Me.pnlIngredients.Controls.Add(ingredientLabel1)
            Me.pnlIngredients.Controls.Add(ingredientCtrl1)
            Me.pnlIngredients.Controls.Add(ingredientLabel2)
            Me.pnlIngredients.Controls.Add(ingredientDropDown1)
            Me.pnlIngredients.Controls.Add(ingredientDropDown2)
            Me.pnlIngredients.Controls.Add(ingredientLabel3)
            Me.pnlIngredients.Controls.Add(ingredientDropDown3)
            Me.pnlIngredients.Controls.Add(ingredientCtrl2)
            Me.pnlIngredients.Controls.Add(New LiteralControl("<br />"))

        Next
    End Sub

    ''' <summary>
    ''' Register_Load - Page Load Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ManageProfile_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' On Page Load of the Registration Page, we want to populate the SecurityQuestion dropdown listbox.
        ' Only perform is it is new.  If Postback, we should already be good to go.
        strLoggedInUser = Session("RMS_LoggedInUser")
        strAuthenticated = Session("RMS_Authenticated")
        strFunction = Session("RMS_Function")
        Me.NumberOfControls = Convert.ToInt16(Session("RMS_NumberOfControls"))
        Me.NumberOfControls = Convert.ToInt16(Session("RMS_IngredientsNumberOfControls"))

        If Not IsPostBack Then
            Me.Populate_ServingSize_DropDown()
            Me.populate_RecipeCategory_DropDown()
            NumberOfControls = 0
            Session("RMS_NumberOfControls") = Me.NumberOfControls.ToString
            Session("RMS_IngredientsNumberOfControls") = Me.NumberOfControls.ToString
        Else
            Me.CreateIngredientsControls()
            Me.CreateControls()
        End If
    End Sub
    Protected Sub AddIngredients(sender As Object, e As EventArgs)

        Dim ingredientCtrl1 As TextBox = New TextBox()
        Dim ingredientCtrl2 As TextBox = New TextBox()
        Dim ingredientLabel1 As Label = New Label()
        Dim ingredientLabel2 As Label = New Label()
        Dim ingredientLabel3 As Label = New Label()
        Dim ingredientDropDown1 As DropDownList = New DropDownList()
        Dim ingredientDropDown2 As DropDownList = New DropDownList()
        Dim ingredientDropDown3 As DropDownList = New DropDownList()

        Me.IngredientsNumberOfControls += 1
        Session("RMS_IngredientsNumberOfControls") = Me.IngredientsNumberOfControls.ToString

        ingredientLabel1.ID = "Label1_" & IngredientsNumberOfControls.ToString
        ingredientLabel1.Text = "Name " & IngredientsNumberOfControls.ToString
        ingredientCtrl1.ID = "TextBox1_" & IngredientsNumberOfControls.ToString
        ingredientLabel2.ID = "Label2_" & IngredientsNumberOfControls.ToString
        ingredientLabel2.Text = " Quantity "
        ingredientDropDown1.ID = "DropDown1_" & IngredientsNumberOfControls.ToString
        ingredientDropDown2.ID = "DropDown2_" & IngredientsNumberOfControls.ToString
        ingredientLabel3.ID = "Label3_" & IngredientsNumberOfControls.ToString
        ingredientLabel3.Text = " UOM "
        ingredientDropDown3.ID = "DropDown3_" & IngredientsNumberOfControls.ToString
        ingredientCtrl2.ID = "TextBox2_" & IngredientsNumberOfControls.ToString

        Me.pnlIngredients.Controls.Add(ingredientLabel1)
        Me.pnlIngredients.Controls.Add(ingredientCtrl1)
        Me.pnlIngredients.Controls.Add(ingredientLabel2)
        Me.pnlIngredients.Controls.Add(ingredientDropDown1)
        Me.pnlIngredients.Controls.Add(ingredientDropDown2)
        Me.pnlIngredients.Controls.Add(ingredientLabel3)
        Me.pnlIngredients.Controls.Add(ingredientDropDown3)
        Me.pnlIngredients.Controls.Add(ingredientCtrl2)
        Me.pnlIngredients.Controls.Add(New LiteralControl("<br />"))

    End Sub

    Private Sub CreateControls()
        Me.NumberOfControls = Session("RMS_NumberOfControls")

        For iCounter As Integer = 1 To NumberOfControls()
            Dim objCtrl As TextBox = New TextBox()
            Dim objLabel As Label = New Label()

            objCtrl.ID = "Control_" & iCounter.ToString
            objCtrl.TextMode = TextBoxMode.MultiLine
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
        objLabel.ID = "Label_" & NumberOfControls.ToString
        objLabel.Text = "Step " & NumberOfControls.ToString & ":  "

        Me.pnlDirections.Controls.Add(objLabel)
        Me.pnlDirections.Controls.Add(objCtrl)
        Me.pnlDirections.Controls.Add(New LiteralControl("<br />"))

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
        Me.RecipeServingSize.Items.Clear()

        ' Iterate Through the DataReader and Populate the Listbox
        Me.RecipeServingSize.Items.Add(New ListItem(""))
        For iServingSize = 1 To 20
            Me.RecipeServingSize.Items.Add(New ListItem(iServingSize.ToString))
        Next
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

End Class

