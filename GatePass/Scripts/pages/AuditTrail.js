//for search box function
$(document).delegate("#AuditTrailGrid_searchField", "keyup", function (e) {
    var columns = ["module_string","user_id_string", "name_string",
                   "operation_string", "ip_address_string", "mac_address_string", "date_added_datetime"];
    generalSearch($('#AuditTrailGrid_searchField').val(), "AuditTrailGrid", columns, e);
});