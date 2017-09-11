var index;
var code = '';
$(document).ready(function () {
    $(document).delegate("#ApproverTypeGrid_searchField", "keyup", function (e) {
        var columns = ["code_string", "name_string", "description_string"];
        generalSearch($('#ApproverTypeGrid_searchField').val(), 'ApproverTypeGrid', columns, e);
    });
});

$(document).delegate("#btnSaveApproverType", "click", function () {
    dbase_operation.addApproverType();
});

$(document).delegate("#btnSaveApproverTypeEdit", "click", function () {
    dbase_operation.EditApproverType();
});

$(document).delegate(".ApproverTypeGrid_Inactive", "click", function () {
    showModal.deactivate();
});

$(document).delegate("#btnProceedDeactivate", "click", function () {
    dbase_operation.DeactivateApproverType();
});


$("#ApproverTypeGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#ApproverTypeGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#ApproverTypeGrid").jqxGrid('getrowdatabyid', rowID);
    index = data;
});

$("#cmdAddNew").click(function () {
    showModal.addApproverType("add", index);
});

$(document).delegate(".ApproverTypeGrid_Edit", "click", function () {
    showModal.addApproverType("edit", index);
});

var showModal = {
    addApproverType: function (operation, index) {

        var name = '';
        var description = '';
       

        if (operation == "edit") {
            name = index.name_string;
            description = index.description_string;
            code = index.code_string;
        }

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

        modal += '<div class="modal fade" id="modalAddApproverType" role="dialog" >';
        modal += '<div class="modal-dialog">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal += '<h4 class="modal-title">Add Approver Type</h4>';
        modal += '</div>';
        modal += '<div class="modal-body">';

        modal += '<br/>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '              <input id="approverTypeName" type="text" class="textstyle form-control" placeholder="*Approver Type"class="form-control companyrequired" style="width:98%;" value="' + ((operation == 'edit') ? name : '') + '"/>';
        modal += '          </div>'
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '               <input id="approverTypeDescription" type="text" class="textstyle form-control" placeholder="*Description"class="form-control companyrequired" style="width:98%;" value="' + ((operation == 'edit') ? description : '') + '"/>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<button class="btn btn-success" id="' + ((operation == 'edit') ? 'btnSaveApproverTypeEdit' : 'btnSaveApproverType') + '"';
        modal += 'style="width: 100px;">';
        modal += 'SAVE</button>';
        modal += '<button type="button" style="width: 100px;" class="btn btn-default" data-dismiss="modal">CANCEL</button> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';



        $("#form_modal").html(modal);
        $("#modalAddApproverType").modal("show");
        $("#modalAddApproverType").css('z-index', '1000000');

    },

    deactivate: function () {
        var modal = '<div class="modal fade" id="modalDeactivate" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#F25656; color:#ffffff">';
        modal += '<h4 class="modal-title">Inactive Record</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to deactivate this Approval Type?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<button class="btn btn-danger" id="btnProceedDeactivate">';
        modal += 'YES</button>';
        modal += '<button type="button" class="btn btn-default" data-dismiss="modal">NO</button> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalDeactivate").modal("show");
        $("#modalDeactivate").css('z-index', '1000000');
    },
}


dbase_operation = {

    addApproverType: function () {
        var elements = ["#approverTypeName", "#approverTypeDescription"]
        var ctr = 0;
        for (var i = 0; i < 2; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "red");
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5");
                ctr++;
            }

            if (ctr == 2) {
                $.ajax({
                    url: '/ApproverType/addApproverType',
                    dataType: 'json',
                    type: 'get',
                    data: {
                        approvalType: $("#approverTypeName").val(),
                        approvalDescription: $("#approverTypeDescription").val(),
                    },
                    success: function (response) {
                        if (response.success) {
                            $("#modalAddApproverType").modal("hide");
                            notification_modal("Add Record", "New Approver Type has been added.", "success");
                            $('#ApproverTypeGrid').jqxGrid('updatebounddata');
                        }
                        else {
                            notification_modal("Error", "Error in adding Approver Type", "danger");
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

    EditApproverType: function () {
        var elements = ["#approverTypeName", "#approverTypeDescription"]
        var ctr = 0;
        for (var i = 0; i < 2; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "red");
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5");
                ctr++;
            }

            if (ctr == 2) {
                $.ajax({
                    url: '/ApproverType/EditApproverType',
                    dataType: 'json',
                    type: 'get',
                    data: {
                        approvalType: $("#approverTypeName").val(),
                        approvalDescription: $("#approverTypeDescription").val(),
                        code: code
                    },
                    success: function (response) {
                        if (response.success) {
                            $("#modalAddApproverType").modal("hide");
                            notification_modal("Add Record", "Approver Type has been updated.", "success");
                            $('#ApproverTypeGrid').jqxGrid('updatebounddata');
                        }
                        else {
                            notification_modal("Error", "Error in updating Approver Type", "danger");
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

    DeactivateApproverType: function () {
                $.ajax({
                    url: '/ApproverType/InactiveApproverType',
                    dataType: 'json',
                    type: 'get',
                    data: {
                        code: index.code_string,
                        id: index.id_string
                    },
                    success: function (response) {
                        if (response.success) {
                            $("#modalDeactivate").modal("hide");
                            notification_modal("Add Record", "Approver Type has been deactivated.", "success");
                            $('#ApproverTypeGrid').jqxGrid('updatebounddata');
                        }
                        else {
                            notification_modal("Error", "Error in deactivating Approver Type", "danger");
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.status);
                        console.log(thrownError);
                    }
                });
         }
};

