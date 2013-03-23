<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskRule.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Layouts.TaskRule" DynamicMasterPageFile="~masterurl/default.master" EnableSessionState="True" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="CustomControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="cm" Src="~/_controltemplates/AIA.Intranet.Infrastructure/TaskRuleEditor.ascx" TagName="TaskRuleEditor" %>

<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
<SharePoint:CssRegistration ID="CssRegistration1" Name="forms.css" runat="server"/>
<style type="text/css">
	#Buttons {
	display:none;
	}
</style>
<script language="javascript">
    function _spBodyOnLoad() {
        var obj = document.getElementById("Buttons");
        if (obj != null && obj.tagName == "TABLE") {
            obj.className = "ms-pickerbuttonsection";

        }
    }

</script>
</asp:Content>
<asp:Content ID="Content4" contentplaceholderid="PlaceHolderMain" runat="server">
	<wssawc:FormDigest runat="server" id="FormDigest" />
	<table class="ms-formtable" cellspacing=0 cellpadding=0 width='100%' height='100%'>
		<tr>
			<td width='15px'>&nbsp;</td>
				<td class=ms-descriptiontext style="color:red;">
					<asp:PlaceHolder runat="server" id="PlaceHolderError"/>
					
				</td>
			<td width='15px'>&nbsp;</td>
		</tr>
		
		<tr height='100%'>
			<td width='15px'><IMG SRC="/_layouts/images/blank.gif" width=15 alt=""></td>
			<td valign="top">
				<table cellspacing=0 cellpadding=0 width='100%' height='100%' class="ms-formtable">
					<tr height="100%">
						<td>
							<cm:TaskRuleEditor runat="server" id="ruleEditor"></cm:TaskRuleEditor>
						</td>
					</tr>
				</table>
			</td>
			<td width='15px'><img src="/_layouts/images/blank.gif" width=15 height=200 alt=""></td>
		</tr>
	</table>
</asp:Content>
