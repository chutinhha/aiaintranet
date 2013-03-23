<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailTemplateSelector.ascx.cs" Inherits="AIA.Intranet.Infrastructure.Controls.EmailTemplateSelector" %>
<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>

<script type="text/javascript">
	function ValidatorEnable_<%=this.ClientID %>(val){
			ValidatorEnable(document.getElementById('<%=txtTemplateUrlValidator.ClientID%>'),val);
			ValidatorEnable(document.getElementById('<%=txtTemplateNameValidator.ClientID%>'),val);             
		   }
</script>
<style >
	.ms-ButtonHeightWidth{
		width:8.2em;
	}
</style>
<wssuc:InputFormControl ID="InputFormControl1" runat="server"  LabelText="Email Template List Url">
	<Template_Control>
    <asp:TextBox runat="server" id="txtTemplateUrl" AutoPostBack="true"></asp:TextBox>
        <wssawc:InputFormRequiredFieldValidator ID="txtTemplateUrlValidator" 
												ControlToValidate="txtTemplateUrl"
												Display="Dynamic" 
												ErrorMessage="Please enter url of the email template list." 
												Runat="server"/>
	</Template_Control>
</wssuc:InputFormControl>
<wssuc:InputFormControl ID="InputFormControl2" runat="server" LabelText="Template Name">
	<Template_Control>
		<asp:DropDownList Title="Template Name" Class="ms-input" ID="txtTemplateName" Runat="server" Width="205px"/>
		<wssawc:InputFormRequiredFieldValidator ID="txtTemplateNameValidator"
												ControlToValidate="txtTemplateName"
												Display="Dynamic" 
												ErrorMessage="Please enter an email template name" 
												Runat="server"/>
	</Template_Control>
</wssuc:InputFormControl>
		
