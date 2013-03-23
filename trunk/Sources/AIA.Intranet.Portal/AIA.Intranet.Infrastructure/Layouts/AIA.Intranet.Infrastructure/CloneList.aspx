<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CloneList.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Layouts.CloneList" DynamicMasterPageFile="~masterurl/default.master" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
	<TABLE border="0" width="100%" cellspacing="0" cellpadding="0" class="ms-descriptiontext">
	<Control id="SingleItemSection" runat="server">

	<wssuc:InputFormSection runat="server"
	    id="VersionCommentSection"
		Title="List Name">
		<Template_Description>
			<SharePoint:EncodedLiteral ID="EncodedLiteral3" runat="server" text="" EncodeMethod='HtmlEncode'/>
		</Template_Description>
		<Template_InputFormControls>
			<wssuc:InputFormControl runat="server"  LabelText="" >
			<Template_Control>
				<asp:TextBox TextMode="SingleLine" id="txtNewName" class="ms-long" runat="server" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNewName" Display="Dynamic" Text="please enter new list name" />
			</Template_Control>
			</wssuc:InputFormControl>
		</Template_InputFormControls>
	</wssuc:InputFormSection>
	</Control>

	<wssuc:ButtonSection runat="server">
		<Template_Buttons>
			<INPUT id="btnOK" runat="server" Type="button" AccessKey="<%$Resources:wss,multipages_okbutton_accesskey%>" class="ms-ButtonHeightWidth" Value="<%$Resources:wss,multipages_okbutton_text%>"  OnServerClick="OnSubmit"/>
		</Template_Buttons>
	</wssuc:ButtonSection>
	</TABLE>


</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Clone List
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Clone List
</asp:Content>
