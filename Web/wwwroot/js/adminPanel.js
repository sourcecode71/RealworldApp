var AdminPanel = {

}

AdminPanel.addNewEmployee = function () {

    AdminPanel.loading();

    var name = $("#addNewName").val();
    var email = $("#addNewUserEmail").val();
    var password = $("#addNewUserPassword").val();
    var confirmPassword = $("#addNewUserConfirmPassword").val();
    var role = $("#addNewUserRole").val();

    //Add validation

    var employee = {
        Name: name,
        Email: email,
        Password: password,
        Role: role
    };

    $.ajax({
        url: '/Admin/AddNewEmployee',
        type: 'POST',
        data: employee,
        success: function (result) {
            AdminPanel.removeLoader();

            if (result.isSuccess) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Employee added successfully!',
                    showConfirmButton: false,
                    timer: 1500
                })
            }
            else {
                Swal.fire({
                    position: 'top-end',
                    title: 'Error!',
                    text: 'Something went wrong.' + result.errorMessage,
                    icon: 'error',
                    confirmButtonText: 'Ok',
                })
            }
        },
        error: function (err) {
            AdminPanel.removeLoader();

            Swal.fire({
                position: 'top-end',
                title: 'Error!',
                text: 'Something went wrong. Error: ' + result.errorMessage,
                icon: 'error',
                confirmButtonText: 'Ok',
            })
        }
    });
}

AdminPanel.loading = function () {

    $('body').append('<div style="" id="loadingDiv"><div class="loader">Loading...</div></div>');
}

AdminPanel.removeLoader = function() {
    $("#loadingDiv").fadeOut(500, function () {
        $("#loadingDiv").remove();
    });
}