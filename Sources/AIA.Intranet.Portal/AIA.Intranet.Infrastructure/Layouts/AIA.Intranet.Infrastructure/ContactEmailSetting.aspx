<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactEmailSetting.aspx.cs"
    Inherits="AIA.Intranet.Infrastructure.Layouts.ContactEmailSetting"
    DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>
<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table border="0" width="100%" cellspacing="0" cellpadding="0" class="ms-propertysheet">
        <thead>
            <td style="width:50%">
                &nbsp;
            </td>
            <td style="width:50%">
                &nbsp;
            </td>
        </thead>
        <tr>
            <td colspan="2" style="color: #525252; font-size: 1.4em; font-weight: bold; margin: 0;
                text-align: left; padding: 10px 6px; background-color: #F1F1F2;">
                Internal Email
            </td>
        </tr>
        <wssuc:InputFormSection ID="InputFormSection1" Title="Users" Description="Active directory users" runat="server">
            <Template_InputFormControls>
                <wssuc:InputFormControl ID="InputFormControl1" runat="server">
                    <Template_Control>
                        <SharePoint:PeopleEditor runat="server" ID="peContactUsers" Width="100%" MultiSelect="true"
                            SelectionSet="User" />
                    </Template_Control>
                </wssuc:InputFormControl>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <tr>
            <td class="ms-sectionline" height="1" colspan="2">
                <img width="1" height="1" alt="" src="/_layouts/images/blank.gif" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="color: #525252; font-size: 1.4em; font-weight: bold; margin: 0;
                text-align: left; padding: 10px 6px; background-color: #F1F1F2;">
                External Email
            </td>
        </tr>
        <wssuc:InputFormSection ID="InputFormSection2" Title="Email address" Description="Separated by a semicolon" runat="server">
            <Template_InputFormControls>
                <wssuc:InputFormControl ID="InputFormControl2" runat="server">
                    <Template_Control>
                        <asp:TextBox ID="txtContactEmail" runat="server" style="width:100%;" ></asp:TextBox>
                    </Template_Control>
                </wssuc:InputFormControl>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <tr>
            <td class="ms-sectionline" height="1" colspan="2">
                <img width="1" height="1" alt="" src="/_layouts/images/blank.gif" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="color: #525252; font-size: 1.4em; font-weight: bold; margin: 0;
                text-align: left; padding: 10px 6px; background-color: #F1F1F2;">
                Email Setting
            </td>
        </tr>
        <wssuc:InputFormSection ID="InputFormSection3" Title="Email title" Description="Title of email" runat="server">
            <Template_InputFormControls>
                <wssuc:InputFormControl ID="InputFormControl3" runat="server">
                    <Template_Control>
                        <asp:TextBox ID="txtEmailTitle" runat="server" style="width:100%;" ></asp:TextBox>
                        <asp:CheckBox ID="chkAddDate" Text="Add Type of Enquiry to title" runat="server" />
                    </Template_Control>
                </wssuc:InputFormControl>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection ID="InputFormSection4" Title="Email title" Description="Title of email" runat="server">
            <Template_InputFormControls>
                <wssuc:InputFormControl ID="InputFormControl4" runat="server">
                    <Template_Control>
                        <asp:TextBox ID="txtHtmlBody" runat="server" TextMode="MultiLine" Rows="8" style="width:100%;" ></asp:TextBox>
                    </Template_Control>
                </wssuc:InputFormControl>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <tr>
            <td class="ms-sectionline" height="1" colspan="2">
                <img width="1" height="1" alt="" src="/_layouts/images/blank.gif" />
            </td>
        </tr>
        <wssuc:ButtonSection ID="ButtonSection1" runat="server" ShowStandardCancelButton="false">
            <Template_Buttons>
                <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="<%$Resources:wss,multipages_okbutton_text%>"
                    ID="btnSave" UseSubmitBehavior="false" AccessKey="<%$Resources:wss,okbutton_accesskey%>" />
                <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Cancel" ID="btnCancel"
                    CausesValidation="False" />
            </Template_Buttons>
        </wssuc:ButtonSection>
    </table>
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Application Page
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    My Application Page
</asp:Content>
