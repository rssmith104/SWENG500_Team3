Public Class Authenticated_Default
    Inherits Page

    Public strAuthenticated As String
    Public strLoggedInUser As String
    Public strFunction As String
    Public imgPath As String = System.Web.VirtualPathUtility.ToAbsolute("~/Images/ChickenStock.jpg")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Creates new session object and loads it from the stored session

        strAuthenticated = Session("RMS_Authenticated")
        strLoggedInUser = Session("RMS_LoggedInUser")
        strFunction = Session("RMS_Function")

    End Sub


End Class