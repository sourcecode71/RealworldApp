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
                window.location.href = "/Home/Login";
            }
        },
        error: function (err) {
            window.location.href = "/Home/Login";
        }
    });
}