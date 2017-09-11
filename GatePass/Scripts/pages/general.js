window.gridTheme = 'metro';
window.screenLock = 300000;   //screen lock in milliseconds 300000 = 5min

$(document).ready(function () {

    $('.counter').counterUp({
        delay: 10,
        time: 1000
    });

    ini_main.elements();

    $(".log-out").click(function () {
        LogOut();
    });

    $('.js-example-basic-single').select2({
        minimumResultsForSearch: Infinity
    });
});



function notification_modal(title, message, type) {
    var header_style;
    var headericon;
    if (type == "success") {
        header_style = 'style="background-color: #1DB198; color: #ffffff;"';
        headericon = '<i class="fa fa-check-circle fa-lg" aria-hidden="true"></i>&nbsp;';
    }
   
    else if (type == "danger") {
        header_style = 'style="background-color: #F25656; color: #ffffff;"';
        headericon = '<i class="fa fa-exclamation-circle fa-lg" aria-hidden="true"></i>&nbsp;';
    }
   

    var modal = '<div class="modal fade" id="modal_div" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false">';
    modal += '<div class="modal-dialog modal-sm">';
    modal += '<div class="modal-content">';

    modal += '<div class="modal-header" ' + header_style + '>';
    modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
    modal += '<h4 class="modal-title" id="myModalLabel">'+ headericon + title + '</h4>';
    modal += '</div>';

    modal += '<div class="modal-body"><br />';
    modal += message;
    modal += '</div>';

    modal += '<div class="modal-footer">';
    modal += '<a id="btn_notifClose" data-dismiss="modal" class="btn btn-small btn-gray">Close</a>';
    modal += '</div>';

    modal += '</div>';
    modal += '</div>';
    modal += '</div>';

    $("#notification_modal").html(modal);
    $("#modal_div").modal("show");
    $("#modal_div").css('z-index', '1000001');
}



function setIdleTimeout(millis, onIdle, onUnidle) {
    var timeout = 0;
    $(startTimer);

    function startTimer() {
        timeout = setTimeout(onExpires, millis);
        $(document).on("mousemove keypress", onActivity);
    }

    function onExpires() {
        timeout = 0;
        onIdle();
    }

    function onActivity() {
        if (timeout) clearTimeout(timeout);
        else onUnidle();
        //since the mouse is moving, we turn off our event hooks for 1 second
        $(document).off("mousemove keypress", onActivity);
        setTimeout(startTimer, 1000);
    }
}

function buildChart(containerId, elemId, _url) {
    $.ajax({
        url: _url,
        dataType: 'html',
        type: 'get',
        data: {},
        beforeSend: function () {

        },
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        },
        success: function (response) {
            $("#" + containerId).html(response);
            //document.getElementById(containerId).innerHTML = response;
            window['init_hchart_' + elemId]();
            //window.init_hchart_id_ac();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(thrownError);
        }
    });


}

var ini_main = {
    elements: function () {
        if ($(".InputDateTimeRange").length > 0) {

            $(".InputDateTimeRange").jqxDateTimeInput({
                theme: 'light',
                width: 200,
                height: 25,
                selectionMode: 'range',
                formatString: 'yyyy-MM-dd',
            });
        }

        if ($(".InputDate").length > 0) {

            $(".InputDate").jqxDateTimeInput({
                theme: 'light',
                width: '100%',
                height: 25,
                //selectionMode: 'range',
                formatString: 'yyyy-MM-dd',
                min: new Date()
            });

        }

        if ($(".jqxgrid").length > 0) {
            $('.jqxgrid').each(function () {
                var elem = $(this);
                initialize_jqxwidget_grid(elem);
                elem.jqxGrid('autoresizecolumns');
            });
        }

        //initialize charts
        if ($('.hchart_container').length > 0) {
            $('.hchart_container').each(function() {
                window['init_chart_' + $(this).attr('id')]();
            });
        }
    },
    element: function (elemtype) {
        switch(elemtype) {
            case 'dropdownlist':

                if ($("." + elemtype).length > 0) {
                    $("." + elemtype).each(function () {
                        var elem = $(this);

                        var w = '95%';
                        if (typeof elem.attr('data-width') != 'undefined' && elem.attr('data-width') != '') {
                            w = elem.attr('data-width');
                        }

                        var h = '20';
                        if (typeof elem.attr('data-height') != 'undefined' && elem.attr('data-height') != '') {
                            h = elem.attr('data-height');
                        }

                        var f = true;
                        if (typeof elem.attr('data-filterable') != 'undefined' && elem.attr('data-filterable') != '') {
                            f = elem.attr('data-filterable');
                        }

                        var d = false;
                        if (typeof elem.attr('data-disabled') != 'undefined' && elem.attr('data-disabled') != '') {
                            d = elem.attr('data-disabled');
                        }

                        //for placeholders -rherejias 11/16/16 10:00 AM
                        var ph = "Please Choose...";
                        if (typeof elem.attr('data-placeholder') != 'undefined' && elem.attr('data-placeholder') != '') {
                            ph = elem.attr('data-placeholder');
                        }

                        //for seklectedindex -rherejias 2/17/17 11:08 AM
                        var si = 0;
                        if (typeof elem.attr('data-selectedindex') != 'undefined' && elem.attr('data-selectedindex') != '') {
                            si = elem.attr('data-selectedindex');
                        }

                        var source =
                                {
                                    datatype: "json",
                                    datafields: [
                                        { name: elem.attr("data-display") },
                                        { name: elem.attr("data-value") }
                                    ],
                                    url: elem.attr("data-url"),
                                    async: true,
                                    root: "data"
                                };
                        var dataAdapter = new $.jqx.dataAdapter(source);
                        
                        // Create a jqxDropDownList
                        elem.jqxDropDownList({
                            theme: window.gridTheme,
                            filterable: f,
                            selectedIndex: si,
                            source: dataAdapter,
                            displayMember: elem.attr("data-display"),
                            valueMember: elem.attr("data-value"),
                            width: w,
                            height: h,
                            disabled: d,
                            placeHolder: ph
                        });

                        //alert('sdfdsfds');
                        //elem.jqxDropDownList('val', "Cecetech Corporation Inc. (Dungo ,Cecilia A.)");
                    });
                }

              
                break;


                case 'dropdownlistApprover':

                if ($("." + elemtype).length > 0) {
                    $("." + elemtype).each(function () {
                        var elem = $(this);

                        var w = '95%';
                        if (typeof elem.attr('data-width') != 'undefined' && elem.attr('data-width') != '') {
                            w = elem.attr('data-width');
                        }

                        var h = '20';
                        if (typeof elem.attr('data-height') != 'undefined' && elem.attr('data-height') != '') {
                            h = elem.attr('data-height');
                        }

                        var f = true;
                        if (typeof elem.attr('data-filterable') != 'undefined' && elem.attr('data-filterable') != '') {
                            f = elem.attr('data-filterable');
                        }

                        var d = false;
                        if (typeof elem.attr('data-disabled') != 'undefined' && elem.attr('data-disabled') != '') {
                            d = elem.attr('data-disabled');
                        }

                        //for placeholders -rherejias 11/16/16 10:00 AM
                        var ph = "Please Choose...";
                        if (typeof elem.attr('data-placeholder') != 'undefined' && elem.attr('data-placeholder') != '') {
                            ph = elem.attr('data-placeholder');
                        }

                        var source =
                                {
                                    datatype: "json",
                                    datafields: [
                                        { name: elem.attr("data-display") },
                                        { name: elem.attr("data-value") }
                                    ],
                                    url: elem.attr("data-url"),
                                    async: true,
                                    root: "data"
                                };
                        var dataAdapter = new $.jqx.dataAdapter(source);
                        
                        // Create a jqxDropDownList
                        elem.jqxDropDownList({
                            theme: window.gridTheme,
                            filterable: f,
                            selectedIndex: 0,
                            source: dataAdapter,
                            displayMember: elem.attr("data-display"),
                            valueMember: elem.attr("data-value"),
                            width: w,
                            height: h,
                            disabled: d,
                            placeHolder: ph
                        });

                        //alert('sdfdsfds');
                        //elem.jqxDropDownList('val', "Cecetech Corporation Inc. (Dungo ,Cecilia A.)");
                    });
                }

              
                break;




            case 'inputtext':
                
                if ($("." + elemtype).length > 0) {
                    $("." + elemtype).each(function () {
                        var elem = $(this);

                        var p = '';
                        if (typeof elem.attr('data-placeHolder') != 'undefined' && elem.attr('data-placeHolder') != '') {
                            p = elem.attr('data-placeHolder');
                        }

                        var w = '95%';
                        if (typeof elem.attr('data-width') != 'undefined' && elem.attr('data-width') != '') {
                            w = elem.attr('data-width');
                        }

                        var h = '20';
                        if (typeof elem.attr('data-height') != 'undefined' && elem.attr('data-height') != '') {
                            h = elem.attr('data-height');
                        }

                        var source =
                          {
                              datatype: "json",
                              datafields: [
                                  { name: 'NAME' },
                                  { name: 'VALUE' }
                              ],
                              url: elem.attr("data-url")
                          };
                        var dataAdapter = new $.jqx.dataAdapter(source);

                        elem.jqxInput({
                            theme: window.gridTheme,
                            placeHolder: p,
                            width: w,
                            height: h,
                            source: dataAdapter,
                            displayMember: "NAME",
                            valueMember: "VALUE"
                        });
                    });
                }
                
                break;
            case 'inputnumber':
                
                if ($("." + elemtype).length > 0) {
                    $("." + elemtype).each(function () {
                        var elem = $(this);

                        var w = '95%';
                        if (typeof elem.attr('data-width') != 'undefined' && elem.attr('data-width') != '') {
                            w = elem.attr('data-width');
                        }

                        var h = '20';
                        if (typeof elem.attr('data-height') != 'undefined' && elem.attr('data-height') != '') {
                            h = elem.attr('data-height');
                        }

                        elem.jqxNumberInput({
                            theme: window.gridTheme,
                            spinButtons: true,
                            width: w,
                            height: h,
                            min: 1,
                            value: 1
                        });
                    });
                }

                break;
        }
    }
};

function LogOut(containerId, elemId, _url) {
    $.ajax({
        url: "/Account/Logout",
        dataType: 'json',
        type: 'get',
        data: {},
        beforeSend: function () {

        },
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        },
        success: function (response) {
            window.location = "/Account/Login";
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(thrownError);
        }
    });
}

function loader_modal(title, message) {
    var header_style;
    header_style = 'style="background-color: #12AECA; color: #ffffff;"';

    var modal = '<div class="modal fade" id="modal_div_loader" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
    modal += '<div class="modal-dialog">';
    modal += '<div class="modal-content">';

    modal += '<div class="modal-header" ' + header_style + '>';
    //modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>';
    modal += '<h4 class="modal-title" id="myModalLabel">' + title + '</h4>';
    modal += '</div>';

    modal += '<div class="modal-body">';
    //modal += message;
    modal += '<br /><br /><div class="progress"><div class="progress-bar progress-bar-info progress-bar-striped active" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%">' + message + '</div></div>';
    modal += '</div>';

    modal += '<div class="modal-footer">';
    //modal += '<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>';
    //modal += '<button type="button" class="btn btn-success">Save changes</button>';
    modal += '</div>';

    modal += '</div>';
    modal += '</div>';
    modal += '</div>';

    $("#loader_modal").html(modal);
    $("#modal_div_loader").modal({ backdrop: 'static', keyboard: false });
    $("#modal_div_loader").modal("show");
    $("#modal_div_loader").css('z-index', '1000000');
}
// rherejias 11/23/16 for special key input on search 
function specialKeys() {
    var keys = ["20", "16", "17", "91", "18","13","93", "27", "112",
            "113", "114", "115", "116", "117", "118", "119", "120", "121", "122",
            "123", "37", "38", "39", "40", "33", "34", "35", "36", "46",
            "45", "19", "145"];

    return keys;
}

//rherejias 11/23/16 for checking of array values
Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        if (this[i] == obj) {
            return true;
        }
    }
    return false;
}

//rherejias 11/23/16 for generalSearch
function generalSearch(input, gridID, columns, e) {

    var keyCode = e.keyCode || e.which;

    var keys = specialKeys();
    if (keys.contains(keyCode)) { }
    else {
        if ($('#' + gridID + '_searchField').val() == '') {
            $('#' + gridID).jqxGrid('clearfilters');
            $('#' + gridID).jqxGrid('updatebounddata');
        }
        else {
            var filtertype = 'stringfilter';
            var filtergroup = new $.jqx.filter();
            var filter_or_operator = 0;
            var filtervalue = input;
            var filtercondition = 'contains';
            var filter = filtergroup.createfilter(filtertype, filtervalue, filtercondition);
            filtergroup.addfilter(1, filter);

            for (var i = 0; i < columns.length; i++) {
                $('#' + gridID).jqxGrid('addfilter', columns[i], filtergroup);
            }
            $('#' + gridID).jqxGrid('applyfilters');
        }
    }

}

var getCookies = function () {
    var pairs = document.cookie.split(";");
    var cookies = {};
    for (var i = 0; i < pairs.length; i++) {
        var pair = pairs[i].split("=");

        if (pair[0].toLowerCase().trim() == "luna2017") {
            var n = pairs[i].indexOf("=");
            var getparams_str = pairs[i].substring(n + 1);
            var getparams_arr = getparams_str.split("&");
            console.log(getparams_arr);
            for (var x = 0; x < getparams_arr.length; x++) {
                var param = getparams_arr[x].split("=");
                cookies[param[0]] = param[1];
            }
        }
    }
    return cookies;
}



/// desc: Validation of maximum length of contact number
/// by : avillena@allegromicro.com
/// jan, 20, 2017
function maxLengthCheck(object) {
    if (object.value.length > object.maxLength)
        object.value = object.value.slice(0, object.maxLength)
}

function isNumeric(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}


// desc : display snacknotification
// date: jan 18, 2017
// by: avillena@allegromicro.com
function Warning_ToastNotif() {

   $('.warning').addClass("show")
   setTimeout(function () { $('.warning').removeClass("show") }, 4000);

}

function Fail_ToastNotif() {
    $('.fail').addClass("show")
    $('.fail').css('z-index', '1000000001');
    setTimeout(function () { $('.fail').removeClass("show") }, 4000);

}

function Save_ToastNotif() {

    $('.save').addClass("show")
    setTimeout(function () { $('.save').removeClass("show") }, 4000);

}


function Save_ToastNotifMaintenance() {

    $('.maintenance').addClass("show")
    $('.maintenance').css('z-index', '1000000001');
    setTimeout(function () { $('.maintenance').removeClass("show") }, 4000);

}


function ReturnDate_Warning() {

    $('.returndateWarning').addClass("show")
    setTimeout(function () { $('.returndateWarning').removeClass("show") }, 4000);

}


// Get Date Today
function getDate() {
        var currentdate = new Date();
        var datetime = currentdate.getFullYear() + "-"
                    + (currentdate.getMonth() + 1) + "-"
                    + currentdate.getDate();
        return datetime;
}



