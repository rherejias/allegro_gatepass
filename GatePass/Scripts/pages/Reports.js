$(document).ready(function () {
    $("#cmbStatus").change(function () {
        if ($(this).val() == "Please choose") {
            //applyFilter('status_string', $(this).attr('data-grid'), "");
        }
        else {
            applyFilter('status_string', $(this).attr('data-grid'), $(this).val());
        }
       
    });
});

//rherejias search 3/16/2017
$(document).delegate("#ReportsGrid_searchField", "keyup", function (e) {
    var columns = ["Gatepass_ID_string", "impex_string",
                   "returndate_date", "purpose_string", "status_string", "requestor_string", "department_string", "dateadded_date"];
    generalSearch($('#ReportsGrid_searchField').val(), "ReportsGrid", columns, e);
});

$(document).delegate("#DeptReport_searchField", "keyup", function (e) {
    var columns = ["Gatepass_ID_string", "impex_string",
                   "returndate_date", "purpose_string", "status_string", "requestor_string", "department_string", "dateadded_date", "approver_string"];
    generalSearch($('#DeptReport_searchField').val(), "DeptReport", columns, e);
});

$(document).delegate("#DeptReportReject_searchField", "keyup", function (e) {
    var columns = ["Gatepass_ID_string", "impex_string",
                   "returndate_date", "purpose_string", "status_string", "requestor_string", "department_string", "dateadded_date", "approver_string"];
    generalSearch($('#DeptReportReject_searchField').val(), "DeptReportReject", columns, e);
});

$(document).delegate("#ITReport_searchField", "keyup", function (e) {
    var columns = ["Gatepass_ID_string", "impex_string",
                   "returndate_date", "purpose_string", "status_string", "requestor_string", "department_string", "dateadded_date", "approver_string"];
    generalSearch($('#ITReport_searchField').val(), "ITReport", columns, e);
});

$(document).delegate("#ITReportReject_searchField", "keyup", function (e) {
    var columns = ["Gatepass_ID_string", "impex_string",
                   "returndate_date", "purpose_string", "status_string", "requestor_string", "department_string", "dateadded_date", "approver_string"];
    generalSearch($('#ITReportReject_searchField').val(), "ITReportReject", columns, e);
});

$(document).delegate("#PurchReport_searchField", "keyup", function (e) {
    var columns = ["Gatepass_ID_string", "impex_string",
                   "returndate_date", "purpose_string", "status_string", "requestor_string", "department_string", "dateadded_date", "approver_string"];
    generalSearch($('#PurchReport_searchField').val(), "PurchReport", columns, e);
});

$(document).delegate("#PurchReportReject_searchField", "keyup", function (e) {
    var columns = ["Gatepass_ID_string", "impex_string",
                   "returndate_date", "purpose_string", "status_string", "requestor_string", "department_string", "dateadded_date", "approver_string"];
    generalSearch($('#PurchReportReject_searchField').val(), "PurchReportReject", columns, e);
});

$(document).delegate("#AcctReport_searchField", "keyup", function (e) {
    var columns = ["Gatepass_ID_string", "impex_string",
                   "returndate_date", "purpose_string", "status_string", "requestor_string", "department_string", "dateadded_date", "approver_string"];
    generalSearch($('#AcctReport_searchField').val(), "AcctReport", columns, e);
});

$(document).delegate("#AcctReportReject_searchField", "keyup", function (e) {
    var columns = ["Gatepass_ID_string", "impex_string",
                   "returndate_date", "purpose_string", "status_string", "requestor_string", "department_string", "dateadded_date", "approver_string"];
    generalSearch($('#AcctReportReject_searchField').val(), "AcctReportReject", columns, e);
});

$(document).delegate("#OverrideReport_searchField", "keyup", function (e) {
    var columns = ["Gatepass_ID_string", "impex_string",
                   "returndate_date", "purpose_string", "requestor_string", "department_string", "dateadded_date", "override_by_string", "overriden_approver_string"];
    generalSearch($('#OverrideReport_searchField').val(), "OverrideReport", columns, e);
});

$(document).delegate("#OverrideReportReject_searchField", "keyup", function (e) {
    var columns = ["Gatepass_ID_string", "impex_string",
                   "returndate_date", "purpose_string", "requestor_string", "department_string", "dateadded_date", "override_by_string", "overriden_approver_string"];
    generalSearch($('#OverrideReportReject_searchField').val(), "OverrideReportReject", columns, e);
});

//rherjeias export of all grids 3/16/2017
$(document).delegate("#ReportsGrid_Export_to_Excel", "click", function () {
    var where = '';
    if ($("#cmbStatus").val() != null) {
        where = " WHERE [Status] ='" + $("#cmbStatus").val() + "'";
        
    }
    $.redirect('/Reports/ExportToExcel', {
        'where': where, 'viewName': ' vwTransactionHeader', 'title': 'All',
        'select': 'SELECT Id,code,ImpexRefNbr,ReturnDate,Purpose,Status,Attachment,Requestor,Department,DateAdded FROM' }, 'post');
  
    $.ajax({
        url: '/Reports/export',
        type: 'post',
        dataType: 'json',
        data: {
            operation: 'Export_Reports_All_Records',
            table: 'vwTransactionRecords'
        }
    });
});

$(document).delegate("#DeptReport_Export_to_Excel", "click", function () {
    $.redirect('/Reports/ExportToExcel', {
        'where': ' WHERE [IsApproved] = 1', 'viewName': ' vwReportDeptApproved', 'title': 'Department Approved',
        'select': 'SELECT Id,code,ImpexRefNbr,ReturnDate,Purpose,Status,Attachment,Requestor,Department,DateAdded FROM' }, 'post');
    $.ajax({
        url: '/Reports/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Reports_Department',
            table: 'vwTransactionRecords'
        }
    });
});

$(document).delegate("#DeptReportReject_Export_to_Excel", "click", function () {
    $.redirect('/Reports/ExportToExcel', {
        'where': ' WHERE [IsApproved] = 0', 'viewName': ' vwRejectedByDeptHead', 'title': 'Department Reject',
        'select': 'SELECT Id,code,ImpexRefNbr,ReturnDate,Purpose,Status,Attachment,Requestor,Department,DateAdded FROM' }, 'post');
    $.ajax({
        url: '/Reports/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Reports_Department_Reject',
            table: 'vwTransactionRecords'
        }
    });
});

$(document).delegate("#ITReport_Export_to_Excel", "click", function () {
    $.redirect('/Reports/ExportToExcel', {
        'where': ' WHERE [IsApproved] = 1', 'viewName': ' vwReportITApproved', 'title': 'IT Approved',
        'select': 'SELECT Id,code,ImpexRefNbr,ReturnDate,Purpose,Status,Attachment,Requestor,Department,DateAdded FROM' }, 'post');
    $.ajax({
        url: '/Reports/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Reports_IT',
            table: 'vwTransactionRecords'
        }
    });
});

$(document).delegate("#ITReportReject_Export_to_Excel", "click", function () {
    $.redirect('/Reports/ExportToExcel', {
        'where': ' WHERE [IsApproved] = 0', 'viewName': ' vwReportITReject', 'title': 'IT Reject',
        'select': 'SELECT Id,code,ImpexRefNbr,ReturnDate,Purpose,Status,Attachment,Requestor,Department,DateAdded FROM' }, 'post');
    $.ajax({
        url: '/Reports/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Reports_IT_Reject',
            table: 'vwTransactionRecords'
        }
    });
});

$(document).delegate("#PurchReport_Export_to_Excel", "click", function () {
    $.redirect('/Reports/ExportToExcel', {
        'where': ' WHERE [IsApproved] = 1', 'viewName': ' vwReportPurch', 'title': 'Purchasing Approved',
        'select': 'SELECT Id,code,ImpexRefNbr,ReturnDate,Purpose,Status,Attachment,Requestor,Department,DateAdded FROM' }, 'post');
    $.ajax({
        url: '/Reports/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Reports_Purch',
            table: 'vwTransactionRecords'
        }
    });
});

$(document).delegate("#PurchReportReject_Export_to_Excel", "click", function () {
    $.redirect('/Reports/ExportToExcel', {
        'where': ' WHERE [IsApproved] = 0', 'viewName': ' vwReportPurchReject', 'title': 'Purchasing Reject',
        'select': 'SELECT Id,code,ImpexRefNbr,ReturnDate,Purpose,Status,Attachment,Requestor,Department,DateAdded FROM' }, 'post');
    $.ajax({
        url: '/Reports/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Reports_Purch_Reject',
            table: 'vwTransactionRecords'
        }
    });
});

$(document).delegate("#AcctReport_Export_to_Excel", "click", function () {
    $.redirect('/Reports/ExportToExcel', {
        'where': ' WHERE [IsApproved] = 1', 'viewName': ' vwReportAcct', 'title': 'Accounting Approved',
        'select': 'SELECT Id,code,ImpexRefNbr,ReturnDate,Purpose,Status,Attachment,Requestor,Department,DateAdded FROM' }, 'post');
    $.ajax({
        url: '/Reports/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Reports_Acct',
            table: 'vwTransactionRecords'
        }
    });
});

$(document).delegate("#AcctReportReject_Export_to_Excel", "click", function () {
    $.redirect('/Reports/ExportToExcel', {
        'where': ' WHERE [IsApproved] = 0', 'viewName': ' vwReportAcctReject', 'title': 'Accounting Reject',
        'select': 'SELECT Id,code,ImpexRefNbr,ReturnDate,Purpose,Status,Attachment,Requestor,Department,DateAdded FROM' }, 'post');
    $.ajax({
        url: '/Reports/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Reports_Acct_Reject',
            table: 'vwTransactionRecords'
        }
    });
});

$(document).delegate("#OverrideReport_Export_to_Excel", "click", function () {
    $.redirect('/Reports/ExportToExcel', {
        'where': 'null', 'viewName': ' vwReportOverrideApprove', 'title': 'Override Approved',
        'select': 'SELECT Id,code,ImpexRefNbr,ReturnDate,Purpose,Status,Attachment,Requestor,Department,DateAdded,Overriden,Approver FROM'}, 'post');
    $.ajax({
        url: '/Reports/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Reports_Acct_Reject',
            table: 'vwTransactionRecords'
        }
    });
});

$(document).delegate("#OverrideReportReject_Export_to_Excel", "click", function () {
    $.redirect('/Reports/ExportToExcel', {
        'where': 'null', 'viewName': ' vwReportOverrideReject', 'title': 'Override Reject',
        'select': 'SELECT Id,code,ImpexRefNbr,ReturnDate,Purpose,Status,Attachment,Requestor,Department,DateAdded FROM'
    }, 'post');
    $.ajax({
        url: '/Reports/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Reports_Acct_Reject',
            table: 'vwTransactionRecords'
        }
    });
});


$(document).delegate("#ReportsGrid_Clear_Filter", "click", function () {
    $('#ReportsGrid').jqxGrid('clearfilters');
    $("#cmbStatus").val('Please choose');
});

var applyFilter = function (datafield, grid, _code) {

    if (typeof grid == "undefined" || grid == '' || typeof grid == "null") {
        console.log("Grid not set!");
    } else {
        var ctr = 0;
        $("#" + grid).jqxGrid('clearfilters');
        var filtertype = 'stringfilter';
        var filtergroup = new $.jqx.filter();

        var filter_or_operator = 0;
        var filtervalue = "'" + _code + "'";
        var filtercondition = 'equal';
        var filter = filtergroup.createfilter(filtertype, filtervalue, filtercondition);
        filtergroup.addfilter(filter_or_operator, filter);

        $("#" + grid).on("bindingcomplete", function (event) {
            if (ctr == 0) {
                $("#" + grid).jqxGrid('addfilter', datafield, filtergroup);
                $("#" + grid).jqxGrid('applyfilters');
            }
            ctr = 1;
        });

    }
}



$("#ReportsGrid").on('rowdoubleclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#ReportsGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#ReportsGrid").jqxGrid('getrowdatabyid', rowID);


    showform.GP_ItemDetails(data.Gatepass_ID_string);

});

var showform = {
    GP_ItemDetails: function (headercode) {
        var modal = '<div class="modal fade" id="modalDetails" role="dialog" >';
        modal += '<div class="modal-dialog modal-lg">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-body" style="margin-top:4%">';

        modal +=  '<div class="row">';
        modal += '<div class="col-md-12">';
        modal +=  '<div class="jqxgrid jqxgrid-filterable jqxgrid-sortable jqxgrid-autoheight jqxgrid-enablehover jqxgrid-enableellipsis jqxgrid-virtualmode jqxgrid-pagesizeoptions jqxgrid-columnsresize jqxgrid-editable"';
        modal +=  'id="girdforreportdetails"';
        modal +=  'grid-width="100"';
        modal += 'data-url="/Transactions/GetAll_Item_ReturnSlip?HeaderKey=' + headercode + '"';
        modal +=  'grid-selection-mode="singlerow"';
        modal +=  'grid-hide-columns="0,1,2,3,4,6,10,13,14,15,16,17,18"></div>';
        modal +=  '</div>';
        modal +=  '</div>';

        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalDetails").modal("show");
        $("#modalDetails").css('z-index', '1000000');
        initialize_jqxwidget_grid($("#girdforreportdetails"));
    }
}