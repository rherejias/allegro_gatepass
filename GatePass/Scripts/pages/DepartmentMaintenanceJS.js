var deptInfo, deptInfoInactive;

$(document).ready(function () {
    $(document).delegate("#DepartmentMaintenanceGrid_searchField", "keyup", function (e) {
        var columns = ["department_name_string", "description_string"];
        generalSearch($('#DepartmentMaintenanceGrid_searchField').val(), 'DepartmentMaintenanceGrid', columns, e);
    });

    $(document).delegate("#DepartmentMaintenanceGridInactive_searchField", "keyup", function (e) {
        var columns = ["department_name_string", "description_string"];
        generalSearch($('#DepartmentMaintenanceGridInactive_searchField').val(), 'DepartmentMaintenanceGridInactive', columns, e);
    });
    $(document).delegate("#activeTab", "click", function () {
        $('#cmdAddNew').show();
    });
    $(document).delegate("#inactiveTab", "click", function () {
        $('#cmdAddNew').hide();
    });
});

$("#cmdAddNew").click(function () {
    show_modal.addModal('add', deptInfo);
});

$(document).delegate("#btnProceedAdd", "click", function () {
    dbase_operation.add($("#txtDeptName").val().substring(0, 1));
});

$(document).delegate("#btnProceedEdit", "click", function () {
    dbase_operation.edit($("#txtDeptName").val().substring(0, 1));
});

$(document).delegate("#btnProceedDeactivate", "click", function () {
    dbase_operation.deactivate();
});

$(document).delegate("#btnProceedActivate", "click", function () {
    dbase_operation.activate();
});

$(document).delegate(".DepartmentMaintenanceGrid_Edit", "click", function () {
    show_modal.addModal('edit', deptInfo);
});

$(document).delegate(".DepartmentMaintenanceGrid_Inactive", "click", function () {
    show_modal.deactivate();
});

$(document).delegate(".DepartmentMaintenanceGridInactive_Activate", "click", function () {
    show_modal.activate();
});


$("#DepartmentMaintenanceGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#DepartmentMaintenanceGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#DepartmentMaintenanceGrid").jqxGrid('getrowdatabyid', rowID);
    deptInfo = data;
});

$("#DepartmentMaintenanceGridInactive").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#DepartmentMaintenanceGridInactive").jqxGrid('getrowid', boundIndex);
    var data = $("#DepartmentMaintenanceGridInactive").jqxGrid('getrowdatabyid', rowID);
    deptInfoInactive = data;
});


var show_modal = {
    addModal: function (operation, index) {

        var deptname = '';
        var descname = '';


        if (operation == 'edit') {
            deptname = index.department_name_string;
            descname = index.description_string;
        }

        var modal = '<div class="modal fade" id="modalAdd" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal += '<h4 class="modal-title">' + ((operation === "edit") ? "Update Department"  : "Add Department") + '</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:3%;">';
        modal += '<input class="form-control" placeholder="Department Name" id="txtDeptName" value="' + deptname + '"></input></br>';
        modal += '<input class="form-control" placeholder="Description" id="txtDeptDesc" value="' + descname + '"></input></br>';
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
        modal += '<h4 class="modal-title">Deactivate Department</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to deactivate this Department?</p>';
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
        modal += '<h4 class="modal-title">Activate Department</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to activate this Department?</p>';
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
        var elements = ["#txtDeptName", "#txtDeptDesc", ];
        var ctr = 0;
        for (var i = 0; i < 2; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "red")
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5")
                ctr++;
            }
        }
        if (ctr == 2) {
            $.ajax({
                url: '/DepartmentMaintenance/AddDept',
                dataType: 'json',
                Type: 'post',
                data: {
                    deptname: $("#txtDeptName").val(),
                    description: $("#txtDeptDesc").val(),
                },
                success: function (response) {
                    $("#modalAdd").modal("hide");
                    $('#DepartmentMaintenanceGrid').jqxGrid('updatebounddata');
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
        var elements = ["#txtDeptName", "#txtDeptDesc"];
        var ctr = 0;
        for (var i = 0; i < 2; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "red")
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5")
                ctr++;
            }
        }
        if (ctr == 2) {
            $.ajax({
                url: '/DepartmentMaintenance/DeptEdit',
                dataType: 'json',
                Type: 'post',
                data: {
                    id: deptInfo.id_number,
                    code: deptInfo.code_string,
                    deptname: $("#txtDeptName").val(),
                    description: $("#txtDeptDesc").val(),
                },
                success: function (response) {
                    $("#modalAdd").modal("hide");
                    $('#DepartmentMaintenanceGrid').jqxGrid('updatebounddata');
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
            url: '/DepartmentMaintenance/Deactivate',
            dataType: 'json',
            Type: 'post',
            data: {
                id: deptInfo.id_number,
                code: deptInfo.code_string,
            },
            success: function (response) {
                $("#modalDeactivate").modal("hide");
                $('#DepartmentMaintenanceGrid').jqxGrid('updatebounddata');
               $('#DepartmentMaintenanceGridInactive').jqxGrid('updatebounddata');
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
            url: '/DepartmentMaintenance/Activate',
            dataType: 'json',
            Type: 'post',
            data: {
                id: deptInfoInactive.id_number,
                code: deptInfoInactive.code_string,
            },
            success: function (response) {
                $("#modalActivate").modal("hide");
                $('#DepartmentMaintenanceGrid').jqxGrid('updatebounddata');
                $('#DepartmentMaintenanceGridInactive').jqxGrid('updatebounddata');
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