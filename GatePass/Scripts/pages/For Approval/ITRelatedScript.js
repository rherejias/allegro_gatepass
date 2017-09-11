var global_indexforApprover;
var itId;
var originalFileName;

$(document).ready(function () {
   
 if ($('#viewbagstatus').val() == 'hidedatagrid') {
    $('#bodygridForApproval').slideUp('slow');
    $('#bodyForApprovalDetails').slideDown('slow');
    $('#cmdForApprovalBack').css('display', 'unset');
    $("#gridForApprovalDetails").attr('data-url', $("#gridForApprovalDetails").attr('data-url') + '?HeaderKey=' + $('#txtGatePassIDforApproval').val());
    initialize_jqxwidget_grid($("#gridForApprovalDetails"));

    if ($('#viewbaguploadedfile').val() == "") {
        $('#currentFile').text('No file attach');
    } else { $('#currentFile').text($('#viewbaguploadedfile').val()); }
 }
});

//$(document).delegate("#file", "click", function () {
//    database_operation.download($(this).attr('data-file'), $(this).attr('data-path'));
//});

function DownloadAttachment() {
    if (originalFileName == "" || originalFileName == null) {
        $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'Uploaded file is empty.');
        Warning_ToastNotif();
    }
    else {
        window.location.href = '/Attachment/DownloadAttachment?filename=' + originalFileName + '&path=' + env;
    }
}

$("#gridForApproval").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;


    global_indexforApprover = boundIndex;

    var rowID = $("#gridForApproval").jqxGrid('getrowid', boundIndex);
    var data = $("#gridForApproval").jqxGrid('getrowdatabyid', rowID);
    itId = data;
});


$(document).delegate(".gridForApproval_Details", "click", function () {
  
    database_operation.viewDetails();
    $('#bodygridForApproval').slideUp('slow');
    $('#bodyForApprovalDetails').slideDown('slow');
    $('#cmdForApprovalBack').css('display', 'unset');
  
});

$(document).delegate(".images", "click", function () {
    show_modal.imageView($(this).attr("src"));
});


$(document).delegate("#btnWithCorrection", "click", function () {
    show_modal.withCorrectionComment();
});


$(document).delegate("#btnGPReject", "click", function () {
    show_modal.rejectComment();
});

$(document).delegate("#btnGPApproved", "click", function () {
    show_modal.confirmationGPApprove($('#txtGatePassIDforApproval').val());
});


$(document).delegate("#btnSubmitWithCorrection", "click", function () {

    if ($('#txtWithCorrectionComment').val() == "" || $('#txtWithCorrectionComment').val() == null) {

        $('#lblcorrectioncommentrequired').show('slow');
        $("#lblcorrectioncommentrequired").delay(3000).hide('slow');
    } else { alert('has a comment');}
      
});

$(document).delegate("#btnSubmitReject", "click", function () {

    if ($('#txtrejectComment').val() == "" || $('#txtrejectComment').val() == null) {

        $('#lblrejectcommentrequired').show('slow');
        $("#lblrejectcommentrequired").delay(3000).hide('slow');
    } else {
        $("#rejectModal").css('z-index', '10000');
        show_modal.confirmationGPReject($('#txtGatePassIDforApproval').val());
        $('#btnModalConfirmRejectCancel').click(function () {
            $('#confirmRejectModal .close').click();
            $("#rejectModal").css('z-index', '1000000');
        });
    }

});
$(document).delegate("#btnModalConfirmRejectSubmit", "click", function () {

    database_operation.GatePassRejected($('#txtrejectComment').val());

});

$(document).delegate("#btnSubmitApproved", "click", function () {
 
    database_operation.GatePassApproved();
   
});


$("#cmdForApprovalBack").click(function () {
    $("#gridForApprovalDetails").attr('data-url', '/Transactions/GetTransDetails');
    $('#bodyForApprovalDetails').slideUp('slow');
    $('#bodygridForApproval').slideDown('slow');
    $('#cmdForApprovalBack').hide();

});




var database_operation = {

    GatePassApproved: function () {

        $.ajax({
            url: '/ForApproval/ApprovedByITRelatedApprover',
            dataType: 'json',
            type: 'get',
            data: {
                id: itId.id_number,
                headercode: $('#txtGatePassIDforApproval').val()

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
                    notification_modal("Approval Failed!", response.message, "danger");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });


    },
    GatePassRejected: function (commentOfRejected) {

        $.ajax({
            url: '/ForApproval/RejectedByITRelated',
            dataType: 'json',
            type: 'get',
            data: {
                id: itId.id_number,
                headercode: $('#txtGatePassIDforApproval').val(),
                remarks: commentOfRejected

            },
            beforeSend: function () {

            },
            headers: {
            },
            success: function (response) {

                if (response.success) {
                    $('#rejectModal .close').click();
                    $("#cmdForApprovalBack").click();
                    toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Gate Pass Rejected!";
                    $('#gridForApproval').jqxGrid('updatebounddata');
                    Save_ToastNotif();
                } else {
                    notification_modal("Approval Failed!", response.message, "danger");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });


    },



    viewDetails: function () {


        var rowID = $("#gridForApproval").jqxGrid('getrowid', global_indexforApprover);
        var data = $("#gridForApproval").jqxGrid('getrowdatabyid', rowID);

        var id = data.id_number;
        var GP_Id = data.gate_pass_id_string;
        var purpose = data.purpose_string;
        var impex = data.impex_ref_number_string;      
        var returndate = data.return_date_date;
        var transtype = data.transaction_type_string;

         database_operation.GetGatePassOwner(GP_Id);

        var attachment = "";
        if (data.attachment_string == null || data.attachment_string == "") {
            attachment = "";
        }
        else {
            attachment = data.attachment_string.substring(data.attachment_string.indexOf('_') + 1);
        }

        var supplier = data.supplier_name_string;
        var contactname = data.contact_name_string;
        originalFileName = data.attachment_string;
        $('#txtGatePassIDforApproval').val(GP_Id);
        $('#txtPurposeforApproval').val(purpose);
        $('#txtImpexRefNbrforApproval').val(impex);
        $('#cmbSupplierforApproval').val(supplier);
        $('#cmdAddContactforApproval').val(contactname);
        $('#cmbTransTypeforApproval').val(transtype);
        if (returndate == "" || returndate == null) {
            $('#txtReturnDateforApproval').val(returndate);
        } else {
           $('#txtReturnDateforApproval').val(returndate.toISOString().slice(0, 10));
        }

        



        if (attachment == "" || attachment == null) {
          $('#currentFile').text("No file attached");
        } else { $('#removeAttachment').show(); $('#currentFile').text(attachment); }


        //$('#file').attr('href', attachmentPath + originalFileName + '');

        $("#gridForApprovalDetails").attr('data-url', $("#gridForApprovalDetails").attr('data-url') + '?HeaderKey=' + GP_Id);
        initialize_jqxwidget_grid($("#gridForApprovalDetails"));

    },

    //rherejias backup download (dynamic)
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
    },


    /// to get the gatepass owner
    GetGatePassOwner: function (headercode) {

     $.ajax({
        url: '/ForApproval/GetGatePassOwner',
        dataType: 'json',
        type: 'get',
        data: {
               
            headercode: headercode
                

        },
        beforeSend: function () {

        },
        headers: {
        },
        success: function (response) {

            if (response.success) {

                $('#txtrequestedby').val(response.message);
                  
            } else {
                notification_modal("Approval Failed!", response.message, "danger");
            }

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(thrownError);
        }
    });


    }
/// End




};
var show_modal = {

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
        $("#form_modal").html(modal);
        $("#modalImageView").modal("show");
        $("#modalImageView").css('z-index', '1000000');
    },

    withCorrectionComment: function () {
  
        var modal = '<div class="modal fade" id="withcorrectionModal" role="dialog" >';
        modal += '<div class="modal-dialog">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#08A7C3; color:#ffffff">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h3 class="modal-title">Comment</h3>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:3%">';
        modal += '<textarea type="text" style="resize:none;border-color:#f98989" rows="3" cols="50" class="form-control" id="txtWithCorrectionComment" placeholder="Please enter a comment.." maxlength="255"></textarea>';
        modal += '<p style="color:#f98989" id="lblcorrectioncommentrequired" hidden>This field is required with the maximum of 255 characters.</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<a class="btn btn-meduim btn-blue" id="btnSubmitWithCorrection">Submit</a>';
        modal += '<a class="btn btn-meduim btn-gray" data-dismiss="modal">Cancel</a>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#withcorrectionModal").modal("show");
        $("#withcorrectionModal").css('z-index', '1000000');
    },

    rejectComment: function () {
  
        var modal = '<div class="modal fade" id="rejectModal" role="dialog" >';
        modal += '<div class="modal-dialog">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#08A7C3; color:#ffffff">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title"><i class="fa fa-comment-o fa-lg" aria-hidden="true"></i>&nbsp;Reject Comment</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:3%">';
        modal += '<textarea type="text" style="resize:none;border-color:#f98989" rows="3" cols="50" class="form-control" id="txtrejectComment" placeholder="Please enter a comment.." maxlength="255"></textarea>';
        modal += '<p style="color:#f98989" id="lblrejectcommentrequired" hidden>This field is required with the maximum of 255 characters.</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<a class="btn btn-small btn-blue" id="btnSubmitReject">Submit</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#rejectModal").modal("show");
        $("#rejectModal").css('z-index', '1000000');
    },


    confirmationGPApprove: function (gatepassId) {
       
        var modal = '<div class="modal fade" id="approveModal" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#08A7C3; color:#ffffff">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title"><i class="fa fa-question-circle fa-lg" aria-hidden="true"></i>&nbsp;Confirmation</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:3%">';
        modal += '<p>Are you sure you want to approve this Gate Pass?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<a class="btn btn-small btn-blue" data-dismiss="modal" id="btnSubmitApproved">YES</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal">NO</a>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#approveModal").modal("show");
        $("#approveModal").css('z-index', '1000000');
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
        modal += '<p>Are you sure you want to reject this Gate Pass?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<a class="btn btn-small btn-blue" data-dismiss="modal" id="btnModalConfirmRejectSubmit">YES</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal" id="btnModalConfirmRejectCancel">NO</a>';
        modal += '</div>';

        $("#confirmation_modal").html(modal);
        $("#confirmRejectModal").modal("show");
        $("#confirmRejectModal").css('z-index', '1000000');
    },
};

// Search function //
$(document).delegate("#gridForApproval_searchField", "keyup", function (e) {
    var columns = ["gate_pass_id_string", "purpose_string", "return_date_date"];
    generalSearch($('#gridForApproval_searchField').val(), "gridForApproval", columns, e);

});
// End//