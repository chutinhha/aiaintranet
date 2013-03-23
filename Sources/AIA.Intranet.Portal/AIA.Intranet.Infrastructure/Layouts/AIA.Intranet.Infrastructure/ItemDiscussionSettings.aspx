<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemDiscussionSettings.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Layouts.ItemDiscussionSettings" DynamicMasterPageFile="~masterurl/default.master" %>


<asp:Content ID="Content1" contentplaceholderid="PlaceHolderPageTitle" runat="server">
    Item Discussion Settings
</asp:Content>

<asp:Content ID="Content2" contentplaceholderid="PlaceHolderPageTitleInTitleArea" runat="server">
    Item Discussion Settings
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    
    <SharePoint:FormDigest runat="server" id="FormDigest" />
    
	<table border="0" cellspacing="0" cellpadding="0" class="ms-propertysheet" width="100%">
		<wssuc:InputFormSection Title="Enable Document Discussions" 
				Description="Allow all documents in this library to be tied to a thread in a discussion board."
				runat="server">
            <Template_InputFormControls>
                <wssawc:InputFormCheckBox ID="enableDocumentDiscussions" LabelText="Enable" runat="server"/>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection Title="Document Discussion Board" 
				Description="Set the location where document threads will be created."
				runat="server">
            <Template_InputFormControls> <!-- Orginally had __spRootFolderUrl for value.  It returns the relative list url -->
                <asp:DropDownList runat="server" ID="listSelection" AppendDataBoundItems="True" DataSourceID="ListsDS" DataTextField="__spTitle" DataValueField="__spTitle">
                    <asp:ListItem Value="" Text="---Select Discussion Board---" />
                </asp:DropDownList>
            </Template_InputFormControls>
        </wssuc:InputFormSection>

		<wssuc:ButtonSection runat="server" ShowStandardCancelButton="false">
			<Template_Buttons>
				<asp:PlaceHolder ID="PlaceHolder1" runat="server">				
						<asp:Button runat="server" class="ms-ButtonHeightWidth" OnClick="BtnSave_Click" Text="<%$Resources:wss,multipages_okbutton_text%>" id="BtnSave" accesskey="<%$Resources:wss,okbutton_accesskey%>"/>
				</asp:PlaceHolder>					
				<asp:PlaceHolder ID="PlaceHolder2" runat="server">					
						<asp:Button runat="server" class="ms-ButtonHeightWidth" OnClick="BtnCancel_Click" Text="<%$Resources:wss,multipages_cancelbutton_text%>" id="BtnCancel" CausesValidation="false"/>
				</asp:PlaceHolder>
			</Template_Buttons>
		</wssuc:ButtonSection>
	</table>
		
	<SharePoint:SPDataSource 
        ID="ListsDS" 
        runat="server"
        DataSourceMode="ListOfLists" >
    </SharePoint:SPDataSource>

</asp:Content>