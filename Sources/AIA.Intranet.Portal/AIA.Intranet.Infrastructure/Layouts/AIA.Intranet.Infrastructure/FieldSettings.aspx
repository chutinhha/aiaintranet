<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FieldSettings.aspx.cs"
    Inherits="AIA.Intranet.Infrastructure.Layouts.FieldSettings" DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="LinkSection" Src="/_controltemplates/LinkSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ActionBar" Src="/_controltemplates/ActionBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="/_controltemplates/ToolBarButton.ascx" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table class="ms-propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0">
        <!-- Columns: New -->
        <wssuc:InputFormSection ID="ColumnsNewSection" Title="Hide Permission Setting" Description="Select groups or users to hide field on New/Edit/View"
            runat="server">
            <Template_InputFormControls>
                <wssuc:InputFormControl runat="server">
                    <Template_Control>
                        <table border="0" width="100%" cellspacing="0" cellpadding="2">
                            <tr>
                                <th scope="col" class="ms-authoringcontrols" valign="top">
                                    <b>
                                        <SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" Text="Column" EncodeMethod='HtmlEncode' /></b>
                                </th>
                                <th scope="col" class="ms-authoringcontrols" style="text-align: center; width: 150px;"
                                    valign="top">
                                    <b>New</b>
                                </th>
                                <th scope="col" class="ms-authoringcontrols" style="text-align: center; width: 150px;"
                                    valign="top">
                                    <b>Edit</b>
                                </th>
                                <th scope="col" class="ms-authoringcontrols" style="text-align: center; width: 150px;"
                                    valign="top">
                                    <b>Display</b>
                                </th>
                            </tr>
                            <asp:Repeater runat="server" ID="fieldRepeater">
                                <ItemTemplate>
                                    <tr>
                                        <td class="ms-authoringcontrols" valign="top">
                                            <asp:Literal runat="server" id="titleLiteral" Text="">
                                               
                                            </asp:Literal>
                                             <asp:HiddenField runat="server" ID="fieldIdHidden" />
                                             
                                        </td>
                                        <td class="ms-authoringcontrols" style="text-align: left; border-left: dotted 1px #222" valign="top">
                                           <asp:CheckBoxList runat="server" ID="addPermissionChecks" RepeatColumns="2" RepeatDirection="Horizontal" width="300px"></asp:CheckBoxList>
                                           <SharePoint:PeopleEditor runat="server" ID="newItemPP" SelectionSet="User, SPGroup" MultiSelect="true" Width="275px" />
                                        </td>
                                        <td class="ms-authoringcontrols" style="text-align: left;  border-left: dotted 1px #222" valign="top">
                                            <asp:CheckBoxList ID="editPermissionChecks" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" width="300px"></asp:CheckBoxList>
                                            <SharePoint:PeopleEditor runat="server" ID="editItemPP" SelectionSet="User, SPGroup" MultiSelect="true" Width="275px" />
                                        </td>
                                        <td class="ms-authoringcontrols" style="text-align: left;  border-left: dotted 1px #222; border-right: dotted 1px #222"" valign="top">
                                          <asp:CheckBoxList ID="viewPermissionChecks" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" width="300px"></asp:CheckBoxList>
                                          <SharePoint:PeopleEditor runat="server" ID="viewItemPP"  SelectionSet="User, SPGroup"  MultiSelect="true" Width="275px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="border-bottom: solid #2222 1px">&nbsp;</td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </Template_Control>
                </wssuc:InputFormControl>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <wssuc:ButtonSection ID="ButtonSection1" runat="server" ShowStandardCancelButton="false">
		<Template_Buttons>
			<asp:Button UseSubmitBehavior="false"  runat="server" class="ms-ButtonHeightWidth" Text="<%$Resources:wss,multipages_okbutton_text%>" 
			id="btnSave" accesskey="<%$Resources:wss,okbutton_accesskey%>" />			
			
			<asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Cancel" ID="btnCancel"
            CausesValidation="False" />

            <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Delete" ID="btnDelete"
            CausesValidation="False" />
		</Template_Buttons>
	</wssuc:ButtonSection>

    </table>
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Field Permission Settings
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
    Use this page to modify the Field permission setting for Edit/View/Add base on groups or users
</asp:Content>

<asp:Content ID="Content1" contentplaceholderid="PlaceHolderPageTitleInTitleArea" runat="server">
	<a id=onetidListHlink HREF=<%SPHttpUtility.AddQuote(SPHttpUtility.UrlPathEncode(CurrentList.DefaultViewUrl,true),Response.Output);%>><%SPHttpUtility.HtmlEncode(CurrentList.Title, Response.Output);%></a>&#32;<SharePoint:ClusteredDirectionalSeparatorArrow ID="ClusteredDirectionalSeparatorArrow1" runat="server" /> <a HREF=<%SPHttpUtility.AddQuote(SPHttpUtility.UrlPathEncode("listedit.aspx?List=" + CurrentList.ID.ToString(),true),Response.Output);%>> <SharePoint:FormattedStringWithListType ID="FormattedStringWithListType1" runat="server" String="<%$Resources:wss,listsettings_titleintitlearea%>" LowerCase="false" /></a>&#32;<SharePoint:ClusteredDirectionalSeparatorArrow ID="ClusteredDirectionalSeparatorArrow2" runat="server" />
	<SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="Field Permission Settings" EncodeMethod='HtmlEncode'/>
</asp:Content>
