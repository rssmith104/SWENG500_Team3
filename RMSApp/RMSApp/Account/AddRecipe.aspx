﻿<%@ Page Title="AddRecipe" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AddRecipe.aspx.vb" Inherits="RMSApp.AddRecipe" %>
<%@ Import Namespace="RMSApp" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>Add Recipe</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    <div class="form-horizontal">
        <h4>Add Recipe for Account:     <b><%=strLoggedInUser %></b> </h4>
        <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="#CC0000">Required fields are marked with an asterisk *</asp:Label>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <asp:table runat="server"></asp:table>

        <asp:Table ID="Table1" runat="server" Width="100%">
            <asp:TableRow>
                <asp:TableCell Width="16%">&nbsp;</asp:TableCell>
                <asp:TableCell Width ="17%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="16%">&nbsp;</asp:TableCell>
                <asp:TableCell Width ="17%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="16%">&nbsp;</asp:TableCell>
                <asp:TableCell Width ="18%">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1">
                        <b>RECIPE TITLE:</b>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="RecipeTitle" CssClass="form-control" TextMode="SingleLine" ReadOnly="false" Width ="90%"/>
                    </div> 
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell ColumnSpan="6"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1">
                        <b>Recipe Serving Size</b>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="RecipeServingSize" CssClass="form-control" ReadOnly="false"/>
                     </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
               <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1">
                        <b>Category</b>
                </asp:TableCell>
                 <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddRecipeCategory" CssClass="form-control" TextMode="SingleLine" ReadOnly="false"/>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1">
                        <b>Search Keywords</b>
                </asp:TableCell>
                  <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtRecipeSearch" CssClass="form-control" TextMode="SingleLine" ReadOnly="false"/>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1">
                        <b>Measurement System</b>
                </asp:TableCell>
                 <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:RadioButtonList id="rbRecipeMeasurement" runat="server">
                            <asp:ListItem>US</asp:ListItem>
                            <asp:ListItem>Metric</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                 </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell><b>Ingredients</b></asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:Button ID="Button1" Text="+" OnClick="AddIngredients" runat="server"/>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="100%" ColumnSpan="6">
                    <asp:Panel ID="pnlIngredients" runat="server" BorderWidth="1" Width="100%" >
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell><b>Directions:</b></asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:Button ID="btnAddStep" Text="+" OnClick="AddAStep" runat="server"/>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="100%" ColumnSpan="6">
                    <asp:Panel ID="pnlDirections" runat="server" BorderWidth="1" Width="100%" >
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="6"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Right" VerticalAlign="Middle">
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="SaveChanges_Click" Text="Save Changes" CssClass="btn btn-primary btn-lg" />
                        </div>
                    </div>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Left" VerticalAlign="Middle">
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="SaveChanges_Click" Text="Cancel Changes" CssClass="btn btn-primary btn-lg" />
                        </div>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div></asp:Content>