<%@ Page Title="FAQ" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FAQ.aspx.vb" Inherits="RMSApp.FAQ" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <p>RMS Frequenty Asks Questions</p>

        <asp:Table ID="Table1" runat="server" Width="100%" GridLines="None" >
            <asp:TableRow>
                <asp:TableCell Width="10%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="10%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="10%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="10%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="10%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="10%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="10%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="10%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="10%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="10%">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Center" VerticalAlign="Middle">
                    <asp:Button ID="btnFAQ1" runat="server" Text="+" OnClick="FAQ1_Click" />
                </asp:TableCell>
                <asp:TableCell ColumnSpan="9" HorizontalAlign="Left" VerticalAlign="Middle">
                    <b><i>What Is RMS?</i></b>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
                <asp:TableCell ColumnSpan="9">
                    <asp:TextBox ID="txtFAQ1" runat="server" Width="100%" TextMode="MultiLine" Rows="6" ReadOnly="true" Visible="false"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="10"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1" HorizontalAlign="Center" VerticalAlign="Middle">
                    <asp:Button ID="btnFAQ2" runat="server" Text="+" OnClick="FAQ2_Click" />
                </asp:TableCell>
                <asp:TableCell ColumnSpan="9" HorizontalAlign="Left" VerticalAlign="Middle">
                    <b><i>Why Do I Need to Register to use RMS?</i></b>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
                <asp:TableCell ColumnSpan="9">
                    <asp:TextBox ID="txtFAQ2" runat="server" Width="100%" TextMode="MultiLine" Rows="6" ReadOnly="true" Visible="false"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="10"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="10" HorizontalAlign="Center" VerticalAlign="Middle">
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="Return_Click" Text="Return to Home Page" CssClass="btn btn-primary btn-md" ID="btnReturn" />
                        </div>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
</asp:Content>
