var indexItemDetailsEdit;
var draftId;
var datatable, datatableContact, indexAllTrans;
var itemreturn, headerdetails_for_RS;
var itemforreturn;
var arrayitemforreturn = new Array;
var selectedrowindexes = new Array;
var imagesrc, returnslip_usercode;
var fileName;
var originalFileName;
var contactpersonCode, contactpersonName, supplier, ctrSupplierDropdown = 0;
var snackbarDraftHTMLText;
var Global_IDAddedItem;
var draftTransId_number;

function clear() {
    $('#txtPurpose').val("");
    $('#txtImpexRefNbr').val("");
    $('#cmbTransType').val("IN");
    $('#txtReturnDate').val("");
    $('#headerAttachment').val("");
};

function DownloadAttachment() {
    if (originalFileName == "" || originalFileName == null) {
        $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'Uploaded file is empty.');
        Warning_ToastNotif();
    }
    else {
        window.location.href = '/Attachment/DownloadAttachment?filename=' + encodeURIComponent(originalFileName) + '&path=' + env;
    }
   
}

$(document).delegate("#cmdAddNewItem", "click", function () {
    showform.item('add');
});

$(document).delegate("#cmdAddNewItemEdit", "click", function () {
    $("#form_modal_divEdit").css('z-index', '100000');
    showform.item('edit');
});

$(document).delegate("#btnSaveItem", "click", function () {
    dbase_operation.addItem($(this).attr("data-operation"));
});

$(document).delegate("#btnCancel", "click", function () {
    $("#form_modal_div").css('z-index', '100000001');
    $("#form_modal_divEdit").css('z-index', '100000001');
});

window.reset = function (e) {
    e.wrap('<form>').closest('form').get(0).reset();
    e.unwrap();
}

$(document).delegate("#removeAttachment", "click", function () {
    $("#currentFile").text("No file chosen");
});

$(document).delegate("#removeAttachmentEdit", "click", function () {
    $("#lblCurrentFilename").text("No file chosen");
});

// for header file attachment
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
    ini_main.element('dropdownlistApprover');
    iniSupplierContactpersonComboDrafts();



    // desc: cmbtranstype dropdown properties and customsise
    $("#cmbTransType").jqxDropDownList({
        theme: window.gridTheme,
        width: '88%',
        height: 20,
        dropDownHeight: 50,
        promptText: "Select Transaction Type...",
    });


    // back button
    $("#cmdDraftsBack").click(function () {

        $("#gridDetails").attr('data-url', '/Transactions/GetTransDetails');
        $('#bodyDraftsDetails').slideUp('slow');
        $('#bodyTransDrafts').slideDown('slow');      
        clear();
        $('#txtImpexRefNbr').css("border-color", "#e5e5e5");
        $('#txtPurpose').css("border-color", "#e5e5e5");
        $('#txtReturnDate').css("border-color", "#e5e5e5");
        $('#cmbTransType').css("border-color", "#e5e5e5");
        $('#cmbDeptApprover').css("border-color", "#e5e5e5");
        $('#cmdDraftsBack').hide();
        ini_main.element('dropdownlistApprover');
        ctrSupplierDropdown = 0;

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

    //rherejias, file extension and special character validation
    //for header attachment
    $('#headerAttachment').change(function () {
        var val = $(this).val().toLowerCase();
        var regexChar = new RegExp("\#|\&|,|;|\\+");
        var regex = new RegExp(ext);
        if (!(regex.test(val))) {
            notification_modal('Error', 'File type not supported.</br></br><b>Supported file type:</b> '+ extMsg +'', 'danger');
            $(this).val('');
            $("#currentFile").text('No file choosen');
        }
        else {
            if ((regexChar.test(val))) {
                notification_modal('Error', 'File name contains special characters. Please rename your file and try again.</br></br><b>Special characters:</b> # & ; , + \ / * ? : < > | ', 'danger');
                $(this).val('');
                $("#currentFile").text('No file choosen');
            }
        }
    });

    //rherejias, file extension and special character validation
    //for image attachment
    $(document).delegate("#inpt_file", "change", function () {
        var val = $(this).val().toLowerCase();
        var regexChar = new RegExp("\#|\&|,|;|\\+");
        var regex = new RegExp(extImg);
        if (!(regex.test(val))) {
            notification_modal('Error', 'File type not supported.</br></br><b>Supported file type:</b> '+ extMsgImg +'', 'danger');
            $("#form_modal_div").css('z-index', '10000');
            $(this).val('');
        }
        else {
            if ((regexChar.test(val))) {
                notification_modal('Error', 'File name contains special characters. Please rename your file and try again.</br></br><b>Special characters:</b> # & ; , + \ / * ? : < > | ', 'danger');
                $("#form_modal_div").css('z-index', '10000');
                $(this).val('');
            }
        }
    });

    //rherejias, file extension and special character validation
    //for image attachment
    $(document).delegate("#inpt_fileEdit", "change", function () {
        var val = $(this).val().toLowerCase();
        var regexChar = new RegExp("\#|\&|,|;|\\+");
        var regex = new RegExp(extImg);
        if (!(regex.test(val))) {
            notification_modal('Error', 'File type not supported.</br></br><b>Supported file type:</b> '+ extMsgImg +'', 'danger');
            $("#form_modal_divEdit").css('z-index', '10000');
            $(this).val('');
        }
        else {
            if ((regexChar.test(val))) {
                notification_modal('Error', 'File name contains special characters. Please rename your file and try again.</br></br><b>Special characters:</b> # & ; , + \ / * ? : < > | ', 'danger');
                $("#form_modal_divEdit").css('z-index', '10000');
                $(this).val('');
            }
        }
    });

    $(document).delegate("#btn_notifClose", "click", function () {
        $("#form_modal_div").css('z-index', '10000000001');
        $("#form_modal_divEdit").css('z-index', '10000000001');
    });

});


$("#cmdSaveAndSubmit").click(function () {

        var returnDate = Date.parse($('#txtReturnDate').val());
        var dateToday = new Date();
        var datainformations = $('#gridDetails').jqxGrid('getdatainformation');
        var rowscounts = datainformations.rowscount;
                           
            var transdetails2 = ["#cmbSupplier", "#txtPurpose", "#cmbTransType", "#cmbDeptApprover"];
            var ctr = 0;
            for (var i = 0; i <= 3; i++) {
                if ($(transdetails2[i]).val() == "") {
                    $(transdetails2[i]).css("border-color", "#f98989");
                    $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'Fields with red color are required to proceed!');
                    Warning_ToastNotif();

                }
                else {
                    $(transdetails2[i]).css("border-color", "#e5e5e5");
                    ctr++;

                }
            }
         if (ctr == 4) {


                if (rowscounts != 0) {

                    if ($("#headerAttachment").val() != "") {
                        if ($('input[name="headerAttachment"]')[0].files[0].size > fileSize) {
                            toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " File cannot be more then 200MB.";
                            Fail_ToastNotif();
                        }
                        else {

                            Edit.isitemreturnable();

                        }
                    }
                    else {
                        
                        Edit.isitemreturnable();

                    }

                }
                else {
                    $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'Please add an item to submit your Gate Pass!');
                    Warning_ToastNotif();
                }

            

        }
    });

    $("#cmdSaveAsDraft").click(function () {    

        var transdetails = ["#txtImpexRefNbr", "#txtPurpose", "#txtReturnDate", "#cmbTransType", "#cmbDeptApprover"];
        var ctr = 0;
        for (var i = 0; i <= 5; i++) {
            if ($(transdetails[i]).val() == "") {
                $(transdetails[i]).css("border-color", "#f98989");

            }
            else {
                $(transdetails[i]).css("border-color", "#e5e5e5");
                ctr++;

            }
        }
        if (ctr == 6) {
            show_modal.confirmationModal('Submit Draft', 'Are you sure you want to submit this draft?', 'Drafted');
        }
    });
    $("#cmdAddItem").click(function () {
        show_modal.addNewItem((typeof $(this).attr("data-transid") == "undefined" ? "" : $(this).attr("data-transid")));

    });




//adding a new transaction or submitting a draft
$(document).delegate("#cmdProceedSaveAndSubmit", "click", function () { 

    var contactCodeTobeSave_Submit;
    if ($("#cmbSupplier").val() == supplier && $("#cmbContactPerson").val() == "" && ctrSupplierDropdown <= 1) {

        contactCodeTobeSave_Submit = contactpersonCode;
    } else {
        contactCodeTobeSave_Submit = $("#cmbContactPerson").val();
    }

    dbase_operation.addHeader($(this).attr("data-transid"), $(this).attr("data-transstat"), contactCodeTobeSave_Submit);

});

$(document).delegate(".gridDrafts_Edit", 'click', function () {
    Edit.getEdit(indexAllTrans);
    $('#bodyDraftsDetails').slideDown('slow');
    $('#bodyTransDrafts').slideUp('slow');
    $('#cmdDraftsBack').css('display', 'unset');

});
var draftId;


var Edit = {


    isitemreturnable: function () {

        $.ajax({
            url: '/Drafts/HasItemreturnable',
            dataType: 'json',
            type: 'get',
            data: { draftId: draftTransId_number },

            beforeSend: function () {

            },
            success: function (response) {

                if (response.success) {

                    if (response.message != 0) {

                        if ($('#txtReturnDate').val() == "") {
                            $('#txtReturnDate').css("border-color", "#f98989");
                            $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'Return Date is required to all returnable item.');
                            Warning_ToastNotif();

                        }
                        else {
                            $('#txtReturnDate').css("border-color", "#e5e5e5");
                            show_modal.confirmationModal('Confirmation', 'Are you sure you want to submit this record for approval?', 'Submitted');

                        }


                    } else {

                        $('#txtReturnDate').val("");
                        $('#txtReturnDate').css("border-color", "#e5e5e5");
                        show_modal.confirmationModal('Confirmation', 'Are you sure you want to submit this record for approval?', 'Submitted');

                    }

                }

            },

            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },




    getEdit: function (index) {
   
        
        var rowID = $("#gridDrafts").jqxGrid('getrowid', index);
        var data = $("#gridDrafts").jqxGrid('getrowdatabyid', rowID);
        var id = data.id_number;
        var code = data.code_string;
        var impex = data.impex_ref_number_string;
        supplier = data.supplier_string;
        contactpersonCode = data.contact_person_string;
        var purpose = data.purpose_string;
        var returndate = data.return_date_date;     
        var transtype = data.transaction_type_string;
        var approvercode = data.dept_approver_code_string;

         var attachment = "";
         if (data.attachment_string == null || data.attachment_string == "") {
             attachment = "";
         }
         else {
             attachment = data.attachment_string.substring(data.attachment_string.indexOf('_') + 1);
         }
        contactpersonName = data.contact_name_string;
        originalFileName = data.attachment_string;
        fileName = attachment;
        

        $('#currentFile').removeAttr('hidden');
        $('#lblcurrentFile').removeAttr('hidden');
        $('#removeAttachment').css('display', 'inline');
        $('#txtPurpose').val(purpose);
        $('#txtImpexRefNbr').val(impex);        
        $('#cmbSupplier').jqxDropDownList('val', supplier);
        $('#cmbDeptApprover').jqxDropDownList('val', approvercode);
        $('#cmbTransType').val(transtype);

        if (returndate == "" || returndate == null) {

            $('#txtReturnDate').val(returndate);
        } else {
            $('#txtReturnDate').val(returndate.toISOString().slice(0, 10));
        }

       
        if (attachment == "" || attachment == null)
        {
            $('#currentFile').text("No file chosen");
            //$('#removeAttachment').hide();
        }
        else
        {
            $('#removeAttachment').show(); $('#currentFile').text(attachment);
        }

        //$('#file').attr('href', attachmentPath + originalFileName);
        $("#cmdAddItem").attr("data-transid", id);
        $("#gridDetails").attr('data-url', $("#gridDetails").attr('data-url') + '?HeaderKey=' + id);

        //initialize grid
        initialize_jqxwidget_grid($("#gridDetails"));
        draftId = id;

        // re-initialize the contact person combo
        $("#cmbContactPerson").jqxDropDownList({ placeHolder: contactpersonName == null ? 'Select Contact Person' : contactpersonName });
        draftTransId_number = id; //set the draft id for

    }
};

$("#gridDrafts").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    indexAllTrans = boundIndex;
});
// end function //




// AddItem //
$(document).delegate("#cmdSaveItem", 'click', function () {


    var ext = $("#inpt_file").val().split('.').pop();
    if (ext == "png" || ext == "jpg" || ext == "PNG" || ext == "JPG" || ext == "jpeg" || ext == "JPEG" || $("#inpt_file").val() == "") {
        if ($("#inpt_file").val() != "") {
            if ($('input[name="inpt_file"]')[0].files[0].size > fileSize) {
                toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " File cannot be more then 200MB.";
                Fail_ToastNotif();
            }
            else {
               dbase_operation_Item.addAddItems($(this).attr("data-transid"));
            }
        }
        else {
            dbase_operation_Item.addAddItems($(this).attr("data-transid"));
        }
    }
    else {

        $("#form_modal_div").css('z-index', '10000');
        notification_modal("Warning", "File extension not supported!", "danger");
        $('#btn_notifClose').click(function () {
            $('#notification_modal .close').click();
            $("#form_modal_div").css('z-index', '1000000000000000');
        });
    }
});

 
var dbase_operation_Item = {

    // show add item modal
    addAddItems: function (parentId) {
        var formData = new FormData();
        formData.append('inpt_file', $('input[name="inpt_file"]')[0].files[0]);
        formData.append('id', parentId);
        formData.append('Quantity', $("#txtQuantity").val());
        formData.append('UnitOfMeasureCode', $("#cmbUom").val());
        formData.append('ItemCode', $('#cmbItem').val());
        formData.append('CategoryCode', $('#cmbCategory').val());
        formData.append('ItemTypeCode', $('#cmbItemType').val());
        formData.append('SerialNbr', $("#txtSerialNbr").val());
        formData.append('TagNbr', $("#txtTagNbr").val());
        formData.append('PONbr', $("#txtPoNbr").val());
        formData.append('IsActive', true);
        formData.append('Remarks', $("#txtItemRemarks").val());
        formData.append('Image', $("#inpt_file").val());
        var itemdetails = ["#txtQuantity", "#cmbUom", "#cmbItem", "#cmbItemType", "#cmbCategory"];
        var ctr = 0;
        for (var i = 0; i <= 4; i++) {
            if ($(itemdetails[i]).val() == "") {
                $(itemdetails[i]).css("border-color", "#f98989");
                $('#required').show('slow');
            }
            else {
                $(itemdetails[i]).css("border-color", "#e5e5e5");
                ctr++;
            }
        }

        if (ctr == 5) {
            $('#required').hide('slow');
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

                },
                success: function (response) {

                    
                    if (response.success) {

                        $("#form_modal_div").modal("hide");
                        $("#gridDetails").jqxGrid('updatebounddata');
                    } else {
                        ///asd

                        $("#form_modal_div").css('z-index', '100000');
                        notification_modal("Warning", "Item already exist with the same serial number!", "danger");

                        $('#btn_notifClose').click(function () {
                            $('#notification_modal .close').click();
                            $("#form_modal_div").css('z-index', '1000000');
                        });

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
     
            },
            success: function (response) {
                if (response.success) {
                    notification_modal("Add Record", response.message, "success");

                    $('#btn_notifClose').click(function () {
                        $('#notification_modal .close').click();
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

    // add header details as draft
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


                toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Draft Successfully Deleted";
                $('#gridDrafts').jqxGrid('updatebounddata');
                Save_ToastNotif();
            },

            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },
    // End Function //

    UpdateAddedItems: function (parentId) {
        var formData = new FormData();
        formData.append('AddedItem_ID', Global_IDAddedItem);        
        formData.append('inpt_fileEdit', $('input[name="inpt_fileEdit"]')[0].files[0]);
        formData.append('IdEdit', parentId);
        formData.append('QuantityEdit', $("#txtQuantityEdit").val());
        formData.append('UnitOfMeasureCodeEdit', $("#cmbUomEdit").val());
        formData.append('ItemCodeEdit', $('#cmbItemEdit').val());
        formData.append('CategoryCodeEdit', $('#cmbCategoryEdit').val());
        formData.append('ItemTypeCodeEdit', $('#cmbItemTypeEdit').val());
        formData.append('SerialNbrEdit', $("#txtSerialNbrEdit").val());
        formData.append('TagNbrEdit', $("#txtTagNbrEdit").val());
        formData.append('PONbrEdit', $("#txtPoNbrEdit").val());
        formData.append('IsActiveEdit', true);
        formData.append('RemarksEdit', $("#txtItemRemarksEdit").val());
        if ($("#inpt_fileEdit").val() == '' && $("#lblCurrentFilename").text() == 'No file chosen') {
            formData.append('ImageEdit', '');
        }
        else if ($("#inpt_fileEdit").val() == '' && $("#lblCurrentFilename").text() != 'No file chosen') {
            formData.append('ImageEdit', origImageName);
        }
        else if ($("#inpt_fileEdit").val() != '') {
            formData.append('ImageEdit', $("#inpt_fileEdit").val());
        }
        var itemdetails = ["#txtQuantityEdit", "#cmbUomEdit", "#cmbItemEdit"];
        var ctr = 0;
        for (var i = 0; i <= 2; i++) {
            if ($(itemdetails[i]).val() == "") {
                $(itemdetails[i]).css("border-color", "#f98989");
            }
            else {
                $(itemdetails[i]).css("border-color", "#e5e5e5");
                ctr++;
            }
        }

        if (ctr == 3) {
            $.ajax({
                url: '/Drafts/UpdateItemsDetailsDraft',
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

                        $("#form_modal_divEdit").modal("hide");
                        $("#gridDetails").jqxGrid('updatebounddata');
                    } else {
                        $("#form_modal_divEdit").css('z-index', '100000');
                        notification_modal("Warning", "Item already exist with the same serial number!", "danger");

                        $('#btn_notifClose').click(function () {
                            $('#notification_modal .close').click();
                            $("#form_modal_divEdit").css('z-index', '1000000');
                        });

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
    addItem: function (data_operation) {
        var elements = ["#name", "#description", "#cmbCategoryItem", "#cmbType", "#cmbDepartment", "#cmbUOM"]
        var ctr = 0;
        for (var i = 0; i <= 5; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "red");
                $('#requireditemdetails').show('slow');
                }
            else {
                $(elements[i]).css("border-color", "#e5e5e5");
                ctr++;
             }
        }

        if (ctr == 6) {
            $('#requireditemdetails').hide('slow');
                var msg = ' New item created successfully.';
                $.ajax({
            url: '/Items/AddItems',
                dataType: 'json',
                type: 'get',
                data: {
            name: $("#name").val(),
            description: $("#description").val(),
                supplier: $("#cmbSupplier").val(),
                category: $("#cmbCategoryItem").val(),
                type: $("#cmbType").val(),
            department: $("#cmbDepartment").val(),
                uom: $("#cmbUOM").val()
            },
                beforeSend: function() {

                },
            headers: { 
                    },
            success: function (response) {
                if (response.success) {
                    if (data_operation == 'add') {
                        $("#modalitemmaster").modal("hide");
                        ini_main.element('dropdownlist');
                        $("#form_modal_div").css('z-index', '100000001');
                        $("#cmbItem").on('bindingComplete', function (event) {
                            $('#cmbItem').jqxDropDownList('val', response.message[0]["Code"]);
                        });
                        $('#notiIdSuccess').fadeIn().delay(5000).fadeOut();
                    }
                    else {
                        $("#modalitemmaster").modal("hide");
                        ini_main.element('dropdownlist');
                        $("#form_modal_divEdit").css('z-index', '100000001');
                        $("#cmbItemEdit").on('bindingComplete', function (event) {
                            $('#cmbItemEdit').jqxDropDownList('val', response.message[0]["Code"]);
                        });
                        $('#notiIdSuccess').fadeIn().delay(5000).fadeOut();
                    }
                } else {
                    $("#NotiId").removeAttr("hidden");
                    $("#NotiId").text(response.message);
                    $("#name").css("border-color", "red");
                }

            },
                 error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status);
                    console.log(thrownError);
                }
                });
         }
   },
    addHeader: function (transactionId, stat, contactCodeTobeSave_Submit) {
        var formData = new FormData();
        formData.append('inpt_file', (originalFileName.split('_').pop() == $("#currentFile").text()) ? originalFileName: $('input[name="headerAttachment"]')[0].files[0]);
        formData.append('Id', transactionId);
        formData.append('ImpexRefNbr', $("#txtImpexRefNbr").val());
        formData.append('DepartmentCode', '');
        formData.append('ReturnDate', $("#txtReturnDate").val());
        formData.append('TransType', $("#cmbTransType").val());
        formData.append('Supplier', $("#cmbSupplier").val());
        formData.append('ContactPerson', contactCodeTobeSave_Submit);
        formData.append('CategoryCode', '');
        formData.append('TypeCode', '');
        formData.append('Purpose', $("#txtPurpose").val());
        formData.append('IsActive', true);
        formData.append('Status', stat);
        formData.append('OriginalFileName', originalFileName.split('_').pop());
        formData.append('CurrentFileName', $("#currentFile").text());
        formData.append('ApproverCode', $("#cmbDeptApprover").val());
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

                ctrSupplierDropdown = 0;

                if (response.success) {
                    toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Your Gate Pass was succesfully submitted!";
                    $("#gridDetails").jqxGrid('updatebounddata');
                    $("#cmdDraftsBack").trigger("click");                 
                    $("#gridDrafts").jqxGrid('updatebounddata');
                    Save_ToastNotif();

                } else {
                    notification_modal("Add Record", response.message, "danger");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {

                ctrSupplierDropdown = 0;
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
        modal += '<img height="auto" width="100%" src="' + source + '"/>';
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
        modal += '<h4 class="modal-title"><i class="fa fa-question-circle fa-lg" aria-hidden="true"></i>&nbsp;Confirmation</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to remove this item?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-small btn-red" id="btnProceedDeactivate"';
        modal += 'data-id="' + Itemid + '">';
        modal += 'YES</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal">NO</a> &nbsp';
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
                    modal += '<h4 class="modal-title"><i class="fa fa-question-circle fa-lg" aria-hidden="true"></i>&nbsp;Confirmation</h4>';
                    modal += '</div>';
                    modal += '<div class="modal-body" style="margin-top:4%">';
                    modal += '<p>Are you sure you want to delete this draft?</p>';
                    modal += '</div>';
                    modal += '<div class="modal-footer">';
                       // modal += '<div class="row">';
                    modal += '<a class="btn btn-small btn-red" data-dismiss="modal" id="btnProceedDeactivateTransDraft"';
                        modal += 'data-id="' + Itemid + '">';
                        modal += 'YES</a>';
                        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal">NO</a>';
                       // modal += '</div>';
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
        modal += '<h4 class="modal-title" id="myModalLabel"><i class="fa fa-question-circle fa-lg" aria-hidden="true"></i>&nbsp;' + title + '</h4>';
        modal += '</div>';
        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += message;
        modal += '</div>';
        modal += '<div class="modal-footer">';
        modal += '<a class="btn btn-small btn-blue" data-dismiss="modal" data-transid="' + trans_id + '" data-transstat="' + status + '" id="cmdProceedSaveAndSubmit">YES</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal">NO</a>';
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
        modal += '<h4 class="modal-title" id="myModalLabel"><i class="fa fa-question-circle fa-lg" aria-hidden="true"></i>&nbsp;' + title + '</h4>';
        modal += '</div>';
        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += message;
        modal += '</div>';
        modal += '<div class="modal-footer">';
        modal += '<a class="btn btn-small btn-blue" data-dismiss="modal" id="cmdUpdateDraft">YES</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal">NO</a>';
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
        modal += '<div class="input-group" style="float:right">';
        modal += '<div class="form-control item dropdownlist" id="cmbItem" data-url="/Items/GetItems?IsActive=1" data-display="name_string" data-value="code_string" data-selectedindex="-1"></div>';
        modal += '<span class="input-group-btn">';
        modal += '<button class="btn btn-default" type="button" id="cmdAddNewItem" style="margin-left:4px; width:95%; border-color:#DCE1E4; background-color:#EEEEEE"><i class="fa fa-plus faa-tada animated-hover" style="color: #31B0D5" id="addContactIcon"></i></button>';
        modal += '</span>';
        modal += '</div>';
        modal += '</div>';

        modal += '  <div class="form-group col-md-3">';
        modal += '    <label for="txtQuantity">Quantity:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
        modal += '    <input type="number" class="form-control" id="txtQuantity" style="font-size:12px; font-family: segoe ui" onkeypress="return isNumeric(event)"  oninput="maxLengthCheck(this)" maxlength = "11" min = "1" max = "99999999999">';
        modal += '  </div>';


        modal += '<div class="form-group col-md-3">';
        modal += '  <label for="txtSerialNbr">Serial Number:</label>';
        modal += '  <input type="text" class="form-control" id="txtSerialNbr" style="font-size:12px; font-family: segoe ui" maxlength="50">';
        modal += '</div>';
        modal += '  </div>';
        modal += '<div class="col-md-12">';
        modal += '  <div class="form-group col-md-3">';
        modal += '  <label for="cmbItemType">Type:</label>';
        modal += '    <div class="form-control dropdownlist" id="cmbItemType" data-url="/Items/GetItemTypes?IsActive=1" data-display="name_string" data-value="code_string"></div>';
        modal += '  <input type="text" class="form-control" id="cmbItemTypeDisplay" style="font-size:12px; font-family: segoe ui; background-color:transparent; display:none" maxlength="50" disabled>';
        modal += '  </div>';


        modal += '  <div class="form-group col-md-3">';
        modal += '    <label for="cmbCategory">Category:</label>';
        modal += '    <div class="form-control dropdownlist" id="cmbCategory" data-url="/Items/GetItemCategories?IsActive=1" data-display="name_string" data-value="code_string"></div>';
        modal += '  <input type="text" class="form-control" id="cmbCategoryDisplay" style="font-size:12px; font-family: segoe ui; background-color:transparent;display:none" maxlength="50" disabled>';
        modal += '  </div>';

       
        modal += '  <div class="form-group col-md-3">';
        modal += '    <label for="txtPoNbr">PO Number:</label>';
        modal += '    <input type="text" class="form-control" id="txtPoNbr" style="font-size:12px; font-family: segoe ui" maxlength="50">';
        modal += '  </div>';

        modal += '  <div class="form-group col-md-3">';
        modal += '    <label for="txtTagNbr">Tag Number:</label>';
        modal += '    <input type="text" class="form-control" id="txtTagNbr" style="font-size:12px; font-family: segoe ui" maxlength="50">';
        modal += '  </div>';


        modal += '</div>';
        modal += '<div class="col-md-12">';
        modal += '  <div class="form-group col-md-3">';
        modal += '    <label for="cmbUom">Unit of Measure:<span class="asterisc" style="color:red;font-size:12px"></span></label>';
        modal += '    <div class="form-control dropdownlist" id="cmbUom" data-url="/Items/GetUnitOfMeasures?IsActive=1" data-display="name_string" data-value="code_string"></div>';
        modal += '  <input type="text" class="form-control" id="cmbUomDisplay" style="font-size:12px; font-family: segoe ui; background-color:transparent;display:none" maxlength="50" disabled>';
        modal += '  </div>';
        modal += '<div class="form-group col-md-3">';
        modal += '<label for="txtItemRemarks">Remarks:</label>';
        modal += '<input type="text" class="form-control" id="txtItemRemarks" style="font-size:12px; font-family: segoe ui" maxlength="255">';
        modal += '</div>';
        modal += '<div class="form-group col-md-6">';
        modal += '<label for="inpt_file">Upload Image:</label>';
        modal += '<div><input type="file" accept="image/jpg, image/jpeg, image/png" id="inpt_file" name="inpt_file" style="color:#F25656; font-size:1.1em; font-size:12px; font-family: segoe ui" /></div>';
        //modal += '<label style="float:left; color:red;">File name can\'t contain special characters.</label>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';

        modal += '  <div class="col-md-12">';
        modal += '<p id="required" hidden style=font-family:arial;color:red>Fields with red color are required to proceed.</p>';
        modal += '  </div>';

        modal += '</div>';

       


        modal += '<div class="modal-footer">';
        modal += '<label class="text-success" id="notiIdSuccess" style="float:left; margin-left:2%; display: none;" >Additional item successfully added.</label>';
        modal += '<a class="btn btn-meduim btn-blue" data-transid="' + transid + '" id="cmdSaveItem">Add</a>';
        modal += '<a class="btn btn-meduim btn-gray" data-dismiss="modal" id="cmdCancel">Cancel</a>';
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

        $("#cmbUom").jqxDropDownList({ width: '165px', });
        $("#cmbItemType").jqxDropDownList({ width: '165px' });
        $("#cmbCategory").jqxDropDownList({ width: '165px' });


        $("#cmbItemType").jqxDropDownList({
            dropDownHeight: 50,
            filterable: false,
        });


    },

};



// Update Function for Draft Transaction //
$(document).delegate("#cmdDraftUpdate", "click", function () {
    var transdetails = ["#cmbSupplier", "#txtPurpose", "#cmbTransType", "#cmbDeptApprover"];
    var ctr = 0;
    for (var i = 0; i <= 5; i++) {
        if ($(transdetails[i]).val() == "") {
            $(transdetails[i]).css("border-color", "#f98989");
            $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'Fields with red color are required to proceed!');
            Warning_ToastNotif();

        }
        else {
            $(transdetails[i]).css("border-color", "#e5e5e5");
            ctr++;

        }
    }
    if (ctr == 6) {      
        if ($("#headerAttachment").val() != "") {
            if ($('input[name="headerAttachment"]')[0].files[0].size > fileSize) {
                toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " File cannot be more then 200MB.";
                Fail_ToastNotif();
            }
            else {
               show_modal.confirmationModal_update('Confirmation', 'Are you sure you want to update this draft?', 'Drafted');

            }
        }
        else {
            show_modal.confirmationModal_update('Confirmation', 'Are you sure you want to update this draft?', 'Drafted');

        }

    }

});

$(document).delegate("#cmdUpdateDraft", "click", function () {


    var contactCodeTobeSave;

    if ($("#cmbSupplier").val() == supplier && $("#cmbContactPerson").val() == "" &&  ctrSupplierDropdown <= 1) {

        contactCodeTobeSave = contactpersonCode;
    } else {
        contactCodeTobeSave = $("#cmbContactPerson").val();
    }

    dbaseOperationsDraft.saveasdraft(draftId, contactCodeTobeSave);
   
});

var dbaseOperationsDraft = {

    saveasdraft: function (draftId, tobesavecode) {
        var formData = new FormData();
        formData.append('headerAttachment', (originalFileName.split('_').pop() == $("#currentFile").text()) ? originalFileName : $('input[name="headerAttachment"]')[0].files[0]);
        formData.append('id', draftId);
        formData.append('ImpexRefNbr', $("#txtImpexRefNbr").val());
        formData.append('ReturnDate', $("#txtReturnDate").val());
        formData.append('TransType', $("#cmbTransType").val());
        formData.append('Supplier', $("#cmbSupplier").val());
        formData.append('ContactPerson', tobesavecode);
        formData.append('Purpose', $("#txtPurpose").val());
        formData.append('OriginalFileName', originalFileName.split('_').pop());
        formData.append('CurrentFileName', $("#currentFile").text());
        formData.append('ApproverCode', $("#cmbDeptApprover").val());

        $.ajax({
            url: '/Transactions/UpdateTransDraft',
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

                ctrSupplierDropdown = 0;

                if (response.success) {
                    toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Record was successfully updated!";                  
                    $("#gridDetails").jqxGrid('updatebounddata');
                    $("#cmdDraftsBack").trigger("click");                    
                    $("#gridDrafts").jqxGrid('updatebounddata');
                    Save_ToastNotif();

                } else {
                    notification_modal("Update Record", response.message, "danger");
                }


                              

            },
            error: function (xhr, ajaxOptions, thrownError) {
                ctrSupplierDropdown = 0;
                console.log(xhr.status);
                console.log(thrownError);
            }
        });

    },

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
   
    indexItemDetailsEdit = boundIndex;
});

// End //


// Inactive Add Item //
$(document).delegate(".gridDetails_Remove", 'click', function () {
    var rowID = $("#gridDetails").jqxGrid('getrowid', indexItemDetailsEdit);
    var data = $("#gridDetails").jqxGrid('getrowdatabyid', rowID);

   show_modal.deactivate(data.id_number);

});

// Inactive confirmation button //
$(document).delegate("#btnProceedDeactivate", "click", function () {
    dbase_operation_draft.deactivate($(this).attr('data-id'));

});
// End function //


// Inactive Transaction //
$(document).delegate(".gridDrafts_Remove", 'click', function () {
    var rowID = $("#gridDrafts").jqxGrid('getrowid', indexAllTrans);
    var data = $("#gridDrafts").jqxGrid('getrowdatabyid', rowID);

    show_modal.deactivateTransDraft(data.id_number);

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
        },
        success: function (response) {
            if (response.success) {
                $('#cmbItemType').val(response.message[0]["TypeCode"]);
                $('#cmbCategory').val(response.message[0]["CategoryCode"]);
                $('#cmbUom').val(response.message[0]["UOMCode"]);
                $('#cmbItemTypeDisplay').val(response.message[0]["Type"]);
                $('#cmbCategoryDisplay').val(response.message[0]["Category"]);
                $('#cmbUomDisplay').val(response.message[0]["UOM"]);
            } else {
                notification_modal("autobind failed!", response.message, "danger");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(thrownError);
        }
    });
});

$(document).delegate("#cmbItemEdit", 'change', function () {
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
        },
        success: function (response) {
            if (response.success) {
                $('#cmbItemTypeEdit').val(response.message[0]["TypeCode"]);
                $('#cmbCategoryEdit').val(response.message[0]["CategoryCode"]);
                $('#cmbUomEdit').val(response.message[0]["UOMCode"]);
                $('#cmbItemTypeEditDisplay').val(response.message[0]["Type"]);
                $('#cmbCategoryEditDisplay').val(response.message[0]["Category"]);
                $('#cmbUomEditDisplay').val(response.message[0]["UOM"]);
            } else {
                notification_modal("autobind failed!", response.message, "danger");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(thrownError);
        }
    });
});


// Search function //
$(document).delegate("#gridDrafts_searchField", "keyup", function (e) {
    var columns = ["return_date_date", "purpose_string", "impex_ref_number_string",
                   "supplier_name_string"];
    generalSearch($('#gridDrafts_searchField').val(), "gridDrafts", columns, e);
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
        promptText: "Select Supplier",
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
        width: '91%',
        height: 20,
        displayMember: 'Name',
        valueMember: 'Code'
    });

    $("#cmdAddContact").prop('disabled', true);
    $("#addContactIcon").css('color', 'gray');
    
     //modify binding
    $("#cmbSupplier").bind('select', function (event) {
        if (event.args) {
            var value = event.args.item.value;

            $("#cmbContactPerson").jqxDropDownList({ placeHolder: "Select Contact Person" });
            ContactPersonCombo(value)

        }

        ctrSupplierDropdown++;

    });

}



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
        var elements = ["#FirstName", "#LastName"]
        var ctr = 0;
        for (var i = 0; i <= 1; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "#f98989");
                $('#required').text('Fields with red color are required to proceed.');
                $('#required').show('slow');

            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5");
                ctr++;
            }
        }

        //if all textbox is filled
        if (ctr == 2) {

            //$('#required').hide('slow');
            //var x = $('#Email').val();
            //var atpos = x.indexOf("@");
            //var dotpos = x.lastIndexOf(".");

            //if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.length) {

            //    $('#required').text('Not a valid email address.');
            //    $('#required').show('slow');

            //    $('#Email').css("border-color", "#f98989");

            //}
            //if all textbox is filled

                     
                var url = '';
                var msg = '';

                if (operation == 'add') {
                    url = '/ContactPerson/AddContactPerson';
                    msg = 'New record successfully added!';
                    title = 'Add Record';
                } else {
                    url = '/ContactPerson/UpdateContactPerson';
                    msg = 'Record successfully updated!';
                    title = 'Update Record';
                }

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

                    },
                    beforeSend: function () {

                    },
                    headers: {
                        //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                    },
                    success: function (response) {

                        if (response.success) {

                            $("#modalContactPerson").modal("hide");
                            ContactPersonCombo($('#cmbSupplier').val());

                        } else {
                            ///asd

                            $("#modalContactPerson").css('z-index', '100000');
                            notification_modal("Warning", "Contact details has been already exist!", "danger");

                            $('#btn_notifClose').click(function () {
                                $('#notification_modal .close').click();
                                $("#modalContactPerson").css('z-index', '1000000');
                            });

                        }


                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.status);
                        console.log(thrownError);
                    }
                });
            


        }
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
    }
};

// add contact details modal
var showform = {

    item: function (operation) {

        var modal = '<div class="modal fade" id="modalitemmaster" role="dialog" data-backdrop="static">';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#12AFCB; color:#ffffff">';
        modal += '<h4 class="modal-title">Item details</h4>';
        modal += '</div>';
        modal += '<div class="modal-body">';

        modal += '<div class="row">';
        modal += '  <input id="Code" type="hidden" />';
        modal += '  <div class="col-md-12" style="margin-top:3%;">';
        modal += '          <div class="form-group">';
        modal += '              <input id="name" type="text" class="form-control" title="Item Name" placeholder="Item Name*"class="form-control companyrequired"/>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '              <input id="description" placeholder="Description*" title="Description"  class="form-control" type="text" class="form-control companyrequired"/>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '              <div class="form-control dropdownlist" id="cmbCategoryItem" title="Item Category" data-placeholder="Item Category*" data-url="/Items/GetItemCategories?IsActive=1" data-display="name_string" data-value="code_string" data-width="92%"></div>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '              <div class="form-control dropdownlist" id="cmbType" title="Item Type" data-placeholder="Item Type*" data-url="/Items/GetItemTypes?IsActive=1" data-display="name_string" data-value="code_string" data-width="92%"></div>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '              <div class="form-control dropdownlist" id="cmbUOM" title="Unit of Measure" data-placeholder="Unit of Measure*" data-url="/Items/GetUnitOfMeasures?IsActive=1" data-display="name_string" data-value="code_string" data-width="92%"></div>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '             <div class="form-control dropdownlist" id="cmbDepartment" title="Item Department Related" data-placeholder="Item Department Related*" data-url="/Items/GetItemDepartment?IsActive=1" data-display="name_string" data-value="code_string" data-width="92%"></div>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        modal += '<p id="requireditemdetails" hidden style="font-family:arial;color:red">Fields with red color are required to proceed.</p>';
        modal += '<p id="NotiId" style="color:red; font-family:arial" hidden="hidden">Notification msg here.</p>';
        modal += '<div class="modal-footer" style="padding-right:0; padding-top:5px;">';
        modal += '<div class="row">';
        modal += '<a data-operation="' + operation + '"class="btn btn-medium btn-blue" id="btnSaveItem"';
        modal += '>';
        modal += 'Add</a>';
        modal += '<a id="btnCancel" type="button" class="btn btn-medium btn-gray" data-dismiss="modal">Cancel</a> &nbsp &nbsp';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#confirmation_modal").html(modal);
        $("#modalitemmaster").modal("show");
        $("#modalitemmaster").css('z-index', '1000000001');

        $("#form_modal_div").css('z-index', '100000');

        ini_main.element('dropdownlist');
        ini_main.element('inputtext');
    },

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
        modal2 += '              <input id="LastName" type="text" placeholder="Last Name" class="form-control companyrequired" style="font-size:12px; font-family: segoe ui;" maxlength="50"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        modal2 += '    <label for="cmbUom">First Name:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
        modal2 += '              <input id="FirstName" type="text" placeholder="First Name" class="form-control companyrequired" style="font-size:12px; font-family: segoe ui;" maxlength="50"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        modal2 += '    <label for="cmbUom">Middle Initial:</label>';
        modal2 += '              <input id="MiddleName" type="text" placeholder="Middle Name" class="form-control companyrequired" style="font-size:12px; font-family: segoe ui;" maxlength="1"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '  </div>';
        modal2 += '</div>';

        modal2 += '<div class="row">';
        modal2 += '  <div class="col-md-12">';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        modal2 += '    <label for="cmbUom">Email Address:</label>';
        modal2 += '              <input id="Email" type="text" placeholder="Email Address" class="form-control companyrequired" style="font-size:12px; font-family: segoe ui;" maxlength="50"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        modal2 += '    <label for="cmbUom">Contact Number:</label>';
        modal2 += '              <input id="ContactNumber" type="number" placeholder="Contact Number" class="form-control companyrequired" style="font-size:12px; font-family: segoe ui" onkeypress="return isNumeric(event)"  oninput="maxLengthCheck(this)" maxlength = "25" min = "1" max = "99999999999"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        modal2 += '    <label for="cmbUom">Department:</label>';
        modal2 += '              <input id="Department" type="text" placeholder="Department" class="form-control companyrequired"  style="font-size:12px; font-family: segoe ui;" maxlength="50"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '  </div>';
        modal2 += '</div>';


        modal2 += '  <div class="col-md-12">';
        modal2 += '<p id="required" hidden style=font-family:arial;color:red>Fields with red color are required to proceed.</p>';
        modal2 += '  </div>';


        modal2 += '<div class="modal-footer">';
        modal2 += '<div class="row">';
        modal2 += '<a class="btn btn-meduim btn-blue" data-recid="' + id + '" id="' + ((operation == 'add') ? 'cmdAddItemContactDrafts' : 'cmdUpdateContactPerson') + '">';
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



    },



// update the Items Modal
        EditAddedItemDraft: function (transid, itemcode, categorycode, serialnumber, itemtypecode, tagnumber, ponumber, quantity, UoMcode, remarks, image,
                               UOMName, catagoryName, typeName) {

           origImageName = image;
           //var displayImageName = image.substring(image.indexOf('_') + 1);
           var displayImageName = "";
           if (image == null || image == "") {
               displayImageName = "";
           }
           else {
               displayImageName = image.substring(image.indexOf('_') + 1);
           }

        var modal = '<div class="modal fade" id="form_modal_divEdit" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
        modal += '<div class="modal-dialog modal-lg">';
        modal += '<div class="modal-content">';

        modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title" id="myModalLabel">Update Item</h4>';
        modal += '</div>';

            modal += '<div class="modal-body">';
           modal += '<br />';
           modal += '<div class="row">';

           modal += '<div class="col-md-12">';
           modal += '<div class="form-group col-md-6">';
           modal += '<label for="cmbItemEdit">Item:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
           modal += '<div class="input-group" style="float:right">';
           modal += '<div class="form-control item dropdownlist" id="cmbItemEdit" data-url="/Items/GetItems?IsActive=1" data-display="name_string" data-value="code_string"></div>';
        modal += '<span class="input-group-btn">';
           modal += '<button class="btn btn-default" type="button" id="cmdAddNewItemEdit" style="margin-left:4px; width:95%; border-color:#DCE1E4; background-color:#EEEEEE"><i class="fa fa-plus faa-tada animated-hover" style="color: #31B0D5" id="addContactIcon"></i></button>';
           modal += '</span>';
           modal += '</div>';
           modal += '</div>';

           modal += '  <div class="form-group col-md-3">';
           modal += '    <label for="txtQuantity">Quantity:<span class="asterisc" style="color:red;font-size:12px">*</span></label>';
           modal += '    <input type="number" class="form-control" id="txtQuantityEdit" style="font-size:12px; font-family: segoe ui" onkeypress="return isNumeric(event)"  oninput="maxLengthCheck(this)" maxlength = "11" min = "1" max = "99999999999" value="' + quantity + '">';
           modal += '  </div>';

           modal += '<div class="form-group col-md-3">';
           modal += '  <label for="txtSerialNbr">Serial Number:</label>';
           modal += '  <input type="text" class="form-control" id="txtSerialNbrEdit" style="font-size:12px; font-family: segoe ui" maxlength="25" value="'+serialnumber +'">';
           modal += '</div>';
        modal += '  </div>';
        modal += '<div class="col-md-12">';
           modal += '  <div class="form-group col-md-3">';
           modal += '  <label for="cmbItemTypeEdit">Type:</label>';
           modal += '  <div class="form-control dropdownlist" id="cmbItemTypeEdit" data-url="/Items/GetItemTypes?IsActive=1" data-display="name_string" data-value="code_string"></div>';
           modal += '  <input type="text" class="form-control" id="cmbItemTypeEditDisplay" style="font-size:12px; font-family: segoe ui; background-color:transparent;display:none" maxlength="25" value="' +typeName + '" disabled>';
           modal += '  </div>';

           modal += '  <div class="form-group col-md-3">';
           modal += '    <label for="cmbCategoryEdit">Category:</label>';
           modal += '    <div class="form-control dropdownlist" id="cmbCategoryEdit" data-url="/Items/GetItemCategories?IsActive=1" data-display="name_string" data-value="code_string"></div>';
           modal += '  <input type="text" class="form-control" id="cmbCategoryEditDisplay" style="font-size:12px; font-family: segoe ui; background-color:transparent;display:none" maxlength="25" value="' + catagoryName + '" disabled>';
           modal += '  </div>';

          
           modal += '  <div class="form-group col-md-3">';
           modal += '    <label for="txtPoNbr">PO Number:</label>';
           modal += '    <input type="text" class="form-control" id="txtPoNbrEdit" style="font-size:12px; font-family: segoe ui" maxlength="25" value="'+ ponumber +'">';
           modal += '  </div>';

           modal += '  <div class="form-group col-md-3">';
           modal += '    <label for="txtTagNbr">Tag Number:</label>';
           modal += '    <input type="text" class="form-control" id="txtTagNbrEdit" style="font-size:12px; font-family: segoe ui" maxlength="25" value="'+tagnumber +'">';
           modal += '  </div>';

       
           modal += '</div>';
           modal += '<div class="col-md-12">';
           modal += '  <div class="form-group col-md-3">';
           modal += '    <label for="cmbUomEdit">Unit of Measure:<span class="asterisc" style="color:red;font-size:12px"></span></label>';
           modal += '    <div class="form-control dropdownlist" id="cmbUomEdit" data-url="/Items/GetUnitOfMeasures?IsActive=1" data-display="name_string" data-value="code_string"></div>';
           modal += '  <input type="text" class="form-control" id="cmbUomEditDisplay" style="font-size:12px; font-family: segoe ui; background-color:transparent;display:none" maxlength="25" value="' + UOMName + '" disabled>';
           modal += '  </div>';
           modal += '<div class="form-group col-md-3">';
           modal += '<label for="txtItemRemarks">Remarks:</label>';
           modal += '<input type="text" class="form-control" id="txtItemRemarksEdit" style="font-size:12px; font-family: segoe ui" maxlength="125" value="'+remarks + '">';
           modal += '</div>';
           modal += '<div class="form-group col-md-3">';
           modal += '<label for="inpt_file">Upload Image:</label>';
           modal += '<div><input type="file" id="inpt_fileEdit" name="inpt_fileEdit" style="color:#F25656; font-size:1.1em; font-size:12px; font-family: segoe ui" /></div>';
           //modal += '<label style="float:left; color:red;">File name can\'t contain special characters.</label>';
           modal += '</div>'
           modal += '<div class="form-group col-md-3">';
           modal += '<label for="currentFile">Current Image:</label>';
           modal += '<div style="float:right" id="removeAttachmentEdit"><i class="fa fa-times-circle" title="Remove" style="cursor:pointer;color:red" aria-hidden="true"></i></div>';
           modal += '<div><a id="fileItem" href="" download title="Download">';
        modal += '<label id="lblCurrentFilename" class="text-success" style="cursor:pointer; font-weight:bold; font-size:12px; font-family: segoe ui;">' +((displayImageName == "") ? 'No file chosen': displayImageName) + '</label>';
        modal += '</a></div>';

        modal += '</div>';
           modal += '</div>';
           modal += '</div>';

           modal += '<div class="modal-footer">';
           modal += '<label class="text-success" id="notiIdSuccess" style="float:left; display: none;" >Additional item successfully added.</label>';
           modal += '<a class="btn btn-meduim btn-blue" data-transid="' + transid + '" id="cmdUpdateItem">Update</a>';
           modal += '<a class="btn btn-meduim btn-gray" id="cmdCancel" data-dismiss="modal">Cancel</a>';

           modal += '</div>';

           modal += '</div>';
           modal += '</div>';
           modal += '</div>';
        $("#form_modal").html(modal);
        $("#form_modal_divEdit").modal("show");
        $("#form_modal_divEdit").css('z-index', '1000000');



        ini_main.element('inputtext');
        ini_main.element('inputnumber');
        ini_main.element('dropdownlist');

        $("#cmbUomEdit").jqxDropDownList({ width: '165px', });
        $("#cmbItemTypeEdit").jqxDropDownList({ width: '165px' });
        $("#cmbCategoryEdit").jqxDropDownList({ width: '165px' });


        $("#cmbItemEdit").on('bindingComplete', function (event) {
            if (itemcode != "") {
                $("#cmbItemEdit").jqxDropDownList('val', itemcode);
            }
            });

        $("#cmbCategoryEdit").on('bindingComplete', function (event) {
            if (categorycode != "") {
                $("#cmbCategoryEdit").jqxDropDownList('val', categorycode);
            }
                });

        $("#cmbItemTypeEdit").on('bindingComplete', function (event) {
            if (itemtypecode != "") {
                $("#cmbItemTypeEdit").jqxDropDownList('val', itemtypecode);
                }
                });

        $("#cmbUomEdit").on('bindingComplete', function (event) {
            if (UoMcode != "") {
                $("#cmbUomEdit").jqxDropDownList('val', UoMcode);
                }
                });


        $("#cmbItemTypeEdit").jqxDropDownList({
                dropDownHeight: 50,
                filterable: false,
                });

    },


};



// desc : display snacknotification
// date: jan 18, 2017
// by: avillena@allegromicro.com
function toastNotification() {
    snackbarDraft = document.getElementById("snackbarDraftNotif");
    snackbarDraft.className = "show";
    setTimeout(function () { snackbarDraft.className = snackbarDraft.className.replace("show", ""); }, 4000);
}


// initialized the updated contact person combo
function ContactPersonCombo(value, contactperson) {

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
        autoDropDownHeight: adapter_contactperson.records.length > 10 ? true : false,
        disabled: false,
        displayMember: 'Name',
        valueMember: 'Code'

    });

    $("#cmdAddContact").prop('disabled', false);
    $("#addContactIcon").css('color', '#31B0D5');

  

}



//Update the Item details
$(document).delegate(".gridDetails_Edit", 'click', function () {
    var rowID = $("#gridDetails").jqxGrid('getrowid', indexItemDetailsEdit);
    var data = $("#gridDetails").jqxGrid('getrowdatabyid', rowID);

    Global_IDAddedItem = data.id_number;
    showform.EditAddedItemDraft(data.header_code_string,
        data.item_code_string,
        data.category_code_string,
        data.serial_number_string,
        data.item_type_code_string,
        data.tag_number_string,
        data.p_o_number_string,
        data.quantity_number,
        data.unit_of_measure_code_string,
        data.remarks_string,
        data.image_image,
        data.unit_of_measure_string,
        data.category_string,
        data.type_string);
    $('#fileItem').attr('href', imagePath + data.image_image);

});

$(document).delegate("#cmdUpdateItem", 'click', function () {
    var ext = $("#inpt_fileEdit").val().split('.').pop();
    if (ext == "png" || ext == "jpg" || ext == "PNG" || ext == "JPG" || ext == "jpeg" || ext == "JPEG" || $("#inpt_fileEdit").val() == "") {
         if ($("#inpt_fileEdit").val() != "") {
            if ($('input[name="inpt_fileEdit"]')[0].files[0].size > fileSize) {
                toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " File cannot be more then 200MB.";
                Fail_ToastNotif();
            }
            else {
                dbase_operation_draft.UpdateAddedItems($(this).attr("data-transid"));
            }
        }
        else {
             dbase_operation_draft.UpdateAddedItems($(this).attr("data-transid"));
        }

    }
    else {

        $("#form_modal_divEdit").css('z-index', '10000');
        notification_modal("Warning", "Attached File extension not supported.", "danger");
        $('#btn_notifClose').click(function () {
            $('#notification_modal .close').click();
            $("#form_modal_divEdit").css('z-index', '10000000');
        });
    }
});