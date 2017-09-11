$(document).ready(function () {
    $("#txtUsername").focus();
    $(".progress").hide();

    $("#txtUsername").keypress(function (e) {
        if (e.which == 13) {
            $("#txtPassword").focus();
        }
    });

    $("#txtPassword").keypress(function (e) {
        if (e.which == 13) {
            $("#cmdLogin").trigger("click");
        }
    });

    $("#cmdLogin").click(function () {
        _action.loginAttempt($("#txtUsername").val(), $("#txtPassword").val(), false);
    });

    var myCookies = getCookies();

    if (typeof myCookies.UName != "undefined" && typeof myCookies.PWord != "undefined") {
        $("#txtUsername").attr('disabled', 'disabled');
        $("#txtPassword").attr('disabled', 'disabled');
        $("#cmdLogin").attr('disabled', 'disabled');
        $("#cmdLogin").text("Logging in using the browser cookie...");
        _action.loginAttempt(myCookies.UName, myCookies.PWord, true);
    }
});

var _action = {
    loginAttempt: function (u, p, a) {
       
        if (!$.trim(u) && !$.trim(p)) {
            notification_modal("Login Failed", "Please provide a username and password!", "danger");
        } else {
            $.ajax({
                url: '/Account/Attempt',
                dataType: 'json',
                type: 'post',
                cache: false,
                data: {
                    username: u,
                    password: p,
                    returnurl: $("#txtReturnURL").val(),
                    isauto: a,
                },
                beforeSend: function () {
                    $("#cmdLogin").hide();
                    $(".progress").show();
                },
                headers: {
                    //'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                },
                success: function (response) {
                    console.log(response);

                    $("#cmdLogin").show();
                    $(".progress").hide();

                    if (response.success) {
 
                        window.location.href = response.message;
                    } else {
                        notification_modal("Login failed!", response.message, "danger");

                        $("#modal_div").on('hidden.bs.modal', function () {
                            $("#txtPassword").val('');
                            $("#txtUsername").focus();
                            $("#txtUsername").select();
                        })
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $("#cmdLogin").show();
                    $(".progress").hide();
                    console.log(xhr.status);
                    console.log(thrownError);
                }
            });
        }
    },
};