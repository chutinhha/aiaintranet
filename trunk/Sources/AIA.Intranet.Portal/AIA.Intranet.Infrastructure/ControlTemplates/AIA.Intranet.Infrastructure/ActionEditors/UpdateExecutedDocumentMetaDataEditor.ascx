<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateExecutedDocumentMetaDataEditor.ascx.cs"
    Inherits="AIA.Intranet.Infrastructure.Controls.UpdateExecutedDocumentMetaDataEditor" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>

<wssawc:FormDigest runat="server" id="FormDigest" />
<div style="color:red" id="errorMessageHolder" enableviewstate="false" runat="server"></div>
<script type="text/javascript">
        function <%=chkRemoveAction.ClientID%>_click(control){
            ValidatorEnable(document.getElementById('<%=ddlItemColumnValidator.ClientID%>'),!control.checked);           
        }
    </script>
<TABLE border="0" width="95%" cellspacing="0" cellpadding="0" class="ms-formtable" >
	<wssuc:InputFormSection runat="server" Title="Update metadata of executed document">
		<Template_Description>
			<SharePoint:EncodedLiteral ID="EncodedLiteral1" runat="server" text="This action will update metadata of retrieved document." EncodeMethod='HtmlEncode'/>
		</Template_Description>
		<Template_InputFormControls>
			<wssuc:InputFormControl runat="server" LabelText="Item's column">
			<Template_Control>
			    <asp:DropDownList ID="ddlItemColumn" runat="server" CssClass="ms-long" AutoPostBack="true" CausesValidation="false"/> 
			     <wssawc:InputFormRequiredFieldValidator ID="ddlItemColumnValidator" 
				                                        ControlToValidate="ddlItemColumn"
					                                    Display="Dynamic" 
					                                    ErrorMessage="Please select a column" 
					                                    Runat="server"
					                                    ValidationGroup="SubmitValidate"/>
			</Template_Control>
			</wssuc:InputFormControl>
			<wssuc:InputFormControl runat="server" LabelText="Metadata property">
			    <Template_Control>
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
	<asp:Literal ID="lbScript" runat="server"/>