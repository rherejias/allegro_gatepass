

$("#cmdAddContact").click(function () {
    alert('a');
    showform.ContactPerson();
});

$("#gridAsdf").on('rowdoubleclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    showform.supplier('edit', boundIndex);
});

$(document).delegate("#AddItemSaveButton", "click", function () {
    dbaseOperations.save('add', '');
});

$(document).delegate("#AddItemUpdateButton", "click", function () {
    if ($("#chkIsAcive").is(":checked")) {
        IsActive = 1;
    }
    dbaseOperations.save('edit', $(this).attr("data-recid"));
});


var showform = {
    ContactPerson: function () {


        var modal2 = '<div class="modal fade" id="modalContactPerson" role="dialog" >';
        modal2 += '<div class="modal-dialog">';
        modal2 += ' <div class="modal-content">';

        modal2 += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal2 += '<h4 class="modal-title">Add Contact Person</h4>';
        modal2 += '</div>';

        modal2 += '<div class="modal-body">';

        modal2 += '<div class="row">';
        modal2 += '  <div class="col-md-12">';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        modal2 += '              <label class="control-label">';
        modal2 += '                  First Name</label>';
        modal2 += '              <input id="FirstName" type="text" class="form-control companyrequired" />';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        modal2 += '              <label class="control-label">';
        modal2 += '                  Middle Name</label>';
        modal2 += '              <input id="MiddleName" type="text" class="form-control companyrequired" />';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        modal2 += '              <label class="control-label">';
        modal2 += '                  Last Name</label>';
        modal2 += '              <input id="LastName" type="text" class="form-control companyrequired" />';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '  </div>';
        modal2 += '</div>';

        modal2 += '<div class="row">';
        modal2 += '  <div class="col-md-12">';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        modal2 += '              <label class="control-label">';
        modal2 += '                 Email</label>';
        modal2 += '              <input id="Email" type="text" class="form-control companyrequired" />';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        modal2 += '              <label class="control-label">';
        modal2 += '                  Contact Number</label>';
        modal2 += '              <input id="ContactNumber" type="text" class="form-control companyrequired" />';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '      <div class="col-md-4">';
        modal2 += '          <div class="form-group">';
        modal2 += '              <label class="control-label">';
        modal2 += '                  Department</label>';
        modal2 += '              <input id="Department" type="text" class="form-control companyrequired" />';
        modal2 += '          </div>';
        modal2 += '      </div>';
        modal2 += '  </div>';
        modal2 += '</div>';

        modal2 += '<div class="modal-footer">';
        modal2 += '<div class="row">';
        modal2 += '<button type="button" style="width: 100px;" class="btn btn-default" data-dismiss="modal">Cancel</button>';
        modal2 += '<button type="button" style="width: 100px;" class="btn btn-default" id="cmdAddContact" data-dismiss="modal">Add</button>';
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

var dbaseOperations = {
    save: function (operation, trans_id) {

        var url = '';
        var msg = '';
        var IsActive = false;
        if (operation == 'add') {
            url = '/Supplier/AddSupplier';
            msg = 'New supplier added!';
        } else {
            url = '/Supplier/UpdateSupplier';
            msg = 'Supplier updated!';
        }
        if ($("#chkIsActive").is(":checked")) {
            IsActive = true;
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
                isactive: IsActive
            },
            beforeSend: function () {

            },
            headers: {
            },
            success: function (response) {
                $("#modal123").modal("hide");
                if (response.success) {
                    notification_modal("Addition Successful!", msg, "success");
                    $('#gridAsdf').jqxGrid('updatebounddata');
                    refresh.dropdownlist($("#cmbSupplier"));
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
};