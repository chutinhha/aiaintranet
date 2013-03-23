<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RedirectSettings.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Pages.RedirectSettingsPage" DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>
<%@ Register TagPrefix="uc" TagName="EmailSelector" Src="~/_controltemplates/AIA.Intranet.Infrastructure/EmailTemplateSelector.ascx" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
       <table border="0" width="100%" cellspacing="0" cellpadding="0">
        <wssuc:InputFormSection ID="InputFormSection1" Title="Notification setting" Description=""
            runat="server">
            <template_inputformcontrols>
                 <wssuc:InputFormControl ID="InputFormControl1" runat="server">
				         <Template_Control>
                            <asp:CheckBox runat="server" ID="chkEnable" Text="Enable" /> <br />

                            <asp:RadioButton runat="server" ID="radHomePage" GroupName="AAA"  Text="Redirect to home page"/> <br />
				           
                           <asp:RadioButton runat="server" ID="radToUrl" GroupName="AAA" Text="Redirect to specific url"/> <br />
                           <asp:TextBox runat="server" ID="txtUrl" class="ms-long"></asp:TextBox> <br/>
                   
							<asp:RadioButton runat="server" ID="radShowError" GroupName="AAA" Text="Show error message"/> <br />
                           <asp:TextBox runat="server" ID="txtErrorMessage" TextMode="MultiLine" Rows="3" Columns="45"></asp:TextBox>
                           <br />
                   

				         </Template_Control>
                 </wssuc:InputFormControl>
	         </template_inputformcontrols>
        </wssuc:InputFormSection>

        <wssuc:ButtonSection ID="ButtonSection1" runat="server" ShowStandardCancelButton="false">
            <template_buttons>
			<asp:Button UseSubmitBehavior="false"  runat="server" class="ms-ButtonHeightWidth" Text="<%$Resources:wss,multipages_okbutton_text%>" 
			id="btnSave" accesskey="<%$Resources:wss,okbutton_accesskey%>" />			
			
			
            <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Delete" ID="btnDelete"
            CausesValidation="False" Visible="false" />

            <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Cancel" ID="btnCancel"
            CausesValidation="False" />

		</template_buttons>
        </wssuc:ButtonSection>

       </table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
