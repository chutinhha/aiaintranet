<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskEvent.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Layouts.TaskEvent" DynamicMasterPageFile="~masterurl/default.master" EnableSessionState="True" %>

<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="CustomControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint.ApplicationPages" %>

<asp:Content ID="Content2" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
<%--<script type="text/javascript" src="/_LAYOUTS/AIA.Intranet.Infrastructure/jquery-1.5.1.min.js" ></script>--%>
<style type="text/css">
	#Buttons {
	display:none;
	}
	.ms-inputformcontrols
	{
		width:450px;
	}
	textarea.ms-long, input.ms-long 
	{
		width:381px;
	}
	select.ms-long
	{
		width:386px;
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
			<td width='15px'><IMG SRC="/_layouts/images/blank.gif" width=15 alt=""></td>
			<td valign="top">
				<table cellspacing=0 cellpadding=0 width='100%' height='100%' class="ms-formtable">
					<tr style="height:25px; vertical-align:text-top">
						<td>
							<span class="ms-standardheader ms-sectionheader" style="font-size:12px">Actions</span>
							<asp:DropDownList runat="server" ID="ddlActionTypes"></asp:DropDownList>
							 
							<asp:LinkButton runat="server" ID="lnkAddAction" Text="Add" CssClass="ms-propertysheet" ValidationGroup="AddAction"></asp:LinkButton>
							
						</td>
					</tr>
					<tr style="height:25px; vertical-align:text-top" >
						<td style="padding-left:115px">
							<asp:RequiredFieldValidator ID="ddlActionTypesValidator" 
														ControlToValidate="ddlActionTypes"
														Display="Static" 
														ErrorMessage="Please select an action to add" 
														ValidationGroup="AddAction"
														CssClass="ms-propertysheet"
														Runat="server"/>
						</td>
					 </tr>
					</tr>
		<tr valign="top">
			<td>
				<asp:Panel runat="server" id="actionControlsPlaceHolder" EnableViewState="true" ></asp:Panel>
			</td>
		</tr>
	</table>
			
	<wssuc:ButtonSection runat="server" ShowStandardCancelButton="false">
		<Template_Buttons>
			<asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" Text="<%$Resources:wss,multipages_okbutton_text%>" id="btnSave"  accesskey="<%$Resources:wss,okbutton_accesskey%>" ValidationGroup="SubmitValidate"/>
			<asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" OnClientClick="javascript:window.frameElement.commitPopup();return false;" Text="<%$Resources:wss,multipages_cancelbutton_text%>" id="btnCancel" accesskey="<%$Resources:wss,cancelbutton_accesskey%>" causevalidation="true" validationgroup="ABC"/>
		</Template_Buttons>
	</wssuc:ButtonSection>
</asp:Content>