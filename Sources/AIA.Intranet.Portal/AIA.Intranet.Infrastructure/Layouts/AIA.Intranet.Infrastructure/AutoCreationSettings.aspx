<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoCreationSettings.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Layouts.AutoCreationSettingPage" DynamicMasterPageFile="~masterurl/default.master" %>

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
        <wssuc:InputFormSection ID="InputFormSection1" Title="List creation settings" Description=""
            runat="server">
            <template_inputformcontrols>
         <wssuc:InputFormControl ID="InputFormControl1" runat="server">
				 <Template_Control>
                 
				   <asp:checkbox runat="server" id="chkListEnable" Text="Enable create list/library"></asp:checkbox> <br />
                     <asp:Label Text="Select a list template" runat="server" />
                   <asp:DropDownList runat="server" ID="ddlTemplates"></asp:DropDownList> <br />
                   <asp:Label ID="Label1" Text="Select content type to add to created list" runat="server" /> 
                     <br />
                   <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <br />
                        <asp:Label Text="Content type groups" runat="server" />
                        <asp:DropDownList runat="server" id="ddlGroup" AutoPostBack="true"  CausesValidation="false">
                            
                        </asp:DropDownList>
                        <table>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lstAvailable" runat="server" SelectionMode="Multiple" Rows="12" Width="250"></asp:ListBox>
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnMoveRight" Text=" -->> " /> <br/>
                                    <asp:Button runat="server" ID="btnMoveLeft" Text=" <<-- " /> <br/>
                                </td>
                                <td>
                                    <asp:ListBox ID="lstSelected" runat="server" SelectionMode="Multiple" Rows="12" Width="250"></asp:ListBox>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnMoveRight" />
                        <asp:PostBackTrigger ControlID="btnMoveLeft" />
                        <asp:PostBackTrigger ControlID="ddlGroup" />
                    </Triggers>
                   </asp:UpdatePanel>
                  
                   <br />
                     

				 </Template_Control>
                 </wssuc:InputFormControl>
        <wssuc:InputFormControl ID="InputFormControl4" runat="server" LabelText="List name formula : ([Title] - [Content Type])">
		<Template_Control>
            <asp:TextBox runat="server" ID="txtListName" CssClass="ms-long" />
        </Template_Control>
        </wssuc:InputFormControl>
        <wssuc:InputFormControl ID="InputFormControl5" runat="server" LabelText="List Url formula : ([Title] - [Content Type])">
		<Template_Control>
            <asp:TextBox runat="server" ID="txtListUrl" CssClass="ms-long" />
        </Template_Control>


        </wssuc:InputFormControl>

         <wssuc:InputFormControl ID="InputFormControl6" runat="server">
				         <Template_Control>
				           <asp:checkbox runat="server" id="chkHidden" Text="Hidden"></asp:checkbox> <br />
                           <asp:checkbox runat="server" id="chkQuicklaunch" Text="Show on quicklaunch"></asp:checkbox><br />
				     </Template_Control>
                 </wssuc:InputFormControl>

<wssuc:InputFormControl ID="InputFormControl7" runat="server" LabelText="Url field to be updated">
		<Template_Control>
            <asp:TextBox runat="server" ID="txtUrlFieldName" CssClass="ms-long" />
        </Template_Control>


        </wssuc:InputFormControl>

	   </template_inputformcontrols>
        </wssuc:InputFormSection>

        <%-- Navigation update --%>
        <wssuc:InputFormSection ID="InputFormSection4" Title="Navigation update" Description=""
            runat="server">
            <Template_InputFormControls>
                <wssuc:InputFormControl ID="InputFormControl8" runat="server">
                    <Template_Control>
                        <asp:CheckBox runat="server" ID="chkEnableNavigationUpdate" Text="Enable navigation update"></asp:CheckBox>
                    </Template_Control>
                </wssuc:InputFormControl>
                <wssuc:InputFormControl ID="InputFormControl9" runat="server" LabelText="NavigatonKey">
                    <Template_Control>
                        <asp:TextBox runat="server" ID="txtNavigationKey" CssClass="ms-long" />
                    </Template_Control>
                </wssuc:InputFormControl>
                <wssuc:InputFormControl ID="InputFormControl10" runat="server" LabelText="Navigaton name formula : ([Title] - [Content Type])">
                    <Template_Control>
                        <asp:TextBox runat="server" ID="txtNavigationName" CssClass="ms-long" />
                    </Template_Control>
                </wssuc:InputFormControl>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <%-- END Navigation update --%>

        <wssuc:InputFormSection ID="InputFormSection2" Title="Trigger settings" Description=""
            runat="server">
            <template_inputformcontrols>
                 <wssuc:InputFormControl ID="InputFormControl2" runat="server">
				         <Template_Control>
				           <asp:checkbox runat="server" id="chkOnCreate" Text="On Created"></asp:checkbox> <br />
                           <asp:checkbox runat="server" id="chkOnChange" Text="On Changed"></asp:checkbox><br />
                           <asp:checkbox runat="server" id="chkFirstCheckIn" Text="On First Checkin"></asp:checkbox><br />
				     </Template_Control>
                 </wssuc:InputFormControl>
	   </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection ID="InputFormSection3" Title="Conditional settings" Description=""
            runat="server">
            <template_inputformcontrols>
         <wssuc:InputFormControl ID="InputFormControl3" runat="server">
				 <Template_Control>
                 
				   <asp:checkbox runat="server" id="Checkbox1" Text="Enable conditional "></asp:checkbox> <br />
                
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
    Auto creation settings
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Auto creation settings
</asp:Content>
