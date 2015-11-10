Imports System.Web
Imports System.Web.UI
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin

' ADDED RSS 9-27-2015
Imports System.Data
Imports System.Data.SqlClient

Partial Public Class Search
    Inherits Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            populate_RecipeCategory_DropDown()
        Else
            RecipeResults()
        End If
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
            Me.ddCategoryList.Items.Clear()

            ' Iterate Through the DataReader and Populate the Listbox
            Me.ddCategoryList.Items.Add(New ListItem(""))
            While objDR.Read()
                Me.ddCategoryList.Items.Add(New ListItem(objDR("CuisineCategory").ToString))
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

    Protected Sub SearchRecipe(Sender As Object, e As EventArgs)
        'RecipeResults()
    End Sub

    Protected Sub RecipeResults()
        Dim strSQL_command As String
        Dim strSQL_where As String = " WHERE "
        Dim strArray() As String
        Dim intCount As Integer
        Dim strInClause As String = ""
        Dim bAnd As Boolean = False
        Dim objData_DB As clsData_DB
        Dim objDR As SqlDataReader
        Dim objWebConfig As New clsWebConfig()
        Dim strConnectionString As String = objWebConfig.GetWebConfig("Connection".ToString)
        Dim intCounter As Integer = 0


        strSQL_command = "SELECT DISTINCT r.RecipeID, " &
                            "LEFT(r.RecipeName,20) As RecipeName, " &
                             "LEFT(r.RecipeDescription,20) As RecipeDescription, " &
                             "u.FirstName + ' ' + u.LastName AS [OwnerName] " &
                        "FROM dbo.Recipe r " &
                        "RIGHT JOIN dbo.RecipeIngredientList il ON il.RecipeID = r.RecipeID " &
                        "JOIN dbo.UserAccount u ON u.LoginID = r.OwnerID "
        '"JOIN dbo.RecipeCuisineCategoryMap rc ON rc.RecipeID = r.RecipeID " &
        '"JOIN dbo.CuisineCatgList cc ON cc.CuisineCategoryID = rc.CuisineCategoryID"


        If ddCategoryList.Text <> "" Then
            strSQL_where &= "r.CuisineCategory = '" & ddCategoryList.Text & "'"
            bAnd = True
        End If

        If txtSearchBox.Text <> "" Then

            If bAnd = True Then
                strSQL_where &= " AND "
            End If

            strInClause = "("
            strArray = Split(txtSearchBox.Text, " ")
            For intCount = LBound(strArray) To UBound(strArray)

                strInClause &= "(" &
                    "r.SearchTerm LIKE '%" & strArray(intCount) & "%' Or " &
                     "r.RecipeName LIKE '%" & strArray(intCount) & "%' Or " &
                     "il.IngredientName LIKE '%" & strArray(intCount) & "%' Or " &
                     "r.RecipeDescription LIKE '%" & strArray(intCount) &
                    "%')"

                If intCount < UBound(strArray) Then
                    strInClause &= ") OR ("
                End If
            Next
            strInClause &= ")"


        End If

        If txtSearchBox.Text = "" And ddCategoryList.Text = "" Then

        Else
            strSQL_command &= strSQL_where & strInClause
        End If

        'strSQL_command &= strSQL_where & strInClause

        'Response.Write(strSQL_command)
        'Response.End()

        objData_DB = New clsData_DB(strConnectionString)
        objDR = objData_DB.RunCommand(strSQL_command)

        If objDR.HasRows Then
            addSearchHeaderItem()
            While objDR.Read()
                addSearchResponseItem(objDR("RecipeID").ToString, objDR("RecipeName").ToString, objDR("RecipeDescription").ToString, objDR("OwnerName").ToString, intCounter)
                intCounter += 1
            End While
        Else
            Me.pnlSearch.Controls.Add(New LiteralControl("<font color=""red"">No Results Found...</font>"))
        End If



    End Sub

    Protected Sub addSearchResponseItem(ByVal strRecipeID As String, ByVal strRecipeName As String, ByVal strRecipeDesc As String, ByVal strOwnerName As String, ByVal intCheck As Integer)
        Dim strRecipeResult As String
        Dim btnSearchCtrl As Button = New Button()
        Dim strBackColor As String

        btnSearchCtrl.ID = "btnView_" & strRecipeID
        btnSearchCtrl.Text = "View Recipe"
        btnSearchCtrl.Attributes.Add("Class", "btn btn-primary btn-sm")
        'btnSearchCtrl.Attributes.Add("OnClick", "btnDisplay_Click")
        AddHandler btnSearchCtrl.Click, AddressOf btnDisplay_Click

        strRecipeResult = padSpaces(strRecipeID, 10) & "&nbsp;" & padSpaces(strRecipeName, 20) & "&nbsp;" & padSpaces(strRecipeDesc, 20) & "&nbsp;" & padSpaces(strOwnerName, 10)

        If intCheck Mod 2 = 0 Then
            strBackColor = "#DCDCDC"
        Else
            strBackColor = "#A9A9A9"
        End If

        Me.pnlSearch.Controls.Add(New LiteralControl("<p style=""background-color:" & strBackColor & """>"))
        Me.pnlSearch.Controls.Add(New LiteralControl("<font face=""Courier"">" & strRecipeResult & "</font>"))
        Me.pnlSearch.Controls.Add(btnSearchCtrl)
        Me.pnlSearch.Controls.Add(New LiteralControl("</p>"))

    End Sub

    Protected Sub addSearchHeaderItem()
        Dim strRecipeHeader As String

        strRecipeHeader = padSpaces("RECIPE ID", 10) & "&nbsp;" & padSpaces("RECIPE NAME", 20) & "&nbsp;" & padSpaces("RECIPE DESCRIPTION", 20) & "&nbsp;" & padSpaces("CREATOR", 10) & "<br />"

        Me.pnlSearch.Controls.Add(New LiteralControl("<b><font face=""Courier"">" & strRecipeHeader & "</font></b>"))
        Me.pnlSearch.Controls.Add(New LiteralControl("<hr />"))
    End Sub

    Protected Function padSpaces(ByVal strResult As String, ByVal intSize As Integer)
        Dim strReturn As String

        strReturn = strResult
        For i As Integer = 1 To intSize - strResult.Length()
            strReturn &= "&nbsp;"
        Next

        Return strReturn

    End Function

    Private Sub btnDisplay_Click(sender As Object, e As EventArgs)
        Dim objBtnCtrl As System.Web.UI.WebControls.Button = sender

        Response.Redirect("~/Account/DisplayRecipe?RecipeID=" & objBtnCtrl.ID.ToString)

    End Sub

End Class
