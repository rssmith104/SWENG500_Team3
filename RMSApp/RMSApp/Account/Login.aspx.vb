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

Partial Public Class Login
    Inherits Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        RegisterHyperLink.NavigateUrl = "Register"
        ' Enable this once you have account confirmation enabled for password reset functionality
        ForgotPasswordHyperLink.NavigateUrl = "Forgot"
        OpenAuthLogin.ReturnUrl = Request.QueryString("ReturnUrl")
        Dim returnUrl = HttpUtility.UrlEncode(Request.QueryString("ReturnUrl"))
        If Not [String].IsNullOrEmpty(returnUrl) Then
            RegisterHyperLink.NavigateUrl += "?ReturnUrl=" & returnUrl
        End If
    End Sub

    Protected Sub LogIn(sender As Object, e As EventArgs)

        If IsValid Then

            Dim strPasswordCheck As String = Me.VerifyLogin(Email.Text, Password.Text)

            Select Case strPasswordCheck
                Case "SUCCESS"
                    'Response.Redirect()
                    FailureText.Text = "Successful Login"
                    ErrorMessage.Visible = True
                    Session("RMS_Authenticated") = "True"
                    Session("RMS_LoggedInUser") = Email.Text
                    Session("RMS_Function") = "LOGIN"
                    Response.Redirect("/RMSApp/Default.aspx")
                    Exit Select

                Case "INVALID_PASSWORD"
                    FailureText.Text = "Invalid login attempt: Either User Account or Password is Invalid.  Try Again"
                    ErrorMessage.Visible = True
                    Exit Select

                Case "INVALID_ACCOUNT"
                    FailureText.Text = "INvalid login attempt: Either User Account or Password is Invalid.  Try Again"
                    ErrorMessage.Visible = True
                    Exit Select
            End Select

            ' Validate the user password
            'Dim manager = Context.GetOwinContext().GetUserManager(Of ApplicationUserManager)()
            'Dim signinManager = Context.GetOwinContext().GetUserManager(Of ApplicationSignInManager)()

            ' This doen't count login failures towards account lockout
            ' To enable password failures to trigger lockout, change to shouldLockout := True
            'Dim result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout:=False)

            'Select Case result
            '    Case SignInStatus.Success
            '        IdentityHelper.RedirectToReturnUrl(Request.QueryString("ReturnUrl"), Response)
            '        Exit Select
            '    Case SignInStatus.LockedOut
            '        Response.Redirect("/Account/Lockout")
            '        Exit Select
            '    Case SignInStatus.RequiresVerification
            '        Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
            '                                        Request.QueryString("ReturnUrl"),
            '                                        RememberMe.Checked),
            '                          True)
            '        Exit Select
            '    Case Else
            '        FailureText.Text = "Invalid login attempt"
            '        ErrorMessage.Visible = True
            '        Exit Select
            'End Select
        End If
    End Sub

    Private Function VerifyLogin(ByVal strEmail As String, ByVal strPassword As String) As String
        Dim strResponse As String = ""

        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader
        Dim strEncryptedPassword As String
        Dim strDecryptedPassword As String

        Dim iStatus As Integer

        ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
        objParams(0) = objData_DB.MakeInParam("@Email", SqlDbType.VarChar, 50, strEmail)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_LoginPassword_Select", objParams)

        ' Do we have rows returned?
        If objDR.HasRows Then
            objDR.Read()
            ' We have a row, check the status first.  If status is zero, we found a record.  We need to check the password.
            iStatus = objDR("Status")
            If iStatus = 0 Then
                ' Check the password.
                strEncryptedPassword = objDR("PasswordHash").ToString
                Dim objDes_Codec As DES_Codec = New DES_Codec()
                strDecryptedPassword = objDes_Codec.DecodeString(strEncryptedPassword)
                objDes_Codec = Nothing

                If strDecryptedPassword = strPassword Then
                    strResponse = "SUCCESS"
                Else
                    strResponse = "INVALID_PASSWORD"
                End If
            Else
                strResponse = "INVALID_ACCOUNT"
            End If
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

        Return strResponse
    End Function

End Class
