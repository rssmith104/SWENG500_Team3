<%@ Page Title="AddRecipe" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AddRecipe.aspx.vb" Inherits="RMSApp.AddRecipe" %>
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

        <asp:Table ID="Table1" runat="server">
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="2">
                     <div class="col-md-10" > 
                        <asp:Label runat="server" AssociatedControlID="RecipeTitle" CssClass="col-md-2 control-label">Recipe Title</asp:Label>
                    </div>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="2">
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="RecipeTitle" CssClass="form-control" TextMode="SingleLine" ReadOnly="false"/>
                    </div> 
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="2">
                  <!-- <div class="col-md-10">-->
                        <asp:Label runat="server" AssociatedControlID="RecipeServingSize" CssClass="col-md-2 control-label">Recipe Serving Size</asp:Label>
                    <!--</div>-->
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="2">
                  <!-- <div class="col-md-10">-->
                        <asp:DropDownList runat="server" ID="RecipeServingSize" CssClass="form-control" ReadOnly="false"/>
                    <!--</div>-->
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
               <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="2">
                    <div class="col-md-10">
                        <asp:Label runat="server" AssociatedControlID="ddRecipeCategory" CssClass="col-md-2 control-label">Category</asp:Label>
                    </div>
                </asp:TableCell>
                 <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="2">
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddRecipeCategory" CssClass="form-control" TextMode="SingleLine" ReadOnly="false"/>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="2">
                    <div class="col-md-10">
                        <asp:Label runat="server" AssociatedControlID="txtRecipeSearch" CssClass="col-md-2 control-label">Search Keywords</asp:Label>
                    </div>
                </asp:TableCell>
                  <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="2">
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtRecipeSearch" CssClass="form-control" TextMode="SingleLine" ReadOnly="false"/>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="2">
                    <div class="col-md-10">
                        <asp:Label runat="server" AssociatedControlID="rbRecipeMeasurement" CssClass="col-md-2 control-label">Measurement System</asp:Label>
                    </div>
                </asp:TableCell>
                  <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="2">
                    <div class="col-md-10">
                        <asp:RadioButtonList id="rbRecipeMeasurement" runat="server">
                            <asp:ListItem>US</asp:ListItem>
                            <asp:ListItem>Metric</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                        <asp:TableCell>Ingredients</asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" ColumnSpan="3"><asp:Button ID="Button1" Text="+" OnClick="AddIngredients" runat="server"/></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="100%" ColumnSpan="4">
                    <asp:Panel ID="pnlIngredients" runat="server" BorderWidth="2" Width="100%" >
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell>Directions:</asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" ColumnSpan="3"><asp:Button ID="btnAddStep" Text="+" OnClick="AddAStep" runat="server"/></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="100%" ColumnSpan="4">
                    <asp:Panel ID="pnlDirections" runat="server" BorderWidth="2" Width="100%" >
                    </asp:Panel>
                </asp:TableCell>
            </asp:TableRow>
            </asp:Table>
        <asp:Table ID="Table2" runat="server">
           
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4" HorizontalAlign="Center">
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="SaveChanges_Click" Text="Save Changes" CssClass="btn btn-primary btn-lg" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button runat="server" OnClick="SaveChanges_Click" Text="Cancel Changes" CssClass="btn btn-primary btn-lg" />
                        </div>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div></asp:Content>