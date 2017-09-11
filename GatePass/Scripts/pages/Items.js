var dataItem, dataCategory, dataType, dataMeasure, dataDepartment;
var dataItemInactive, dataCategoryInactive, dataTypeInactive, dataMeasureInactive, dataDepartmentInactive;

$('#btnSupplier').click(function () {
    showforms.item('add');
});

$('#btnCategory').click(function () {
    showforms.add('Category','Category');
});

$('#btnType').click(function () {
    showforms.add('Item Type', 'Type');
});

$('#btnMeasure').click(function () {
    showforms.add('Unit of Measure', 'Measure');
});

$('#btnDept').click(function () {
    showforms.add('Item Department', 'Department');
});

//@ver 1.0 @author rherejias export to excel button onclick event for active grid
$(document).delegate("#ItemGrid_Export_to_Excel", "click", function () {
    $("#ItemGrid").jqxGrid('exportdata', 'xls', 'Item_Details');
    $.ajax({
        url: '/Items/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Item',
            table: 'tblItems'
        }
    });
});

$(document).delegate("#CategoryGrid_Export_to_Excel", "click", function () {
    $("#CategoryGrid").jqxGrid('exportdata', 'xls', 'ItemCategory_Details');
    $.ajax({
        url: '/Items/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Category',
            table: 'tblCategories'
        }
    });
});

$(document).delegate("#TypeGrid_Export_to_Excel", "click", function () {
    $("#TypeGrid").jqxGrid('exportdata', 'xls', 'ItemType_Details');
    $.ajax({
        url: '/Items/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_ItemType',
            table: 'tblItemTypes'
        }
    });
});

$(document).delegate("#MeasureGrid_Export_to_Excel", "click", function () {
    $("#MeasureGrid").jqxGrid('exportdata', 'xls', 'ItemMeasure_Details');
    $.ajax({
        url: '/Items/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Measure',
            table: 'tblUnitOfMeasure'
        }
    });
});

$(document).delegate("#DepartmentGrid_Export_to_Excel", "click", function () {
    $("#DepartmentGrid").jqxGrid('exportdata', 'xls', 'ItemDepartment_Details');
    $.ajax({
        url: '/Items/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Department',
            table: 'tblItemDepartmentRelationship'
        }
    });
});


//@ver 1.0 @author rherejias export to excel button onclick event for inactive grid
$(document).delegate("#ItemGridInactive_Export_to_Excel", "click", function () {
    $("#ItemGridInactive").jqxGrid('exportdata', 'xls', 'Item_DetailsInactive');
    $.ajax({
        url: '/Items/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Item_Inactive',
            table: 'tblItems'
        }
    });
});

$(document).delegate("#CategoryGridInactive_Export_to_Excel", "click", function () {
    $("#CategoryGridInactive").jqxGrid('exportdata', 'xls', 'ItemCategory_DetailsInactive');
    $.ajax({
        url: '/Items/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Category_Inactive',
            table: 'tblCategories'
        }
    });
});

$(document).delegate("#TypeGridInactive_Export_to_Excel", "click", function () {
    $("#TypeGridInactive").jqxGrid('exportdata', 'xls', 'ItemType_DetailsInactive');
    $.ajax({
        url: '/Items/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_ItemType_Inactive',
            table: 'tblItemTypes'
        }
    });
});

$(document).delegate("#MeasureGridInactive_Export_to_Excel", "click", function () {
    $("#MeasureGridInactive").jqxGrid('exportdata', 'xls', 'ItemMeasure_DetailsInactive');
    $.ajax({
        url: '/Items/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Measure_Inactive',
            table: 'tblUnitOfMeasure'
        }
    });
});

$(document).delegate("#DepartmentGridInactive_Export_to_Excel", "click", function () {
    $("#DepartmentGridInactive").jqxGrid('exportdata', 'xls', 'ItemDepartment_DetailsInactive');
    $.ajax({
        url: '/Items/export',
        type: 'get',
        dataType: 'json',
        data: {
            operation: 'Export_Department_Inactive',
            table: 'tblItemDepartmentRelationship'
        }
    });
});

$(document).delegate('#btnProceedDeactivate', 'click', function () {
    dbase_operation.deactivate($(this).attr('data-source') ,$(this).attr('data-id'), $(this).attr('itemID'));
});

$(document).delegate('#btnProceedActivate', 'click', function () {
    dbase_operation.activate($(this).attr('data-source'), $(this).attr('data-id'), $(this).attr('itemID'));
});

$(document).delegate('#btnSaveItem', 'click', function () {
    dbase_operation.addItem('add');
});

$(document).delegate('#btnAdd', 'click', function () {
    dbase_operation.add($(this).attr('data-source'));
});

$(document).delegate('#btnEdit', 'click', function () {
    dbase_operation.edit($(this).attr('data-source'),$(this).attr('data-id'),$(this).attr('data-code'));
});

//ver 1.0 @author rherejias onclick event of edit button on item grids
$(document).delegate(".ItemGrid_Edit", "click", function (event) {
    showforms.item('edit',dataItem);
});

$(document).delegate(".CategoryGrid_Edit", "click", function (event) {
    showforms.edit('Category', 'Category', dataCategory);
});

$(document).delegate(".TypeGrid_Edit", "click", function (event) {
    showforms.edit('Item Type', 'Type', dataType);
});

$(document).delegate(".MeasureGrid_Edit", "click", function (event) {
    showforms.edit('Unit of Measure', 'Measure', dataMeasure);
});

$(document).delegate(".DepartmentGrid_Edit", "click", function (event) {
    showforms.edit('Item Department', 'Department', dataDepartment);
});

//@ver 1.0 @author rherejias onclick event of proceed edit 
$(document).delegate('#btnEditItem', 'click', function () {
    dbase_operation.addItem('edit');
});

//@ver 1.0 @author rherejias onclick event of inactive button on item grids
$(document).delegate(".ItemGrid_Inactive", "click", function (event) {
    showforms.deactivate('edit', dataItem.code_string, 'ItemGrid', dataItem.id_number, "Item");
});

$(document).delegate(".CategoryGrid_Inactive", "click", function (event) {
    showforms.deactivate('edit', dataCategory.code_string, 'CategoryGrid', dataCategory.id_number, "Item Category");
});

$(document).delegate(".TypeGrid_Inactive", "click", function (event) {
    showforms.deactivate('edit', dataType.code_string, 'TypeGrid', dataType.id_number, "Item Type");
});

$(document).delegate(".MeasureGrid_Inactive", "click", function (event) {
    showforms.deactivate('edit', dataMeasure.code_string, 'MeasureGrid', dataMeasure.id_number, "Item Measure");
});

$(document).delegate(".DepartmentGrid_Inactive", "click", function (event) {
    showforms.deactivate('edit', dataDepartment.code_string, 'DepartmentGrid', dataDepartment.id_number, "Item Department");
});

//@ver 1.0 @author rherejias onclick event of activate button on item grids
$(document).delegate(".ItemGridInactive_Activate", "click", function (event) {
    showforms.activate('activate', dataItemInactive.code_string, 'ItemGridInactive', dataItemInactive.id_number, "Item");
});

$(document).delegate(".CategoryGridInactive_Activate", "click", function (event) {
    showforms.activate('edit', dataCategoryInactive.code_string, 'CategoryGridInactive', dataCategoryInactive.id_number, "Item Category");
});

$(document).delegate(".TypeGridInactive_Activate", "click", function (event) {
    showforms.activate('edit', dataTypeInactive.code_string, 'TypeGridInactive', dataTypeInactive.id_number, "Item Type");
});

$(document).delegate(".MeasureGridInactive_Activate", "click", function (event) {
    showforms.activate('edit', dataMeasureInactive.code_string, 'MeasureGridInactive', dataMeasureInactive.id_number, "Item Measure");
});

$(document).delegate(".DepartmentGridInactive_Activate", "click", function (event) {
    showforms.activate('edit', dataDepartmentInactive.code_string, 'DepartmentGridInactive', dataDepartmentInactive.id_number, "Item Department");
});

//@ver 1.0 @author rherejias row click of active item grids
$("#ItemGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#ItemGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#ItemGrid").jqxGrid('getrowdatabyid', rowID);
    dataItem = data;
    //showforms.deactivate('edit', data.code_string, 'ItemGrid');
});

$("#CategoryGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#CategoryGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#CategoryGrid").jqxGrid('getrowdatabyid', rowID);
    dataCategory = data;
    //showforms.deactivate('edit', data.code_string, 'CategoryGrid');
});

$("#TypeGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#TypeGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#TypeGrid").jqxGrid('getrowdatabyid', rowID);
    dataType = data;
    //showforms.deactivate('edit', data.code_string, 'TypeGrid');
});

$("#MeasureGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#MeasureGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#MeasureGrid").jqxGrid('getrowdatabyid', rowID);
    dataMeasure = data;
    //showforms.deactivate('edit', data.code_string, 'MeasureGrid');
});

$("#DepartmentGrid").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#DepartmentGrid").jqxGrid('getrowid', boundIndex);
    var data = $("#DepartmentGrid").jqxGrid('getrowdatabyid', rowID);
    dataDepartment = data
    //showforms.deactivate('edit', data.code_string, 'DepartmentGrid');
});


//@ver 1.0 @author rherejias row click of inactive item grids
$("#ItemGridInactive").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#ItemGridInactive").jqxGrid('getrowid', boundIndex);
    var data = $("#ItemGridInactive").jqxGrid('getrowdatabyid', rowID);
    dataItemInactive = data;
    //showforms.deactivate('edit', data.code_string, 'ItemGrid');
});

$("#CategoryGridInactive").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#CategoryGridInactive").jqxGrid('getrowid', boundIndex);
    var data = $("#CategoryGridInactive").jqxGrid('getrowdatabyid', rowID);
    dataCategoryInactive = data;
    //showforms.deactivate('edit', data.code_string, 'CategoryGrid');
});

$("#TypeGridInactive").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#TypeGridInactive").jqxGrid('getrowid', boundIndex);
    var data = $("#TypeGridInactive").jqxGrid('getrowdatabyid', rowID);
    dataTypeInactive = data;

});

$("#MeasureGridInactive").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#MeasureGridInactive").jqxGrid('getrowid', boundIndex);
    var data = $("#MeasureGridInactive").jqxGrid('getrowdatabyid', rowID);
    dataMeasureInactive = data;
    //showforms.deactivate('edit', data.code_string, 'MeasureGrid');
});

$("#DepartmentGridInactive").on('rowclick', function (event) {
    var args = event.args;
    // row's bound index.
    var boundIndex = args.rowindex;
    // row's visible index.
    var visibleIndex = args.visibleindex;
    // right click.
    var rightclick = args.rightclick;
    // original event.
    var ev = args.originalEvent;

    var rowID = $("#DepartmentGridInactive").jqxGrid('getrowid', boundIndex);
    var data = $("#DepartmentGridInactive").jqxGrid('getrowdatabyid', rowID);
    dataDepartmentInactive = data
    //showforms.deactivate('edit', data.code_string, 'DepartmentGrid');
});

//@ver 1.0 @authoer rherjeias 2/1/2017 active grid search textbox
$(document).delegate("#ItemGrid_searchField", "keyup", function (e) {
    var columns = ["supplier_string", "supplier_contact_string", "category_string",
                   "type_string", "related_to_string", "name_string", "description_string"];
    generalSearch($('#ItemGrid_searchField').val(), 'ItemGrid', columns, e);
});

$(document).delegate("#CategoryGrid_searchField", "keyup", function (e) {
    var columns = ["name_string", "description_string"];
    generalSearch($('#CategoryGrid_searchField').val(), 'CategoryGrid', columns, e);
});

$(document).delegate("#TypeGrid_searchField", "keyup", function (e) {
    var columns = ["name_string", "description_string"];
    generalSearch($('#TypeGrid_searchField').val(), 'TypeGrid', columns, e);
});

$(document).delegate("#MeasureGrid_searchField", "keyup", function (e) {
    var columns = ["name_string", "description_string"];
    generalSearch($('#MeasureGrid_searchField').val(), 'MeasureGrid', columns, e);
});

$(document).delegate("#DepartmentGrid_searchField", "keyup", function (e) {
    var columns = ["name_string", "description_string"];
    generalSearch($('#DepartmentGrid_searchField').val(), 'DepartmentGrid', columns, e);
});

//@ver 1.0 @authoer rherjeias 2/1/2017 inactive grid search textbox
$(document).delegate("#ItemGridInactive_searchField", "keyup", function (e) {
    var columns = ["supplier_string", "supplier_contact_string", "category_string",
                   "type_string", "related_to_string", "name_string", "description_string"];
    generalSearch($('#ItemGridInactive_searchField').val(), 'ItemGridInactive', columns, e);
});

$(document).delegate("#CategoryGridInactive_searchField", "keyup", function (e) {
    var columns = ["name_string", "description_string"];
    generalSearch($('#CategoryGridInactive_searchField').val(), 'CategoryGridInactive', columns, e);
});

$(document).delegate("#TypeGridInactive_searchField", "keyup", function (e) {
    var columns = ["name_string", "description_string"];
    generalSearch($('#TypeGridInactive_searchField').val(), 'TypeGridInactive', columns, e);
});

$(document).delegate("#MeasureGridInactive_searchField", "keyup", function (e) {
    var columns = ["name_string", "description_string"];
    generalSearch($('#MeasureGridInactive_searchField').val(), 'MeasureGridInactive', columns, e);
});

$(document).delegate("#DepartmentGridInactive_searchField", "keyup", function (e) {
    var columns = ["name_string", "description_string"];
    generalSearch($('#DepartmentGridInactive_searchField').val(), 'DepartmentGridInactive', columns, e);
});

var showforms = {
    item: function (operation, boundIndex) {
        var name = '';
        var description = '';
        var category = '';
        var type = '';
        var uom = '';
        var dept = '';

        if (operation == 'edit') {
            name = boundIndex.origname_string;
            description = boundIndex.description_string;
            category = boundIndex.category_code_string;
            type = boundIndex.type_code_string;
            uom = boundIndex.uom_code_string;
            dept = boundIndex.dept_code_string;
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
        modal += '      border-right: 0;';
        modal += '      border-bottom: 1px solid #e5e5e5;';
        modal += '}';
        modal += '</style>';

        modal += '<div class="modal fade" id="modalitemmaster" role="dialog" >';
        modal += '<div class="modal-dialog">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal += '<h4 class="modal-title">Add Item</h4>';
        modal += '</div>';
        modal += '<div class="modal-body">';

        modal += '<div class="row">';
        modal += '  <input id="Code" type="hidden" />';
        modal += '  <div class="col-md-12" style="margin-top:3%;">';
        modal += '          <div class="form-group">';
        modal += '              <input id="name" type="text" class="textstyle form-control" placeholder="*Item name"class="form-control companyrequired" style="width:98%;" value="'+ name +'"/>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '              <input id="description" placeholder="*Description"  class="textstyle form-control" type="text" class="form-control companyrequired" style="width:98%;" value="'+ description +'"/>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '              <div class="form-control dropdownlist textstyle" id="cmbCategory" data-placeholder="*Item Category" data-url="/Items/GetItemCategories?IsActive=1" data-display="name_string" data-value="code_string"></div>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '              <div class="form-control dropdownlist textstyle" id="cmbType" data-placeholder="*Item Type" data-url="/Items/GetItemTypes?IsActive=1" data-display="name_string" data-value="code_string"></div>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '              <div class="form-control dropdownlist textstyle" id="cmbUOM" data-placeholder="*Unit of measure" data-url="/Items/GetUnitOfMeasures?IsActive=1" data-display="name_string" data-value="code_string"></div>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';
        modal += '<div class="row">';
        modal += '  <div class="col-md-12">';
        modal += '          <div class="form-group">';
        modal += '             <div class="form-control dropdownlist textstyle" id="cmbDepartment" data-placeholder="*Item Department" data-url="/Items/GetItemDepartment?IsActive=1" data-display="name_string" data-value="code_string"></div>';
        modal += '          </div>';
        modal += '  </div>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-medium btn-blue" id="' + ((operation == 'edit') ? 'btnEditItem' : 'btnSaveItem') + '"';
        modal += 'style="width: 100px;">';
        modal += '' + ((operation == 'edit') ? 'Edit' : 'Add') + '</a>';
        modal += '<a type="button" style="width: 100px;" class="btn btn-medium btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modalitemmaster").modal("show");
        $("#modalitemmaster").css('z-index', '1000000');

        ini_main.element('dropdownlist');
        ini_main.element('inputtext');

        $("#cmbCategory").on('bindingComplete', function (event) {
            if (category != "") {
                $("#cmbCategory").jqxDropDownList('val', category);
            }
        });

        $("#cmbType").on('bindingComplete', function (event) {
            if (type != "") {
                $("#cmbType").jqxDropDownList('val', type);
            }
        });

        $("#cmbUOM").on('bindingComplete', function (event) {
            if (uom != "") {
                $("#cmbUOM").jqxDropDownList('val', uom);
            }
        });

        $("#cmbDepartment").on('bindingComplete', function (event) {
            if (dept != "") {
                $("#cmbDepartment").jqxDropDownList('val', dept);
            }
        });
    },

    deactivate: function (operation, code, source, id, title) {
        var modal = '<div class="modal fade" id="modalDeactivate" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#F25656; color:#ffffff">';
        modal += '<h4 class="modal-title">Inactive ' + title + '</h4>';
        modal += '</div>';
       // modal += '<br/>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to deactivate this record?</p>';
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

    activate: function (operation, code, source, id, title) {
        var modal = '<div class="modal fade" id="modalActivate" role="dialog" >';
        modal += '<div class="modal-dialog modal-sm">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal += '<h4 class="modal-title">Activate ' + title + '</h4>';
        modal += '</div>';
        // modal += '<br/>';

        modal += '<div class="modal-body" style="margin-top:4%">';
        modal += '<p>Are you sure you want to reactivate this '+ title +'?</p>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-small btn-blue" id="btnProceedActivate"';
        modal += 'data-source="' + source + '" data-id="' + code + '" itemID="' + id + '">';
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

    add: function (title, source) {

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
        modal += '      border-right: 0;';
        modal += '      border-bottom: 1px solid #e5e5e5;';
        modal += '}';
        modal += '</style>';

        modal += '<div class="modal fade" id="modaladd" role="dialog" >';
        modal += '<div class="modal-dialog">';
        modal += ' <div class="modal-content">';

        modal += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
        modal += '<h4 class="modal-title">Add ' + title + '</h4>';
        modal += '</div>';
        modal += '<div class="modal-body" style="margin-top:3%;">';
        //modal += '   <label style="margin-top:3%;">Name</label>'
        modal += '   <input id="name" type="text" placeholder="*Name" class="form-control companyrequired textstyle" />';
        //modal += '   <label>Description</label>'
        modal += '   <input id="description" type="text" placeholder="*Description" class="form-control companyrequired textstyle" style="margin-top:3%;"/>';
        modal += '</div>';

        modal += '<div class="modal-footer">';
        modal += '<div class="row">';
        modal += '<a class="btn btn-medium btn-blue" id="btnAdd" data-source="' + source + '"';
        modal += 'style="width: 100px;">';
        modal += 'Add</a>';
        modal += '<a type="button" style="width: 100px;" class="btn btn-medium btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
        modal += '</div>';
        modal += '</div>';

        modal += '</div>';
        modal += '</div>';
        modal += '</div>';

        $("#form_modal").html(modal);
        $("#modaladd").modal("show");
        $("#modaladd").css('z-index', '1000000');
    },

    edit: function (title, source, boundIndex) {
    var name = boundIndex.name_string;
    var description = boundIndex.description_string;

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
    modal += '      border-right: 0;';
    modal += '      border-bottom: 1px solid #e5e5e5;';
    modal += '}';
    modal += '</style>';

    modal += '<div class="modal fade" id="modaladd" role="dialog" >';
    modal += '<div class="modal-dialog">';
    modal += ' <div class="modal-content">';

    modal += '<div class="modal-header" style="background-color:#76cad4; color:#ffffff">';
    modal += '<h4 class="modal-title">Edit ' + title + '</h4>';
    modal += '</div>';
    modal += '<div class="modal-body" style="margin-top:3%;">';
    modal += '   <input id="name" type="text" placeholder="*Name" class="form-control companyrequired textstyle" value="'+ name +'"/>';
    modal += '   <input id="description" type="text" placeholder="*Description" class="form-control companyrequired textstyle" style="margin-top:3%;" value="'+ description +'"/>';
    modal += '</div>';

    modal += '<div class="modal-footer">';
    modal += '<div class="row">';
    modal += '<a class="btn btn-medium btn-blue" id="btnEdit" data-code="' + boundIndex.code_string + '" data-id="' + boundIndex.id_number + '" data-source="' + source + '"';
    modal += 'style="width: 100px;">';
    modal += 'Edit</a>';
    modal += '<a type="button" style="width: 100px;" class="btn btn-medium btn-gray" data-dismiss="modal">Cancel</a> &nbsp ';
    modal += '</div>';
    modal += '</div>';

    modal += '</div>';
    modal += '</div>';
    modal += '</div>';

    $("#form_modal").html(modal);
    $("#modaladd").modal("show");
    $("#modaladd").css('z-index', '1000000');
}
};

var dbase_operation = {
    activate: function (grid, code, id) {
        var url = '';
        var msg = '';
        if (grid == "ItemGridInactive") {
            url = '/Items/ActivateItem'
            msg = ' Item activated successfully.';
        } else if (grid == "CategoryGridInactive") {
            url = '/Items/ActivateCategory'
            msg = ' Category activated successfully.';
        } else if (grid == "TypeGridInactive") {
            url = '/Items/ActivateType'
            msg = ' Item type activated successfully.';
        } else if (grid == "MeasureGridInactive") {
            url = '/Items/ActivateMeasure'
            msg = ' Item measure activated successfully.';
        } else if (grid == "DepartmentGridInactive") {
            url = '/Items/ActivateDepartment'
            msg = ' Item department activated successfully.';
        }
        $.ajax({
            url: url,
            dataType: 'json',
            type: 'get',
            data: {
                Id: id,
                code: code
            },
            beforeSend: function () {

            },
            success: function (response) {
                $("#modalActivate").modal("hide");
                if (response.success) {
                    toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + msg;
                    Save_ToastNotifMaintenance();
                    if (grid == "ItemGridInactive") {
                        $('#ItemGrid').jqxGrid('updatebounddata');
                        $('#ItemGridInactive').jqxGrid('updatebounddata');
                    }
                    else if (grid == "CategoryGridInactive") {
                        $('#CategoryGrid').jqxGrid('updatebounddata');
                        $('#CategoryGridInactive').jqxGrid('updatebounddata');
                    }
                    else if (grid == "TypeGridInactive") {
                        $('#TypeGrid').jqxGrid('updatebounddata');
                        $('#TypeGridInactive').jqxGrid('updatebounddata');
                    }
                    else if (grid == "MeasureGridInactive") {
                        $('#MeasureGrid').jqxGrid('updatebounddata');
                        $('#MeasureGridInactive').jqxGrid('updatebounddata');
                    }
                    else if (grid == "DepartmentGridInactive") {
                        $('#DepartmentGrid').jqxGrid('updatebounddata');
                        $('#DepartmentGridInactive').jqxGrid('updatebounddata');
                    }
                } else {
                    notification_modal("Activation Failed!", response.message, "danger");
                }
            },

            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    },

    deactivate: function (grid, code, id) {
        var url = '';
        var msg = '';
        if (grid == "ItemGrid") {
            url = '/Items/DeactivateItem'
            msg = ' Item deactivated successfully.';
        } else if (grid == "CategoryGrid") {
            url = '/Items/DeactivateCategory'
            msg = ' Category deactivated successfully.';
        } else if (grid == "TypeGrid") {
            url = '/Items/DeactivateType'
            msg = ' Item type deactivated successfully.';
        } else if (grid == "MeasureGrid") {
            url = '/Items/DeactivateMeasure'
            msg = ' Item measure deactivated successfully.';
        } else if (grid == "DepartmentGrid") {
            url = '/Items/DeactivateDepartment'
            msg = ' Item department deactivated successfully.';
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
                    toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + msg;
                    Save_ToastNotifMaintenance();
                    if (grid == "ItemGrid") {
                        $('#ItemGrid').jqxGrid('updatebounddata');
                        $('#ItemGridInactive').jqxGrid('updatebounddata');
                    }
                    else if (grid == "CategoryGrid") {
                        $('#CategoryGrid').jqxGrid('updatebounddata');
                        $('#CategoryGridInactive').jqxGrid('updatebounddata');
                    }
                    else if (grid == "TypeGrid") {
                        $('#TypeGrid').jqxGrid('updatebounddata');
                        $('#TypeGridInactive').jqxGrid('updatebounddata');
                    }
                    else if (grid == "MeasureGrid") {
                        $('#MeasureGrid').jqxGrid('updatebounddata');
                        $('#MeasureGridInactive').jqxGrid('updatebounddata');
                    }
                    else if (grid == "DepartmentGrid") {
                        $('#DepartmentGrid').jqxGrid('updatebounddata');
                        $('#DepartmentGridInactive').jqxGrid('updatebounddata');
                    }
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

    addItem: function (operation) {
        var url = '';
        var code = '';
        var id = '';
        var msg = ' Item updated successfully.';
        if (operation == 'edit') {
            url = '/Items/EditItems';
            code = dataItem.code_string;
            id = dataItem.id_number;
        }
        else {
            url = '/Items/AddItems';
            msg = ' Item created successfully';
        }
        var elements = ["#name", "#description", "#cmbCategory", "#cmbType", "#cmbDepartment", "#cmbUOM"]
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
        
        if (ctr == 6) {
            $.ajax({
                url: url,
                dataType: 'json',
                type: 'get',
                data: {
                    code: code,
                    id: id,
                    name: $("#name").val(),
                    description: $("#description").val(),
                    supplier: $("#cmbSupplier").val(),
                    category: $("#cmbCategory").val(),
                    type: $("#cmbType").val(),
                    department: $("#cmbDepartment").val(),
                    uom: $("#cmbUOM").val()
                },
                beforeSend: function () {

                },
                headers: {
                    //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                },
                success: function (response) {
                    $("#modalitemmaster").modal("hide");
                    if (response.success) {
                        toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + msg;
                        Save_ToastNotifMaintenance();
                        $('#ItemGrid').jqxGrid('updatebounddata');
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
        else {
            console.log("fill up all fields");
        }
    },

    add: function (source) {

        var elements = ["#name", "#description"]
        var ctr = 0;
        for (var i = 0; i <= 1; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "red");
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5");
                ctr++;
            }
        }
        if (ctr == 2) {
            var url = '';
            var msg = '';
            if (source == "Category") {
                url = '/Items/AddCategory'
                msg = ' New category created successfully.'
            } else if (source == "Type") {
                url = '/Items/AddType'
                msg = ' New item type created successfully.'
            } else if (source == "Measure") {
                url = '/Items/AddMeasure'
                msg = ' New item measure created successfully.'
            } else if (source == "Department") {
                url = '/Items/AddDepartment'
                msg = ' New item department created successfully.'
            }
            $.ajax({
                url: url,
                dataType: 'json',
                type: 'get',
                data: {
                    name: $("#name").val(),
                    description: $("#description").val()
                },
                beforeSend: function () {

                },
                headers: {
                    //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                },
                success: function (response) {
                    $("#modaladd").modal("hide");
                    if (response.success) {
                        toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + msg;
                        Save_ToastNotifMaintenance();
                        if (source == "Category")
                            $('#CategoryGrid').jqxGrid('updatebounddata');
                        else if (source == "Type")
                            $('#TypeGrid').jqxGrid('updatebounddata');
                        else if (source == "Measure")
                            $('#MeasureGrid').jqxGrid('updatebounddata');
                        else if (source == "Department")
                            $('#DepartmentGrid').jqxGrid('updatebounddata');
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

    edit: function (source, id, code) {
        var elements = ["#name", "#description"]
        var ctr = 0;
        for (var i = 0; i <= 1; i++) {
            if ($(elements[i]).val() == "") {
                $(elements[i]).css("border-color", "red");
            }
            else {
                $(elements[i]).css("border-color", "#e5e5e5");
                ctr++;
            }
        }
        if (ctr == 2) {
            var url = '';
            var msg = '';
            if (source == "Category") {
                url = '/Items/EditCategory'
                msg = ' Category updated successfully.'
            } else if (source == "Type") {
                url = '/Items/EditType'
                msg = ' Item type updated successfully.'
            } else if (source == "Measure") {
                url = '/Items/EditMeasure'
                msg = ' Item measure update successfully.'
            } else if (source == "Department") {
                url = '/Items/EditDepartment'
                msg = ' Item department updated successfully.'
            }
            $.ajax({
                url: url,
                dataType: 'json',
                type: 'get',
                data: {
                    id: id,
                    code: code,
                    name: $("#name").val(),
                    description: $("#description").val()
                },
                beforeSend: function () {

                },
                headers: {
                    //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                },
                success: function (response) {
                    $("#modaladd").modal("hide");
                    if (response.success) {
                        toastNotif = document.getElementById("toastMaintenance").innerHTML = '<i class="fa fa-check fa-lg"></i>' + msg;
                        Save_ToastNotifMaintenance();
                        if (source == "Category")
                            $('#CategoryGrid').jqxGrid('updatebounddata');
                        else if (source == "Type")
                            $('#TypeGrid').jqxGrid('updatebounddata');
                        else if (source == "Measure")
                            $('#MeasureGrid').jqxGrid('updatebounddata');
                        else if (source == "Department")
                            $('#DepartmentGrid').jqxGrid('updatebounddata');
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