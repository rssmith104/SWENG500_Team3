' ADDED RSS 9-21-2015
Imports System.Data
Imports System.Data.SqlClient
' END ADD

Public Class ModifyRecipeStep
    Inherits Page

    Public strStepID As String
    Public strLoggedInUser As String
    Public strAuthenticated As String
    Public strRecipeName As String
    Public strRecipeOwnerName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            strStepID = Me.ExtractStepID(Request.QueryString("StepID"))
            strLoggedInUser = Session("RMS_LoggedInUser")
            Me.hdnStepID.Value = strStepID
            Me.hdnLoggedInUser.Value = strLoggedInUser

            ' First Time Through
            ' We need to get the Recipe Name and its owners name
            Me.FillInStepInfo(strStepID)

        Else
            ' Coming back through in PostBack
            strStepID = Me.hdnStepID.Value
            strLoggedInUser = Me.hdnLoggedInUser.Value

        End If
    End Sub

    Private Sub FillInStepInfo(ByVal strStepID As String)
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
        objParams(0) = objData_DB.MakeInParam("@InstructionStepID ", SqlDbType.Int, 4, CInt(strStepID))

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_InstructionStep_Select_ByStepID", objParams)

        If objDR.HasRows Then
            objDR.Read()

            Me.txtStepText.Text = objDR("StepText").ToString
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


    Protected Sub SaveChanges_Click(sender As Object, e As EventArgs)
        Dim objData_DB As clsData_DB
        Dim strConnectionString As String
        Dim objParams(1) As SqlParameter
        Dim objDR As SqlDataReader

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@InstructionStepID", SqlDbType.Int, 4, CInt(Me.strStepID))
        objParams(1) = objData_DB.MakeInParam("@StepText", SqlDbType.VarChar, 200, Me.txtStepText.Text)

        objDR = objData_DB.RunStoredProc("usp_InstructionStep_Update_ByStepID", objParams)

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