<%@ Page Title="DisplayReview" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayReview.aspx.vb" Inherits="RMSApp.DisplayReview" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>RMS Recipe Reviews</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    
    <div class="form-horizontal">
        <h4>Recipe Rating Reviews For Recipe: <b>
            <asp:Label ID="lblRecipeName" runat="server" Text=""></asp:Label></b> </h4>
        <h4>Recipe Owner: <b><asp:Label ID="lblRecipeOwner" runat="server" Text=""></asp:Label></b> </h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />

        <asp:HiddenField ID="hdnRecipeID" runat="server" />
        <asp:HiddenField ID="hdnLoggedInUser" runat="server" />

        <asp:Table ID="Table1" runat="server" Width="100%" GridLines="Both" >
            <asp:TableRow>
                <asp:TableCell Width="5%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="30%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="30%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="30%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="5%">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Left"><b>Recipe Reviews</b></asp:TableCell>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Left">
                    <asp:Panel ID="pnlReviews" runat="server" BorderWidth="1"></asp:Panel>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="5">&nbsp</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                    <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="OK_Click" CssClass="btn btn-primary btn-lg" />
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>
