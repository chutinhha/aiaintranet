<%@ Control Language="C#" Inherits="AIA.Intranet.Infrastructure.Controls.ImageFieldControl, $SharePoint.Project.AssemblyFullName$"
    AutoEventWireup="true" CompilationMode="Always" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<wssuc:InputFormSection runat="server" ID="ifsImageField">
    <template_inputformcontrols>
             <wssuc:InputFormControl runat="server" ID="ifcWeb">
                <Template_Control>
                    <asp:DropDownList ID="ddlWebs" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlWebs_OnSelectedIndexChanged" />
                </Template_Control>
             </wssuc:InputFormControl>
             <wssuc:InputFormControl runat="server" ID="ifcPictureLibrary">
                <Template_Control>
                    <asp:DropDownList ID="ddlPictureLibrary" runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="ddlPictureLibrary" ID="rfvPictureLibrary" runat="server" />
                </Template_Control>
             </wssuc:InputFormControl>
             <wssuc:InputFormControl runat="server" ID="ifcDefaultPicture">
                <Template_Control>
                    <asp:FileUpload runat="server" ID="fuDefaultPicture" />
                    <asp:Image ID="imgDefaultPicture" runat="server" Visible="false"/>
                    <br />
                    <asp:CheckBox ID="cbClearDefaultPicture" runat="server"></asp:CheckBox>
                </Template_Control>
             </wssuc:InputFormControl>
             <wssuc:InputFormControl runat="server" ID="ifcFormatName">
                <Template_ExampleText>
                    <asp:Label runat="server" ID="lbFormatNameExample" />
                </Template_ExampleText>
                <Template_Control>
                    <asp:TextBox ID="tbFormatName" runat="server" />
                </Template_Control>
             </wssuc:InputFormControl>
             <wssuc:InputFormControl runat="server" ID="ifcOverwrite">
                <Template_Control>
                    <asp:CheckBox ID="cbOverwrite" runat="server" />
                </Template_Control>
             </wssuc:InputFormControl>
       </template_inputformcontrols>
</wssuc:InputFormSection>