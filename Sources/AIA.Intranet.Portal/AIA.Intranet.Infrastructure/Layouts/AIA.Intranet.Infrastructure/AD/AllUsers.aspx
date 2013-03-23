<%@ Page Language="C#" Inherits="AIA.Intranet.Infrastructure.Pages.AllUsers, $SharePoint.Project.AssemblyFullName$" DynamicMasterPageFile="~masterurl/default.master" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> 
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="AIA.Intranet.Infrastructure.Pages" %>

<asp:Content ID="Content1" contentplaceholderid="PlaceHolderPageTitle" runat="server">
Manage Active Directory
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
Manage Active Directory
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderMain" runat="server">
<link rel="stylesheet" href="./css/application.css">
    <asp:Label ID="Label1" runat="server" Text="Active Directory Users Management" Font-Bold="True" CssClass="ms-rteElement-H1B"></asp:Label>
    <br><br>
    <asp:Label runat="server" ID = "ErrorMessage" CssClass="ms-error"></asp:Label>
    <br><br>
    <asp:Label ID="Label2" runat="server" Text="Enter User Name:"></asp:Label>
    <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
    <br>
    <br>
    
    <asp:GridView ID="grUsers" runat="server" AutoGenerateColumns="False" OnRowDataBound="gr_RowDataBound" Width ="1000px" HeaderStyle-CssClass="GridHeader">
        <EmptyDataTemplate> 
                <asp:Label ID="lblEmpty" runat="server">No Results Found</asp:Label> 
        </EmptyDataTemplate> 
        <columns>
            <asp:BoundField DataField="LoginName" HeaderText="LogIn Name" />
            <asp:BoundField DataField="DisplayName" HeaderText="Display Name" />
            <asp:BoundField DataField="Path" HeaderText="Distinguished Name" ItemStyle-CssClass="hideContent" HeaderStyle-CssClass="hideContent" />
            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
            <asp:BoundField DataField="LastName" HeaderText="Last Name" />
            <asp:BoundField DataField="Office" HeaderText="Office" />
            <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
        </columns>
    </asp:GridView>
    <br>
    <asp:Button ID="btnAdd" runat="server" Text="Add New User" OnClick="btnAdd_Click" Width="110px"/>
    <br>
    <br>
    <!--<asp:LinkButton ID="btnManageGroup" runat="server" OnClick="btnManageGroup_Click">Manage Groups</asp:LinkButton>-->

</asp:Content>
