var indexItemDetails;
var draftId;
var datatable, datatableContact, indexAllTrans;
var itemreturn, headerdetails_for_RS;
var itemforreturn;
var arrayitemforreturn = new Array;
var selectedrowindexes = new Array;
var imagesrc, returnslip_usercode;
var fileName;
var originalFileName;


function clear() {
    $('#txtPurpose').val("");
    $('#txtImpexRefNbr').val("");
    $('#cmbTransType').val("IN");
    $('#txtReturnDate').val("");
};

$("#removeAttachment").click(function () {
    $("#currentFile").text("No file chosen");
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

$(document).ready(function () {

    iniSupplierContactpersonComboDrafts();

    $("#cmdSaveAndSubmit").click(function () {

        var datainformations = $('#gridDetails').jqxGrid('getdatainformation');
        var rowscounts = datainformations.rowscount;

        // alert(rowscounts);

        if (typeof $("#cmdAddItem").attr("data-transid") == "undefined" || $("#cmdAddItem").attr("data-transid") == "") {
            //console.log("add new");
            var transdetails = ["#txtImpexRefNbr", "#txtPurpose", "#txtReturnDate", "#cmbTransType"];
            var ctr = 0;
            for (var i = 0; i <= 4; i++) {
                if ($(transdetails[i]).val() == "") {
                    $(transdetails[i]).css("border-color", "red");

                }
                else {
                    $(transdetails[i]).css("border-color", "#e5e5e5");
                    ctr++;

                }
            }
            if (ctr == 5) {

                if (rowscounts != 0) {    //($(".jqx-grid-empty-cell-metro").text() != "No data to display") {
                    show_modal.confirmationModal('Submit Record', 'Are you sure you want to submit this record?', 'Submitted');

                }
                else {
                    notification_modal("Warning", "Please add an item to submit transactions!", "danger");
                    $('#btn_notifClose').click(function () {
                        $('#notification_modal .close').click();
                    });
                }

            }



        } else {
            //console.log("submit draft");
            var transdetails2 = ["#txtImpexRefNbr", "#txtPurpose", "#txtReturnDate", "#cmbTransType"];
            var ctr = 0;
            for (var i = 0; i <= 4; i++) {
                if ($(transdetails2[i]).val() == "") {
                    $(transdetails2[i]).css("border-color", "red");

                }
                else {
                    $(transdetails2[i]).css("border-color", "#e5e5e5");
                    ctr++;

                }
            }
            if (ctr == 5) {
                if (rowscounts != 0) {    //($(".jqx-grid-empty-cell-metro").text() != "No data to display") {
                    show_modal.confirmationModal('Submit Draft', 'Are you sure you want to submit this draft?', 'Submitted');

                }
                else {
                    notification_modal("Warning", "Please add an item to submit transactions!", "danger");
                    $('#btn_notifClose').click(function () {
                        $('#notification_modal .close').click();
                    });
                }


            }

        }
    });

    $("#cmdSaveAsDraft").click(function () {
        //show_modal.confirmationModal('addHeader_draft', 'Add as draft', 'Are you sure you want to continue this transaction?', 'draft');      

        var transdetails = ["#txtImpexRefNbr", "#txtPurpose", "#txtReturnDate", "#cmbTransType"];
        var ctr = 0;
        for (var i = 0; i <= 4; i++) {
            if ($(transdetails[i]).val() == "") {
                $(transdetails[i]).css("border-color", "red");

            }
            else {
                $(transdetails[i]).css("border-color", "#e5e5e5");
                ctr++;

            }
        }
        if (ctr == 5) {
            show_modal.confirmationModal('Submit Draft', 'Are you sure you want to submit this draft?', 'Drafted');
        }



    });

    $("#cmdAddItem").click(function () {
        show_modal.addNewItem((typeof $(this).attr("data-transid") == "undefined" ? "" : $(this).attr("data-transid")));

    });



    $("#cmdInboxBack").click(function () {

        $("#gridDetails").attr('data-url', '/Transactions/GetTransDetails');

        $('#bodyDraftsDetails').slideUp('slow');
        $('#bodyTransDrafts').slideDown('slow');
        
        clear();
        $('#txtImpexRefNbr').css("border-color", "#e5e5e5");
        $('#txtPurpose').css("border-color", "#e5e5e5");
        $('#txtReturnDate').css("border-color", "#e5e5e5");
        $('#cmbTransType').css("border-color", "#e5e5e5");
        $('#txtGatePassIDLabel').hide();
        $('#txtGatePassID').hide();


    });


    // DATE PICKER FUNCTION //
    $(".form_datetime").datetimepicker({
        minView: 2,
        autoclose: true,
        todayBtn: true,
        pickerPosition: "bottom-left",
        startDate: '-0d'
    });
    // END FUNCTION //

});

//adding a new transaction or submitting a draft
$(document).delegate("#cmdProceedSaveAndSubmit", "click", function () {
    //console.log($(this).attr("data-transid"));
    dbase_operation.addHeader($(this).attr("data-transid"), $(this).attr("data-transstat"));
});


$(document).delegate(".gridInbox_Edit", 'click', function () {

    Edit.getEdit(indexAllTrans);
   
    $('#bodyDraftsDetails').slideDown('slow');
    $('#bodyTransDrafts').slideUp('slow');
    $('#cmdInboxBack').css('display', 'unset');

});
var draftId;


var Edit = {
    getEdit: function (index) {

        var rowID = $("#gridInbox").jqxGrid('getrowid', index);
        var data = $("#gridInbox").jqxGrid('getrowdatabyid', rowID);
        var id = data.id_number;
        var code = data.gate_pass_id_string;
        var impex = data.impex_ref_number_string;
        var supplier = data.supplier_string;
        var contactperson = data.contact_person_string;
        var purpose = data.purpose_string;
        var returndate = data.return_date_datetime;
        var transtype = data.transaction_type_string;

        $('#cmbSupplier').jqxDropDownList('val', supplier);
        var returndateindex = returndate.indexOf("T");
        var returndatesubstrg = returndate.substring(0, returndateindex);
        var attachment = data.attachment_string.substring(data.attachment_string.indexOf('_') + 1);
        originalFileName = data.attachment_string;
        fileName = attachment;
       
        
        $('#txtGatePassIDLabel').show();
        $('#txtGatePassID').show();

        $('#cmbContactPerson').jqxDropDownList('val', contactperson);
        $('#txtGatePassID').text(code);
        $('#currentFile').removeAttr('hidden');
        $('#lblcurrentFile').removeAttr('hidden');
        $('#removeAttachment').css('display', 'inline');

        $('#txtPurpose').val(purpose);
        $('#txtImpexRefNbr').val(impex);
     
      
        $('#cmbTransType').val(transtype);
        $('#txtReturnDate').val(returndatesubstrg);
        $('#currentFile').text((attachment == "") ? "No file chosen" : attachment);
        
        $('#file').attr('href', '../FileUploads/attachment/files/' + originalFileName + '');
        //add attribute to the add item button
        $("#cmdAddItem").attr("data-transid", id);

        //clone grid element
        $("#gridDetails").clone().appendTo("#detailsContainer");
        //delete first grid element
        $("#gridDetails").remove();

        //$("#gridDetails").attr('data-url', $("#gridDetails").attr('data-url') + '?HeaderKey=' + ((code != "" || typeof code =="undefined") ? code : id));
        //change the data-url attribute of the newly cloned element
        $("#gridDetails").attr('data-url', $("#gridDetails").attr('data-url') + '?HeaderKey=' + code);
        //initialize grid
        initialize_jqxwidget_grid($("#gridDetails"));

        draftId = id;
    }

};

$("#gridInbox").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#gridInbox").jqxGrid('getrowid', boundIndex);
    var data = $("#gridInbox").jqxGrid('getrowdatabyid', rowID);

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

    addAddItems: function (parentId) {
        var formData = new FormData();
        formData.append('inpt_file', $('input[name="inpt_file"]')[0].files[0]);
        formData.append('id', parentId);
        formData.append('Quantity', $("#txtQuantity").val());
        formData.append('UnitOfMeasureCode', $("#cmbUom").val());
        formData.append('ItemCode', $('#cmbItem').val());
        formData.append('SerialNbr', $("#txtSerialNbr").val());
        formData.append('TagNbr', $("#txtTagNbr").val());
        formData.append('PONbr', $("#txtPoNbr").val());
        formData.append('IsActive', true);
        formData.append('Remarks', $("#txtItemRemarks").val());
        formData.append('Image', $("#inpt_file").val());
        var itemdetails = ["#txtQuantity", "#cmbUom", "#cmbItem", "#txtSerialNbr", "#txtTagNbr",
                                                 "#txtPoNbr", "#txtItemRemarks", "#cmbSupplier", "#cmbCategory", "#cmbItemType"];
        var ctr = 0;
        for (var i = 0; i <= 9; i++) {
            if ($(itemdetails[i]).val() == "") {
                $(itemdetails[i]).css("border-color", "red");
            }
            else {
                $(itemdetails[i]).css("border-color", "#e5e5e5");
                ctr++;
            }
        }

        if (ctr == 10) {
            $.ajax({
                url: '/Transactions/AddItemsDetails',
                type: 'post',
                dataType: 'json',
                data: formData,
                processData: false,
                contentType: false,
                beforeSend: function () {

                },
                headers: {
                    //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                },
                success: function (response) {

                    if (response.success) {

                        // notification_modal("Addition successful!", response.message, "success");
                        //initialize_jqxwidget_grid($("#gridDetails"));
                        $("#form_modal_div").modal("hide");
                        $("#gridDetails").jqxGrid('updatebounddata');
                    } else {
                        notification_modal("Addition failed!", response.message, "danger");
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status);
                    console.log(thrownError);
                }
            });
        }
    },
    removeItems: function () {
        $.ajax({
            url: '/Transactions/RemoveItems',
            dataType: 'json',
            type: 'post',
            data: {},
            beforeSend: function () {

            },
            headers: {
                //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {
                if (response.success) {
                    console.log("Removed items with session id = " + response.message);
                    $("#gridDetails").jqxGrid('updatebounddata');
                } else {
                    console.log("Addition failed! " + response.message);
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },


    // Ajax Load Category and Type //
    itemsloadCatg_Type: function () {

        $.ajax({
            url: '/Transactions/LoadCategoryType',
            dataType: 'json',
            type: 'post',
            data: {
                ItemCode: $('#cmdItem').val(),

            },
            beforeSend: function () {

            },
            headers: {
                //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {
                if (response.success) {
                    notification_modal("Add Record", response.message, "success");

                    $('#btn_notifClose').click(function () {
                        $('#notification_modal .close').click();
                    });

                    //initialize_jqxwidget_grid($("#gridDetails"));
                    //$("#gridDetails").jqxGrid('updatebounddata');
                } else {
                    notification_modal("Add Record", response.message, "danger");
                }

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
                //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {
                if (response.success) {
                    notification_modal("Add Record", response.message, "success");

                    $('#btn_notifClose').click(function () {
                        window.location.reload();
                        //$('#notification_modal .close').click();
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
    // End function //


    //Inactive_DraftTransaction //
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
                    $('#gridInbox').jqxGrid('updatebounddata');
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
    //console.log($(this).attr("data-procedure"));
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
                //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {
                if (response.success) {

                    notification_modal("Add Record", response.message, "success");
                    $('#btn_notifClose').click(function () {
                        // $('#notification_modal .close').click();
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
    }
};


var show_modal = {
   
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
        modal += '<img height="auto" width="100%" class="images" src="' + source + '"/>';
        modal += '</div>';
        $("#form_modal").html(modal);
        $("#modalImageView").modal("show");
        $("#modalImageView").css('z-index', '1000000');
    },


    // Inactive modal confirmation //
    deactivate: function (Itemid) {
        var modal = '<div class="modal fade" id="modalDeactivate" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#F25656; color:#ffffff">';
        modal += '<h4 class="modal-title">Inactive Record</h4>';
        modal += '</div>';
        // modal += '<br/>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to remove this record?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<button class="btn btn-danger" id="btnProceedDeactivate"';
        modal += 'data-id="' + Itemid + '">';
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
    // end function //

    // this is for inactive draft modal confirmation //
    deactivateTransDraft: function (Itemid) {
        var modal = '<div class="modal fade" id="modalDeactivate" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';
        modal += '<div class="modal-header" style="background-color:#F25656; color:#ffffff">';
        modal += '<h4 class="modal-title">Inactive Record</h4>';
        modal += '</div>';
        //modal += '<br/>';
        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to inactive this record?</p>';
        modal += '</div>';
        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<button type="button" class="btn btn-danger" id="btnProceedDeactivateTransDraft"';
        modal += 'data-id="' + Itemid + '">';
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
    // end function //


    // confirmation modal //
    confirmationModal: function (title, message, status) {
        var trans_id = "";

        if (typeof $("#cmdAddItem").attr("data-transid") != "undefined" && $("#cmdAddItem").attr("data-transid") != "") {
            trans_id = $("#cmdAddItem").attr("data-transid");
        }

        var modal = '<div class="modal fade" id="conf_modal_div" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
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
        modal += '<button type="button" class="btn btn-default" data-dismiss="modal">No</button>';
        modal += '<button type="button" class="btn btn-success" data-dismiss="modal" data-transid="' + trans_id + '" data-transstat="' + status + '" id="cmdProceedSaveAndSubmit">Yes</button>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#confirmation_modal").html(modal);
        $("#conf_modal_div").modal("show");
        $("#conf_modal_div").css('z-index', '1000000');
    },
    // End function //


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
        modal += '<button type="button" class="btn btn-default" data-dismiss="modal">No</button>';
        modal += '<button type="button" class="btn btn-success" data-dismiss="modal" id="cmdUpdateDraft">Yes</button>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#confirmation_modal").html(modal);
        $("#conf_modal_div2").modal("show");
        $("#conf_modal_div2").css('z-index', '1000000');
    },

    // Add Item modal //
    addNewItem: function (transid) {

        var modal = '<div class="modal fade" id="form_modal_div" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
        modal += '<div class="modal-dialog modal-lg">';
        modal += '<div class="modal-content">';

        modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title" id="myModalLabel">Add New Item</h4>';
        modal += '</div>';

        modal += '<div class="modal-body">';
        modal += '<br />';
        modal += '<div class="row">';


        modal += '<div class="col-md-12">';
        modal += '<div class="form-group col-md-6">';
        modal += '<label for="cmbItem">Item:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
        modal += '<div class="form-control item dropdownlist" id="cmbItem" data-url="/Items/GetItems" data-display="name_string" data-value="code_string"></div>';
        modal += '</div>';
        modal += '  <div class="form-group col-md-3">';
        modal += '    <label for="cmbCategory">Category:</label>';
        // modal += '   <input type="text" class="form-control" id="cmbCategory" readonly style="font-size:12px; font-family: segoe ui; background-color:transparent">';
        modal += '    <div class="form-control dropdownlist" id="cmbCategory" data-url="/Items/GetItemCategories" data-display="name_string" data-value="code_string"></div>';
        modal += '  </div>';
        modal += '<div class="form-group col-md-3">';
        modal += '  <label for="txtSerialNbr">Serial Number:</label>';
        modal += '  <input type="text" class="form-control" id="txtSerialNbr" style="font-size:12px; font-family: segoe ui">';
        modal += '</div>';
        modal += '  </div>';
        modal += '<div class="col-md-12">';
        modal += '  <div class="form-group col-md-3">';
        modal += '  <label for="cmbItemType">Type:</label>';
        // modal += '  <input type="text" class="form-control" id="cmbItemType" readonly style="font-size:12px; font-family: segoe ui; background-color:transparent">';
        modal += '    <div class="form-control dropdownlist" id="cmbItemType" data-url="/Items/GetItemTypes" data-display="name_string" data-value="code_string"></div>';
        modal += '  </div>';
        modal += '  <div class="form-group col-md-3">';
        modal += '    <label for="txtTagNbr">Tag Number:</label>';
        modal += '    <input type="text" class="form-control" id="txtTagNbr" style="font-size:12px; font-family: segoe ui">';
        modal += '  </div>';
        modal += '  <div class="form-group col-md-3">';
        modal += '    <label for="txtPoNbr">PO Number:</label>';
        modal += '    <input type="text" class="form-control" id="txtPoNbr" style="font-size:12px; font-family: segoe ui">';
        modal += '  </div>';
        modal += '  <div class="form-group col-md-3">';
        modal += '    <label for="txtQuantity">Quantity:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
        modal += '    <input type="number" class="form-control" id="txtQuantity" style="font-size:12px; font-family: segoe ui">';
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="col-md-12">';
        modal += '  <div class="form-group col-md-3">';
        modal += '    <label for="cmbUom">Unit of Measure:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
        modal += '    <div class="form-control dropdownlist" id="cmbUom" data-url="/Items/GetUnitOfMeasures" data-display="name_string" data-value="code_string"></div>';
        modal += '  </div>';
        modal += '<div class="form-group col-md-3">';
        modal += '<label for="txtItemRemarks">Remarks:</label>';
        modal += '<input type="text" class="form-control" id="txtItemRemarks" style="font-size:12px; font-family: segoe ui">';
        modal += '</div>';
        modal += '<div class="form-group col-md-6">';
        modal += '<label for="inpt_file">Upload Image:</label>';
        //modal += '<div class="list-group">';
        //modal += '<a class="list-group-item">';
        modal += '<div><input type="file" id="inpt_file" name="inpt_file" style="color:#F25656; font-size:1.1em;" /></div>';
        //modal += '</a>'
        //modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';


        //modal += '<div class="col-md-12">';
        //modal += '<div class="form-group col-md-6">';
        //modal += '  <label for="cmbSupplier">Supplier*:</label>';
        //modal += '  <div class="form-control dropdownlist" id="cmbSupplier" data-url="/Transactions/GetSupplierCombo" data-display="supplier_string" data-value="code_string"></div>';
        //modal += '</div>';
        //modal += '<div class="form-group col-md-3">';
        //modal += '  <label for="txtSerialNbr">Serial Number*:</label>';
        //modal += '  <input type="text" class="form-control" id="txtSerialNbr" style="font-size:12px; font-family: segoe ui">';
        //modal += '</div>';
        //modal += '  <div class="form-group col-md-3">';
        //modal += '    <label for="txtTagNbr">Tag Number*:</label>';
        //modal += '    <input type="text" class="form-control" id="txtTagNbr" style="font-size:12px; font-family: segoe ui">';
        //modal += '  </div>';
        //modal += '  </div>';

        //modal += '<div class="col-md-12">';
        //modal += '<div class="form-group col-md-3">';
        //modal += '  <label for="cmbItem">Item*:</label>';
        //modal += '  <div class="form-control item dropdownlist" id="cmbItem" data-url="/Items/GetItems" data-display="name_string" data-value="code_string"></div>';
        //modal += '</div>';
        //modal += '  <div class="form-group col-md-3">';
        //modal += '    <label for="cmbCategory">Category*:</label>';
        //modal += '   <input type="text" class="form-control" id="cmbCategory" readonly style="font-size:12px; font-family: segoe ui; background-color:transparent">';
        //// modal += '    <div class="form-control dropdownlist" id="cmbCategory" data-url="/Items/GetItemCategories" data-display="name_string" data-value="code_string"></div>';
        //modal += '  </div>';
        //modal += '  <div class="form-group col-md-3">';
        //modal += '  <label for="cmbItemType">Type*:</label>';
        //modal += '  <input type="text" class="form-control" id="cmbItemType" readonly style="font-size:12px; font-family: segoe ui; background-color:transparent">';
        //// modal += '    <div class="form-control dropdownlist" id="cmbItemType" data-url="/Items/GetItemTypes" data-display="name_string" data-value="code_string"></div>';
        //modal += '  </div>';
        //modal += '  <div class="form-group col-md-3">';
        //modal += '    <label for="txtQuantity">Quantity*:</label>';
        //modal += '    <input type="number" class="form-control" id="txtQuantity" style="font-size:12px; font-family: segoe ui">';
        //modal += '  </div>';
        //modal += '</div>';

        //modal += '<div class="col-md-12">';
        //modal += '  <div class="form-group col-md-3">';
        //modal += '    <label for="cmbUom">Unit of Measure*:</label>';
        //modal += '    <div class="form-control dropdownlist" id="cmbUom" data-url="/Items/GetUnitOfMeasures" data-display="name_string" data-value="code_string"></div>';
        //modal += '  </div>';
        //modal += '  <div class="form-group col-md-3">';
        //modal += '    <label for="txtPoNbr">PO Number*:</label>';
        //modal += '    <input type="text" class="form-control" id="txtPoNbr" style="font-size:12px; font-family: segoe ui">';
        //modal += '  </div>';
        //modal += '<div class="form-group col-md-6">';
        //modal += '  <label for="txtItemRemarks">Remarks*:</label>';
        //modal += '  <input type="text" class="form-control" id="txtItemRemarks" style="font-size:12px; font-family: segoe ui">';
        //modal += '</div>';
        //modal += '</div>';

        //modal += '<div class="col-md-12">';
        //modal += '  <div class="form-group col-md-3">';
        //modal += '    <label for="inpt_file">Upload Image:</label>';
        //modal += '      <div><input type="file" id="inpt_file" name="inpt_file" style="color:#F25656; font-size:1.1em;" /></div>';
        ////  modal += '    <div class="form-control dropdownlist" id="cmbUom" data-url="/Items/GetUnitOfMeasures" data-display="name_string" data-value="code_string"></div>';
        //modal += '  </div>';
        //modal += '</div>';

        //modal += '</div>';
        //modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>';
        modal += '<button type="button" class="btn btn-success" data-transid="' + transid + '" id="cmdSaveItem">Add</button>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);

        $("#form_modal_div").modal("show");
        $("#form_modal_div").css('z-index', '1000000');



        ini_main.element('inputtext');
        ini_main.element('inputnumber');
        ini_main.element('dropdownlist');

    },
    // end function //

    // Show details modal //
    //showmoredetails: function () {

    //    var modal = '<div class="modal fade" id="form_modal_div1" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
    //    modal += '<div class="modal-dialog modal-lg">';
    //    modal += '<div class="modal-content">';

    //    modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
    //    modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
    //    modal += '<h4 class="modal-title" id="myModalLabel">Show Details</h4>';
    //    modal += '</div>';

    //    modal += '<div class="modal-body">';
    //    modal += '<br />';
    //    modal += '<div class="row">';
    //    modal += '<div class="col-md-6">';
    //    modal += '<div class="row">';
    //    modal += '<div class="form-group col-md-6">';
    //    modal += '<label for="txtGatePassID">Gate Pass ID*:</label>';
    //    modal += '<input type="text" class="form-control" id="txtGatePassID" placeholder="System Generated" readonly="readonly">';
    //    modal += ' </div>';
    //    modal += '<div class="form-group col-md-6">';
    //    modal += '<label for="txtImpexRefNbr">IMPEX Ref. Number*:</label>';
    //    modal += '<input type="text" class="form-control" id="txtImpexRefNbr" placeholder="IMPEX Ref. Number">';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '<div class="row">';
    //    modal += ' <div class="col-md-6">';
    //    modal += '<label for="cmbTransType">Transaction Type*:</label>';
    //    modal += '<input type="text" class="form-control" id="txtGatePassID" placeholder="System Generated" readonly="readonly">';
    //    modal += '</div>';
    //    modal += '<div class="col-md-6">';
    //    modal += '<label for="txtReturnDate">Return Date*:</label>';
    //    modal += '<div class="input-group input-append date form_datetime">';
    //    modal += '<input id="txtReturnDate" type="text" class="form-control" style="background-color:transparent" readonly />';
    //    modal += '<span class="input-group-addon add-on">';
    //    modal += '<i class="fa fa-calendar faa-tada animated-hover"';
    //    modal += 'style="color: #31B0D5">';
    //    modal += '</i>';
    //    modal += '</span>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '<div class="col-md-6">';
    //    modal += '<div class="col-md-12">';
    //    modal += '<label for="txtPurpose">Purpose*:</label>';
    //    modal += ' <textarea class="form-control" placeholder="Purpose" rows="5=6" id="txtPurpose"></textarea>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '<br />';
    //    modal += '<div class="modal-footer">';
    //    modal += '<button type="button" class="btn btn-default" data-dismiss="modal">No</button>';
    //    modal += '<button type="button" class="btn btn-success" data-dismiss="modal">Yes</button>';
    //    modal += '</div>';

    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '</div>';

    //    $("#form_modal").html(modal);

    //    $("#form_modal_div1").modal("show");
    //    $("#form_modal_div1").css('z-index', '1000000');

    //    ini_main.element('inputtext');
    //    ini_main.element('inputnumber');
    //    ini_main.element('dropdownlist');

    //},
    // end function //


    // Item-Details Modal //
    //show_Itemdetails_returnslip: function (returnslip_code, dateadded, purpose, impexnumber, transtype, usercode) {

    //    var modal = '<div class="modal fade" id="form_modal_returnslip_details" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
    //    modal += '<div class="modal-dialog modal-lg">';
    //    modal += '<div class="modal-content">';

    //    modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
    //    modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
    //    modal += '<h4 class="modal-title" id="myModalLabel">Item Details</h4>';
    //    modal += '</div>';

    //    modal += '<div class="modal-body">';
    //    modal += '<br />';
    //    modal += '<div class="row">';
    //    modal += '<div class="col-md-6">';
    //    modal += '<div class="row">';
    //    modal += '<div class="form-group col-md-6">';
    //    modal += '<label for="txtGatePassID" style="font-size:12px; font-family: segoe ui">Gate Pass ID*:</label>';
    //    modal += '<input type="text" class="form-control input-sm" id="txtGatePassID" style="background-color:transparent;font-size:12px; font-family: segoe ui" value="' + returnslip_code + '" readonly="readonly">';
    //    modal += ' </div>';
    //    modal += '<div class="form-group col-md-6">';
    //    modal += '<label for="txtImpexRefNbr">IMPEX Ref. Number*:</label>';
    //    modal += '<input type="text" class="form-control input-sm" id="txtImpexRefNbr" placeholder="IMPEX Ref. Number" style="background-color:transparent" value="' + impexnumber + '" readonly>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '<div class="row">';
    //    modal += ' <div class="col-md-6">';
    //    modal += '<label for="cmbTransType">Transaction Type*:</label>';
    //    modal += '<input type="text" class="form-control input-sm" id="txtGatePassID" value= "' + transtype + '" style="background-color:transparent" readonly="readonly">';
    //    modal += '</div>';
    //    modal += '<div class="col-md-6">';
    //    modal += '<label for="txtReturnDate">Return Date*:</label>';
    //    modal += '<div class="input-group input-append date form_datetime">';
    //    modal += '<input id="txtReturnDate" type="text" class="form-control input-sm" style="background-color:transparent" value= "' + dateadded + '" readonly/>';
    //    modal += '<span class="input-group-addon add-on">';
    //    modal += '<i class="fa fa-calendar faa-tada animated-hover"';
    //    modal += 'style="color: #31B0D5">';
    //    modal += '</i>';
    //    modal += '</span>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '<div class="col-md-6">';
    //    modal += '<div class="col-md-8">';
    //    modal += '<label for="txtPurpose">Purpose*:</label>';
    //    modal += ' <textarea class="form-control input-sm" rows="5=6" style="background-color:transparent;font-size:12px; font-family: segoe ui" id="txtPurpose" readonly>' + purpose + '</textarea>';
    //    modal += '</div>';
    //    modal += '<div class="col-md-4">';
    //    modal += '<label id="lblcurrentFile" for="current" style="margin-top:9.5%;" >Uploaded File: </label>';
    //    modal += '<a id="file" href="" download ><label id="currentFile" class="text-success" style="margin-top:9.5%; cursor:pointer; font-weight:bold;"></label></a>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '<hr/>';

    //    modal += '<div class="row">';
    //    modal += '<div class="col-md-12">';
    //    modal += '<div class="jqxgrid jqxgrid-filterable jqxgrid-sortable jqxgrid-autoheight jqxgrid-enablehover jqxgrid-enableellipsis jqxgrid-pageable jqxgrid-virtualmode jqxgrid-pagesizeoptions jqxgrid-columnsresize jqxgrid-editable"';
    //    modal += 'id="grid_item_returnslip"';
    //    modal += 'grid-width="100"';
    //    modal += 'data-url="/Transactions/GetAll_Item_ReturnSlip?HeaderKey=' + returnslip_code + '"';
    //    modal += 'grid-pagesizeoptions="5,10,20,50,100"';

    //    modal += 'grid-hide-columns="0,1,2,3,4,6,10,12,14,15,16">';
    //    modal += '</div>';
    //    modal += '</div>';

    //    modal += '<div class="modal-footer">';

    //    modal += '<button type="button" class="btn btn-default"  style="margin-top:3%" data-dismiss="modal">Close</button>';
    //    modal += '</div>';

    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '</div>';
    //    modal += '</div>';

    //    $("#form_modal").html(modal);

    //    $("#form_modal_returnslip_details").modal("show");
    //    $("#form_modal_returnslip_details").css('z-index', '1000000');



    //    ini_main.element('inputtext');
    //    ini_main.element('inputnumber');
    //    ini_main.element('dropdownlist');
    //    initialize_jqxwidget_grid($("#grid_item_returnslip"));

    //$("#grid_item_returnslip").on('rowclick', function (event) {
    //    var args = event.args;
    //    // row's bound index.
    //    var boundIndex = args.rowindex;
    //    // row's visible index.
    //    var visibleIndex = args.visibleindex;
    //    // right click.
    //    var rightclick = args.rightclick;
    //    // original event.
    //    var ev = args.originalEvent;
    //    var rowID = $("#grid_item_returnslip").jqxGrid('getrowid', boundIndex);

    //    itemforreturn = $("#grid_item_returnslip").jqxGrid('getrowdatabyid', rowID);

    //});
    //},
    // End //


};

//$(document).delegate("#cmdTestNo", "click", function () {
//    var selectedrowindexes = $('#grid_item_returnslip').jqxGrid('selectedrowindexes');
//    var item_id_arr = [];

//    for (var ctr = 0; ctr < selectedrowindexes.length; ctr++) {
//        var rowID = $("#grid_item_returnslip").jqxGrid('getrowid', selectedrowindexes[ctr]);
//        itemforreturn = $("#grid_item_returnslip").jqxGrid('getrowdatabyid', rowID);
//        item_id_arr.push(itemforreturn.id_number);

//    }

//});




// Update Function for Draft Transaction //
$(document).delegate("#cmdDraftUpdate", "click", function () {
    var transdetails = ["#txtImpexRefNbr", "#txtPurpose", "#txtReturnDate", "#cmbTransType"];
    var ctr = 0;
    for (var i = 0; i <= 4; i++) {
        if ($(transdetails[i]).val() == "") {
            $(transdetails[i]).css("border-color", "red");

        }
        else {
            $(transdetails[i]).css("border-color", "#e5e5e5");
            ctr++;

        }
    }
    if (ctr == 5) {
        // dbaseOperations.save(draftId);       
        show_modal.confirmationModal_update('Update Draft', 'Are you sure you want to update this record?', 'Drafted');

    }

});

$(document).delegate("#cmdUpdateDraft", "click", function () {

    dbaseOperations.save(draftId);

});


var dbaseOperations = {
    save: function (draftId) {
        var formData = new FormData();
        formData.append('headerAttachment', (originalFileName.split('_').pop() == $("#currentFile").text()) ? originalFileName : $('input[name="headerAttachment"]')[0].files[0]);
        formData.append('id', draftId);
        formData.append('ImpexRefNbr', $("#txtImpexRefNbr").val());
        formData.append('ReturnDate', $("#txtReturnDate").val());
        formData.append('TransType', $("#cmbTransType").val());
        formData.append('Purpose', $("#txtPurpose").val());
        formData.append('OriginalFileName', originalFileName.split('_').pop());
        formData.append('CurrentFileName', $("#currentFile").text());
        //alert(draftId);
        var msg = 'Record successfully updated';

        $.ajax({
            url: '/Transactions/UpdateTransDraft',
            dataType: 'json',
            type: 'post',
            data: formData,
            //{
            //    id: draftId,
            //    ImpexRefNbr: $("#txtImpexRefNbr").val(),
            //    ReturnDate: $("#txtReturnDate").val(),
            //    TransType: $("#cmbTransType").val(),
            //    Purpose: $("#txtPurpose").val(),
            //},
            processData: false,
            contentType: false,
            beforeSend: function () {

            },
            headers: {
                //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {

                if (response.success) {

                    notification_modal("Update Record", msg, "success");

                    $('#btn_notifClose').click(function () {
                        //$('#notification_modal .close').click();
                        window.location.reload();
                    });

                } else {
                    notification_modal("Update Record", response.message, "danger");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });

    },

};
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

        // alert(unitofmeasure);
        $('#txtQuantity').val(quantity);
        $('#cmbUom').val(unitofmeasure);
        $('#txtSerialNbr').val(serial);
        $('#txtTagNbr').val(tagnumber);
        $('#txtPoNbr').val(ponumber);


        // $("#gridDetails").attr('data-url', $("#gridDetails").attr('data-url') + '?HeaderKey=' + id)
        // alert($("#gridDetails").attr('data-url'));
        // initialize_jqxwidget_grid($("#gridDetails"));

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
    // var data = $("#gridDetails").jqxGrid('getrowdatabyid', rowID);
    //datatable = $("#gridDetails").jqxGrid('getrowdatabyid', rowID);


    indexItemDetails = boundIndex;
});

// End //


// Inactive Add Item //
$(document).delegate(".gridDetails_Remove", 'click', function () {
    var rowID = $("#gridDetails").jqxGrid('getrowid', indexItemDetails);
    var data = $("#gridDetails").jqxGrid('getrowdatabyid', rowID);

    show_modal.deactivate(data.id_number);
    //alert(data.id_number);
});

// Inactive confirmation button //
$(document).delegate("#btnProceedDeactivate", "click", function () {
    dbase_operation_draft.deactivate($(this).attr('data-id'));

});
// End function //


// Inactive Transaction //
$(document).delegate(".gridInbox_Inactive", 'click', function () {
    var rowID = $("#gridInbox").jqxGrid('getrowid', indexAllTrans);
    var data = $("#gridInbox").jqxGrid('getrowdatabyid', rowID);



    show_modal.deactivateTransDraft(data.id_number);
    // alert(data.id_number);


});
$(document).delegate("#btnProceedDeactivateTransDraft", "click", function () {
    dbase_operation_draft.deactivate_transDraft($(this).attr('data-id'));

});

// End Function//

//Load Category and Type//
$(document).delegate("#cmbItem", 'change', function () {
    $.ajax({
        url: '/Items/GetItemDetailsByItemCode',
        dataType: 'json',
        type: 'get',
        data: {
            itemcode: $(this).val(),
        },
        beforeSend: function () {

        },
        headers: {
            //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        },
        success: function (response) {

            if (response.success) {
                // alert(response.message[0]["Type"]);
                $('#cmbItemType').val(response.message[0]["Type"]);
                $('#cmbCategory').val(response.message[0]["Category"]);
            } else {
                notification_modal("Addition failed!", response.message, "danger");
            }

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(thrownError);
        }
    });


});


// Search function //
$(document).delegate("#gridAllTrans_searchField", "keyup", function (e) {
    var columns = ["gate_pass_id_string", "purpose_string", "impex_ref_number_string",
                   "transaction_type_string"];
    generalSearch($('#gridAllTrans_searchField').val(), "gridAllTrans", columns, e);
});
// End//




// load supplier and conatact details dropdown
function iniSupplierContactpersonComboDrafts() {

    // prepare suppliers combo
    var source_supplier = {
        dataType: "json",
        dataFields: [
            { name: 'Name' },
            { name: 'Code' },
        ],
        root: 'message',
        url: '/Supplier/GetSuppliersForCombo',
    };
    var adapter_supplier = new $.jqx.dataAdapter(source_supplier);

    // create the supplier element
    $("#cmbSupplier").jqxDropDownList({
        theme: window.gridTheme,
        source: adapter_supplier,
        width: '95%',
        height: 20,
        promptText: "Select supplier...",
        displayMember: 'Name',
        valueMember: 'Code'
    });



    // prepare contact person combo
    var source_contactperson = {
        dataType: "json",
        dataFields: [
             { name: 'Name' },
            { name: 'Code' },
            { name: 'SupplierKey' },
        ],
        root: 'message',
        url: '/ContactPerson/GetContactPerson',
    };
    var adapter_contactperson = new $.jqx.dataAdapter(source_contactperson);
    // create an empty element for contact person
    $("#cmbContactPerson").jqxDropDownList({
        theme: window.gridTheme,


        source: adapter_contactperson,
        //  disabled: true,
        width: '91%',
        height: 20,
        promptText: "Select Contact Person...",
        displayMember: 'Name',
        valueMember: 'Code'
    });

    $("#cmdAddContact").prop('disabled', true);
    $("#addContactIcon").css('color', 'gray');

    // modify binding
    $("#cmbSupplier").bind('select', function (event) {
        if (event.args) {
            //$("#cmbContactPerson").jqxDropDownList({ disabled: false, selectedIndex: -1 });
            var value = event.args.item.value;

            // prepare contact person combo
            var source_contactperson = {
                dataType: "json",
                dataFields: [
                    { name: 'Name' },
                    { name: 'Code' },
                    { name: 'FK' },
                ],
                root: 'message',
                url: '/ContactPerson/GetContactPersonsForCombo?fk=' + value,
            };
            var adapter_contactperson = new $.jqx.dataAdapter(source_contactperson);

            // re-initialize the contact person combo
            $("#cmbContactPerson").jqxDropDownList({
                source: adapter_contactperson,
                autoDropDownHeight: adapter_contactperson.records.length > 10 ? false : true,
                // disabled: false,

                displayMember: 'Name',
                valueMember: 'Code',
                selectedIndex: -1
            });

            $("#cmdAddContact").prop('disabled', false);
            $("#addContactIcon").css('color', '#31B0D5');

        }
    });



    // add contact details for supplier
    $("#cmdAddContact").click(function () {
        showform.ContactPerson('add');
    });

    $(document).delegate("#cmdAddItemContactDrafts", "click", function () {
        dbaseOperations.SaveContactPerson('add', '');
    });

    var dbaseOperations = {
        SaveContactPerson: function (operation, trans_id) {


            //for validation if textbox is empty
            var elements = ["#FirstName", "#MiddleName", "#LastName", "#Email", "#ContactNumber", "#Department"]
            var ctr = 0;
            for (var i = 0; i <= 5; i++) {
                if ($(elements[i]).val() == "") {
                    $(elements[i]).css("border-color", "red");
                }
                else {
                    $(elements[i]).css("border-color", "#e5e5e5");
                    ctr++;
                }
            }

            //if all textbox is filled
            if (ctr == 6) {
                var url = '';
                var msg = '';
                //var IsActive = false;
                if (operation == 'add') {
                    url = '/ContactPerson/AddContactPerson';
                    msg = 'New record successfully added!';
                    title = 'Add Record';
                } else {
                    url = '/ContactPerson/UpdateContactPerson';
                    msg = 'Record successfully updated!';
                    title = 'Update Record';
                }
                //if ($("#chkIsActive").is(":checked")) {
                //    IsActive = true;
                //}
                $.ajax({
                    url: url,
                    dataType: 'json',
                    type: 'get',
                    data: {
                        id: trans_id,
                        Code: $("#Code").val(),
                        SupplierKey: $('#cmbSupplier').val(),
                        FirstName: $("#FirstName").val(),
                        MiddleName: $("#MiddleName").val(),
                        LastName: $("#LastName").val(),
                        Email: $("#Email").val(),
                        ContactNumber: $("#ContactNumber").val(),
                        Department: $("#Department").val(),
                        //IsActive: IsActive
                    },
                    beforeSend: function () {

                    },
                    headers: {
                        //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                    },
                    success: function (response) {
                        if (response.success) {
                            $("#modalContactPerson").modal("hide");
                            notification_modal(title, msg, "success");

                        } else {
                            notification_modal("Addition failed!", response.message, "danger");
                        }

                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.status);
                        console.log(thrownError);
                    }
                });
            }
        },
    };

    // add contact details modal
    var showform = {

        ContactPerson: function (operation, boundIndex) {
            var id = '';
            var firstname = '';
            var middlename = '';
            var lastname = '';
            var email = '';
            var contactnumber = '';
            var department = '';
            //var IsActive;
            var code;

            if (operation != "add") {

                var rowID = $("#ContactPersonGrid").jqxGrid('getrowid', boundIndex);
                var data = $("#ContactPersonGrid").jqxGrid('getrowdatabyid', rowID);

                id = data.id_number;
                firstname = data.First_Name_string;
                middlename = data.Middle_Name_string;
                lastname = data.Last_Name_string;
                email = data.Email_string;
                contactnumber = data.Contact_Number_string;
                department = data.Department_string;
                code = data.code_string;
            }

            var modal2 = '<div class="modal fade" id="modalContactPerson" role="dialog" >';
            modal2 += '<div class="modal-dialog">';
            modal2 += ' <div class="modal-content">';

            modal2 += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
            modal2 += '<h4 class="modal-title">Add Contact Details</h4>';
            modal2 += '</div>';
            modal2 += '<div class="modal-body" style="margin-top:3%;">';

            modal2 += '<div class="row">';
            modal2 += '  <input id="Code" type="hidden" value="' + code + '" />';
            modal2 += '  <div class="col-md-12">';
            modal2 += '      <div class="col-md-4">';
            modal2 += '          <div class="form-group">';
            modal2 += '    <label for="cmbUom">Last Name:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
            modal2 += '              <input id="LastName" type="text" placeholder="Last Name" class="form-control companyrequired" style="font-size:12px; font-family: segoe ui"/>';
            modal2 += '          </div>';
            modal2 += '      </div>';
            modal2 += '      <div class="col-md-4">';
            modal2 += '          <div class="form-group">';
            modal2 += '    <label for="cmbUom">First Name:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
            modal2 += '              <input id="FirstName" type="text" placeholder="First Name" class="form-control companyrequired" style="font-size:12px; font-family: segoe ui"/>';
            modal2 += '          </div>';
            modal2 += '      </div>';
            modal2 += '      <div class="col-md-4">';
            modal2 += '          <div class="form-group">';
            modal2 += '    <label for="cmbUom">Middle Name:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
            modal2 += '              <input id="MiddleName" type="text" placeholder="Middle Name" class="form-control companyrequired" style="font-size:12px; font-family: segoe ui"/>';
            modal2 += '          </div>';
            modal2 += '      </div>';
            modal2 += '  </div>';
            modal2 += '</div>';

            modal2 += '<div class="row">';
            modal2 += '  <div class="col-md-12">';
            modal2 += '      <div class="col-md-4">';
            modal2 += '          <div class="form-group">';
            modal2 += '    <label for="cmbUom">Email Address:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
            modal2 += '              <input id="Email" type="text" placeholder="Email Address" class="form-control companyrequired" style="font-size:12px; font-family: segoe ui"/>';
            modal2 += '          </div>';
            modal2 += '      </div>';
            modal2 += '      <div class="col-md-4">';
            modal2 += '          <div class="form-group">';
            modal2 += '    <label for="cmbUom">Contact Number:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
            modal2 += '              <input id="ContactNumber" type="number" placeholder="Contact Number" class="form-control companyrequired" style="font-size:12px; font-family: segoe ui"/>';
            modal2 += '          </div>';
            modal2 += '      </div>';
            modal2 += '      <div class="col-md-4">';
            modal2 += '          <div class="form-group">';
            modal2 += '    <label for="cmbUom">Department:</label>';
            modal2 += '              <input id="Department" type="text" placeholder="Department" class="form-control companyrequired"  style="font-size:12px; font-family: segoe ui"/>';
            modal2 += '          </div>';
            modal2 += '      </div>';
            modal2 += '  </div>';
            modal2 += '</div>';


            modal2 += '<div class="modal-footer">';
            modal2 += '<div class="row">';
            modal2 += '<a class="btn btn-meduim btn-blue" data-recid="' + id + '" id="' + ((operation == 'add') ? 'cmdAddItemContactDrafts' : 'cmdUpdateContactPerson') + '">';
            // modal2 += 'style="width: 100px;">';
            modal2 += 'Add</a>';
            modal2 += '<a type="button" class="btn btn-meduim btn-gray" data-dismiss="modal">Cancel</a>';

            modal2 += '</div>';
            modal2 += '</div>';

            modal2 += '</div>';

            modal2 += '</div>';
            modal2 += '</div>';
            modal2 += '</div>';

            $("#form_modal").html(modal2);
            $("#modalContactPerson").modal("show");
            $("#modalContactPerson").css('z-index', '1000000');
        }
    };
}

