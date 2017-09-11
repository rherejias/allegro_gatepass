$(document).ready(function () {
    
    if ($('#GPhastransaction').val() == 'GPTransacted') {

        $('#div_hasTransaction').css('display', 'unset');
        $('#div_accessdenied').css('display', 'none');
        $('#accessdeniedIconWarning').css('display', 'none');
        $('#accessdeniedIconInfo').css('display', 'unset');
        $('#accessdenied_bgcolor').css('background-color', '#a1bbfc')


    }
   
});