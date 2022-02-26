var Site = {

}

Site.login = function () {

    var email = $("#loginEmail").val();
    var password = $("#loginPassword").val();

    var loginUser = {
        Email: email,
        Password: password
    };

    $.ajax({
        url: '/Home/Login',
        type: 'POST',
        data: loginUser,
        success: function (response) {
            if (response.isSuccess) {
                if (response.result.role == "Admin") {
                    window.location.href = "/Admin/Index";
                }
                else {
                    window.location.href = "/Employee/Index";
                }
            }
            else {
                $("#loginValidationMessage").css("display", "block");
            }
        },
        error: function (err) {
            $("#loginValidationMessage").css("display", "block");
        }
    });
}


function formatAmountNoDecimals(number) {
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(number)) {
        number = number.replace(rgx, '$1' + ',' + '$2');
    }
    return number;
}

function formatAmount(number) {

    // remove all the characters except the numeric values
    number = number.replace(/[^0-9]/g, '');

    // set the default value
    if (number.length == 0) number = "0.00";
    else if (number.length == 1) number = "0.0" + number;
    else if (number.length == 2) number = "0." + number;
    else number = number.substring(0, number.length - 2) + '.' + number.substring(number.length - 2, number.length);

    // set the precision
    number = new Number(number);
    number = number.toFixed(2);    // only works with the "."

    // change the splitter to ","
    number = number.replace(/\./g, '.');

    // format the amount
    x = number.split(',');
    x1 = x[0];
    x2 = x.length > 1 ? ',' + x[1] : '';

    return formatAmountNoDecimals(x1) + x2;
}


$(function () {

    $('#projectPaid').keyup(function () {
        $(this).val("$ " + formatAmount($(this).val()));
    });

    $('#addProjectBudget').keyup(function () {
        $(this).val("$ " + formatAmount($(this).val()));
    }); 

    $('#approvedBudget').keyup(function () {
        $(this).val("$ " + formatAmount($(this).val()));
    });

    $('.moneyFormat').keyup(function () {
        $(this).val("$ " + formatAmount($(this).val()));
    });
});