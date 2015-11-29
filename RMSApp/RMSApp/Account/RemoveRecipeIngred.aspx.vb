' ADDED RSS 9-21-2015
Imports System.Data
Imports System.Data.SqlClient
' END ADD

Public Class RemoveRecipeIngred
    Inherits Page

    Public strIngredID As String
    Public strLoggedInUser As String
    Public strAuthenticated As String
    Public strRecipeName As String
    Public strRecipeOwnerName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            strIngredID = Me.ExtractStepID(Request.QueryString("IngredID"))
            strLoggedInUser = Session("RMS_LoggedInUser")
            Me.hdnIngredID.Value = strIngredID
            Me.hdnLoggedInUser.Value = strLoggedInUser

            ' First Time Through
            ' We need to get the Recipe Name and its owners name
            'Me.Populate_Uom_DropDown(ddUOM)
            Me.FillInIngredInfo(strIngredID)

        Else
            ' Coming back through in PostBack
            strIngredID = Me.hdnIngredID.Value
            strLoggedInUser = Me.hdnLoggedInUser.Value

        End If
    End Sub

    Private Sub FillInIngredInfo(ByVal strIngredID As String)
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
        objParams(0) = objData_DB.MakeInParam("@IngredientID", SqlDbType.Int, 4, CInt(strIngredID))

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_RecipeIngredient_Select_ByIngredID", objParams)

        If objDR.HasRows Then
            objDR.Read()

            Me.txtIngredName.Text = objDR("IngredientName").ToString
            Me.txtQuantity.Text = objDR("Qty").ToString
            Me.ddUOM.Text = objDR("UOMType").ToString
            Me.txtPrepText.Text = objDR("PreparationText").ToString
            Me.hdnRecipeID.Value = objDR("RecipeID").ToString
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

    Protected Sub Remove_Click(sender As Object, e As EventArgs)
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
        objParams(0) = objData_DB.MakeInParam("@IngredientID", SqlDbType.Int, 4, CInt(Me.strIngredID))


        objDR = objData_DB.RunStoredProc("usp_RecipeIngredient_Delete_ByIngredID", objParams)

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

        Response.Redirect("~/Account/ManageMyRecipe?RecipeID=btnView_" & Me.hdnRecipeID.Value)

    End Sub

    Protected Sub CancelChanges_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/Account/ManageMyRecipe?RecipeID=btnView_" & Me.hdnRecipeID.Value)
    End Sub


    Private Function ExtractStepID(ByVal strStepID As String) As String
        Dim strReturnID As Int16
        Dim strTokens() As String

        strTokens = Split(strStepID, "_")

        Try
            strReturnID = strTokens(1)
        Catch ex As Exception
            strReturnID = -1
        End Try

        Return strReturnID
    End Function
End Class