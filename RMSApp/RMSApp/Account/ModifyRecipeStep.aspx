<%@ Page Title="ModifyRecipeStep" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModifyRecipeStep.aspx.vb" Inherits="RMSApp.ModifyRecipeStep" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Modify Recipe Step</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Modify Recipe Step For:&nbsp;&nbsp;&nbsp;&nbsp;<b><%=strLoggedInUser %></b> </h4>
        <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="#CC0000">Required fields are marked with an asterisk *</asp:Label>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />

        <asp:HiddenField ID="hdnStepID" runat="server" />
        <asp:HiddenField ID="hdnLoggedInUser" runat="server" />
        <asp:HiddenField ID="hdnRecipeID" runat="server" />

        <asp:Table ID="Table1" runat="server" Width="100%" GridLines="None" >
            <asp:TableRow>
                <asp:TableCell Width="20%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="80%">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Center">
                    <b>Step Instruction:</b>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Center">
                    <asp:TextBox runat="server" ID="txtStepText" CssClass="form-control" TextMode="MultiLine" ReadOnly="false" MaxLength="1000" Rows="5" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtStepText" CssClass="text-danger" ErrorMessage="Recipe Step Text is Required." />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                    <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="SaveChanges_Click" CssClass="btn btn-primary btn-md" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="btn btn-primary btn-md" OnClick="CancelChanges_Click"/>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>
