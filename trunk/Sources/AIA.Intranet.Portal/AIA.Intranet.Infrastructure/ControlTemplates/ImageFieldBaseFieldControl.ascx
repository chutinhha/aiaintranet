<%@ Control Language="C#" %>  
<%@ Assembly Name="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" Namespace="Microsoft.SharePoint.WebControls" %>  
  
<SharePoint:RenderingTemplate ID="ImageFieldTemplate" runat="server">  
    <Template>  
        <p>
            <asp:Label runat="server" ID="lbOrNewPicture" />: <asp:FileUpload ID="imageFieldPicture" runat="server" />
            <br />
            <asp:RegularExpressionValidator ID="revUploadPicture" runat="server" ControlToValidate="imageFieldPicture" ValidationExpression=".*(.jpg|.JPG|.jpeg|.JPEG|.png|.PNG|.gif|.GIF)$"></asp:RegularExpressionValidator>
        </p>
        <p><asp:Label runat="server" ID="lbExistingPicture" />: <asp:DropDownList ID="ddlExistingPicture" AutoPostBack="true" runat="server" /></p>
        <p><asp:Image ID="imgExistingPicture" runat="server" /></p>
    </Template>  
</SharePoint:RenderingTemplate> 