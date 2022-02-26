var SearchWorksOrder = () => {

    $("#workOrderName").val("");

    $('#workOrderName').on('input', function () {
        clearTimeout(this.delay);
        this.delay = setTimeout(function () {
            CallWorkOrderSearch(this.value);

        }.bind(this), 550);
    });

}

var CallWorkOrderSearch = (param) => {


    var base_url = window.location.origin;

    $("#SBoxDiv").removeClass("hide").addClass("show");
    $("#SpinnerDiv").removeClass("hide").addClass("show");
    $("#tableBody").removeClass("show").addClass("hide");

    ClearAllInvoice();

    var searchURL = base_url + "/api/workOrder/work-order-search?strOT=" + param;

    $.ajax({
        url: searchURL,
        type: 'get',
        contentType: 'application/json; charset=utf-8',
    }).then(
        function fulfillHandler(data) {
            wrkData = data;

            if (data.length > 3) {
                $("#tableBody").addClass("scroller");
            } else {
                $("#tableBody").removeClass("scroller");
            }

            $("#SpinnerDiv").removeClass("show").addClass("hide");

            var tbRow = "";
            data.forEach(function (item, index) {
                tbRow += "<tr> <th scope='row'>" + (parseInt(index) + 1) + "</th>  <td class='pc-30 tb-text-center wrkNo' > "+ item.workOrderNo +"</td>  <td class='pc-30 tb-text-center tagId'>" + item.otDescription + "</td>" +
                    " <td class='pc-30 tr-src tb-text-center '>" + item.approvedBudget + "</td>  </tr > ";
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


    $("#SBoxDiv").on('click', '.pc-30', function () {


        var wrkNo = $(this).closest("tr").find(".wrkNo").text();
        var rowData = wrkData.filter(p => p.workOrderNo == wrkNo.trim());

        console.log("rowData ", rowData);

        if (wrkNo && rowData.length > 0) {

            $("#wrkNoTx").text(wrkNo);
            $("#wrkDes").text(rowData[0].otDescription);
            $("#wrkBudget").text(formatMoney(rowData[0].approvedBudget));
            $("#pjClient").text(rowData[0].clinetName);
            $("#pjNo").text(rowData[0].projectNo);
            $("#projectId").val(rowData[0].projectId);
            $("#pjYear").text(rowData[0].projectYear);
            $("#wrkId").val(rowData[0].id);
            $("#wrkNo").val(wrkNo);
            $("#divPmDescription").addClass("show").removeClass("hide");
        } else {
            $("#divPmDescription").removeClass("show").addClass("hide");
        }

        $("#tableBody").html("");
        $("#SBoxDiv").removeClass("show").addClass("hide");
    });
}


var ClearAllInvoice = () => {

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

var WorkOrder = function () {
    "use strict";
    return {
        initWorkOrderFilter: function () {
            this.WorkOrderFilterInitialize();
        },

        WorkOrderFilterInitialize: function () {
            SearchWorksOrder();
        }
    }
}();