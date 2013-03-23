<%@ Control Language="C#" AutoEventWireup="false" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    Namespace="Microsoft.SharePoint.WebControls" %>
<%@ Register TagPrefix="ApplicationPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    Namespace="Microsoft.SharePoint.ApplicationPages.WebControls" %>
<%@ Register TagPrefix="SPHttpUtility" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"
    Namespace="Microsoft.SharePoint.Utilities" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>
<%@ Register Assembly="AIA.Intranet.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=079ac6f381ab0c9f"
    Namespace="AIA.Intranet.Common.Controls" TagPrefix="ioffice" %>
<%@ Register Assembly="AIA.Intranet.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=079ac6f381ab0c9f"
    Namespace="AIA.Intranet.Infrastructure.CustomFields" TagPrefix="ioffice" %>

<%@ Register TagPrefix="uc" TagName="TaskConfigurationEditor" Src="~/_controltemplates/AIA.Intranet.Infrastructure/TaskConfigurationEditor.ascx" %>
<%@ Register TagPrefix="uc" TagName="EntryApprovalEditor" Src="~/_controltemplates/AIA.Intranet.Infrastructure/EntryApprovalEditor.ascx" %>
<%@ Register TagPrefix="uc" TagName="CommentBoxNewControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/CommentBoxNewControl.ascx" %>
<%@ Register TagPrefix="uc" TagName="ILinkFormControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/ILinkForm.ascx" %>
<%@ Register TagPrefix="uc" TagName="TopEmployeeControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/TopEmployeeNewForm.ascx" %>
<%@ Register TagPrefix="uc" TagName="DepartmentCalendarControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/DepartmentCalendarForm.ascx" %>
<%@ Register TagPrefix="uc" TagName="DepartmentDocumentControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/DepartmentDocumentForm.ascx" %>

<%@ Register TagPrefix="uc" TagName="DepartmentTaskFormControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/DepartmentTaskForm.ascx" %>
<%@ Register TagPrefix="uc" TagName="PrivateTaskFormControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/PrivateTaskForm.ascx" %>
<%@ Register TagPrefix="uc" TagName="ProjectTaskFormControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/ProjectTaskForm.ascx" %>

<%@ Register TagPrefix="uc" TagName="DepartmentTaskFormDispControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/DepartmentTaskFormDisp.ascx" %>
<%@ Register TagPrefix="uc" TagName="PrivateTaskFormDispControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/PrivateTaskFormDisp.ascx" %>
<%@ Register TagPrefix="uc" TagName="ProjectTaskFormDispControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/ProjectTaskFormDisp.ascx" %>

<%@ Register TagPrefix="uc" TagName="TimesheetFormControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/TimesheetForm.ascx" %>

<%@ Register TagPrefix="uc" TagName="StationeryFormControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/StationeryForm.ascx" %>

<%@ Register TagPrefix="uc" TagName="WorkingToolFormControl" Src="~/_controltemplates/AIA.Intranet.Infrastructure/WorkingToolForm.ascx" %>

<%--Working Tool--%>
<SharePoint:RenderingTemplate ID="WorkingToolFormTemplate" runat="server">
  <Template>
        <uc:WorkingToolFormControl ID="WorkingToolFormTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<%--Stationery--%>
<SharePoint:RenderingTemplate ID="StationeryFormTemplate" runat="server">
  <Template>
        <uc:StationeryFormControl ID="StationeryFormTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<%--Timesheet--%>
<SharePoint:RenderingTemplate ID="TimesheetFormTemplate" runat="server">
  <Template>
        <uc:TimesheetFormControl ID="TimesheetFormTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<%--Task--%>
<SharePoint:RenderingTemplate ID="DepartmentTaskFormDispTemplate" runat="server">
  <Template>
        <uc:DepartmentTaskFormDispControl ID="DepartmentTaskFormDispTemplate" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="ProjectTaskFormDispTemplate" runat="server">
  <Template>
        <uc:ProjectTaskFormDispControl ID="ProjectTaskFormDispTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="PrivateTaskFormDispTemplate" runat="server">
  <Template>
        <uc:PrivateTaskFormDispControl ID="PrivateTaskFormDispTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="DepartmentTaskFormTemplate" runat="server">
  <Template>
        <uc:DepartmentTaskFormControl ID="DepartmentTaskFormTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="ProjectTaskFormTemplate" runat="server">
  <Template>
        <uc:ProjectTaskFormControl ID="ProjectTaskFormTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="PrivateTaskFormTemplate" runat="server">
  <Template>
        <uc:PrivateTaskFormControl ID="PrivateTaskFormTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="DepartmentDocumentTemplate" runat="server">
  <Template>
        <uc:DepartmentDocumentControl ID="DepartmentDocumentTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="DepartmentCalendarTemplate" runat="server">
  <Template>
        <uc:DepartmentCalendarControl ID="DepartmentCalendarTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="TopEmployeeNewFormTemplate" runat="server">
  <Template>
        <uc:TopEmployeeControl ID="TopEmployeeNewFormTemplate1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="ILinkFormTemplate" runat="server">
  <Template>
        <uc:ILinkFormControl ID="ILinkFormControl1" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="EmployeeTemplate" runat="server">
    <Template>
        <span id='part1'>
			<SharePoint:InformationBar ID="InformationBar2" runat="server"/>
			<div id="listFormToolBarTop">
			<wssuc:ToolBar CssClass="ms-formtoolbar" id="toolBarTbltop" RightButtonSeparator="&amp;#160;" runat="server">
					<Template_RightButtons>
						<SharePoint:NextPageButton ID="NextPageButton2" runat="server"/>
						<SharePoint:SaveButton ID="SaveButton3" runat="server"/>
						<SharePoint:GoBackButton ID="GoBackButton3" runat="server"/>
					</Template_RightButtons>
			</wssuc:ToolBar>
			</div>
			<SharePoint:FormToolBar ID="FormToolBar2" runat="server"/>
			<SharePoint:ItemValidationFailedMessage ID="ItemValidationFailedMessage2" runat="server"/>
			<table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="0" cellspacing="0" width="100%">
			<SharePoint:ChangeContentType ID="ChangeContentType2" runat="server"/>
			<SharePoint:FolderFormFields ID="FolderFormFields2" runat="server"/>
			<ioffice:EmployeeFieldIterator ID="ListFieldIterator2" runat="server" />
			<SharePoint:ApprovalStatus ID="ApprovalStatus2" runat="server"/>
			<SharePoint:FormComponent ID="FormComponent2" TemplateName="AttachmentRows" runat="server"/>
			</table>
			<table cellpadding="0" cellspacing="0" width="100%"><tr><td class="ms-formline"><img src="/_layouts/images/blank.gif" width='1' height='1' alt="" /></td></tr></table>
			<table cellpadding="0" cellspacing="0" width="100%" style="padding-top: 7px"><tr><td width="100%">
			<SharePoint:ItemHiddenVersion ID="ItemHiddenVersion2" runat="server"/>
			<SharePoint:ParentInformationField ID="ParentInformationField2" runat="server"/>
			<SharePoint:InitContentType ID="InitContentType2" runat="server"/>
			<wssuc:ToolBar CssClass="ms-formtoolbar" id="toolBarTbl" RightButtonSeparator="&amp;#160;" runat="server">
					<Template_Buttons>
						<SharePoint:CreatedModifiedInfo ID="CreatedModifiedInfo2" runat="server"/>
					</Template_Buttons>
					<Template_RightButtons>
						<SharePoint:SaveButton ID="SaveButton4" runat="server"/>
						<SharePoint:GoBackButton ID="GoBackButton4" runat="server"/>
					</Template_RightButtons>
			</wssuc:ToolBar>
			</td></tr></table>
		</span>
		<SharePoint:AttachmentUpload ID="AttachmentUpload2" runat="server"/>
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="FieldPermissionTemplate" runat="server">
    <Template>
        <h5>
            Some field was by your permission</h5>
        <span id='part1'>
            <SharePoint:InformationBar ID="InformationBar1" runat="server" />
            <div id="listFormToolBarTop">
                <wssuc:ToolBar CssClass="ms-formtoolbar" ID="toolBarTbltop" RightButtonSeparator="&amp;#160;"
                    runat="server">
                    <Template_RightButtons>
                        <SharePoint:NextPageButton ID="NextPageButton1" runat="server" />
                        <SharePoint:SaveButton ID="SaveButton1" runat="server" />
                        <SharePoint:GoBackButton ID="GoBackButton1" runat="server" />
                    </Template_RightButtons>
                </wssuc:ToolBar>
            </div>
            <SharePoint:FormToolBar ID="FormToolBar1" runat="server" />
            <SharePoint:ItemValidationFailedMessage ID="ItemValidationFailedMessage1" runat="server" />
            <table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="0" cellspacing="0"
                width="100%">
                <SharePoint:ChangeContentType ID="ChangeContentType1" runat="server" />
                <SharePoint:FolderFormFields ID="FolderFormFields1" runat="server" />
                <ioffice:PermissionFieldIterator ID="ListFieldIterator1" runat="server" />
                <SharePoint:ApprovalStatus ID="ApprovalStatus1" runat="server" />
                <SharePoint:FormComponent ID="FormComponent1" TemplateName="AttachmentRows" runat="server" />
            </table>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="ms-formline">
                        <img src="/_layouts/images/blank.gif" width='1' height='1' alt="" />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" width="100%" style="padding-top: 7px">
                <tr>
                    <td width="100%">
                        <SharePoint:ItemHiddenVersion ID="ItemHiddenVersion1" runat="server" />
                        <SharePoint:ParentInformationField ID="ParentInformationField1" runat="server" />
                        <SharePoint:InitContentType ID="InitContentType1" runat="server" />
                        <wssuc:ToolBar CssClass="ms-formtoolbar" ID="toolBarTbl" RightButtonSeparator="&amp;#160;"
                            runat="server">
                            <Template_Buttons>
                                <SharePoint:CreatedModifiedInfo ID="CreatedModifiedInfo1" runat="server" />
                            </Template_Buttons>
                            <Template_RightButtons>
                                <SharePoint:SaveButton ID="SaveButton2" runat="server" />
                                <SharePoint:GoBackButton ID="GoBackButton2" runat="server" />
                            </Template_RightButtons>
                        </wssuc:ToolBar>
                    </td>
                </tr>
            </table>
        </span>
        <SharePoint:AttachmentUpload ID="AttachmentUpload1" runat="server" />
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="AssignmentFieldControlTemplate" runat="server">
    <Template>
        <table>
            <tr>
                <td>
                    <asp:RadioButton runat="server" ID="radDefault" GroupName="Assignement" Text="Default"
                        Checked="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButton runat="server" ID="radAll" GroupName="Assignement" Text="All Users"
                        Checked="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButton ID="radCustom" runat="server" GroupName="Assignement" Text="Custom"
                        Checked="false" /> 
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="panelAssignment" runat="server"></asp:Panel>
                </td>
            </tr>
        </table>
        <br />
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="AssignmentFieldControlForDisplayTemplate" runat="server">
    <Template>
        <asp:Label  runat="server" Text="" ID="lblDisplay" />
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="AttachmentDisplayFieldControlTemplate" runat="server">
    <Template>
        <asp:Panel  runat="server" Text="" ID="pnlDisplay" />
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="TaskConfigurationTemplate" runat="server">
    <Template>
        <uc:TaskConfigurationEditor runat="server"></uc:TaskConfigurationEditor>
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="EntryApprovalEditorTemplate" runat="server">
    <Template>
        <uc:EntryApprovalEditor ID="EntryApprovalEditor" runat="server"></uc:EntryApprovalEditor>
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="RegularExpressionValidatorFieldControlTemplate" runat="server">
    <Template>
        <asp:TextBox ID="txtValidateRegex" runat="server" />
    </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="EmailFieldControl" runat="server">
  <Template>
   
    <table>
    <tr>
    <td >
    <asp:Label ID="EmailPrefix"  runat="server" />

    </td> 
     <td>
   <asp:TextBox id="TextField" runat="server" CssClass="ms-long"></asp:TextBox> 
   </td>

     </tr>
     <tr>
   
   </tr>
   </table>
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="EmailFieldControlForDisplay" runat="server">
  <Template>
    <asp:Label ID="EmailValueForDisplay" runat="server" />
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="CommentBoxRenderTemplate" runat="server">
  <Template>
        <uc:CommentBoxNewControl runat="server" />
  </Template>
  </SharePoint:RenderingTemplate>

  <SharePoint:RenderingTemplate ID="LinkToItemControl" runat="server">
  <Template>
   
    <table>
    <tr>
    <td >
    <asp:Label ID="EmailPrefix"  runat="server" />

    </td> 
     <td>
   <asp:TextBox id="TextField" runat="server" CssClass="ms-long"></asp:TextBox> 
   </td>

     </tr>
     <tr>
   
   </tr>
   </table>
  </Template>
</SharePoint:RenderingTemplate>

<SharePoint:RenderingTemplate ID="LinkToItemControlForDisplay" runat="server">
  <Template>
    <asp:Label ID="EmailValueForDisplay" runat="server" />
    <asp:TextBox id="TextField" runat="server" CssClass="ms-long"></asp:TextBox> 
  </Template>
</SharePoint:RenderingTemplate>

