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

        <asp:Table ID="Table1" runat="server" Width="100%" GridLines="None" >
            <asp:TableRow>
                <asp:TableCell Width="16%">&nbsp;</asp:TableCell>
                <asp:TableCell Width ="17%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="16%">&nbsp;</asp:TableCell>
                <asp:TableCell Width ="17%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="16%">&nbsp;</asp:TableCell>
                <asp:TableCell Width ="18%">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1" ForeColor="#CC0000">
                        <b>Recipe Title*</b>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="RecipeTitle" CssClass="form-control" TextMode="MultiLine" ReadOnly="false" MaxLength="150" Rows="1" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="RecipeTitle"
                             CssClass="text-danger" ErrorMessage="The Recipe Title field is required." />
                    </div> 
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1">
                     <b>Recipe Description</b>
                    </asp:TableCell> 
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="RecipeDescription" CssClass="form-control" TextMode="MultiLine" ReadOnly="false" Rows="4" MaxLength="150" />
                    </div> 
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1">
                    <b>Approximate Cooking Time</b>
                    </asp:TableCell>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="RecipeCookingTime" CssClass="form-control" TextMode="SingleLine" ReadOnly="false" MaxLength="50" />
                    </div> 
                </asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell ColumnSpan="6"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1" ForeColor="#CC0000">
                        <b>Recipe Serving Size*</b>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="RecipeServingSize" CssClass="form-control" ReadOnly="false"/>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="RecipeServingSize"
                             CssClass="text-danger" ErrorMessage="The Recipe Serving Size field is required." />
                     </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
               <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1">
                        <b>Recipe Category</b>
                </asp:TableCell>
                 <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddRecipeCategory" CssClass="form-control" TextMode="SingleLine" ReadOnly="false"/>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
               <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1" ForeColor="#CC0000">
                   <b>Recipe Sharing*</b>
                   </asp:TableCell>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddRecipeSharing" CssClass="form-control" TextMode="SingleLine" ReadOnly="false" ForeColor="Black" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddRecipeSharing"
                             CssClass="text-danger" ErrorMessage="The Recipe Sharing field is required." />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="1">
                        <b>Search Keywords</b>
                </asp:TableCell>
                  <asp:TableCell VerticalAlign="Middle" HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtRecipeSearch" CssClass="form-control" TextMode="MultiLine" ReadOnly="false"/>
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
                        <asp:Button ID="Button1" Text="+" OnClick="AddIngredients" runat="server" CausesValidation="false"/>
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
                <asp:TableCell><b>Directions</b></asp:TableCell>
                <asp:TableCell HorizontalAlign="Left" ColumnSpan="5">
                    <div class="col-md-10">
                        <asp:Button ID="btnAddStep" Text="+" OnClick="AddAStep" runat="server" CausesValidation="false"/>
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
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Right">
                    <h3><b>Recipe Image:&nbsp</b></h3>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                    <asp:Image ID="imRecipeImage" runat="server" width="300px" Height="300px" ImageUrl="~/images/Default_Image.png"/>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Left">
                    <asp:FileUpload ID="fuRecipePicUpload" runat="server" /><br /><asp:Button ID="btnRecipeImageUpload" runat="server" Text="Upload Image" OnClick="Recipe_Image_Upload" CausesValidation="false"/>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="6"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Right" VerticalAlign="Middle">
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="SaveRecipeChanges_Click" Text="Save Changes" CssClass="btn btn-primary btn-lg" ID="btnSave" />
                        </div>
                    </div>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="3" HorizontalAlign="Left" VerticalAlign="Middle">
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="SaveRecipeChanges_Click" Text="Cancel Changes" CssClass="btn btn-primary btn-lg" />
                        </div>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div></asp:Content>