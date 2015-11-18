<%@ Page Title="RMS Add A Friends To My Circle" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AddAFriend.aspx.vb" Inherits="RMSApp.AddAFriend" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>RMS Circle of Friends - Add A Friend</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>&nbsp;&nbsp;&nbsp;&nbsp;Add A Friend for User:&nbsp;&nbsp;<b><asp:Label ID="lblCircleOwner" runat="server" Text=""></asp:Label></b> 
        &nbsp;&nbsp;<asp:Image runat="server" ID="imgProfile" width="150px" height="150px" /></h4>
        <hr />
        <asp:Table ID="Table1" runat="server" Width="100%" GridLines="None" >
            <asp:TableRow>
                <asp:TableCell Width="5%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="10%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="40%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="40%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="5%">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp;</asp:TableCell>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Center"><h4><b>Select Friend to Invite:</b></h4></asp:TableCell>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Left">
                    <asp:DropDownList ID="ddAddAFriend" runat="server"></asp:DropDownList></asp:TableCell>
                <asp:TableCell ColumnSpan="1">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="5">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp;</asp:TableCell>
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Center">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="Add_Click" CssClass="btn btn-primary btn-md" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnReturn" runat="server" Text="Return" OnClick="Return_Click" CssClass="btn btn-primary btn-md" />
                </asp:TableCell>
                <asp:TableCell ColumnSpan="1">&nbsp;</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>

</asp:Content>
