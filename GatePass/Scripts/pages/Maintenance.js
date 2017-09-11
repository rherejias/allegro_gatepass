var mCode;
var datatable,datatableContact;
var index, indexContact;

$(document).delegate("#SupplierGrid_Export_to_Excel", "click", function () {
    $("#SupplierGrid").jqxGrid('exportdata', 'xls', 'Supplier_Details');
    $.ajax({
        url: '/Supplier/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Supplier',
            table: 'tblSuppliers'
        }
    });
});

$(document).delegate("#SupplierGrid_Clear_Filter", "click", function () {
    $('#SupplierGrid').jqxGrid('clearselection');
    $('#ContactPersonGrid').jqxGrid('clearfilters');
    $('#cmdAddContact').css("display", "none");
});

$(document).delegate("#SupplierGrid_Upload_Excel_File", "click", function () {
    showform.upload();
});

$(document).delegate("#btnProceedUpload", "click", function () {
    var ext = $("#inpt_file").val().split('.').pop();
    if ($("#inpt_file").val() != "" && ext == "xlsx" || ext == "xls") {
        dbaseOperations.upload();
        $.ajax({
            url: '/Supplier/upload',
            type: 'get',
            dataType: 'json',
        });
    }
});

$(document).delegate("#ContactPersonGrid_Export_to_Excel", "click", function () {
    $("#ContactPersonGrid").jqxGrid('exportdata', 'xls', 'ContactPerson_Details');
    $.ajax({
        url: '/Supplier/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_ContactPerson',
            table: 'tblContactPerson'
        }
    });
});

$("#cmdAddNew").click(function () {
    showform.supplier('add');
    $('.modal-title').text('Add Supplier');
});

$("#cmdAddContact").click(function () {
    showform.ContactPerson('add');
});

$("#SupplierGrid").on('rowdoubleclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;
    
   // showform.supplier('edit', boundIndex);
});

$("#ContactPersonGrid").on('rowdoubleclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;
    
    
   // showform.ContactPerson('edit', boundIndex);
});

$("#ContactPersonGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#ContactPersonGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#ContactPersonGrid").jqxGrid('getrowdatabyid', rowID);
    datatableContact = $("#ContactPersonGrid").jqxGrid('getrowdatabyid', rowID);
    indexContact = boundIndex;
});

$('#SupplierGrid').on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#SupplierGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#SupplierGrid").jqxGrid('getrowdatabyid', rowID);
    datatable = $("#SupplierGrid").jqxGrid('getrowdatabyid', rowID);
    index = boundIndex;
    console.log(data.code_string);
    applyFilter('supplier_key_string', $(this).attr('grid-child'), data.code_string);
    document.getElementById('cmdAddContact').style.display = 'inline';
    mCode = data.code_string;
});

$(document).delegate("#AddItemSaveButton", "click", function () {
    dbaseOperations.save('add', '');
});

$(document).delegate("#AddItemUpdateButton", "click", function () {
    //if ($("#chkIsAcive").is(":checked")) {
    //    IsActive = 1;
    //}
    dbaseOperations.save('edit', $(this).attr("data-recid"));
});

$(document).delegate("#cmdAddItemContact", "click", function () {
    dbaseOperations.SaveContactPerson('add', '');
});

$(document).delegate("#cmdUpdateContactPerson", "click", function () {
    dbaseOperations.SaveContactPerson('edit', $(this).attr("data-recid"));
});

$(document).delegate(".SupplierGrid_Inactive", "click", function (event) {
    showform.deactivate(datatable.id_number,datatable.code_string, 'Supplier')
});

$(document).delegate(".SupplierGrid_Edit", "click", function () {
    
    showform.supplier('edit', index);
    $('.modal-title').text('Update Supplier');
});

$(document).delegate("#btnProceedDeactivate", "click", function () {
    dbaseOperations.deactivate($(this).attr('data-source'), $(this).attr('data-id'), $(this).attr('itemID'))
});

$(document).delegate(".ContactPersonGrid_Inactive", "click", function (event) {
    showform.deactivate(datatableContact.id_number,datatableContact.code_string, 'Contact')
   
});

$(document).delegate(".ContactPersonGrid_Edit", "click", function () {
    showform.ContactPerson('edit', indexContact);
});



$(document).delegate("#SupplierGrid_searchField", "keyup", function (e) {
    var columns = ["contact_number_string", "name_string", "email_string", "unit_string", "street_name_string", "barangay_string", "city_string", "country_string", "zip_string"];
   
    generalSearch($('#SupplierGrid_searchField').val(),"SupplierGrid",columns,e);
});

$(document).delegate("#ContactPersonGrid_searchField", "keyup", function (e) {
    var columns = ["Contact_Number_string", "Email_string", "Department_string",
                   "First_Name_string", "Last_Name_string", "Middle_Name_string"];
    generalSearch($('#ContactPersonGrid_searchField').val(), "ContactPersonGrid", columns, e);
});

var showform = {

    supplier: function (operation, boundIndex) {
        var id = '';
        var code = '';
        var name = '';
        var email = '';
        var contactNumb = '';
        var unitNum = '';
        var street = '';
        var municipality = '';
        var city = '';
        var country = '';
        var zip = '';
        var impexnumber = '';
        var IsActive = '';
        

        if (operation != "add") {

            var rowID = $("#SupplierGrid").jqxGrid('getrowid', boundIndex);
            var data = $("#SupplierGrid").jqxGrid('getrowdatabyid', rowID);

            id = data.id_number;
            code = data.code_string;
            name = data.name_string;
            email = data.email_string;
            contactNumb = data.contact_number_string;
            unitNum = data.unit_string;
            street = data.street_name_string;
            municipality = data.barangay_string;
            city = data.city_string;
            country = data.country_string;
            zip = data.zip_string;
            impexnumber = data.impex_number_string;
            if (data.active_bool) {
                IsActive = 'checked = "checked"';
                var IsActive2 = 1;
            }
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
        modal += '      border-right: 0;'
        modal += '      border-bottom: 1px solid #e5e5e5;';
        modal += '}';
        modal += '</style>';

        modal += '<div class="modal fade" id="modal123" role="dialog" >';
        modal += '<div class="modal-dialog">';
        modal += '<div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal += '<h4 class="modal-title"></h4>';
        modal += '</div>';
        modal += '<div class="modal-body" style="margin-top:3%;">';
        modal += '<div class="row">';
        modal += '<div class="col-md-12">';
        modal += '<div class="col-md-12">';
        modal += '<div class="form-group">';
      
        modal += '<input id="CompanyName" type="text" placeholder="*Company Name" class="form-control companyrequired textstyle" value="' + name + '"/>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        modal += '<div class="row">';
        modal += '  <input id="Code" type="hidden" value="' + code + '" />';
        modal += '  <div class="col-md-12">';
        modal += '      <div class="col-md-6">';
        modal += '          <div class="form-group">';
   
        modal += '              <input id="EmailAddress" type="text"  placeholder="*Email" class="form-control companyrequired textstyle"  value="' + email + '"/>';
        modal += '          </div>';
        modal += '      </div>';

        modal += '      <div class="col-md-6">';
        modal += '          <div class="form-group">';
        modal += '              <input id="ContactNumber" type="text" placeholder="*Contact Number" class="form-control companyrequired textstyle" value="' + contactNumb + '"/>';
        modal += '          </div>';
        modal += '      </div>';
        modal += '  </div>';
        modal += '</div>';


        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '      <div class="col-md-6">';
        modal += '          <div class="form-group">';
      
        modal += '              <input id="ComUnitCode" type="text" placeholder="*Unit Number" class="form-control companyrequired textstyle"  value="' + unitNum + '"/>';
        modal += '          </div>';
        modal += '      </div>';
        modal += '      <div class="col-md-6">';
        modal += '          <div class="form-group">';
      
        modal += '              <input id="ComBldgName" type="text" placeholder="*Bldg/Street Name" class="form-control companyrequired textstyle" value="' + street + '"/>';
        modal += '          </div>';
        modal += '      </div>';
        modal += '  </div>';
        modal += '</div>';

        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';

        modal += '      <div class="col-md-6">';
        modal += '          <div class="form-group">';
      
        modal += '              <input id="ComMunicipality" type="text" placeholder="*Barangay" class="form-control companyrequired textstyle" value="' + municipality + '"/>';
        modal += '          </div>';
        modal += '      </div>';
        modal += '      <div class="col-md-6">';
        modal += '          <div class="form-group">';
      
        modal += '              <input id="ComCity" type="text" placeholder="*City" class="form-control companyrequired textstyle" value="' + city + '"/>';
        modal += '          </div>';
        modal += '      </div>';
        modal += '  </div>';
        modal += '</div>';

        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '      <div class="col-md-6">';
        modal += '          <div class="form-group">';
      
        modal += '              <input id="ComCountry" type="text" placeholder="*Country" class="form-control companyrequired textstyle" value="' + country + '"/>';
        modal += '          </div>';
        modal += '      </div>';
        modal += '      <div class="col-md-6">';
        modal += '          <div class="form-group">';
       
        modal += '              <input id="ComZipCode" type="number" placeholder="*Zip" class="form-control companyrequired textstyle" value="' + zip + '"/>';
        modal += '          </div>';
        modal += '      </div>';

        modal += '<div class="row" style="display:none">';
        modal += '  <div class="col-md-12">';
        modal += '      <div class="col-md-6">';
        modal += '          <div class="form-group">';
        modal += '              <input id="txtimpexnum" type="text" placeholder="*IMPEX Reference Number" class="form-control companyrequired textstyle" value="' + impexnumber + '"/>';
        modal += '          </div>';
        modal += '      </div>';
        modal += '      </div>';
        modal += '      </div>';
        modal += '  </div>';
        modal += '</div>';


        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-medium btn-blue" data-recid="' + id + '" id="' + ((operation == 'add') ? 'AddItemSaveButton' : 'AddItemUpdateButton') + '"';
        modal += 'style="width: 100px;">';
        modal += ''+((operation == 'add') ? 'Add' : 'Update' )+'</a>';
        modal += '<a class="btn btn-medium btn-gray" type="button" style="width: 100px;" data-dismiss="modal">Cancel</a> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';



        $("#form_modal").html(modal);
        $("#modal123").modal("show");
        $("#modal123").css('z-index', '1000000');
    },

    deactivate: function (id,code, source) {
        var modal = '<div class="modal fade" id="modalDeactivate" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#F25656; color:#ffffff">';
        modal += '<h4 class="modal-title">'+((source == "Supplier") ? 'Inactive Supplier':'Inactive Contact Person')+'</h4>';
        modal += '</div>';


        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to be inactive this record?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-small btn-red" id="btnProceedDeactivate"';
        modal += 'data-source="' + source + '" data-id="' + code + '" itemID="'+id+'">';
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

    ContactPerson: function (operation, boundIndex,title) {
        var id = '';
        var firstname = '';
        var middlename = '';
        var lastname = '';
        var email = '';
        var contactnumber = '';
        var department = '';
  
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

        var modal2 = '<style>';
        modal2 += ' .textstyle {';
        modal2 += '      background-color: transparent;';
        modal2 += '      outline: none;';
        modal2 += '      outline-style: none;';
        modal2 += '      outline-offset: 0;';
        modal2 += '      border-top: none;';
        modal2 += '      border-left: none;';
        modal2 += '      border-right: none;';
        modal2 += '      border-bottom: 1px solid #e5e5e5;';
        modal2 += '      padding: 3px 10px;';
        modal2 += '}';
        modal2 += ' .form-control:focus {';
        modal2 += '      border-top: 0;';
        modal2 += '      border-left: 0;';
        modal2 += '      border-right: 0;';
        modal2 += '      border-bottom: solid 1px;';
        modal2 += '}';
        modal2 += '</style>';

        modal2 += '<div class="modal fade" id="modalContactPerson" role="dialog" >';
        modal2 += '<div class="modal-dialog">';
        modal2 += ' <div class="modal-content">';

        modal2 += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal2 += '<h4 class="modal-title">' + ((operation == "edit") ? 'Update Contact Person' : 'Add Contact Person') + '</h4>';
        modal2 += '</div>';
        modal2 += '<div class="modal-body" style="margin-top:3%;">';

        modal2 += '<div class="row">';
        modal2 += '  <div class="col-md-12">';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
    
        modal2 += '              <input id="Email" type="text" placeholder="*E-mail" class="form-control companyrequired textstyle" value= "' + email + '"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
 
        modal2 += '              <input id="ContactNumber" type="text" placeholder="*Contact Number" class="form-control companyrequired textstyle" value= "' + contactnumber + '"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';

        modal2 += '              <input id="Department" type="text" placeholder="*Department" class="form-control companyrequired textstyle" value= "' + department + '"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '  </div>';
        modal2 += '</div>';

        modal2 += '<div class="row">';
        modal2 += '  <input id="Code" type="hidden" value="' + code + '" />';
        modal2 += '  <div class="col-md-12">';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
      
        modal2 += '              <input id="FirstName" type="text" placeholder="*First Name" class="form-control companyrequired textstyle" value= "' + firstname + '"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        
        modal2 += '              <input id="MiddleName" type="text" placeholder="*Middle Name" class="form-control companyrequired textstyle" value= "' + middlename + '"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
     
        modal2 += '              <input id="LastName" type="text" placeholder="*Last Name" class="form-control companyrequired textstyle" value= "' + lastname + '"/>';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '  </div>';
        modal2 += '</div>';
       
      
        modal2 += '<div class="modal-footer">';
        modal2 += '<div class="row">';
        modal2 += '<a class="btn btn-medium btn-blue" data-recid="' + id + '" id="' + ((operation == 'add') ? 'cmdAddItemContact' : 'cmdUpdateContactPerson') + '"';
        modal2 += 'style="width: 100px;">';
        modal2 += ''+((operation == 'add') ? 'Add' : 'Update' )+'</a>';
        modal2 += '<a type="button" style="width: 100px;" class="btn btn-medium btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
        modal2 += '</div>';
        modal2 += '</div>';

        modal2 += '</div>';

        modal2 += '</div>';
        modal2 += '</div>';
        modal2 += '</div>';

        $("#form_modal").html(modal2);
        $("#modalContactPerson").modal("show");
        $("#modalContactPerson").css('z-index', '1000000');

        $("#numericInput").jqxNumberInput({ width: '154px', height: '31px', inputMode: 'simple', decimalDigits: 0 });
    },

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
        modal += '<h4 class="modal-title">File Upload</h4>';
        modal += '<div class="loader"  style="float:right; top:-26px; position:relative; display:none"></div>';
        modal += '</div>';
 
        modal += '<div class="modal-body" style="margin-top:8%;">';
        modal += '<input type="file" id="inpt_file" name="inpt_file" accept="application/vnd.ms-excel" style="color:#F25656; font-weight:bold; font-size:1.1em;" />';
        modal += '</br>';
        modal += '<label style="color:red;" >NOTE: Excel file only</label>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-small btn-blue" id="btnProceedUpload"';
        modal += 'style="width: 80px;">';
        modal += 'Upload</a>';
        modal += '<a id="btnCancel" "type="button" style="width: 80px;" class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalUpload").modal("show");
        $("#modalUpload").css('z-index', '1000000');
    },
};

//impexadd

var dbaseOperations = {
    save: function (operation, trans_id) {
        //for validation if textbox is empty
        var elements = ["#CompanyName"];
        var ctr = 0;
        for (var i = 0; i < 1; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "red");
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5");
                ctr++;
            }
        }

        //if all textbox is filled
        if (ctr == 1) {
            var url = '';
            var msg = '';
            //var IsActive = false;
            if (operation == 'add') {
                url = '/Supplier/AddSupplier';
                msg = ' Supplier created successfully.';
                title = 'Add Record';
            } else {
                url = '/Supplier/UpdateSupplier';
                msg = ' Supplier updated successfully.';
                title = 'Update Record';
            }
           
            $.ajax({
                url: url,
                dataType: 'json',
                type: 'get',
                data: {
                    id: trans_id,
                    code: $("#Code").val(),
                    Name: $("#CompanyName").val(),
                    Email: $("#EmailAddress").val(),
                    ContactNbr: $("#ContactNumber").val(),
                    UnitNbr: $("#ComUnitCode").val(),
                    StreetName: $("#ComBldgName").val(),
                    Municipality: $("#ComMunicipality").val(),
                    City: $("#ComCity").val(),
                    Country: $("#ComCountry").val(),
                    Zip: $("#ComZipCode").val(),
                    ImpexRefNbr: $("#txtimpexnum").val(),
                    //isactive: IsActive
                },
                beforeSend: function () {

                },
                headers: {
                    //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                },
                success: function (response) {
                    $("#modal123").modal("hide");
                    if (response.success) {
                        toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " "+ msg;
                        Save_ToastNotifMaintenance();
                        $('#SupplierGrid').jqxGrid('updatebounddata');
                    } else {
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

    SaveContactPerson: function (operation, trans_id) {
        //for validation if textbox is empty
        var elements = ["#FirstName", "#MiddleName", "#LastName", "#Email", "#ContactNumber"]
        var ctr = 0;
        for (var i = 0; i <= 4; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "red");
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5");
                ctr++;
            }
        }

        //if all textbox is filled
        if (ctr == 5) {
            var url = '';
            var msg = '';
            //var IsActive = false;
            if (operation == 'add') {
                url = '/ContactPerson/AddContactPerson';
                msg = ' Contact person created successfully.';
                title = 'Add Record';
            } else {
                url = '/ContactPerson/UpdateContactPerson';
                msg = ' Contact person updated successfully.';
                title = 'Update Record';
            }
           
            $.ajax({
                url: url,
                dataType: 'json',
                type: 'get',
                data: {
                    id: trans_id,
                    Code: $("#Code").val(),
                    SupplierKey: mCode,
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
                    $("#modalContactPerson").modal("hide");
                    if (response.success) {
                        toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " " +msg;
                        Save_ToastNotifMaintenance();
                        $('#ContactPersonGrid').jqxGrid('updatebounddata');
                    } else {
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

    deactivate: function (source, code, id) {
        var url = '';
        var msg = '';
        if (source == "Supplier") {
            url = '/Supplier/DeactivateSupplier';
            msg = ' Record deactivated successfully.';
        } else if (source == "Contact") {
            url = '/ContactPerson/DeactivateContact';
            msg = ' Record deactivated successfully.';
        }
        $.ajax({
            url: url,
            dataType: 'json',
            type: 'get',
            data: {
                Id: id,
                code: code,
                isactive: false
            },
            beforeSend: function () {

            },
            success: function (response) {
                $("#modalDeactivate").modal("hide");
                if (response.success) {

                    toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' +" "+ msg;
                    Save_ToastNotifMaintenance();
                    if (source == "Supplier")
                        $('#SupplierGrid').jqxGrid('updatebounddata');
                    else if (source == "Contact")
                        $('#ContactPersonGrid').jqxGrid('updatebounddata');
                } else {
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

    upload: function () {
        var formData = new FormData();
        formData.append('inpt_file', $('input[name="inpt_file"]')[0].files[0]);
      
        $.ajax({
            url: '/Supplier/TargetUpload',
            type: 'post',
            dataType: 'json',
            //async: false,
            data: formData,
            processData: false,
            contentType: false,
            beforeSend: function () {
                $(".loader").css('display', 'inline');
                $("#btnProceedUpload").attr("disabled", true);
                $("#btnCancel").attr("disabled", true);
            },
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {
                $("#modalUpload").modal("hide");
                if (response.success) {
                    $('#SupplierGrid').jqxGrid('updatebounddata');
                    toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Upload successful.";
                    Save_ToastNotifMaintenance();
                }
                else {
                    toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + " " + "Uploaded file is empty or is in the wrong format.";
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

var applyFilter = function (datafield, childgrid, _code) {
    if (typeof childgrid == "undefined" || childgrid == '' || typeof childgrid=="null") {
        console.log("Child grid not set!");
    }else{
        //static counter to prevent infinite looping
        var ctr = 0;
        $("#" + childgrid).jqxGrid('clearfilters');
        var filtertype = 'stringfilter';
        var filtergroup = new $.jqx.filter();

        var filter_or_operator = 0;
        var filtervalue = "'" + _code + "'";
        var filtercondition = 'equal';
        var filter = filtergroup.createfilter(filtertype, filtervalue, filtercondition);
        filtergroup.addfilter(filter_or_operator, filter);

        $("#" + childgrid).on("bindingcomplete", function (event) {
            if (ctr == 0) {
                $("#" + childgrid).jqxGrid('addfilter', datafield, filtergroup);
                $("#" + childgrid).jqxGrid('applyfilters');
            }
            ctr = 1;
        });
    }
}



