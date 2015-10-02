Public Class Logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("RMS_LoggedInUser") = ""
        Session("RMS_Authenticated") = "False"
        Session("RMS_Function") = "LOGOUT"
        Response.Redirect("/RMSApp/Default.aspx")
    End Sub

End Class