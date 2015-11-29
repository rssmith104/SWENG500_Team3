<%@ Page Title="AddRecipeIngred" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddRecipeIngred.aspx.vb" Inherits="RMSApp.AddRecipeIngred" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Modify Recipe Ingredient</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Modify Ingredient For:&nbsp;&nbsp;&nbsp;&nbsp;<b><%=strLoggedInUser %></b> </h4>
        <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="#CC0000">Required fields are marked with an asterisk *</asp:Label>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />

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
                    <asp:TextBox runat="server" ID="txtIngredName" CssClass="form-control" ReadOnly="false" MaxLength="1000" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtIngredName" CssClass="text-danger" ErrorMessage="Recipe Ingredient is Required." />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Right" VerticalAlign="Middle">
                    <b>Quantity:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Left" >
                    <asp:TextBox runat="server" ID="txtQuantity" CssClass="form-control" ReadOnly="false" MaxLength="3" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="Ingredient Qty is Required." />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Right" VerticalAlign="Middle">
                    <b>Unit of Measure:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Left" >
                    <asp:DropDownList runat="server" ID="ddUOM" CssClass="form-control" ReadOnly="false" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Right" VerticalAlign="Middle">
                    <b>Preparation:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Left" >
                    <asp:TextBox runat="server" ID="txtPrepText" CssClass="form-control" ReadOnly="false" MaxLength="100" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow><asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                    <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="SaveChanges_Click" CssClass="btn btn-primary btn-md" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="btn btn-primary btn-md" OnClick="CancelChanges_Click"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>
