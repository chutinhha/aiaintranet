<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OtherNewsListViewUserControl.ascx.cs" Inherits="AIA.Intranet.Infrastructure.WebParts.OtherNewsListView.OtherNewsListViewUserControl" %>

<div class="box-container">
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
</div>