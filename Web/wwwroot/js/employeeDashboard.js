var EmployeeDashboard = {

}

EmployeeDashboard.getProjects = function () {

    $.ajax({
        url: '/Employee/GetProjects',
        type: 'GET',
        success: function (result) {
            var i = 1;
            $.each(result, function (index, value) {
                $('#Projectslist').append($('<li/>', {
                    value: i,
                    text: value.name + "\n" + value.description
                }));

                i++
            });
        },
        error: function (err) {

        }
    });
}

$(document).ready(function () {
    EmployeeDashboard.getProjects();
});