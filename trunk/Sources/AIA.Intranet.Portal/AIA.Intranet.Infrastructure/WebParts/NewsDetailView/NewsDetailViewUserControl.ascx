<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsDetailViewUserControl.ascx.cs"
    Inherits="AIA.Intranet.Infrastructure.WebParts.NewsDetailView.NewsDetailViewUserControl" %>

<div class="box-container">
<%--
    <div class="box-header uppercase box-title">
        <%= SPContext.Current.List.Title %>
    </div>
--%>
    <div class="wp-news-details whatNews_description">
        <div class="wp-news-title-area">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# ShowDateTime %>'>
                <div class="wp-news-date">
                    <asp:Literal ID="ltNewsDate" runat="server"></asp:Literal>
                </div>
            </asp:PlaceHolder>
            <div class="wp-news-title">
                <h1><asp:Literal ID="ltNewsTitle" runat="server"></asp:Literal></h1>
            </div>
        </div>
        <div class="wp-news-desc">
            <h3><asp:Literal ID="ltNewsDescription" runat="server"></asp:Literal></h3>
        </div>
        <div class="wp-news-content">
            <asp:Literal ID="ltNewsContent" runat="server"></asp:Literal>
        </div>
<%--
        <div class="wp-news-utils">
            <div class="wp-news-utils-box">
                <img align="absmiddle" alt="" src="<%= SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/') %>/Style Library/images/gotop.gif"
                    class="js-go-top handover" />
                <a href="#top" class="js-go-top handover">Về đầu trang</a>
            </div>
        </div>
--%>
    </div>
</div>

<%--
<div class="col_right">
    <div class="whatNews_box">
        <div class="whatNews_description">
            <h1>
                <asp:Literal ID="ltNewsTitle" runat="server"></asp:Literal>
            </h1>
            <h3>
                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# ShowDateTime %>'>
                    <asp:Literal ID="ltNewsDate" runat="server"></asp:Literal>
                </asp:PlaceHolder>
            </h3>
            <p>
                <asp:Literal ID="ltNewsDescription" runat="server"></asp:Literal>
            </p>
        </div>
        <div>
            <asp:Literal ID="ltNewsContent" runat="server"></asp:Literal>
        </div>
    </div>
</div>
--%>