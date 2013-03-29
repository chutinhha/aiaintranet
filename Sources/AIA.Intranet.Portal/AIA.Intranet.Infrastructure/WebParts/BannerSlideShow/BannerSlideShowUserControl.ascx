<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BannerSlideShowUserControl.ascx.cs"
    Inherits="AIA.Intranet.Infrastructure.WebParts.BannerSlideShow.BannerSlideShowUserControl" %>
<link href="../../../_layouts/images/AIA.Intranet.Infrastructure/styles/styles.css"
    rel="stylesheet" type="text/css" />
<link href="../../../_layouts/images/AIA.Intranet.Infrastructure/styles/jquery.ad-gallery.css"
    rel="stylesheet" type="text/css" />

<script src="../../../_layouts/1033/jquery-1.8.2.min.js" type="text/javascript"></script>
<script src="../../../_layouts/1033/slimScroll.js" type="text/javascript"></script>
<script src="../../../_layouts/1033/jquery.ad-gallery.js" type="text/javascript"></script>
<div class="main_content">
    <div class="div_slider">
        <div class="col_left">
            <div class="beauty_slideShowContent">
                <!-- SLIDE SHOW -->
                <script type="text/javascript">
                    $(function () {
                        var galleries = $('.ad-gallery').adGallery();
                        $('#switch-effect').change(
                          function () {
                              galleries[0].settings.effect = $(this).val();
                              return false;
                          }
                        );
                        $('#toggle-slideshow').click(
                          function () {
                              galleries[0].slideshow.toggle();
                              return false;
                          }
                        );
                        $('#toggle-description').click(
                          function () {
                              if (!galleries[0].settings.description_wrapper) {
                                  galleries[0].settings.description_wrapper = $('#descriptions');
                              } else {
                                  galleries[0].settings.description_wrapper = false;
                              }
                              return false;
                          }
                        );
                    });
                </script>
                <!--end slideshow scripts-->
                <div id="gallery" class="ad-gallery">
                    <div class="ad-image-wrapper">
                    </div>
                    <div class="ad-controls">
                    </div>
                    <div class="ad-nav">
                        <div class="ad-thumbs">
                            <ul class="ad-thumb-list" id="ulThumbList" runat="server">
                                
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<%--<ul class="ad-thumb-list">
    <li><a href="../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/1.jpg">
        <img src="../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/space.png"
            style="background: url('../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/1.jpg') no-repeat center center;
            -webkit-background-size: cover; -moz-background-size: cover; -o-background-size: cover;
            background-size: cover; filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/1.jpg', sizingMethod='scale');
            -ms-filter: 'progid:DXImageTransform.Microsoft.AlphaImageLoader(src='../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/1.jpg', sizingMethod='scale')';
            width: 52px; height: 35px" title="Title 01" alt="Alt 01" class="image01" />
    </a></li>
    <li><a href="../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/2.jpg">
        <img src="../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/space.png"
            style="background: url('../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/2.jpg') no-repeat center center;
            -webkit-background-size: cover; -moz-background-size: cover; -o-background-size: cover;
            background-size: cover; filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/2.jpg', sizingMethod='scale');
            -ms-filter: 'progid:DXImageTransform.Microsoft.AlphaImageLoader(src='../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/2.jpg', sizingMethod='scale')';
            width: 52px; height: 35px" title="Title 02" alt="Alt 02" class="image02" />
    </a></li>
    <li><a href="../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/3.jpg">
        <img src="../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/space.png"
            style="background: url('../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/3.jpg') no-repeat center center;
            -webkit-background-size: cover; -moz-background-size: cover; -o-background-size: cover;
            background-size: cover; filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/3.jpg', sizingMethod='scale');
            -ms-filter: 'progid:DXImageTransform.Microsoft.AlphaImageLoader(src='../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/3.jpg', sizingMethod='scale')';
            width: 52px; height: 35px" title="Title 03" alt="Alt 03" class="image03" />
    </a></li>
    <li><a href="../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/4.jpg">
        <img src="../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/space.png"
            style="background: url('../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/4.jpg') no-repeat center center;
            -webkit-background-size: cover; -moz-background-size: cover; -o-background-size: cover;
            background-size: cover; filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/4.jpg', sizingMethod='scale');
            -ms-filter: 'progid:DXImageTransform.Microsoft.AlphaImageLoader(src='../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/4.jpg', sizingMethod='scale')';
            width: 52px; height: 35px" title="Title 04" alt="Alt 04" class="image04" />
    </a></li>
    <li><a href="../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/5.jpg">
        <img src="../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/space.png"
            style="background: url('../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/5.jpg') no-repeat center center;
            -webkit-background-size: cover; -moz-background-size: cover; -o-background-size: cover;
            background-size: cover; filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(src='../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/5.jpg', sizingMethod='scale');
            -ms-filter: 'progid:DXImageTransform.Microsoft.AlphaImageLoader(src='../../../_layouts/images/AIA.Intranet.Infrastructure/slideshow/5.jpg', sizingMethod='scale')';
            width: 52px; height: 35px" title="Title 05" alt="Alt 05" class="image05" />
    </a></li>
</ul>--%>
