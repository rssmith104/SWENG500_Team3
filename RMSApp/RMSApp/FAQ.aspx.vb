Public Class FAQ
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub FAQ1_Click(sender As Object, e As EventArgs)
        If Me.txtFAQ1.Visible = False Then
            Me.txtFAQ1.Visible = True
            Me.txtFAQ1.Text = "RMS stands for Recipe Management System.  Preserve all those family favorite recipes in a safe, digital format for family and friends to share." & vbCrLf &
                "Recipes are easily entered using the RMS patented Ingreditent and Instruction Contorls."
            Me.btnFAQ1.Text = "-"
        Else
            Me.txtFAQ1.Visible = False
            Me.btnFAQ1.Text = "+"
        End If

    End Sub

    Protected Sub FAQ2_Click(sender As Object, e As EventArgs)
        If Me.txtFAQ2.Visible = False Then
            Me.txtFAQ2.Visible = True
            Me.txtFAQ2.Text = "RMS is a free web site.  We do not charge for its use.  However, we do request your email address so that we might exploit your account " & vbCrLf &
                " through sending ridiculous amounts of SPAM mail and selling your email and personal information to anyone who may wish to buy it."
            Me.btnFAQ2.Text = "-"
        Else
            Me.txtFAQ2.Visible = False
            Me.btnFAQ2.Text = "+"
        End If

    End Sub

    Protected Sub Return_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/Authenticated_Default")
    End Sub




End Class