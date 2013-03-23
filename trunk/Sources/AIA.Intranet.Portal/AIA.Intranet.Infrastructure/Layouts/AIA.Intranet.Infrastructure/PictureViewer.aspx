<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PictureViewer.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Layouts.PictureViewer" DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register TagPrefix="uc" TagName="CommentBoxWebPartUserControl" Src="~/_controltemplates/AIA.Intranet.Webparts/CommentBoxWebPartV2UserControl.ascx" %>

<%@ Register TagPrefix="webpart" Namespace="AIA.Intranet.Webparts" Assembly="AIA.Intranet.Webpart, Version=1.0.0.0, Culture=neutral, PublicKeyToken=079ac6f381ab0c9f" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

<SharePoint:CssLink runat="server"></SharePoint:CssLink>
<SharePoint:CssRegistration ID="CssRegistration2" Name="corev4.css" runat="server"/>
<SharePoint:CssRegistration ID="CssRegistration1" Name="forms.css" After="corev4.css" runat="server"/>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<SharePoint:InformationBar ID="InformationBar1" runat="server"/>

<SharePoint:FormToolBar ID="FormToolBar1" runat="server" ControlMode="Display"/>
<div id="Container">
    <div class="picturecolumn">
        <table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="0" cellspacing="0" width="100%">
            <SharePoint:ListFieldIterator ID="ListFieldIterator1" runat="server" ControlMode="Display"/>
           </table>
    </div>
        
    <div class="commentcol">
            
        <uc:CommentBoxWebPartUserControl runat="Server" />

    </div>
</div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">

</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<a id=onetidListHlink HREF=<%SPHttpUtility.AddQuote(SPHttpUtility.UrlPathEncode(CurrentList.DefaultViewUrl,true),Response.Output);%>><%SPHttpUtility.HtmlEncode(CurrentList.Title, Response.Output);%></a>&#32;
    <SharePoint:ClusteredDirectionalSeparatorArrow ID="ClusteredDirectionalSeparatorArrow1" runat="server" /> 
    <%SPHttpUtility.HtmlEncode(DisplayName, Response.Output);%>
    
</asp:Content>


