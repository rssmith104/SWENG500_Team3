<%@ Page Title="DisplayRecipe" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="DisplayRecipe.aspx.vb" Inherits="RMSApp.DisplayRecipe" %>
<%@ Import Namespace="RMSApp" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>Display Recipe</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    <div class="form-horizontal">
        <h4>RMS RECIPE DISPLAY</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />

        <asp:Table ID="DisplayTable" runat="server" Width="100%" GridLines="Both">
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
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Center" ColumnSpan="2">
                        <b>Recipe Name:</b>&nbsp;&nbsp;
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="8">
                    <b><asp:Literal ID="ltRecipeName" runat="server"></asp:Literal></b>
                 </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
                <asp:TableCell ColumnSpan="1" VerticalAlign="Middle" HorizontalAlign="Left"><p>Submitted By:</p></asp:TableCell>
                <asp:TableCell ColumnSpan="1" VerticalAlign="Middle" HorizontalAlign="Left"><b><asp:Literal ID="ltOwner" runat="server"></asp:Literal></b></asp:TableCell>
                <asp:TableCell ColumnSpan="2" VerticalAlign="Middle" HorizontalAlign="Center"><i><asp:Literal ID="ltSubmissionDate" runat="server"></asp:Literal></i></asp:TableCell>
                <asp:TableCell ColumnSpan="1" VerticalAlign="Middle" HorizontalAlign="Right"><p><i>Rating:&nbsp;</i></p></asp:TableCell>
                <asp:TableCell ColumnSpan="1" VerticalAlign="Middle" HorizontalAlign="Center"><asp:Image ID="imgRating" runat="server" Width="130px" Height="25px"/></asp:TableCell>
                <asp:TableCell ColumnSpan="1" VerticalAlign="Middle" HorizontalAlign="Center"><p><asp:Literal ID="ltRating" runat="server"></asp:Literal></p></asp:TableCell>
                <asp:TableCell ColumnSpan="2" VerticalAlign="Middle" HorizontalAlign="Center"><asp:HyperLink ID="hypReadReviews" runat="server">Read Reviews</asp:HyperLink></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="10" VerticalAlign="Middle" HorizontalAlign="Center"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" VerticalAlign="Middle" HorizontalAlign="Center">&nbsp;</asp:TableCell>
                <asp:TableCell ColumnSpan="8" VerticalAlign="Middle" HorizontalAlign="Left"><asp:Image ID="imgRecipeImage" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
                <asp:TableCell ColumnSpan="1" VerticalAlign="Middle" HorizontalAlign="Left"><p>Recipe Description:&nbsp;</p></asp:TableCell>
                <asp:TableCell ColumnSpan="7" VerticalAlign="Middle" HorizontalAlign="Left"><asp:Literal ID="ltRecipeDescription" runat="server"></asp:Literal></asp:TableCell>
                <asp:TableCell ColumnSpan="1" VerticalAlign="Middle" HorizontalAlign="Center">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
                <asp:TableCell ColumnSpan="1" VerticalAlign="Middle" HorizontalAlign="Left"><p>Serving Size:</p></asp:TableCell>
                <asp:TableCell ColumnSpan="1" VerticalAlign="Middle" HorizontalAlign="Center"><asp:Literal ID="ltServingSize" runat="server"></asp:Literal></asp:TableCell>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
                <asp:TableCell ColumnSpan="6" VerticalAlign="Middle" HorizontalAlign="Left"><asp:Button ID="btnServingSizeAdjustment" runat="server" Text="Adjust Serving Size" CssClass="btn btn-primary btn-sm" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="10" VerticalAlign="Middle" HorizontalAlign="Center"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
                <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="1"><b>Ingredients:</b>&nbsp;&nbsp;</asp:TableCell>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="8"><asp:Panel ID="pnlIngredients" runat="server" BorderWidth="1px"></asp:Panel></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="10" VerticalAlign="Middle" HorizontalAlign="Center">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="1">&nbsp</asp:TableCell>
                <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="1"><b>Instructions:</b>&nbsp;&nbsp;</asp:TableCell>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="8"><asp:Panel ID="pnlInstructions" runat="server" BorderWidth="1px"></asp:Panel></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="10" VerticalAlign="Middle" HorizontalAlign="Center">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="3">&nbsp</asp:TableCell>
                <asp:TableCell ColumnSpan="2" VerticalAlign="Middle" HorizontalAlign="Center"><asp:Button ID="btnReview" runat="server" Text="Review Recipe" CssClass="btn btn-primary btn-lg" /></asp:TableCell>
                <asp:TableCell ColumnSpan="2" VerticalAlign="Middle" HorizontalAlign="Center"><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-lg" /></asp:TableCell>
                <asp:TableCell ColumnSpan="3">&nbsp</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div></asp:Content>