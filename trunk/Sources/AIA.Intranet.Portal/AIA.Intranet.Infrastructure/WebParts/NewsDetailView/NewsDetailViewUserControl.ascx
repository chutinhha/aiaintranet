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
    <div class="box-header uppercase box-title">
        <%= SPContext.Current.List.Title %>
    </div>
    <div class="wp-news-details">
        <div class="wp-news-title-area">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%# ShowDateTime %>'>
                <div class="wp-news-date">
                    <asp:Literal ID="ltNewsDate" runat="server"></asp:Literal>
                </div>
            </asp:PlaceHolder>
            <div class="wp-news-title">
                <asp:Literal ID="ltNewsTitle" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="wp-news-desc">
            <asp:Literal ID="ltNewsDescription" runat="server"></asp:Literal>
        </div>
        <div class="wp-news-content">
            <asp:Literal ID="ltNewsContent" runat="server"></asp:Literal>
            <%--<div id="divContent" runat="server"></div>--%>
        </div>
        <div class="wp-news-utils">
            <div class="wp-news-utils-box">
                <img align="absmiddle" alt="" src="<%= SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/') %>/Style Library/images/gotop.gif"
                    class="js-go-top handover" />
                <a href="#top" class="js-go-top handover">Về đầu trang</a>
            </div>
        </div>
    </div>
</div>

<%--<div class="contentContainer clearfix">
    <div class="pageContentContainer">

        <h1>
            Những cột mốc phát triển</h1>
        <div class="imageBox imageBoxOnLeft clearfix">
            <div class="imageBoxImage">
                <img border="0" alt="AIA Việt Nam &ndash; Những cột mốc phát triển" src="/vn/resources/c972a80049bddebbbe17ffb927f11de2/AIA+VN_+Royal+Centre.jpg">
            </div>
            <div class="imageBoxText">
            </div>
        </div>
        <div class="mainText">
            <p>
                <strong><span style="color: #BA1419;">2012:</span></strong></p>
            <ul>
                <li>Ra mắt sản phẩm An Phúc Thành Tài, An Phúc Hưng Gia, Tài Lộc An Phát, và An Lộc
                    Phát.</li>
                <li>Nhận giải thưởng TOP 20 - Sản phẩm, Dịch vụ Tin cậy vì Người Tiêu dùng</li>
                <li>Nhận giải thưởng TOP 1.000 doanh nghiệp nộp thuế thu nhập doanh nghiệp lớn nhất
                    Việt Nam năm 2012 (AIA Việt Nam ở trong top 300)</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2011:</span></strong></p>
            <ul>
                <li>Ra mắt sản phẩm An Phúc Trọn Đời Nâng Cao, An Nghiệp Bảo Nhân, An Tâm Tịnh Dưỡng,
                    An Sinh Bình An và gói sản phẩm An Trí Bảo Gia.</li>
                <li>Khai trương văn phòng đại lý thứ 3 tại TP. Hồ Chí Minh.</li>
                <li>Nhận bằng khen của UBND TP. Hà Nội</li>
                <li>Phát động chương trình “Sống Khỏe” (Healthy Living)</li>
                <li>Phát động chiến dịch truyền thông “Đừng Để Hụt Chân” (Mind the Gap)</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2010:</span></strong></p>
            <ul>
                <li>Nhận Giải thưởng Rồng Vàng lần thứ 6.</li>
                <li>Được người tiêu dùng bình chọn danh hiệu "Thương hiệu nổi tiếng năm 2010”</li>
                <li>Triển khai chương trình "Kiểm tra sức khỏe tài chính chuyên sâu".</li>
                <li>Mở rộng hai văn phòng đại lý tại Hà Nội và khai trương các trung tâm dịch vụ khách
                    hàng kiểu mẫu tại Nha Trang, Đà Nẵng, Cần Thơ và Qui Nhơn.</li>
                <li>Tăng vốn điều lệ lên thành 1.035 tỉ đồng.</li>
                <li>Phát động chiến dịch "The Power of We" (Sức Mạnh của Chúng Ta - tạm dịch).</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2009:</span></strong></p>
            <ul>
                <li>Nhận Bằng khen của Bộ Tài chính, Bằng khen của UBND TP. Hà Nội và Giải thưởng Rồng
                    Vàng lần thứ 5.</li>
                <li>Khai trương văn phòng đại lý thứ 2 tại Hà Nội và trung tâm dịch vụ khách hàng kiểu
                    mẫu đầu tiên tại TP. Hồ Chí Minh.</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2008:</span></strong></p>
            <ul>
                <li>Tăng vốn điều lệ lên thành 970 tỉ đồng.</li>
                <li>Triển khai chương trình "Kiểm tra sức khỏe tài chính" trên toàn quốc.</li>
                <li>Được người tiêu dùng bình chọn danh hiệu "Thương hiệu nổi tiếng năm 2008" và nhận
                    Giải thưởng Rồng Vàng lần thứ 4.</li>
                <li>Khai trương văn phòng đại lý thứ 2 tại TP. Hồ Chí Minh.</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2007:</span></strong></p>
            <ul>
                <li>Khai trương văn phòng đại diện tại Tây Ninh và Quảng Ngãi.</li>
                <li>Nhận Giải thưởng Rồng Vàng lần thứ 3.</li>
                <li>Giới thiệu chương trình "Kiểm tra sức khỏe tài chính" tại Hà Nội và TP. Hồ Chí Minh.</li>
                <li>Giới thiệu sản phẩm "An Phúc Trọn Đời".</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2006:</span></strong></p>
            <ul>
                <li>Khai trương văn phòng đại diện tại Bình Định.</li>
                <li>Ra mắt gói sản phẩm Bảo hiểm Sức khỏe và Tai nạn với tên gọi An Tâm Toàn Diện.</li>
                <li>Được người tiêu dùng bình chọn danh hiệu "Thương hiệu nổi tiếng năm 2006".</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2005:</span></strong></p>
            <ul>
                <li>Khai trương văn phòng tổng đại lý tại Kon Tum, Hải Dương, Gia Lai và ĐăkLăk.</li>
                <li>Lần đầu tiên ra mắt dòng sản phẩm Bảo hiểm Sức khỏe và Tai nạn với sản phẩm đầu
                    tiên mang tên An Tâm Bảo Gia. Đây là sản phẩm đặt nền tảng cho gói sản phẩm bảo
                    vệ toàn diện được triển khai vào những năm tiếp theo.</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2004:</span></strong></p>
            <ul>
                <li>Khai trương văn phòng chi nhánh tại Đà Nẵng Cần Thơ và văn phòng tổng đại lý đầu
                    tiên tại Đồng Tháp và Kiên Giang.</li>
                <li>Đưa vào sử dụng Hệ thống Giải đáp Thông tin Tự động (Interactive Voice Response
                    - IVR).</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2003:</span></strong></p>
            <ul>
                <li>Khai trương văn phòng đại lý tại TP. Hồ Chí Minh.</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2002:</span></strong></p>
            <ul>
                <li>Nhận Giải thưởng Rồng Vàng lần thứ 2 cho phong cách kinh doanh và phục vụ tốt nhất.</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2001:</span></strong></p>
            <ul>
                <li>Nhận Giải thưởng Rồng Vàng cho phong cách kinh doanh và phục vụ tốt nhất.</li>
                <li>Khai trương các văn phòng giao dịch tại Cần Thơ, Đà Nẵng, Nha Trang, An Giang, Tiền
                    Giang, Hải Phòng, Đồng Nai, Cà Mau và Bà Rịa - Vũng Tàu.</li>
            </ul>
            <p>
                <strong><span style="color: #BA1419;">2000:</span></strong></p>
            <ul>
                <li>Nhận giấy phép thành lập công ty bảo hiểm nhân thọ 100% vốn nước ngoài tại Việt
                    Nam với Trụ sở chính đặt tại TP. Hồ Chí Minh và chi nhánh tại Hà Nội.</li>
            </ul>
        </div>
 
    </div>
    <div class="pageComponentContainer">
        <div class="componentBox h54ImageOnLeft clearfix">
            <div class="componentBoxImage">
                <img border="0" width="70" height="39" title="" src="/vn/resources/91003c80498006f98e1c8f4e12e54bc2/ph_contact_us_thumbnail.jpg">
            </div>
            <div class="componentBoxText">
                <h4>
                    Liên hệ với chuyên viên tư vấn của chúng tôi ngay hôm nay!</h4>
                Điện thoại: (84-8) 38122 777
            </div>
        </div>
    </div>
</div>--%>
