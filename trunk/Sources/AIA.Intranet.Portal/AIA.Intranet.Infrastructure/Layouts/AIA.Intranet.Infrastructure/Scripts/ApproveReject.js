/// <reference path="/_layouts/MicrosoftAjax.js"/>
/// <reference path="/_layouts/SP.debug.js"/>
/// <reference path="/_layouts/SP.Core.debug.js"/>
/// <reference path="/_layouts/SP.Ribbon.debug.js"/>
/// <reference path="_layouts/SP.UI.Dialog.debug.js"/>

/// <reference path="/_layouts/actionmenu.js" />
/// <reference path="/_layouts/ajaxtoolkit.js" />
/// <reference path="/_layouts/CUI.debug.js" />
/// <reference path="/_layouts/portal.js" />
/// <reference path="/_layouts/SP.Exp.debug.js" />
/// <reference path="/_layouts/SP.Runtime.debug.js" />
/// <reference path="/_layouts/SP.UI.Dialog.debug.js" />

//used to show approve/reject dialog
function showApproveAll() {
    var ctx = new SP.ClientContext.get_current();
    var ItemIds = "";
    //get current list id
    var listId = SP.ListOperation.Selection.getSelectedList();
    //get all selected list items
    var selectedItems = SP.ListOperation.Selection.getSelectedItems(ctx);

    //collect selected item ids
    for (var i = 0; i < selectedItems.length; i++) {
        ItemIds += selectedItems[i].id + ",";
    }

    //prepare cutom approval page with listid 
    //and selected item ids passed in querystring
    var pageUrl = SP.Utilities.Utility.getLayoutsPageUrl(
        '/AIA.Intranet.Infrastructure/ApproveAll.aspx?ids=' + ItemIds + '&listid=' + listId);
    var options = SP.UI.$create_DialogOptions();
    options.width = 420;
    options.height = 250;
    options.url = pageUrl;
    options.dialogReturnValueCallback = Function.createDelegate(null, OnDialogClose);
    SP.UI.ModalDialog.showModalDialog(options);
}

//used to determine whether the 'approve/reject selection' 
//ribbon will be enalbed or disabled
function enableApprovalAll() {
    
    var ctx = new SP.ClientContext.get_current();
    return SP.ListOperation.Selection.getSelectedItems(ctx).length > 1;
}


//called on dialog closed
function OnDialogClose(result, target) {
    //if ok button is clicked in dialog, reload the grid.
    if (result == SP.UI.DialogResult.OK) {
        location.reload(true);
    }
}