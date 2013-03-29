<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>

<%@ Assembly Name="AIA.Intranet.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=0b6a88a58a49868d" %>
<%@ Import Namespace="AIA.Intranet.Model"%>
<%@ Import Namespace="AIA.Intranet.Model.Workflow"%>

<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskConfigurationEditor.ascx.cs" Inherits="AIA.Intranet.Infrastructure.Controls.TaskConfigurationEditor" %>
<%--<%@ Register Tagprefix="CMS" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>--%>

<style type="text/css">
	.style1
	{
		width: 100px;
	}
	.style2
	{
		color: #FF3300;
	}
	.style3
	{
		color: #FF0000;
	}
	.ms-ButtonHeightWidth
	{
		width: 7.2em;
	}
	.EditedEvent
	{
		color: #72520B;
	}
</style>
<SharePoint:FormToolBar ID="FormToolBar1" runat="server" />
<table class="ms-formtable" style="margin-top: 8px;" border="0" cellpadding="2" cellspacing="0"
	width="750">
	
	<tr>
		<td valign="top">
			&nbsp;
		</td>
		<td align="right">
			<table class="style1"> 
				<tr>
					<td>
						<asp:Button ID="SaveButtonTop" runat="server" Text="Save" class="ms-ButtonHeightWidth" ValidationGroup="SubmitGroup" OnClick="SaveButton_Click" />
					</td>
					<td>
						<SharePoint:GoBackButton ID="GoBackButtonTop" runat="server">
						</SharePoint:GoBackButton>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	
	 <tr>
		<td colspan="2">
			<TABLE border="0" cellpadding="0" cellspacing="0" width="100%">
				<TR>
					<TD nowrap class="ms-linksectionheader" style="padding: 4px;" width="100%">
						<H3 class="ms-standardheader">
						General Information
						</H3>
					</TD>
					<tr> <td style="height:10px;"> </td></tr>
				</TR>
			</TABLE>
		</td>
	</tr>
	
	<tr>
		<td class="ms-formlabel" valign="top">
			Configuration Name <span class="style2">*</span></td>
		<td class="ms-formbody">
			<asp:TextBox ID="ConfigurationName" runat ="server" style="width:100%"></asp:TextBox> 
			<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter a name" ControlToValidate="ConfigurationName" ValidationGroup="SubmitGroup"></asp:RequiredFieldValidator>
			</td>
	</tr>
	<tr>
		<td class="ms-formlabel" valign="top">
			Workflow Tasks
		</td>
		<td class="ms-formbody">
			Assign to<asp:RadioButtonList ID="ExecutionModeRadioList" runat="server" class="ms-descriptiontext">
				<asp:ListItem Value="Parallel">All participants simultaneously (parallel)</asp:ListItem>
				<asp:ListItem Selected="True" Value="Sequence">One participant at a time (serial)</asp:ListItem>
			</asp:RadioButtonList>
		</td>
	</tr>
		<tr>
		<td class="ms-formlabel" valign="top">
		  <b>Participants&nbsp; <span class="style3">*</span></b>
		  </td>
		<td class="ms-formbody" valign="top">
		   <table cellpadding="3" cellspacing="3" class="ms-descriptiontext" class="ms-descriptiontext">
		   <tr>
				<td>
				<asp:RadioButton runat="server" ID="SpecifiedParticipantsRadio" AutoPostBack="true"
				 Checked="true" GroupName="Participants"  Text="Specified participants" OnCheckedChanged="ParticipantsRadioChange" />                                  
				</td>
		   </tr>
		   <div id="ApproversPeoplePickerDiv" runat="server" >
		   <tr>
					<td>
						 &nbsp;&nbsp;&nbsp;Type the names of people you want to participate when this workflow is started.<br />
						 &nbsp;&nbsp;&nbsp;Add names in the order in which you want the tasks assigned (for serial workflows).<br />
						 <SharePoint:PeopleEditor EnableBrowse="true" MultiSelect="true" runat="server" ID="ApproversPeoplePicker" SelectionSet="User,SPGroup"  AllowEmpty ="false" ValidatorEnabled="true" />
						 <asp:CustomValidator ID="validApproversPeoplePicker" runat="server" ErrorMessage="The participant is not a member of site." Display="Dynamic" ControlToValidate = "ApproversPeoplePicker" />
					</td>
			</tr> 
			</div> 
		   
		   <tr>
				<td>
				<asp:RadioButton runat="server" ID="UseMetadataAssignmentRadio" AutoPostBack="true"
				  GroupName="Participants" Text="Use metadata assignment" OnCheckedChanged="ParticipantsRadioChange"  />

				</td>
		   </tr>
		   
			 <div id="ApproversDropDownDiv" runat="server" visible="false">
		   <tr>
				<td>
					&nbsp;&nbsp;&nbsp;
					<asp:DropDownList runat="server"  ID="ApproversDropDown"></asp:DropDownList>
				</td>
		   </tr>
		   </div>
		   
			<tr>
				<td>
					<asp:CheckBox ID="DoNotExpandGroupCheckBox" runat="server" Text="Assign a single task to each group (Do not expand groups)" />
				</td>
			</tr>        
			
			<tr>
				<td>
					<asp:CheckBox ID="IgnoreIfNoParticipantCheckBox" runat="server" Text="Ignore if there are no participants" />
				</td>
			</tr>            
	
		   </table>    
		</td>
	</tr>
	
		<tr>
		<td class="ms-formlabel" valign="top">
			Complete the workflow</td>
		<td class="ms-formbody" valign="top">
				Complete this workflow when:
				<br />
				<asp:CheckBox ID="RequireNumberCheckbox" runat="server" Text="Following number of tasks are finished:" />
				<br />
				&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				<asp:TextBox ID="TasksTextBox" runat="server" class="ms-input" Size="11" Style="ime-mode: inactive;"></asp:TextBox>
				<asp:RangeValidator MaximumValue="365" MinimumValue="1" ID="RangeValidator4" runat="server"
				   ControlToValidate="TasksTextBox" ErrorMessage="Please enter a integer number larger than 1"
					Type="Integer" ValidationGroup="SubmitGroup"></asp:RangeValidator>            
		</td>
	</tr>
	<tr>
		<td colspan="2" style="height:20px"> 
		</td>
	</tr>

	<tr>
		<td colspan="2">
			<TABLE border="0" cellpadding="0" cellspacing="0" width="100%">
				<TR>
					<TD nowrap class="ms-linksectionheader" style="padding: 4px;" width="100%">
						<H3 class="ms-standardheader">
						 Task Configuration
						</H3>
					</TD>
					<tr> <td style="height:10px;"> </td></tr>
				</TR>
			</TABLE>
		</td>
	</tr>
	
	<tr >
		<td class="ms-formlabel" valign="top">
			Default Task Values
		</td>
		<td class="ms-formbody">
			<table cellpadding="3" cellspacing="3" class="ms-descriptiontext" class="ms-descriptiontext" >
				<tr>
						<td valign="top" align="right">
							<b>Task title prefix</b>
						</td>
						<td>
							<span style="padding-bottom:5px">Type a title prefix to include with your request</span> <br />
							<asp:TextBox ID="TaskTitlePrefixTextBox" runat="server" TextMode="SingleLine" Columns="73" Rows="5" />
						</td>
				</tr>
				
				<tr>
						<td valign="top" align="right">
							<b>Task Instructions</b>
						</td>
						<td>
							<span>Type a message to include with your request:</span> <br />
							<asp:TextBox ID="TaskInstructionsTextBox" runat="server" TextMode="MultiLine" Columns="60" Rows="5" />
						</td>
				</tr>
				<tr>
					<td valign="top" align="right" colspan="2">
						&nbsp;
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b>Task content type</b>
					</td>
					<td>
						<asp:DropDownList ID="TaskContentTypesDropDown" AutoPostBack="true" onChange="alert('You must update the Completion Rules after the content type has been changed!')" OnSelectedIndexChanged="TaskContentTypesDropDown_SelectedIndexChanged" runat="server" EnableViewState="true" >
						</asp:DropDownList>
						<a href="#" onclick="showContentTypeEditorDialog();return false" id="createLink" runat="server">Create new</a>
						<table class="ms-descriptiontext" id="WorkflowTaskSettingTable" runat="server" visible="false">
							<tr><td colspan="2">&nbsp;</td></tr>
							<tr><td colspan="2"><asp:CheckBox ID="ReassignCheckBox" runat="server" Text="Reassign the task to another person" /></td></tr>
							<tr><td></td><td>&nbsp;&nbsp;&nbsp;<asp:CheckBox Enabled="false" ID="AllowDueDateChangeRessignmentCheckBox" runat="server" Text="Allow Due Date Change on Reassignment" /></td></tr>
							<tr><td colspan="2">&nbsp;</td></tr>
							
							<tr>
								<td colspan="2">
									<asp:CheckBox ID="RequestInfomationCheckBox" runat="server" Text="Request Information before completing the task" />
								</td>
							</tr>
							<tr>
								<td></td>
								<td>
									&nbsp;&nbsp;&nbsp;<asp:CheckBox  Enabled="false" ID="AllowDueDateChangeRequestInfomationCheckBox" runat="server" Text="Allow Due Date Change on Request Information" />
								</td>
							</tr>
						</table>
						<table class="ms-descriptiontext" id="WorkflowTaskExSettingTable" runat="server" visible="false">
							<tr>
								<td >
									<asp:CheckBox ID="PlaceHoldOnCheckBox" runat="server"  Text="Allow Place on Hold" />
								</td>
						   </tr>
							<tr>
								<td >
									<asp:CheckBox ID="SendEECCheckBox" runat="server"  Text="Allow Send External Email Collaboration" />
								</td>
						   </tr>
						</table>
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b>&nbsp;&nbsp;</b>
					</td>
					<td>
						&nbsp;&nbsp;
					</td>
				</tr>
				 <tr>
					<td valign="top" align="right">
						<b>Completion Rules</b>
					</td>
					<td>
						<table class="ms-descriptiontext" border="0" cellpadding="0" cellspacing="0 " >
						<div id="AllTaskRuleOfCCIappWorkflowTaskDiv" runat="server" visible="true">
							<tr>
								<td style="padding-bottom:5px"><img alt="" src="/_layouts/images/square.gif">
								</td>
								<td  style="padding-left:5px;padding-bottom:5px">
									<a id="A2" href="#" onclick="showTaskRuleEditorDialog(0);return false"  runat="server">Task Approved rule</a>
									
								</td>
								<td  style="padding-left:5px;padding-bottom:5px">
									<asp:Label ID="ApprovedRuleErrorLabel" runat ="server" style="color:Red" Text="Cannot be empty" Visible="false"></asp:Label>
								</td>
							</tr>
							
							<tr>
								<td  style="padding-bottom:5px"><img alt="" src="/_layouts/images/square.gif">
								</td>
								<td style="padding-left:5px;padding-bottom:5px">
									<a id="A3"  href="#" onclick="showTaskRuleEditorDialog(1);return false"  runat="server">Task Rejected rule</a>
								</td>
								<td  style="padding-left:5px;padding-bottom:5px">
								</td>
							</tr>
							
							<div id="CCIWorkflowTaskSettingDiv" runat="server" visible="false">
							 <tr>
								<td  style="padding-bottom:5px"><img alt="" src="/_layouts/images/square.gif">
								</td>
								<td style="padding-left:5px;padding-bottom:5px">
									<a id="A1"   href="#" onclick="showTaskRuleEditorDialog(2);return false"  runat="server">Task Reassigned rule</a>
								</td>
								<td  style="padding-left:5px;padding-bottom:5px">
								</td>
							</tr>
							<tr>
								<td  style="padding-bottom:5px"><img alt="" src="/_layouts/images/square.gif">
								</td>
								<td style="padding-left:5px;padding-bottom:5px">
									<a id="A4"   href="#" onclick="showTaskRuleEditorDialog(3);return false"  runat="server">Task Requested Information rule</a>
								</td>
								<td  style="padding-left:5px;padding-bottom:5px">
								</td>
							</tr>
							<tr>
								<td  style="padding-bottom:5px"><img alt="" src="/_layouts/images/square.gif">
								</td>
								<td style="padding-left:5px;padding-bottom:5px">
									<a id="A6"   href="#" onclick="showTaskRuleEditorDialog(5);return false"  runat="server">Task Sent Information</a>
								</td>
								<td  style="padding-left:5px;padding-bottom:5px">
								</td>
							</tr>                            
							 
							</div>                            
							
							<tr>
								<td  style="padding-bottom:5px"><img alt="" src="/_layouts/images/square.gif">
								</td>
								<td style="padding-left:5px;padding-bottom:5px">
									<a id="A5"  href="#" onclick="showTaskRuleEditorDialog(4);return false"  runat="server">Workflow Terminated rule</a>
								</td>
								<td  style="padding-left:5px;padding-bottom:5px">
								</td>

							</tr>
							</div>
							<div id="SignatureVerificationTaskRuleDiv" runat="server" visible="false">
							<tr>
								<td  style="padding-bottom:5px"><img alt="" src="/_layouts/images/square.gif">
								</td>
								<td style="padding-left:5px;padding-bottom:5px">
									<a id="A7"   href="#" onclick="showTaskRuleEditorDialog(6);return false"  runat="server">Task Signature Verified rule</a>
								</td>
								<td  style="padding-left:5px;padding-bottom:5px">
									<asp:Label ID="SignatureVerificationRuleErrorLabel" runat ="server" style="color:Red" Text="Cannot be empty" Visible=false></asp:Label>
								</td>
							</tr>
							</div>
							
							<div id="DataQualityCompletedTaskRuleDiv" runat="server" visible="false">
							<tr>
								<td  style="padding-bottom:5px"><img alt="" src="/_layouts/images/square.gif">
								</td>
								<td style="padding-left:5px;padding-bottom:5px">
									<a id="A8"   href="#" onclick="showTaskRuleEditorDialog(7);return false"  runat="server">Data Quality Completed rule</a>
								</td>
								<td  style="padding-left:5px;padding-bottom:5px">
									<asp:Label ID="DataQualityCompletedRuleErrorLabel" runat ="server" style="color:Red" Text="Cannot be empty" Visible=false></asp:Label>
								</td>
							</tr>
							</div>
														  
							 <tr>
								<td  colspan=3>
								&nbsp;
								</td>
							</tr>
							<tr>
								<td  colspan=3>
								<asp:CheckBox ID="ByPassTaskCheckBox" AutoPostBack="true" OnCheckedChanged="ByPassTaskCheckedChanged" runat="server" Checked="false" Text="Bypass Task If File Is Not Modified" />
								</td>
							</tr>

						</table>
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						&nbsp;
					</td>
					<td>
						&nbsp;
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b>Due date</b>
					</td>
					<td>
						Give each person the following amount of time to finish their task
												<br />
						&nbsp;<asp:TextBox ID="DueDateDurationTextBox" runat="server" class="ms-input" Size="11"
							Style="ime-mode: inactive;"></asp:TextBox>
						&nbsp;<asp:DropDownList ID="DueDateMessureDropDown" runat="server">
						</asp:DropDownList>
						<asp:RangeValidator MaximumValue="365" MinimumValue="1" ID="RangeValidator1" runat="server"
							ControlToValidate="DueDateDurationTextBox" ErrorMessage="Please enter a number from 1 to 365"
							Type="Integer" ValidationGroup="SubmitGroup"></asp:RangeValidator>

						</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						&nbsp;
					</td>
					<td>
						&nbsp;
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b>Email template list url</b>
					</td>
					<td>
						Enter url of the email template list and click out side the textbox to show email template 
						names.<br />
						<i>Ex: http://myportal/Lists/Mail Template/</i>
						<br /><br />
						<asp:Textbox runat="server" id="txtTemplateUrl"  AutoPostBack="true"/>
						<br />
						<asp:Label ID="UrlErrorMessageLabel" runat ="server" style="color:Red" Text="Cannot get email template from this url" Visible="false" ></asp:Label>
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b></b>
					</td>
					<td>
						&nbsp;
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b>Assigment Email template</b> </td>
					<td>
						<asp:DropDownList ID="AssigmentEmailTemplateDropDown" runat="server">
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b>&nbsp;&nbsp;</b>
					</td>
					<td>
						&nbsp;
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b>Reminder date</b>
					</td>
					<td>
						<asp:TextBox ID="ReminderDateDurationTextBox" runat="server" class="ms-input" Size="11"
							Style="ime-mode: inactive;"></asp:TextBox>
						&nbsp;<asp:DropDownList ID="ReminderDateMessureDropDown" runat="server">
						</asp:DropDownList>
						<asp:RangeValidator MaximumValue="365" MinimumValue="1" ID="RangeValidator2" runat="server"
							ControlToValidate="ReminderDateDurationTextBox" ErrorMessage="Please enter a number from 1 to 365"
							Type="Integer" ValidationGroup="SubmitGroup"></asp:RangeValidator>

					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b></b>
					</td>
					<td valign="top">
						Reminder Email template<br/>
						<asp:DropDownList ID="ReminderEmailTemplateDropDown" runat="server">
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b>&nbsp;&nbsp;</b>
					</td>
					<td>
						&nbsp;
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b>Escalation date</b>
					</td>
					<td valign="top">
						<asp:TextBox ID="EscalationDateDurationTextBox" runat="server" class="ms-input" Size="11"
							Style="ime-mode: inactive;"></asp:TextBox>
						&nbsp;<asp:DropDownList ID="EscalationDateMessureDropDown" runat="server">
						</asp:DropDownList>
						 <asp:RangeValidator MaximumValue="365" MinimumValue="1" ID="RangeValidator3" runat="server"
							ControlToValidate="EscalationDateDurationTextBox" ErrorMessage="Please enter a number from 1 to 365"
							Type="Integer" ValidationGroup="SubmitGroup"></asp:RangeValidator>
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						&nbsp;</td>
					<td valign="top">
						Escalation Party
						<br />
						<SharePoint:PeopleEditor EnableBrowse="true" runat="server" ID="EscalationPartyPeoplePicker"  MultiSelect="false" MaximumEntities="1" ValidatorEnabled="true" width="360"/><br />
						<asp:CustomValidator ID="validEscalationPartyPeoplePicker" runat="server" ErrorMessage="The participant is not a member of site." Display="Dynamic" ControlToValidate = "EscalationPartyPeoplePicker" />
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b>&nbsp;&nbsp;</b>
					</td>
					<td valign="top">
						Escalation Email template<br/>
						<asp:DropDownList ID="EscalationEmailTemplateDropDown" runat="server">
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b></b>
					</td>
					<td>
						&nbsp;
					</td>
				</tr>
			</table>
			&nbsp;
		</td>
	</tr>

	 
	  <tr>
		<td class="ms-formlabel" valign="top">
			Task security</td>
			<td class="ms-formbody" valign="top">
			<table cellpadding="3" cellspacing="3" class="ms-descriptiontext" class="ms-descriptiontext">
				<tr>
					<td valign="top" align="right">
						<b>Contributors&nbsp; </b></td>
					<td>
						Type the names of people or group that have contribute permissions on the task.
						<SharePoint:PeopleEditor EnableBrowse="true" MultiSelect="true" runat="server" ID="TaskContributorsPeoplePicker" SelectionSet="User,SPGroup"  AllowEmpty ="true" ValidatorEnabled="true"/>
						<asp:CustomValidator ID="validTaskContributorsPeoplePicker" runat="server" ErrorMessage="The participant is not a member of site." Display="Dynamic" ControlToValidate = "TaskContributorsPeoplePicker" />
					</td>
				</tr>
				<tr>
					<td valign="top" align="right">
						<b>Observers &nbsp; </b></td>
					<td>
						Type the names of people or group that have read access on the task.
						<SharePoint:PeopleEditor EnableBrowse="true" MultiSelect="true" runat="server" ID="TaskObserversPeoplePicker" SelectionSet="User,SPGroup"  AllowEmpty ="true" ValidatorEnabled="true"/>
						<asp:CustomValidator ID="validTaskObserversPeoplePicker" runat="server" ErrorMessage="The participant is not a member of site." Display="Dynamic" ControlToValidate = "TaskObserversPeoplePicker" />
					</td>
				</tr>
			</table>
			</td>
	  </tr>
	  <tr>
		<td class="ms-formlabel" valign="top">Events and Actions</td>
			<td class="ms-formbody" valign="top">
			<table cellspacing="0 " cellpadding="0" border="0" class="ms-descriptiontext">
				<tbody>
					<tr>
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
							<a onclick="showTaskEventEditorDialog(0,'Task Created Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.TaskCreated.ToString() %>');return false" id="" href="#">Task Created Event</a>
							<asp:Label Text="" runat="server" ID="lbTaskCreatedActions" />
						</td>
					</tr>
					<tr>
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
						<a onclick="showTaskEventEditorDialog(0,'Task Approved Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.TaskApproved.ToString() %>');return false" id="A18" href="#">Task Approved Event</a>
						<asp:Label Text="" runat="server" ID="lbTaskApprovedActions" />
						</td>
					</tr>
					<tr>
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
							<a onclick="showTaskEventEditorDialog(0,'Task Rejected Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.TaskRejected.ToString() %>');return false" id="A9" href="#">Task Rejected Event</a>
							<asp:Label Text="" runat="server" ID="lbTaskRejectedActions" />
						</td>
					</tr>
					<tr>
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
						<a onclick="showTaskEventEditorDialog(0,'Workflow Terminated Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.WorkflowTerminated.ToString() %>');return false" id="A10" href="#">Workflow Terminated Event</a>
						<asp:Label Text="" runat="server" ID="lbWorkflowTerminatedActions" />    
						</td>
					</tr>
					<tr>
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
						<a onclick="showTaskEventEditorDialog(0,'Task Reassigned Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.TaskReassigned.ToString() %>');return false" id="A11" href="#">Task Reassigned Event</a>
						<asp:Label Text="" runat="server" ID="lbTaskReassignedActions" />
						</td>
					</tr>
					<tr>
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
							<a onclick="showTaskEventEditorDialog(0,'Task Information Requested Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.TaskInformationRequested.ToString() %>');return false" id="A12" href="#">Task Information Requested Event</a>
							<asp:Label Text="" runat="server" ID="lbTaskInformationRequestedActions" />
						</td>
					</tr>
					<tr>
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
						<a onclick="showTaskEventEditorDialog(0,'Task Information Sent Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.TaskInformationSent.ToString() %>');return false" id="A13" href="#">Task Information Sent Event</a>
						<asp:Label Text="" runat="server" ID="lbTaskInformationSentActions" />
						</td>
					</tr>
					<tr>
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
							  <a onclick="showTaskEventEditorDialog(0,'Reminder Date Reached Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.ReminderDateReached.ToString() %>');return false" id="A14" href="#">Reminder Date Reached Event</a>
							  <asp:Label Text="" runat="server" ID="lbReminderDateReachedActions" />
						</td>
					</tr>
					<tr>
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
							<a onclick="showTaskEventEditorDialog(0,'Escalation Date Reached Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.EscalationDateReached.ToString() %>');return false" id="A15" href="#">Escalation Date Reached Event</a>
							<asp:Label Text="" runat="server" ID="lbEscalationDateReachedActions" />
					   </td>
					</tr>
					<tr>
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
							<a onclick="showTaskEventEditorDialog(0,'Task Completed Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.TaskCompleted.ToString() %>');return false" id="A16" href="#">Task Completed Event</a>
							<asp:Label Text="" runat="server" ID="lbTaskCompletedActions" />
						</td>
					</tr>
					
					<tr>
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
							<a onclick="showTaskEventEditorDialog(0,'Task On Hold Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.TaskOnHold.ToString() %>');return false" id="A17" href="#">Task On Hold Event</a>
							<asp:Label Text="" runat="server" ID="lbTaskOnHoldActions" />
						</td>
					</tr>  
					
					<div id="ByPassTaskDiv" runat="server" visible="false">
					<tr> 
						<td style="padding-bottom: 5px; padding-left:10px"><img src="/_layouts/images/square.gif" alt=""></td>
						<td style="padding-left: 5px; padding-bottom: 5px;">
							<a onclick="showTaskEventEditorDialog(0,'Bypass Task If File Is Not Modified Event','<%=_hiddenUniqueKey.Value %>', '<%=TaskEventTypes.ByPassTask.ToString() %>');return false" id="A19" href="#">Bypass Task If File Is Not Modified Event </a>
							<asp:Label Text="" runat="server" ID="lbByPassTaskActions" /> 
						</td>
					</tr>                                         
					</div>  
																																																						   
				</tbody>
			</table>
						
		</td>
	</tr>
	<tr>
		<td valign="top">
			&nbsp;
		</td>
		<td align="right">
			<table class="style1">
				<tr>
					<td>
						<asp:Button id="RefeshButton" runat="server" value="Refesh" style="display:none" CausesValidation="false"/>
						<asp:Button ID="SaveButton" runat="server" Text="Save" class="ms-ButtonHeightWidth" OnClick="SaveButton_Click" ValidationGroup="SubmitGroup"/>
						<asp:Button id="bntVirtualButton" value="Search" style="display:none" runat="server" CausesValidation="false"/>
						<asp:TextBox ID="ApprovedSessionKeyTextBox" runat="server" style="display:none"  CssClass= CausesValidation="false" EnableViewState="true"></asp:TextBox>
						<asp:TextBox ID="TerminationSessionKeyTextBox" runat="server" style="display:none" CssClass= CausesValidation="false" EnableViewState="true"></asp:TextBox>
						<asp:TextBox ID="RejectedSessionKeyTextBox" runat="server" style="display:none" CssClass= CausesValidation="false" EnableViewState="true"></asp:TextBox>
						<asp:TextBox ID="ReassignSessionKeyTextBox" runat="server" style="display:none" CssClass= CausesValidation="false" EnableViewState="true"></asp:TextBox>
						<asp:TextBox ID="RequestInformationSessionKeyTextBox" runat="server" style="display:none" CssClass= CausesValidation="false" EnableViewState="true"></asp:TextBox>
						<asp:TextBox ID="FinishedSessionKeyTextBox" runat="server" style="display:none" CssClass= CausesValidation="false" EnableViewState="true"></asp:TextBox>
						<asp:TextBox ID="SignatureVerificationSessionKeyTextBox" runat="server" style="display:none" CssClass= CausesValidation="false" EnableViewState="true"></asp:TextBox>
						<asp:TextBox ID="DataQualityCompletedSessionKeyTextBox" runat="server" style="display:none" CssClass= CausesValidation="false" EnableViewState="true"></asp:TextBox>
						
						<asp:HiddenField id="_hiddenUniqueKey" runat="server" />
					</td>
					<td>
						<SharePoint:GoBackButton ID="GoBackButton" runat="server" />						
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<script language="javascript" type="text/javascript">
	_spBodyOnLoadFunctionNames.push("RegisterEventOnSaveButton");
	function SaveButton_ClickHandler(e) {
		document.getElementById('<%=SaveButton.ClientID%>').click();
	}

	function RegisterEventOnSaveButton() {
		var saveButton = document.getElementById('Ribbon.ListForm.Edit.Commit.Publish-Large');
		if (saveButton == null) return;
		if (saveButton.addEventListener)
			saveButton.addEventListener("click", SaveButton_ClickHandler, false);
		else if (saveButton.attachEvent)
			saveButton.attachEvent("onclick", SaveButton_ClickHandler);
	}

	function handleDependentCheckBox(sender, checkbox) {
		var obj = document.getElementById(checkbox);
		obj.checked = "";
		obj.disabled = (sender.checked == false);
		obj.parentNode.disabled = obj.disabled;
	}
	function CreateWorkflowTaskContentType_Callback(data) {
		document.getElementById('<%=bntVirtualButton.ClientID%>').click();
	}
	function AssignTaskEventComplete_Callback(dialogResult, returnValue) {
		if (dialogResult == SP.UI.DialogResult.OK) {
			document.getElementById('<%=RefeshButton.ClientID%>').click();
		}
	}

	function onDialogClose(dialogResult, returnValue) {
		if (dialogResult == SP.UI.DialogResult.OK) {
			document.getElementById('<%=bntVirtualButton.ClientID%>').click();
			//alert('goodbye world!');
		}
		if (dialogResult == SP.UI.DialogResult.cancel) {
			//alert(returnValue);
		}
	}

	function showContentTypeEditorDialog() {
	    var sDialogUrl = '<%=SPContext.Current.Web.Url%>\u002f_layouts\u002fAIA.Intranet.Infrastructure\u002fCreateWorkflowTaskContentTypePopup.aspx?CloseOnCancel=true';
		var sFeatures = 'resizable=yes,status=no,scrollbars=no,menubar=no,directories=no,location=no,width=550,height=450';
		if (browseris.ie55up)
			sFeatures = 'resizable:yes;status:no;scrollbars:no;menubar:no;directories:no;location:no;dialogWidth:550px;dialogHeight=450px';
		//var rv = commonShowModalDialog(sDialogUrl, sFeatures, CreateWorkflowTaskContentType_Callback);

		var options = {
			url: sDialogUrl,
			width: 550,
			height: 400,

			title: "Create new task content type",
			dialogReturnValueCallback: onDialogClose
		};
		SP.UI.ModalDialog.showModalDialog(options);

	}

	function showTaskRuleEditorDialog(mode, key) {
		var contentypeId = getContentTypeId();
		var key = getSessionkey(mode);
		var sDialogUrl = '<%=SPContext.Current.Web.Url%>\u002f_layouts\u002fAIA.Intranet.Infrastructure\u002fTaskRule.aspx?CloseOnCancel=true&mode=' + mode + '&ctype=' + contentypeId + '&List=<%=SPContext.Current.List.ID.ToString()%>&ID=<%=SPContext.Current.ListItem.ID.ToString()%>&skey=' + key + '&read=<%=FormReadOnly.ToString()%>';
		//commonShowModalDialog(sDialogUrl, getRuleFormSize(), CreateWorkflowTaskContentType_Callback);

		var options = {
			url: sDialogUrl,
			width: 650,
			height: 550,
			title: "Task Rule Editor",
			dialogReturnValueCallback: onDialogClose
		};
		SP.UI.ModalDialog.showModalDialog(options);
	}

	function showTaskEventEditorDialog(key, title, session, type) {

		var contentypeId = getContentTypeId();
		var key = getSessionkey(mode);
		var mode = '<%=FormReadOnly %>';
		var eventOwner = '<%=EventOwners.Workflow.ToString()%>';
		var sDialogUrl = '<%=SPContext.Current.Web.Url%>\u002f_layouts\u002fAIA.Intranet.Infrastructure\u002fTaskEvent.aspx?CloseOnCancel=true&readonly=' + mode + "&title=" + title + "&session=" + session + "&type=" + type + "&taskContentTypeId=" + getContentTypeId();
		sDialogUrl = sDialogUrl + "&eventOwner=" + eventOwner + "&timer=" + (new Date()).getTime();
		//commonShowModalDialog(sDialogUrl + "&timer=" + (new Date()).getTime(), getEventFormSize(), AssignTaskEventComplete_Callback);

		var options = {
			url: sDialogUrl,
			width: 650,
			height: 550,
			title: title,
			dialogReturnValueCallback: AssignTaskEventComplete_Callback
		};
		SP.UI.ModalDialog.showModalDialog(options);

	}

	function getSessionkey(mode) {
		switch (mode) {
			case 0:
				return '<%=ApprovedSessionKey%>';
			case 1:
				return '<%=RejectedSessionKey%>';
			case 2:
				return '<%=ReassignSessionKey%>';
			case 3:
				return '<%=RequestInformationSessionKey%>';
			case 4:
				return '<%=TerminationSessionKey%>';
			case 5:
				return '<%=FinishedSessionKey%>';
			case 6:
				return '<%=SignatureVerificationSessionKey%>';
			case 7:
				return '<%=DataQualityCompletedSessionKey%>';
		}
	}

	function getRuleFormSize() {
		var sFeatures = 'resizable=yes,status=no,scrollbars=no,menubar=no,directories=no,location=no,width=700,height=500';
		if (browseris.ie55up)
			sFeatures = 'resizable:yes;status:no;scrollbars:no;menubar:no;directories:no;location:no;dialogWidth:700px;dialogHeight=500px';
		return sFeatures;
	}

	function getEventFormSize() {
		var sFeatures = 'resizable=yes,status=no,scrollbars=no,menubar=no,directories=no,location=no,width=620,height=500';
		if (browseris.ie55up)
			sFeatures = 'resizable:yes;status:no;scrollbars:no;menubar:no;directories:no;location:no;dialogWidth:620px;dialogHeight=500px';
		return sFeatures;
	}

	function getContentTypeId() {
		var contentypeId = null;
		var ddlcontentype = document.getElementById('<%=TaskContentTypesDropDown.ClientID%>');
		if (ddlcontentype != null) {
			contentypeId = ddlcontentype.options[ddlcontentype.selectedIndex].value;
		}
		return contentypeId;
	}

	g_bWarnBeforeLeave = false;
</script>
