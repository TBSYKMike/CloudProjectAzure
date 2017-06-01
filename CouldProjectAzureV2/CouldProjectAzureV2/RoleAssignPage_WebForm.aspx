<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleAssignPage_WebForm.aspx.cs" Inherits="CouldProjectAzureV2.Account.RoleAssignPage_WebForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
          <asp:DropDownList ID="RoleDropDownList" runat="server" style="margin-left: 2px"  AutoPostBack="True" OnSelectedIndexChanged="RoleDropDownList_SelectedIndexChanged" >
        </asp:DropDownList>
      <asp:Button ID="roleAssignButton" runat="server" Font-Bold="True" Font-Size="Larger" Text="Assign role to user" Width="234px" Height="38px" OnClick="roleAssignButton_Click" />
</asp:Content>
