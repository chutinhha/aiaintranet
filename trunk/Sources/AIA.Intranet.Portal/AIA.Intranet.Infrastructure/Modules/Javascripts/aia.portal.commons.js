JSRequest.EnsureSetup();

$(document).ready(function () {

    $('.scroll_a').slimScroll({
        start: 'top',
        disableFadeOut: true
    });

    //Fix the scrollbar in Chrome browser
    var isChrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;
    if (isChrome) {
        //SharePoint function
        FixRibbonAndWorkspaceDimensions();

        //make sure ExecuteOrDelayUntilScriptLoaded works
        if (typeof (_spBodyOnLoadWrapper) != 'undefined') {
            _spBodyOnLoadWrapper();
        }
    }

    //fix picker's width for some browsers (ie: chrome)
    $("table[id$='_OuterTable']").each(function () {
        var $tbl = $(this);

        $tbl.width("100%");
        $tbl.find(">tbody>tr>td:first").css("width", "100%");
        $tbl.find("td:has(>a[id$='checkNames'])").attr("align", "right");
    });

    //remove !important for ms-vh2 class
    $(".ms-viewheadertr > .ms-vh2").css("border", "0");

    //format rating table in survey
    $("td.ms-formbodysurvey table[summary]>tbody>tr:odd").addClass("survey-oddrow");

    //fix width in dialog form
    if (JSRequest.QueryString["IsDlg"]) {
        $("#s4-bodyContainer").removeClass("width960");
        $(".ms-cui-ribbonTopBars").width("auto");
    }

});