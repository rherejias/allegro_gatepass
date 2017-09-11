/**
 * Created by aabasolo on 7/14/2016.
 */
function initialize_jqxwidget_grid(elem) {
    
    if (typeof elem.attr('data-url') != 'undefined') {

        var _params = {};
        var _colFields = [];
        var _dataFields = [];
        var _records = [];
        var _totalRows = 0;

        if (typeof elem.attr('data-dateRange') != 'undefined') {
            var range = $("#" + elem.attr('data-dateRange')).jqxDateTimeInput('getRange');
            _params = {
                dateFrom: range.from.toString(),
                dateTo: range.to.toString(),
            };
        }
        
        if (typeof elem.attr('data-datecustomformat') != 'undefined') {
            _params['date'] = elem.attr('data-datecustomformat');
        }

        if (elem.hasClass("jqxgrid-haschart")) {
            _params['chartId'] = elem.attr('data-chartid');
        }

        if (elem.hasClass("rep-summary")) {
            //alert('summary');
            _params['rep'] = 'summary';
        }

        if (elem.hasClass("rep-details")) {
            //alert('details');
            _params['rep'] = 'details';
            _params['rep_det'] = elem.attr('data-repdetail');
        }

        var _additional_param_elems = [];
        var str_additional_params = '';
        if (typeof elem.attr('data-additionalparamelems') != 'undefined') {
            
            var ape = elem.attr('data-additionalparamelems');
            _additional_param_elems = ape.split(',');
            
            for (var i in _additional_param_elems) {
                str_additional_params += '&' + $("#" + _additional_param_elems[i]).attr('data-fieldname') + '=' + $("#" + _additional_param_elems[i]).val();
                _params[$("#" + _additional_param_elems[i]).attr('data-fieldname')] = $("#" + _additional_param_elems[i]).val();
                
            }
        }

        _params["cols_only"] = true;

        var _method = 'get';
        if(typeof elem.attr('data-method') != 'undefined'){
            _method = elem.attr('data-method');
        }
        
        $.ajax({
            //url: elem.attr('data-url')+(typeof elem.attr('data-additionalparams')=='undefined' ? "" : elem.attr('data-additionalparams')),
            //url: elem.attr('data-url')+str_additional_params,
            url: elem.attr('data-url'),
            dataType: 'json',
            type: _method,
            data: _params,
            beforeSend: function () {
                //                elem.html(' <div style="position:absolute; top:0; left:0; width:100%; height:100%; color:#202020; text-align:center">\n\
                //                            <div style="">\n\
                //                            <br/>\n\
                //                            <img src="../Content/template/images/reload.GIF" style="width:25px;" /><br/>\n\
                //                            '+elem.attr('data-loaderMessage')+' ...\n\
                //                            </div>\n\
                //                        </div>').css('min-height', '100px');
                elem.jqxGrid('showloadelement');

            },
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
            },
            success: function (response) {
                elem.jqxGrid('hideloadelement');
                try {
                    _colFields = response.message.column_config.columnfields;
                    _dataFields = response.message.column_config.datafields;
                    _records = typeof response.message.data == 'undefined' ? '' : response.message.data;
                    _chart = typeof response.message.chart == 'undefined' ? '' : response.message.chart;
                    _totalRows = typeof response.message.TotalRows == 'undefined' ? 0 : response.message.TotalRows;

                    DrawGrid(elem, _colFields, _dataFields, _records, _chart, _totalRows);
                } catch (err) {
                    console.log('error: ' + err + ' / elemid= ' + elem.attr('id'));


                    $('.cmdRefreshGrid[data-grid="gridForTestIC"]').hide();
                    $('.cmdExport[data-grid="gridForTestIC"]').hide();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    }
}

function DrawGrid(elem, colF, dataF, rec, chart, totalrows) {

    console.log(colF);
    
    var elem_id = elem.attr('id');
    var obj = {
        theme: window.gridTheme,
    };

    //This property enables or disables the grouping feature.
    if ($("#" + elem_id).hasClass("jqxgrid-groupable")) {
        obj.groupable = true;
    }

    if ($("#" + elem_id).hasClass("jqxgrid-toolbar")) {
        obj.showtoolbar = true;
        obj.rendertoolbar = function (toolbar) {
            var container = $("<div style='margin: 3px;'></div>");

             input = $("<input class='jqx-input jqx-widget-content jqx-rc-all' id='"+ elem_id +'_'+"searchField' type='text' style='height: 23px; float: left; width: 223px;' placeholder='Search...'/>");
            // var input = $(" <select name='c' id='cmbStatus' class='form-control' data-fieldname='status_string' data-grid='gridAllTrans'><option value='Submitted'>Submitted</option><option value='Approved'>Approved</option><option value='Drafted'>Drafted</option><option value='Rejected'>Rejected</option></select>");
                                    //<option value='Submitted'>Submitted</option> 
                                    //<option value='Approved'>Approved</option>
                                    //<option value='Drafted'>Drafted</option>
                                    //<option value='Rejected'>Rejected</option>
                                
            toolbar.append(container);
            container.append(input);
        };
    }

    //Enables or disables the columns resizing.
    obj.columnsresize = ($("#" + elem_id).hasClass("jqxgrid-columnsresize") ? true : false);

    //Sets or gets the height of the grid to be equal to the summary height of the grid rows. This option should be set when the Grid is in paging mode.
    if ($("#" + elem_id).hasClass("jqxgrid-autoheight")) {
        obj.autoheight = true;
    } else {
        if ($("#" + elem_id).hasClass("jqxgrid-customheight")) {
            obj.height = $("#" + elem_id).attr('grid-height');
        } else {
            obj.height = '100%';
        }
    }

    //Enables or disables the Grid Paging feature. When the value of this property is true, the Grid displays a pager below the rows.
    if ($("#" + elem_id).hasClass("jqxgrid-pageable")) {
        obj.pageable = true;
        rec_limit = 0;
        //Sets or gets the available page size options.
        if ($("#" + elem_id).hasClass("jqxgrid-pagesizeoptions") && $("#" + elem_id).attr("grid-pagesizeoptions") !== undefined) {
            var pagesizeops = $("#" + elem_id).attr('grid-pagesizeoptions');
            var pagesizeops_arr = pagesizeops.split(',');

            //Sets or gets the number of visible rows per page when the Grid paging is enabled.
            obj.pagesize = pagesizeops_arr[0];
            obj.pagesizeoptions = pagesizeops_arr;
        } else {
            obj.pagesizeoptions = ['10', '20', '30', '50', '100'];
        }

    } else {
        if ($("#" + elem_id).attr("grid-record-limit") !== undefined) {
            rec_limit = $("#" + elem_id).attr("grid-record-limit");
        } else {
            rec_limit = 10;
        }
    }

    //selection mode
    if ($("#" + elem_id).attr("grid-selection-mode") !== undefined) {
        obj.selectionmode = $("#" + elem_id).attr("grid-selection-mode");

    }
    else {
        obj.selectionmode = 'none';
    }


    obj.autoshowloadelement = true;

    //Enables or disables the Grid Filtering feature. When the value of this property is true, the Grid displays a filtering panel in the columns popup menus.
    obj.filterable = ($("#" + elem_id).hasClass("jqxgrid-filterable") ? true : false);

    //The sortable property enables or disables the sorting feature.
    obj.sortable = ($("#" + elem_id).hasClass("jqxgrid-sortable") ? true : false);

    //Sets or gets the Grid's width.
    obj.width = ($("#" + elem_id).hasClass("jqxgrid-customwidth") ? $("#" + elem_id).attr('grid-width') : "100%");

    //Sets or gets the height of the grid rows.
    //obj.rowsheight = 60;
    if ($("#" + elem_id).attr("grid-rowsheight") !== undefined) {
        obj.rowsheight = parseInt($("#" + elem_id).attr("grid-rowsheight"));
    }

    //Determines whether ellipsis will be displayed, if the cells or columns content overflows.
    obj.enableellipsis = ($("#" + elem_id).hasClass("jqxgrid-enableellipsis") ? true : false);

    //Enables or disables the alternating rows.
    obj.altrows = ($("#" + elem_id).hasClass("jqxgrid-altrows") ? true : false);

    obj.enablehover = ($("#" + elem_id).hasClass("jqxgrid-enablehover") ? true : false);

    obj.editable = ($("#" + elem_id).hasClass("jqxgrid-editable") ? true : false);

    //Shows or hides the filter row.
    //obj.showfilterrow = true;

    //Determines whether the loading image should be displayed until the Grid's data is loaded.
    obj.autoshowloadelement = true;

    //Shows or hides the aggregates in the grid's statusbar.
    obj.showaggregates = ($("#" + elem_id).hasClass("jqxgrid-showaggregates") ? true : false);

    //Shows or hides the grid's statusbar.
    if ($("#" + elem_id).hasClass("jqxgrid-showstatusbar")) {
        obj.showstatusbar = true;

        //Sets the statusbar's height.
        obj.statusbarheight = 35;

        //for grid button - rherejias 11/18/16 9:04 AM
        var stats_btns = $("#" + elem_id).attr('data-statusbar-buttons');

        if (typeof stats_btns != 'undefined' && stats_btns != '') {
            var stats_btns_array = stats_btns.split(',');
            var container = $("<div style='overflow: hidden; position: relative; margin: 5px;'></div>");
           
            if (stats_btns_array.length > 0) {
                obj.renderstatusbar = function (statusbar) {
                    var addButton = null;
                    for (var i = 0; i < stats_btns_array.length; i++) {
                        if (stats_btns_array[i] == 'Reject') {
                            addButton = $("<button style='margin-left:5px;font-size:12px; font-family: segoe ui;' class='btnx btn btn-danger' id=" + elem_id + '_' + stats_btns_array[i].split(' ').join('_') + ">"
                           + "<i class='fa fa-close' aria-hidden='true'></i>" + " " + stats_btns_array[i] + "</button>");
                        }
                        else {
                            addButton = $("<button style='margin-left:5px;font-size:12px; font-family: segoe ui;' class='btnx btn btn-success' id=" + elem_id + '_' + stats_btns_array[i].split(' ').join('_') + ">"
                           + ((stats_btns_array[i] == 'Clear Filter') ? "<i class='fa fa-eraser' aria-hidden='true'></i>" : "<i class='fa fa-file-excel-o' aria-hidden='true'></i>") + " " + stats_btns_array[i] + "</button>");

                        }
                        container.append(addButton);
                    }
                    statusbar.append(container);
                    $('.btnx').jqxButton({
                        //theme: window.gridTheme,
                        //template: "success"
                    });
                };
            }
        }
    }



    //Enables or disables the columns resizing when the column's border is double-clicked and columnsresize is set to true.
    obj.columnsautoresize = true;

    //Enables or disables the columns reordering.
    obj.columnsreorder = true;

    //Sets or gets the columns menu width.
    obj.columnsmenuwidth = 25;

    //Displays the filter icon only when the column is filtered.
    obj.autoshowfiltericon = true;

    //The property specifies the type of rendering of the Filter Menu.
    //obj.filtermode = 'excel';

    //obj.rowdetailstemplate = { rowdetails: "<div id='grid' class='subgrid' style='margin: 10px;'></div>", rowdetailsheight: 200, rowdetailshidden: true };

    //This property works along with the "autoheight" property. When it is set to true, the height of the Grid rows is dynamically changed depending on the cell values.
    //obj.autorowheight = true;

    //obj.serverProcessing = true;

    //render links
    var linkrenderer = function (row, column, value) {
        if (value.indexOf('#') != -1) {
            value = value.substring(0, value.indexOf('#'));
        }
        var format = { target: '"_blank"' };
        //var html = $.jqx.dataFormat.formatlink(value, format);
        //var html = '<div style="line-height:28px;"><a target="_blank" href="'+value+'">'+value+'</a></div>';
        var html = '<div style="line-height:28px;">' + value + '</div>';
        return html;
    }

    //check if gen search is active
    var genSearch = false;
    if ($("#" + elem_id).hasClass("jqxgrid-gensearch")) {
        var searchStr = $("#" + elem_id).attr('data-gensearch');
        if (typeof searchStr != 'undefined' && searchStr != '') {

            $("#SupplierGrid").appendTo("#mainGridContainer");

            genSearch = true;
        }
    }
    //Enables or disables the virtual data mode.
    obj.virtualmode = ($("#" + elem_id).hasClass("jqxgrid-virtualmode") ? true : false);
    if (obj.virtualmode) {
        var _method = 'get';
        if(typeof elem.attr('data-method') != 'undefined'){
            _method = elem.attr('data-method');
        }

        var source =
        {
            datatype: "json",
            //localdata: rec,
            url: $("#" + elem_id).attr("data-url") + (genSearch ? "?searchStr=" + $("#"+$("#" + elem_id).attr('data-gensearch')).val() : ''),
            datafields: dataF,
            type: _method,
            cache: false,
            root: "data",
            beforeprocessing: function (data) {
                source.totalrecords = data.message.TotalRows;
            },
            filter: function () {
                // update the grid and send a request to the server.
                $("#" + elem_id).jqxGrid('updatebounddata', 'filter');
            },
            sort: function () {
                // update the grid and send a request to the server.
                $("#" + elem_id).jqxGrid('updatebounddata', 'sort');
            }
        };

        var dataAdapter = new $.jqx.dataAdapter(source);

        obj.source = dataAdapter;

        obj.rendergridrows = function (params) {
            return params.data;
        }
    } else {
        var source =
        {
            datatype: "json",
            //localdata: rec,
            url: $("#" + elem_id).attr("data-url") + "?showAll=true" + (genSearch ? "&searchStr=" + $("#" + $("#" + elem_id).attr('data-gensearch')).val() : ''),
            datafields: dataF,
            cache: false,
            root: "data",
        };

        var dataAdapter = new $.jqx.dataAdapter(source);

        obj.source = dataAdapter;
    }

    /*for(var item_prop in column_fields) {
        //if (object.hasOwnProperty(index)) {
        //    var attr = object[index];
        //}
        var attr = column_fields[item_prop]['cellsrenderer'];
        if(attr && attr=='link'){
            column_fields[item_prop]['cellsrenderer'] = linkrenderer;
        }

    }*/

    //hide columns
    var hidden_cols_count = 0;
    var hide_cols = $("#" + elem_id).attr('grid-hide-columns');
    if (typeof hide_cols != 'undefined' && hide_cols != '') {
        var hide_cols_array = hide_cols.split(',');
        hidden_cols_count = hide_cols_array.length;
        for (i = 0; i < hide_cols_array.length; i++) {
            colF[hide_cols_array[i]]['hidden'] = 'true';
        }
    }




    //custom of pagermode
    //var autorow = $("#" + elem_id).attr('grid-autorowheight');
    //if (typeof autorow != 'undefined' && autorow != '') {
    //    obj.autorowheight = pmode;
    //} else {
    //    obj.autorowheight = 'false';
    //}





    //button columns
    var btn_cols = $("#" + elem_id).attr('data-action-buttons');

    var btn_cols_array = {};

    if (typeof btn_cols != 'undefined' && btn_cols != '') {
        btn_cols_array = btn_cols.split(',');
    }

    var actionbtnrenderer = function (row, datafield, value) {
        //get details from other columns
        var dataRecord = $("#" + elem_id).jqxGrid('getrowdata', row);

        //from ren new
        var str_btn_col = '<div style="height:100%; width:100%; text-align:center; margin-top:6px; cursor:pointer;">';

        if (btn_cols_array.length > 0) {
            for (i = 0; i < btn_cols_array.length; i++) {
                
                //if (dataRecord.status_string != 'Drafted' && elem_id == 'gridAllTrans') {   //special case for gridAllTrans

                //    if (btn_cols_array[i] == 'Details') {
                //        str_btn_col += '<span class="label label-primary ' + elem_id + '_' + btn_cols_array[i] + '">&nbsp;<i class="fa fa-info fa-lg" aria-hidden="true"></i>&nbsp;</span> ';
                //    }
                //}
                //else {
                    //modified 11-29-16
                    if (btn_cols_array[i] == 'Details') {
                        str_btn_col += '<span title=' + btn_cols_array[i] + ' class="label label-primary ' + elem_id + '_' + btn_cols_array[i] + '">&nbsp;<i class="fa fa-info fa-lg" aria-hidden="true"></i>&nbsp;</span> ';
                    }
                    else if (btn_cols_array[i] == 'Inactive' || btn_cols_array[i] == 'Remove') {

                        str_btn_col += '<span title=' + btn_cols_array[i] + ' class="label label-danger ' + elem_id + '_' + btn_cols_array[i] + '"><i class="fa fa-minus fa-lg" aria-hidden="true"></i></span> ';
                    }
                    else if (btn_cols_array[i] == 'Activate' || btn_cols_array[i] == 'Remove') {

                        str_btn_col += '<span title=' + btn_cols_array[i] + ' class="label label-success ' + elem_id + '_' + btn_cols_array[i] + '"><i class="fa fa-plus fa-lg" aria-hidden="true"></i></span> ';
                    }
                    else if (btn_cols_array[i] == '<input type=checkbox>') {
                        str_btn_col += '<input type=checkbox id="chkItemReturn" class="' + elem_id + '_' + btn_cols_array[i] + '">';
                    }
                    else if (btn_cols_array[i] == 'Set_as_primary') {
                        str_btn_col += '<span title="Set as primary" class="label label-success ' + elem_id + '_' + btn_cols_array[i] + '"><i class="fa fa-key fa-lg" aria-hidden="true"></i></span> ';
                    }
                    else if (btn_cols_array[i] == 'Assign') {
                        str_btn_col += '<span title="Assign" class="label label-success ' + elem_id + '_' + btn_cols_array[i] + '"><i class="fa fa-key fa-lg" aria-hidden="true"></i></span> ';
                    }
                    else if (btn_cols_array[i] == 'Attach File') {
                        str_btn_col += '<span title="Attach File" class="label label-success ' + elem_id + '_' + btn_cols_array[i] + '"><i class="fa fa-paperclip fa-lg" aria-hidden="true"></i></span> ';
                    }
                    else if (btn_cols_array[i] == 'Override') {
                        str_btn_col += '<span title="Override" class="label label-success ' + elem_id + '_' + btn_cols_array[i] + '"><i class="fa fa-check fa-lg" aria-hidden="true"></i></span> ';
                    }
                    else if (btn_cols_array[i] == 'Reject') {
                        str_btn_col += '<span title="Reject" class="label label-danger ' + elem_id + '_' + btn_cols_array[i] + '"><i class="fa fa-close fa-lg" aria-hidden="true"></i></span> ';
                    }
                    else if (btn_cols_array[i] == 'PrintableCopy') {
                        str_btn_col += '<span title="Print" class="label label-info ' + elem_id + '_' + btn_cols_array[i] + '"><i class="fa fa-print fa-lg" aria-hidden="true"></i></span> ';
                    }
                    else{
                        str_btn_col += '<span title=' + btn_cols_array[i] + ' class="label label-success ' + elem_id + '_' + btn_cols_array[i] + '"><i class="fa fa-pencil fa-lg" aria-hidden="true"></i></span> ';
                    }
                //}
            }
        }
        str_btn_col += '</div>'
        return str_btn_col;
        
    }
    if (typeof btn_cols != 'undefined' && btn_cols != '') {
        var btn_obj = {};
        btn_obj.datafield = "PendingApprovers";
        btn_obj.filterable = false;
        btn_obj.filtertype = "";
        btn_obj.sortable = false;
        if (elem_id == 'grid_item_returnslip') {
            btn_obj.text = "Select";
        }
        else {
            btn_obj.text = "Actions";
        }  
        
        btn_obj.cellsrenderer = actionbtnrenderer;

        colF.push(btn_obj);

        //fix for column width
        var newWidth = 0;
        newWidth = (100 / (colF.length - hidden_cols_count)) + '%';
        for (var i in colF) {
            colF[i].width = newWidth;
        }
        
    }



    ///approval tracking
    var btn_colsapprovaltracking = $("#" + elem_id).attr('data-action-buttonsapproval');
    var btn_colsapprovaltracking_array = {};
    if (typeof btn_colsapprovaltracking != 'undefined' && btn_colsapprovaltracking != '') {
        btn_colsapprovaltracking_array = btn_colsapprovaltracking.split(',');
    }

    var actionbtnrenderer1 = function (row, datafield, value) {

        //get details from other columns
        var dataRecord = $("#" + elem_id).jqxGrid('getrowdata', row);

        //from ren new
        var str_btn_col = '<div style="height:100%; width:100%; text-align:center; margin-top:6px;">';

        if (btn_colsapprovaltracking_array.length > 0) {
            for (i = 0; i < btn_colsapprovaltracking_array.length; i++) {

                if (btn_colsapprovaltracking_array[i] == 'Dept') {
                    str_btn_col += '<span title=' + btn_colsapprovaltracking_array[i] + ' class="label label-primary ' + elem_id + '_' + btn_colsapprovaltracking_array[i] + '">Dept</span> ';
                }               
                else if(btn_colsapprovaltracking_array[i] == 'IT'){
                    str_btn_col += '<span title=' + btn_colsapprovaltracking_array[i] + ' class="label label-primary ' + elem_id + '_' + btn_colsapprovaltracking_array[i] + '">IT</span> ';
                } else if (btn_colsapprovaltracking_array[i] == 'Purch') {
                    str_btn_col += '<span title=' + btn_colsapprovaltracking_array[i] + ' class="label label-primary ' + elem_id + '_' + btn_colsapprovaltracking_array[i] + '">Purch</span> ';
                
                } else if (btn_colsapprovaltracking_array[i] == 'Acc') {
                    str_btn_col += '<span title=' + btn_colsapprovaltracking_array[i] + ' class="label label-primary ' + elem_id + '_' + btn_colsapprovaltracking_array[i] + '">Acc</span> ';
                
                }                             
               
          }
        }
        str_btn_col += '</div>'
        return str_btn_col;

    }
    if (typeof btn_colsapprovaltracking != 'undefined' && btn_colsapprovaltracking != '') {
        var btn_obj = {};

        btn_obj.datafield = "Approval Tracking";
        btn_obj.filterable = false;
        btn_obj.filtertype = "";
        btn_obj.sortable = false;
        btn_obj.text = "Approval Tracking";
        //if (elem_id == 'grid_item_returnslip') {
        //    btn_obj.text = "Select";
        //}
        //else {
        //    btn_obj.text = "Actions";
        //}

        btn_obj.cellsrenderer = actionbtnrenderer1;

        colF.push(btn_obj);
        //fix for column width
        var newWidth = 0;
        newWidth = (100 / (colF.length - hidden_cols_count)) + '%';
        for (var i in colF) {
            colF[i].width = newWidth;
        }

    }

    //end

  

    var cellclass = function (row, columnfield, value, rowdata) {
        if (row == 1) {
            if (value < 100) {
                return 'red';
            } else if (value >= 100) {
                return 'green';
            }
        }
    }

    var cellsrenderer = function (row, columnfield, value, defaulthtml, columnproperties) {
        if (row <= 1) {
            if (isNaN(parseFloat(value).toFixed(2)) == true || typeof value == 'undefined') {
                return '<div class="jqx-grid-cell-right-align" style="margin-top: 6px;"></div>';
            } else {

                return '<div class="jqx-grid-cell-right-align" style="margin-top: 6px;">' + parseFloat(value).toFixed(2) + '%</div>';
            }

        }
    }

    /*
     * image renderer
     */
    var imagerenderer = function (row, datafield, value) {
        if (value != "") {
            return '<img style="cursor:pointer;" height="60" width="100%" class="images" src="' + gridPath + value + '"/>';
        }
        
    }

    /*obj.ready = function(){
     var rowscount = $("#"+elem_id).jqxGrid('getdatainformation').rowscount;
     $("#"+elem_id).jqxGrid({ pagesizeoptions: ['10', '20', rowscount]});
     }*/
    
    if ($("#" + elem_id).hasClass("jqxgrid-custom")) {
        for (var key in colF) {
            switch (colF[key].text) {
                case ' Image':
                    console.log('imagerenderer here');
                    colF[key].cellsrenderer = imagerenderer;
                    break;
                //default:
                //    colF[key].cellclassname = cellclass;
                //    colF[key].cellsrenderer = '';
                //    if(colF[key].text.length<=4){
                //        colF[key].cellclassname = cellclass;
                //    }
            }
        }
    }



    //custom of pagermode
    var pmode = $("#" + elem_id).attr('grid-pagermode');
    if (typeof pmode != 'undefined' && pmode != '') {
        obj.pagermode = pmode;
    } else {
        obj.pagermode = 'default';
    }




    obj.columns = colF;

    $("#" + elem_id).jqxGrid(obj);

    if (elem.hasClass("jqxgrid-haschart") == true) {
        //buildChart(elem.attr('data-chartcontainer'), elem.attr('data-chartid'), elem.attr('data-url'));
        $("#" + elem.attr('data-chartcontainer')).html(chart);
        window['init_hchart_' + elem.attr('data-chartid')]();
    }
}



