Imports System
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Owin

' ADDED RSS 9-21-2015
Imports System.Data
Imports System.Data.SqlClient
' END ADD

Partial Public Class Register
    Inherits Page
    Protected Sub CreateUser_Click(sender As Object, e As EventArgs)
        Dim strErr As String = ""
        Dim strLoginID As String
        Dim strUserAccount As String

        Dim strEmail As String = Me.Email.Text
        Dim strPassword As String = Me.Password.Text
        Dim strFirstName As String = Me.FirstName.Text
        Dim strLastName As String = Me.LastName.Text
        Dim strPhone As String = Me.Phone.Text
        Dim strAddress As String = Me.Address.Text
        Dim strCity As String = Me.City.Text
        Dim strZIP As String = Me.Zip.Text
        Dim strState As String = Me.ddState.SelectedValue.ToString
        Dim strSecurityQuestion As String = Me.ddSecurityQuestion.SelectedValue.ToString
        Dim strSecurityResponse As String = Me.SecurityResponse.Text

        Dim objData_DB As clsData_DB
        Dim objParams(5) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader

        strErr = Me.ValidateInput()
        If strErr = "NOERROR" Then

            ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
            Dim objWebConfig As New clsWebConfig()
            strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

            ' Use the Connection string to instantiate a new Database class object.
            objData_DB = New clsData_DB(strConnectionString)

            objParams(0) = objData_DB.MakeInParam("@Email", SqlDbType.VarChar, 50, strEmail)
            objParams(1) = objData_DB.MakeInParam("@isGoogle", SqlDbType.Bit, 1, (0))
            objParams(2) = objData_DB.MakeInParam("@PasswordHash", SqlDbType.VarChar, 128, strPassword)
            objParams(3) = objData_DB.MakeInParam("@PasswordSalt", SqlDbType.VarChar, 10, "0123456789")
            objParams(4) = objData_DB.MakeInParam("@SecurityQuestion", SqlDbType.VarChar, 200, strSecurityQuestion)
            objParams(5) = objData_DB.MakeInParam("@SecurityResponse", SqlDbType.VarChar, 200, UCase(strSecurityResponse))

            ' Run the stored procedure by name.  Pass with it the parameter list.
            objDR = objData_DB.RunStoredProc("usp_LoginAccount_Insert_AltOutput", objParams)

            If objDR.HasRows Then
                objDR.Read()
                strLoginID = objDR("LoginID").ToString

                If strLoginID = "-1" Then
                    ErrorMessage.Text = "The Email Address " & strEmail & " is already used.  Try something else."
                    strErr = "DUPLICATE"
                    Me.Email.Focus()
                ElseIf strLoginID = "0" Then
                    ErrorMessage.Text = "Unexpected Error Occurred. Try Again Later"
                    strErr = "ERROR"
                Else
                    strErr = "User Account " & strLoginID.ToString & " was Successfully Created.  Go to Login to use Account"
                    ErrorMessage.Text = strErr

                    ' If we successfully created the login account, we need to create its user account.
                    strUserAccount = Me.CreateUserAccount(strConnectionString, strEmail, strFirstName, strLastName, strPhone, strAddress, strCity, strState, strZIP, "Not Used")

                    strErr = "User Account " & strUserAccount.ToString & "/" & strLoginID.ToString & " was Successfully Created.  Go to Login to use Account"
                    ErrorMessage.Text = strErr

                End If
            End If

        Else
            ErrorMessage.Text = strErr
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

        'Dim userName As String = Email.Text
        'Dim manager = Context.GetOwinContext().GetUserManager(Of ApplicationUserManager)()
        'Dim signInManager = Context.GetOwinContext().Get(Of ApplicationSignInManager)()
        'Dim user = New ApplicationUser() With {.UserName = userName, .Email = userName}
        'Dim result = manager.Create(user, Password.Text)
        'If result.Succeeded Then
        '    ' For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        '    ' Dim code = manager.GenerateEmailConfirmationToken(user.Id)
        '    ' Dim callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request)
        '    ' manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=""" & callbackUrl & """>here</a>.")

        '    signInManager.SignIn(user, isPersistent := False, rememberBrowser := False)
        '    IdentityHelper.RedirectToReturnUrl(Request.QueryString("ReturnUrl"), Response)
        'Else
        '    ErrorMessage.Text = result.Errors.FirstOrDefault()
        'End If
    End Sub

    Private Function CreateUserAccount(ByVal strCon As String, ByVal strEmail As String,
                                  ByVal strFN As String, ByVal strLN As String,
                                  ByVal strPhone As String, ByVal strAddr As String,
                                  ByVal strCity As String, ByVal strState As String,
                                  ByVal strZip As String, ByVal strCountry As String) As String
        Dim objData_DB As clsData_DB
        Dim objParams(9) As SqlParameter
        Dim objDR As SqlDataReader

        Dim strAccountID As String = ""

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strCon)

        objParams(0) = objData_DB.MakeInParam("@Email", SqlDbType.VarChar, 50, strEmail)
        objParams(1) = objData_DB.MakeInParam("@FirstName", SqlDbType.VarChar, 50, strFN)
        objParams(2) = objData_DB.MakeInParam("@LastName", SqlDbType.VarChar, 50, strLN)
        objParams(3) = objData_DB.MakeInParam("@PhoneNumber", SqlDbType.VarChar, 12, strPhone)
        objParams(4) = objData_DB.MakeInParam("@StreetAddressLine1", SqlDbType.VarChar, 60, strAddr)
        objParams(5) = objData_DB.MakeInParam("@StreetAddressLine2", SqlDbType.VarChar, 60, "Not Used")
        objParams(6) = objData_DB.MakeInParam("@City ", SqlDbType.VarChar, 50, strCity)
        objParams(7) = objData_DB.MakeInParam("@StateProvName ", SqlDbType.VarChar, 50, strState)
        objParams(8) = objData_DB.MakeInParam("@PostalCode ", SqlDbType.NChar, 10, strZip)
        objParams(9) = objData_DB.MakeInParam("@CountryName ", SqlDbType.VarChar, 50, strCountry)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_UserAccount_Insert_AltOutput", objParams)

        If objDR.HasRows Then
            objDR.Read()
            strAccountID = objDR("UserAccountID").ToString
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

        Return strAccountID
    End Function
    Private Function ValidateInput() As String
        Dim strRet As String = "NOERROR"

        If Me.Email.Text = "" Then
            strRet = "Email Address is Required"
        End If

        Return strRet

    End Function

    ''' <summary>
    ''' Register_Load - Page Load Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Register_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' On Page Load of the Registration Page, we want to populate the SecurityQuestion dropdown listbox.
        ' Only perform is it is new.  If Postback, we should already be good to go.
        If Not IsPostBack Then
            Me.Populate_SecurityQuestion_DropDown()
            Me.Populate_StateProv_DropDown()
        End If

    End Sub

    Private Sub Populate_SecurityQuestion_DropDown()
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
        objParams(0) = objData_DB.MakeInParam("@SecurityQuestion ", SqlDbType.VarChar, 200, "ALL")

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_SecurityQuestionList_Select", objParams)

        ' Do we have rows returned?
        If objDR.HasRows Then
            ' Clear the List before we populate
            Me.ddSecurityQuestion.Items.Clear()

            ' Iterate Through the DataReader and Populate the Listbox
            Me.ddSecurityQuestion.Items.Add(New ListItem(""))
            While objDR.Read()
                Me.ddSecurityQuestion.Items.Add(New ListItem(objDR("SecurityQuestion").ToString))
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

    Private Sub Populate_StateProv_DropDown()
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
        objParams(0) = objData_DB.MakeInParam("@TranCode", SqlDbType.VarChar, 10, "NC")

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_StateProList_Select", objParams)

        ' Do we have rows returned?
        If objDR.HasRows Then
            ' Clear the List before we populate
            Me.ddState.Items.Clear()

            ' Iterate Through the DataReader and Populate the Listbox
            Me.ddState.Items.Add(New ListItem(""))
            While objDR.Read()
                Me.ddState.Items.Add(New ListItem(objDR("StateProvName").ToString))
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
End Class

