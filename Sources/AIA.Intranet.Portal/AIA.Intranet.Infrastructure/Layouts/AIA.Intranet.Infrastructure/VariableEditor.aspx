<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VariableEditor.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Pages.VariableEditor" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<html>
<head>
    <title>Please select variable</title>
	<SharePoint:ScriptLink ID="importCore" language="javascript" name="core.js" Defer="true" runat="server"/>
    <script type="text/javascript">
        var lsbColumnID = '<%=lsbColumn.ClientID %>';
        var ddlTypeID = '<%=ddlType.ClientID %>';

        function insertVariable() {
            try {
                alert('insertVariable');
                var lsbColumn = document.getElementById(lsbColumnID);
                var ColumnSelected;
                for (i = 0; i < lsbColumn.options.length; i++) {
                    if (lsbColumn.options[i].selected) {
                        ColumnSelected = lsbColumn.options[i];
                        break;
                    }
                }
                var variableString = '%' + document.getElementById(ddlTypeID).value + ':' + ColumnSelected.text + '%';
                SP.UI.ModalDialog.commonModalDialogClose('OK', variableString);
            }
            catch (err) {
                alert(err);
            }
        }
        function ClosePopup() {
            SP.UI.ModalDialog.commonModalDialogClose('cancel', 'Cancelled clicked');
        }
    </script>
    <style type="text/css">
        .ms-formbody {
            background-color:rgb(241, 241, 242);
            border-bottom: 1px solid #D8D8D8;
            font-family: verdana;
            font-size: 8pt;
            padding: 3px 6px 4px;
            vertical-align: top;
        }
        .ms-formlabel {
            border-bottom: 1px solid #D8D8D8;
            color: #525252;
            font-family: verdana;
            font-size: 0.7em;
            font-weight: bold;
            padding-bottom: 6px;
            padding-right: 8px;
            padding-top: 3px;
            text-align: left;
        }
		.ms-long {
			font-family: Verdana,sans-serif;
            font-size: 8pt;
		}
		.ms-ButtonHeightWidth {
			font: 8pt tahoma;
			height: 2.1em;
			padding-bottom: 0.4em;
			padding-top: 0.1em;
			width: 7.5em;
		}
    </style>
</head>
<body style="margin: 10px; font-family: verdana; font-size: 10px;">
<form id="form1" runat="server">
	
    <table cellspacing="0" width="100%" border="0" style="border-style: none; border-collapse: collapse;" id="ctl00_PlaceHolderMain_RuleListGrid" class="ms-listviewtable">
			<tr>
				<td colspan="2" class="ms-formlabel" >
					Variable Editor
				</td>
			</tr>
            <tr >
			    <td class="ms-formlabel" width="30%">Type</td>
			    <td class="ms-formbody">
			       &nbsp;<asp:DropDownList runat="server" ID="ddlType" CssClass="ms-long" AutoPostBack="true">
			            <asp:ListItem Value="Item" Selected="True" >Item</asp:ListItem>
			            <asp:ListItem Value="Task">Task</asp:ListItem>
                        <asp:ListItem Value="TaskList">Task List</asp:ListItem>
						<asp:ListItem Value="Global">Global Variables</asp:ListItem>
			       </asp:DropDownList> 
			    </td>
            </tr>
            <tr>
                <td class="ms-formlabel" style="white-space: nowrap;vertical-align:top"  width="30%" ><asp:Label ID="lbDescription" runat="server" Text="Site Columns" /></td>
                <td class="ms-formbody" style="white-space: nowrap;vertical-align:top">
                    <asp:Panel ID="pnlGroup" runat="server">
				        <asp:Label ID="lbGroup" runat="server" >Select site columns from:</asp:Label><br/>
				        &nbsp;<asp:DropDownList runat="server" ID="ddlGroup" CssClass="ms-long" AutoPostBack="true" Width="250"></asp:DropDownList><br/><br/>
				    </asp:Panel>
					<asp:Label ID="lbColumn" runat="server">Available site columns:</asp:Label><br/>
                    &nbsp;<asp:ListBox ID="lsbColumn" runat="server" CssClass="ms-long" SelectionMode="Single" Rows="10" Width="250"></asp:ListBox>
                </td>
            </tr>
            <tr>
				<td colspan="2">
					<table cellspacing="0" border="0" width="100%">
					<tr>
						<td style="text-align:right">
							<button class="ms-ButtonHeightWidth" onclick="insertVariable()" >Insert</button>&nbsp;&nbsp;
                            <button class="ms-ButtonHeightWidth" onclick="ClosePopup()">Close</button>
						</td>
					</tr>
					</table>
				</td>
			</tr>
    </table>
</form>
</body>
</html>
