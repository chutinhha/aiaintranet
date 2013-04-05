<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewNewsCategoryWebPartUserControl.ascx.cs"
    Inherits="AIA.Intranet.Infrastructure.WebParts.ViewNewsCategoryWebPart.ViewNewsCategoryWebPartUserControl" %>
<div class="wp-catnews">
    <div class="wp-catnews-main-news">
        <asp:Literal ID="ltMainNews" runat="server"></asp:Literal>
        <div class="clear-both">
        </div>
    </div>
    <div class="wp-catnews-others">
        <ul>
            <asp:Literal ID="ltOtherNews" runat="server"></asp:Literal>
        </ul>
    </div>
</div>

<%--<style type="text/css">
.clearfix:after {
    clear: both;
    content: ".";
    display: block;
    height: 0;
    visibility: hidden;
}
.gridBox584w {
    width: 584px;
}
.gridBox {
    clear: both;
    height: auto;
    margin-bottom: 7px;
    margin-left: -10px;
}
.clearfix {
    display: block;
}
.clearfix {
    display: inline-block;
}

.gridBox584w .gridBoxHeader {
    background: url("http://aia.com.vn/vn/resources/7f62b38046d3e6a689edfd48a4199252/whiteContainerHeader.gif") repeat scroll 0 0 transparent;
    width: 584px;
}
.gridBox .gridBoxHeader {
    height: 12px;
    margin-bottom: -2px;
}

.clearfix:after {
    clear: both;
    content: ".";
    display: block;
    height: 0;
    visibility: hidden;
}
.gridBox584w .gridBoxMain {
    background: url("http://aia.com.vn/vn/resources/448d698046d3e7008a0cfe48a4199252/whiteContainerMiddle.gif?MOD=AJPERES") repeat-y scroll 0 0 transparent;
}
.gridBox .gridBoxMain {
    border: 0 solid #E6E6E0;
    padding: 10px 14px 0;
    width: auto;
}
.clearfix {
    display: block;
}

.gridBox .gridBoxMainContent {
    padding: 1px 17px 14px;
}
.clearfix:after {
    clear: both;
    content: ".";
    display: block;
    height: 0;
    visibility: hidden;
}
.gridBoxMainContent .gridBoxRow {
    padding: 4px 0 8px;
}
.clearfix {
    display: block;
}

.clearfix:after {
    clear: both;
    content: ".";
    display: block;
    height: 0;
    visibility: hidden;
}
.gridBoxMainContent .division {
    height: 6px;
    margin: 0;
    padding-bottom: 6px;
}
.clearfix {
    display: block;
}
.clearfix {
    display: inline-block;
}
.gridBoxMainContent .division {
    height: 6px;
    margin: 0;
    padding-bottom: 6px;
}

.gridBoxMainContent .division .divisionOnLeft {
    float: left;
    margin-right: 10px;
    width: 256px;
}

.gridBoxMainContent .division .divisionOnRight {
    float: left;
    width: 255px;
}
HR {
    background-color: #EEEEE6;
    border: medium none;
    clear: both;
    display: block;
    height: 7px;
}
</style>--%>

<%--<div class="gridBox gridBox584w clearfix">
    <div class="gridBoxHeader">
    </div>
    <div class="gridBoxMain clearfix">
        <div class="gridBoxMainContent">
            <div class="gridBoxRow clearfix">
                <div class="gridBoxRow clearfix">
                    <div class="gridBoxColOnLeft clearfix">
                        <h2>
                            Những cột mốc phát triển</h2>
                        <div class="gridBoxImage">
                            <a target="_self" href="http://aia.com.vn/vn/about/about-us/key-milestones/key-milestones.html">
                                <img border="0" title="Tin tức &ndash; Sự kiện" alt="Tin tức &ndash; Sự kiện" src="/vn/resources/c51aa30049c316f3bf21ffb927f11de2/AIA_au_new_events_thumbnail_vn.jpg"></a></div>
                        <div class="gridBoxText">
                            <a class="arrow" target="_self" href="http://aia.com.vn/vn/about/about-us/key-milestones/key-milestones.html">
                                2000: Nhận giấy phép thành lập công ty bảo hiểm nhân thọ 100% vốn nước ngoài tại
                                Việt Nam với Trụ sở chính đặt tại TP. Hồ Chí Minh và chi nhánh tại Hà Nội.</a></div>
                    </div>
                    <div class="gridBoxColOnRight clearfix">
                        <h2>
                            Hoạt động cộng đồng</h2>
                        <div class="gridBoxImage">
                            <a target="_self" href="http://aia.com.vn/vn/about/about-us/community/community.html">
                                <img border="0" title="Corporate Social Responsibility" alt="Corporate Social Responsibility"
                                    src="/vn/resources/64b2b90049c03053891bf9b927f11de2/AIA_au_community_thumbnail_vn.jpg"></a></div>
                        <div class="gridBoxText">
                            <a class="arrow" target="_self" href="http://aia.com.vn/vn/about/about-us/community/community.html">AIA
                                Việt Nam luôn cam kết góp phần nâng cao chất lượng cuộc sống của người dân và trở
                                thành một doanh nghiệp có trách nhiệm tại cộng đồng.</a></div>
                    </div>
                </div>
            </div>
            <div class="division clearfix">
                <div class="divisionOnLeft">
                    <hr>
                </div>
                <div class="divisionOnRight">
                    <hr>
                </div>
            </div>
            <div class="gridBoxRow clearfix">
                <div class="gridBoxRow clearfix">
                    <div class="gridBoxColOnLeft clearfix">
                        <h2>
                            Giải thưởng và Danh hiệu</h2>
                        <div class="gridBoxImage">
                            <a target="_self" href="http://aia.com.vn/vn/about/about-us/awards-recognitions/awards-recognitions.html">
                                <img border="0" title="Awards and Recognitions" alt="Awards and Recognitions" src="/vn/resources/8a1fe10049c030ac8931f9b927f11de2/AIA_au_awards_recognitions_thumbnail_vn.jpg"></a></div>
                        <div class="gridBoxText">
                            <a class="arrow" target="_self" href="http://aia.com.vn/vn/about/about-us/awards-recognitions/awards-recognitions.html">
                                AIA Việt Nam cũng vinh dự đạt được những danh hiệu cao quý như “Thương hiệu nổi
                                tiếng”, “Giải Rồng Vàng”, “TOP 20 - Sản phẩm, Dịch vụ Tin cậy vì Người Tiêu dùng”,
                                bBằng khen của Bộ Tài chính, bằng khen của UBND TP. Hà Nội... về những đóng góp
                                nổi bật đối với ngành bảo hiểm nhân thọ tại Việt Nam.</a></div>
                    </div>
                    <div class="gridBoxColOnRight clearfix">
                        <h2>
                            Tuyển dụng</h2>
                        <div class="gridBoxImage">
                            <a target="_self" href="http://aia.com.vn/vn/about/about-us/recruitment/recruitment.html">
                                <img border="0" title="Recruitment" alt="Recruitment" src="http://aia.com.vn/vn/resources/6e35330049c030de893cf9b927f11de2/AIA_au_recuitment_thumbnail_vn.jpg"></a></div>
                        <div class="gridBoxText">
                            <a class="arrow" target="_self" href="http://aia.com.vn/vn/about/about-us/recruitment/recruitment.html">
                                Với mạng lưới hoạt động rộng khắp tại các tỉnh thành trên toàn quốc, AIA Việt Nam
                                liên tục tìm kiếm những ứng viên xuất sắc vào những vị trí làm việc tại công ty
                                theo phương châm: Tin tưởng và tôn trọng lẫn nhau, Tinh thần đồng đội, Chính trực,
                                Quyết thắng và Không sợ thất bại.</a></div>
                    </div>
                </div>
            </div>
            <div class="division clearfix">
                <div class="divisionOnLeft">
                    <hr>
                </div>
                <div class="divisionOnRight">
                    <hr>
                </div>
            </div>
            <div class="gridBoxRow clearfix">
                <div class="gridBoxRow clearfix">
                    <div class="gridBoxColOnLeft clearfix">
                        <h2>
                            Tin tức &ndash; Sự kiện</h2>
                        <div class="gridBoxImage">
                            <a target="_self" href="http://aia.com.vn/vn/about/about-us/news-events/news-events.html">
                                <img border="0" title="Tin tức &ndash; Sự kiện" alt="Tin tức &ndash; Sự kiện" src="http://aia.com.vn/vn/resources/c51aa30049c316f3bf21ffb927f11de2/AIA_au_new_events_thumbnail_vn.jpg"></a></div>
                        <div class="gridBoxText">
                            <a class="arrow" target="_self" href="http://aia.com.vn/vn/about/about-us/news-events/news-events.html">
                                Ngày 29/10/2011, Tập đoàn AIA đã phát động chương trình “SỐNG KHỎE” (HEALTHY LIVING)
                                để khuyến khích đội ngũ nhân viên, đại lý của AIA và cộng đồng ý thức về sống khỏe
                                cũng như có những thói quen tốt để có một cuộc sống khỏe mạnh cả thể chất và tinh
                                thần.</a></div>
                    </div>
                    <div class="gridBoxColOnRight clearfix">
                    </div>
                </div>
            </div>
            <div class="division clearfix" style="display: none;">
                <div class="divisionOnLeft">
                    <hr>
                </div>
                <div class="divisionOnRight">
                    <hr>
                </div>
            </div>
            <div class="gridBoxRow clearfix" style="display: none;">
            </div>
        </div>
    </div>
    <div class="gridBoxBottom">
    </div>
</div>--%>
