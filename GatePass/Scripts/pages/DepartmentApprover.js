var selectedRow;
var selectedRowParameter;

$(document).ready(function () {
    $(document).delegate("#DepartmentApproverGrid_searchField", "keyup", function (e) {
        var columns = ["approval_type_string", "department_string", "user_string", "email_string"];
        generalSearch($('#DepartmentApproverGrid_searchField').val(), 'DepartmentApproverGrid', columns, e);
    });
});

//rherjais assign button onclick
$(document).delegate(".DepartmentApproverGrid_Assign", "click", function () {
    showForm.deactivate(selectedRow);
});

//rherejias yes button onlick
$(document).delegate("#btnProceedDepartmentApprove", "click", function () {
    $("#modalDepartmentApprover").modal("hide");
    dbase_operations.assignApprover();
});

//row click get grid details
$("#DepartmentApproverGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#DepartmentApproverGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#DepartmentApproverGrid").jqxGrid('getrowdatabyid', rowID);
    selectedRow = data.user_string;
    selectedRowParameter = data;
});


//rherejias modal for assign approver (confirmation)
showForm = {
    deactivate: function (name) {
        var modal = '<div class="modal fade" id="modalDepartmentApprover" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#1DB198; color:#ffffff">';
        modal += '<h4 class="modal-title">Department Approver</h4>';
        modal += '</div>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to assign <b>'+ name +'</b> as your temporary approver?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-small btn-green" id="btnProceedDepartmentApprove">';
        modal += 'Confirm</a>';
        modal += '<a type="button" class="btn btn-small btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalDepartmentApprover").modal("show");
        $("#modalDepartmentApprover").css('z-index', '1000000');
    },
};


//rherejias ajax call for assign approver 
//get code
// get approvertype code
// get department code
dbase_operations = {
    assignApprover: function () {

        if (selectedRowParameter.approval_type_code_string != departmentApprover) {
            $.ajax({
                url: '/DepartmentApprover/AssignApprover',
                dataType: 'json',
                type: 'get',
                data: {
                    id: selectedRowParameter.id_number,
                    module: 'DEPT_APPROVER',
                    code: selectedRowParameter.code_string,
                    type: selectedRowParameter.approval_type_code_string,
                    department: selectedRowParameter.department_code_string
                },
                beforeSend: function () {

                },
                success: function (response) {
                $("#modalDepartmentApprover").modal("hide");
                    if (response.success) {
                        toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + " Approver assigning successfully.";
                        Save_ToastNotifMaintenance();
                        $('#DepartmentApproverGrid').jqxGrid('updatebounddata');
                    } else {
                       toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' +" "+ response.message;
                         Fail_ToastNotif();
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    console.log(xhr.status);
                    console.log(thrownError);
                }
            });
        }
        else {
            toastNotif = document.getElementById("toastMaintenanceFail").innerHTML = '<i class="fa fa-close fa-lg"></i>' + "<b>" + " " + selectedRowParameter.user_string + "</b> is the assigned approver.";
            Fail_ToastNotif();
        }
    },
};