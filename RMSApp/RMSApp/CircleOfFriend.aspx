<%@ Page Title="RMS Circle of Friends" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CircleOfFriend.aspx.vb" Inherits="RMSApp.CircleOfFriends" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>RMS Circle of Friends</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>&nbsp;&nbsp;&nbsp;&nbsp;Circle for User:&nbsp;&nbsp;<b><asp:Label ID="lblCircleOwner" runat="server" Text=""></asp:Label></b> 
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
                <asp:TableCell ColumnSpan="5" HorizontalAlign="Left"><h4><b>My Friends</b>&nbsp;&nbsp;
                    <asp:Button ID="btnInviteAFriend" runat="server" Text="Invite A Friend" ToolTip="Invite A Friend" CssClass="btn btn-primary btn-sm" OnClick="AddAFriend_Click" /></h4></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp;</asp:TableCell>
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Left"><asp:Panel ID="pnlFriends" runat="server" BorderWidth="1" Width="100%" /></asp:TableCell>
                <asp:TableCell ColumnSpan="1">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="5">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="5" HorizontalAlign="Left"><h4><b>My Invites</b></h4></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp;</asp:TableCell>
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Left"><asp:Panel ID="pnlInvites" runat="server" BorderWidth="1" Width="100%" /></asp:TableCell>
                <asp:TableCell ColumnSpan="1">&nbsp;</asp:TableCell>
            </asp:TableRow> 
            <asp:TableRow>
                <asp:TableCell ColumnSpan="5" HorizontalAlign="Left">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp;</asp:TableCell>
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Center"><asp:Button ID="btnReturn" runat="server" Text="Return" OnClick="Return_Click" CssClass="btn btn-primary btn-md" /></asp:TableCell>
                <asp:TableCell ColumnSpan="1">&nbsp;</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>

</asp:Content>
