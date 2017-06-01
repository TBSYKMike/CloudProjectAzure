<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateRole_WebForm.aspx.cs" Inherits="CouldProjectAzureV2.CreateRole_WebForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <asp:Label ID="Label2" runat="server" Height="39px" Text="Role Name" Width="150px"></asp:Label>
     <asp:TextBox ID="roleNameTextBox" runat="server" Height="43px" Width="313px"></asp:TextBox>
    <asp:Button ID="createRoleButton" runat="server" Font-Bold="True" Font-Size="Larger" Text ="Create Role" Width="343px" OnClick="createRoleButton_Click" />
</asp:Content>
