' ADDED RSS 9-21-2015
Imports System.Data
Imports System.Data.SqlClient
' END ADD

Public Class DisplayReview
    Inherits Page

    Public strRecipeID As String
    Public strLoggedInUser As String
    Public strAuthenticated As String
    Public strRecipeName As String
    Public strRecipeOwnerName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            strRecipeID = Request.QueryString("RecipeID")
            strLoggedInUser = Session("RMS_LoggedInUser")
            Me.hdnRecipeID.Value = strRecipeID
            Me.hdnLoggedInUser.Value = strLoggedInUser

            ' First Time Through
            ' We need to get the Recipe Name and its owners name
            Me.FillInHeaderInfo(strRecipeID)
        Else
            ' Coming back through in PostBack
            strRecipeID = Me.hdnRecipeID.Value
            strLoggedInUser = Me.hdnLoggedInUser.Value

        End If
    End Sub

    Private Sub FillInHeaderInfo(ByVal strRecipeID As String)
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
        objParams(0) = objData_DB.MakeInParam("@RecipeID ", SqlDbType.Int, 4, CInt(strRecipeID))

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_Recipe_Select_byRecipeID", objParams)

        If objDR.HasRows Then
            objDR.Read()

            Me.lblRecipeOwner.Text = objDR("Owner_Email").ToString
            Me.lblRecipeName.Text = objDR("RecipeName").ToString
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

    Protected Sub OK_Click(sender As Object, e As EventArgs)

    End Sub

End Class