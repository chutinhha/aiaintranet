<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoAlertSettings.aspx.cs" Inherits=" AIA.Intranet.Infrastructure.Layouts.AutoAlertSettings" DynamicMasterPageFile="~masterurl/default.master" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

<table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0">
	<!-- content types -->
	
    <wssuc:InputFormSection runat="server"
		id="InputFormSection1"
		Title="Schedule Setting"
		>
		<Template_Description>
			<SharePoint:FormattedStringWithListType ID="FormattedStringWithListType2" runat="server"
				String="hehe" />
		</Template_Description>
		<Template_InputFormControls>
			<wssuc:InputFormControl ID="InputFormControl2" runat="server"
				LabelText=""
				>
				<Template_Control>
					<asp:CheckBox runat="server" ID="chkEnable" Text="Enable" />
                    <br />
                    Schedule:
                    <asp:RadioButton runat="server" Text="Immediately" />
                    <asp:RadioButton ID="RadioButton1" runat="server" Text="Immediately" />
                    <asp:RadioButton ID="RadioButton2" runat="server" Text="Send After" />
                    <asp:TextBox runat="server" id="txtDelay" /> Minute(s)
				</Template_Control>
			</wssuc:InputFormControl>
		</Template_InputFormControls>
	</wssuc:InputFormSection>

    <wssuc:InputFormSection runat="server"
		id="ScheduleSettings1"
		Title="Target users"
		>
		<Template_Description>
			<SharePoint:FormattedStringWithListType ID="FormattedStringWithListType1" runat="server"
				String="Select when it allow select target users to recieve email" />
		</Template_Description>
		<Template_InputFormControls>
			<wssuc:InputFormControl ID="InputFormControl1" runat="server"
				LabelText=""
				>
				<Template_Control>
					<table class="ms-authoringcontrols" width="100%">
						<tr>
							<td nowrap="nowrap" width="50%">
								<asp:CheckBox runat="server" Text="Internal User" />
							</td>
							<td nowrap="nowrap">
								<SharePoint:PeopleEditor runat="server" />
							</td>
						</tr>
                       
                       <tr>
							<td nowrap="nowrap" width="50%">
								<asp:CheckBox ID="CheckBox1" runat="server" Text="Select from columns User" />
							</td>
							<td nowrap="nowrap">
								<asp:ListBox runat="server"></asp:ListBox>
							</td>
						</tr>
                        <tr>
							<td nowrap="nowrap" width="50%">
								<asp:CheckBox ID="CheckBox2" runat="server" Text="Author" />
							</td>
							<td nowrap="nowrap">
								
							</td>
						</tr>
					</table>
				</Template_Control>
			</wssuc:InputFormControl>
		</Template_InputFormControls>
	</wssuc:InputFormSection>

<wssuc:InputFormSection runat="server" id="pushDownSection" 
		Title="<%$Resources:wss,changecontenttypeoptionalsettings_pushdown_header%>"
		Description="<%$Resources:wss,changecontenttypeoptionalsettings_pushdown_description%>">
		<Template_InputFormControls>
			<wssuc:InputFormControl ID="InputFormControl3" runat="server" LabelText="<%$Resources:wss,changecontenttypeoptionalsettings_pushdown_instruction%>">
			</wssuc:InputFormControl>
			<wssawc:InputFormRadioButton GroupName="rdoPushDown" id="radPushDownYes"
				LabelText="<%$Resources:wss,changecontenttypeoptionalsettings_pushdown_yes%>"
				runat="server">
			</wssawc:InputFormRadioButton>
			<wssawc:InputFormRadioButton GroupName="rdoPushDown" id="radPushDownNo"
				LabelText="<%$Resources:wss,changecontenttypeoptionalsettings_pushdown_no%>"
				Checked="true"
				runat="server">
			</wssawc:InputFormRadioButton>
		</Template_InputFormControls>
	</wssuc:InputFormSection>
	<wssuc:ButtonSection ID="ButtonSection1" runat="server" ShowStandardCancelButton="false">
		<Template_Buttons>
			<asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Remove" ID="btnRemove" CausesValidation="False" Enabled="False" Visible="false" />
			<asp:Button UseSubmitBehavior="false" runat="server" class="ms-ButtonHeightWidth" Text="<%$Resources:wss,multipages_okbutton_text%>" id="btnSave" accesskey="<%$Resources:wss,okbutton_accesskey%>" ValidationGroup="SubmitValidate"/>
			<asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Cancel" ID="btnCancel"
			CausesValidation="False" />
			
		</Template_Buttons>
	</wssuc:ButtonSection>
	
    </table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">

</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >

</asp:Content>
