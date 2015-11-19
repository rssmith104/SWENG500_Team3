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

Partial Public Class CircleOfFriends
    Inherits Page

    Public strAuthenticated As String
    Public strLoggedInUser As String
    Public strFunction As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        strAuthenticated = Session("RMS_Authenticated")
        strLoggedInUser = Session("RMS_LoggedInUser")
        strFunction = Session("RMS_Function")

        'First Time Through
        lblCircleOwner.Text = strLoggedInUser
        Me.GetImageProfile(strLoggedInUser)

        AddActiveAdPendingFriends(strLoggedInUser)
        AddMyInvitations(strLoggedInUser)

        If Not IsPostBack Then
            Response.Write("First Time Through")
        Else
            'In PostBack Condition
            Response.Write("PostBack Code")
        End If

    End Sub

    Private Sub GetImageProfile(ByVal strEmailID As String)
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader
        Dim strConnectionString As String

        Dim strAccountID As String = ""
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        objParams(0) = objData_DB.MakeInParam("@EmailID", SqlDbType.VarChar, 50, strEmailID)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_UserAccountImage_Select", objParams)

        If objDR.HasRows Then
            objDR.Read()
            Me.imgProfile.ImageUrl = objDR("ImageText").ToString
            'Session("RMS_ProfileImage") = objDR("ImageText").ToString

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

    Private Function ReturnImageProfile(ByVal strEmail As String) As String
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader
        Dim strConnectionString As String
        Dim strRetVal As String

        Dim strAccountID As String = ""
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        objParams(0) = objData_DB.MakeInParam("@EmailID", SqlDbType.VarChar, 50, strEmail)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_UserAccountImage_Select", objParams)

        If objDR.HasRows Then
            objDR.Read()
            strRetVal = objDR("ImageText").ToString
            'Session("RMS_ProfileImage") = objDR("ImageText").ToString
        Else
            strRetVal = "NOIMAGE"
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

    Public Sub AddActiveAdPendingFriends(ByVal strUserName As String)
        Dim i As Integer = AddMyFriends(strUserName)
        AppendPendingRequests(strUserName, i)
    End Sub

    Public Function AddMyFriends(ByVal strUserName As String) As Int16
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader
        Dim strConnectionString As String
        Dim strPending As String
        Dim strImage As String


        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@OwnerEmail", SqlDbType.VarChar, 50, strUserName)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_CircleOfFriends_SelectActiveFriends", objParams)

        Dim iCounter As Int16 = 0

        If objDR.HasRows Then

            While objDR.Read
                iCounter += 1

                If objDR("Pending").ToString = "True" Then
                    strPending = "Pending"
                Else
                    strPending = "Active"
                End If

                strImage = ReturnImageProfile(objDR("Email").ToString)

                Me.AddAFriendControl(objDR("Friend Name").ToString & " (" & objDR("Email").ToString & ")", strImage, strPending, objDR("LoginID").ToString, iCounter)
            End While
        Else
            Me.AddAFriendControl("NOFRIEND", "NOIMAGE", "NOSTATUS", "0", 1)
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

        Return iCounter
        'Mockup Code used to test AddFriendControl
        'AddAFriendControl("John.Mason@gmail.com", "NOIMAGE", "Active", 1)
        'AddAFriendControl("Phyllis.Mason@gmail.com", "~/images/phyllis.png", "Active", 2)
        'AddAFriendControl("Scott.heigel@gmail.com", "~/images/Scott.png", "Pending", 3)
        'AddAFriendControl("Lydell.Peters@gmail.com", "NOIMAGE", "Active", 4)
        'AddAFriendControl("Jimmy.Jones@swanson.com", "~/images/jimmy.jpg", "Pending", 5)
    End Function

    Public Function AppendPendingRequests(ByVal strUserName As String, ByVal iCounter As Integer) As Int16
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader
        Dim strConnectionString As String
        Dim strPending As String
        Dim strImage As String


        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@OwnerEmail", SqlDbType.VarChar, 50, strUserName)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_CircleOfFriends_SelectPendingFriends", objParams)

        If objDR.HasRows Then

            While objDR.Read
                iCounter += 1

                If objDR("Pending").ToString = "True" Then
                    strPending = "Pending"
                Else
                    strPending = "Active"
                End If

                strImage = ReturnImageProfile(objDR("Email").ToString)

                Me.AddAFriendControl(objDR("Friend Name").ToString & " (" & objDR("Email").ToString & ")", strImage, strPending, objDR("LoginID").ToString, iCounter)
            End While
        Else
            Me.AddAFriendControl("NOFRIEND", "NOIMAGE", "NOSTATUS", "0", 1)
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

        Return iCounter
        'Mockup Code used to test AddFriendControl
        'AddAFriendControl("John.Mason@gmail.com", "NOIMAGE", "Active", 1)
        'AddAFriendControl("Phyllis.Mason@gmail.com", "~/images/phyllis.png", "Active", 2)
        'AddAFriendControl("Scott.heigel@gmail.com", "~/images/Scott.png", "Pending", 3)
        'AddAFriendControl("Lydell.Peters@gmail.com", "NOIMAGE", "Active", 4)
        'AddAFriendControl("Jimmy.Jones@swanson.com", "~/images/jimmy.jpg", "Pending", 5)
    End Function


    Public Sub AddMyInvitations(ByVal strUserName As String)
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader
        Dim strConnectionString As String
        Dim strImage As String

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@OwnerEmail", SqlDbType.VarChar, 50, strUserName)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_CircleOfFriends_SelectMyInvites", objParams)

        If objDR.HasRows Then
            Dim iCounter As Int16 = 0

            While objDR.Read
                iCounter += 1

                strImage = ReturnImageProfile(objDR("Email").ToString)
                Me.AddAnInviteControl(objDR("Friend Name").ToString & " (" & objDR("Email").ToString & ")", strImage, objDR("CircleOfFriendsID").ToString, iCounter)

            End While
        Else
            Me.AddAnInviteControl("NOINVITE", "NOIMAGE", "0", 1)
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

        'AddAnInviteControl("Heisenberg@AMC.com", "~/images/heisenberg.jpg", 1)
        'AddAnInviteControl("Jessie@AMC.com", "~/images/Jessie.png", 2)
    End Sub


    Public Sub AddAFriendControl(ByVal strName As String,
                                 ByVal strImage As String,
                                 ByVal strStatus As String,
                                 ByVal strID As String,
                                 ByVal intCounter As Int16)

        Dim lblItemNo As Label = New Label()
        Dim lblName As Label = New Label()
        Dim btnAction As Button = New Button()
        Dim lblStatus As Label = New Label()
        Dim imgFriend As Image = New Image()
        Dim strBackColor As String

        Dim iCounter As Int16 = 0

        If strName = "NOFRIEND" Then
            Me.pnlFriends.Controls.Add(New LiteralControl("<p style=""background-color:white""><font face="" Courier""><i>You Have No Friends</i></font></p>"))
        Else
            If intCounter Mod 2 = 0 Then
                strBackColor = "#DCDCDC"
            Else
                strBackColor = "#A9A9A9"
            End If

            lblItemNo.ID = "lblItemNo_" & strID.ToString
            lblItemNo.Text = intCounter.ToString & "."

            ' Setup the Name Control
            lblName.ID = "lblFriendName_" & strID.ToString
            lblName.Text = strName

            ' Setup the Image
            imgFriend.ID = "imgFriend_" & strID.ToString
            imgFriend.Width = 50
            imgFriend.Height = 50
            imgFriend.BorderWidth = 1
            If strImage = "NOIMAGE" Then
                imgFriend.ImageUrl = "~/images/NoFriendImage.png"
            Else
                imgFriend.ImageUrl = strImage
            End If

            '"<p style=""background-color:" & strBackColor & """>"

            lblStatus.ID = "lblStatus_" & strID.ToString
            lblStatus.Text = strStatus

            btnAction.ID = "btnAction_" & strID.ToString
            If strStatus = "Active" Then
                btnAction.Text = "Un-Friend"
            Else
                btnAction.Text = "Cancel Invitation"
            End If
            btnAction.Attributes.Add("Class", "btn btn-primary btn-sm")
            AddHandler btnAction.Click, AddressOf btnCancelFriend_Click

            Me.pnlFriends.Controls.Add(New LiteralControl("<p style=""background-color:" & strBackColor & """><font face="" Courier"">"))
            Me.AddSpaces(Me.pnlFriends, 1)
            Me.pnlFriends.Controls.Add(lblItemNo)

            Me.AddSpaces(Me.pnlFriends, 4 - (iCounter.ToString).Length)
            Me.pnlFriends.Controls.Add(imgFriend)

            Me.AddSpaces(Me.pnlFriends, 4)
            Me.pnlFriends.Controls.Add(lblName)

            Me.AddSpaces(Me.pnlFriends, 50 - strName.Length)
            Me.pnlFriends.Controls.Add(lblStatus)

            Me.AddSpaces(Me.pnlFriends, 15 - strStatus.Length)

            Me.pnlFriends.Controls.Add(btnAction)
            Me.pnlFriends.Controls.Add(New LiteralControl("</font></p>"))
        End If


    End Sub

    Public Sub AddAnInviteControl(ByVal strName As String,
                                  ByVal strImage As String,
                                  ByVal strID As String,
                                  ByVal intCounter As Int16)

        Dim lblItemNo As Label = New Label()
        Dim lblName As Label = New Label()
        Dim btnAccept As Button = New Button()
        Dim btnReject As Button = New Button()
        Dim imgFriend As Image = New Image()
        Dim strBackColor As String

        Dim iCounter As Int16 = 0

        If strName = "NOINVITE" Then
            Me.pnlInvites.Controls.Add(New LiteralControl("<p style=""background-color:white""><font face="" Courier""><i>No Outstanding Invitations</i></font></p>"))
        Else

            If intCounter Mod 2 = 0 Then
                strBackColor = "#DCDCDC"
            Else
                strBackColor = "#A9A9A9"
            End If

            lblItemNo.ID = "lblItemNo_" & strID.ToString
            lblItemNo.Text = intCounter.ToString & "."

            ' Setup the Name Control
            lblName.ID = "lblFriendName_" & strID.ToString
            lblName.Text = strName

            ' Setup the Image
            imgFriend.ID = "imgFriend_" & strID.ToString
            imgFriend.Width = 50
            imgFriend.Height = 50
            imgFriend.BorderWidth = 1
            If strImage = "NOIMAGE" Then
                imgFriend.ImageUrl = "~/images/NoFriendImage.png"
            Else
                imgFriend.ImageUrl = strImage
            End If

            btnAccept.ID = "btnAccept_" & strID.ToString
            btnAccept.Text = "Accept"
            btnAccept.Attributes.Add("Class", "btn btn-primary btn-sm")
            AddHandler btnAccept.Click, AddressOf btnAccept_Click

            btnReject.ID = "btnReject_" & strID.ToString
            btnReject.Text = "Reject"
            btnReject.Attributes.Add("Class", "btn btn-primary btn-sm")
            AddHandler btnReject.Click, AddressOf btnReject_Click

            ' Add Leading Item Number
            Me.pnlInvites.Controls.Add(New LiteralControl("<p style=""background-color:" & strBackColor & """><font face="" Courier"">"))
            Me.AddSpaces(Me.pnlInvites, 1)
            Me.pnlInvites.Controls.Add(lblItemNo)

            ' Add the user's profile image
            Me.AddSpaces(Me.pnlInvites, 4 - (iCounter.ToString).Length)
            Me.pnlInvites.Controls.Add(imgFriend)

            ' Add the user's name
            Me.AddSpaces(Me.pnlInvites, 4)
            Me.pnlInvites.Controls.Add(lblName)

            Me.AddSpaces(Me.pnlInvites, 50 - strName.Length)
            Me.pnlInvites.Controls.Add(btnAccept)

            Me.AddSpaces(Me.pnlInvites, 7 - btnAccept.Text.Length)
            Me.pnlInvites.Controls.Add(btnReject)

            Me.pnlInvites.Controls.Add(New LiteralControl("</font></p>"))
        End If

    End Sub

    Private Sub AddSpaces(ByRef objPanel As Panel, ByVal iLength As Int16)
        Dim iCounter As Int16

        For iCounter = 0 To iLength
            objPanel.Controls.Add(New LiteralControl("&nbsp;"))
        Next
    End Sub

    Protected Sub Return_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/Authenticated_Default")
    End Sub

    Protected Sub AddAFriend_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/AddAFriend")
    End Sub

    Private Sub btnAccept_Click(sender As Object, e As EventArgs)
        Dim objBtnCtrl As System.Web.UI.WebControls.Button = sender
        Dim iCircleOfFriendsID As Int16

        iCircleOfFriendsID = Me.ExtractCircleOfFriendsID(objBtnCtrl.ID.ToString)

        Me.AcceptCircleOfFriendsRequest(iCircleOfFriendsID)

        Response.Redirect("~/CircleOfFriend")

    End Sub

    Private Sub btnReject_Click(sender As Object, e As EventArgs)
        Dim objBtnCtrl As System.Web.UI.WebControls.Button = sender
        Dim iCircleOfFriendsID As Int16

        iCircleOfFriendsID = Me.ExtractCircleOfFriendsID(objBtnCtrl.ID.ToString)
        Me.RejectCircleOfFriendsRequest(iCircleOfFriendsID)

        Response.Redirect("~/CircleOfFriend")

    End Sub

    Private Sub btnCancelFriend_Click(sender As Object, e As EventArgs)
        Dim objBtnCtrl As System.Web.UI.WebControls.Button = sender
        Dim iCircleOfFriendsID As Int16

        iCircleOfFriendsID = Me.ExtractCircleOfFriendsID(objBtnCtrl.ID.ToString)

        Me.RejectCircleOfFriendsRequest(iCircleOfFriendsID)

        Response.Redirect("~/CircleOfFriend")
    End Sub

    Private Sub RejectCircleOfFriendsRequest(ByVal intCircleOfFriendsID As Int16)
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
        objParams(0) = objData_DB.MakeInParam("@CircleOfFriendsID", SqlDbType.Int, 2, intCircleOfFriendsID)

        Try
            ' Run the stored procedure by name.  Pass with it the parameter list.
            objDR = objData_DB.RunStoredProc("usp_CircleOfFriends_MyInvitesDelete_ByID", objParams)

        Catch ex As Exception


        Finally
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
        End Try

    End Sub

    Private Sub AcceptCircleOfFriendsRequest(ByVal intCircleOfFriendsID As Int16)
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
        objParams(0) = objData_DB.MakeInParam("@CircleOfFriendsID", SqlDbType.Int, 2, intCircleOfFriendsID)

        Try
            ' Run the stored procedure by name.  Pass with it the parameter list.
            objDR = objData_DB.RunStoredProc("usp_CircleOfFriends_MyInvitesAccept_ByID", objParams)

        Catch ex As Exception

        Finally
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
        End Try


    End Sub

    Private Function ExtractCircleOfFriendsID(ByVal strID As String) As Int16
        Dim intReturnID As Int16
        Dim strTokens() As String

        strTokens = Split(strID, "_")

        Try

            intReturnID = CInt(strTokens(1))
        Catch ex As Exception
            intReturnID = -1
        End Try

        Return intReturnID
    End Function

End Class
