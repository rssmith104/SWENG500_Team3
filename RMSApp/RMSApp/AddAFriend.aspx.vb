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

Partial Public Class AddAFriend
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

        If Not IsPostBack Then
            PopulateFriendDropDown(strLoggedInUser)
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



    Protected Sub Return_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/CircleOfFriend")
    End Sub

    Protected Sub Add_Click(sender As Object, e As EventArgs)
        Dim objData_DB As clsData_DB
        Dim objParams(1) As SqlParameter
        Dim strConnectionString As String
        Dim objDR As SqlDataReader

        If ddAddAFriend.SelectedValue <> "" Then
            ' Get the connection string out of the Web.Config file.  Connection is tha actual name portion of the name value pair
            Dim objWebConfig As New clsWebConfig()
            strConnectionString = objWebConfig.GetWebConfig("Connection".ToString)

            ' Use the Connection string to instantiate a new Database class object.
            objData_DB = New clsData_DB(strConnectionString)

            ' Setup the parameters.  NOte that the Name, type and size must all match.  The final value is the paramter value
            objParams(0) = objData_DB.MakeInParam("@OwnerEmail ", SqlDbType.VarChar, 50, strLoggedInUser)
            objParams(1) = objData_DB.MakeInParam("@FriendEmail ", SqlDbType.VarChar, 50, ddAddAFriend.SelectedValue)

            Try
                ' Run the stored procedure by name.  Pass with it the parameter list.
                objDR = objData_DB.RunStoredProc("usp_CircleOfFriends_Insert", objParams)

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
        End If

        Response.Redirect("~/CircleOfFriend")
    End Sub

    Private Sub PopulateFriendDropDown(ByVal strEmail As String)
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
        objParams(0) = objData_DB.MakeInParam("@OwnerEmail ", SqlDbType.VarChar, 50, strEmail)

        ' Run the stored procedure by name.  Pass with it the parameter list.
        objDR = objData_DB.RunStoredProc("usp_CircleOfFriends_SelectAvailable", objParams)

        ' Do we have rows returned?
        If objDR.HasRows Then
            ' Clear the List before we populate
            Me.ddAddAFriend.Items.Clear()

            ' Iterate Through the DataReader and Populate the Listbox
            Me.ddAddAFriend.Items.Add(New ListItem(""))
            While objDR.Read()
                Me.ddAddAFriend.Items.Add(New ListItem(objDR("Email").ToString))
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
