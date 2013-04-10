<%-- _lcid="1033" _version="14.0.4762" _dal="1" --%>
<%-- _LocalBinding --%>
<%@ Register TagPrefix="WpNs1" Namespace="AIA.Intranet.Infrastructure.WebParts.HomeNews"
    Assembly="AIA.Intranet.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0b6a88a58a49868d" %>
<%@ Register TagPrefix="WpNs0" Namespace="AIA.Intranet.Infrastructure.WebParts.BannerSlideShow"
    Assembly="AIA.Intranet.Infrastructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0b6a88a58a49868d" %>

<%@ Page Language="C#" MasterPageFile="~masterurl/default.master" Inherits="Microsoft.SharePoint.WebPartPages.WebPartPage,Microsoft.SharePoint,Version=14.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <SharePoint:ListItemProperty Property="BaseName" MaxLength="40" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    <WebPartPages:WebPartZone runat="server" Title="loc:TitleBar" ID="TitleBar" AllowLayoutChange="false"
        AllowPersonalization="false" />
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderTitleAreaClass" runat="server">
    <style type="text/css">
        Div.ms-titleareaframe
        {
            height: 100%;
        }
        .ms-pagetitleareaframe table
        {
            background: none;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
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
        <contenttemplate>
<style type="text/css">
    body #s4-leftpanel
    {
        display: none;
    }
    .s4-ca
    {
        margin-left: 0px;
    }
</style>
		</contenttemplate>
    </SharePoint:UIVersionedContent>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderSearchArea" runat="server">
    <SharePoint:DelegateControl runat="server" ControlId="SmallSearchInputBox" />
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderLeftActions" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
    <SharePoint:ProjectProperty Property="Description" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderBodyRightMargin" runat="server">
    <div height="100%" class="ms-pagemargin">
        <img src="/_layouts/images/blank.gif" width="10" height="1" alt="" /></div>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderPageImage" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderNavSpacer" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderLeftNavBar" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table cellpadding="4" cellspacing="0" border="0" width="100%">
        <tr>
            <td valign="top" style="padding: 0">
                <table cellpadding="4" cellspacing="0" border="0" style="width: 100%; height: 100%;">
                    <tr>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" colspan="3" valign="top">
                            <WebPartPages:WebPartZone runat="server" Title="loc:Header" ID="Header" FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                    </tr>
                    <tr>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" colspan="3" valign="top">
                            <WebPartPages:WebPartZone runat="server" Title="loc:TopRow" ID="TopRow" FrameType="TitleBarOnly"
                                Orientation="Horizontal">
                                <ZoneTemplate>
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                    </tr>
                    <tr>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" colspan="2" valign="top" height="100%">
                            
                            <WebPartPages:WebPartZone runat="server" Title="loc:LeftTop" ID="LeftTop" FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                    
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" height="100%">

                            <WebPartPages:WebPartZone runat="server" Title="loc:RightTop" ID="RightTop" FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                    
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                    </tr>
                    <tr>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" style="width:33%; height: 100%;">
                            <WebPartPages:WebPartZone runat="server" Title="loc:CenterLeftColumn" ID="CenterLeftColumn"
                                FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                    
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" style="width:33%; height: 100%;">
                            <WebPartPages:WebPartZone runat="server" Title="loc:CenterColumn" ID="CenterColumn"
                                FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                    
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" style="width:34%; height: 100%;">
                            <WebPartPages:WebPartZone runat="server" Title="loc:CenterRightColumn" ID="CenterRightColumn"
                                FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                    
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                    </tr>

                    <tr>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" colspan="2"  style="width:66%; height: 100%;">
                            <WebPartPages:WebPartZone runat="server" Title="loc:LeftBelow" ID="LeftBelow"
                                FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                    
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" style="width:34%; height: 100%;">
                            <WebPartPages:WebPartZone runat="server" Title="loc:RightBelow" ID="RightBelow"
                                FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                    
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                    </tr>

                    <tr>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" style="width:33%; height: 100%;">
                            <WebPartPages:WebPartZone runat="server" Title="loc:BelowLeftColumn" ID="BelowLeftColumn"
                                FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                    
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" style="width:33%; height: 100%;">
                            <WebPartPages:WebPartZone runat="server" Title="loc:BelowCenterColumn" ID="BelowCenterColumn"
                                FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                    
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" valign="top" style="width:34%; height: 100%;">
                            <WebPartPages:WebPartZone runat="server" Title="loc:BelowRightColumn" ID="BelowRightColumn"
                                FrameType="TitleBarOnly">
                                <ZoneTemplate>
                                    
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                    </tr>

                    <tr>
                        <td id="_invisibleIfEmpty" name="_invisibleIfEmpty" colspan="3" valign="top">
                            <WebPartPages:WebPartZone runat="server" Title="loc:Footer" ID="Footer">
                                <ZoneTemplate>
                                </ZoneTemplate>
                            </WebPartPages:WebPartZone>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <script type="text/javascript" language="javascript">            if (typeof (MSOLayout_MakeInvisibleIfEmpty) == "function") { MSOLayout_MakeInvisibleIfEmpty(); }</script>
    </table>
</asp:Content>
