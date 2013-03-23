<%@ Page Language="C#" Inherits="AIA.Intranet.Infrastructure.Pages.EditGroup, $SharePoint.Project.AssemblyFullName$" DynamicMasterPageFile="~masterurl/default.master" EnableSessionState="True"%> 
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
    <script type="text/javascript">
        function OpenSearchPage() {
            var options = SP.UI.$create_DialogOptions();
            options.url = "SearchUser.aspx";
            options.width = 900;
            options.height = 600;
            options.dialogReturnValueCallback = CloseCallback;
            var dialogSP = SP.UI.ModalDialog.showModalDialog(options);
        }

        function CloseCallback(dialogResult, returnValue) {
            __doPostBack('__Page', '');
        }

        function ConfirmDelete() {
            if (confirm("Are you sure you want to delete?")) {
                return true;
            }
            return false;
        }
    </script>

    <asp:Label ID="lblTitle" runat="server" Text="Add New Active Directory Group" Font-Bold="True" CssClass="ms-rteElement-H1B"></asp:Label>
    <br><br>

<asp:Label runat="server" ID = "lblErrorMessage" CssClass="ms-error"></asp:Label>
    
    <table style="width: 800px;">
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Group Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtGroupName" runat="server" Width="500px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Description"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="6" Width= "500px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan=2>
                <asp:Label ID="Label3" runat="server" Text="Members:"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan=2>
                <asp:GridView ID="grMember" runat="server" AutoGenerateColumns="False" SelectedRowStyle-BackColor="#CCCCCC" Width ="100%" HeaderStyle-CssClass="GridHeader">
                    <EmptyDataTemplate> 
                            <asp:Label ID="lblEmpty" runat="server">No Results Found</asp:Label> 
                    </EmptyDataTemplate> 
                    <columns>
                      <asp:boundfield datafield="LoginName" headertext="LogIn Name" ItemStyle-Width="200px" />
                      <asp:boundfield datafield="DisplayName" headertext="Display Name"/>
                      <asp:boundfield datafield="Path" headertext="Distinguished Name" ItemStyle-CssClass="hideContent" HeaderStyle-CssClass="hideContent" />
                      <asp:TemplateField ItemStyle-Width="5px">
                            <ItemTemplate>
                                <asp:CheckBox ID="isSelect" runat="server" />
                            </ItemTemplate>
                      </asp:TemplateField>
                    </columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <a href="#" id="btnAddMember" onclick="javascript:OpenSearchPage(); return false;">Add User</a>
                &nbsp; | &nbsp;
                <%--<inputtype="button" value="Add User" onclick="javascript:OpenSearchPage(); return false;"/>--%>
                <asp:LinkButton ID="btnRemoveMember" runat="server" OnClick="btnRemove_Click">Remove User</asp:LinkButton>
                <%--<asp:Button ID="btnRemoveMember" runat="server" Text="Remove User" OnClick="btnRemove_Click"/>--%>
                <br /><br>
            </td>
        </tr>
        <tr>
            <td colspan ="2" align="right">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="80px"/> &nbsp; &nbsp;
                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" Width="80px" OnClientClick="return ConfirmDelete();" /> &nbsp; &nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Width="80px"/>
            </td>
        </tr>
    </table>


   
</asp:Content>
