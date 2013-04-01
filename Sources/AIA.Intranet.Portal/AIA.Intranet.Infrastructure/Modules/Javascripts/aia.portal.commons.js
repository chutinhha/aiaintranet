$(document).ready(function () {

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

    //Elements with the potential to be too wide.
    elements = $(".ms-bodyareacell > div > *, .main-container > div > *");
    leftPanelWidth = $("#s4-leftpanel").width();

    //For each Elements
    $(elements).each(function () {

        //if it's wider than the side width
        if ($(this).width() > ($("#s4-bodyContainer").width() - leftPanelWidth)) {
            //Calculate the new width taking the left nav into account
            newWidth = leftPanelWidth + $(this).width();
            //Set the width!
            $("#s4-bodyContainer").attr("style", "width:" + newWidth + "px!important")
        }
    });
});