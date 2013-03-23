<%@ Page Language="C#" Inherits="AIA.Intranet.Infrastructure.Pages.SearchUser, $SharePoint.Project.AssemblyFullName$" DynamicMasterPageFile="~masterurl/default.master" EnableSessionState="True"%> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> 
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="AIA.Intranet.Infrastructure.Pages" %>
<%@ Register src="~/_controltemplates/AIA.Intranet.Infrastructure/Search.ascx" tagname="Search" tagprefix="uc1" %>

<asp:Content ID="Content1" contentplaceholderid="PlaceHolderPageTitle" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderMain" runat="server">
<asp:Label runat="server" ID = "ErrorMessage" CssClass="ms-error"></asp:Label>

   <uc1:Search ID="Search1" runat="server" />
</asp:Content>
