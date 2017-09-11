var guardInfo, guardInfoInactive;

$(document).ready(function () {
    $(document).delegate("#GuardMaintenanceGrid_searchField", "keyup", function (e) {
        var columns = ["username_string", "password_string", "givenname_string", "lastname_string"];
        generalSearch($('#GuardMaintenanceGrid_searchField').val(), 'GuardMaintenanceGrid', columns, e);
    });

    $(document).delegate("#GuardMaintenanceGridInactive_searchField", "keyup", function (e) {
        var columns = ["username_string", "password_string", "givenname_string", "lastname_string"];
        generalSearch($('#GuardMaintenanceGridInactive_searchField').val(), 'GuardMaintenanceGridInactive', columns, e);
    });
    $(document).delegate("#activeTab", "click", function () {
        $('#cmdAddNew').show();
    });
    $(document).delegate("#inactiveTab", "click", function () {
        $('#cmdAddNew').hide();
    });
});

$("#cmdAddNew").click(function () {
    show_modal.addModal('add', guardInfo);
});

$(document).delegate("#btnProceedAdd", "click", function () {
    dbase_operation.add($("#txtGivenName").val().substring(0, 1));
});

$(document).delegate("#btnProceedEdit", "click", function () {
    dbase_operation.edit($("#txtGivenName").val().substring(0, 1));
});

$(document).delegate("#btnProceedDeactivate", "click", function () {
    dbase_operation.deactivate();
});

$(document).delegate("#btnProceedActivate", "click", function () {
    dbase_operation.activate();
});

$(document).delegate(".GuardMaintenanceGrid_Edit", "click", function () {
    show_modal.addModal('edit', guardInfo);
});

$(document).delegate(".GuardMaintenanceGrid_Inactive", "click", function () {
    show_modal.deactivate();
});

$(document).delegate(".GuardMaintenanceGridInactive_Activate", "click", function () {
    show_modal.activate();
});


$("#GuardMaintenanceGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#GuardMaintenanceGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#GuardMaintenanceGrid").jqxGrid('getrowdatabyid', rowID);
    guardInfo = data;
});

$("#GuardMaintenanceGridInactive").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#GuardMaintenanceGridInactive").jqxGrid('getrowid', boundIndex);
    var data = $("#GuardMaintenanceGridInactive").jqxGrid('getrowdatabyid', rowID);
    guardInfoInactive = data;
});


var show_modal = {
    addModal: function (operation, index) {

        var givenname = '';
        var lastname = '';
        var password = '';
        var username = '';

        if (operation == 'edit') {
            givenname = index.givenname_string;
            lastname = index.lastname_string;
            username = index.username_string;
            password = index.password_string;
        }

        var modal = '<div class="modal fade" id="modalAdd" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal += '<h4 class="modal-title">' + ((operation === "edit") ? "Update Guard" : "Add Guard") + '</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:3%;">';
        modal += '<input class="form-control" placeholder="Given Name" id="txtGivenName" value="' + givenname + '"></input></br>';
        modal += '<input class="form-control" placeholder="Last Name" id="txtLastName" value="' + lastname + '"></input></br>';
        modal += '<input class="form-control" placeholder="Username" id="txtUsername" value="' + username + '"></input></br>';
        modal += '<input class="form-control" placeholder="Password" id="txtPassword" value="' + password + '"></input>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-medium btn-blue" id="' + ((operation === "edit") ? "btnProceedEdit" : "btnProceedAdd") + '">' + ((operation === "edit") ? "Update" : "Add") + '</a>';
        modal += '<a type="button" class="btn btn-medium btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalAdd").modal("show");
        $("#modalAdd").css('z-index', '1000000');

    },

    deactivate: function () {
        var modal = '<div class="modal fade" id="modalDeactivate" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#F25656; color:#ffffff">';
        modal += '<h4 class="modal-title">Deactivate Guard</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to deactivate this Guard?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-small btn-red" id="btnProceedDeactivate">';
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

    activate: function () {
        var modal = '<div class="modal fade" id="modalActivate" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal += '<h4 class="modal-title">Activate Guard</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to activate this Guard?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-small btn-blue" id="btnProceedActivate">';
        modal += 'Confirm</a>';
        modal += '<a type="button" class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalActivate").modal("show");
        $("#modalActivate").css('z-index', '1000000');
    },
};


dbase_operation = {
    add: function (letter) {
        var elements = ["#txtUsername", "#txtPassword", "#txtGivenName", "#txtLastName"];
        var ctr = 0;
        for (var i = 0; i < 4; i++) {
            if ($(elements[i]).val() == ""){
                $(elements[i]).css("border-color", "red")
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5")
                ctr++;
            }
        }
        if (ctr == 4) {
            $.ajax({
                url: '/GuardMaintenance/Add',
                dataType: 'json',
                Type: 'post',
                data: {
                    username: $("#txtUsername").val(),
                    password: $("#txtPassword").val(),
                    givenname: $("#txtGivenName").val(),
                    lastname: $("#txtLastName").val()
                },
                success: function (response) {
                    $("#modalAdd").modal("hide");
                    $('#GuardMaintenanceGrid').jqxGrid('updatebounddata');
                    if (response.success) {
                        toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " " + response.message;
                        Save_ToastNotif();
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

    edit: function (letter) {
        var elements = ["#txtUsername", "#txtPassword", "#txtGivenName", "#txtLastName"];
        var ctr = 0;
        for (var i = 0; i < 4; i++) {
            if ($(elements[i]).val() == ""){
                $(elements[i]).css("border-color", "red")
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5")
                ctr++;
            }
        }
        if (ctr == 4) {
            $.ajax({
                url: '/GuardMaintenance/Edit',
                dataType: 'json',
                Type: 'post',
                data: {
                    id: guardInfo.id_number,
                    code: guardInfo.code_string,
                    username: $("#txtUsername").val(),
                    password: $("#txtPassword").val(),
                    givenname: $("#txtGivenName").val(),
                    lastname: $("#txtLastName").val()
                },
                success: function (response) {
                    $("#modalAdd").modal("hide");
                    $('#GuardMaintenanceGrid').jqxGrid('updatebounddata');
                    if (response.success) {
                        toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " " + response.message;
                        Save_ToastNotif();
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

    deactivate: function () {
        
            $.ajax({
                url: '/GuardMaintenance/Deactivate',
                dataType: 'json',
                Type: 'post',
                data: {
                    id: guardInfo.id_number,
                    code: guardInfo.code_string,
                },
                success: function (response) {
                    $("#modalDeactivate").modal("hide");
                    $('#GuardMaintenanceGrid').jqxGrid('updatebounddata');
                    $('#GuardMaintenanceGridInactive').jqxGrid('updatebounddata');
                    if (response.success) {
                        toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " " + response.message;
                        Save_ToastNotif();
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

    activate: function () {

        $.ajax({
            url: '/GuardMaintenance/Activate',
            dataType: 'json',
            Type: 'post',
            data: {
                id: guardInfo.id_number,
                code: guardInfoInactive.code_string,
            },
            success: function (response) {
                $("#modalActivate").modal("hide");
                $('#GuardMaintenanceGrid').jqxGrid('updatebounddata');
                $('#GuardMaintenanceGridInactive').jqxGrid('updatebounddata');
                if (response.success) {
                    toastNotif = document.getElementById("saveAsdraftAndsubmit").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " " + response.message;
                    Save_ToastNotif();
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
};