<%@ Page Title="Search" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Search.aspx.vb" Inherits="RMSApp.Search" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>RMS Search</h2>

    <div class="row">
        <div class="col-md-8">
            <asp:Table Width="100%" runat="server" BorderWidth="1px">

                <asp:TableRow runat="server" BorderWidth="1px">
                    <asp:TableCell runat="server" BorderWidth="1px">
                        <b>Search for a new recipe or an old favorite!</b>
                    </asp:TableCell>
                    <asp:TableCell runat="server" BorderWidth="1px">
                        <b>Category</b>
                    </asp:TableCell>
                    <asp:TableCell>
                        <b>Privacy</b>
                    </asp:TableCell>
                    <asp:TableCell runat="server" BorderWidth="1px">
                        &nbsp;
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow runat="server" BorderWidth="1px">
                    <asp:TableCell Width="100%" runat="server" BorderWidth="1px">
                        <asp:TextBox ID="txtSearchBox" Width="100%" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell runat="server" BorderWidth="1px">
                        <asp:DropDownList ID="ddCategoryList" runat="server"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell runat="server" BorderWidth="1px">
                        <asp:DropDownList ID="ddShareList" runat="server"></asp:DropDownList>
                    </asp:TableCell>
                    <asp:TableCell runat="server" BorderWidth="1px">
                        <asp:Button ID="btnSearchButton" runat="server" Text="Search" CssClass="btn btn-primary btn-sm" OnClick="SearchRecipe" />
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3">
                        <h2>RMS Results</h2>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3">
                        &nbsp;
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell ColumnSpan="3">
                        <asp:Panel ID="pnlSearch" runat="server" BorderWidth="2" Width="100%" >
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>

            </asp:Table>
        </div>

        <hr />
        
    </div>
</asp:Content>
