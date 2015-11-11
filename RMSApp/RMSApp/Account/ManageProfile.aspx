<%@ Page Title="MangeProfile" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ManageProgile.aspx.vb" Inherits="RMSApp.ManageProfile" %>
<%@ Import Namespace="RMSApp" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>RMS Manage Profile</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>
    <div class="form-horizontal">
        <h4>Manage Profile for Account:     <b><%=strLoggedInUser %></b> </h4>
        <asp:Label ID="Label1" runat="server" Text="Label" ForeColor="#CC0000">Required fields are marked with an asterisk *</asp:Label>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <asp:table runat="server"></asp:table>

        <asp:Table ID="Table1" runat="server" GridLines="Both">
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top" HorizontalAlign="Center" ColumnSpan="4">
                    <div class="col-md-10">
                        <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
                        <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" ReadOnly="true"/>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="FirstName" CssClass="col-md-2 control-label">First Name</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="FirstName" CssClass="form-control"></asp:TextBox>
                    </div>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="LastName" CssClass="col-md-2 control-label">Last Name</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="LastName" CssClass="form-control" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="Phone" CssClass="col-md-2 control-label">Phone</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="Phone" CssClass="form-control" />
                    </div>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="Address" CssClass="col-md-2 control-label">Address</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="Address" CssClass="form-control" />
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="City" CssClass="col-md-2 control-label">City</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="City" CssClass="form-control" />
                    </div>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="ddState" CssClass="col-md-2 control-label">State</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddState" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="Zip" CssClass="col-md-2 control-label" ForeColor="#CC0000">Zip*</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="Zip" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Zip"
                            CssClass="text-danger" Display="Dynamic" ErrorMessage="The Zip Field is required." />
                    </div>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">&nbsp;</asp:TableCell>
                <asp:TableCell>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="ddSecurityQuestion" CssClass="col-md-2 control-label" ForeColor="#CC0000">Security Question*</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <div class="col-md-10">
                        <asp:DropDownList ID="ddSecurityQuestion" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                </asp:TableCell>
                <asp:TableCell VerticalAlign="Top">
                    <asp:Label runat="server" AssociatedControlID="SecurityResponse" CssClass="col-md-2 control-label" ForeColor="#CC0000">Security Response*</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                        <asp:TextBox runat="server" ID="SecurityResponse" CssClass="form-control" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                    <h3>Profile Pic</h3>&nbsp;&nbsp;&nbsp;<asp:Image ID="imProfileImage" runat="server" width="200px" Height="200px"  />
                </asp:TableCell>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                    <asp:FileUpload ID="fuProfilePicUpload" runat="server" /><br /><asp:Button ID="btnImageUpload" runat="server" Text="Image Upload" OnClick="Image_Upload" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4" HorizontalAlign="Center">
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button ID="btnSave" runat="server" OnClick="SaveChanges_Click" Text="Save Changes" CssClass="btn btn-primary btn-md" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" OnClick="CancelChanges_Click" Text="Cancel Changes" CssClass="btn btn-primary btn-md" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button runat="server" OnClick="ResetPassword_Click" Text="Reset Password" CssClass="btn btn-primary btn-md" />
                        </div>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div></asp:Content>