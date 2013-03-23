<%@ Page Language="C#" Inherits="AIA.Intranet.Infrastructure.Pages.AllGroups, $SharePoint.Project.AssemblyFullName$" DynamicMasterPageFile="~masterurl/default.master" %> 
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
    <asp:Label ID="Label1" runat="server" Text="Active Directory Groups Management" Font-Bold="True" CssClass="ms-rteElement-H1B"></asp:Label>
    <br><br>
    <asp:Label runat="server" ID = "ErrorMessage" CssClass="ms-error"></asp:Label>

    <br>
    <asp:GridView ID="grGroups" runat="server" AutoGenerateColumns="False" OnRowDataBound="gr_RowDataBound" Width ="800px" HeaderStyle-CssClass="GridHeader">
        <EmptyDataTemplate> 
                <asp:Label ID="lblEmpty" runat="server">No Results Found</asp:Label> 
        </EmptyDataTemplate> 
        <columns>
          <asp:boundfield datafield="Name" headertext="Group Name" ItemStyle-Width="200px" />
          <asp:boundfield datafield="Description" headertext="Description" ItemStyle-Width="300px" />
          <asp:boundfield datafield="Path" headertext="Distinguished Name" ReadOnly="True" ItemStyle-CssClass="hideContent" HeaderStyle-CssClass="hideContent" />
        </columns>
    </asp:GridView>
    <br>
    <asp:Button ID="btnAdd" runat="server" Text="Add New Group" OnClick="btnAdd_Click" Width="110px"/>
    <br>
    <br>
    <!--<asp:LinkButton ID="btnManageUser" runat="server" OnClick="btnManageUser_Click">Manage Users</asp:LinkButton>-->
</asp:Content>
