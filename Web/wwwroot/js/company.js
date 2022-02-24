
var CompanyActivity=()=>
{

}

function SubmitClient() {

    var base_url = window.location.origin;
    var clientURL = base_url + "/api/Company/save-client";

    var clientData = {
        name: $("#clientName").val(),
        address: $("#clientAddress").val()
    }

    $.ajax({
        url: clientURL,
        type: 'post',
        data: JSON.stringify(clientData),
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {

            console.log("  data ", data);

            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Record has been added successfully!',
                showConfirmButton: false,
                timer: 1500
            })
            ClearAllClient();
            LoadAllClient();

        },
        function rejectHandler(jqXHR, textStatus, errorThrown) {
            Swal.fire({
                position: 'top-end',
                title: 'Error!',
                text: 'Something went wrong.' + errorThrown.errorMessage,
                icon: 'error',
                confirmButtonText: 'Ok',
            })
        }
    ).catch(function errorHandler(error) {
        Swal.fire({
            position: 'top-end',
            title: 'Error!',
            text: 'Something went wrong.' + error,
            icon: 'error',
            confirmButtonText: 'Ok',
        })
    });


}

function SubmitCompany() {

    var base_url = window.location.origin;
    var companyURL = base_url + "/api/Company/save-company";

    if (ValidationCompany()) {

    var companyData = {
        clientId: $("#companyCLient").children("option:selected").val(),
        name: $("#companyName").val(),
        address: $("#companyAddress").val()
        }

        console.log(" companyData ---- ", companyData);


    $.ajax({
        url: companyURL,
        type: 'post',
        data: JSON.stringify(companyData),
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {

            console.log("  data ", data);

            Swal.fire({
                position: 'top-end',
                icon: 'success',
                title: 'Record has been added successfully!',
                showConfirmButton: false,
                timer: 1500
            })
            ClearCompany();
            LoadAllCompany();

        },
        function rejectHandler(jqXHR, textStatus, errorThrown) {
            Swal.fire({
                position: 'top-end',
                title: 'Error!',
                text: 'Something went wrong.' + errorThrown.errorMessage,
                icon: 'error',
                confirmButtonText: 'Ok',
            })
        }
    ).catch(function errorHandler(error) {
        Swal.fire({
            position: 'top-end',
            title: 'Error!',
            text: 'Something went wrong.' + error,
            icon: 'error',
            confirmButtonText: 'Ok',
        })
    });


  }
}

var LoadAllClient = () => {
    var base_url = window.location.origin;
    var clientURL = base_url + "/api/company/all-clients";

    $.ajax({
        url: clientURL,
        type: 'get',
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {


            loadedClientData = data;

            $("#tbClient").empty();
            var tbRow = "";
            data.forEach(function (item, index) {
                tbRow += "<tr> <th scope='row'>" + (parseInt(index) + 1) + "</th>  <td class='pc-30 tb-text-left clientName' > " + item.name + "</td>  <td class='pc-30 tb-text-left clientAddress'>" + item.address + "</td> </tr>";
            });
            $("#tbClient").append(tbRow);

            var $dropdown = $("#companyCLient");
            $dropdown.append($("<option />").val(0).text(" Select Client "));
            $.each(loadedClientData, function () {
                $dropdown.append($("<option />").val(this.id).text(this.name));
            });

        },
        function rejectHandler(jqXHR, textStatus, errorThrown) {
            Swal.fire({
                position: 'top-end',
                title: 'Error!',
                text: 'Something went wrong.' + errorThrown.errorMessage,
                icon: 'error',
                confirmButtonText: 'Ok',
            })
        }
    ).catch(function errorHandler(error) {
        Swal.fire({
            position: 'top-end',
            title: 'Error!',
            text: 'Something went wrong.' + error,
            icon: 'error',
            confirmButtonText: 'Ok',
        })
    });
}

var LoadAllCompany = () => {
    var base_url = window.location.origin;
    var clientURL = base_url + "/api/company/all-companies";

    $.ajax({
        url: clientURL,
        type: 'get',
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {


            loadedClientData = data;

            $("#tbCompany").empty();
            var tbRow = "";
            data.forEach(function (item, index) {
                tbRow += "<tr> <th scope='row'>" + (parseInt(index) + 1) + "</th> <td class='pc-30 tb-text-left clientName' > " + item.clientName + "</td>  <td class='pc-30 tb-text-left clientName' > " + item.name + "</td>  <td class='pc-30 tb-text-left clientAddress'>" + item.address + "</td> </tr>";
            });
            $("#tbCompany").append(tbRow);
        },
        function rejectHandler(jqXHR, textStatus, errorThrown) {
            Swal.fire({
                position: 'top-end',
                title: 'Error!',
                text: 'Something went wrong.' + errorThrown.errorMessage,
                icon: 'error',
                confirmButtonText: 'Ok',
            })
        }
    ).catch(function errorHandler(error) {
        Swal.fire({
            position: 'top-end',
            title: 'Error!',
            text: 'Something went wrong.' + error,
            icon: 'error',
            confirmButtonText: 'Ok',
        })
    });
}

var ValidationClient=()=> {
    var isFormValid = true;

    if ($("#clientName").val() == "" || $("#clientName").length == 0) {

        $("#clientNameError").addClass("show").removeClass("hide");
        $("#clientNameError").text(" Please enter the client name. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        $("#clientNameError").addClass("hide").removeClass("show");
        $("#clientNameError").text(" ");
    }

    return isFormValid;
}

var ValidationCompany = () => {
    var isFormValid = true;

    if ($("#companyCLient").children("option:selected").val()== 0) {

        $("#companyCLientError").addClass("show").removeClass("hide");
        $("#companyCLientError").text(" Please select the client name. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        $("#clientNameError").addClass("hide").removeClass("show");
        $("#clientNameError").text(" ");
    }

    if ($("#companyName").val() == "" || $("#companyName").length == 0) {

        $("#companyNameError").addClass("show").removeClass("hide");
        $("#companyNameError").text(" Please enter the company name. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        $("#companyNameError").addClass("hide").removeClass("show");
        $("#companyNameError").text(" ");
    }

    return isFormValid;
}

var ClearAllClient = () => {
    $("#clientName").val("");
    $("#clientAddress").val("");
    $("#clientNameError").addClass("hide").removeClass("show");
    $("#clientNameError").text(" ");
}

var ClearCompany = () => {
    $("#companyCLient").select(0).attr("selected", "selected");
    $("#companyCLientError").addClass("hide").removeClass("show");
    $("#companyCLientError").text(" ");
    $("#companyName").val("");
    $("#companyAddress").val("");
    $("#companyNameError").addClass("hide").removeClass("show");
    $("#companyNameError").text(" ");
  
}

var Company = function () {
    "use strict";
    return {
        initCompany: function () {
            this.CompanyInitialize();
        },

        CompanyInitialize : function()
        {
            LoadAllClient();
            CompanyActivity();
            ClearAllClient();
            LoadAllCompany();
            ClearCompany();
        }
    }
}();