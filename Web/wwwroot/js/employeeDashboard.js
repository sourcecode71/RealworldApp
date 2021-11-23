var EmployeeDashboard = {

}

EmployeeDashboard.addActivity = function (admin) {

    EmployeeDashboard.loading();

    var employeeEmail = $("#currentEmail").val();
    var id = $("#projectId").val();
    var date = $("#addActivityDate").val();
    var hours = $("#addActivityHours").val();
    var comment = $("#addActivityComment").val();
    var status = $("#addActivityStatus").val();
    var statusComment = $("#addActivityStatusComment").val();
    var isAdmin = admin == 1;

    var activity = {
        SelfProjectId: id,
        DateTime: date,
        Duration: hours,
        Comment: comment,
        Status: status,
        EmployeeEmail: employeeEmail,
        StatusComment: statusComment,
        IsAdmin: isAdmin
    };

    $.ajax({
        url: '/Project/AddActivity',
        type: 'POST',
        data: activity,
        success: function (result) {
            EmployeeDashboard.removeLoader();

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
            EmployeeDashboard.removeLoader();

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

EmployeeDashboard.loading = function () {

    $('body').append('<div style="" id="loadingDiv"><div class="loader">Loading...</div></div>');
}

EmployeeDashboard.removeLoader = function () {
    $("#loadingDiv").fadeOut(500, function () {
        $("#loadingDiv").remove();
    });
}

$(document).ready(function () {
    var today = new Date();

    $("#addActivityDate").val(today.getFullYear() + '-' + ('0' + (today.getMonth() + 1)).slice(-2) + '-' + ('0' + today.getDate()).slice(-2));
});