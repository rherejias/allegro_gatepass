
var indexItemDetails;
var draftId, GPTransId_Global;
var datatable, datatableContact, indexAllTrans;
var itemreturn, headerdetails_for_RS;
var itemforreturn;
var arrayitemforreturn = new Array;
var selectedrowindexes = new Array;
var imagesrc, returnslip_usercode;
var fileName;
var originalFileName;


//$(document).delegate("#file", "click", function () {
//    dbase_operation.download($(this).attr('data-file'), $(this).attr('data-path'));
//});

function DownloadAttachment() {
    if (originalFileName == "" || originalFileName == null) {
        $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'Uploaded file is empty.');
        Warning_ToastNotif();
        $(".warning").css('z-index', '1000000000000000');
    }
    else {
    window.location.href = '/Attachment/DownloadAttachment?filename=' + originalFileName + '&path=' + env;
    }
}

$(document).delegate("#grid_item_returnslip_searchField", "keyup", function (e) {
    if (e.which == 13) {
        var columns = ["serial_number_string", "tag_number_string", "p_o_number_string"];
        generalSearch($('#grid_item_returnslip_searchField').val(), "grid_item_returnslip", columns, e);

    } else {
        var columns1 = ["serial_number_string", "tag_number_string", "p_o_number_string"];
        generalSearch($('#grid_item_returnslip_searchField').val(), "grid_item_returnslip", columns1, e);
    }

});

$(document).delegate("#grid_returnslip_searchField", "keyup", function (e) {
    if (e.which == 13) {
        var columns = ["gate_pass_id_string", "purpose_string", "impex_ref_number_string", "supplier_name_string", "return_date_date", "supplier_string", "return_slip_status_string"];
        generalSearch($('#grid_returnslip_searchField').val(), "grid_returnslip", columns, e);

    } else {
        var columns1 = ["gate_pass_id_string", "purpose_string", "impex_ref_number_string", "supplier_name_string", "return_date_date", "supplier_string", "return_slip_status_string"];
        generalSearch($('#grid_returnslip_searchField').val(), "grid_returnslip", columns1, e);
    }

});

$(document).delegate("#gridAllTransGuard_searchField", "keyup", function (e) {
    if (e.which == 13) {
        var columns = ["gate_pass_id_string", "purpose_string", "impex_ref_number_string", "supplier_name_string", "return_date_date", "supplier_string"];
        generalSearch($('#gridAllTransGuard_searchField').val(), "gridAllTransGuard", columns, e);

    } else {
        var columns1 = ["gate_pass_id_string", "purpose_string", "impex_ref_number_string", "supplier_name_string", "return_date_date", "supplier_string"];
        generalSearch($('#gridAllTransGuard_searchField').val(), "gridAllTransGuard", columns1, e);
    }
   
});

$(document).delegate("#grid_item_forGuardApprove_searchField", "keyup", function (e) {
   

    if (e.which == 13) {
        var columns = [ "serial_number_string", "tag_number_string", "p_o_number_string"];
        generalSearch($('#grid_item_forGuardApprove_searchField').val(), "grid_item_forGuardApprove", columns, e);

    } else {
        var columns1 = ["serial_number_string","tag_number_string", "p_o_number_string"];
        generalSearch($('#grid_item_forGuardApprove_searchField').val(), "grid_item_forGuardApprove", columns1, e);
    }

});


function clear() {
    $('#txtPurpose').val("");
    $('#txtImpexRefNbr').val("");
    $('#cmbTransType').val("IN");
    $('#txtReturnDate').val("");
};

$("#removeAttachment").click(function () {
    $("#currentFile").text("No File Attached");
});

$("#headerAttachment").change(function () {
    if ($("#headerAttachment").val() == "") {
        $("#currentFile").text(fileName);
    }
    else {
        var currentFileName = $("#headerAttachment").val().split('\\').pop();
        $("#currentFile").text(currentFileName);
    }

});


$(document).delegate(".images", "click", function () {
    show_modal.imageView($(this).attr("src"));
});



$("#gridAllTrans").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#gridAllTrans").jqxGrid('getrowid', boundIndex);
    var data = $("#gridAllTrans").jqxGrid('getrowdatabyid', rowID);

    indexAllTrans = boundIndex;
});
// end function //

// AddItem //
$(document).delegate("#cmdSaveItem", 'click', function () {
    var ext = $("#inpt_file").val().split('.').pop();
    if (ext == "png" || ext == "jpg" || ext == "PNG" || ext == "JPG" || ext == "jpeg" || ext == "JPEG" || $("#inpt_file").val() == "") {
        dbase_operation_Item.addAddItems($(this).attr("data-transid"));
    }
    else {

        $("#form_modal_div").css('z-index', '10000');
        notification_modal("Alert", "File extension not supported.", "danger");
        $('#btn_notifClose').click(function () {
            $('#notification_modal .close').click();
            $("#form_modal_div").css('z-index', '1000000000000000');
        });
    }
});


var dbase_operation_Item = {
    // updatet the partial return item by quantity
    partialreturnByQuantity: function (id, headercode, itemcodeArr, partialquantity_returnArr, itemGUIDArr) {

            $.ajax({
            url: '/Guard/PartialReturnQuantity',
            dataType: 'json',
            type: 'get',
            data: {
                Id:id,
                HeaderCode: headercode,
                ItemCode: itemcodeArr.join(),
                PartialQuantity: partialquantity_returnArr.join(),
                PartialReturnComment: $('#txtComment').val(),
                ItemGUID_Arr: itemGUIDArr.join()
                
            },
            beforeSend: function () {

            },
            headers: {
            },
            success: function (response) {

                if (response.success) {
                    $("#form_modal_returnslip_details").modal("hide");
                    $("#grid_returnslip").jqxGrid('updatebounddata');

                    toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Item was successfully returned!";
                    Save_ToastNotif();


                } else {
                    notification_modal("Addition failed!", response.message, "danger");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });


    },

    // end function


    UpdateReturnedItem: function (itemidArray, headercode_details, returnslip_usercode) {

        $.ajax({

            url: '/Transactions/UpdateItemReturned',
            dataType: 'json',
            type: 'get',
            data: {
                itemId: itemidArray.join(),
                headercode: headercode_details,
                returnslipUsercode: returnslip_usercode


            },
            beforeSend: function () {

            },
            headers: {
            },
            success: function (response) {

                if (response.success) {
                    $("#form_modal_returnslip_details").modal("hide");

                    $("#grid_returnslip").jqxGrid('updatebounddata');
                } else {
                    notification_modal("Addition failed!", response.message, "danger");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });


    },


    // to set the returnslip status as 'Not Return' and generate return slip of gatepass
    GuardApprovedGatePass: function (gatepassid,gpassIdNumber) {
        $.ajax({
            url: '/Guard/GuardApproved',
            dataType: 'json',
            type: 'post',
            data: {
                Id: gatepassid,
                returndate: $('#txtReturnDate').val(),
                IdNumber: gpassIdNumber,
                
            },
            beforeSend: function () {
            },
            headers: {
            },
            success: function (response) {

                if (response.success) {
                    $("#form_modal_div1").modal("hide");
                    toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Successfully apprroved by AMPI Guard!";
                    Save_ToastNotif();
                    $('#gridAllTransGuard').jqxGrid('updatebounddata');
                    $('#grid_returnslip').jqxGrid('updatebounddata');
                } else {
                    notification_modal("An Error Occured!", response.message, "danger");
                }
               
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },

    GuardRejectGatePass: function (gatepassid, gpassIdNumber) {

    $.ajax({
        url: '/Guard/GuardRejected',
        dataType: 'json',
        type: 'post',
        data: {
            Id: gatepassid,
            IdNumber: gpassIdNumber,
            Remarks: $('#txtCommentGatePass').val()

        },
        beforeSend: function () {

        },
        headers: {

        },
        success: function (response) {

            $("#form_modal_div1").modal("hide");
            toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Gate Pass Rejected!";
            Save_ToastNotif();
            $('#gridAllTransGuard').jqxGrid('updatebounddata');
            $('#grid_returnslip').jqxGrid('updatebounddata');
                

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(thrownError);
        }
    });
   }

};


$(document).delegate("#cmdProceedDrafted", 'click', function () {
    switch ($(this).attr("data-procedure")) {
        case "addHeader_draft":
            dbase_operation_draft.addHeaderdraft();
            break;
        default:
            console.log('procedure not found');
    }
});

var dbase_operation_draft = {
    addHeaderdraft: function () {
        $.ajax({
            url: '/Transactions/AddHeader',
            dataType: 'json',
            type: 'post',
            data: {
                ImpexRefNbr: $("#txtImpexRefNbr").val(),
                DepartmentCode: '',
                ReturnDate: $("#txtReturnDate").val(),
                TransType: $("#cmbTransType").val(),
                CategoryCode: '',
                TypeCode: '',
                Purpose: $("#txtPurpose").val(),
                IsActive: true,
                Status: 'Drafted',
            },
            beforeSend: function () {

            },
            headers: {

            },
            success: function (response) {
                if (response.success) {
                    notification_modal("Add Record", response.message, "success");

                    $('#btn_notifClose').click(function () {
                        window.location.reload();

                    });


                } else {
                    notification_modal("Add Record", response.message, "danger");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },
    addDetails: function () {
        console.log('add details here');
    },

    // Inactive_ItemDraft and CreateNewItem //
    deactivate: function (Itemid) {

        var transaction;


        if (draftId == null) {

            transaction = 'CreateNew_AddItem';
        } else {
            transaction = 'Edit_AddItem';

        }

        $.ajax({
            url: '/Transactions/DeactivateAddItemDraft',
            dataType: 'json',
            type: 'get',
            data: {
                id: Itemid,
                isactive: false,
                Action: transaction
            },
            beforeSend: function () {

            },
            success: function (response) {
                if (response.success) {
                    $("#modalDeactivate").modal("hide");
                    $('#gridDetails').jqxGrid('updatebounddata');
                    notification_modal("Inactive Record", msg, "success");


                } else {
                    notification_modal("Deactivation Failed!", response.message, "danger");
                }
            },

            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },

    deactivate_transDraft: function (Itemid) {

        $.ajax({
            url: "/Transactions/DeactivateTransDraft",
            dataType: 'json',
            type: 'get',
            data: {
                id: Itemid,
                isactive: false

            },
            beforeSend: function () {

            },
            success: function (response) {
                if (response.success) {
                    $("#modalDeactivate").modal("hide");
                    $('#gridAllTrans').jqxGrid('updatebounddata');
                    notification_modal("Inactive Record", msg, "success");


                } else {
                    notification_modal("Deactivation Failed!", response.message, "danger");
                }
            },

            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    }
    // End Function //
};


$(document).delegate("#cmdProceed", 'click', function () {
 
    switch ($(this).attr("data-procedure")) {
        case "addHeader":
            dbase_operation.addHeader();
            break;
        default:
            console.log('procedure not found');
    }
});

var dbase_operation = {
    addHeader: function (transactionId, stat) {
        var formData = new FormData();
        formData.append('inpt_file', $('input[name="headerAttachment"]')[0].files[0]);
        formData.append('Id', transactionId);
        formData.append('ImpexRefNbr', $("#txtImpexRefNbr").val());
        formData.append('DepartmentCode', '');
        formData.append('ReturnDate', $("#txtReturnDate").val());
        formData.append('TransType', $("#cmbTransType").val());
        formData.append('CategoryCode', '');
        formData.append('TypeCode', '');
        formData.append('Purpose', $("#txtPurpose").val());
        formData.append('IsActive', true);
        formData.append('Status', stat);
        formData.append('Attachment', $("#headerAttachment").val());
        var url = "";
        if (transactionId != "") {
            url = "/Transactions/UpdateHeader";
        } else {
            url = "/Transactions/AddHeader";
        }
        $.ajax({
            url: url,
            dataType: 'json',
            type: 'post',
            data: formData,
            processData: false,
            contentType: false,

            beforeSend: function () {

            },
            headers: {
           
            },
            success: function (response) {
                if (response.success) {

                    notification_modal("Add Record", response.message, "success");
                    $('#btn_notifClose').click(function () {
                      
                        window.location.reload();
                    });


                } else {
                    notification_modal("Add Record", response.message, "danger");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },
    addDetails: function () {
        console.log('add details here');
    },

    download: function (file, path) {
        $.ajax({
            url: '/download/download',
            dataType: 'json',
            type: 'get',
            data: {
                filename: file,
                filepath: path,
                uname: username
            },
            success: function (response) {
                if (response.success) {
                    $("#saveAsdraftAndsubmit").css('z-index', '1000000000');
                    toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Download successful.";
                    Save_ToastNotif();
                }
                else {
                    $("#toastMaintenanceFail").css('z-index', '1000000000');
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
};

// close button of confirmation modal
$(document).delegate('#RS_confirm_modal', 'click', function () {
   $("#form_modal_returnslip_details").css('z-index', '1000000');
});

var show_modal = {
    
    // confimation modal of return slip //
    confirmationModal_returnItem: function (title, message, status) {      
        var modal = '<div class="modal fade" id="conf_modal_returnitem" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-backdrop="static">';
        modal += '<div class="modal-dialog modal-sm">';
        modal += '<div class="modal-content">';
        modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title" id="myModalLabel"><i class="fa fa-question-circle fa-lg" aria-hidden="true"></i>&nbsp;' + title + '</h4>';
        modal += '</div>';
        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += message;
        modal += '</div>';
        modal += '<div class="modal-footer">';
         modal += '<a class="btn btn-small btn-blue" data-dismiss="modal" id="cmdReturnItem">YES</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal" id="RS_confirm_modal">NO</a>';
       
        modal += '</div>';


        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        $("#confirmation_modal").html(modal);
        $("#conf_modal_returnitem").modal("show");
        $("#conf_modal_returnitem").css('z-index', '1000000');
    },
    // end //

    //rherejias modal for image view
    imageView: function (source) {
        var title = source.split('_').pop();
        var modal = '<div class="modal fade" id="modalImageView" role="dialog" >';
        modal += '<div class="modal-dialog modal-lg">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#08A7C3; color:#ffffff">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h3 class="modal-title">' + title + '</h3>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:3%">';
        modal += '<img height="auto" width="100%" src="' + source + '"/>';
        modal += '</div>';

        $("#image_modal").html(modal);
        $("#modalImageView").modal("show");
        $("#modalImageView").css('z-index', '1000000');
    },

    // confirmation modal //
    confirmationModal_GatePass: function (title, message, status) {
       

        var modal = '<div class="modal fade" id="conf_modal_div" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-backdrop="static">';
        modal += '<div class="modal-dialog modal-sm">';
        modal += '<div class="modal-content">';
        modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title" id="myModalLabel"><i class="fa fa-question-circle fa-lg" aria-hidden="true"></i>&nbsp;' + title + '</h4>';
        modal += '</div>';
        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += message;
        modal += '</div>';
        modal += '<div class="modal-footer">';
        modal += '<a class="btn btn-small btn-blue" data-dismiss="modal" id="confirmModalGPConfirm">YES</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal" id="confirmModalGPClose">NO</a>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#confirmation_modal").html(modal);
        $("#conf_modal_div").modal("show");
        $("#conf_modal_div").css('z-index', '1000000');
    },
    // End function //

    rejectComment_guard: function () {
       
        var modal = '<div class="modal fade" id="rejectModal_guard" role="dialog" >';
        modal += '<div class="modal-dialog">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#08A7C3; color:#ffffff">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h3 class="modal-title">Comment</h3>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:3%">';
        modal += '<textarea type="text" style="resize:none;border-color:#f98989" rows="3" cols="50" class="form-control" id="txtrejectComment" placeholder="Please enter a comment.." maxlength="255"></textarea>';
        modal += '<p style="color:#f98989" id="lblrejectcommentrequired" hidden>This field is required with the maximum of 255 characters.</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<a class="btn btn-meduim btn-blue" id="btnSubmitGuard_CommentModal">Submit</a>';
        modal += '<a class="btn btn-meduim btn-gray" id="btnRejectGuard_CommentModal" data-dismiss="modal">Cancel</a>';
        modal += '</div>';

        $("#image_modal").html(modal);
        $("#rejectModal_guard").modal("show");
        $("#rejectModal_guard").css('z-index', '1000000');
    },

    confirmationGPReject: function (gatepassId) {
      

        var modal = '<div class="modal fade" id="confirmRejectModal" role="dialog" data-backdrop="static">';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#F25656; color:#ffffff">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title"><i class="fa fa-question-circle fa-lg" aria-hidden="true"></i>&nbsp;Confirmation</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:3%">';
        modal += '<p>Are you sure you want to reject this Gate Pass? </p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<a class="btn btn-small btn-red" data-dismiss="modal" id="btnModalConfirmRejectSubmit">YES</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal" id="btnModalConfirmRejectCancel">NO</a>';
        modal += '</div>';

        $("#confirmation_modal").html(modal);
        $("#confirmRejectModal").modal("show");
        $("#confirmRejectModal").css('z-index', '1000000');
    },


    // confirmation modal for draft //
    confirmationModal_update: function (title, message, status) {


        var modal = '<div class="modal fade" id="conf_modal_div2" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
        modal += '<div class="modal-dialog modal-sm">';
        modal += '<div class="modal-content">';
        modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title" id="myModalLabel">' + title + '</h4>';
        modal += '</div>';
        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += message;
        modal += '</div>';
        modal += '<div class="modal-footer">';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a>';
        modal += '<a class="btn btn-small btn-blue" data-dismiss="modal" id="cmdUpdateDraft">Confirm</a>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#confirmation_modal").html(modal);
        $("#conf_modal_div2").modal("show");
        $("#conf_modal_div2").css('z-index', '1000000');
    },

 
    // Show details modal aaaa//
    showmoredetails: function (idnumber, returnslip_code, dateadded, purpose, impexnumber, transtype, supplier, origFileName) {


        var modal = '<div class="modal fade" id="form_modal_div1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
        modal += '<div class="modal-dialog modal-lg">';
        modal += '<div class="modal-content">';

        modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
        modal += '<a class="close" data-dismiss="modal" aria-label="Close" style="color:red"><span aria-hidden="true" style="color:red">&times;</span></a>';
        modal += '<h4 class="modal-title" id="myModalLabel">Details</h4>';
        modal += '</div>';

        modal += '<div class="modal-body">';
        modal += '<br />';


        modal += '<div class="row">';

        modal += '<div class="col-md-12">';

        modal += '<div class="row">';
        modal += '<div class="form-group col-md-3">';
        modal += '<label for="txtGatePassID" style="font-size:12px; font-family: segoe ui">Gate Pass Id:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtGatePassID" style="background-color:transparent;font-size:12px; font-family: segoe ui" value="' + returnslip_code + '" readonly="readonly">';
        modal += ' </div>';

        modal += '<div class="form-group col-md-3">';
        modal += '<label for="txtImpexRefNbr">IMPEX Ref. Number*:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtImpexRefNbr" placeholder="IMPEX Ref. Number" style="background-color:transparent" value="' + impexnumber + '" readonly>';
        modal += '</div>';

        modal += '<div class="form-group col-md-6">';
        modal += '<label for="txtImpexRefNbr">Supplier:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtImpexRefNbr" placeholder="IMPEX Ref. Number" style="background-color:transparent" value="' + supplier + '" readonly>';
        modal += '</div>';
        modal += '</div>';

        modal += '<div class="row">';

        modal += ' <div class="col-md-3">';
        modal += '<label for="cmbTransType">Transaction Type*:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtGatePassID" value= "' + transtype + '" style="background-color:transparent" readonly="readonly">';
        modal += '</div>';

        modal += '<div class="col-md-3">';
        modal += '<label for="txtReturnDate">Return Date*:</label>';
        modal += '<div class="input-group input-append date form_datetime">';
        modal += '<input id="txtReturnDate" type="text" class="form-control input-sm" style="background-color:transparent" value= "' + dateadded + '" readonly/>';
        modal += '<span class="input-group-addon add-on">';
        modal += '<i class="fa fa-calendar faa-tada animated-hover"';
        modal += 'style="color: #31B0D5">';
        modal += '</i>';
        modal += '</span>';
        modal += '</div>';
        modal += '</div>';

        modal += '<div class="col-md-6">';
        modal += '<label for="txtPurpose">Purpose*:</label>';
        // modal += '<input type="text" class="form-control input-sm" id="txtPurpose" value= "' + purpose + '" style="background-color:transparent" readonly="readonly">';
        modal += '<textarea style="resize:none; background-color:transparent; font-size:12px; font-family: segoe ui" rows="2" cols="50" class="form-control input-sm" id="txtPurpose" readonly>' + purpose + '</textarea>';
        modal += '</div>';

        modal += '</div>';
  
        modal += '</div>';
       
        modal += '</div>';

        //modal += '<br/>';
        modal += '<div class="row">';

        modal += '<div class="col-md-6">';
        modal += '<label id="lblcurrentFile" for="current">Attached File: </label>';
        modal += '<div class="list-group">';
        modal += '<a class="list-group-item" id="file" onclick="DownloadAttachment()" title="Download">';
        modal += '<label id="currentFile" class="text-success" style="cursor:pointer; font-weight:bold;"></label>';
        modal += '</a>';            
        modal += '</div>';       
        modal += '</div>';

        modal += '<div class="col-md-6" hidden>';
        modal += '<label for="current">Attached File: </label>';
        modal += '<div class="list-group">';
        modal += '<a class="list-group-item" id="file" onclick="DownloadAttachment()" title="Download">';
        modal += '<label class="text-success" style="cursor:pointer; font-weight:bold;"></label>';
        modal += '</a>';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
    

       // modal += '<hr/>';
        modal += '<div id="grid_gate_pass_items"></div>';
        modal += '<div class="row">';
        modal += '<div class="col-md-12">';      
        modal += '</div>';
        modal += '<br/>';
        modal += '<div class="modal-footer">';
        modal += '<div class="row">'

        modal += '<div class="col-md-6">';
        modal += '<textarea class="form-control" rows="3" id="txtCommentGatePass" cols="50" style="font-size:12px; font-family: segoe ui; resize:none; display:none" placeholder="Please enter a comment here..."></textarea>'
        modal += '<p style="color:#f98989; text-align:left" id="lblrejectcommentrequired" hidden>This field is required with the maximum of 255 characters.</p>';
        modal += '</div>';

        modal += '<div class="col-md-6">';
        modal += '<a class="btn btn-meduim btn-green" style="margin-top:4%" id="btnGuardApproved"><i class="fa fa-check" aria-hidden="true"></i> Approve</a>';
        modal += '<a class="btn btn-meduim btn-red" style="margin-top:4%" id="btnGuardRejected"><i class="fa fa-times" aria-hidden="true"></i> Reject</a>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);

        $("#form_modal_div1").modal("show");
        $("#form_modal_div1").css('z-index', '1000000');

        ini_main.element('inputtext');
        ini_main.element('inputnumber');
        ini_main.element('dropdownlist');
        initialize_jqxwidget_grid($("#grid_item_forGuardApprove"));
        custom_grid_ini.gate_pass_items($("#grid_gate_pass_items"), returnslip_code);


        $("#grid_item_forGuardApprove").on('rowclick', function (event) {
            var args = event.args;
            // row's bound index.

            var boundIndex = args.rowindex;
            // row's visible index.
            var visibleIndex = args.visibleindex;
            // right click.
            var rightclick = args.rightclick;
            // original event.
            var ev = args.originalEvent;
            var rowID = $("#grid_item_forGuardApprove").jqxGrid('getrowid', boundIndex);

            itemforreturn = $("#grid_item_forGuardApprove").jqxGrid('getrowdatabyid', rowID);

        });

    },
    // end function //


    // Item-Details Modal Return Slip //
    /*
     * this modal shows when the Item _details on the "Return Slip Transactions" is clicked
     */
    show_Itemdetails_returnslip: function (returnslip_code) {

        var modal = '<div class="modal fade" id="form_modal_returnslip_details" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
        modal += '<div class="modal-dialog modal-lg">';
        modal += '<div class="modal-content">';

        modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title" id="myModalLabel">Item Details</h4>';
        modal += '</div>';

        modal += '<div class="modal-body">';

        modal += '<div class="row">';
        modal += '<br/>';
        modal += '<div class="col-md-12">';
        modal += '<p id="lblGridValidations" hidden style="color:red">Item quantity to be returned should not be greater than to the declared item quantity in the Gate Pass!</p>';
       
        modal += '<div id="grid_return_slip"></div>';
        modal += '<br/>';
        modal += '<br/>';
        modal += '</div>'; 

        modal += '<div class="col-md-6">';
        modal += '<textarea class="form-control" rows="3" id="txtComment" cols="50" style="font-size:12px; font-family: segoe ui; resize:none" placeholder="Please enter a comment here..."></textarea>'
        modal += '</div>';
        modal += '<div class="col-md-6">';
        
        modal += '<a class="btn btn-meduim btn-gray" data-dismiss="modal" style="margin-top:3%;float:right">Cancel</a>';
        modal += '<label style="margin-top:3%;float:right">&nbsp;&nbsp;</label>';
        modal += '<a class="btn btn-meduim btn-blue" id="cmdReturn" style="margin-top:3%; float:right">Return</a>';
        

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);

        $("#form_modal_returnslip_details").modal("show");
        $("#form_modal_returnslip_details").css('z-index', '1000000');

        custom_grid_ini.return_items($("#grid_return_slip"), returnslip_code);

    },
    // End //


};

//angel

$(document).delegate("#btnGuardApproved", "click", function () {
    $('#txtCommentGatePass').css('display', 'none');
    $('#txtCommentGatePass').val('');

    $("#form_modal_div1").css('z-index', '100000');
    show_modal.confirmationModal_GatePass('Confirmation', 'Are you sure that the items being inspected are the exact same items in the approved Gate Pass?', 'Success');
    $('#txtCommentGatePass').css('border-color', '#c3c7c9');
});

$(document).delegate("#btnGuardRejected", "click", function () {
  

    if ($('#txtCommentGatePass').val() == "") {
        $('#txtCommentGatePass').css('display', 'unset');
        $('#txtCommentGatePass').css('border-color', '#f98989');
        $('#lblrejectcommentrequired').show('slow');
        $("#lblrejectcommentrequired").delay(3000).hide('slow');

    } else {

        $("#form_modal_div1").css('z-index', '100000');
        show_modal.confirmationGPReject($('#txtGatePassID').val());
    }

});




$(document).delegate("#btnModalConfirmRejectCancel", "click", function () {

    $("#form_modal_div1").css('z-index', '1000000');

});
$(document).delegate("#confirmModalGPClose", "click", function () {

    $("#form_modal_div1").css('z-index', '1000000');

});
$(document).delegate("#confirmModalGPConfirm", "click", function () {

    dbase_operation_Item.GuardApprovedGatePass(GPTransId_Global,GPId_Global);

});

$(document).delegate("#btnModalConfirmRejectSubmit", "click", function () {

    dbase_operation_Item.GuardRejectGatePass(GPTransId_Global,GPId_Global);

});


$(document).delegate("#cmdTestNo", "click", function () {
    var selectedrowindexes = $('#grid_item_returnslip').jqxGrid('selectedrowindexes');
    var item_id_arr = [];

    for (var ctr = 0; ctr < selectedrowindexes.length; ctr++) {
        var rowID = $("#grid_item_returnslip").jqxGrid('getrowid', selectedrowindexes[ctr]);
        itemforreturn = $("#grid_item_returnslip").jqxGrid('getrowdatabyid', rowID);
        item_id_arr.push(itemforreturn.id_number);

    }

});

// View by status function //
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
// End Function //

// Edit Add item //

$(document).delegate(".gridDetails_Edit", 'click', function () {

    if (draftId == null) {

        show_modal.addNewItem();

    }
    else {
        show_modal.addNewItem();
        $('#myModalLabel').text('Update Item Details');
        Edit_Item.getEdit_Item(indexItemDetails);

    }


});

var Edit_Item = {
    getEdit_Item: function (index) {

        var rowID = $("#gridDetails").jqxGrid('getrowid', index);
        var data = $("#gridDetails").jqxGrid('getrowdatabyid', rowID);

        var id = data.id_number;
        var code = data.code_string;
        var quantity = data.quantity_number;
        var unitofmeasure = data.unit_of_measure_string;
        var serial = data.serial_number_string;
        var tagnumber = data.tag_number_string;
        var ponumber = data.p_o_number_string;

        $('#txtQuantity').val(quantity);
        $('#cmbUom').val(unitofmeasure);
        $('#txtSerialNbr').val(serial);
        $('#txtTagNbr').val(tagnumber);
        $('#txtPoNbr').val(ponumber);


    }

};

$("#gridDetails").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#gridDetails").jqxGrid('getrowid', boundIndex);

    indexItemDetails = boundIndex;
});


// Inactive Add Item //
$(document).delegate(".gridDetails_Inactive", 'click', function () {
    var rowID = $("#gridDetails").jqxGrid('getrowid', indexItemDetails);
    var data = $("#gridDetails").jqxGrid('getrowdatabyid', rowID);

    show_modal.deactivate(data.id_number);

});

// Inactive confirmation button //
$(document).delegate("#btnProceedDeactivate", "click", function () {
    dbase_operation_draft.deactivate($(this).attr('data-id'));

});
// End function //

// Inactive Transaction //
$(document).delegate(".gridAllTrans_Inactive", 'click', function () {
    var rowID = $("#gridAllTrans").jqxGrid('getrowid', indexAllTrans);
    var data = $("#gridAllTrans").jqxGrid('getrowdatabyid', rowID);

    show_modal.deactivateTransDraft(data.id_number);


});
$(document).delegate("#btnProceedDeactivateTransDraft", "click", function () {
    dbase_operation_draft.deactivate_transDraft($(this).attr('data-id'));

});

$("#gridAllTransGuard").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;
    var rowID = $("#gridAllTransGuard").jqxGrid('getrowid', boundIndex);
    item_returnslipIndex = boundIndex;
});

// show details //
$(document).delegate(".gridAllTransGuard_Details", "click", function () {
    var rowID = $("#gridAllTransGuard").jqxGrid('getrowid', item_returnslipIndex);
    itemdetails_guardapprove = $("#gridAllTransGuard").jqxGrid('getrowdatabyid', rowID);

    var returndate = itemdetails_guardapprove.return_date_date;
    if (returndate == "" || returndate == null) {
        returndate = ""
    } else {
        returndate =  returndate.toISOString().slice(0, 10);
    }

    var attachment = "";
    if (itemdetails_guardapprove.attachment_string == null || itemdetails_guardapprove.attachment_string == "") {
        attachment = "";
    }
    else {
        attachment = itemdetails_guardapprove.attachment_string.substring(itemdetails_guardapprove.attachment_string.indexOf('_') + 1);
    }

    originalFileName = itemdetails_guardapprove.attachment_string;
    fileName = attachment;

    


    show_modal.showmoredetails(
        itemdetails_guardapprove.id_number,
        itemdetails_guardapprove.gate_pass_id_string, 
        returndate,
        itemdetails_guardapprove.purpose_string,
        itemdetails_guardapprove.impex_ref_number_string,
        itemdetails_guardapprove.transaction_type_string,
        itemdetails_guardapprove.supplier_name_string,
        originalFileName);
    $('#currentFile').text((attachment == "") ? "No File Attached" : attachment);
    //$('#file').attr('href', attachmentPath + originalFileName + '');
   
    GPTransId_Global = itemdetails_guardapprove.gate_pass_id_string;
    GPId_Global = itemdetails_guardapprove.id_number;
});
// end function //

// Show More Detail of Return Slip /
$("#grid_returnslip").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;
    var rowID = $("#grid_returnslip").jqxGrid('getrowid', boundIndex);
    item_returnslipIndex = boundIndex;
});

$(document).delegate('.grid_returnslip_Item_Details', 'click', function () {

    var rowID = $("#grid_returnslip").jqxGrid('getrowid', item_returnslipIndex);
    headerdetails_for_RS = $("#grid_returnslip").jqxGrid('getrowdatabyid', rowID);
    show_modal.show_Itemdetails_returnslip(headerdetails_for_RS.gate_pass_id_string, headerdetails_for_RS.usercode_string);

    returnslip_usercode = headerdetails_for_RS.usercode_string;

});
// End //

// desc: ajax call with passing parameters for update of return item
// by: avillena@allegromicro.com
$(document).delegate('#cmdReturnItem', 'click', function () {
    
    // the grid that we are going to get data from
    var elem = $('#grid_return_slip');
    // get the selected indexes
    selectedrowindexes = elem.jqxGrid('selectedrowindexes');
   
    // declare some array containers for the id + returned qty
    var item_id_arr = [];
    var item_itemCode_arr = [];
    var item_returned_qty = [];
    var item_guid = [];

    // loop through the selected row indexes
    for (var ctr = 0; ctr < selectedrowindexes.length; ctr++) {

        var rowID = elem.jqxGrid('getrowid', selectedrowindexes[ctr]);
        itemforreturn = elem.jqxGrid('getrowdatabyid', rowID);
        // push the id into the container
        item_id_arr.push(itemforreturn.Id);
        // push the itemcode into the container
        item_itemCode_arr.push(itemforreturn.ItemCode);
        // push the returned qty to the container
        item_returned_qty.push(itemforreturn.Remaining);

        item_guid.push(itemforreturn.GUID);
    }
   
    dbase_operation_Item.partialreturnByQuantity(itemforreturn.Id, itemforreturn.HeaderCode, item_itemCode_arr, item_returned_qty, item_guid);
});

// desc : display of warning and confirmaion modal
// modified by : avillena@allegromicro.com
$(document).delegate('#cmdReturn', 'click', function () {
    /*
       * @modified by         :   AC <aabasolo@allegromicro.com>
       * @date                :   JAN 10, 2017 9:30 AM
       * @modification        :   modified to populate 2 arrays to get the selected id from the grid + the returned quantity
       */

    var elem = $('#grid_return_slip');
    // get the selected indexes
    selectedrowindexes = elem.jqxGrid('selectedrowindexes');

    var item_itemCode_arr = [];

    // loop through the selected row indexes
    for (var ctr = 0; ctr < selectedrowindexes.length; ctr++) {

        var rowID = elem.jqxGrid('getrowid', selectedrowindexes[ctr]);
        itemforreturn = elem.jqxGrid('getrowdatabyid', rowID);
        item_itemCode_arr.push(itemforreturn.ItemCode);

    }

    //display of warning and confirmaion modal
    if (item_itemCode_arr == "") {
        $("#form_modal_returnslip_details").css('z-index', '100000');
                notification_modal("Warning", "Please check first the item you want to return!", "danger");

                $('#btn_notifClose').click(function () {
                    $('#notification_modal .close').click();
                    $("#form_modal_returnslip_details").css('z-index', '1000000');
                });
    }
    else {
        $("#form_modal_returnslip_details").css('z-index', '100000');
        show_modal.confirmationModal_returnItem('Confirmation', 'Are you sure you want to return this item/s?', 'Success');
    }

});

/*
     * @modified by         :   AC <aabasolo@allegromicro.com>
     * @date                :   JAN 10, 2017 9:30 AM
     * @modification        :   function for loading the custom grid
     */
var custom_grid_ini = {
    return_items: function(elem, fk) {

        var url = "/Transactions/GetTransactionDetailsWithItemsToBeReturned";

        $.ajax({
            url: url,
            dataType: 'json',
            type: 'get',
            data: {
                HeaderKey: fk
            },
            beforeSend: function () {
            },
            headers: {
            },
            success: function (response) {
                if (response.success) {

                    /*
                  * image renderer
                  */
                    var imagerenderer = function (row, datafield, value) {
                        if (value != "") {
                            return '<img style="cursor:pointer;" height="60" width="auto" class="images" src="' + gridPath + value + '"/>';
                        }

                    }

                    // declare the source array
                    var source = {
                        localdata: response.message,
                        datatype: "JSON",
                        datafields: [
                            { name: 'Id', type: 'number' },
                            { name: 'HeaderCode', type: 'string' },
                            { name: 'ItemCode', type: 'string' },
                            { name: 'ItemName', type: 'string' },
                            { name: 'TagNbr', type: 'string' },
                            { name: 'PONbr', type: 'string' },
                            { name: 'SerialNbr', type: 'string' },
                            { name: 'CategoryName', type: 'string' },
                            { name: 'Quantity', type: 'number' },
                            { name: 'ToBeReturned', type: 'number' },
                            { name: 'Remaining', type: 'number' },
                             { name: 'Image', type: 'image' },
                             { name: 'GUID', type: 'string' },
                        ]
                    };

                    // declare the data adapter for the grid
                    var dataAdapter = new $.jqx.dataAdapter(source);

                    // initialize the grid
                    elem.jqxGrid({
                        theme: window.gridTheme,
                        width: '100%',
                        autoheight: true,
                        selectionmode: 'checkbox',
                        source: dataAdapter,
                        editable: true,
                        enabletooltips: true,
                        filterable: true,
                        showfilterrow: true,
                        
                    
                        columns: [

                             {text: 'Declared Quantity', datafield: 'Quantity', width: '13%', editable: false, align: 'right', cellsalign: 'right', filterable: false },
                            {
                                text: 'To be Returned', datafield: 'Remaining', width: '11%', align: 'right', cellsalign: 'right', columntype: 'numberinput', filterable: false,
                                validation: function (cell, value, row) {
                                    var data = elem.jqxGrid('getrowdata', row);                                 
                                    return true;
                                },
                                createeditor: function (row, cellvalue, editor) {
                                    var data = elem.jqxGrid('getrowdata', row);
                                    editor.jqxNumberInput({
                                        //  decimalDigits: 2,
                                        min: 1,
                                        //max: data.ToBeReturned,
                                        disabled: false
                                    });
                                },
                                cellbeginedit: function (row, datafield, columntype) {
                                    var selectedrowindexes = elem.jqxGrid('selectedrowindexes');
                                    if (selectedrowindexes.indexOf(row) <= -1) {
                                        return false;
                                    }
                                },
                                cellvaluechanging: function (row, datafield, columntype, oldvalue, newvalue) {
                                    var data = elem.jqxGrid('getrowdata', row);
                              

                                    if (newvalue > data.ToBeReturned) {
                              
                                        $("#lblGridValidations").show('slow');
                                        $("#lblGridValidations").delay(5000).hide('slow');
                                        return data.ToBeReturned;

                                    }
                             
                                }

                            },                      
                            { text: 'Item Name', datafield: 'ItemName', width: '22%', editable: false, filterable: false },
                            { text: 'Serial Number', datafield: 'SerialNbr', width: '16%', editable: false },
                            { text: 'Tag Number', datafield: 'TagNbr', width: '13%', editable: false },
                            { text: 'PO Number', datafield: 'PONbr', width: '13%', editable: false },
                            { text: 'Image', datafield: 'Image', editable: false, filterable: false, cellsrenderer: imagerenderer },
                        ],
                    });


                } else {
                    notification_modal("An error occured!", response.message, "danger");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });


    },




    // grid for gate pass transaction
    gate_pass_items: function (elem, fk) {

        var url = "/Transactions/GetTransactionDetailsOfGatePass";

        $.ajax({
            url: url,
            dataType: 'json',
            type: 'get',
            data: {
                HeaderKey: fk
            },
            beforeSend: function () {
            },
            headers: {
            },
            success: function (response) {
                if (response.success) {

                    /*
                  * image renderer
                  */
                    var imagerenderer = function (row, datafield, value) {
                        if (value != "") {
                            return '<img style="cursor:pointer;" height="60" width="auto" class="images" src="'+ gridPath + value + '"/>';
                        }

                    }

                    // declare the source array
                    var source = {
                        localdata: response.message,
                        datatype: "JSON",
                        datafields: [
                            { name: 'Id', type: 'number' },
                            { name: 'HeaderCode', type: 'string' },
                            { name: 'ItemCode', type: 'string' },
                            { name: 'ItemName', type: 'string' },
                            { name: 'TagNbr', type: 'string' },
                            { name: 'PONbr', type: 'string' },
                            { name: 'SerialNbr', type: 'string' },
                            { name: 'CategoryName', type: 'string' },
                            { name: 'Quantity', type: 'number' },
                            { name: 'Image', type: 'image' },
                            { name: 'GUID', type: 'string' },
                        ]
                    };

                    // declare the data adapter for the grid
                    var dataAdapter = new $.jqx.dataAdapter(source);

                    // initialize the grid
                    elem.jqxGrid({
                        theme: window.gridTheme,
                        width: '100%',
                        autoheight: true,
                       /// selectionmode: 'checkbox',
                        source: dataAdapter,
                        editable: true,
                        enabletooltips: true,
                        filterable: true,
                        showfilterrow: true,
                       // showtoolbar: true,


                        columns: [
                            { text: 'Quantity', datafield: 'Quantity', width: '10%', editable: false, cellsalign: 'right', filterable: false },
                            { text: 'Item Name', datafield: 'ItemName', width: '30%', editable: false, filterable: false },
                            { text: 'Tag Number', datafield: 'TagNbr', width: '16%', editable: false },
                            { text: 'PO Number', datafield: 'PONbr', width: '16%', editable: false },
                            { text: 'Serial Number', datafield: 'SerialNbr', width: '16%', editable: false },
                            { text: 'Image', datafield: 'Image', editable: false, cellsrenderer: imagerenderer, filterable: false },
                        ]



                    });


                } else {
                    notification_modal("An error occured!", response.message, "danger");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });



    },
  
};





// desc : display snacknotification
// date: jan 18, 2017
// by: avillena@allegromicro.com
function toastNotification() {
    // Get the snackbar DIV
    var x = document.getElementById("snackbar")

    // Add the "show" class to DIV
    x.className = "show";

    // After 3 seconds, remove the show class from DIV
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
}



$(document).ready(function () {
    // prepare the data
    var source =
    {
        datatype: "jsonp",
        datafields: [
            { name: 'countryName' },
            { name: 'name' },
            { name: 'population', type: 'float' },
            { name: 'continentCode' },
            { name: 'adminName1' }
        ],
        async: false,
        url: "http://api.geonames.org/searchJSON",
        data: {
            featureClass: "P",
            style: "full",
            maxRows: 20,
            username: "jqwidgets"
        }
    };
    var dataAdapter = new $.jqx.dataAdapter(source,
        {
            formatData: function (data) {
                data.name_startsWith = $("#searchField").val();
                return data;
            }
        }
    );
    $("#jqxgrid").jqxGrid(
    {
        width: 850,
        source: dataAdapter,
        columns: [
            { text: 'City', datafield: 'name', width: 170 },
            { text: 'Country Name', datafield: 'countryName', width: 200 },
            { text: 'Population', datafield: 'population', cellsformat: 'f', width: 170 },
            { text: 'Continent Code', datafield: 'continentCode', minwidth: 110 }
        ],
        showtoolbar: true,
        autoheight: true,
        rendertoolbar: function (toolbar) {
            var me = this;
            var container = $("<div style='margin: 5px;'></div>");
            var span = $("<span style='float: left; margin-top: 5px; margin-right: 4px;'>Search City: </span>");
            var input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='searchField' type='text' style='height: 23px; float: left; width: 223px;' />");
            toolbar.append(container);
            container.append(span);
            container.append(input);
            if (theme != "") {
                input.addClass('jqx-widget-content-' + theme);
                input.addClass('jqx-rc-all-' + theme);
            }
            var oldVal = "";
            input.on('keydown', function (event) {
                if (input.val().length >= 2) {
                    if (me.timer) {
                        clearTimeout(me.timer);
                    }
                    if (oldVal != input.val()) {
                        me.timer = setTimeout(function () {
                            $("#jqxgrid").jqxGrid('updatebounddata');
                        }, 1000);
                        oldVal = input.val();
                    }
                }
                else {
                    $("#jqxgrid").jqxGrid('updatebounddata');
                }
            });
        }
    });
});

