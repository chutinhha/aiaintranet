<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeNewsUserControl.ascx.cs"
    Inherits="AIA.Intranet.Infrastructure.WebParts.HomeNews.HomeNewsUserControl" %>
<div class="homenews">
    <div class="title_red_1">
        <h1>
            <asp:Literal ID="literalWebPartTitle" runat="server"></asp:Literal>
        </h1>
    </div>
        <div class="div_hotnews_right scroll_a">
            <asp:Repeater ID="repeaterHotNews" runat="server">
                <ItemTemplate>
                    <div class="gto_news">
                        <h2 class="titleRed">
                            <asp:HyperLink ID="hyperLinkTitle" NavigateUrl="#" runat="server">HyperLink</asp:HyperLink>
                        </h2>
                        <asp:Label ID="lableDate" CssClass="date-time" runat="server" Text="Label"></asp:Label>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater ID="repeaterCommingUp" runat="server">
                <ItemTemplate>
                    <div style="margin-bottom: 14px">
                        <asp:HyperLink ID="hyperLinkCommingUp" CssClass="button" NavigateUrl="#" runat="server"><span>&nbsp;&nbsp;&nbsp;&nbsp;</span>HyperLink</asp:HyperLink>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
</div>
<%--<div class="gto_news">
    <h2 class="titleRed">
        <a href="#">Procedure &amp; Manuals</a></h2>
    <span class="date-time">22/03/2013 &nbsp; 08:20 AM</span>
</div>
<div class="gto_news">
    <h2 class="titleRed">
        <a href="#">Community Activities</a></h2>
    <span class="date-time">22/03/2013 &nbsp; 08:20 AM</span>
</div>
<div class="gto_news">
    <h2 class="titleRed">
        <a href="#">Healthy Living, Procedure &amp; Manuals, Healthy Living</a></h2>
    <span class="date-time">22/03/2013 &nbsp; 08:20 AM</span>
</div>
<div class="gto_news">
    <h2 class="titleRed">
        <a href="#">We Care About Ewaste</a></h2>
    <span class="date-time">22/03/2013 &nbsp; 08:20 AM</span>
</div>
<div class="gto_news">
    <h2 class="titleRed">
        <a href="#">Procedure &amp; Manuals</a></h2>
    <span class="date-time">22/03/2013 &nbsp; 08:20 AM</span>
</div>
<div class="gto_news">
    <h2 class="titleRed">
        <a href="#">Community Activities</a></h2>
    <span class="date-time">22/03/2013 &nbsp; 08:20 AM</span>
</div>--%>
<%--<div style="margin-bottom: 14px">
                <a href="#" class="button"><span>&nbsp;&nbsp;&nbsp;&nbsp;</span>See what's comming up</a></div>
            <div style="margin-bottom: 14px">
                <a href="#" class="button"><span>&nbsp;&nbsp;&nbsp;&nbsp;</span>See what's comming up</a></div>--%>
