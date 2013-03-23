<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotificationSettingControl.ascx.cs" Inherits="AIA.Intranet.Infrastructure.Controls.NotificationSettingControl" %>

<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>
<%@ Register TagPrefix="uc" TagName="EmailSelector" Src="~/_controltemplates/AIA.Intranet.Infrastructure/EmailTemplateSelector.ascx" %>

<table border="0" width="100%" cellspacing="0" cellpadding="0">
        <wssuc:InputFormSection ID="InputFormSection1" Title="Notification setting" Description=""
            runat="server">
            <template_inputformcontrols>
         <wssuc:InputFormControl ID="InputFormControl1" runat="server">
				 <Template_Control>
                 
				   <asp:checkbox runat="server" id="chkEnable" Text="Enable"></asp:checkbox>
                   <uc:EmailSelector runat="server" ID="notifyEmail" />

                   

				 </Template_Control>
                 </wssuc:InputFormControl>
	   </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection ID="InputFormSection21" Title="Target users" Description=""
            runat="server">
            <template_inputformcontrols>
         <wssuc:InputFormControl ID="InputFormControl12" runat="server">
				 <Template_Control>
                         <asp:checkbox runat="server" id="chkMaillist" Text="Send to maillist"></asp:checkbox><br />

                        <SharePoint:GroupedItemPicker id="MultiLookupPicker" runat="server"
			                CandidateControlId="SelectCandidate"
			                ResultControlId="SelectResult"
			                AddButtonId="AddButton"
			                RemoveButtonId="RemoveButton"
			                />
		                <table class="ms-long" cellpadding="0" cellspacing="0" border="0">
			                <tr>
				                <td class="ms-input">
					                <SharePoint:SPHtmlSelect id="SelectCandidate" width="143" height="125" runat="server" multiple="true" />
				                </td>
				                <td style="padding-left:10px">
				                <td align="center" valign="middle" class="ms-input"><button class="ms-ButtonHeightWidth" id="AddButton" runat="server"> <SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="<%$Resources:wss,multipages_gip_add%>" EncodeMethod='HtmlEncode'/> </button><br />
					                <br /><button class="ms-ButtonHeightWidth" id="RemoveButton" runat="server"> <SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="<%$Resources:wss,multipages_gip_remove%>" EncodeMethod='HtmlEncode'/> </button>
				                </td>
				                <td style="padding-left:10px">
				                <td class="ms-input">
					                <SharePoint:SPHtmlSelect id="SelectResult" width="143" height="125" runat="server" multiple="true" />
				                </td>
			                </tr>
		                </table>
                   
                        <asp:checkbox runat="server" id="chkAllUsers" Text="All users"></asp:checkbox><br />
                        <asp:checkbox runat="server" id="chkSpecifyUsers" Text="Below users or groups"></asp:checkbox><br />
                        <SharePoint:PeopleEditor runat="server" ID="ppUsers" SelectionSet="User, SPGroup" MultiSelect="true" Width="275px" /> <br/>
                        <asp:checkbox runat="server" id="chkMetadata" Text="Select from metadata"></asp:checkbox><br />
                        <asp:listbox runat="server" Rows="10" ID="lstColumns" width="250px" SelectionMode="Multiple"></asp:listbox>
                   

				 </Template_Control>
                 </wssuc:InputFormControl>
	   </template_inputformcontrols>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection ID="InputFormSection4" Title="Infom Users" Description=""
            runat="server">
            <template_inputformcontrols>
         <wssuc:InputFormControl ID="InputFormControl4" runat="server">
				 <Template_Control>
                        <SharePoint:PeopleEditor runat="server" ID="ppInfomUsers" SelectionSet="User, SPGroup" MultiSelect="true" Width="275px" /> <br/>
                         or cc this notification for metadata below <br />
                        <asp:listbox runat="server" Rows="10" ID="lstCCMetadata" width="250px" SelectionMode="Multiple"></asp:listbox>
				 </Template_Control>
                 </wssuc:InputFormControl>
	   </template_inputformcontrols>
        </wssuc:InputFormSection>

        <wssuc:InputFormSection ID="InputFormSection2" Title="Trigger settings" Description=""
            runat="server">
            <template_inputformcontrols>
         <wssuc:InputFormControl ID="InputFormControl2" runat="server">
				 <Template_Control>
                 
				   <asp:checkbox runat="server" id="chkOnCreate" Text="On Created"></asp:checkbox> <br />
                   <asp:checkbox runat="server" id="chkOnChange" Text="On Changed"></asp:checkbox><br />

                   <asp:checkbox runat="server" id="chkFirstCheckIn" Text="On First Checkin"></asp:checkbox><br />
                   
                 <%--  <wssuc:InputFormControl ID="InputFormControl3" runat="server" LabelText="Template Name">
                        <asp:checkbox runat="server" id="chkAdd" Text="On First Checkin"></asp:checkbox>
                   </wssuc:InputFormControl>--%>
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
		
            <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Edit" ID="btnEdit"
            CausesValidation="False" />

		</template_buttons>
        </wssuc:ButtonSection>


    </table>
