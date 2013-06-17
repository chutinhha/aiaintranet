<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeMenuOrderNo.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Layouts.ChangeMenuOrderNo" DynamicMasterPageFile="~masterurl/default.master" %>

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
        <wssuc:inputformsection id="InputFormSection1" title="Change order number to" description="" runat="server">
            <template_inputformcontrols>
                <wssuc:InputFormControl ID="InputFormControl1" runat="server">
                    <Template_Control>
                        <asp:TextBox ID="txtOrderNo" runat="server" MaxLength="9"></asp:TextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please specify an order number" 
                            Display="Dynamic" ControlToValidate="txtOrderNo"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RangeValidator1" runat="server" Display="Dynamic" 
                            ErrorMessage="Please specify an order number between 1 and 999999999" ControlToValidate="txtOrderNo"
                            MinimumValue="1" MaximumValue="999999999" Type="Integer"></asp:RangeValidator>
                    </Template_Control>
                </wssuc:InputFormControl>
            </template_inputformcontrols>
        </wssuc:inputformsection>
        
        <wssuc:buttonsection id="ButtonSection1" runat="server" showstandardcancelbutton="false">
            <template_buttons>
			    <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="<%$Resources:wss,multipages_okbutton_text%>" 
			                id="btnSave" UseSubmitBehavior="false" accesskey="<%$Resources:wss,okbutton_accesskey%>" />			
			
                <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Delete" ID="btnDelete" CausesValidation="False" Visible="false" />

                <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Cancel" ID="btnCancel" CausesValidation="False" />
		</template_buttons>
        </wssuc:buttonsection>
    </table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">

</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >

</asp:Content>
