<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditRule.aspx.cs" Inherits="AIA.Intranet.Infrastructure.Layouts.EditRule"
    DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" Src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" Src="~/_controltemplates/ToolBarButton.ascx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Security Rule Editor
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    <span class="ms-sitemapdirectional">Edit Security Rule:
        <%=this.CurrentList.Title %>
        list </span>
</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="PlaceHolderPageDescription">
    <!-- Message to user-->
    <table width="100%" class="propertysheet" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td class="ms-error">
                <asp:Literal ID="literalMessage" runat="server" EnableViewState="false" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <wssuc:ButtonSection runat="server" BottomSpacing="0">
        <Template_Buttons>
            <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Add" ID="BtnSaveTop"
                OnClick="BtnSave_Click" AccessKey="<%$Resources:wss,okbutton_accesskey%>" />
            <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Delete" Enabled="false"
                ID="BtnDeleteTop" OnClick="BtnDelete_Click" OnClientClick="return confirmSubmit()"
                AccessKey="<%$Resources:wss,okbutton_accesskey%>" />
        </Template_Buttons>
    </wssuc:ButtonSection>
    <table width="100%" cellspacing="3" cellpadding="0">
        <tbody>
            <tr height="10">
                <td style="padding: 4px;" class="ms-linksectionheader">
                    General Information
                </td>
            </tr>
        </tbody>
    </table>
    <table border="0" width="100%" cellspacing="0" cellpadding="0">
        <wssuc:InputFormSection Title="Content Type" Description="" runat="server">
            <Template_InputFormControls>
                <template_control>
				   <asp:DropDownList runat="server" id="drpContentTypes"></asp:DropDownList>	
                   <span><SharePoint:EncodedLiteral runat="server" ID="ltrDescription"></SharePoint:EncodedLiteral></span>
				 </template_control>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection Title="Rule Name" Description="" runat="server">
            <Template_InputFormControls>
                <template_control>
				    <asp:TextBox ID="txtRuleName" runat="server" EnableViewState="true" class="ms-long" Width="360px"/><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRuleName" Text="Required" />
				 </template_control>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection Title="Run On" Description="" runat="server">
            <Template_InputFormControls>
                <template_control>
				    
                                <span class="ms-RadioText">
                                    <asp:CheckBox ID="chkItemAdded" runat="server" Text="Item Added" /> <br />
                                </span>
                    
                                <span class="ms-RadioText">
                                    <asp:CheckBox ID="chkFirstUpdate"  runat="server" Text="First Check In" /><br />
                                </span>
                                <span class="ms-RadioText">
                                    <asp:CheckBox ID="chkAnyUpdate" runat="server" Text="Any Update" /><br />
                                </span>
                    
				 </template_control>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection Title="Document Types" Description="Define document types that this rule to be applied."
            runat="server" ID="documentTypesRow">
            <Template_InputFormControls>
                <template_control>
				        <asp:TextBox ID="txtDocumentTypes" runat="server" EnableViewState="true" class="ms-long" Width="360px"/> <br />
						<span class="ms-descriptiontext" >
		                    Enter multi document types separated by semicolons. E.g: doc; docx; pdf
						</span>

				 </template_control>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
    </table>
    <table width="100%" cellspacing="3" cellpadding="0">
        <tbody>
            <tr height="10">
                <td style="padding: 4px;" class="ms-linksectionheader">
                    Field Value Definitions
                </td>
            </tr>
        </tbody>
    </table>
    <table border="0" width="100%" cellspacing="0" cellpadding="0">
        <tr>
            <td width="5">
            </td>
            <td class="ms-formdescription" colspan="2">
                Select Operators to include fields into this rule
            </td>
        </tr>
        <tr>
            <td colspan="3" height="2">
                <img src="/_layouts/images/blank.gif" width='1' height='1' alt="" />
            </td>
        </tr>
        <tr>
            <td width="5">
            </td>
            <td>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: #f2f2f2">
                    <tr class="ms-vh2-nofilter ms-vh2-gridview">
                        <td nowrap="true" valign="top" class="ms-descriptiontext">
                            Field Name
                        </td>
                        <td valign="top" style="width: 25%" class="ms-descriptiontext">
                            Operator
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: #f2f2f2">
                    <tr class="ms-vh2-nofilter ms-vh2-gridview">
                        <td nowrap="true" valign="top" class="ms-descriptiontext">
                            Value
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <wssawc:ListFieldIterator runat="server" ID="listFieldsContainer" ControlMode="Edit">
            <CustomTemplate>
                <tr>
                    <td>
                    </td>
                    <td valign="top" class="ms-formlabel" style="padding-right: 0px">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td nowrap="true" valign="top" class="ms-formlabel" style="border-top: none">
                                    <wssawc:FieldLabel ID="FieldLabel2" runat="server">
                                    </wssawc:FieldLabel>
                                </td>
                                <td align="left" valign="top" style="width: 25%">
                                    <asp:DropDownList runat="server" ID="dropdownOperators" Width="100px" class="ms-advsrchOperatorDDL" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="ms-formbody" style="border-top: 1px solid #d8d8d8;">
                        <wssawc:FormField ID="ffContent" runat="server" ControlMode="New" IsValid="true">
                        </wssawc:FormField>
                    </td>
                </tr>
            </CustomTemplate>
        </wssawc:ListFieldIterator>
    </table>
    <table width="100%" cellspacing="3" cellpadding="0">
        <tbody>
            <tr height="10">
                <td style="padding: 4px;" class="ms-linksectionheader">
                    Permission Assignments
                </td>
            </tr>
        </tbody>
    </table>
    <table border="0" width="100%" cellspacing="0" cellpadding="0">
        <wssuc:InputFormSection Title="Preserve Existing Security" Description="Selecting this option will cause the security of this rule to be added to the current item permissions"
            runat="server">
            <Template_InputFormControls>
                <wssuc:InputFormControl runat="Server">
                    <Template_Control>
                        <asp:CheckBox runat="server" ID="chkPreserveExistingSecurity" />
                    </Template_Control>
                </wssuc:InputFormControl>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <wssuc:InputFormSection Title="Creator Permission" Description="" runat="server">
            <Template_InputFormControls>
                <wssuc:InputFormControl runat="Server">
                    <Template_Control>
                        <asp:DropDownList runat="server" ID="cboOwnerPermission">
                        </asp:DropDownList>
                    </Template_Control>
                </wssuc:InputFormControl>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
        <!-- Repeater PE -->
        <asp:Repeater ID="repeaterPermissionAssignments" runat="server" OnItemDataBound="repeaterPermissionAssignments_OnItemDataBound">
            <ItemTemplate>
                <wssuc:InputFormSection ID="inputFormSection" Title='<%# DataBinder.Eval (Container.DataItem, "Name").ToString() %>'
                    Description='<%# DataBinder.Eval (Container.DataItem, "Description").ToString() %>'
                    runat="server" Padding="false">
                    <Template_InputFormControls>
                        <wssuc:InputFormControl runat="Server" ID="inputForm">
                            <Template_Control>
                                <asp:HiddenField runat="server" ID="txtPermissionName" />
                                <SharePoint:PeopleEditor ID="userEditor" runat="server" SelectionSet="User,SPGroup "
                                    MultiSelect="True" />
                                <asp:UpdatePanel ID="UP1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <div style="width: 143px; height: 125px; overflow: scroll">
                                                        <asp:ListBox runat="server" Rows="10" ID="lsbAvailabaleFields" SelectionMode="Multiple"
                                                            Style="width: 100%; min-width: 200px" size="20"></asp:ListBox>
                                                    </div>
                                                </td>
                                                <td style="padding-left: 5px">
                                                </td>
                                                <td valign="middle" align="center">
                                                    <asp:Button runat="server" ID="bntAdd" Text="Add" CausesValidation="false" OnClick="btnAdd_Click"
                                                        class="ms-ButtonHeightWidth"></asp:Button>
                                                    <br />
                                                    <asp:Button runat="server" ID="bntRemove" Text="Remove" CausesValidation="false"
                                                        OnClick="btnRemove_Click" class="ms-ButtonHeightWidth"></asp:Button>
                                                </td>
                                                <td style="padding-left: 5px">
                                                </td>
                                                <td>
                                                    <div style="width: 143px; height: 125px; overflow: scroll">
                                                        <asp:ListBox runat="server" Rows="10" ID="lsbSelectedFields" SelectionMode="Multiple"
                                                            Style="width: 100%; min-width: 200px" size="20"></asp:ListBox>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </Template_Control>
                        </wssuc:InputFormControl>
                    </Template_InputFormControls>
                </wssuc:InputFormSection>
            </ItemTemplate>
        </asp:Repeater>
        <wssuc:ButtonSection runat="server">
            <Template_Buttons>
                <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Add" ID="BtnSave" OnClick="BtnSave_Click"
                    AccessKey="<%$Resources:wss,okbutton_accesskey%>" />
                <asp:Button runat="server" class="ms-ButtonHeightWidth" Text="Delete" Enabled="false"
                    ID="BtnDelete" OnClick="BtnDelete_Click" OnClientClick="return confirmSubmit()"
                    AccessKey="<%$Resources:wss,okbutton_accesskey%>" />
            </Template_Buttons>
        </wssuc:ButtonSection>
        <wssawc:FormDigest runat="server" ID="FormDigest" />
        <script language="JavaScript">
            function confirmSubmit() {
                var agree = confirm("Are you sure you want to delete?");
                if (agree)
                    return true;
                else
                    return false;
            }

            function ChangeContentTypeS(id) {
                var obj = document.getElementById(id);
                var strUrl = window.location.href;
                var idxQuery = strUrl.indexOf("?");
                if (strUrl.indexOf("?") <= 0) {
                    if (obj.value != null && obj.value != "") {
                        strUrl = strUrl + "?ContentTypeId=" + obj.value;
                    }
                }
                else if (strUrl.indexOf("&ContentTypeId=") <= 0) {
                    if (obj.value != null && obj.value != "") {
                        strUrl = strUrl + "&ContentTypeId=" + obj.value;
                    }
                }
                else {
                    var pattern = /&ContentTypeId=[^&]*/i;
                    if (obj.value != null && obj.value != "") {
                        strUrl = strUrl.replace(pattern, "&ContentTypeId=" + obj.value);
                    }
                    else {
                        strUrl = strUrl.replace(pattern, "");
                    }
                }


                if (strUrl.indexOf("&RuleName=") <= 0) {
                    strUrl += "&RuleName=" + document.getElementById("<%=txtRuleName.ClientID %>").value
                }
                else {

                    var pattern = /&RuleName=[^&]*/i;
                    var obj = document.getElementById("<%=txtRuleName.ClientID %>");

                    if (obj.value != null && obj.value != "") {
                        strUrl = strUrl.replace(pattern, "&RuleName=" + obj.value)
                    }
                    else {
                        strUrl = strUrl.replace(pattern, "")
                    }

                }
                var events = "";
                if (document.getElementById("<%=chkItemAdded.ClientID%>").checked) {
                    events += "ItemAdded,";
                }
                if (document.getElementById("<%=chkFirstUpdate.ClientID%>").checked) {
                    events += "FirstUpdate,";
                }
                if (document.getElementById("<%=chkAnyUpdate.ClientID%>").checked) {
                    events += "AnyUpdate,";
                }
                if (strUrl.indexOf("&Events=") <= 0) {
                    strUrl += "&Events=" + events;
                }
                else {

                    var pattern = /&Events=[^&]*/i;

                    if (obj.value != null && obj.value != "") {
                        strUrl = strUrl.replace(pattern, "&Events=" + events)
                    }
                    else {
                        strUrl = strUrl.replace(pattern, "")
                    }

                }


                var type = document.getElementById("<%=txtDocumentTypes.ClientID %>");
                if (type != null) {
                    if (strUrl.indexOf("&DocTypes=") <= 0) {
                        strUrl += "&DocTypes=" + type.value
                    }
                    else {

                        var pattern = /&DocTypes=[^&]*/i;
                        if (type.value != null && type.value != "") {
                            strUrl = strUrl.replace(pattern, "&DocTypes=" + type.value)
                        }
                        else {
                            strUrl = strUrl.replace(pattern, "")
                        }

                    }
                }
                STSNavigate(strUrl);

            }
        </script>
        <asp:Literal ID="ScriptPlaceHolder" runat="server"></asp:Literal>
        <link rel="stylesheet" type="text/css" href="/_layouts/1033/styles/portal.css" />
        <style type="text/css">
            .hiddencol
            {
                display: none;
            }
            .viscol
            {
                display: block;
            }
            .ms-ButtonHeightWidth
            {
                font: 8pt tahoma;
                height: 2.1em;
                padding-bottom: 0.4em;
                padding-top: 0.1em;
                width: 7.0em;
            }
        </style>
</asp:Content>
