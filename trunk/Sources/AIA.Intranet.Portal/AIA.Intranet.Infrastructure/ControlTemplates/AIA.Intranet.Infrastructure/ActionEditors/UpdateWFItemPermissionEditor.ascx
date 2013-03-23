<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateWFItemPermissionEditor.ascx.cs" Inherits="AIA.Intranet.Infrastructure.Controls.UpdateWFItemPermissionEditor" %>

<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>

<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>

<wssawc:FormDigest runat="server" id="FormDigest" />
<div style="color:red" id="errorMessageHolder" enableviewstate="false" runat="server"></div>
<script type="text/javascript">
        function <%=chkRemoveAction.ClientID%>_click(control){
            ValidatorEnable(document.getElementById('<%=ddlTaskColumnValidator.ClientID%>'),!control.checked);
        }
    </script>
    
<TABLE border="0" width="95%" cellspacing="0" cellpadding="0" class="ms-formtable" >
	<wssuc:InputFormSection runat="server" Title="Update permissions of item">
		<Template_Description>
			<SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="Use this action to update Workflow item permission" EncodeMethod='HtmlEncode'/>
		</Template_Description>
		<Template_InputFormControls>
		
			<wssuc:InputFormControl runat="server" LabelText="Role Definition">
			<Template_Control>
			    <asp:DropDownList ID="ddlRoleDefinitions" runat="server" CssClass="ms-long" AutoPostBack="true" CausesValidation="false" /> 
			    <wssawc:InputFormRequiredFieldValidator ID="ddlTaskColumnValidator" 
				                                        ControlToValidate="ddlRoleDefinitions"
					                                    Display="Dynamic" 
					                                    ErrorMessage="Please select a column" 
					                                    Runat="server"
                                                        ValidationGroup="SubmitValidate"/>
            <br />
            <asp:CheckBox Text="Keep current permission" runat="server" ID="chkKeepExisting" />
            <br />

             <asp:CheckBox Text="All approvers" runat="server"  id="chkApprovers"/> <br />
             <label> Select users from metadata:</label><br />

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
				    <td align="center" valign="middle" class="ms-input"><button class="ms-ButtonHeightWidth" id="AddButton" runat="server"> <SharePoint:EncodedLiteral ID="EncodedLiteral2" runat="server" text="<%$Resources:wss,multipages_gip_add%>" EncodeMethod='HtmlEncode'/> </button><br />
					    <br /><button class="ms-ButtonHeightWidth" id="RemoveButton" runat="server"> <SharePoint:EncodedLiteral ID="EncodedLiteral3" runat="server" text="<%$Resources:wss,multipages_gip_remove%>" EncodeMethod='HtmlEncode'/> </button>
				    </td>
				    <td style="padding-left:10px">
				    <td class="ms-input">
					    <SharePoint:SPHtmlSelect id="SelectResult" width="143" height="125" runat="server" multiple="true" />
				    </td>
			    </tr>
		    </table>
            <br />
                <label>Specific users/groups:</label>
                <table border="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top" class="ms-formbody">
                            <SharePoint:PeopleEditor runat="server" ID="peSpecificUsersGroups" Width="100%" MultiSelect="true" SelectionSet="User,SecGroup,SPGroup"/>
                        </td>
                    </tr>
                </table>
			</Template_Control>
			</wssuc:InputFormControl>
			
           

			
            <asp:PlaceHolder runat="server" ID="ltrHolder" />
			</Template_Control>
			</wssuc:InputFormControl>
			<wssuc:InputFormControl runat="server">
			<Template_Control>
			    <asp:CheckBox ID="chkRemoveAction" Text="Remove this action" runat="server" />
			</Template_Control>
			</wssuc:InputFormControl>

		</Template_InputFormControls>
	</wssuc:InputFormSection>
</TABLE>
<asp:Literal ID="ltrScript" runat="server"/>