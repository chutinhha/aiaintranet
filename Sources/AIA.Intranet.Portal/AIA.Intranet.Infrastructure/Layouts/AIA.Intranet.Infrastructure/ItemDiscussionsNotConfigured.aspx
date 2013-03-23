<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Page Language="C#" AutoEventWireup="true"  Inherits="Microsoft.SharePoint.WebControls.LayoutsPageBase" DynamicMasterPageFile="~masterurl/default.master" %>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
  <asp:Label ID="Label1" Text="Item discussions have been disabled, or are not properly configured on this list." runat="server" ></asp:Label>
</asp:Content>