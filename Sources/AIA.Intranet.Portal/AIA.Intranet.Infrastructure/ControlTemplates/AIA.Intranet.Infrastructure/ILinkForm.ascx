<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ILinkForm.ascx.cs" Inherits="AIA.Intranet.Infrastructure.ControlTemplates.AIA.Intranet.Infrastructure.ILinkForm" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>

<span id='part1'>
			<SharePoint:InformationBar ID="InformationBar1" runat="server"/>
			<div id="listFormToolBarTop">
			<wssuc:ToolBar CssClass="ms-formtoolbar" id="toolBarTbltop" RightButtonSeparator="&amp;#160;" runat="server">
					<Template_RightButtons>
						<SharePoint:NextPageButton ID="NextPageButton1" runat="server"/>
						<SharePoint:SaveButton ID="SaveButton1" runat="server"/>
						<SharePoint:GoBackButton ID="GoBackButton1" runat="server"/>
					</Template_RightButtons>
			</wssuc:ToolBar>
			</div>
			<SharePoint:FormToolBar ID="FormToolBar1" runat="server"/>
			<SharePoint:ItemValidationFailedMessage ID="ItemValidationFailedMessage1" runat="server"/>
			<table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="0" cellspacing="0" width="100%">
			<SharePoint:ChangeContentType ID="ChangeContentType1" runat="server"/>
			<SharePoint:FolderFormFields ID="FolderFormFields1" runat="server"/>
			<%--<SharePoint:ListFieldIterator ID="ListFieldIterator1" runat="server"/>--%>
            <table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                            <h3 class="ms-standardheader">
                                <nobr><SharePoint:FieldLabel runat="server" ID="flblTitle" FieldName="Title" /></nobr>
                            </h3>
                        </td>
                        <td class="ms-formbody" valign="top">
                            <SharePoint:FormField runat="server" ID="ffdTitle"  FieldName="Title" />
                        </td>
                    </tr>
                    <tr>
                        <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                            <h3 class="ms-standardheader">
                                <nobr><SharePoint:FieldLabel runat="server" ID="flbLinkUrl" FieldName="LinkUrl" /></nobr>
                            </h3>
                        </td>
                        <td class="ms-formbody" valign="top">
                            <SharePoint:FormField runat="server" ID="ffdLinkUrl"  FieldName="LinkUrl" />
                        </td>
                    </tr>
                    <tr>
                        <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                            <h3 class="ms-standardheader">
                                <nobr><SharePoint:FieldLabel runat="server" ID="flbType" FieldName="Type" /></nobr>
                            </h3>
                        </td>
                        <td class="ms-formbody" valign="top">
                            <SharePoint:FormField runat="server" ID="ffdType"  FieldName="Type" />
                        </td>
                    </tr>
                    <tr>
                        <td width="190" class="ms-formlabel" nowrap="nowrap" valign="top">
                            <h3 class="ms-standardheader">
                                <nobr><SharePoint:FieldLabel runat="server" ID="flbCategory" FieldName="Category" /></nobr>
                            </h3>
                        </td>
                        <td class="ms-formbody" valign="top">
                            <SharePoint:FormField runat="server" ID="fldCategory"  FieldName="Category" />
                        </td>
                    </tr>
                </tbody>
            </table>
			<SharePoint:ApprovalStatus ID="ApprovalStatus1" runat="server"/>
			<SharePoint:FormComponent ID="FormComponent1" TemplateName="AttachmentRows" runat="server"/>
			</table>
			<table cellpadding="0" cellspacing="0" width="100%"><tr><td class="ms-formline"><img src="/_layouts/images/blank.gif" width='1' height='1' alt="" /></td></tr></table>
			<table cellpadding="0" cellspacing="0" width="100%" style="padding-top: 7px"><tr><td width="100%">
			<SharePoint:ItemHiddenVersion ID="ItemHiddenVersion1" runat="server"/>
			<SharePoint:ParentInformationField ID="ParentInformationField1" runat="server"/>
			<SharePoint:InitContentType ID="InitContentType1" runat="server"/>
			<wssuc:ToolBar CssClass="ms-formtoolbar" id="toolBarTbl" RightButtonSeparator="&amp;#160;" runat="server">
					<Template_Buttons>
						<SharePoint:CreatedModifiedInfo ID="CreatedModifiedInfo1" runat="server"/>
					</Template_Buttons>
					<Template_RightButtons>
						<SharePoint:SaveButton ID="SaveButton2" runat="server"/>
						<SharePoint:GoBackButton ID="GoBackButton2" runat="server"/>
					</Template_RightButtons>
			</wssuc:ToolBar>
			</td></tr></table>
		</span>
		<SharePoint:AttachmentUpload ID="AttachmentUpload1" runat="server"/>