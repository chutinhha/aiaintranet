<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskRuleEditor.ascx.cs" Inherits="AIA.Intranet.Infrastructure.Controls.TaskRuleEditor" %>

<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="/_controltemplates/ButtonSection.ascx" %> 
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" src="/_controltemplates/ToolBarButton.ascx" %>

<wssawc:FormDigest runat="server" id="FormDigest" />
<div style="color:red" id="errorMessageHolder" enableviewstate="false" runat="server"></div>
<table runat="server" id="tblFieldsValueDefinition" width='100%' cellpadding='0' cellspacing='0' border='0'>
<tr class="ms-vh2-nofilter ms-vh2-gridview">
                                    <td nowrap="true" valign="top" class="ms-descriptiontext"  >
                                        Field Name
                                    </td>
                                    <td  valign="top"  class="ms-descriptiontext"  >
                                        Operator
                                    </td>
                                    <td nowrap="true" class="ms-descriptiontext"  >
			                                Value
                                    </td>
                                </tr>
</table>
<wssuc:ButtonSection runat="server" ShowStandardCancelButton="false">
		<Template_Buttons>
			<asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" OnClick="btnSave_Click" Text="<%$Resources:wss,multipages_okbutton_text%>" id="btnSave"  accesskey="<%$Resources:wss,okbutton_accesskey%>"/>
			<asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" OnClientClick="javascript:window.frameElement.commitPopup();return false;" Text="<%$Resources:wss,multipages_cancelbutton_text%>" id="btnCancel" accesskey="<%$Resources:wss,cancelbutton_accesskey%>" causevalidation="true" validationgroup="ABC"/>
		</Template_Buttons>
	</wssuc:ButtonSection>
<asp:Literal ID="scriptData" runat="server"></asp:Literal> 
<style type="text/css">
    .hidden{ display:none; }
    .vis{ display:block; }
     .ms-bodyareaframe {
	    padding: 8px;
	    border: none;
    }
    
    td.ms-formbody  span,
    td.ms-formbody  label
    {
        font-size:8pt !important
    }
</style>
