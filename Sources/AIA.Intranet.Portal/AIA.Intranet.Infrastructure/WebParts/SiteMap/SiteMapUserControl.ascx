<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteMapUserControl.ascx.cs" Inherits="AIA.Intranet.Infrastructure.WebParts.SiteMap.SiteMapUserControl" %>
<%@ Register TagPrefix="uc" Namespace="AIA.Intranet.Infrastructure.WebParts.SiteMap" Assembly="$SharePoint.Project.AssemblyFullName$" %>
<script type="text/javascript" src="/Style%20Library/js/simpletreemenu.js"  language="javascript" ></script>
<div class="main_content main_bg">
    <div class="viewlist_page">
      <div class="col_right w100">
        <div class="whatNews_box noPaddingL">
          <div class="whatNews_description">
            <h3>Sitemap</h3>
          </div>
          <!--sitemap-->
          <div class="leftMenuWidth">
            <div class="link_home"><a href="#" runat="server" id="lnkHome">Home</a></div>
           <%-- <ul id="treemenu1" class="treeview">
              <li class="submenu" style="background-image: url(/Style Library/images/closed.gif);"><a href="#" class="level_1">Folder 1</a>
                <ul rel="closed" style="display: none;">
                  <li><a href="#">Sub Item 1.1</a></li>
                  <li class="li_last"><a href="#">Nấu gì hôm nay?</a></li>
                </ul>
              </li>
              <li><a href="#" class="level_1">Item 3</a></li>
              <li><a href="#" class="li_last level_1">Item 4</a></li>
              <li class="submenu"><a href="#" class="level_1">Folder 2</a>
                <ul rel="closed">
                  <li><a href="#" class="level_2">Sub Item 2.1</a></li>
                  <li class="submenu"><a href="#" class="level_2">Sub Item 2.2</a>
                    <ul rel="closed">
                      <li><a href="#" class="level_3">Sub Item 2.2_1</a></li>
                      <li class="submenu"><a href="#" class="level_3">Sub Item 2.2_2</a>
                        <ul rel="closed">
                          <li><a href="#" class="level_4">Sub Item 2.2_2_1</a></li>
                          <li class="submenu"><a href="#" class="level_4">Sub Item 2.2_2_2</a>
                          	<ul rel="closed">
                            	<li><a href="#" class="level_5">Sub Item 2.2_2_2__1</a></li>
                                <li><a href="#" class="level_5">Sub Item 2.2_2_2__2</a></li>
                            </ul>
                          </li>
                        </ul>
                      </li>
                      <li><a href="#" class="level_3">Sub Item 2.2_3</a></li>
                    </ul>
                  </li>
                  <li><a href="#" class="level_2">Sub Item 2.3</a></li>
                  <li class="li_last"><a href="#" class="level_2">Sub Item 2.4</a></li>
                </ul>
              </li>
              <li><a href="#" class="level_1">Item 6</a></li>
              <li class="li_last"><a href="#" class="level_1">Item 7</a></li>
            </ul>--%>

            <uc:SiteMapControl runat="server" RootId="treemenu2"/>
          </div>
          <script type="text/javascript">
              //ddtreemenu.createTree(treeid, enablepersist, opt_persist_in_days (default is 1))
              ddtreemenu.createTree("treemenu2", true)
              
              //ddtreemenu.createTree("treemenu2", false)
            </script>
        </div>
      </div>
    </div>
  </div>
