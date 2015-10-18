Imports System
Imports System.Threading.Tasks
Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.EntityFramework
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports Owin

Partial Public Class PanelControl
    Inherits System.Web.UI.Page
    Protected Property SuccessMessage() As String
        Get
            Return m_SuccessMessage
        End Get
        Private Set(value As String)
            m_SuccessMessage = value
        End Set
    End Property
    Private m_SuccessMessage As String

    Public Property LoginsCount As Integer

    Protected Property NumberOfControls() As Integer
        Get
            Return m_NumberOfControls
        End Get
        Private Set(value As Integer)
            m_NumberOfControls = value
        End Set
    End Property
    Protected m_NumberOfControls As Integer

    Protected Sub Page_Load() Handles Me.Load
        If Not IsPostBack Then
            NumberOfControls = 0
            Session("RMS_NumberOfControls") = Me.NumberOfControls.ToString
        Else
            Me.CreateControls()
        End If
    End Sub

    Private Sub CreateControls()
        Me.NumberOfControls = Session("RMS_NumberOfControls")

        For iCounter As Integer = 1 To NumberOfControls()
            Dim objCtrl As TextBox = New TextBox()
            Dim objLabel As Label = New Label()

            objCtrl.ID = "Control_" & iCounter.ToString
            objLabel.ID = "Label_" & iCounter.ToString
            objLabel.Text = "Step " & iCounter.ToString & ":  "

            Me.pnlDirections.Controls.Add(objLabel)
            Me.pnlDirections.Controls.Add(objCtrl)
            Me.pnlDirections.Controls.Add(New LiteralControl("<br />"))

        Next
    End Sub


    Protected Sub AddAStep(sender As Object, e As EventArgs)

        Dim objCtrl As TextBox = New TextBox()
        Dim objLabel As Label = New Label()


        Me.NumberOfControls += 1
        Session("RMS_NumberOfControls") = Me.NumberOfControls.ToString

        objCtrl.ID = "Control_" & NumberOfControls.ToString
        objLabel.ID = "Label_" & NumberOfControls.ToString
        objLabel.Text = "Step " & NumberOfControls.ToString & ":  "

        Me.pnlDirections.Controls.Add(objLabel)
        Me.pnlDirections.Controls.Add(objCtrl)
        Me.pnlDirections.Controls.Add(New LiteralControl("<br />"))

    End Sub

End Class