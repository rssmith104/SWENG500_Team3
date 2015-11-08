' ADDED RSS 9-21-2015
Imports System.Data
Imports System.Data.SqlClient
' END ADD

Public Class SubmitRating
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
            Me.lblRatingSubmitter.Text = strLoggedInUser
            Me.FillInHeaderInfo(strRecipeID)
            Me.PopulateRatingDropDown()
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
    Private Sub PopulateRatingDropDown()
        Dim objData_DB As clsData_DB
        Dim strConnectionString As String
        Dim strSQLCmd As String
        Dim objDR As SqlDataReader

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)
        strSQLCmd = "SELECT CONVERT(varchar(4),Rating) As Rating FROM Rating ORDER BY Rating ASC"

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        objDR = objData_DB.RunCommand(strSQLCmd)
        ' Do we have rows returned?
        If objDR.HasRows Then
            ' Clear the List before we populate
            Me.ddRatings.Items.Clear()
            ' Iterate Through the DataReader and Populate the Listbox
            Me.ddRatings.Items.Add(New ListItem(""))

            While objDR.Read()
                Me.ddRatings.Items.Add(New ListItem(objDR("Rating").ToString))
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

    Protected Sub SaveChanges_Click(sender As Object, e As EventArgs)
        Dim objData_DB As clsData_DB
        Dim strConnectionString As String
        Dim objParams(3) As SqlParameter
        Dim objDR As SqlDataReader

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@Comments ", SqlDbType.VarChar, 1000, Me.txtRatingComment.Text)
        objParams(1) = objData_DB.MakeInParam("@RatingValue ", SqlDbType.Real, 4, Me.ddRatings.SelectedValue)
        objParams(2) = objData_DB.MakeInParam("@RecipeID ", SqlDbType.Int, 4, CInt(strRecipeID))
        objParams(3) = objData_DB.MakeInParam("@Email", SqlDbType.VarChar, 50, strLoggedInUser)

        objDR = objData_DB.RunStoredProc("usp_Review_Insert", objParams)

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

End Class