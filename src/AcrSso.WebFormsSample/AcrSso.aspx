<%@ Page Title="ACR SSO" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AcrSso.aspx.cs" Inherits="AcrSso.WebFormsSample.AcrSso" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>This is a sample page that demonstrates how to initiate login process with ACR SSO.</h3>
    <br />
    <h4>Click <asp:LinkButton Text="Sign in" runat="server" OnClick="SignIn_Click" /> to login with ACR SSO.</h4>
    <br />
    <br />
    <asp:Panel ID="TokensPanel" runat="server">
        <h4>Access Token:</h4>
        <asp:Label ID="AccessToken" runat="server" />
        <h4>ID Token:</h4>
        <asp:Label ID="IdToken" runat="server" />
        <h4>Refresh Token:</h4>
        <asp:Label ID="RefreshToken" runat="server" />
    </asp:Panel>
</asp:Content>
