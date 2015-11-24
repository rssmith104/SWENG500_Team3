Imports System
Imports System.Web
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Owin

Imports System.Data
Imports System.Data.SqlClient
Partial Public Class ForgotPassword
    Inherits System.Web.UI.Page

    Protected Property StatusMessage() As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


    End Sub

    Protected Sub Forgot(sender As Object, e As EventArgs)
        'Dim objCommunicate As clsCommunication = New clsCommunication()

        Response.Write("Before the Email Call <br />")

        'Dim StrErr As String = objCommunicate.SendEmail("smtp.gmail.com", "rss261@psu.edu", "RMSAPP_Server@psu.edu", "Test Line", "Does this message get through?",,)


    End Sub

    Protected Sub SecurityQuestion(sender As Object, e As EventArgs)
        Dim objData_DB As clsData_DB
        Dim objParams(0) As SqlParameter
        Dim objDR As SqlDataReader
        Dim strConnectionString As String

        Dim strAccountID As String = ""
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        objParams(0) = objData_DB.MakeInParam("@Email", SqlDbType.VarChar, 50, txtEmail.Text)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_SecurityQuestion_Select_ByEmail", objParams)

        If objDR.HasRows Then
            objDR.Read()
            If objDR("SecurityQuestion").ToString = "NOTFOUND" Then
                Me.FailureText.Text = "CRITICAL ERROR:  Email Unrecognized.  Please use a Valid Account"
                Me.ErrorMessage.Visible = True
            Else
                Me.lblSecurityQuestion.Text = objDR("SecurityQuestion")
                Me.phDisplaySecurityQuestion.Visible = True
                Me.ErrorMessage.Visible = False
            End If

        Else
            Me.FailureText.Text = "CRITICAL ERROR:  Email Unrecognized.  Please use a Valid Account"
            Me.ErrorMessage.Visible = True
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


    Protected Sub VerifyAnswer(sender As Object, e As EventArgs)

        Dim objData_DB As clsData_DB
        Dim objParams(1) As SqlParameter
        Dim objDR As SqlDataReader
        Dim strConnectionString As String

        Dim strAccountID As String = ""
        Dim objWebConfig As New clsWebConfig()
        strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

        ' Use the Connection string to instantiate a new Database class object.
        objData_DB = New clsData_DB(strConnectionString)

        objParams(0) = objData_DB.MakeInParam("@Email", SqlDbType.VarChar, 50, txtEmail.Text)
        objParams(1) = objData_DB.MakeInParam("@UserResponse", SqlDbType.VarChar, 20, Me.txtAnswer.Text)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_SecurityQuestion_Verify_ByEmail", objParams)

        If objDR.HasRows Then
            objDR.Read()
            If objDR("ValidResponse").ToString = "NO" Then
                Me.FailureText.Text = "CRITICAL ERROR:  Server Unable to Process Request.  Try Again Later"
            Else
                Dim objDes_Codec As DES_Codec = New DES_Codec()
                Dim strEncryptString As String = objDes_Codec.EncodeString(txtEmail.Text)
                objDes_Codec = Nothing

                Response.Redirect("~/Account/ManagePassword.aspx?et=" & strEncryptString)
            End If

        Else
            Me.FailureText.Text = "CRITICAL ERROR:  Server Unable to Process Request.  Try Again Later"
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