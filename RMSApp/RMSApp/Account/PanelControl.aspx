<%@ Page Title="Panel Control Sample" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="PanelControl.aspx.vb" Inherits="RMSApp.PanelControl" %>

<%@ Import Namespace="RMSApp" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>

    <div>
        <asp:PlaceHolder runat="server" ID="SuccessMessagePlaceHolder" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"></p>
        </asp:PlaceHolder>
    </div>

    <div class="row">
        <asp:Table ID="tblDirections" runat="server">
            <asp:TableRow>
                <asp:TableCell>Recipe Directions:</asp:TableCell>
                <asp:TableCell><asp:Button ID="btnAddStep" Text="Add A Step" OnClick="AddAStep" runat="server"/></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="100%" ColumnSpan="2">
                    <asp:Panel ID="pnlDirections" runat="server" BorderWidth="4" Width="300" >
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table> 
    </div>
</asp:Content>

