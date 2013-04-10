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
<script type="text/javascript" language="javascript" >

    function setNodeImage() {
        $(".submenu").each(function () {
            $(this).has(">ul:visible").css("background-image", "url('/Style Library/images/open.gif')");
        });
    }

    $(function () {
        $(".submenu").click(function () {
            $(this).has(">ul:visible").css("background-image", "url('/Style Library/images/open.gif')");
            $(this).has(">ul:hidden").css("background-image", "url('/Style Library/images/closed.gif')");
        });
    })

</script>

<div class="main_content">
    <div class="viewlist_page">
      <div class="col_right w100">
        <div class="whatNews_box noPaddingL">
          <div class="whatNews_description">
            <h3>Sitemap</h3>
          </div>
          <!--sitemap-->
          <div class="leftMenuWidth">
            <div class="link_home"><a href="#" runat="server" id="lnkHome">AIA Portal</a></div>
            <uc:SiteMapControl runat="server" RootId="treemenu2"/>
          </div>
          <script type="text/javascript">
              ddtreemenu.createTree("treemenu2", false);
              ddtreemenu.flatten('treemenu2', 'expand');
              setNodeImage();
            </script>
        </div>
      </div>
    </div>
  </div>
