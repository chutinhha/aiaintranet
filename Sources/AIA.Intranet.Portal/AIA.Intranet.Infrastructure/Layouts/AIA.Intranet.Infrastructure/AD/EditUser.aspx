<%@ Page Language="C#" Inherits="AIA.Intranet.Infrastructure.Pages.EditUser, $SharePoint.Project.AssemblyFullName$"
    DynamicMasterPageFile="~masterurl/default.master" EnableSessionState="True" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="AIA.Intranet.Infrastructure.Pages" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Manage Active Directory
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    Manage Active Directory
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <link rel="stylesheet" href="./css/application.css">
    <script type="text/javascript">
        function OpenSearchPage() {
            var options = SP.UI.$create_DialogOptions();
            options.url = "SearchGroup.aspx";
            options.width = 900;
            options.height = 600;
            options.dialogReturnValueCallback = CloseCallback;
            var dialogSP = SP.UI.ModalDialog.showModalDialog(options);
        }

        function OpenResetPasswordPage() {
            var options = SP.UI.$create_DialogOptions();
            options.url = "ResetPassword.aspx?userDn=" + getQuerystring("Path");
            options.width = 450;
            options.height = 200;
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

        function getQuerystring(key) {
            var result = "";
            key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
            var qs = regex.exec(window.location.href);
            if (qs == null)
                return result;
            else
                return qs[1];
        }

    </script>
    <asp:Label ID="lblTitle" runat="server" Text="Add New Active Directory User" Font-Bold="True"
        CssClass="ms-rteElement-H1B"></asp:Label>
    <br>
    <br>
    <asp:Label runat="server" ID="lblErrorMessage" CssClass="ms-error"></asp:Label>
    <table style="width: 800px;">
        <tr>
            <td style="width: 120px;">
                <asp:Label ID="Label1" runat="server" Text="Login Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtLoginName" runat="server" Width="500px"></asp:TextBox>
                <asp:Label ID="Label10" runat="server" Text="(example: user1)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Text="First Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFirstName" runat="server" Width="500px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Last Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtLastName" runat="server" Width="500px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label5" runat="server" Text="Display Name"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDisplayName" runat="server" Width="500px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Office"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtOffice" runat="server" Width="500px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Email"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" Width="500px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                    ErrorMessage="Please enter a valid email" ControlToValidate="txtEmail" ValidationExpression="(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"></asp:RegularExpressionValidator>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnEditMode" runat="server">
        <table width="800px">
            <tr>
                <td style="width: 120px;">
                    <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" Width="500px" TextMode="Password" EnableViewState="true"></asp:TextBox><br>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" Width="500px" TextMode="Password"
                        EnableViewState="true"></asp:TextBox><br>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="Dynamic"
                        ErrorMessage="The password does not meet the password policy requirements. Check the minimum password length, password complexity and password history requirements."
                        ControlToValidate="txtPassword" ValidationExpression="(?=^.{8,255}$)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*">
                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="cbRequireChangePass" runat="server" Text="User must change password at next logon"
                        OnCheckedChanged="cbRequireChangePass_Changed" AutoPostBack="True" Checked="true" />
                    <br>
                    <asp:CheckBox ID="cbCannotChagnePass" runat="server" Text="User cannot change password"
                        Enabled="false" /><br>
                    <asp:CheckBox ID="cbPassNeverExpired" runat="server" Text="Password never expires"
                        Enabled="false" /><br>
                    <asp:CheckBox ID="cbAccountIsDisable" runat="server" Text="Account is disabled" /><br>
                </td>
            </tr>
        </table>
    </asp:Panel>
    
    <asp:Panel ID="pnResetPassword" runat="server" Visible="False">
        <table style="width: 800px;">
            <tr>
                <td>
                    <a href="#" id="btnResetPassword" onclick="javascript:OpenResetPasswordPage(); return false;">
                        Reset Password</a>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <table width="800px">
        <tr>
            <td colspan="2">
                <asp:Panel ID="pnGroups" runat="server">
                    <asp:Label ID="Label3" runat="server" Text="Member of:" class="ms-sectionheader"></asp:Label><br>
                    <asp:GridView ID="grGroups" runat="server" AutoGenerateColumns="False" SelectedRowStyle-BackColor="#CCCCCC"
                        Width="100%" HeaderStyle-CssClass="GridHeader">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server">No Results Found</asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Group Name" ItemStyle-Width="200px" />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:BoundField DataField="Path" HeaderText="Distinguished Name" ReadOnly="True"
                                ItemStyle-CssClass="hideContent" HeaderStyle-CssClass="hideContent" />
                            <asp:TemplateField ItemStyle-Width="5px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="isSelect" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Panel ID="pnGroupButtons" runat="server">
                    <a href="#" id="btnAddGroup" onclick="javascript:OpenSearchPage(); return false;">Add
                        Group</a> &nbsp; | &nbsp;
                    <%--<inputtype="button" value="Add User" onclick="javascript:OpenSearchPage(); return false;"/>--%>
                    <asp:LinkButton ID="btnRemoveGroup" runat="server" OnClick="btnRemoveGroup_Click">Remove Group</asp:LinkButton>
                    <%--<asp:Button ID="btnRemoveMember" runat="server" Text="Remove User" OnClick="btnRemove_Click"/>--%>
                    <br />
                    <br>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Width="80px" />
                &nbsp; &nbsp;
                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"
                    Width="80px" OnClientClick="return ConfirmDelete();" />
                &nbsp; &nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                    Width="80px" CausesValidation="False" />
            </td>
        </tr>
    </table>
</asp:Content>
