$(document).ready(function () {

    $.ajax({
        url: "/home/foo",
        dataType: 'json',
        type: 'get',
        data: {},
        beforeSend: function () {

        },
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        },
        success: function (response) {
            if (response.success) {
                $("#testChartContainer").html(response.message);
                window['init_hchart_chartid']();
            } else {
                console.log("err");
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status);
            console.log(thrownError);
        }
    });




    if ($('#userlogin').val() == "Security") {
        $('#dashguard').show();

    } else if ($('#userlogin').val() == "User") {
        $('#dashuser').show();
    } else {
        $('#dashapprover').show();
    }



    //progress bar for user
    $("#jqxProgressBarSubmitted").css('width', $('#totalsubmitted').val() + '%');
    $("#approved").css('width', $('#totalapproved').val() + '%');
    $("#rejected").css('width', $('#totalrejected').val() + '%');
    $("#drafted").css('width', $('#totaldrafted').val() + '%');
    // end

    // progress bar for approver
    $("#approverpending").css('width', $('#percentpending').val() + '%');
    $("#approverapproved").css('width', $('#percentapproved').val() + '%');
    $("#approverrejected").css('width', $('#percentrejected').val() + '%');
    $("#approveroverall").css('width', '100%');
    // end

    // progress for guard
    $("#guardjqxProgressBarSubmitted").css('width', '100%');
    $("#guardapproved").css('width', '100%');
    $("#guardrejected").css('width', '100%');
    $("#guarddrafted").css('width', '100%');
    //end
});