var AdminPanel = {
    href: "http://ec2-3-144-165-25.us-east-2.compute.amazonaws.com/api/Project/get-excel-report?projectStatus="
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

AdminPanel.savePaid = function () {

    AdminPanel.loading();

    var paid = $("#projectPaid").val();
    var selfProjectId = $("#projectId").val();

    //Add validation

    var project = {
        Paid: paid,
        SelfProjectId: selfProjectId
    };

    $.ajax({
        url: '/Admin/SavePaid',
        type: 'POST',
        data: project,
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

AdminPanel.saveModifiedComment = function () {

    AdminPanel.loading();

    var id = $("#projectId").val();
    var modifiedComment = $("#modifiedComment").val();
    var modifiedBudget = $("#modifiedBudget").val();


    var project = {
        SelfProjectId: id,
        AdminModifiedComment: modifiedComment,
        Budget: modifiedBudget
    };

    $.ajax({
        url: '/Admin/SaveModifiedComment',
        type: 'POST',
        data: project,
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

AdminPanel.saveDelayedComment = function () {

    AdminPanel.loading();

    var id = $("#projectId").val();
    var delayedComment = $("#delayedComment").val();

    var project = {
        SelfProjectId: id,
        AdminDelayedComment: delayedComment
    };

    $.ajax({
        url: '/Admin/SaveDelayedComment',
        type: 'POST',
        data: project,
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

AdminPanel.assignEmployee = function () {

    AdminPanel.loading();

    var id = $("#projectId").val();
    var listofEmployees = $("#assignEmployeesList").val();
    var employeesEmails = "";

    for (var i = 0; i < listofEmployees.length; i++) {
        employeesEmails += listofEmployees[i] + ", ";
    }

    var project = {
        SelfProjectId: id,
        EmployeesNames: employeesEmails
    };

    $.ajax({
        url: '/Admin/AssignEmployee',
        type: 'POST',
        data: project,
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

AdminPanel.completeProject = function () {

    AdminPanel.loading();

    var id = $("#projectId").val();

    //Add validation

    var project = {
        SelfProjectId: id
    };

    $.ajax({
        url: '/Admin/CompleteProject',
        type: 'POST',
        data: project,
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

                window.location = window.location.href;
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

AdminPanel.invoiceProject = function () {

    AdminPanel.loading();

    var id = $("#projectId").val();

    var project = {
        SelfProjectId: id
    };

    $.ajax({
        url: '/Admin/InvoiceProject',
        type: 'POST',
        data: project,
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

                window.location = window.location.href;
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


AdminPanel.archiveProject = function () {

    AdminPanel.loading();

    var id = $("#projectId").val();

    var project = {
        SelfProjectId: id
    };

    $.ajax({
        url: '/Admin/ArchiveProject',
        type: 'POST',
        data: project,
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

                window.location = "/Admin/Index";
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

AdminPanel.addNewproject = function () {

    AdminPanel.loading();

    var name = $("#addProjectName").val();
    var client = $("#addProjectClient").val();
    var eng = $("#addProjectEngineering").val();
    var drawing = $("#addProjectDrawing").val();
    var approval = $("#addProjectApproval").val();
    var delivery = $("#addProjectDelivery").val();
    var schedule = $("#addProjectSchedule").val();
    var budget = $("#addProjectBudget").val();
    var estatus = $("#addProjectEStatus").val();

    var project = {
        Name: name,
        Client: client,
        Engineering: eng,
        Drawing: drawing,
        Approval: approval,
        DeliveryDate: delivery,
        Schedule: schedule,
        Budget: budget,
        EStatus: estatus
    };

    $.ajax({
        url: '/Admin/AddNewProject',
        type: 'POST',
        data: project,
        success: function (result) {
            AdminPanel.removeLoader();

            if (result.isSuccess) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Project added successfully!',
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

$("#reportType").on('change', function () {

    var href = AdminPanel.href + this.value;

    $("#generateReportHref").attr('href', href);
});

$(document).ready(function () {
});