' ADDED RSS 9-21-2015
Imports System.Data
Imports System.Data.SqlClient
' END ADD

Public Class AddRecipeIngred
    Inherits Page

    Public strRecipeID As String
    Public strLoggedInUser As String
    Public strAuthenticated As String
    Public strRecipeName As String
    Public strRecipeOwnerName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            strRecipeID = Me.ExtractStepID(Request.QueryString("RecipeID"))
            strLoggedInUser = Session("RMS_LoggedInUser")
            Me.hdnRecipeID.Value = strRecipeID
            Me.hdnLoggedInUser.Value = strLoggedInUser

            ' First Time Through
            ' We need to get the Recipe Name and its owners name
            Me.Populate_Uom_DropDown(ddUOM)
            'Me.FillInIngredInfo(strIngredID)

        Else
            ' Coming back through in PostBack
            strRecipeID = Me.hdnRecipeID.Value
            strLoggedInUser = Me.hdnLoggedInUser.Value

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
        objDR = objData_DB.RunStoredProc("usp_UOM_Select_US", objParams)

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


    Protected Sub SaveChanges_Click(sender As Object, e As EventArgs)
        Dim objData_DB As clsData_DB
        Dim strConnectionString As String
        Dim objParams(4) As SqlParameter
        Dim objDR As SqlDataReader

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@RecipeStr ", SqlDbType.VarChar, 6, strRecipeID)
        objParams(1) = objData_DB.MakeInParam("@IngredientName", SqlDbType.VarChar, 100, Me.txtIngredName.Text)
        objParams(2) = objData_DB.MakeInParam("@UnitOfMeasureType", SqlDbType.VarChar, 50, Me.ddUOM.SelectedValue)
        objParams(3) = objData_DB.MakeInParam("@Qty", SqlDbType.VarChar, 10, Me.txtQuantity.Text)
        objParams(4) = objData_DB.MakeInParam("@PrepText", SqlDbType.VarChar, 100, Me.txtPrepText.Text)

        objDR = objData_DB.RunStoredProc("usp_RecipeIngredientList_Insert_Prep", objParams)

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