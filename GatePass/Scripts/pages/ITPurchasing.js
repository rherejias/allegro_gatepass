$(document).ready(function () {
    $(document).delegate("#ITApproverGrid_searchField", "keyup", function (e) {
        var columns = ["approval_type_string", "department_string", "user_string", "email_string"];
        generalSearch($('#ITApproverGrid_searchField').val(), 'ITApproverGrid', columns, e);
    });

    $(document).delegate("#PurchasingApproverGrid_searchField", "keyup", function (e) {
        var columns = ["approval_type_string", "department_string", "user_string", "email_string"];
        generalSearch($('#PurchasingApproverGrid_searchField').val(), 'PurchasingApproverGrid', columns, e);
    });
});

$("#ITApproverGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#ITApproverGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#ITApproverGrid").jqxGrid('getrowdatabyid', rowID);
    indexIT = data;
});

$("#PurchasingApproverGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#PurchasingApproverGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#PurchasingApproverGrid").jqxGrid('getrowdatabyid', rowID);
    indexPurchasing = data;
});

$(document).delegate(".ITApproverGrid_Assign", "click", function () {
    if (indexIT.approval_type_code_string == itApprover) {
        toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " " + "<b>" + indexIT.user_string + "</b> is the current approver.";
        Fail_ToastNotif();
    }
    else {
        showModal.assignApproverModal('it', indexIT.user_string);
    }

});

$(document).delegate(".PurchasingApproverGrid_Assign", "click", function () {
    if (indexPurchasing.approval_type_code_string == purchasingApprover) {
        toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " " + "<b>" + indexPurchasing.user_string + "</b> is the current approver.";
        Fail_ToastNotif();
    }
    else {
        showModal.assignApproverModal('purchasing', indexPurchasing.user_string);
    }
});

$(document).delegate("#btnProceedAssignApprover", "click", function () {
    if ($(this).attr('data-type') == 'it')
        indexAssign = indexIT;
    else if ($(this).attr('data-type') == 'purchasing')
        indexAssign = indexPurchasing;

    dbase_operation.assignApprover($(this).attr('data-type'), indexAssign);
});


var showModal = {
    assignApproverModal: function (dataType, name) {
        var modal = '<div class="modal fade" id="modalAssignApprover" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#1DB198; color:#ffffff">';
        modal += '<h4 class="modal-title">'+((dataType == 'it') ? 'IT Approver':'Purchasing Approver')+'</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to assign <b>' + name + '</b> as primary approver?</p>';
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

dbase_operation = {
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
                    if (dataType == "it") {
                        $('#ITApproverGrid').jqxGrid('updatebounddata');
                    }
                    else {
                        $('#PurchasingApproverGrid').jqxGrid('updatebounddata');
                    }
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
    }
}

