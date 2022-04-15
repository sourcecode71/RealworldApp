const app = new Vue({
    el: '#archiveDiv',
    beforeMount() {
        this.loadAllWorkOrder();
    },
    data: {
        errors: [],
        workOrders: [],
        wrkS: '0',
        allInv: false,
        allHrs: false,
        isHrsDetails: false,
        projectNo: '',
        consProject: '',
        projectYear: '',
        wrkNo: '',
        wrkConsWork: '',
        wrkBudgetNo: '',
        wrkStartDate: '',
        wrkEndDate: '',
        sbudget: '',
        abudget: '',
        ahours: '',
        companyName: '',
        description: '',
        bhours: '',
        ahours: '',
        sbudget: '',
        abudget: '',
        paidAmt: 0,
        clientName: '',
        wrkStatusStr: '',
        assignEmp: [],
        empHrs:[]
    },
    methods: {

       
        loadAllWorkOrder: function () {

            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/WorkOrder/load-work-orders/active";

            axios.get(clientURL, config).then(
                (result) => {

                    $("#wrkAdminPanel").dataTable().fnDestroy();

                    this.workOrders = result.data;

                    setTimeout(() => {
                        $('#wrkAdminPanel').DataTable({
                            "scrollY": "500px",
                            "scrollCollapse": true,
                            "paging": false,
                            "columns": [
                                { "width": "3%" },
                                { "width": "5%" },
                                { "width": "9%" },
                                { "width": "9%" },
                                { "width": "18%" },
                                { "width": "9%" },
                                { "width": "15%" },
                                { "width": "7%" },
                                { "width": "12%" }
                            ]
                        });
                    }, 100);

                    console.log(" datata ", this.workOrders);

                },
                (error) => {
                    console.error(error);
                }
            );

        },

        AssignInfo: function (wrk) {
            this.wrk = wrk;
            this.projectYear = wrk.projectYear;
            this.projectNo = wrk.projectNo;
            this.consProject = wrk.projectName;

            this.wrkNo = wrk.workOrderNo;
            this.wrkStartDate = wrk.startDateStr;
            this.wrkEndDate = wrk.endDateStr;
            this.wrkBudgetNo = wrk.wrkBudgetNo;
            this.wrkConsWork = wrk.consecutiveWork;
            this.bhours = wrk.budgetHour;
            this.ahours = wrk.spentHour;
            this.sbudget = wrk.originalBudget;
            this.abudget = wrk.approvedBudget;
            this.clientName = wrk.clientName;
            this.companyName = wrk.companyName;
            this.description = wrk.otDescription;
            this.paidAmt = wrk.approvedBudget - wrk.balance;
            this.wrkStatusStr = wrk.wrkStatusStr;
        },

        ShowAllInvoice: function (wrk) {
            this.AssignInfo(wrk);
            this.allInv = true;
            this.allHrs = false;

        },

        ShowAllHRS: function (wrk) {
            this.AssignInfo(wrk);
            this.allInv = false;
            this.allHrs = true;
            this.LoadHoursLogSummery(wrk.id);

        },

        LoadHoursLogSummery: function (wrkId) {
            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const wrkURL = base_url + "/api/Employee/workorder/emp-hour-summery?wrkId=" + wrkId;

            axios.get(wrkURL, config).then(
                (result) => {
                    this.assignEmp = result.data;
                },
                (error) => {
                    console.error(error);
                });

        },

        LoadHoursLogDetails: function (wrkId) {
            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const wrkURL = base_url + "/api/Employee/workorder/emp-hour-details?wrkId=" + wrkId;

            axios.get(wrkURL, config).then(
                (result) => {

                    $("#empHrsDetails").dataTable().fnDestroy();

                    setTimeout(() => {
                        this.empHrsDetails = result.data;
                    }, 100);

                    setTimeout(() => {
                        $("#empHrsDetails").DataTable({
                            scrollY: "500px",
                            scrollCollapse: true,
                            paging: false,
                        });
                    }, 500);



                    //empHrsDetails
                },
                (error) => {
                    console.error(error);
                });

        },
        EmpHoursLogDetails: function (empid) {

            this.isHrsDetails = true;
            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const wrkURL = base_url + "/api/Employee/emp-ot-hour?empId=" + empid + "&wrkId=" + this.wrk.id;

            axios.get(wrkURL, config).then(
                (result) => {

                    console.log(" emp hrs ", this.empHrs);

                    this.empHrs = result.data;
                },
                (error) => {
                    console.error(error);
                });

        },

        empTotalLog: function (hrs) {
            var total_amount = 0;
            $.each(hrs, function (i, v) { total_amount += v.lhour; });
            return total_amount;
        },

        showInvoiceDetails: function (wrk) {
            var base_url = window.location.origin;
            const wrkURL = base_url + "/Dashboard/InvoiceDetails?wrkId=" + wrk.id;
            window.location = wrkURL;
        },

        wrkProgress: function (wrk) {

            if (wrk.budgetHour != 0) {
                var wrkPC =100*(wrk.spentHour / wrk.budgetHour).toFixed(4) + "%";
                return wrkPC;
            } else {
                return "0%";
            }
        },

        formatCurrency: function (Crn) {
            let dollarUS = Intl.NumberFormat("en-US", {
                style: "currency",
                currency: "USD",
            });

            return  dollarUS.format(Crn);
            
        }
  }


});