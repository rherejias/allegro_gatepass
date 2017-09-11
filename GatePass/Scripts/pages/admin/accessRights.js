/**
 * Created by aabasolo on 7/15/2016.
 */
$(document).ready(function () {
    $("#gridUsers").bind('rowselect', function (event) {
        var new_tree = '<div id="treeModules"></div>';
        $(".tree-container").html(new_tree);
        $("#treeModules").attr('data-url', $("#gridUsers").attr('data-tree-url'));
        var row = event.args.rowindex;
        var datarow = $("#gridUsers").jqxGrid('getrowdata', row);
        _ini.moduleTree('treeModules', datarow.Id_int);
    });

    $("#cmdValidate").click(function () {
        _dbase.validateUser($("#txtUsername").val());
    });

    $("#cmdSubmitDept").click(function () {
        notification_modal('Validation failed', 'Module under construction.', 'danger');
    });
});

$(document).delegate("#gridUsers_searchField", "keyup", function (e) {
    var columns = ["Username_string"];
    generalSearch($('#gridUsers_searchField').val(), 'gridUsers', columns, e);
});

$(document).delegate("#cmdAddUser", 'click', function () {
    _dbase.addUser($("#txtUsername").val());
});

var _ini = {
    moduleTree: function (elem_id, object_id) {
        console.log($("#" + elem_id).attr('data-url') + "?userId=" + object_id);

        $.ajax({
            url: $("#" + elem_id).attr('data-url'),
            dataType: 'json',
            type: 'get',
            data: {
                'userId': object_id
            },
            beforeSend: function () {

            },
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {
                //console.log(response.success);
                if (response.success) {
                    ini_tree(elem_id, response.message, object_id);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    }
}

var newUserArr = {};
var _dbase = {
    updatePermission: function (user_id, module_id, is_checked) {

        var isChecked = false;

        if (is_checked) {
            isChecked = true;
        }

        $.ajax({
            url: '/Permissions/Update',
            dataType: 'json',
            type: 'get',
            data: {
                'type': 'module',
                'parentid': 0,
                'id': module_id,
                'flag': isChecked,
                'userid': user_id
            },
            beforeSend: function () {

            },
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {
                if (response.success != true) {
                    notification_modal('An error occurred!', response.message, 'danger');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },

    validateUser: function (uname) {
        $.ajax({
            url: '/Account/Validate',
            dataType: 'json',
            type: 'get',
            data: {
                'username': uname,
                'adduser': "NO"
            },
            beforeSend: function () {

            },
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {
                if (response.success != true) {
                    notification_modal('Validation failed', response.message, 'danger');
                } else {

                    newUserArr = {
                        username: uname,
                        Photo: response.message.ThumbnailPhoto,
                        LastName: response.message.LastName,
                        GivenName: response.message.GivenName,
                        EmployeeNbr: response.message.EmployeeNbr,
                        Department: response.message.Department,
                        Email: response.message.Email
                    };

                    var modal = '<div class="modal fade" id="modal_div" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">';
                    modal += '<div class="modal-dialog">';
                    modal += '<div class="modal-content">';

                    modal += '<div class="modal-header" style="background-color: #08A7C3; color:#ffffff;">';
                    modal += '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>';
                    modal += '<h4 class="modal-title" id="myModalLabel">Validation Successfull!</h4>';
                    modal += '</div>';

                    modal += '<div class="modal-body"><br />';
                    modal += '<div class="row">';
                    modal += '<div class="col-md-2">';
                    modal += '<img style="width:100%; height:auto;" src="data:image/jpeg;base64,' + newUserArr.Photo + '" width="52" alt="' + newUserArr.LastName + ', ' + newUserArr.GivenName + '"/>';
                    modal += '</div>';
                    modal += '<div class="col-md-10">';
                    modal += 'Complete Name: <code>' + newUserArr.LastName + ', ' + newUserArr.GivenName + '</code><br />';
                    modal += 'Employee #: <code>' + newUserArr.EmployeeNbr + '</code><br />';
                    modal += 'Department: <code>' + newUserArr.Department + '</code><br />';
                    modal += 'Email: <code>' + newUserArr.Email + '</code>';
                    modal += '</div>';
                    modal += '</div>';
                    modal += '<hr />';
                   // modal += '<br />';
                    modal += '<h4>Are you sure you want to add this user?<h4/>';
                    modal += '</div>';

                    modal += '<div class="modal-footer">';
                    modal += '<a type="button" class="btn btn-medium btn-gray" data-dismiss="modal">NO</a>';
                    modal += '<a type="button" class="btn btn-medium btn-green" data-dismiss="modal" id="cmdAddUser">YES</a>';
                    modal += '</div>';

                    modal += '</div>';
                    modal += '</div>';
                    modal += '</div>';

                    $("#notification_modal").html(modal);
                    $("#modal_div").modal("show");
                    $("#modal_div").css('z-index', '1000000');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },
    addUser: function (uname) {
        $.ajax({
            url: '/Account/Validate',
            dataType: 'json',
            type: 'get',
            data: {
                'adduser': "YES",
                'username': uname
            },
            beforeSend: function () {

            },
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {
                if (response.success != true) {
                    notification_modal('An error occurred!',response.message, 'danger');
                } else {
                    notification_modal('Addition Successfull!', 'You have successfully added a new user.', 'success');
                    $('#gridUsers').jqxGrid('updatebounddata');

                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    }
};

function ini_tree(elem_id, jsondata, user_id) {
    var source =
    {
        datatype: "json",
        datafields: [
            { name: 'id' },
            { name: 'parent' },
            { name: 'text' },
            { name: 'checked' },
        ],
        id: 'id',
        localdata: jsondata
    };
    // create data adapter.
    var dataAdapter = new $.jqx.dataAdapter(source);
    // perform Data Binding.
    dataAdapter.dataBind();
    // get the tree items. The first parameter is the item's id. The second parameter is the parent item's id. The 'items' parameter represents
    // the sub items collection name. Each jqxTree item has a 'label' property, but in the JSON data, we have a 'text' field. The last parameter
    // specifies the mapping between the 'text' and 'label' fields.
    console.log(source);
    var records = dataAdapter.getRecordsHierarchy(
        'id',
        'parent',
        'items',
        [
            {
                name: 'text',
                map: 'label',
                checked: 'checked'
            }
        ]
    );
    
    $('#' + elem_id).jqxTree({
        theme: window.gridTheme,
        source: records,
        width: '100%',
        checkboxes: true,
        hasThreeStates: true,
        allowDrag: false,
        allowDrop: false,
    });

    $('#' + elem_id).on('checkChange', function (event) {
        var args = event.args;
        var element = args.element;
        var checked = args.checked;

        var parentElement = event.args.element.parentElement.parentElement;
        var parent = $('#' + elem_id).jqxTree('getItem', parentElement);
        _dbase.updatePermission(user_id, element.id, checked);
    });

}