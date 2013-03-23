<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentBoxNewControl.ascx.cs"
    Inherits="AIA.Intranet.Infrastructure.ControlTemplates.AIA.Intranet.Infrastructure.CommentBoxNewControl" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" src="~/_controltemplates/ToolBarButton.ascx" %>

<span id='part1'>
    <SharePoint:InformationBar ID="InformationBar1" runat="server" />
    <div id="listFormToolBarTop">
        <wssuc:ToolBar CssClass="ms-formtoolbar" ID="toolBarTbltop" RightButtonSeparator="&amp;#160;"
            runat="server">
            <Template_RightButtons>
                <SharePoint:NextPageButton ID="NextPageButton1" runat="server" />
                <SharePoint:SaveButton ID="SaveButton2" runat="server" />
                <SharePoint:GoBackButton ID="GoBackButton1" runat="server" />
            </Template_RightButtons>
        </wssuc:ToolBar>
    </div>
    <SharePoint:FormToolBar ID="FormToolBar1" runat="server" />
    <SharePoint:ItemValidationFailedMessage ID="ItemValidationFailedMessage1" runat="server" />
    <table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="0" cellspacing="0"
        width="100%">
        <tr>
            <td width="190px" valign="top" class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <nobr>Title<span class="ms-formvalidation"> *</span>
								</nobr>
                </h3>
            </td>
            <td width="400px" valign="top" class="ms-formbody">
                <SharePoint:FormField runat="server" ControlMode="New" FieldName="Title" />
                <SharePoint:FieldDescription runat="server" FieldName="Title" ControlMode="New" />
            </td>
        </tr>
        <tr>
            <td width="190px" valign="top" class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <nobr>Nội Dung</nobr>
                </h3>
            </td>
            <td width="400px" valign="top" class="ms-formbody">
                <SharePoint:FormField runat="server" ControlMode="New" FieldName="CommentText" />
                <SharePoint:FieldDescription runat="server" FieldName="CommentText" ControlMode="New" />
            </td>
        </tr>
        <tr style="display:none">
            <td width="190px" valign="top" class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <nobr>Số Phản Hồi</nobr>
                </h3>
            </td>
            <td width="400px" valign="top" class="ms-formbody">
                <SharePoint:FormField runat="server" ControlMode="New" FieldName="RepliesCount" />
                <SharePoint:FieldDescription runat="server" FieldName="RepliesCount" ControlMode="New" />
            </td>
        </tr>
        <tr style="display:none">
            <td width="190px" valign="top" class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <nobr>Phản Hồi Cho</nobr>
                </h3>
            </td>
            <td width="400px" valign="top" class="ms-formbody">
                <SharePoint:FormField runat="server" ControlMode="New" FieldName="ReplyTo" />
                <SharePoint:FieldDescription runat="server" FieldName="ReplyTo" ControlMode="New" />
            </td>
        </tr>
        <tr style="display:none">
            <td width="190px" valign="top" class="ms-formlabel">
                <h3 class="ms-standardheader">
                    <nobr>Nguồn bình luận</nobr>
                </h3>
            </td>
            <td width="400px" valign="top" class="ms-formbody">
                <SharePoint:FormField runat="server" ControlMode="New" FieldName="CommentUrl" />
                <SharePoint:FieldDescription runat="server" FieldName="CommentUrl" ControlMode="New" />
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
                        <SharePoint:SaveButton ID="SaveButton1" runat="server" />
                        <SharePoint:GoBackButton ID="GoBackButton2" runat="server" />
                    </Template_RightButtons>
                </wssuc:ToolBar>
            </td>
        </tr>
    </table>
</span>