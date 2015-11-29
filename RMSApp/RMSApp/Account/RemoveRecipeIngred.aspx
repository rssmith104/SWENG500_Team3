<%@ Page Title="RemoveRecipeIngred" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RemoveRecipeIngred.aspx.vb" Inherits="RMSApp.RemoveRecipeIngred" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Remove Recipe Ingredient</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Remove Ingredient For:&nbsp;&nbsp;&nbsp;&nbsp;<b><%=strLoggedInUser %></b> </h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />

        <asp:HiddenField ID="hdnIngredID" runat="server" />
        <asp:HiddenField ID="hdnLoggedInUser" runat="server" />
        <asp:HiddenField ID="hdnRecipeID" runat="server" />

        <asp:Table ID="Table1" runat="server" Width="100%" GridLines="None" >
            <asp:TableRow>
                <asp:TableCell Width="20%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="80%">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Right" VerticalAlign="Middle">
                    <b>Ingredient:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Left" >
                    <asp:Label runat="server" ID="txtIngredName" CssClass="form-control" ReadOnly="false" MaxLength="1000" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Right" VerticalAlign="Middle">
                    <b>Quantity:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Left" >
                    <asp:Label runat="server" ID="txtQuantity" CssClass="form-control" ReadOnly="false" MaxLength="3" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Right" VerticalAlign="Middle">
                    <b>Unit of Measure:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Left" >
                    <asp:Label runat="server" ID="ddUOM" CssClass="form-control" ReadOnly="false" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Right" VerticalAlign="Middle">
                    <b>Preparation:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Left" >
                    <asp:Label runat="server" ID="txtPrepText" CssClass="form-control" ReadOnly="false" MaxLength="100" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow><asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                    <asp:Button ID="btnSubmit" runat="server" Text="Remove" OnClick="Remove_Click" CssClass="btn btn-primary btn-md" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="btn btn-primary btn-md" OnClick="CancelChanges_Click"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>
