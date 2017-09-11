var index, indexIT, indexPurchasing, indexAccounting;
var indexRE, indexITRE, indexPurchasingRE, indexAccountingRE;
var approvalType = '';
var department = '';
var user = '';
var userEdit = '';
var departmentEdit = '';
var indexEdit = '';
var indexDeactivate = '';
var indexReactivate = '';
var indexAssign = '';


//rherjias for grid search
$(document).ready(function () {
    $(document).delegate("#ApproverGrid_searchField", "keyup", function (e) {
        var columns = ["approval_type_string", "department_string", "user_string", "email_string"];
        generalSearch($('#ApproverGrid_searchField').val(), 'ApproverGrid', columns, e);
    });

    $(document).delegate("#ApproverGridInactive_searchField", "keyup", function (e) {
        var columns = ["approval_type_string", "department_string", "user_string", "email_string"];
        generalSearch($('#ApproverGridInactive_searchField').val(), 'ApproverGridInactive', columns, e);
    });

    $(document).delegate("#ITRelatedGrid_searchField", "keyup", function (e) {
        var columns = ["approval_type_string", "department_string", "user_string", "email_string"];
        generalSearch($('#ITRelatedGrid_searchField').val(), 'ITRelatedGrid', columns, e);
    });

    $(document).delegate("#ITRelatedGridInactive_searchField", "keyup", function (e) {
        var columns = ["approval_type_string", "department_string", "user_string", "email_string"];
        generalSearch($('#ITRelatedGridInactive_searchField').val(), 'ITRelatedGridInactive', columns, e);
    });

    $(document).delegate("#PurchasingGrid_searchField", "keyup", function (e) {
        var columns = ["approval_type_string", "department_string", "user_string", "email_string"];
        generalSearch($('#PurchasingGrid_searchField').val(), 'PurchasingGrid', columns, e);
    });

    $(document).delegate("#PurchasingGridInactive_searchField", "keyup", function (e) {
        var columns = ["approval_type_string", "department_string", "user_string", "email_string"];
        generalSearch($('#PurchasingGridInactive_searchField').val(), 'PurchasingGridInactive', columns, e);
    });

    $(document).delegate("#AccountingGrid_searchField", "keyup", function (e) {
        var columns = ["approval_type_string", "department_string", "user_string", "email_string"];
        generalSearch($('#AccountingGrid_searchField').val(), 'AccountingGrid', columns, e);
    });

    $(document).delegate("#AccountingGridInactive_searchField", "keyup", function (e) {
        var columns = ["approval_type_string", "department_string", "user_string", "email_string"];
        generalSearch($('#AccountingGridInactive_searchField').val(), 'AccountingGridInactive', columns, e);
    });
});

//rherejias onlick events
$(document).delegate("#btnSaveApprover", "click", function () {
    dbase_operation.addApprover($(this).attr('data-type'));
});

$(document).delegate("#btnSaveApproverEdit", "click", function () {
    if ($(this).attr('data-type') == 'department')
        indexEdit = index;
    else if ($(this).attr('data-type') == 'it')
        indexEdit = indexIT;
    else if ($(this).attr('data-type') == 'purchasing')
        indexEdit = indexPurchasing;
    else if ($(this).attr('data-type') == 'accounting')
        indexEdit = indexAccounting;

    dbase_operation.editApprover($(this).attr('data-type'), indexEdit);
});

$(document).delegate("#btnProceedActivate", "click", function () {
    if ($(this).attr('data-type') == 'department')
        indexReactivate = indexRE;
    else if ($(this).attr('data-type') == 'it')
        indexReactivate = indexITRE;
    else if ($(this).attr('data-type') == 'purchasing')
        indexReactivate = indexPurchasingRE;
    else if ($(this).attr('data-type') == 'accounting')
        indexReactivate = indexAccountingRE;

    dbase_operation.activeApprover($(this).attr('data-type'), indexReactivate);
});

$(document).delegate("#btnProceedDeactivate", "click", function () {
    if ($(this).attr('data-type') == 'department')
        indexDeactivate = index;
    else if ($(this).attr('data-type') == 'it')
        indexDeactivate = indexIT;
    else if ($(this).attr('data-type') == 'purchasing')
        indexDeactivate = indexPurchasing;
    else if ($(this).attr('data-type') == 'accounting')
        indexDeactivate = indexAccounting;

    dbase_operation.inactiveApprover($(this).attr('data-type'),indexDeactivate);
});
//rherejias assign btn
$(document).delegate(".ITRelatedGrid_Set_as_primary", "click", function () {
    if (indexIT.approval_type_code_string == itApprover) {
        toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " " + "<b>"+indexIT.user_string + "</b> is the current approver.";
        Fail_ToastNotif();
    }
    else {
        showModal.assignApproverModal('it', indexIT.user_string);
    }
  
});

$(document).delegate(".PurchasingGrid_Set_as_primary", "click", function () {
    if (indexPurchasing.approval_type_code_string == purchasingApprover) {
        toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " " + "<b>" + indexPurchasing.user_string + "</b> is the current approver.";
        Fail_ToastNotif();
    }
    else {
        showModal.assignApproverModal('purchasing', indexPurchasing.user_string);
    }
});

$(document).delegate(".AccountingGrid_Set_as_primary", "click", function () {
    if (indexAccounting.approval_type_code_string == accountingApprover) {
        toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " " + "<b>" + indexAccounting.user_string + "</b> is the current approver.";
        Fail_ToastNotif();
    }
    else {
        showModal.assignApproverModal('accounting', indexAccounting.user_string);
    }
});

//rherejias assign proceed btn
$(document).delegate("#btnProceedAssignApprover", "click", function () {
    if ($(this).attr('data-type') == 'it')
        indexAssign = indexIT;
    else if ($(this).attr('data-type') == 'purchasing')
        indexAssign = indexPurchasing;
    else if ($(this).attr('data-type') == 'accounting')
        indexAssign = indexAccounting;

    dbase_operation.assignApprover($(this).attr('data-type'), indexAssign);
});

//rherejias activate grid
$("#ApproverGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#ApproverGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#ApproverGrid").jqxGrid('getrowdatabyid', rowID);
    index = data;
});

$("#ITRelatedGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#ITRelatedGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#ITRelatedGrid").jqxGrid('getrowdatabyid', rowID);
    indexIT = data;
});

$("#PurchasingGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#PurchasingGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#PurchasingGrid").jqxGrid('getrowdatabyid', rowID);
    indexPurchasing = data;
});

$("#AccountingGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#AccountingGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#AccountingGrid").jqxGrid('getrowdatabyid', rowID);
    indexAccounting = data;
});

//rherejias inactive grid
$("#ApproverGridInactive").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#ApproverGridInactive").jqxGrid('getrowid', boundIndex);
    var data = $("#ApproverGridInactive").jqxGrid('getrowdatabyid', rowID);
    indexRE = data;
});

$("#ITRelatedGridInactive").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#ITRelatedGridInactive").jqxGrid('getrowid', boundIndex);
    var data = $("#ITRelatedGridInactive").jqxGrid('getrowdatabyid', rowID);
    indexITRE = data;
});

$("#PurchasingGridInactive").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#PurchasingGridInactive").jqxGrid('getrowid', boundIndex);
    var data = $("#PurchasingGridInactive").jqxGrid('getrowdatabyid', rowID);
    indexPurchasingRE = data;
});

$("#AccountingGridInactive").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#AccountingGridInactive").jqxGrid('getrowid', boundIndex);
    var data = $("#AccountingGridInactive").jqxGrid('getrowdatabyid', rowID);
    indexAccountingRE = data;
});

// rherejais add btn
$("#cmdAddNew").click(function () {
    showModal.addApprover("add", index, "department");
});

$("#cmdAddNewIT").click(function () {
    showModal.addApprover("add", indexIT, "it");
});

$("#cmdAddNewPurchasing").click(function () {
    showModal.addApprover("add", indexPurchasing, "purchasing");
});

$("#cmdAddNewAccounting").click(function () {
    showModal.addApprover("add", indexAccounting, "accounting");
});

//rherejias edit btn
$(document).delegate(".ApproverGrid_Edit", "click", function () {
    showModal.addApprover("edit", index, "department");
});

$(document).delegate(".ITRelatedGrid_Edit", "click", function () {
    showModal.addApprover("edit", indexIT, "it");
});

$(document).delegate(".PurchasingGrid_Edit", "click", function () {
    showModal.addApprover("edit", indexPurchasing, "purchasing");
});

$(document).delegate(".AccountingGrid_Edit", "click", function () {
    showModal.addApprover("edit", indexAccounting, "accounting");
});

//rherejias inactive btn
$(document).delegate(".ApproverGrid_Inactive", "click", function () {
    showModal.deactivate('department');
});

$(document).delegate(".ITRelatedGrid_Inactive", "click", function () {
    showModal.deactivate('it');
});

$(document).delegate(".PurchasingGrid_Inactive", "click", function () {
    showModal.deactivate('purchasing');
});

$(document).delegate(".AccountingGrid_Inactive", "click", function () {
    showModal.deactivate('accounting');
});

//rherejias activate btn
$(document).delegate(".ApproverGridInactive_Activate", "click", function () {
    showModal.activate('department');
});

$(document).delegate(".ITRelatedGridInactive_Activate", "click", function () {
    showModal.activate('it');
});

$(document).delegate(".PurchasingGridInactive_Activate", "click", function () {
    showModal.activate('purchasing');
});

$(document).delegate(".AccountingGridInactive_Activate", "click", function () {
    showModal.activate('accounting');
});

//end of onclick events

//rherejias modals
var showModal = {
    addApprover: function (operation, index, dataType) {
        userEdit = "";
        departmentEdit = "";
        if (operation == "edit") {
            approvalType = index.approval_type_string;
            department = index.department_string;
            user = index.user_string;
            userEdit = index.user_code_string;
            departmentEdit = index.department_code_string;
        }
        var source = ["Primary Approver","Secondary Approver" ];
      

        var modal = '<style>';
        modal += ' .textstyle {';
        modal += '      background-color: transparent;';
        modal += '      outline: none;';
        modal += '      outline-style: none;';
        modal += '      outline-offset: 0;';
        modal += '      border-top: none;';
        modal += '      border-left: none;';
        modal += '      border-right: none;';
        modal += '      border-bottom: 1px solid #e5e5e5;';
        modal += '      padding: 3px 10px;';
        modal += '}';
        modal += ' .form-control:focus {';
        modal += '      border-top: 0;';
        modal += '      border-left: 0;';
        modal += '      border-right: 0;';
        modal += '      border-bottom: 1px solid #e5e5e5;';
        modal += '}';
        modal += '</style>';

        modal += '<div class="modal fade" id="modalAddApprover" role="dialog" >';
        modal += '<div class="modal-dialog">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal += '<h4 class="modal-title">'+ ((operation == 'edit') ? 'Update Approver' : 'Add Approver') +'</h4>';
        modal += '</div>';
        modal += '<div class="modal-body">';

        modal += '<br/>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '              <div class="form-control dropdownlist textstyle" data-placeholder="User" id="cmbUser" data-url="/Users/GetRegisteredUsers" data-display="Username_string" data-value="code_string"></div>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        if (dataType == 'department') {
            modal += '<div class="row">';
            modal += '  <div class="col-md-12">';
            modal += '          <div class="form-group">';
            modal += '             <div class="form-control dropdownlist textstyle" data-placeholder="Department" id="cmbDepartment" data-url="/Departments/GetDepartment" data-display="name_string" data-value="code_string"></div>';
            modal += '          </div>';
            modal += '  </div>';
            modal += '</div>';
        }
        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-medium btn-blue" data-grid=' + index + ' data-type= ' + dataType + '  id="' + ((operation == 'edit') ? 'btnSaveApproverEdit' : 'btnSaveApprover') + '"';
        modal += 'style="width: 100px;">';
        modal += ''+((operation == 'add') ? 'Add' : 'Update')+'</a>';
        modal += '<a type="button" style="width: 100px;" class="btn btn-medium btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

 

        $("#form_modal").html(modal);
        $("#modalAddApprover").modal("show");
        $("#modalAddApprover").css('z-index', '1000000');

        ini_main.element('dropdownlist');
        ini_main.element('inputtext');

        $("#cmbUser").on('bindingComplete', function (event) {
            if (userEdit != "") {
                $("#cmbUser").jqxDropDownList('val', userEdit);
            }
        });
        $("#cmbDepartment").on('bindingComplete', function (event) {
            if (departmentEdit != "") {
                $("#cmbDepartment").jqxDropDownList('val', departmentEdit);
            }
        });
       
    },

    //@ver 1.0 rherejias 1/18/2017 used for inactive approver confimation modal for activate
    activate: function (dataType) {
        var modal = '<div class="modal fade" id="modalDeactivate" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#1DB198; color:#ffffff">';
        modal += '<h4 class="modal-title">Activate Approver</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to re-activate this Approver?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a data-type=' + dataType + ' class="btn btn-small btn-blue" id="btnProceedActivate">';
        modal += 'Confirm</a>';
        modal += '<a type="button" class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalDeactivate").modal("show");
        $("#modalDeactivate").css('z-index', '1000000');
    },


    deactivate: function (dataType) {
        var modal = '<div class="modal fade" id="modalDeactivate" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#F25656; color:#ffffff">';
        modal += '<h4 class="modal-title">Deactivate Approver</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to deactivate this Approver?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a data-type=' + dataType + ' class="btn btn-small btn-red" id="btnProceedDeactivate">';
        modal += 'Confirm</a>';
        modal += '<a type="button" class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalDeactivate").modal("show");
        $("#modalDeactivate").css('z-index', '1000000');
    },

        assignApproverModal: function (dataType, name) {
        var modal = '<div class="modal fade" id="modalAssignApprover" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#1DB198; color:#ffffff">';
        modal += '<h4 class="modal-title">Assign Approver</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to assign <b>'+ name +'</b> as primary approver?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a data-type=' + dataType + ' class="btn btn-small btn-green" id="btnProceedAssignApprover">';
        modal += 'Confirm</a>';
        modal += '<a type="button" class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalAssignApprover").modal("show");
        $("#modalAssignApprover").css('z-index', '1000000');
    },
}
//end of modals

//rherejias database operation add,edit,inactive,assignapprover
dbase_operation = {

    addApprover: function (dataType) {
        var elements = ["#cmbUser", "#cmbDepartment"]
        var ctrAdd = 0;
        var approvalType = '';
        if (dataType == 'department') 
            approvalType = departmentApprover
        else if (dataType == 'it')
            approvalType = secondaryITApprover
        else if (dataType == 'purchasing') {
            approvalType = secondaryPurchasingApprover;
        }
        else if (dataType == 'accounting') {
            approvalType = secondaryAccountingApprover;
        }
        for (var i = 0; i < 2; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "red");
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5");
                ctrAdd++;
            }
        }
        if (ctrAdd == 2) {
            $.ajax({
                url: '/Approver/addApprover',
                dataType: 'json',
                type: 'post',
                data: {
                    approvalTypeCode: approvalType,
                    departmentCode: $("#cmbDepartment").val(),
                    userCode: $("#cmbUser").val()
                },
                success: function (response) {
                    $("#modalAddApprover").modal("hide");
                    if (response.success) {
                        toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Approver created successfully.";
                        Save_ToastNotifMaintenance();
                        if (dataType == 'department')
                            $('#ApproverGrid').jqxGrid('updatebounddata');
                        else if (dataType == 'it')
                            $('#ITRelatedGrid').jqxGrid('updatebounddata');
                        else if (dataType == 'purchasing') 
                            $('#PurchasingGrid').jqxGrid('updatebounddata');
                        else if (dataType == 'accounting') 
                            $('#AccountingGrid').jqxGrid('updatebounddata');
                    }
                    else {
                        toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " " + response.message;
                        Fail_ToastNotif();
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status);
                    console.log(thrownError);
                }
            });
        }
    },

    editApprover: function (dataType, index) {
        if ($("#cmbDepartment").val() != departmentEdit || $("#cmbUser").val() != userEdit) {
            $.ajax({
                url: '/Approver/editApprover',
                dataType: 'json',
                type: 'post',
                data: {
                    id: index.id_number,
                    code: index.code_string,
                    approvalTypeCode:  index.approval_type_code_string,
                    departmentCode: ($("#cmbDepartment").val() == "") ? index.department_code_string : $("#cmbDepartment").val(),
                    userCode: ($("#cmbUser").val() == "") ? index.user_code_string : $("#cmbUser").val()
                },
                success: function (response) {
                    $("#modalAddApprover").modal("hide");
                    if (response.success) {
                        toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Approver updated successfully.";
                        Save_ToastNotifMaintenance();
                        if (dataType == 'department')
                            $('#ApproverGrid').jqxGrid('updatebounddata');
                        else if (dataType == 'it')
                            $('#ITRelatedGrid').jqxGrid('updatebounddata');
                        else if (dataType == 'purchasing')
                            $('#PurchasingGrid').jqxGrid('updatebounddata');
                        else if (dataType == 'accounting')
                            $('#AccountingGrid').jqxGrid('updatebounddata');
                    }
                    else {
                        toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' +" "+ response.message;
                         Fail_ToastNotif();
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status);
                    console.log(thrownError);
                }
            });
        }
        else {
            $("#modalAddApprover").modal("hide");
        }
       
    },

    //@ver 1.0 rherejias 1/18/2017 used by inactive approver when re-activating records ajax call
    activeApprover: function (dataType, index) {
        $.ajax({
            url: '/Approver/ActiveApprover',
            dataType: 'json',
            type: 'post',
            data: {
                code: index.code_string,
                id: index.id_number
            },
            success: function (response) {
                $("#modalDeactivate").modal("hide");
                if (response.success) {
                    toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Approver re-activated successfully.";
                     Save_ToastNotifMaintenance();
                     if (dataType == 'department') {
                         $('#ApproverGridInactive').jqxGrid('updatebounddata');
                         $('#ApproverGrid').jqxGrid('updatebounddata');
                     }
                     else if (dataType == 'it') {
                         $('#ITRelatedGridInactive').jqxGrid('updatebounddata');
                         $('#ITRelatedGrid').jqxGrid('updatebounddata');
                     }
                     else if (dataType == 'purchasing') {
                         $('#PurchasingGridInactive').jqxGrid('updatebounddata');
                         $('#PurchasingGrid').jqxGrid('updatebounddata');
                     }
                     else if (dataType == 'accounting') {
                         $('#AccountingGridInactive').jqxGrid('updatebounddata');
                         $('#AccountingGrid').jqxGrid('updatebounddata');
                     }
                }
                else {
                    toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " " +response.message;
                    Fail_ToastNotif();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },

    inactiveApprover: function (dataType, index) {
        $.ajax({
            url: '/Approver/inactiveApprover',
            dataType: 'json',
            type: 'post',
            data: {
                approverType: index.approval_type_code_string,
                code: index.code_string,
                id: index.id_number
            },
            success: function (response) {
                $("#modalDeactivate").modal("hide");
                if (response.success) {
                    toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Approver deactivated successfully.";
                    Save_ToastNotifMaintenance();
                    if (dataType == 'department') {
                        $('#ApproverGridInactive').jqxGrid('updatebounddata');
                        $('#ApproverGrid').jqxGrid('updatebounddata');
                    }
                    else if (dataType == 'it') {
                        $('#ITRelatedGridInactive').jqxGrid('updatebounddata');
                        $('#ITRelatedGrid').jqxGrid('updatebounddata');
                    }
                    else if (dataType == 'purchasing') {
                        $('#PurchasingGridInactive').jqxGrid('updatebounddata');
                        $('#PurchasingGrid').jqxGrid('updatebounddata');
                    }
                    else if (dataType == 'accounting') {
                        $('#AccountingGridInactive').jqxGrid('updatebounddata');
                        $('#AccountingGrid').jqxGrid('updatebounddata');
                    }
                }
                else {
                    toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " " +response.message;
                    Fail_ToastNotif();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },

    assignApprover: function (dataType, index) {
        var url = '';
            url = '/Approver/AssignApprover'
            $.ajax({
                url: url,
                dataType: 'json',
                type: 'get',
                data: {
                    id: index.id_number,
                    module: 'APPROVER',
                    code: index.code_string,
                    type: index.approval_type_code_string,
                    department: index.department_code_string
                },
                beforeSend: function () {

                },
                success: function (response) {
                    $("#modalAssignApprover").modal("hide");
                    if (response.success) {
                        toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Approver assigning successfully.";
                        Save_ToastNotifMaintenance();
                        if (dataType == 'department')
                            $('#ApproverGrid').jqxGrid('updatebounddata');
                        else if (dataType == 'it')
                            $('#ITRelatedGrid').jqxGrid('updatebounddata');
                        else if (dataType == 'purchasing')
                            $('#PurchasingGrid').jqxGrid('updatebounddata');
                        else if (dataType == 'accounting')
                            $('#AccountingGrid').jqxGrid('updatebounddata');

                    } else {
                        toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " " +response.message;
                        Fail_ToastNotif();
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status);
                    console.log(thrownError);
                }
            });
    },

};
//end of dbase operation

