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

<style type='text/css'>
    body #s4-leftpanel{display: none;}
    .s4-ca{margin-left: 0;}
</style>

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
