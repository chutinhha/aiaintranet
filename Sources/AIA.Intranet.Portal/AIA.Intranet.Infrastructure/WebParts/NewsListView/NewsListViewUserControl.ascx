<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsListViewUserControl.ascx.cs" Inherits="AIA.Intranet.Infrastructure.WebParts.NewsListView.NewsListViewUserControl" %>

<div class="box-container">
    <div class="box-header">
        <div class="uppercase box-title" style="float: left;"><%= SPContext.Current.List.Title %></div>
        <Sharepoint:SPSecurityTrimmedControl ID="SPSecurityTrimmedControl1" runat="server" PermissionContext="CurrentList" Permissions="EditListItems">
            <div style="float:right;">
                <a href='<%= SPContext.Current.Web.ServerRelativeUrl + "/" + SPContext.Current.List.Views["All Items"].Url %>' target="_blank">(biên tập tin)</a>
            </div>
        </Sharepoint:SPSecurityTrimmedControl>
        <div class="clear-both"></div>
    </div>
    <div class="box-desc">
        <%= SPContext.Current.List.Description %>
    </div>
    <div class="box-content">
        <asp:Repeater ID="rptNews" runat="server">
            <ItemTemplate>
                <div class="wp-newslist-box">
                    <a href='<%# Eval("ViewUrl") %>' class="wp-newslist-title"><%# Eval("Title") %></a>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# ShowDateTime %>'>
                        <span class="wp-newslist-date">(<%# Convert.ToDateTime(Eval("Created")).ToString(DateTimeFormat) %>)</span>
                    </asp:PlaceHolder>
                    <div class="wp-newslist-detail">
                        <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible='<%# Eval("Thumbnail") != null && Eval("Thumbnail").ToString() != "" %>'>
                            <a href='<%# Eval("ViewUrl") %>'><img src='<%# Eval("Thumbnail") %>' align="left" alt="" style='width:<%= MainPicWidth %>'/></a>
                        </asp:PlaceHolder>
                        <span class="wp-newslist-desc"><%# Eval("ShortDescription")%></span>
                    </div>
                    <div class="clear-both"></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <div class="custom-paging">
            <center>
                <table class="tbl-custom-paging" cellpadding="0" cellspacing="0">
                    <tbody>
                        <tr>
                            <td valign="middle">
                                <asp:ImageButton ID="ibtnPrev" runat="server" 
                                    ImageUrl="/_layouts/1033/images/prev.gif" onclick="ibtnPrev_Click" CssClass="btn-paging-prev"/>
                            </td>
                            <td valign="middle">
                                <asp:Label ID="lblPage" runat="server" Text=""></asp:Label> 
                            </td>
                            <td valign="middle">
                                <asp:ImageButton ID="ibtnNext" runat="server" 
                                    ImageUrl="/_layouts/1033/images/next.gif" onclick="ibtnNext_Click" CssClass="btn-paging-next"/>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </center>
        </div>
    </div>
</div>

