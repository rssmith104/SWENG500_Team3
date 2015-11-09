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
            Me.FillInReviews(strRecipeID)
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

    Private Sub FillInReviews(ByVal strRecipeID As String)
        Dim objData_DB As clsData_DB
        Dim strConnectionString As String
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader
        Dim iCounter As Int16 = 1

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@RecipeID ", SqlDbType.Int, 4, CInt(strRecipeID))

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_Review_Select_byRecipeID", objParams)

        If objDR.HasRows Then
            While objDR.Read()
                Me.AddReviewCtrl(objDR("RatingValue").ToString,
                             objDR("Comments").ToString,
                             objDR("ModifiedDate").ToString,
                             objDR("Submitter").ToString,
                             objDR("Email").ToString, iCounter)
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

    Private Sub AddReviewCtrl(ByVal strRating As String,
                             ByVal strComment As String,
                             ByVal strModifiedDate As String,
                             ByVal strName As String,
                             ByVal strEmail As String,
                             ByVal intCounter As Int16)



        '''''''Dim strReviewCtrl As String = "<asp:Table ID=""Table""" & intCounter.ToString & " runat=""server""  HorizontalAlign=""Center"" Gridlines=""Both"">" & vbCrLf &
        '''''''                              "  <asp:TableRow>" & vbCrLf &
        '''''''                              "    <asp:TableCell HorizontalAlign=""Center""> " & vbCrLf &
        '''''''                              "      <b>Submitter: </b>" & strName & "<br />" & "<b>Email: </b>" & strEmail & "<br />" & "<b>Date: </b>" & strModifiedDate & vbCrLf &
        '''''''                              "    </asp:TableCell>" & vbCrLf &
        '''''''                              "    <asp:TableCell HorizontalAlign=""Center""> " & vbCrLf &
        '''''''                              "      <b>Rating: </b>" & strRating & "<br />" & "<asp:Image ID=""imgRating" & intCounter.ToString & """ runat=""server"" Width=""130px"" Height=""25px"" ImageUrl=""" & strImageURL & """/>" & vbCrLf &
        '''''''                              "    </asp:TableCell>" & vbCrLf &
        '''''''                              "    <asp:TableCell HorizontalAlign=""Center""> " & vbCrLf &
        '''''''                              "      <asp:TextBox runat=""server"" ID=""RecipeReview" & intCounter.ToString & """ CssClass=""form-control"" TextMode=""MultiLine"" ReadOnly=""true"" MaxLength=""1000"" Rows=""3"" />" & vbCrLf &
        '''''''                              "    </asp:TableCell>" & vbCrLf &
        '''''''                              "  </asp:TableRow>" & vbCrLf &
        '''''''                              "</Table>"

        Dim strReviewCtrl As String = "<b>Submitter: </b>" & strName & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & "<b>Email: </b>" & strEmail & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & "<b>Date: </b>" & strModifiedDate
        Dim strReviewCtrl2 As String = "<b>Rating: </b>" & strRating & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
        Dim objImage As Image = New Image()
        Dim objTextBox As TextBox = New TextBox()

        If intCounter <> 1 Then
            Me.pnlReviews.Controls.Add(New LiteralControl("<br /><hr size=""2"" style=""color:black;"" />"))
        End If

        With objImage
            .ID = "imgRating" & intCounter.ToString
            .Width = "130"
            .Height = "25"
            .ImageUrl = RatingImageSelection(strRating)
        End With

        With objTextBox
            .ID = "RecipeReview" & intCounter.ToString
            .Rows = 3
            .ReadOnly = True
            .TextMode = TextBoxMode.MultiLine
            .MaxLength = 1000
            .Text = strComment
            .Width = Unit.Percentage(100)
        End With

        Me.pnlReviews.Controls.Add(New LiteralControl(strReviewCtrl))
        Me.pnlReviews.Controls.Add(New LiteralControl("<br />"))
        Me.pnlReviews.Controls.Add(New LiteralControl(strReviewCtrl2))
        Me.pnlReviews.Controls.Add(objImage)
        Me.pnlReviews.Controls.Add(New LiteralControl("<br />"))
        Me.pnlReviews.Controls.Add(objTextBox)

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
    Protected Sub OK_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/Account/DisplayRecipe?RecipeID=" & Me.hdnRecipeID.Value)
    End Sub

End Class