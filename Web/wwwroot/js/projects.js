var SearchProjects = function () {
    $("#projectName").val("");

    $('input:radio[name="apStatus"]').filter('[value="1"]').attr('checked', true);

    $('#projectName').on('input', function () {
        clearTimeout(this.delay);
        this.delay = setTimeout(function () {
             CallProjectSearch(this.value);
            console.log(this.value);

        }.bind(this), 550);
    });

    function CallProjectSearch(tagName) {

        var base_url = window.location.origin;

        $("#SBoxDiv").removeClass("hide").addClass("show");
        $("#SpinnerDiv").removeClass("hide").addClass("show");
        $("#tableBody").removeClass("show").addClass("hide");

        $("#projectNameError").addClass("hide").removeClass("show");
        $("#projectNameError").text(" ");

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


    $("#tableBody").on('click', '.pc-30', function () {

        var tagID = $(this).closest("tr").find(".tagId").text();

        if (pmData && tagID) {
            var pmRow = (pmData.filter(p => p.projectNo == tagID))[0];

            console.log(" data ", pmRow);

            $("#pmId").val(pmRow.id);
            $("#projectName").val(pmRow.name);
            $("#pjNo").text(pmRow.projectNo);
            $("#pjYear").text(pmRow.year);
            $("#pjClient").text(pmRow.client);
            $("#pjBudget").text(formatMoney(pmRow.budget, ',', '.'));
            $("#spBudgetBalance").text(formatMoney(pmRow.balance,',','.'));

            $("#divPmDescription").addClass("show").removeClass("hide");

        } else {
            $("#divPmDescription").removeClass("show").addClass("hide");
        }
        $("#tableBody").html("");
        $("#SBoxDiv").removeClass("show").addClass("hide");
    });
    //CallTagSearch("acti");
}


function SubmitBudget() {

     $("#SBoxDiv").removeClass("show").addClass("hide");

    var validation = PmBudgetApprovalValidation();

    if (validation) {


        //console.log($("#pmId").val(), " validation --- ", $("#pjNo").text());

        var slApproval = $("input[type='radio'][name='apStatus']:checked").val();

        var approvedBudget = $("#addProjectBudget").val().split(" ");

        var approvalData = {
            id: $("#pmId").val(),
            projectNo: $("#pjNo").text(),
            apporvalSatus: slApproval,
            approvedBudget: approvedBudget.length > 0 ? approvedBudget[1] : $("#addProjectBudget").val() ,
            comments: $("#comments").val()
        }

        var base_url = window.location.origin;
        var searchURL = base_url + "/api/project/project-approval/status";

        $.ajax({
            url: searchURL,
            type: 'post',
            data: JSON.stringify(approvalData),
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

                LoadProjectBudgetActivities();
               
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

var SelectControls = function () {

    $('input[type=radio][name=apStatus]').change(function () {

        if (this.value == 1) {
            $("#divBudget").addClass("show").removeClass("hide");
            $("#spComment").text("Comments");

        } else {
            $("#divBudget").addClass("hide").removeClass("show");
            $("#spComment").text("Reasonnot Approved Reason");
            $("#addProjectBudget").val('');
        }
    });
}


function LoadProjectBudgetActivities() {
    var PmName = "NA"
    var base_url = window.location.origin;
    var searchURL = base_url + "/api/project/project-activities/budget-load?PmName=" + PmName;

    $.ajax({
        url: searchURL,
        type: 'get',
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {

            console.log(" project name ", data);

            loadedData = data;

            var tbRow = "";
            data.forEach(function (item, index) {
                tbRow += "<tr> <th scope='row'>" + (parseInt(index) + 1) + "</th>  <td class='pc-30 tb-text-center budgetNo' > " + item.budegtNo + "</td>  <td class='pc-30 tb-text-center tagId'>" + item.projectName + "</td>" +
                    " <td class='pc-30 tr-src tb-text-center '>" + item.clientName + "</td>   <td class='pc-30 tr-src tb-text-center '>" + item.approvedBudget + "</td>  " +
                    " <td class='pc-30 tr-src tb-text-center '>" + item.balance + "</td>   <td class='pc-30 tr-src tb-text-center '>" + item.approvalDateStr + "</td>   <td class='pc-30 tr-src tb-text-center '>"+
                    "<button class='mb-2 mr-2 btn-transition btn btn-outline-focus ' >  <i class='fa fa-check-square-o' aria-hidden='true'> </i>  Select </button > </td>  </tr > ";
            });

            $("#budgetApprovalAct").append(tbRow);
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

function SelectedProject() {

    $("#budgetApprovalAct").on('click', '.btn', function () {

        var budgetID = $(this).closest("tr").find(".budgetNo").text();

        if (loadedData && budgetID) {

            var rowData = loadedData.filter(p => parseInt(p.budegtNo) == parseInt(budgetID));

            console.log(" rowData ", rowData);

            $("#projectName").val(rowData[0].projectName);
            $("#spBudgetBalance").text(formatMoney(rowData[0].balance));
            $("#addProjectBudget").val(rowData[0].approvedBudget);
            $("#spComment").val(rowData[0].comments);
            $("#pmId").val(rowData[0].projectId);
        }
    });

}

function formatMoney(number, decPlaces, decSep, thouSep) {

    decPlaces = isNaN(decPlaces = Math.abs(decPlaces)) ? 2 : decPlaces,
        decSep = typeof decSep === "undefined" ? "." : decSep;
    thouSep = typeof thouSep === "undefined" ? "," : thouSep;
    var sign = number < 0 ? "-" : "";
    var i = String(parseInt(number = Math.abs(Number(number) || 0).toFixed(decPlaces)));
    var j = (j = i.length) > 3 ? j % 3 : 0;

    return "$" + sign +
        (j ? i.substr(0, j) + thouSep : "") +
        i.substr(j).replace(/(\decSep{3})(?=\decSep)/g, "$1" + thouSep) +
        (decPlaces ? decSep + Math.abs(number - i).toFixed(decPlaces).slice(2) : "");
}

function PmBudgetApprovalValidation() {

    var isFormValid = true;

    if ($("#projectName").val() == "" || $("#projectName").length == 0) {
        $("#projectNameError").addClass("show").removeClass("hide");
        $("#projectNameError").text(" Please select the project name. ");
        isFormValid = false;
    }
    else {
        $("#projectNameError").addClass("hide").removeClass("show");
        $("#projectNameError").text(" ");
        isFormValid = true;
    }

    var slApproval = $("input[type='radio'][name='apStatus']:checked").val();


    if (($("#addProjectBudget").val() == "" || $("#addProjectBudget").length == 0) && slApproval == 1) {
        isFormValid = false;
        $("#approvalBudgetError").addClass("show").removeClass("hide");
        $("#approvalBudgetError").text("Please enter the approved budget.");
    }
    else {
        isFormValid = true;
        $("#approvalBudgetError").addClass("hide").removeClass("show");
        $("#approvalBudgetError").text(" ");
    }

    return isFormValid;
}


var Projects = function () {
    "use strict";

    return {
        init: function () {
            this.initProjects();
        },
        initProjects: function () {
            SearchProjects();
            SelectControls();
            LoadProjectBudgetActivities();
            SelectedProject();
        }
    }
}();

$(function () {
    Projects.init();
});