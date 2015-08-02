<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="TestWebApplication.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="form-horizontal">
        <h4>Create a new Story</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Heading" CssClass="col-md-2 control-label">Heading</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Heading" CssClass="form-control" Text="How Deforestation in south central america is causing global warming" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="CreatedDate" CssClass="col-md-2 control-label">Created Date</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="CreatedDate" CssClass="form-control" TextMode="Date" Text="12/23/2001" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="CreatedDate"
                    CssClass="text-danger" ErrorMessage="The CreatedDate field is required." />
                <%--<asp:RegularExpressionValidator ID="regexpDate" runat="server" ControlToValidate="DateOfBirth" 
                    ErrorMessage="Invalid Date Format" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>--%>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="OwnerId" CssClass="col-md-2 control-label">OwnerId</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="OwnerId" CssClass="form-control" TextMode="Email" Text="hemhedge@gmail.com" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="OwnerId"
                    CssClass="text-danger" ErrorMessage="The OwnerId field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Message" CssClass="col-md-2 control-label">Message</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Message" CssClass="form-control" Text="This is a very important issue to solve. and we will use strong measures to solve it." />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Message"
                    CssClass="text-danger" ErrorMessage="The password field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Category" CssClass="col-md-2 control-label">Story Category</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="Category" CssClass="form-control">
                    <asp:ListItem Text="Environmental" Value="Environmental"></asp:ListItem>
                    <asp:ListItem Text="Social" Value="Social"></asp:ListItem>
                    <asp:ListItem Text="Political" Value="Political"></asp:ListItem>
                    <asp:ListItem Text="Economical" Value="Economical"></asp:ListItem>
                    <asp:ListItem Text="Local" Value="Local"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="IsLocal" CssClass="col-md-2 control-label">Is issue local</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="IsLocal" CssClass="form-control">
                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Keywords" CssClass="col-md-2 control-label">Enter the keywords seperated by comma.</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Keywords" CssClass="form-control" Text="Environment, deforestation, south america, central america" />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ZipCode" CssClass="col-md-2 control-label">Zip Code</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ZipCode" CssClass="form-control" Text="98007" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ZipCode"
                    CssClass="text-danger" ErrorMessage="The ZipCode field is required." />
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="Country" CssClass="col-md-2 control-label">Country</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="Country" CssClass="form-control" Text="USA" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Country"
                    CssClass="text-danger" ErrorMessage="The Country field is required." />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Label runat="server" AssociatedControlID="MediaUpload"  CssClass="col-md-2 control-label">Choose video/picture to upload which communicates your message</asp:Label>
                <asp:FileUpload ID="MediaUpload" runat="server" CssClass="form-control" AllowMultiple="false" />
                <%--<asp:Button runat="server" OnClick="UploadMedia" Text="Upload Profile Picture" CssClass="btn btn-default" />--%>
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button runat="server" OnClick="CreateStory_Click" Text="Create Story" CssClass="btn btn-default" />
            </div>
        </div>
        
    </div>
</asp:Content>
