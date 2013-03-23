﻿<%-- _lcid="1033" _version="14.0.4762" _dal="1" --%>
<%-- _LocalBinding --%>
<%@ Page language="C#" MasterPageFile="~masterurl/default.master"    Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c"  %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %> <%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:ListItemProperty Property="BaseName" maxlength="40" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<WebPartPages:WebPartZone runat="server" title="loc:TitleBar" id="TitleBar" AllowLayoutChange="false" AllowPersonalization="false"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
  <style type="text/css">
	Div.ms-titleareaframe {
	height: 100%;
	}
	.ms-pagetitleareaframe table {
	background: none;
	}
  </style>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
	<meta name="GENERATOR" content="Microsoft SharePoint" />
	<meta name="ProgId" content="SharePoint.WebPartPage.Document" />
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="CollaborationServer" content="SharePoint Team Web Site" />
	<script type="text/javascript">
// <![CDATA[
	var navBarHelpOverrideKey = "WSSEndUser";
// ]]>
	</script>
	<SharePoint:UIVersionedContent ID="WebPartPageHideQLStyles" UIVersion="4" runat="server">
		<ContentTemplate>
<style type="text/css">
body #s4-leftpanel {
	display:none;
}
.s4-ca {
	margin-left:0px;
}
</style>
		</ContentTemplate>
	</SharePoint:UIVersionedContent>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderSearchArea" runat="server">
	<SharePoint:DelegateControl runat="server"
		ControlId="SmallSearchInputBox" />
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderLeftActions" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageDescription" runat="server">
	<SharePoint:ProjectProperty Property="Description" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderBodyRightMargin" runat="server">
	<div height="100%" class="ms-pagemargin"><img src="/_layouts/images/blank.gif" width="10" height="1" alt="" /></div>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageImage" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderNavSpacer" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderLeftNavBar" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
		<table cellpadding="4" cellspacing="0" border="0" width="100%">
				<tr>
					<td valign="top" style="padding:0">
						<table cellpadding="4" cellspacing="0" border="0" width="100%" height="100%">
                        <tr>
                            <td id="_invisibleIfEmpty" colspan="3" name="_invisibleIfEmpty" valign="top" height="100%"> <WebPartPages:WebPartZone runat="server" Title="loc:MyTop" ID="MyTop" FrameType="TitleBarOnly"/> </td>
                        </tr>
                            <tr>
                                <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" colspan="2" valign="top" height="100%" > <WebPartPages:WebPartZone runat="server" Title="loc:LeftTop" ID="LeftTop" FrameType="TitleBarOnly"/> </td>
                                <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%"> <WebPartPages:WebPartZone runat="server" Title="loc:RightTop" ID="RightTop" FrameType="TitleBarOnly"/> </td>
								
                            </tr>
                            <tr>
                                <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%"> <WebPartPages:WebPartZone runat="server" Title="loc:LeftButton" ID="LeftButton" FrameType="TitleBarOnly"/> </td>
								<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%"> <WebPartPages:WebPartZone runat="server" Title="loc:CenterButton" ID="CenterButton" FrameType="TitleBarOnly"/> </td>
                                <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%"> <WebPartPages:WebPartZone runat="server" Title="loc:RightButton" ID="RightButton" FrameType="TitleBarOnly"/> </td>
                            </tr>
                            <tr>
								<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" colspan="3" valign="top"> <WebPartPages:WebPartZone runat="server" Title="loc:Header1" ID="Header1" FrameType="TitleBarOnly"/> </td>
							</tr>

							<tr>
								<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" colspan="3" valign="top"> <WebPartPages:WebPartZone runat="server" Title="loc:Header" ID="Header" FrameType="TitleBarOnly"/> </td>
							</tr>
							<tr>
								<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" colspan="3" valign="top"> <WebPartPages:WebPartZone runat="server" Title="loc:TopRow" ID="TopRow" FrameType="TitleBarOnly" Orientation="Horizontal"/> </td>
							</tr>
							<tr>
								<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%"> <WebPartPages:WebPartZone runat="server" Title="loc:CenterLeftColumn" ID="CenterLeftColumn" FrameType="TitleBarOnly"/> </td>
								<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%"> <WebPartPages:WebPartZone runat="server" Title="loc:CenterColumn" ID="CenterColumn" FrameType="TitleBarOnly"/> </td>
								<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%"> <WebPartPages:WebPartZone runat="server" Title="loc:CenterRightColumn" ID="CenterRightColumn" FrameType="TitleBarOnly"/> </td>
							</tr>
							<tr>
								<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" colspan="3" valign="top"> <WebPartPages:WebPartZone runat="server" Title="loc:Footer" ID="Footer"/> </td>
							</tr>
						</table>
					</td>
					<td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%"> <WebPartPages:WebPartZone runat="server" Title="loc:RightColumn" ID="RightColumn"/> </td>
				</tr>
				<script type="text/javascript" language="javascript">if(typeof(MSOLayout_MakeInvisibleIfEmpty) == "function") {MSOLayout_MakeInvisibleIfEmpty();}</script>
		</table>
</asp:Content>
