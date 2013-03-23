<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveAll.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Layouts.ApproveAll" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <script type="text/javascript">
        function closeDialog() {
            SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.cancel, 'Cancelled clicked');
        }
        function finisheDialog() {
            SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.OK, 'Cancelled clicked');
        }
    </script>
    <h2 id="divMessage" runat="server">
    </h2>
    <table>
        <tr>
            <td>
                Status:
            </td>
            <td>
                <asp:DropDownList ID="ddlAprovalOptions" runat="server">
                    <asp:ListItem Text="Approve" Value="Approved" />
                    <asp:ListItem Text="Pending" Value="Pending" />
                    <asp:ListItem Text="Reject" Value="Denied" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Comments:
            </td>
            <td>
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Columns="40" Rows="5" MaxLength="255" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="OK" OnClick="btnOk_Click" />
                <input type="button" runat="server" id="btnCancel" value="Cancel" onclick="closeDialog()" />
            </td>
        </tr>
    </table>
</asp:Content>