<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="TestWebApplication.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="form-horizontal">
        <h4>User Details</h4>
        <hr />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DisplayName" CssClass="col-md-2 control-label">Display Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="DisplayName" CssClass="form-control"/>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DateOfBirth" CssClass="col-md-2 control-label">Date of Birth</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="DateOfBirth" CssClass="form-control" TextMode="Date"/>

            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Email</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" Text="hemhedge@gmail.com"/>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Password" CssClass="form-control"/>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ZipCode" CssClass="col-md-2 control-label">Zip Code</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ZipCode" CssClass="form-control"/>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Country" CssClass="col-md-2 control-label">Country</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Country" CssClass="form-control"/>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Image ID="ProfilePicture" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="GetUser_Click" Text="Get User" CssClass="btn btn-default" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="GetStories_Click" Text="Get Stories for User" CssClass="btn btn-default" />
            </div>
        </div>
    </div>
</asp:Content>
