var override = '';


$(document).ready(function () {
    $(document).delegate("#OverrideGrid_searchField", "keyup", function (e) {
        if ($("#OverrideGrid_searchField").val().length == 11) {
            var columns = ["trans_header_code_string"];
            generalSearch($('#OverrideGrid_searchField').val(), 'OverrideGrid', columns, e);
        }
        else if ($("#OverrideGrid_searchField").val().length == 10) {
            $('#OverrideGrid').jqxGrid('clearfilters');
        }
        else if ($("#OverrideGrid_searchField").val().length == 12) {
            $('#OverrideGrid').jqxGrid('clearfilters');
        }
        });
});



$(document).delegate(".OverrideGrid_Override", "click", function () {

    showModal.reamrks(override.approval_type_code_string);
});

$(document).delegate("#btnProceedRemarks", "click", function () {
    dbase_operationOverride.override($(this).attr("data-approvalType"));
});

$(document).delegate("#OverrideGrid_Reject", "click", function () {
    if (override.trans_header_code_string != null) {
        showModal.reject(override.trans_header_code_string);
    }
    else {
        toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " " + "Please select a gatepass record.";
        Fail_ToastNotif();
    }
});

$(document).delegate("#btnProceedReject", "click", function () {
    dbase_operationOverride.reject($(this).attr("data-code"));
});

$("#OverrideGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#OverrideGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#OverrideGrid").jqxGrid('getrowdatabyid', rowID);
    override = data;
});

showModal = {
    reamrks: function (dataType) {
        var modal = '<div class="modal fade" id="modalRemarks" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#1DB198; color:#ffffff">';
        modal += '<h4 class="modal-title">Remarks</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:6%">';
        modal += '<textarea id="remarksTextarea" rows="5" cols="40" placeholder="Insert comment..." style="border-color:'+ '#e5e5e5'+'"></textarea>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a data-approvalType=' + dataType + ' class="btn btn-small btn-green" id="btnProceedRemarks">';
        modal += 'Submit</a>';
        modal += '<a type="button" class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a> &nbsp&nbsp&nbsp&nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalRemarks").modal("show");
        $("#modalRemarks").css('z-index', '1000000');
    },

    reject: function (headercode) {
        var modal = '<div class="modal fade" id="modalReject" role="dialog">';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#F25656; color:#ffffff">';
        modal += '<h4 class="modal-title">Reject</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to reject this gatepass? <b>Gatepass ID : ' + headercode + ' </b></p>';
        modal += '<textarea id="remarksTextarea" rows="5" cols="40" placeholder="Insert comment..." style="border-color:' + '#e5e5e5' + '"></textarea>';
        modal += '<p class="text-danger">Note : This action is irreversable</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a data-code=' + headercode + ' class="btn btn-small btn-red" id="btnProceedReject">';
        modal += 'Submit</a>';
        modal += '<a type="button" class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a> &nbsp&nbsp&nbsp&nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalReject").modal("show");
        $("#modalReject").css('z-index', '1000000');
    }
}

dbase_operationOverride = {
    override: function (ApprovalType) {
        var elements = ["#remarksTextarea"];
        var ctr = 0;
        for (var i = 0; i < 1; i++) {
            if ($(elements[i]).val() == ""){
                $(elements[i]).css("border-color", "red")
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5")
                ctr++;
            }
        }
        if (ctr == 1) {
            $.ajax({
                url: '/Override/Override',
                dataType: 'json',
                Type: 'POST',
                data: {
                    id: override.id_number,
                    code: override.code_string,
                    headercodeAdd: override.trans_header_code_string,
                    approvalType: override.approval_type_code_string,
                    remarks: $("#remarksTextarea").val()
                },
                success: function () {
                    $("#modalRemarks").modal("hide");
                    toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Overriding successful.";
                    Save_ToastNotifMaintenance();
                    $("#OverrideGrid").jqxGrid('updatebounddata');
                },
                error: function () {
                    $("#modalRemarks").modal("hide");
                    toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " Overriding failed.";
                    Fail_ToastNotif();
                }
            })

            $.ajax({
                url: '/Override/EmailContent',
                dataType: 'json',
                Type: 'get',
                data: {
                    distinguisher: 'approve',
                    headercode: override.trans_header_code_string,
                    approvalType: override.approval_type_code_string
                }
            })

            var urlEmail = "";
            if (ApprovalType == departmentApprover) {
                urlEmail = '/ForApproval/ApprovedByDepartment';
            } else if (ApprovalType == itApprover) {
                urlEmail = '/ForApproval/ApprovedByITRelatedApprover';
            } else if (ApprovalType == purchasingApprover) {
                urlEmail = '/ForApproval/ApprovedByPurchasingRelatedApprover';
            }
            
            if (ApprovalType != accountingApprover) {
                $.ajax({
                url: urlEmail,
                dataType: 'json',
                type: 'get',
                data: {
                    headercode: override.trans_header_code_string,
                    identifier: 'override'
                },
                beforeSend: function () {

                },
                headers: {
                },
                success: function (response) {

                    if (response.success) {
                        $("#cmdForApprovalBack").click();
                        toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Gate Pass Approved!";
                        $('#gridForApproval').jqxGrid('updatebounddata');
                        Save_ToastNotif();
                    } else {
                        toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + response.message;
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
    },

    reject: function (headercode) {
        var elements = ["#remarksTextarea"];
        var ctr = 0;
        for (var i = 0; i < 1; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "red")
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5")
                ctr++;
            }
        }

        if (ctr == 1) {
            $.ajax({
                url: '/Override/Reject',
                dataType: 'json',
                Type: 'POST',
                data: {
                    id: override.id_number,
                    headercode: headercode,
                    remarks: $("#remarksTextarea").val()
                },
                success: function () {
                    $("#modalReject").modal("hide");
                    toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Reject successful.";
                    Save_ToastNotifMaintenance();
                    $("#OverrideGrid").jqxGrid('updatebounddata');
                },
                error: function () {
                    $("#modalReject").modal("hide");
                    toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + "rejection failed.";
                    Fail_ToastNotif();
                }
            })

            $.ajax({
                url: '/Override/EmailContent',
                dataType: 'json',
                Type: 'get',
                data: {
                    distinguisher: 'reject',
                    headercode: override.trans_header_code_string,
                    approvalType: override.approval_type_code_string
                }
            })
        }
    }
}


