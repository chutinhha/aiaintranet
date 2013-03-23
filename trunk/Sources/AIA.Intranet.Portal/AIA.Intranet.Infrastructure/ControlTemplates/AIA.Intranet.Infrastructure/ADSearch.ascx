<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ADSearch.ascx.cs" Inherits="AIA.Intranet.Infrastructure.ControlTemplates.Search, $SharePoint.Project.AssemblyFullName$" %>

<link rel="stylesheet" href="./css/application.css">
<asp:Label ID="Label1" runat="server" Text="Enter User Name:"></asp:Label>
<asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />

<br>
<br>
<asp:Label ID="Label2" runat="server" Text="Result(s):"></asp:Label>
<br>
<br>
<asp:GridView ID="grMember" runat="server" AutoGenerateColumns="False" SelectedRowStyle-BackColor="#CCCCCC" Width="800px" HeaderStyle-CssClass="GridHeader">
    <EmptyDataTemplate>
        <asp:Label ID="lblEmpty" runat="server">No Results Found</asp:Label>
    </EmptyDataTemplate>
    <Columns>
        <asp:BoundField DataField="LoginName" HeaderText="LogIn Name" />
        <asp:BoundField DataField="DisplayName" HeaderText="Display Name" />
        <asp:boundfield datafield="Path" headertext="Distinguished Name" ItemStyle-CssClass="hideContent" HeaderStyle-CssClass="hideContent"/>
        <asp:BoundField DataField="EmailAddress" HeaderText="Email" />
        <asp:TemplateField ItemStyle-Width="5px">
            <ItemTemplate>
                <asp:CheckBox ID="isSelect" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<br>
<asp:Button ID="btnSelect" runat="server" Text="Add To Group" 
    onclick="btnSelect_Click" Width="100px"/>
<asp:Button ID="btnAddNewUser" runat="server" Text="Add New User" onclick="btnAddNewUser_Click" Width="100px"/>
<asp:Button ID="btnCancel" runat="server" Text="Cancel" 
    onclick="btnCancel_Click" Width="100px"/>