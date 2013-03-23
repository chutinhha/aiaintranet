<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Control Language="C#" CodeBehind="InsertScript.ascx.cs" Inherits="AIA.Intranet.Infrastructure.Controls.InsertScript" %>
<SharePoint:ScriptLink ID="CoreScript" language="javascript" name="core.js" Defer="true" runat="server"/> 


<script language="javascript" type="text/javascript">
    var siteCollectionUrl = '<%=SiteUrl %>';

    function OpenDuplicatePage(urlString) {
        var options = {
            url: urlString,
            height: 340,
            dialogReturnValueCallback: OpenEditPage
        };
        SP.UI.ModalDialog.showModalDialog(options);
    }

    function OpenEditPage(dialogResult, returnValue) {
        var options = {
            url: returnValue,
            dialogReturnValueCallback: CloseCallback
        };
        SP.UI.ModalDialog.showModalDialog(options);
    }

    function CloseCallback(result, target) {
        window.location.href = window.location.href;
    }

    function OpenVariableEditor() {
        var options = {
            url: siteCollectionUrl + '/_layouts/AIA.Intranet.Infrastructure/VariableEditor.aspx',
            height: 340,
            dialogReturnValueCallback: InsertVariableCallback
        };
        SP.UI.ModalDialog.showModalDialog(options);
    }

    function InsertVariableCallback(dialogResult, returnValue) {
        if (dialogResult == "OK") {
            RTE.Cursor.get_range().deleteContent();
            var rng = RTE.Cursor.get_range().$3_0;
            var divValue = document.createElement("span");
            divValue.innerHTML = returnValue;
            rng.insertBefore(divValue);
            RTE.Cursor.update();
        }
    }
</script>
