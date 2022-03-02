var SearchProjects = function () {
    $("#projectName").val("");

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
        var approvedBudget = $("#addProjectBudget").val().split(" ");
        var dcMoney = approvedBudget[1].replace(",", "");

        var approvalData = {
            id: $("#pmId").val(),
            projectNo: $("#pjNo").text(),
            apporvalSatus: 0,
            budget: approvedBudget.length > 0 ? dcMoney : $("#addProjectBudget").val() ,
            comments: $("#comments").val()
        }

        var base_url = window.location.origin;
        var searchURL = base_url + "/api/project/project-budget/submit";

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
                ClearAllProjectActivities();
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

            loadedData = data;

            $("#budgetApprovalAct").empty();
            var tbRow = "";
            data.forEach(function (item, index) {

                console.log(" item.approvalDateStr  ", item.approvalDateStr);

                var appBudget = item.approvedBudget ? formatMoney(item.approvedBudget) : " -";
                var appDate = item.approvalDateStr == "01/01/0001" ? "-" : item.approvalDateStr;
                
                tbRow += "<tr> <th scope='row'>" + (parseInt(index) + 1) + "</th>  <td class='pc-30 tb-text-center budgetNo' > " + item.budegtNo + "</td>  <td class='pc-30 tb-text-center tagId'>" + item.projectName + "</td>" +
                    " <td class='pc-30 tr-src tb-text-center'>" + formatMoney(item.budget) + "</td> "+
                    " <td class='pc-30 tr-src tb-text-center'>" + item.budgetSubmitDateStr + "</td> "+
                    " <td class='pc-30 tr-src tb-text-center'>" + item.approvalStatusStr + "</td> "+
                    " <td class='pc-30 tr-src tb-text-center'>" + appBudget + "</td> "+
                    " <td class='pc-30 tr-src tb-text-center'>" + appDate + "</td>"+
                    "<td class='pc-30 tr-src tb-text-center'> <button class='mb-2 mr-2 btn-transition btn btn-outline-warning btnBudgetApr ' >  <i class='fa fa-pencil-square-o' aria-hidden='true'> </i>  change </button > </td>  </tr > ";
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

function SelectedActivitiesProject() {

    $("#budgetApprovalAct").on('click', '.btnBudgetApr', function () {

        var budgetID = $(this).closest("tr").find(".budgetNo").text();
        var rowData = loadedData.filter(p => parseInt(p.budegtNo) == parseInt(budgetID));

        if (rowData[0].approvalStatus == 0) {

            $("#exampleModal").modal("show");

            setTimeout(() => {
                $('input:radio[name="apStatus"]').filter('[value="1"]').attr('checked', true);
            }, 100);


            $("#appComment").text("Comments :");

            if (loadedData && budgetID) {
                $("#submitBudget").text("Submitted Budget :  " + formatMoney(rowData[0].budget));
                $("#pmId").val(rowData[0].projectId)
            }

        } else {

            var msg = rowData[0].approvalStatus == 1 ? "Already the budget is approved" : "Already the budget is refused";

            Swal.fire({
                position: 'top-end',
                title: 'Info!',
                text: msg,
                icon: 'info',
                showConfirmButton: false,
                timer: 2000
            });

        }

        
    });

}

var SelectControls = function () {
    $("#spComment").text("Comments");
    $('input[type=radio][name=apStatus]').change(function () {

        if (this.value == 1) {
            $("#divAppBudget").addClass("show").removeClass("hide");
            $("#appComment").text("Comments :");

        } else {
            $("#divAppBudget").addClass("hide").removeClass("show");
            $("#appComment").text("Not approval reason :");
            $("#addProjectBudget").val('');
        }



    });
}


$("#submitBudgetApproval").click(() => {

    if (ValidationApprovedBudget()) {

        var approvedBudget = $("#approvedBudget").val().split(" ");
        var dcMoney = approvedBudget[1].replace(",", "");
        var pmId = $("#pmId").val();

        var slBudget = loadedData.filter(p => p.projectId == pmId);
        var slApproval = $("input[type='radio'][name='apStatus']:checked").val();

        var appBudget = {
            "id": pmId,
            "budegtNo": slBudget[0].budegtNo,
            "status": slApproval,
            "approvedBudget": dcMoney,
            "comments": $("appComments").val()
        }

        var base_url = window.location.origin;
        var searchURL = base_url + "/api/project/budegt-approval/status";


        $.ajax({
            url: searchURL,
            type: 'post',
            data: JSON.stringify(appBudget),
            contentType: 'application/json; charset=utf-8',
        }).then(
            function fulfillHandler(data) {

                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Record has been added successfully!',
                    showConfirmButton: false,
                    timer: 1500
                })
                ClearAllProjectActivities();
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
});


function SubmitWorkOrder() {

    if ($("#spSaveUpdate").text().trim() == "Submit") {
        SaveUpateWorkOrder("sb");
    } else {
        SaveUpateWorkOrder("up");
    }

}


var SaveUpateWorkOrder = (operation) => {


    $("#SBoxDiv").removeClass("show").addClass("hide");

    var validation = PmBudgetWorkOrderValidation();


    if (validation) {

        var approvedBudget = $("#addProjectBudget").val().split(" ");
        var dcMoney = approvedBudget[1].replace(",", "");

        var approvalData = {
            projectId: $("#pmId").val(),
            workOrderId: $("#wrkId").val(),
            projectNo: $("#pjNo").text(),
            oTDescription: $("#consecutiveWork").val(),
            approvedBudget: approvedBudget.length > 0 ? dcMoney : $("#addProjectBudget").val(),
            comments: $("#comments").val()
        }

        var strOperation = operation == "sb" ? "store-work-order" : "update-work-order";

        var base_url = window.location.origin;
        var searchURL = base_url + "/api/WorkOrder/" + strOperation;

        $.ajax({
            url: searchURL,
            type: operation == "sb"? 'post' : "put",
            data: JSON.stringify(approvalData),
            contentType: 'application/json; charset=utf-8',
        }).then(
            function fulfillHandler(data) {

                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Record has been added successfully!',
                    showConfirmButton: false,
                    timer: 1500
                });

                ClearAllWorkOrder();
                LoadProjectWorkOrder();

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



function LoadProjectWorkOrder() {

    var base_url = window.location.origin;
    var searchURL = base_url + "/api/WorkOrder/load-approved-orders";

    $.ajax({
        url: searchURL,
        type: 'get',
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {

            loadedWrkData = data;
            $("#budgetApprovalAct").empty();
            var tbRow = "";
            data.forEach(function (item, index) {
                tbRow += "<tr> <th scope='row'>" + (parseInt(index) + 1) + "</th>  <td class='pc-30 tb-text-center' > " + item.projectName + "</td>  <td class='pc-30 tb-text-center wrkNo'>" + item.workOrderNo + "</td>" +
                    " <td class='pc-30 tr-src'>" + item.otDescription + "</td>   <td class='pc-30 tr-src tb-text-center '>" + formatMoney(item.approvedBudget) + "</td>  " +
                    " <td class='pc-30 tr-src tb-text-center '>" + item.approvedDateStr + "</td>   <td class='pc-30 tr-src tb-text-center '>" +
                    "<button class='mb-2 mr-2 btn-transition btn btn-outline-focus btn-wrk' >  <i class='fa fa-pencil-square-o' aria-hidden='true'> </i>  Edit </button > </td>  </tr > ";
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


var SelectWrkRow = ()=> {

    $("#budgetApprovalAct").on('click', '.btn-wrk', function () {

        var wrkOD = $(this).closest("tr").find(".wrkNo").text();

        if (loadedWrkData && wrkOD) {
            var wrkRow = (loadedWrkData.filter(p => p.workOrderNo == wrkOD))[0];

            $("#wrkId").val(wrkRow.id);
            $("#pmId").val(wrkRow.projectId);
            $("#workOrderNo").val(wrkOD);
            $("#projectName").val(wrkRow.projectName);
            $("#pjNo").text(wrkRow.projectNo);
            $("#pjYear").text(wrkRow.projectYear);
            $("#pjClient").text(wrkRow.clinetName);
            $("#pjBudget").text(formatMoney(wrkRow.projectBudget, ',', '.'));
            $("#divPmDescription").addClass("show").removeClass("hide");

            $("#addProjectBudget").val(formatMoney(wrkRow.approvedBudget, ',', '.'));
            $("#consecutiveWork").val(wrkRow.otDescription);
            $("#comments").val(wrkRow.comments);
            $("#spSaveUpdate").text("Update");
            $("#otBudget").text(" Modified Budget :");


        } else {
            $("#divPmDescription").removeClass("show").addClass("hide");
        }
        $("#tableBody").html("");
        $("#SBoxDiv").removeClass("show").addClass("hide");
    });

}

var LoadActiveProject = () => {

    var base_url = window.location.origin;
    var searchURL = base_url + "/api/project/load-active-projects";

    $.ajax({
        url: searchURL,
        type: 'get',
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {
            loadedPrjData = data;
            $("#tbProjectStatus").empty();
            var tbRow = "";
            data.forEach(function (item, index) {
                tbRow += "<tr> <th scope='row'>" + (parseInt(index) + 1) + "</th>  <td class='pc-30 tb-text-center prjNo' > " + item.projectNo + "</td>  <td class='pc-30 tb-text-center'>" + item.name + "</td>" +
                    " <td class='pc-30 tr-src'>" + item.clientName + "</td>  <td class='pc-30 tr-src'>" + item.budgetApprovalStr + "</td>  <td class='pc-30 tr-src tb-text-center '>" + formatMoney(item.budget) + "</td>  " +
                    " <td class='pc-30 tr-src tb-text-center '>" + item.status + "</td>   <td class='pc-30 tr-src tb-text-center '>" +
                    "<button class='mb-2  btn-transition btn btn-outline-info btn-prj' >  <i class='fa fa-pencil-square-o' aria-hidden='true'> </i>  Status Change </button > </td>  </tr > ";
            });

            $("#tbProjectStatus").append(tbRow);
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


var SelectedProject=()=> {

    $("#tbProjectStatus").on('click', '.btn-prj', function () {

        var projectNo = $(this).closest("tr").find(".prjNo").text();
        var rowData = loadedPrjData.filter(p => parseInt(p.projectNo) == parseInt(projectNo));

        if (projectNo == null || projectNo == 'null'  || rowData.length == 0) {
            return false;
        }

        $("#pmNo").val(projectNo);
        $("#pmId").val(rowData[0].id);

        if (rowData.length>0 && rowData[0].approvalStatus != 0) {

            var pStatusText = (rowData[0].status).toLowerCase();
            $("#projectstatusModal").modal("show");

            


            $("#projectStatus").each(function () {
                $('option', this).each(function () {
                    if ($(this).text().toLowerCase() == pStatusText) {
                        $(this).attr('selected', 'selected')
                    };
                });
            });


        } else {

            var msg = rowData[0].approvalStatus == 0 ? "The budget is not approved" : "Already the budget is refused";

            Swal.fire({
                position: 'top-end',
                title: 'Info!',
                text: msg,
                icon: 'info',
                showConfirmButton: false,
                timer: 2000
            });

        }


    });

}

var LoadProjectStatus = () => {


    var base_url = window.location.origin;
    var searchURL = base_url + "/api/project/project-status";

    $.ajax({
        url: searchURL,
        type: 'get',
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {
            loadedPrjData = data;
            var $dropdown = $("#projectStatus");
            $dropdown.append($("<option />").val(0).text(" Select Project Status "));
            $.each(loadedPrjData, function () {
                $dropdown.append($("<option />").val(this.value).text(this.statusName));
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


$("#submitProjectStatusChange").click(()=> {

    var projectNo=  $("#pmNo").val();
    var projectId = $("#pmId").val();


    if (projectNo || projectId) {
        if (ValidateStatusChange()) {

            var pStatus = {
                projectId: projectId,
                projectNo: projectNo,
                Status: $("#projectStatus").children("option:selected").val()
            }

            console.log(" projectNo ---  ", pStatus);

            var base_url = window.location.origin;
            var statusChangeURL = base_url + "/api/project/project-activities/status";


            $.ajax({
                url: statusChangeURL,
                type: 'put',
                data: JSON.stringify(pStatus),
                contentType: 'application/json; charset=utf-8',
            }).then(
                function fulfillHandler(data) {

                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Record has been added successfully!',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    LoadActiveProject();

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



            $("#projectstatusModal").modal('hide');

        }

    } else {
        Swal.fire({
            position: 'top-end',
            title: 'Info!',
            text: "something went wrong",
            icon: 'info',
            showConfirmButton: false,
            timer: 2000
        });
    }

});

var LoadProjectActivitiesStatus = () => {

    var base_url = window.location.origin;
    var searchURL = base_url + "/api/project/load-active-projects";

    $.ajax({
        url: searchURL,
        type: 'get',
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {
            prjActStatusData = data;

            console.log(" prjActStatusData --- ", prjActStatusData);

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

// Commmon Work


function ClearAllProjectActivities() {
    $("#pmId").val("");
    $("#pjNo").text("");
    $("#SBoxDiv").removeClass("show").addClass("hide");
    $("#divPmDescription").removeClass("show").addClass("hide");
    $("#comments").val("");
    $("#addProjectBudget").val("");
    $("#projectName").val("");
}

function ClearAllWorkOrder() {
    $("#pmId").val("");
    $("#pjNo").text("");
    $("#SBoxDiv").removeClass("show").addClass("hide");
    $("#divPmDescription").removeClass("show").addClass("hide");
    $("#comments").val("");
    $("#addProjectBudget").val("");
    $("#consecutiveWork").val("");
    $("#projectName").val("");
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

function PmBudgetWorkOrderValidation() {
    var isFormValid = true;

    if (($("#projectName").val() == "" || $("#projectName").length == 0)  )  {
        $("#projectNameError").addClass("show").removeClass("hide");
        $("#projectNameError").text(" Please select the project name. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        $("#projectNameError").addClass("hide").removeClass("show");
        $("#projectNameError").text(" ");
    }

    if ($("#addProjectBudget").val() == "" || $("#addProjectBudget").length == 0) {
        $("#addProjectBudgetError").addClass("show").removeClass("hide");
        $("#addProjectBudgetError").text(" Please enter work order budget. ");
        isFormValid = false;
    }
    else if (isFormValid ) {
        var approvedBudget = $("#addProjectBudget").val().split(" ");

        if (parseInt(approvedBudget[1]) > 0) {
            $("#addProjectBudgetError").addClass("hide").removeClass("show");
            $("#addProjectBudgetError").text(" ");
        } else {
            $("#addProjectBudgetError").addClass("show").removeClass("hide");
            $("#addProjectBudgetError").text(" Approved budget should be more than 0. ");
            isFormValid = false;
        }
    }

    if ($("#consecutiveWork").val() == "" || $("#consecutiveWork").length == 0) {
        $("#consecutiveWorkError").addClass("show").removeClass("hide");
        $("#consecutiveWorkError").text(" Please enter consecutive work description. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        $("#consecutiveWorkError").addClass("hide").removeClass("show");
        $("#consecutiveWorkError").text(" ");
    }

    return isFormValid;
}

var ValidationApprovedBudget = () =>
{
    var isFormValid = true;
    var slApproval = $("input[type='radio'][name='apStatus']:checked").val();

    if (slApproval == 1) {
        var approvedBudget = $("#approvedBudget").val().split(" ");

        if ($("#approvedBudget").val() == "" || $("#approvedBudget").length == 0) {

            $("#approvedBudgetError").addClass("show").removeClass("hide");
            $("#approvedBudgetError").text(" Please enter work order budget. ");
            isFormValid = false;
        }
        else if (isFormValid) {

            var approvedBudget = $("#approvedBudget").val().split(" ");
            var dcMoney = approvedBudget[1].replace(",", "");

            if (dcMoney > 0) {
                $("#approvedBudgetError").addClass("hide").removeClass("show");
                $("#approvedBudgetError").text(" ");
            } else {
                $("#approvedBudgetError").addClass("show").removeClass("hide");
                $("#approvedBudgetError").text(" Approved budget should be more than 0. ");
                isFormValid = false;
            }
        }
 }

    if (slApproval == 2) {
        if ($("#appComment").val() == "" || $("#appComment").length == 0) {
            $("#appCommentError").addClass("show").removeClass("hide");
            $("#appCommentError").text(" Please enter reason of not approved the budget. ");
            isFormValid = false;
        } else {
            $("#appCommentError").addClass("hide").removeClass("show");
            $("#appCommentError").text("  ");
            isFormValid = true;
        }
    } else {
        $("#appCommentError").addClass("hide").removeClass("show");
        $("#appCommentError").text("  ");
    }

      
    
    return isFormValid;
}

var ValidateStatusChange = () => {

    var isFormValid = true;
    if ($("#projectStatus").children("option:selected").val() == 0 ) {

        $("#projectStatusError").addClass("show").removeClass("hide");
        $("#projectStatusError").text(" Please select the status. ");
        isFormValid = false;
    }
    else if (isFormValid) {
        $("#projectStatusError").addClass("hide").removeClass("show");
    }

    return isFormValid;

}


function formatMoney(number, decPlaces, decSep, thouSep) {

    decPlaces = isNaN(decPlaces = Math.abs(decPlaces)) ? 2 : decPlaces,
        decSep = typeof decSep === "undefined" ? "." : decSep;
    thouSep = typeof thouSep === "undefined" ? "," : thouSep;
    var sign = number < 0 ? "-" : "";
    var i = String(parseInt(number = Math.abs(Number(number) || 0).toFixed(decPlaces)));
    var j = (j = i.length) > 3 ? j % 3 : 0;

    return "$ " + sign +
        (j ? i.substr(0, j) + thouSep : "") +
        i.substr(j).replace(/(\decSep{3})(?=\decSep)/g, "$1" + thouSep) +
        (decPlaces ? decSep + Math.abs(number - i).toFixed(decPlaces).slice(2) : "");
}


var Projects = function () {
    "use strict";

    return {
        initBudgetAct: function () {
            this.initProjects();
        },
        initProjects: function () {
            SearchProjects();
            SelectControls();
            LoadProjectBudgetActivities();
            SelectedActivitiesProject();
            SelectedProject();
            LoadActiveProject();
            LoadProjectStatus();
        },

        initWorkOrder: function () {
            SearchProjects();
            LoadProjectWorkOrder();
            SelectWrkRow();
        }
    }
}();

