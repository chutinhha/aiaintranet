<%@ Page Language="C#" Inherits="AIA.Intranet.Infrastructure.Pages.Settings, $SharePoint.Project.AssemblyFullName$" DynamicMasterPageFile="~masterurl/default.master" %> 
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
    <asp:Label ID="Label1" runat="server" Text="Set account to access AD" Font-Bold="True" CssClass="ms-rteElement-H1B"></asp:Label>
    <br><br>
    <asp:Label ID="lblMessage" runat="server" Text="Setting is saved successfuly. " Visible="False"></asp:Label>
    <asp:Label runat="server" ID = "ErrorMessage" CssClass="ms-error"></asp:Label>
    
    
    <table style="width: 610px; vertical-align:top" class="ms-v4propertysheetspacing">
        
        <tr >
            <td colspan=2 class="ms-sectionheader">
                <asp:Label ID="Label3" runat="server" Text="Account to access AD:"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:RadioButton ID="rbImpersonation" runat="server" Text="Impersonation" Visible="false" GroupName="authenticate"/>
                <asp:Label ID="lblUserName" runat="server" Text="Username"></asp:Label> &nbsp;
            </td>
            <td>
                <asp:RadioButton ID="rbSpecificUser" runat="server" Text="Use this account" GroupName="authenticate" Checked="true" Visible="false"/>
                <asp:TextBox ID="txtUsername" runat="server" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
               <asp:Label ID="lblPassWord" runat="server" Text="Password"></asp:Label> &nbsp;
            </td>
            <td>
               <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td align="right">
               <asp:Label ID="Label2" runat="server" Text="LDAP://"></asp:Label> &nbsp;
            </td>
            <td>
               <asp:TextBox ID="txtLDAP" runat="server" TextMode="SingleLine" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="left">
                <asp:Button ID="btnSave" runat="server" Text="Save Settings" OnClick="btnSave_Click"/>
            </td>
        </tr>
        
    </table>

</asp:Content>
