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
        url: '/Admin/Login',
        type: 'POST',
        data: loginUser,
        success: function (result) {
            if (result.isSuccess) {
               
            }
            else {
              
            }
        },
        error: function (err) {
           
        }
    });
}