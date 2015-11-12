Imports System
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Owin

Imports System.IO
Imports System.Collections.Generic

' ADDED RSS 9-21-2015
Imports System.Data
Imports System.Data.SqlClient
' END ADD

Partial Public Class ManageProfile
    Inherits Page

    Public strLoggedInUser As String
    Public strAuthenticated As String
    Public strFunction As String
    Public strProfileImage As String

    Public strFirstName As String
    Public strLastName As String

    Public strMessage As String

    Protected Sub SaveChanges_Click(sender As Object, e As EventArgs)
        Dim strErr As String = ""

        ErrorMessage.Text = Me.UpdateUserAccount()
        If ErrorMessage.Text = "" Then
            ErrorMessage.Text = Me.UpdateSecurityQuestion()
            Me.UpdateInsertProfileImage()
        End If

        If ErrorMessage.Text = "" Then
            ErrorMessage.Text = "Record Successfully Updated"
        End If

    End Sub

    Protected Sub ResetPassword_Click(sender As Object, e As EventArgs)
        Dim strEncryptString As String

        Session("RMS_LoggedInUser") = strLoggedInUser
        Session("RMS_Authenticated") = strAuthenticated
        Session("RMS_Function") = "PASSWORD_RESET"
        Session("RMS_ReturnPage") = "~/Account/ManageProfile"

        Dim objDes_Codec As DES_Codec = New DES_Codec()
        strEncryptString = objDes_Codec.EncodeString(strLoggedInUser)
        objDes_Codec = Nothing

        Response.Redirect("~/Account/ManagePassword.aspx?et=" & strEncryptString)
    End Sub

    Private Function UpdateUserAccount() As String
        Dim strErr As String
        Dim strEmail As String = Me.Email.Text
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
        Dim objParams(11) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader

        Dim strDebugMsg As String = ""

        strErr = Me.ValidateInput()
        If strErr = "NOERROR" Then

            ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
            Dim objWebConfig As New clsWebConfig()
            strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

            ' Use the Connection string to instantiate a new Database class object.
            objData_DB = New clsData_DB(strConnectionString)

            objParams(0) = objData_DB.MakeInParam("@Email", SqlDbType.VarChar, 50, strEmail)
            objParams(1) = objData_DB.MakeInParam("@FirstName", SqlDbType.VarChar, 50, strFirstName)
            objParams(2) = objData_DB.MakeInParam("@LastName", SqlDbType.VarChar, 50, strLastName)
            objParams(3) = objData_DB.MakeInParam("@PhoneNumber", SqlDbType.VarChar, 12, strPhone)
            objParams(4) = objData_DB.MakeInParam("@StreetAddressLine1", SqlDbType.VarChar, 60, strAddress)
            objParams(5) = objData_DB.MakeInParam("@StreetAddressLine2", SqlDbType.VarChar, 60, "NA")
            objParams(6) = objData_DB.MakeInParam("@City", SqlDbType.VarChar, 50, strCity)
            objParams(7) = objData_DB.MakeInParam("@StateProvName", SqlDbType.VarChar, 50, strState)
            objParams(8) = objData_DB.MakeInParam("@PostalCode", SqlDbType.NChar, 10, strZIP)
            objParams(9) = objData_DB.MakeInParam("@CountryName", SqlDbType.VarChar, 50, "NA")
            objParams(10) = objData_DB.MakeInParam("@isLocked", SqlDbType.Bit, 1, 0)
            objParams(11) = objData_DB.MakeInParam("@isActive", SqlDbType.Bit, 1, 0)

            ' Run the stored procedure by name.  Pass with it the parameter list.
            objDR = objData_DB.RunStoredProc("usp_UserAccount_Update_AltOutput", objParams)

            If objDR.HasRows Then
                objDR.Read()

                If objDR("UserAccountID").ToString = "-1" Then
                    strErr = "ERROR"
                Else
                    strErr = ""
                End If
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

        Return strErr
    End Function

    Private Sub UpdateInsertProfileImage()
        Dim strEmail As String = Me.Email.Text
        Dim strImageURL As String = Me.imProfileImage.ImageUrl.ToString()

        Dim objData_DB As clsData_DB
        Dim objParams(1) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader
        Dim objByte As Byte = &H20

        If strImageURL <> "" Then
            ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
            Dim objWebConfig As New clsWebConfig()
            strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

            ' Use the Connection string to instantiate a new Database class object.
            objData_DB = New clsData_DB(strConnectionString)

            objParams(0) = objData_DB.MakeInParam("@Email", SqlDbType.VarChar, 50, strEmail)
            objParams(1) = objData_DB.MakeInParam("@ImageText", SqlDbType.VarChar, 200, strImageURL)

            ' Run the stored procedure by name.  Pass with it the parameter list.
            objDR = objData_DB.RunStoredProc("usp_UserAccountImage_InsertUpdate", objParams)

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

        End If
    End Sub

    Private Function UpdateSecurityQuestion() As String
        Dim strErr As String
        Dim strEmail As String = Me.Email.Text
        Dim strSecurityQuestion As String = Me.ddSecurityQuestion.SelectedValue.ToString
        Dim strSecurityResponse As String = Me.SecurityResponse.Text

        Dim objData_DB As clsData_DB
        Dim objParams(2) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader

        Dim strDebugMsg As String = ""

        strErr = Me.ValidateInput()
        If strErr = "NOERROR" Then

            ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
            Dim objWebConfig As New clsWebConfig()
            strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

            ' Use the Connection string to instantiate a new Database class object.
            objData_DB = New clsData_DB(strConnectionString)

            objParams(0) = objData_DB.MakeInParam("@Email", SqlDbType.VarChar, 50, strEmail)
            objParams(1) = objData_DB.MakeInParam("@SecurityQuestion", SqlDbType.VarChar, 200, strSecurityQuestion)
            objParams(2) = objData_DB.MakeInParam("@SecurityResponse", SqlDbType.VarChar, 200, strSecurityResponse)

            ' Run the stored procedure by name.  Pass with it the parameter list.
            objDR = objData_DB.RunStoredProc("usp_SecurityQuestionResponse_ByEmail_Update", objParams)

            If objDR.HasRows Then
                objDR.Read()
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

    End Function

    Private Function SaveProfileUpdate(ByVal strCon As String, ByVal strEmail As String,
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

    Private Sub GetProfileData(ByVal strEmailID)
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
        objDR = objData_DB.RunStoredProc("usp_Profile_Select", objParams)

        If objDR.HasRows Then
            objDR.Read()

            Me.FirstName.Text = objDR("FirstName").ToString
            Me.LastName.Text = objDR("LastName").ToString
            Me.Phone.Text = objDR("PhoneNumber").ToString
            Me.Address.Text = objDR("StreetAddressLine1").ToString
            Me.Zip.Text = objDR("PostalCode").ToString
            Me.City.Text = objDR("City").ToString
            Me.Email.Text = objDR("Email").ToString
            Me.ddState.SelectedIndex = ddState.Items.IndexOf(ddState.Items.FindByText(objDR("StateProvName").ToString))
            Me.ddSecurityQuestion.SelectedIndex = ddSecurityQuestion.Items.IndexOf(ddSecurityQuestion.Items.FindByText(objDR("SecurityQuestion").ToString))
            Me.SecurityResponse.Text = objDR("SecurityResponse").ToString

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
            Me.imProfileImage.ImageUrl = objDR("ImageText").ToString
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

    Protected Sub Image_Upload(sender As Object, e As EventArgs)
        If Me.fuProfilePicUpload.HasFile Then
            Dim strFileName As String = Path.GetFileName(Me.fuProfilePicUpload.PostedFile.FileName)
            fuProfilePicUpload.PostedFile.SaveAs(Server.MapPath("~/Images/") + strFileName)

            Me.imProfileImage.ImageUrl = "~/images/" & strFileName

            'Me.imProfileImage.ImageUrl = Server.MapPath("~/Images/") + strFileName
            Session("RMS_ProfileImage") = "~/Images/" & strFileName
            SetFocus(Me.btnSave)
            'Response.Redirect(Request.Url.AbsoluteUri)

        End If
    End Sub

    ''' <summary>
    ''' Register_Load - Page Load Event Handler
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ManageProfile_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' On Page Load of the Registration Page, we want to populate the SecurityQuestion dropdown listbox.
        ' Only perform is it is new.  If Postback, we should already be good to go.
        strLoggedInUser = Session("RMS_LoggedInUser")
        strAuthenticated = Session("RMS_Authenticated")
        strFunction = Session("RMS_Function")
        strProfileImage = Session("RMS_ProfileImage")

        strMessage = Request.QueryString("m")
        If strMessage = "SetPwdSuccess" Then
            ErrorMessage.Text = "Password Change Successful"
        End If

        If Not IsPostBack Then
            Me.Populate_SecurityQuestion_DropDown()
            Me.Populate_StateProv_DropDown()

            ' Get Profile Data
            Me.GetProfileData(strLoggedInUser)
            Me.GetImageProfile(strLoggedInUser)
        End If

        If strProfileImage <> "" Then
            Me.imProfileImage.ImageUrl = strProfileImage
        End If

    End Sub

    Protected Sub CancelChanges_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/Authenticated_Default")
    End Sub

End Class

