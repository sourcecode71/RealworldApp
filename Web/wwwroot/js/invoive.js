
var LoadInvoices = () => {

    var base_url = window.location.origin;
    var searchURL = base_url + "/api/Project/project-search?searchTag=" + tagName;

    $.ajax({
        url: searchURL,
        type: 'get',
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {
            pmData = data;

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

var Invoice = function () {
    "use strict";
    return {
        initInvoice: function () {
            this.InvoiceInitialize();
        },

        InvoiceInitialize: function () {
            clearAllInvoice();
           // LoadInvoices();
           // SaveInvoice();
        }
    }
}();