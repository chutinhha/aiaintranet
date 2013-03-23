<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssignmentFieldEditor.ascx.cs" Inherits="AIA.Intranet.Infrastructure.Controls.AssignmentFieldEditor" %>

<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>

<wssuc:InputFormSection runat="server" id="MySections" Title="[I-Office]Assignment Field settings">
	   <Template_InputFormControls>
			 <wssuc:InputFormControl ID="InputFormControl1" runat="server" Visible="False" 
					LabelText="Send notify email">
					<Template_Control>
						  <asp:CheckBox runat="server" ID="chkSendNotifyEmail" />
					</Template_Control>
			 </wssuc:InputFormControl>
             <wssuc:InputFormControl ID="InputFormControl4" runat="server"
                    LabelText="Lists">
                    <Template_Control>
                          <asp:DropDownList ID="ddlWebs" runat="server" AutoPostBack="true" ></asp:DropDownList>
                    </Template_Control>
             </wssuc:InputFormControl>
			 <wssuc:InputFormControl ID="InputFormControl2" runat="server"
					LabelText="Lists">
					<Template_Control>
						  <asp:DropDownList ID="ddlLists" runat="server" AutoPostBack="true" ></asp:DropDownList>
					</Template_Control>
			 </wssuc:InputFormControl>
			 <wssuc:InputFormControl ID="InputFormControl3" runat="server"
					LabelText="Columns">
					<Template_Control>
						  <asp:DropDownList ID="ddlColumns" runat="server" ></asp:DropDownList>
					</Template_Control>
			 </wssuc:InputFormControl>
	   </Template_InputFormControls>
</wssuc:InputFormSection>