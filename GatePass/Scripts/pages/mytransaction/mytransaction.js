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
var returnOriginalFileName;


function windowprintfunction() {
    window.print();
}

function DownloadAttachment() {
  if (originalFileName == "" || originalFileName == null) {
        $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'Uploaded file is empty.');
        Warning_ToastNotif();
        $(".warning").css('z-index', '1000000000000000');
    }
  else {
      //window.location.href = '/Attachment/DownloadAttachment?filename=' + encodeURIComponent(originalFileName) + '&path=' + env;
    window.location.href = '/Attachment/DownloadAttachment?filename=' + originalFileName + '&path=' + env;
    }
}

function DownloadReturnAttachment() {
    if (returnOriginalFileName == "" || returnOriginalFileName == null) {
        $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'Uploaded file is empty.');
        Warning_ToastNotif();
        $(".warning").css('z-index', '1000000000000000');
    }
    else {
    window.location.href = '/Attachment/DownloadReturnAttachment?filename=' + returnOriginalFileName + '&path=' + env;
    }
}

function clear() {
    $('#txtPurpose').val("");
    $('#txtImpexRefNbr').val("");
    $('#cmbTransType').val("IN");
    $('#txtReturnDate').val("");
};

//rherejias 1/3/2017 for attachment deletion
$(document).delegate("#btnDel", "click", function () {
     dbase_operation_Item.delReturnAttachment();
    
});

//rherejias 1/3/2017 for attachment deletion
$(document).delegate("#btnCancelRemoveFile", "click", function () {
    //dbase_operation_Item.delReturnAttachment();
    $("#form_modal_returnslip_details").css('z-index', '1000000');
});

//rherejias 1/3/2017 for confirmation modal 
$(document).delegate("#removeReturnAttachment", "click", function () {
    show_modal.confirmationModalDeleteAttachment();
    $("#form_modal_returnslip_details").css('z-index', '100000');
});

//rherejias 1/3/2017 for return slip attach file
$(document).delegate(".File", "click", function () {
    show_modal.upload();
});

//rherejias 1/3/2017 for return slip attach file proceed upload
$(document).delegate("#btnProceedUpload", "click", function () {
    var ext = $("#inpt_file_returnslip").val().split('.').pop();
    if (ext == "doc" || ext == "docx" || ext == "xls" || ext == "xlsx" || ext == "ppt" || ext == "pptx" || ext == "png" ||
        ext == "jpg" || ext == "PNG" || ext == "JPG" || ext == "jpeg" || ext == "JPEG" || ext == "txt") {
        if ($("#inpt_file_returnslip").val() != "") {
            if ($('input[name="inpt_file_returnslip"]')[0].files[0].size > fileSize) {
                toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " File cannot be more then 200MB.";
                Fail_ToastNotif();
            }
            else {
               dbase_operation_Item.returnAttachment();
            }
        }
        else {
             dbase_operation_Item.returnAttachment();
        }
    }
    else if (ext == '') {


       
        $('#lblNoSelectedFile').show('slow');
        setTimeout(function () { $('#lblNoSelectedFile').css('display', 'none'); }, 4000);
    } else {
       
        $('#lblNotSupported').show('slow');
        setTimeout(function () { $('#lblNotSupported').css('display', 'none'); }, 4000);
        
    }
});




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

    $('#optionmytrans').click(function () {
        $('#gridAllTransDept').hide();
        $('#dropdownMyTransDept').hide();
        $('#gridAllTrans').show();
        $('#dropdownMyTrans').show();
    });

    $('#optionmydept').click(function () {
        $('#gridAllTransDept').show();
        $('#dropdownMyTransDept').show();
        $('#gridAllTrans').hide();
        $('#dropdownMyTrans').hide();
    });

    $("#cmdSaveAndSubmit").click(function () {

        var datainformations = $('#gridDetails').jqxGrid('getdatainformation');
        var rowscounts = datainformations.rowscount;


        if (typeof $("#cmdAddItem").attr("data-transid") == "undefined" || $("#cmdAddItem").attr("data-transid") == "") {
        
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

                if (rowscounts != 0) {    
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
                if (rowscounts != 0) {    
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


    $("#cmdCreateNew").click(function () {
        $('#bodyCreateNew').slideDown('slow');
        $('#bodyViewTrans').slideUp('slow');
        $('#titleCreate').css('display', 'unset');
        $('#titleView').css('display', 'none');
        $('#cmdCreateNew').css('visibility', 'hidden');
        $('#cmdButton').css('display', 'unset');
        $('#cmdDraftUpdate').css('display', 'none');
        $('#cmdSaveAsDraft').css('display', 'unset');
        $('#gridDetails').jqxGrid('updatebounddata');
        $('#currentFile').attr('hidden','hidden');
        $('#lblcurrentFile').attr('hidden','hidden');
        $('#removeAttachment').css('display', 'none');

        //remove data-transid attribute
        $("#cmdAddItem").removeAttr("data-transid");
  
        dbase_operation_Item.removeItems();
   
        
    });

    $("#cmdButton").click(function () {
       
        $("#gridDetails").attr('data-url', '/Transactions/GetTransDetails');

        $('#bodyCreateNew').slideUp('slow');
        $('#bodyViewTrans').slideDown('slow');
        $('#titleCreate').css('display', 'none');
        $('#titleView').css('display', 'unset');
        $('#gridDetails').jqxGrid('updatebounddata');
        $('#cmdCreateNew').css('visibility', 'visible');
        $('#cmdButton').css('display', 'none');
        clear();
        initialize_jqxwidget_grid($("#gridDetails"));
 
        $('#txtImpexRefNbr').css("border-color", "#e5e5e5");
        $('#txtPurpose').css("border-color", "#e5e5e5");
        $('#txtReturnDate').css("border-color", "#e5e5e5");
        $('#cmbTransType').css("border-color", "#e5e5e5");
        
        
        
       
    });
  
    $("#cmbStatus").change(function () {
  
        applyFilter('status_string', $(this).attr('data-grid'), $(this).val());       
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
    //for returnslip attachment
    $(document).delegate("#inpt_file_returnslip", "change", function () {
        var val = $(this).val().toLowerCase();
        var regexChar = new RegExp("\#|\&|,|;|\\+");
        var regex = new RegExp(ext);
        if (!(regex.test(val))) {
            notification_modal('Error', 'File type not supported.</br></br><b>Supported file type:</b> '+ extMsg +'', 'danger');
            $(this).val('');
        }
        else {
            if ((regexChar.test(val))) {
                notification_modal('Error', 'File name contains special characters. Please rename your file and try again.</br></br><b>Special characters:</b> # & ; , + \ / * ? : < > | ', 'danger');
                $(this).val('');
            }
        }
    });

});

//adding a new transaction or submitting a draft
$(document).delegate("#cmdProceedSaveAndSubmit", "click", function() {
   
    dbase_operation.addHeader($(this).attr("data-transid"), $(this).attr("data-transstat"));
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

    //tracking for printable copy

    approvaltrackingforprintable: function (headercode) {
        $.ajax({
            url: '/Transactions/Approvaltracking',
            type: 'post',
            dataType: 'json',
            data: {
                Id: headercode,

            },
            beforeSend: function () {

            },
            headers: {
            },
            success: function (response) {


                if (response.success) {

                    if (response.message == 'Guard Approver') {
                 
                        $('#cmdBackmyTrans').show();
                        $('#mytransactionpage').hide();
                        $('#hardcopypage').show();

                        var rowID = $("#gridAllTrans").jqxGrid('getrowid', indexAllTrans);
                        gatepass_details = $("#gridAllTrans").jqxGrid('getrowdatabyid', rowID);
                        var returndate = gatepass_details.return_date_date
                        if (returndate == "" || returndate == null) {
                            $('#tdreturndate').html("");
                        } else {
                            $('#tdreturndate').html(returndate.toISOString().slice(0, 10));
                        }
                        $('#tdGPId').html(gatepass_details.gate_pass_id_string);
                        $('#tdpurpose').html(gatepass_details.purpose_string);
                        $('#tdimpexref').html(gatepass_details.impex_ref_number_string);
                        $('#tdsupplier').html(gatepass_details.supplier_name_string);
                        $('#tdcontactname').html(gatepass_details.supplier_address_string);

                        $("#girdforpirnt").attr('data-url', "/Transactions/GetAll_Item_ReturnSlip" + '?HeaderKey=' + gatepass_details.gate_pass_id_string);
                        initialize_jqxwidget_grid($("#girdforpirnt"));

                    } else {
                            
                        $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'To generate printable copy, make sure that this Gate Pass has an accounting approval');
                        Warning_ToastNotif();
                    }


                }




            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },



    ///for tracking
    approvaltracking: function (headercode) {
        $.ajax({
            url: '/Transactions/Approvaltracking',
            type: 'post',
            dataType: 'json',
            data: {
                Id: headercode,

            },
            beforeSend: function () {

            },
            headers: {
            },
            success: function (response) {


                if (response.success) {

                    if (response.message == "ApprovedOrRejected") {

                        $('#approvaltracking_div').css('display', 'none');
                    } else {

                        $('#txtnextapprover').val(response.message);

                    }

                }


               

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },

    //End

    /// not used
    checkifapproved: function (gatepassheader) {
        $.ajax({
            url: '/Transactions/hardcopyapproved',
            type: 'post',
            dataType: 'json',
            data: {
                Id: gatepassheader,
               
            },
            beforeSend: function () {

            },
            headers: {
              
            },
            success: function (response) {

                if (response.success) {

                   
                
                    var deptype = departmentApprover;
                    var ITtype = itApprover;
                    var purchtype = purchasingApprover;
                    var accountingtype = accountingApprover;
                    
                    for (var i = 0; i < response.message.length; i++) {                   

                        var dept = response.message[i]["ApprovalTypeCode"];
            
                        if (dept == deptype) {

                            if (response.message[i]["IsApproved"] == 1) {
                              
                                $('#tddept').html('APPROVED');
                            } else { $('#tddept').html('To be Approved'); }

                        }
                        else if (dept == ITtype) {

                            if (response.message[i]["IsApproved"] == 1) {
                                $('#tdIT').html('APPROVED');
                            } else { $('#tdIT').html('To be Approved'); }

                          
                        }
                        else if (dept == purchtype) {

                            if (response.message[i]["IsApproved"] == 1) {
                                $('#tdpurch').html('APPROVED');
                            } else { $('#tdpurch').html('To be Approved'); }
                           
                        }
                        else if (dept == accountingtype) {
                            if (response.message[i]["IsApproved"] == 1) {
                                $('#tdacc').html('APPROVED');
                            } else { $('#tdacc').html('To be Approved'); }
                            
                        }

                    }
    
                    if ($('#tdpurch').html() == '-') {
                        $('#tdpurch').html('NOT APPLICABLE');
                    }
                    else if ($('#tdIT').html() == '-') {
                        $('#tdIT').html('NOT APPLICABLE');
                    }
                    else if ($('#tddept').html() == '-') {
                        alert('sfs');
                        
                    }
                    else if ($('#tdacc').html() == '-') {
                        $('#tdacc').html('NOT APPLICABLE');
                    }

                  

                } else {
                    notification_modal("autobind failed!", response.message, "danger");
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },
    /// End




    //rherejias 1/3/2017 for delete attachment for return slip ajax call
    delReturnAttachment: function () {
        $.ajax({
            url: '/Transactions/ReturnSlipDelAttachment',
            type: 'post',
            dataType: 'json',
            data: {
                Id: headerdetails_for_RS.id_number,
                Code: headerdetails_for_RS.gate_pass_id_string,
            },
            beforeSend: function () {

            },
            headers: {
                //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {

                $("#form_modal_returnslip_details").modal("hide");
                $("#grid_returnslip").jqxGrid('updatebounddata');
                toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Attach file was successfully removed!";
                Save_ToastNotif();


            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },

    //rherejias 1/3/2017 for return slip attachment ajax call
    returnAttachment: function () {
        var rowID = $("#grid_returnslip").jqxGrid('getrowid', item_returnslipIndex);
        headerdetails_for_RS = $("#grid_returnslip").jqxGrid('getrowdatabyid', rowID);
        var formData = new FormData();
        formData.append('inpt_file', $('input[name="inpt_file_returnslip"]')[0].files[0]);
        formData.append('code', headerdetails_for_RS.gate_pass_id_string);
        formData.append('id', headerdetails_for_RS.id_number);
        $.ajax({
            url: '/Transactions/ReturnSlipAttachment',
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

                $("#modalUpload").modal("hide");
                toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " File was successfully attached!";
                $("#grid_returnslip").jqxGrid('updatebounddata');
                Save_ToastNotif();
                //pogi

            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },

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
                  
                },
                success: function (response) {
                   
                    if (response.success) {
                     
                    
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
        itemsloadCatg_Type: function (){

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
    addDetails: function() {
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


$(document).delegate('#RS_confirm_modal', 'click', function(){
    $("#form_modal_returnslip_details").css('z-index', '1000000000000000000');

});

var show_modal = {
    //rherejias 1/3/2017 fro confirmation modal 
    confirmationModalDeleteAttachment: function () {
        var modal = '<div class="modal fade" id="conf_modal_returnitem" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
        modal += '<div class="modal-dialog modal-sm">';
        modal += '<div class="modal-content">';
        modal += '<div class="modal-header" style="background-color: #F25656; color:#ffffff;">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title" id="myModalLabel"><i class="fa fa-exclamation-triangle" aria-hidden="true"></i> Remove File</h4>';
        modal += '</div>';
        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += 'Are you sure you want to remove this attachment?';
        modal += '</div>';
        modal += '<div class="modal-footer">';       
        modal += '<a class="btn btn-small btn-blue" data-dismiss="modal" id="btnDel">YES</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal" id=btnCancelRemoveFile>NO</a>';
        modal += '</div>';


        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        $("#confirmation_modal").html(modal);
        $("#conf_modal_returnitem").modal("show");
        $("#conf_modal_returnitem").css('z-index', '100000000000000');
    },

    //rherejias 1/3/2017 for return slip attach file modal
    upload: function () {
        var modal = '<style>';
        modal += '.loader{';
        modal += 'border:2px solid #E5E5E5;';
        modal += 'border-radius: 50%;';
        modal += 'border-top: 5px solid #76cad4;';
        modal += 'width: 30px;';
        modal += 'height: 30px;';
        modal += '-webkit-animation: spin 2s linear infinite;';
        modal += 'animation: spin 2s linear infinite;';
        modal += '}';
        modal += '@-webkit-keyframes spin {';
        modal += '0% { -webkit-transform: rotate(0deg); }';
        modal += '100% { -webkit-transform: rotate(360deg); }';
        modal += '}';
        modal += '@keyframes spin {';
        modal += '0% { transform: rotate(0deg); }';
        modal += '100% { transform: rotate(360deg); }';
        modal += '}';
        modal += '</style>';
        modal += '<div class="modal fade" id="modalUpload" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal += '<h4 class="modal-title"><i class="fa fa-paperclip" aria-hidden="true"></i> Attach File</h4>';
        modal += '<div class="loader"style="float:right; top:-26px; position:relative; display:none"></div>';
        modal += '</div>';
        modal += '<div class="modal-body" style="margin-top:4%;">';

        modal += '<div class="row">';
        modal += ' <div class="list-group">'; 
        modal += ' <a class="list-group-item">'; 
        modal += ' <input type="file" id="inpt_file_returnslip" name="inpt_file_returnslip" style="color:#F25656; font-size:1.1em;" />';
        modal += '</a>';
       // modal += '<label style="float:left; color:red; margin-top:5px; margin-left:5px;">File name can\'t contain special characters.</label>';
       
        modal += '<p id="lblNoSelectedFile" style="display:none;font-family:arial;color:red"><i class="fa fa-exclamation-triangle" aria-hidden="true"></i> No File Seleceted!</p>';
        modal += '<p id="lblNotSupported" style="display:none;font-family:arial;color:red"><i class="fa fa-exclamation-triangle" aria-hidden="true"></i> File Not Supported!</p>';

        modal += ' </div>';
        
        modal += '</div>';
        modal += '</div>';
   
        modal += '<div class="modal-footer"style="margin-top:-5%;">';   
        modal += '<a class="btn btn-small btn-blue" id="btnProceedUpload">Upload</a>';
        modal += '<a id="btnCancel" class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalUpload").modal("show");
        $("#modalUpload").css('z-index', '1000000');
    },

    

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


        $("#image_modal").html(modal);
        $("#modalImageView").modal("show");
        $("#modalImageView").css('z-index', '1000000');
    },


   


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
        modal += '<a class="btn btn-small btn-blue" data-dismiss="modal" data-transid="' + trans_id + '" data-transstat="' + status + '" id="cmdProceedSaveAndSubmit">Confirm</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a>';
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
        modal += '<a class="btn btn-small btn-blue" data-dismiss="modal" id="cmdUpdateDraft">Confirm</a>';
        modal += '<a class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#confirmation_modal").html(modal);
        $("#conf_modal_div2").modal("show");
        $("#conf_modal_div2").css('z-index', '1000000');
    },

   

    // Item-Details Modal //
    show_Itemdetails_returnslip: function (returnslip_code, returndate, purpose, impexnumber, transtype, usercode, GPFileAttach, supplier, origName) {

      
        var modal = '<div class="modal fade" id="form_modal_returnslip_details" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
        modal += '<div class="modal-dialog modal-lg">';
        modal += '<div class="modal-content">';

        modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title" id="myModalLabel">Gate Pass Details</h4>';
        modal += '</div>';

        modal += '<div class="modal-body">';
        modal += '<br />';



        modal += '<div class="row">';
        modal += '<div class="form-group col-md-3">';
        modal += '<label for="txtGatePassID" style="font-size:12px; font-family: segoe ui">Gate Pass ID*:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtGatePassID" style="background-color:transparent;font-size:12px; font-family: segoe ui" value="' + returnslip_code + '" readonly="readonly">';
        modal += ' </div>';

        modal += '<div class="form-group col-md-3">';
        modal += '<label for="txtImpexRefNbr">IMPEX Ref. Number:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtImpexRefNbr" placeholder="IMPEX Ref. Number" style="background-color:transparent" value="' + impexnumber + '" readonly>';
        modal += '</div>';

        modal += '<div class="form-group col-md-6">';
        modal += '<label for="cmdSupplier">Supplier:</label>';
        modal += '<input type="text" class="form-control input-sm" id="cmdSupplier" placeholder="IMPEX Ref. Number" style="background-color:transparent" value="' + supplier + '" readonly>';
        modal += '</div>';
        modal += '</div>';

        modal += '<div class="row">';
        modal += ' <div class="col-md-3">';
        modal += '<label for="cmbTransType">Transaction Type:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtGatePassID" value= "' + transtype + '" style="background-color:transparent" readonly="readonly">';
        modal += '</div>';

        modal += '<div class="col-md-3">';
        modal += '<label for="txtReturnDate">Return Date:</label>';
        modal += '<div class="input-group input-append date form_datetime">';
        modal += '<input id="txtReturnDate" type="text" class="form-control input-sm" style="background-color:transparent" value= "' + returndate + '" readonly/>';
        modal += '<span class="input-group-addon add-on">';
        modal += '<i class="fa fa-calendar faa-tada animated-hover"';
        modal += 'style="color: #31B0D5">';
        modal += '</i>';
        modal += '</span>';
        modal += '</div>';
        modal += '</div>';
     
        modal += '<div class="col-md-6">';
        modal += '<label for="txtPurpose">Purpose:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtPurpose" value= "' + purpose + '" style="background-color:transparent" readonly="readonly">';
      
        modal += '</div>';
        modal += '</div>';       


        modal += '<div class="row">';
        modal += '<br/>';

        modal += '<div class="col-md-6">';        
        modal += '<label id="lblcurrentFile" for="currentFile">Gate Pass Attached File: </label>';
        modal += '<div class="list-group">';
        modal += '<a class="list-group-item" id="gatepassfile1" onclick="DownloadAttachment()" title="Download">';
        modal += '<label id="currentFile" class="text-success" style="cursor:pointer; font-weight:bold;"> '+ GPFileAttach +' </label>';
        modal += '</a>';            
        modal += '</div>';       
        modal += '</div>';

        modal += '<div class="col-md-6" id="approvaltracking_div">';
        modal += '<label for="txtnextapprover">Pending Approver: </label>';
        modal += '<input type="text" class="form-control input-sm" id="txtnextapprover" style="background-color:transparent" readonly="readonly">';
       
        modal += '</div>';

        modal += '</div>';

        modal += '<div class="row">';
       

        modal += '<div class="col-md-12">';

        modal += '<div class="jqxgrid jqxgrid-filterable jqxgrid-sortable jqxgrid-autoheight jqxgrid-enablehover jqxgrid-enableellipsis jqxgrid-pageable jqxgrid-virtualmode jqxgrid-pagesizeoptions jqxgrid-columnsresize jqxgrid-editable jqxgrid-custom"';
        modal += 'id="grid_item_returnslip"';
        modal += 'grid-width="100"';
        modal += 'data-url="/Transactions/GetAll_Item_ReturnSlip?HeaderKey=' + returnslip_code + '"';
        modal += 'grid-pagesizeoptions="5,10,20,50,100"';
        modal += 'grid-hide-columns="0,1,2,3,4,6,10,14,15,17,18"';
        modal += 'grid-pagermode="simple">';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '<div class="modal-footer">';

        modal += '<a class="btn btn-medium btn-gray" data-dismiss="modal">Close</a>';
        modal += '</div>';




        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
          

        $("#form_modal").html(modal);

        $("#form_modal_returnslip_details").modal("show");
        $("#form_modal_returnslip_details").css('z-index', '1000000');


       
        ini_main.element('inputtext');
        ini_main.element('inputnumber');
        ini_main.element('dropdownlist');
        initialize_jqxwidget_grid($("#grid_item_returnslip"));

                $("#grid_item_returnslip").on('rowclick', function (event) {
                    var args = event.args;
                    // row's bound index.
                    var boundIndex = args.rowindex;
                    // row's visible index.
                    var visibleIndex = args.visibleindex;
                    // right click.
                    var rightclick = args.rightclick;
                    // original event.
                    var ev = args.originalEvent;
                    var rowID = $("#grid_item_returnslip").jqxGrid('getrowid', boundIndex);
                    
                    itemforreturn = $("#grid_item_returnslip").jqxGrid('getrowdatabyid', rowID);
                
                });
    },
    // End //




    // Item-Details Modal //
    showReturnSlipItemDetails_Modal: function (returnslip_code, returndate, purpose, impexnumber, transtype, usercode, GPFileAttach, supplier, returnattachment, origName, origRetName) {

     
        
        var modal = '<div class="modal fade" id="form_modal_returnslip_details" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
        modal += '<div class="modal-dialog modal-lg">';
        modal += '<div class="modal-content">';

        modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
        modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"></button>';
        modal += '<h4 class="modal-title" id="myModalLabel">Return Slip Details</h4>';
        modal += '</div>';

        modal += '<div class="modal-body">';
        modal += '<br />';

        modal += '<div class="row">';
        modal += '<div class="form-group col-md-3">';
        modal += '<label for="txtGatePassID" style="font-size:12px; font-family: segoe ui">Gate Pass ID*:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtGatePassID" style="background-color:transparent;font-size:12px; font-family: segoe ui" value="' + returnslip_code + '" readonly="readonly">';
        modal += ' </div>';

        modal += '<div class="form-group col-md-3">';
        modal += '<label for="txtImpexRefNbr">IMPEX Ref. Number:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtImpexRefNbr" placeholder="IMPEX Ref. Number" style="background-color:transparent" value="' + impexnumber + '" readonly>';
        modal += '</div>';

        modal += '<div class="form-group col-md-6">';
        modal += '<label for="cmdSupplier">Supplier:</label>';
        modal += '<input type="text" class="form-control input-sm" id="cmdSupplier" placeholder="IMPEX Ref. Number" style="background-color:transparent" value="' + supplier + '" readonly>';
        modal += '</div>';
        modal += '</div>';

        modal += '<div class="row">';
        modal += ' <div class="col-md-3">';
        modal += '<label for="cmbTransType">Transaction Type:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtGatePassID" value= "' + transtype + '" style="background-color:transparent" readonly="readonly">';
        modal += '</div>';

        modal += '<div class="col-md-3">';
        modal += '<label for="txtReturnDate">Return Date:</label>';
        modal += '<div class="input-group input-append date form_datetime">';
        modal += '<input id="txtReturnDate" type="text" class="form-control input-sm" style="background-color:transparent" value= "' + returndate + '" readonly/>';
        modal += '<span class="input-group-addon add-on">';
        modal += '<i class="fa fa-calendar faa-tada animated-hover"';
        modal += 'style="color: #31B0D5">';
        modal += '</i>';
        modal += '</span>';
        modal += '</div>';
        modal += '</div>';

        modal += '<div class="col-md-6">';
        modal += '<label for="txtPurpose">Purpose:</label>';
        modal += '<input type="text" class="form-control input-sm" id="txtPurpose" value= "' + purpose + '" style="background-color:transparent" readonly="readonly">';
       // modal += '<textarea type="text" rows="1" class="form-control input-sm" id="txtPurpose" style="background-color:transparent;resize:none;font-size:12px; font-family: segoe ui" readonly="readonly">' + purpose + '</textarea>';
        modal += '</div>';
        modal += '</div>';

        modal += '<br/>';
        modal += '<div class="row">';
        //modal += '<div class="col-md-12">';

        modal += '<div class="col-md-6">';
        modal += '<label id="lblcurrentFile" for="currentFile">Gate Pass Attached File: </label>';
        modal += '<div class="list-group">';
        modal += '<a class="list-group-item" id="gatepassfile2" onclick="DownloadAttachment()" title="Download">';
        modal += '<label id="currentFile" class="text-success" style="cursor:pointer; font-weight:bold;"> ' + GPFileAttach + ' </label>';
        modal += '</a>';
        modal += '</div>';
        modal += '</div>';

        modal += '<div class="col-md-6">';
        modal += '<label id="lblcurrentFilereturn"  for="currentFile">Return Slip Attached File:</label><span class="asterisc" id="removeReturnAttachment" style="color:red;font-size:14px; float:right"><i class="fa fa-times-circle" title="Remove file" style="cursor:pointer; color:red" aria-hidden="true"></i></span>';
        modal += '<div class="list-group">';
        modal += '<a class="list-group-item" id="returnfile" onclick="DownloadReturnAttachment()" title="Download">';
        modal += '<label id="currentFile" class="text-success" style="cursor:pointer; font-weight:bold;"> ' + returnattachment + '&nbsp;&nbsp;' + ' </label></a>';
    
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';

  
        modal += '<div class="row">';


        modal += '<div class="col-md-12">';

        modal += '<div class="jqxgrid jqxgrid-filterable jqxgrid-sortable jqxgrid-autoheight jqxgrid-enablehover jqxgrid-enableellipsis jqxgrid-pageable jqxgrid-virtualmode jqxgrid-pagesizeoptions jqxgrid-columnsresize jqxgrid-editable jqxgrid-custom"';
        modal += 'id="grid_item_returnslip"';
        modal += 'grid-width="100"';
        modal += 'data-url="/Transactions/GetAll_Item_ReturnSlip?HeaderKey=' + returnslip_code + '"';
        modal += 'grid-pagesizeoptions="5,10,20,50,100"';       
        modal += 'grid-hide-columns="0,1,2,3,4,6,10,14,15,17,18"';
        modal += 'grid-pagermode="simple">';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        modal += '<div class="modal-footer">'; 
        modal += '<a class="btn btn-meduim btn-gray" data-dismiss="modal">Close</a>';

      

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);

        $("#form_modal_returnslip_details").modal("show");
        $("#form_modal_returnslip_details").css('z-index', '1000000');



        if (returnattachment == 'No File Attached') {

            $('#removeReturnAttachment').hide();
        } else {
            $('#removeReturnAttachment').show();
        }




        ini_main.element('inputtext');
        ini_main.element('inputnumber');
        ini_main.element('dropdownlist');
        initialize_jqxwidget_grid($("#grid_item_returnslip"));

        $("#grid_item_returnslip").on('rowclick', function (event) {
            var args = event.args;
            // row's bound index.
            var boundIndex = args.rowindex;
            // row's visible index.
            var visibleIndex = args.visibleindex;
            // right click.
            var rightclick = args.rightclick;
            // original event.
            var ev = args.originalEvent;
            var rowID = $("#grid_item_returnslip").jqxGrid('getrowid', boundIndex);

            itemforreturn = $("#grid_item_returnslip").jqxGrid('getrowdatabyid', rowID);

        });
    },
    // End //



};

$(document).delegate("#cmdTestNo", "click", function () {
    var selectedrowindexes = $('#grid_item_returnslip').jqxGrid('selectedrowindexes');
    var item_id_arr = [];

    for (var ctr = 0; ctr < selectedrowindexes.length; ctr++){
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
    if(ctr == 5){
    
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
              
                processData: false,
                contentType: false,
                beforeSend: function () {

                },
                headers: {
                  
                },
                success: function (response) {
                    
                    if (response.success) {
                        
                        notification_modal("Update Record", msg, "success");

                        $('#btn_notifClose').click(function () {
                          
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

// End //


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

// End Function//

//Load Category and Type//
$(document).delegate("#cmbItem", 'change', function () {
    $.ajax({
        url: 'Items/GetItemDetailsByItemCode',
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
$(document).delegate("#grid_returnslip_searchField", "keyup", function (e) {
    var columns = ["gate_pass_id_string", "purpose_string", "return_date_date"];
    generalSearch($('#grid_returnslip_searchField').val(), "grid_returnslip", columns, e);


});
// End//

// Search function //
$(document).delegate("#gridAllTrans_searchField", "keyup", function (e) {
   var columns = ["gate_pass_id_string", "purpose_string", "return_date_date", "impex_ref_number_string", "supplier_name_string", "contact_name_string"];
   generalSearch($('#gridAllTrans_searchField').val(), "gridAllTrans", columns, e);
    
});
// End//

$(document).delegate("#gridAllTransDept_searchField", "keyup", function (e) {
    var columns = ["gate_pass_id_string", "purpose_string", "return_date_date", "impex_ref_number_string", "supplier_name_string", "contact_name_string"];
    generalSearch($('#gridAllTransDept_searchField').val(), "gridAllTransDept", columns, e);

});


// show details //
$(document).delegate(".Details", "click", function () {


    var rowID = $("#grid_returnslip").jqxGrid('getrowid', item_returnslipIndex);
    headerdetails_for_RS = $("#grid_returnslip").jqxGrid('getrowdatabyid', rowID);
    show_modal.show_Itemdetails_returnslip(
        headerdetails_for_RS.gate_pass_id_string, 
        headerdetails_for_RS.return_date_datetime,
        headerdetails_for_RS.purpose_string,
        headerdetails_for_RS.impex_ref_number_string,
        headerdetails_for_RS.transaction_type_string,
         headerdetails_for_RS.usercode_string);

    var attachment = headerdetails_for_RS.attachment_string;
   
    $('#currentFile').text((attachment == "") ? "No file chosen" : attachment);

    returnslip_usercode = headerdetails_for_RS.usercode_string;
});
// end function //

// show gate pass details yee//
$(document).delegate(".gridAllTrans_Details", "click", function () {
   
    var rowID = $("#gridAllTrans").jqxGrid('getrowid', indexAllTrans);
    gatepass_details = $("#gridAllTrans").jqxGrid('getrowdatabyid', rowID);
    var dateofreturn;
    var GPFileAttach;
    var attachment = gatepass_details.attachment_string;
    var returndate = gatepass_details.return_date_date;

    if (returndate == "" || returndate == null) {
        dateofreturn = "";
    } else {
        dateofreturn = returndate.toISOString().slice(0, 10);
    }

    var gatepassOriginalFileName = gatepass_details.attachment_string;
    originalFileName = gatepass_details.attachment_string;

    if (attachment == "" || attachment == null){
         GPFileAttach = "No File Attached"
    }else{
        GPFileAttach = attachment.substring(gatepass_details.attachment_string.indexOf('_') + 1);
    }


    
    show_modal.show_Itemdetails_returnslip(
        gatepass_details.gate_pass_id_string,
        dateofreturn,
        gatepass_details.purpose_string,
        gatepass_details.impex_ref_number_string,
        gatepass_details.transaction_type_string,
        gatepass_details.usercode_string,
        GPFileAttach,
        gatepass_details.supplier_name_string,
        gatepassOriginalFileName);
        returnslip_usercode = gatepass_details.usercode_string;
      dbase_operation_Item.approvaltracking(gatepass_details.gate_pass_id_string);
       
});
// end function //





// Show More Detail of Return Slip //
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


//rherejias 1/3/2017 modified return slip "details" function added return slip file 
$(document).delegate('.grid_returnslip_Details', 'click', function () {
    
    var rowID = $("#grid_returnslip").jqxGrid('getrowid', item_returnslipIndex);
    headerdetails_for_RS = $("#grid_returnslip").jqxGrid('getrowdatabyid', rowID);
    var dateofreturn;
    var datereturn = headerdetails_for_RS.return_date_date;
    if (datereturn == "" || datereturn == null) {
        dateofreturn = "";
    } else {
        dateofreturn = datereturn.toISOString().slice(0, 10);
    }


    var attachment = headerdetails_for_RS.attachment_string;
    var returnattachment;

    if (headerdetails_for_RS.return_slip_attachment_string == null || headerdetails_for_RS.return_slip_attachment_string == "") {
        returnattachment = "No File Attached";
        $('#removeReturnAttachment').hide();
    }
    else {
        returnattachment = headerdetails_for_RS.return_slip_attachment_string.substring(headerdetails_for_RS.return_slip_attachment_string.indexOf('_') + 1);
        $('#removeReturnAttachment').show();
    }


    if (attachment == "" || attachment == null) {
        GPFileAttach = "No File Attached"
    } else {
        GPFileAttach = attachment.substring(headerdetails_for_RS.attachment_string.indexOf('_') + 1);
    }

    returnOriginalFileName = headerdetails_for_RS.return_slip_attachment_string;
    gatepassOriginalFileName = headerdetails_for_RS.attachment_string;
    originalFileName = headerdetails_for_RS.attachment_string;

    show_modal.showReturnSlipItemDetails_Modal(
       headerdetails_for_RS.gate_pass_id_string,
       dateofreturn,
       headerdetails_for_RS.purpose_string,
       headerdetails_for_RS.impex_ref_number_string,
       headerdetails_for_RS.transaction_type_string,
       headerdetails_for_RS.usercode_string,
       GPFileAttach,
       headerdetails_for_RS.supplier_name_string,
       returnattachment,
       gatepassOriginalFileName,
       returnOriginalFileName);

    returnslip_usercode = headerdetails_for_RS.usercode_string;
});
// End //

$(document).delegate('#cmdReturn', 'click', function () {
    var selectindex = $('#grid_item_returnslip').jqxGrid('selectedrowindexes');
    var hasselected = false;
    for (var ctr = 0; ctr < selectindex.length; ctr++) {
        hasselected = true;
        show_modal.confirmationModal_returnItem('Confirmation', 'Are you sure you want to continue?', 'Success');
    }

    if (hasselected == false) {

        $("#form_modal_returnslip_details").css('z-index', '100000');
        notification_modal("Warning", "Please check first the item you want to return!", "danger");    

        $('#btn_notifClose').click(function () {
            $('#notification_modal .close').click();
            $("#form_modal_returnslip_details").css('z-index', '100000000000');
        });
    }
    
});

$(document).delegate('#cmdReturnItem', 'click', function () {

    selectedrowindexes = $('#grid_item_returnslip').jqxGrid('selectedrowindexes');
   
    var item_id_arr = [];

    for (var ctr = 0; ctr < selectedrowindexes.length; ctr++) {
       
            var rowID = $("#grid_item_returnslip").jqxGrid('getrowid', selectedrowindexes[ctr]);
            itemforreturn = $("#grid_item_returnslip").jqxGrid('getrowdatabyid', rowID);
            item_id_arr.push(itemforreturn.id_number);
           
        }
        
    dbase_operation_Item.UpdateReturnedItem(item_id_arr, itemforreturn.header_code_string, returnslip_usercode);
});



// back button
$("#cmdBackmyTrans").click(function () {

    $('#mytransactionpage').show();
    $('#hardcopypage').hide();

});


// show gate pass details //
$(document).delegate(".gridAllTrans_PrintableCopy", "click", function () {
    var rowID = $("#gridAllTrans").jqxGrid('getrowid', indexAllTrans);
    gatepass_details = $("#gridAllTrans").jqxGrid('getrowdatabyid', rowID);

    if ($('#cmbStatus').val() == 'Rejected') {
 
        $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'Printable copy is not applicable on Rejected status.');
        Warning_ToastNotif();
    }
    else if ($('#cmbStatus').val() == 'Approved')
    {

        $('#cmdBackmyTrans').show();
            $('#mytransactionpage').hide();
            $('#hardcopypage').show();        
            var returndate = gatepass_details.return_date_date
            if (returndate == "" || returndate == null) {
                $('#tdreturndate').html("");
            } else {
                $('#tdreturndate').html(returndate.toISOString().slice(0, 10));
            }
            $('#tdGPId').html(gatepass_details.gate_pass_id_string);
            $('#tdpurpose').html(gatepass_details.purpose_string);
            $('#tdimpexref').html(gatepass_details.impex_ref_number_string);
            $('#tdsupplier').html(gatepass_details.supplier_name_string);
            $('#tdcontactname').html(gatepass_details.supplier_address_string);

            $("#girdforpirnt").attr('data-url', "/Transactions/GetAll_Item_ReturnSlip" + '?HeaderKey=' + gatepass_details.gate_pass_id_string);
            initialize_jqxwidget_grid($("#girdforpirnt"));


    } else {

   
        dbase_operation_Item.approvaltrackingforprintable(gatepass_details.gate_pass_id_string);
    }

});
// end function //

// this is for My Department Table and Function

$("#cmbStatusDept").change(function () {

    applyFilter('status_string', $(this).attr('data-grid'), $(this).val());
});


$("#gridAllTransDept").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#gridAllTransDept").jqxGrid('getrowid', boundIndex);
    var data = $("#gridAllTransDept").jqxGrid('getrowdatabyid', rowID);

    indexAllTrans = boundIndex;
});
// show gate pass details yee//
$(document).delegate(".gridAllTransDept_Details", "click", function () {

    var rowID = $("#gridAllTransDept").jqxGrid('getrowid', indexAllTrans);
    gatepass_details = $("#gridAllTransDept").jqxGrid('getrowdatabyid', rowID);
    var dateofreturn;
    var GPFileAttach;
    var attachment = gatepass_details.attachment_string;
    var returndate = gatepass_details.return_date_date;

    if (returndate == "" || returndate == null) {
        dateofreturn = "";
    } else {
        dateofreturn = returndate.toISOString().slice(0, 10);
    }

    var gatepassOriginalFileName = gatepass_details.attachment_string;
    originalFileName = gatepass_details.attachment_string;

    if (attachment == "" || attachment == null) {
        GPFileAttach = "No File Attached"
    } else {
        GPFileAttach = attachment.substring(gatepass_details.attachment_string.indexOf('_') + 1);
    }



    show_modal.show_Itemdetails_returnslip(
        gatepass_details.gate_pass_id_string,
        dateofreturn,
        gatepass_details.purpose_string,
        gatepass_details.impex_ref_number_string,
        gatepass_details.transaction_type_string,
        gatepass_details.usercode_string,
        GPFileAttach,
        gatepass_details.supplier_name_string,
        gatepassOriginalFileName);
    returnslip_usercode = gatepass_details.usercode_string;
    dbase_operation_Item.approvaltracking(gatepass_details.gate_pass_id_string);

});
// end function //

// show gate pass details //
$(document).delegate(".gridAllTransDept_PrintableCopy", "click", function () {
    var rowID = $("#gridAllTransDept").jqxGrid('getrowid', indexAllTrans);
    gatepass_details = $("#gridAllTransDept").jqxGrid('getrowdatabyid', rowID);

    if ($('#cmbStatus').val() == 'Rejected') {

        $('.warning').html('<i class="fa fa-exclamation-triangle fa-lg"></i>' + ' ' + 'Printable copy is not applicable on Rejected status.');
        Warning_ToastNotif();
    }
    else if ($('#cmbStatus').val() == 'Approved') {

        $('#cmdBackmyTrans').show();
        $('#mytransactionpage').hide();
        $('#hardcopypage').show();
        var returndate = gatepass_details.return_date_date
        if (returndate == "" || returndate == null) {
            $('#tdreturndate').html("");
        } else {
            $('#tdreturndate').html(returndate.toISOString().slice(0, 10));
        }
        $('#tdGPId').html(gatepass_details.gate_pass_id_string);
        $('#tdpurpose').html(gatepass_details.purpose_string);
        $('#tdimpexref').html(gatepass_details.impex_ref_number_string);
        $('#tdsupplier').html(gatepass_details.supplier_name_string);
        $('#tdcontactname').html(gatepass_details.supplier_address_string);

        $("#girdforpirnt").attr('data-url', "/Transactions/GetAll_Item_ReturnSlip" + '?HeaderKey=' + gatepass_details.gate_pass_id_string);
        initialize_jqxwidget_grid($("#girdforpirnt"));


    } else {


        dbase_operation_Item.approvaltrackingforprintable(gatepass_details.gate_pass_id_string);
    }
    // Search function //
  
});
// end function //


