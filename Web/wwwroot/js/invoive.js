
var LoadInvoices = () => {

    var base_url = window.location.origin;
    var searchURL = base_url + "/api/Project/project-search?searchTag=" + tagName;

    $.ajax({
        url: searchURL,
        type: 'get',
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {

            if (data.length > 3) {
                $("#tableBody").addClass("scroller");
            } else {
                $("#tableBody").removeClass("scroller");
            }

            $("#SpinnerDiv").removeClass("show").addClass("hide");

            var tbRow = "";
            data.forEach(function (item, index) {
                tbRow += "<tr> <th scope='row'>" + (parseInt(index) + 1) + "</th>  <td class='pc-30 tb-text-center' > " + item.name + "</td>  <td class='pc-30 tb-text-center tagId'>" + item.projectNo + "</td>" +
                    " <td class='pc-30 tr-src tb-text-center '>" + item.client + "</td>  </tr > ";
            });

            $("#tableBody").removeClass("hide").addClass("show");
            $("#tableBody").html(tbRow);
        },
        function rejectHandler(jqXHR, textStatus, errorThrown) {
            console.log(" error ", textStatus);
        }
    ).catch(function errorHandler(error) {
        console.log(" error ", error);
    });
}

var SubmitInvoice = () => {

    if (InvoiceValidation()) {

        var wrkId = $("#wrkId").val();
        var wrkNo = $("#wrkNo").val();
        var pjId = $("#projectId").val();
        var partialBill = $("#partialbill").val();
        var invBill = $("#invoicebill").val();
        var invNo = $("#invoiceNumber").val();
        var invDate = $("#invoiceDate").val();
        var remark = $("#remark").val();

        var prBills = partialBill.split(" ");
        var prBill = prBills[1].replace(",", "");

        var invBills = invBill.split(" ");
        var invoBill = invBills[1].replace(",", "");

        var invData = {
            workOrderId: wrkId,
            workNo: wrkNo,
            projectId: pjId,
            partialBill: prBill,
            invoiceBill: invoBill,
            invoiceNumber: invNo,
            invoiceDate: invDate,
            remarks: remark
        }


        var base_url = window.location.origin;
        var searchURL = base_url + "/api/WorkOrder/save-invoic";

        $.ajax({
            url: searchURL,
            type: "post",
            data: JSON.stringify(invData),
            contentType: 'application/json; charset=utf-8',
        }).then(
            function fulfillHandler(data) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Record has been save successfully!',
                    showConfirmButton: false,
                    timer: 1500
                });

                clearAllInvoice();

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

var LoadProject = () => {

    var base_url = window.location.origin;
    var searchURL = base_url + "/api/project/emp-wise/load-project?empId=" + "";

    $.ajax({
        url: searchURL,
        type: 'get',
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(result) {
            allProject = result
            var $dropdown = $("#project");
            $.each(result, function () {
                $dropdown.append($("<option />").val(this.id).text(this.name));
            });

        },
        function rejectHandler(jqXHR, textStatus, errorThrown) {
            console.log(" error ", textStatus);
        }
    ).catch(function errorHandler(error) {
        console.log(" error ", error);
    });


    $('#project').change(function () {
        var pRow = allProject.filter(p => p.id == $(this).val());
        if (pRow.length > 0) {

            $("#divPmDescription").removeClass("hide").addClass("show");

            $("#prjNo").text(pRow[0].projectNo);
            $("#prjName").text(pRow[0].name);
            $("#pjClient").text(pRow[0].clientName);
            $("#pjClient").text(pRow[0].clientName);
            $("#pjBudgetHours").text(pRow[0].budgetHours);

        }

    });
}

var SubmitHourLogs = () => {

    if (HourLogValidation()) {

        var projectId = $('#project :selected').val();
        var spentHour = $("#spentHours").val();
        var spenDate = $("#hourLogDate").val();

        var hrl = {
            projectId: projectId,
            spentHour: spentHour,
            spentDate: spenDate
        }


        var base_url = window.location.origin;
        var searchURL = base_url + "/api/company/save-hour-log";

        $.ajax({
            url: searchURL,
            type: "post",
            data: JSON.stringify(hrl),
            contentType: 'application/json; charset=utf-8',
        }).then(
            function fulfillHandler(data) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Record has been save successfully!',
                    showConfirmButton: false,
                    timer: 1500
                });

                clearAllHourLog();

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


var InvoiceValidation = () => {
    var isFormValid = true;
    var partialBill = $("#partialbill").val();
    var invBill = $("#invoicebill").val();
    var invNo = $("#invoiceNumber").val();
    var invDate = $("#invoiceDate").val();

    if (partialBill == "" || partialBill.length == 0) {
        $("#partialbillError").addClass("show").removeClass("hide");
        $("#partialbillError").text(" Please enter work partial bill. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        var partialBills = partialBill.split(" ");

        if (parseInt(partialBills[1]) > 0) {
            $("#partialbillError").addClass("hide").removeClass("show");
            $("#partialbillError").text(" ");
        } else {
            $("#partialbillError").addClass("show").removeClass("hide");
            $("#partialbillError").text(" Should be more than 0. ");
            isFormValid = false;
        }
    }

    if (invBill == "" || invBill.length == 0) {
        $("#invoicebillError").addClass("show").removeClass("hide");
        $("#invoicebillError").text(" Please enter invoice bill. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        var invBills = invBill.split(" ");

        if (parseInt(invBills[1]) > 0) {
            $("#invoicebillError").addClass("hide").removeClass("show");
            $("#invoicebillError").text(" ");
        } else {
            $("#invoicebillError").addClass("show").removeClass("hide");
            $("#invoicebillError").text(" Should be more than 0. ");
            isFormValid = false;
        }
    }


    if (invNo == "" || invNo.length == 0) {
        $("#invoiceNumberError").addClass("show").removeClass("hide");
        $("#invoiceNumberError").text(" Please enter invoice no. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        $("#invoiceNumberError").addClass("hide").removeClass("show");
        $("#invoiceNumberError").text(" ");
    }

    if (invDate == "" || invDate.length == 0) {
        $("#invoiceDateError").addClass("show").removeClass("hide");
        $("#invoiceDateError").text(" Select invoice date. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        $("#invoiceDateError").addClass("hide").removeClass("show");
        $("#invoiceDateError").text(" ");
    }

    return isFormValid;
}


var HourLogValidation = () => {
    var isFormValid = true;

    var project = $('#project :selected').val();
    var hspent = $("#spentHours").val();
    var invDate = $("#hourLogDate").val();

    if (project == "Select the project" || project.length == 0) {
        $("#projectError").addClass("show").removeClass("hide");
        $("#projectError").text(" Please select your project. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        $("#projectError").addClass("hide").removeClass("show");
        $("#projectError").text(" ");
    }

    if (hspent == "" || hspent.length == 0) {
        $("#spentHoursError").addClass("show").removeClass("hide");
        $("#spentHoursError").text(" Please enter your spent hour. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        $("#spentHoursError").addClass("hide").removeClass("show");
        $("#spentHoursError").text(" ");
    }

    if (invDate == "" || invDate.length == 0) {
        $("#hourLogDateError").addClass("show").removeClass("hide");
        $("#hourLogDateError").text(" Please select hour log date. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        $("#hourLogDateError").addClass("hide").removeClass("show");
        $("#hourLogDateError").text(" ");
    }

   

    return isFormValid;
}

var clearAllInvoice = () => {
    $("#workOrderName").val("");
    $("#divPmDescription").removeClass("show").addClass("hide");
    $("#wrkId").val("");
    $("#wrkNo").val("");
    $("#projectId").val("");
    $("#partialbill").val("");
    $("#invoicebill").val("");
    $("#invoiceNumber").val("");
    $("#invoiceDate").val("");
    $("#remark").val("");
    $("#partialbillError").addClass("hide").removeClass("show");
    $("#invoicebillError").addClass("hide").removeClass("show");
    $("#invoiceNumberError").addClass("hide").removeClass("show");
    $("#invoiceDateError").addClass("hide").removeClass("show");
}

var clearAllHourLog = () => {
    $("#divPmDescription").removeClass("show").addClass("hide");
    $("#spentHours").val("");
    $("#hourLog").val("");
    $("#remarks").val("");
    $("#spentHoursError").addClass("hide").removeClass("show");
    $("#hourLogDateError").addClass("hide").removeClass("show");
    $("#projectError").addClass("hide").removeClass("show");
}



var Invoice = function () {
    "use strict";
    return {
        initInvoice: function () {
            this.InvoiceInitialize();
        },

        initHourLog: function () {
            this.HourLogInisilize();
        },

        HourLogInisilize: function () {
            clearAllHourLog();
            LoadProject();
        },
        InvoiceInitialize: function () {
            clearAllInvoice();
           // LoadInvoices();
           // SaveInvoice();
        }
    }
}();