Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin

' ADDED RSS 9-21-2015
Imports System.Data
Imports System.Data.SqlClient
' END ADD

Partial Public Class ManagePassword
    Inherits System.Web.UI.Page
    Protected Property SuccessMessage() As String
      Get
            Return m_SuccessMessage
        End Get
        Private Set
            m_SuccessMessage = Value
        End Set
    End Property
    Private m_SuccessMessage As String

    Protected Property LoggedInUser() As String
        Get
            Return m_strLoggedInUser
        End Get
        Set(value As String)
            m_strLoggedInUser = value
        End Set
    End Property
    Private m_strLoggedInUser

    Private Function HasPassword(manager As ApplicationUserManager) As Boolean
        Return manager.HasPassword(User.Identity.GetUserId())
    End Function

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Dim manager = Context.GetOwinContext().GetUserManager(Of ApplicationUserManager)()

        If Not IsPostBack Then
            Dim strUser As String = Request.QueryString("et")

            'Dim objDes_Codec As DES_Codec = New DES_Codec()
            'LoggedInUser = objDes_Codec.DecodeString(strUser)
            LoggedInUser = strUser
            'objDes_Codec = Nothing

            'Session("RMS_LoggedInUser") = LoggedInUser()

            setPassword.Visible = True
            changePasswordHolder.Visible = False

            ' Determine the sections to render
            'If HasPassword(manager) Then
            '    changePasswordHolder.Visible = True
            'Else
            '    setPassword.Visible = True
            '    changePasswordHolder.Visible = False
            'End If

            '' Render success message
            Dim message = Request.QueryString("m")
            If message IsNot Nothing Then
                ' Strip the query string from action
                If Me.m_strLoggedInUser = "" Then
                    Form.Action = ResolveUrl("~/Account/Login")
                Else
                    Form.Action = ResolveUrl("~/Account/ManageProfile")
                End If
            End If
        Else
            LoggedInUser = Session("RMS_LoggedInUser")
        End If
    End Sub

      Protected Sub ChangePassword_Click(sender As Object, e As EventArgs)
        If IsValid Then
            Dim manager = Context.GetOwinContext().GetUserManager(Of ApplicationUserManager)()
            Dim signInManager = Context.GetOwinContext().Get(Of ApplicationSignInManager)()
            Dim result As IdentityResult = manager.ChangePassword(User.Identity.GetUserId(), CurrentPassword.Text, NewPassword.Text)
            If result.Succeeded Then
                Dim userInfo = manager.FindById(User.Identity.GetUserId())
                signInManager.SignIn(userInfo, isPersistent := False, rememberBrowser := False)
                Response.Redirect("~/Account/Manage?m=ChangePwdSuccess")
            Else
                AddErrors(result)
            End If
        End If
    End Sub

    Protected Sub SetPassword_Click(sender As Object, e As EventArgs)
        Dim strResult As String

        If IsValid Then
            ' Create the local login info and link the local account to the user
            'Dim manager = Context.GetOwinContext().GetUserManager(Of ApplicationUserManager)()
            'Dim result As IdentityResult = manager.AddPassword(User.Identity.GetUserId(), password.Text)

            Dim objData_DB As clsData_DB
            Dim objParams(2) As SqlParameter
            Dim strConnectionString As String
            Dim objDR As SqlDataReader

            Dim objDes_Codec As DES_Codec = New DES_Codec()


            ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
            Dim objWebConfig As New clsWebConfig()
            strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

            ' Use the Connection string to instantiate a new Database class object.
            objData_DB = New clsData_DB(strConnectionString)

            objParams(0) = objData_DB.MakeInParam("@email", SqlDbType.VarChar, 50, LoggedInUser())
            objParams(1) = objData_DB.MakeInParam("@PasswordHash", SqlDbType.VarChar, 128, objDes_Codec.EncodeString(password.Text))
            objParams(2) = objData_DB.MakeInParam("@PasswordSalt", SqlDbType.VarChar, 10, "0123456789")

            'Response.Write(LoggedInUser() & " - " & password.Text & " - " & objDes_Codec.EncodeString(password.Text))
            'Response.End()

            Try
                objDR = objData_DB.RunStoredProc("usp_password_reset", objParams)
                strResult = ""
            Catch ex As Exception
                strResult = ex.Message
                'Response.Write(strResult)
                'Response.End()
            End Try

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

            If strResult = "" Then
                Response.Redirect("~/Account/ManageProfile?m=SetPwdSuccess")
            Else
                Response.Redirect("~/Account/Manage?m=" & strResult)
            End If
        End If
    End Sub

    Private Sub AddErrors(result As IdentityResult)
        For Each [error] As String In result.Errors
            ModelState.AddModelError("", [error])
        Next
    End Sub
End Class

