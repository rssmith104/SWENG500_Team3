<%@ Page Title="SubmitRating" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SubmitRating.aspx.vb" Inherits="RMSApp.SubmitRating" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>RMS Recipe Rating Submission</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Recipe Rating Submission For:&nbsp;&nbsp;&nbsp;&nbsp;<b><%=strLoggedInUser %></b> </h4>
        <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="#CC0000">Required fields are marked with an asterisk *</asp:Label>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />

        <asp:HiddenField ID="hdnRecipeID" runat="server" />
        <asp:HiddenField ID="hdnLoggedInUser" runat="server" />

        <asp:Table ID="Table1" runat="server" Width="100%" GridLines="None" >
            <asp:TableRow>
                <asp:TableCell Width="20%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="80%">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Right"><b>Rating Recipe:</b>&nbsp;</asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Left">
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblRecipeName" runat="server" Text=""></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Right"><b>By:</b>&nbsp;</asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Left">
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblRecipeOwner" runat="server" Text=""></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Right"><b>Rating Submitter:</b>&nbsp;</asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Left">
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblRatingSubmitter" runat="server" Text=""></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                 <asp:TableCell HorizontalAlign="Right" ColumnSpan="1" ForeColor="#CC0000">
                        <b>Rating:*&nbsp;</b><br /><i>(0 to 5)<br />0=Lowest; 5=Highest</i>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" ColumnSpan="1">&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddRatings" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddRatings" CssClass="text-danger" ErrorMessage="A Rating Value is required." />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                 <asp:TableCell HorizontalAlign="Right" ColumnSpan="1" ForeColor="#CC0000">
                        <b>Rating Comment:*&nbsp;</b>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                    <asp:TextBox runat="server" ID="txtRatingComment" CssClass="form-control" TextMode="MultiLine" ReadOnly="false" MaxLength="1000" Rows="5" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRatingComment" CssClass="text-danger" ErrorMessage="The Rating Comment field is required." />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit Rating" OnClick="SaveChanges_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>
