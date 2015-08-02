<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="TestWebApplication.Account.Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>
    <p class="text-danger">
        <asp:Literal runat="server" ID="ErrorMessage" />
    </p>

    <div class="form-horizontal">
        <h4>Create a new account</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DisplayName" CssClass="col-md-2 control-label">Display Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="DisplayName" CssClass="form-control" Text="Hemanth"/>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DateOfBirth" CssClass="col-md-2 control-label">Date of Birth</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="DateOfBirth" CssClass="form-control" TextMode="Date" Text="12/23/2001"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="DateOfBirth"
                    CssClass="text-danger" ErrorMessage="The Date Of Birth field is required." />
                <%--<asp:RegularExpressionValidator ID="regexpDate" runat="server" ControlToValidate="DateOfBirth" 
                    ErrorMessage="Invalid Date Format" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>--%>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" Text="hemhedge@gmail.com"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                    CssClass="text-danger" ErrorMessage="The email field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" Text="hemanth"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="text-danger" ErrorMessage="The password field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ZipCode" CssClass="col-md-2 control-label">Zip Code</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ZipCode" CssClass="form-control" Text="98007"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ZipCode"
                    CssClass="text-danger" ErrorMessage="The ZipCode field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Country" CssClass="col-md-2 control-label">Country</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Country" CssClass="form-control" Text="USA"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Country"
                    CssClass="text-danger" ErrorMessage="The Country field is required." />
            </div>
        </div>
        <%--<div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Confirm password</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control"  Text="hemanth"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
            </div>
        </div>--%>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-default" />
            </div>
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="AddUserPreference_Click" Text="Add Preference" CssClass="btn btn-default" />
            </div>
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="RemoveUserPreference_Click" Text="Remove Preference" CssClass="btn btn-default" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Label runat="server" AssociatedControlID="ProfilePictureUpload" CssClass="col-md-2 control-label">Choose profile picture to upload</asp:Label>
                <asp:FileUpload ID="ProfilePictureUpload" runat="server" CssClass="form-control" AllowMultiple="false" />
                <asp:Button runat="server" OnClick="UploadProfilePicture" Text="Upload Profile Picture" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
