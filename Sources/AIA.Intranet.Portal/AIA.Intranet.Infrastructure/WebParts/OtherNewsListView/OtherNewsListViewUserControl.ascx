<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtherNewsListViewUserControl.ascx.cs"
    Inherits="AIA.Intranet.Infrastructure.WebParts.OtherNewsListView.OtherNewsListViewUserControl" %>
<%--<div class="box-container">
    <div class="box-header uppercase box-title">
        <%= ((System.Web.UI.WebControls.WebParts.WebPart)this.Parent).Title.ToString().Trim() != "" ? ((System.Web.UI.WebControls.WebParts.WebPart)this.Parent).Title.ToString().Trim() : "Tin cùng chuyên mục"%>
    </div>
    <div class="wp-news-others">
        <ul>
            <asp:Repeater ID="rptOtherNews" runat="server">
                <ItemTemplate>
                    <li>
                        <a class='wp-news-others-title' href='<%# Eval("ViewUrl") %>'><%# Eval("Title") %></a>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# ShowDateTime %>'>
                            <span class='wp-news-others-date'>(<%# Convert.ToDateTime(Eval("Created")).ToString(DateTimeFormat) %>)</span>
                        </asp:PlaceHolder>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</div>--%>
<div class="whatNews_box">
    <div class="news_listItems">
        <div class="listItem">
            <h2>
                <%= ((System.Web.UI.WebControls.WebParts.WebPart)this.Parent).Title.ToString().Trim() != "" ? ((System.Web.UI.WebControls.WebParts.WebPart)this.Parent).Title.ToString().Trim() : "Other news"%>
            </h2>
            <ul class="other-news">
                <asp:Repeater ID="rptOtherNews" runat="server">
                    <ItemTemplate>
                        <li>
                            <a class='wp-news-others-title' href='<%# Eval("ViewUrl") %>'><%# Eval("Title") %></a>
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# ShowDateTime %>'><span
                                class='news_others_date'>(<%# Convert.ToDateTime(Eval("Created")).ToString(DateTimeFormat) %>)</span>
                            </asp:PlaceHolder>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>
</div>
<%--<a href="#">Australia</a><br>
<a href="#">China</a><br>
<a href="#">Hong Kong</a><br>
<a href="#">Indonesia</a><br>
<a href="#">Malaysia</a><br>
<a href="#">New Zealand</a><br>
<a href="#">Philippines</a><br>--%>