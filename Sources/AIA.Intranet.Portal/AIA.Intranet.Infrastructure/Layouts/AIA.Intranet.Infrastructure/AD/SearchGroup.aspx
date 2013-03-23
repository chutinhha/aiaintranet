<%@ Page Language="C#" Inherits="AIA.Intranet.Infrastructure.Pages.SearchGroup, $SharePoint.Project.AssemblyFullName$" DynamicMasterPageFile="~masterurl/default.master" EnableSessionState="True" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> 
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="AIA.Intranet.Infrastructure.Pages" %>
<%@ Register src="~/_controltemplates/AIA.Intranet.Infrastructure/ADSearch.ascx" tagname="Search" tagprefix="uc1" %>

<asp:Content ID="Content1" contentplaceholderid="PlaceHolderPageTitle" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderId="PlaceHolderMain" runat="server">
<link rel="stylesheet" href="./css/application.css">
    <asp:Label ID="Label1" runat="server" Text="Enter Group Name:"></asp:Label>
    <asp:TextBox ID="txtGroupName" runat="server"></asp:TextBox>
    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />

    <br>
    <br>
    <asp:Label ID="Label2" runat="server" Text="Result(s):"></asp:Label>
    <br>
    <br>
    <asp:GridView ID="grGroup" runat="server" AutoGenerateColumns="False" SelectedRowStyle-BackColor="#CCCCCC" Width="800px" HeaderStyle-CssClass="GridHeader">
        <EmptyDataTemplate>
            <asp:Label ID="lblEmpty" runat="server">No Results Found</asp:Label>
        </EmptyDataTemplate>
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Group Name" ItemStyle-Width="200px" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Path" HeaderText="Distinguished Name" ReadOnly="True" ItemStyle-CssClass="hideContent" HeaderStyle-CssClass="hideContent"/>
            <asp:TemplateField ItemStyle-Width="5px">
                <ItemTemplate>
                    <asp:CheckBox ID="isSelect" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br>
    <asp:Button ID="btnSelect" runat="server" Text="Add" OnClick="btnSelect_Click" Width="100px"/>
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Width="100px" CausesValidation="False" />
</asp:Content>
