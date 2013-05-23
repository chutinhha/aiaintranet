<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactSendUserControl.ascx.cs"
    Inherits="AIA.Intranet.Infrastructure.WebParts.ContactSend.ContactSendUserControl" %>
<div class="main_content main_bg">
    <div class="viewlist_page">
        <div class="col_right w100">
            <div class="whatNews_box noPaddingL">
                <div class="whatNews_description">
                    <h3>
                        <asp:Literal ID="literalWebPartTitle" Text="Looking for the right person to talk to" runat="server"></asp:Literal>
                    </h3>
                    <p>
                        <asp:Literal ID="literalWebPartDescription" Text="The information presented herein is intended to comply with the relevant disclosure
                        requirements of the Rules Governing The Listing of Securities on the Stock Exchange of Hong Kong Limited." runat="server"></asp:Literal>
                    </p>
                </div>
                <div id="divMessages" runat="server" style="display:none;">
                    Thanks for your feedbak.
                </div>
                <div id="divContent" runat="server">
                    <div style="padding-top: 10px">
                        <div class="div40">
                            <div class="comment_page">
                                <h2>
                                    Contact type
                                </h2>
                                <div class="fl">
                                    Type of Enquiry &nbsp;
                                </div>
                                <asp:DropDownList ID="ddlTypeOfEnquiry" CssClass="dropdownlist" style="min-width:200px;" runat="server">
                                </asp:DropDownList>
                                <h2>
                                    Content contact</h2>
                                <div class="fl">
                                    Comment&nbsp;<asp:RequiredFieldValidator ID="rfvContent" ControlToValidate="txtContent" runat="server" ErrorMessage="(*)"></asp:RequiredFieldValidator>
                                </div>
                                <asp:TextBox ID="txtContent" style="width: 385px" CssClass="textarea" TextMode="MultiLine"
                                Rows="6" runat="server"></asp:TextBox>

                                <%--<SharePoint:FormField ID="ffContent" FieldName="Content" runat="server" ControlMode="New"></SharePoint:FormField>--%>
                                
                                <div style="padding: 10px 0 10px 50px">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="Submit" />
                                    <input id="btnReset" type="reset" class="button" value="Reset" />
                                </div>
                            </div>
                        </div>
                        <%--<div class="div40 noMarginR">
                            <div class="comment_page">
                                <h2>
                                    Contact infomation</h2>
                                <p>
                                    <asp:Literal ID="literalContactInfomation" Text="Our vision is to be a leading provider of financial prosperity and security solutions
                                    through a relentless focus on customers, distributors and employees." runat="server"></asp:Literal>
                                </p>
                            </div>
                        </div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
