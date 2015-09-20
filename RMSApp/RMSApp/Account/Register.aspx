<%@ Page Title="Register" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Register.aspx.vb" Inherits="RMSApp.Register" %>

<%@ Import Namespace="RMSApp" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>RMS Account Registration</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    <div class="form-horizontal" style="background-image: url('/Images/vegetable-stock-recipe.jpg')">
        <h4>Create a new account</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <asp:table runat="server"></asp:table>

        <asp:Table ID="Table1" runat="server">
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                             CssClass="text-danger" ErrorMessage="The email field is required." />
                    </div>
                </asp:TableCell><asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                </asp:TableCell><asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                            CssClass="text-danger" ErrorMessage="The password field is required." />
                    </div>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>&nbsp;</asp:TableCell><asp:TableCell>&nbsp;</asp:TableCell><asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Confirm Password</asp:Label>
                </asp:TableCell><asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                        <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
                    </div>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell ColSpan="4"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="FirstName" CssClass="col-md-2 control-label">First Name</asp:Label>
                </asp:TableCell><asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="FirstName" CssClass="form-control" />
                    </div>
                </asp:TableCell><asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="LastName" CssClass="col-md-2 control-label">Last Name</asp:Label>
                </asp:TableCell><asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="LastName" CssClass="form-control" />
                    </div>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="Phone" CssClass="col-md-2 control-label">Phone</asp:Label>
                </asp:TableCell><asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="Phone" CssClass="form-control" />
                    </div>
                </asp:TableCell><asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="Address" CssClass="col-md-2 control-label">Address</asp:Label>
                </asp:TableCell><asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="Address" CssClass="form-control" />
                    </div>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="City" CssClass="col-md-2 control-label">City</asp:Label>
                </asp:TableCell><asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="City" CssClass="form-control" />
                    </div>
                </asp:TableCell><asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="State" CssClass="col-md-2 control-label">State</asp:Label>
                </asp:TableCell><asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="State" CssClass="form-control" />
                    </div>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="Zip" CssClass="col-md-2 control-label">Zip</asp:Label>
                </asp:TableCell><asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="Zip" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Zip"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="The Zip Field is required." />
                    </div>
                </asp:TableCell><asp:TableCell VerticalAlign="Top">&nbsp;</asp:TableCell><asp:TableCell>&nbsp;</asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="SecurityQuestion" CssClass="col-md-2 control-label">Security Question</asp:Label>
                </asp:TableCell><asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="SecurityQuestion" CssClass="form-control" />
                    </div>
                </asp:TableCell><asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="SecurityResponse" CssClass="col-md-2 control-label">Security Response</asp:Label>
                </asp:TableCell><asp:TableCell>
                        <asp:TextBox runat="server" ID="SecurityResponse" CssClass="form-control" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4" HorizontalAlign="Center">
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-primary btn-lg" />
                        </div>
                    </div>
                </asp:TableCell></asp:TableRow></asp:Table></div></div></asp:Content>