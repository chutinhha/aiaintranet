<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnreadContentNotificationSettings.aspx.cs"
    Inherits="AIA.Intranet.Infrastructure.Layouts.UnreadContentNotificationSettings"
    DynamicMasterPageFile="~masterurl/default.master" %>


<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>
<%@ Register TagPrefix="uc" TagName="EmailSelector" Src="~/_controltemplates/AIA.Intranet.Infrastructure/EmailTemplateSelector.ascx" %>


<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
	<a id="onetidListHlink" HREF=<%SPHttpUtility.AddQuote(SPHttpUtility.UrlPathEncode(CurrentList.DefaultViewUrl,true),Response.Output);%>><%SPHttpUtility.HtmlEncode(CurrentList.Title, Response.Output);%></a>&#32;
    <SharePoint:ClusteredDirectionalSeparatorArrow ID="ClusteredDirectionalSeparatorArrow1" runat="server" /> 
    Unread content settingsa
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table border="0" width="100%" cellspacing="0" cellpadding="0">
        <wssuc:inputformsection id="InputFormSection1" title="Unread Notification setting" description=""
            runat="server">
            <template_inputformcontrols>
         <wssuc:InputFormControl ID="InputFormControl1" runat="server">
				 <Template_Control>
                     
				   <asp:checkbox runat="server" id="chkEnable" Text="Enable" AutoPostBack="true"></asp:checkbox>
                   <br />
                   <asp:checkbox runat="server" id="chkEnableCreateUnreadTask" Text="Create unread document task" Enabled="false" AutoPostBack="true"></asp:checkbox>
                   <br />
                   <asp:TextBox runat="server" ID="txtCreateUnreadTask" Enable="false" ></asp:TextBox>
                   <br />
                   <asp:checkbox runat="server" id="chkEnableSendEmail" Text="Enable send notification email" Enabled="false" AutoPostBack="true"></asp:checkbox>
                   <br />
                   <uc:EmailSelector runat="server" ID="notifyEmail" AllowNull="true"/>
                   <br />
				 </Template_Control>
                 </wssuc:InputFormControl>
	   </template_inputformcontrols>
        </wssuc:inputformsection>
        <wssuc:buttonsection id="ButtonSection1" runat="server" showstandardcancelbutton="false">
            <template_buttons>
			<asp:Button UseSubmitBehavior="false"  runat="server" class="ms-ButtonHeightWidth" Text="<%$Resources:wss,multipages_okbutton_text%>" 
			id="btnSave" accesskey="<%$Resources:wss,okbutton_accesskey%>" />			
			
			
            <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Delete" ID="btnDelete"
            CausesValidation="False" Visible="false" />

            <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Cancel" ID="btnCancel"
            CausesValidation="False" />

		</template_buttons>
        </wssuc:buttonsection>
    </table>
</asp:Content>
