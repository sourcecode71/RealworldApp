const app = new Vue({
    el: '#invoiceDiv',
    beforeMount() {
        this.LoadWorkOrderById();
        this.LoadInvoiceDetails();
    },
    data: {
        errors: [],
        workOrders: [],
        wrkS: '0',
        allInv: false,
        allHrs: false,
       
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
        invDetails:[],
    },
    methods: {

        LoadInvoiceDetails: function () {

            var url_string =location.href;
            var url = new URL(url_string);
            var wrkId = url.searchParams.get("wrkId");


            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/invoice/work-order/load-invoices?wrkId=" + wrkId;

            axios.get(clientURL, config).then(
                (result) => {
                    setTimeout(() => {
                        this.invDetails = result.data;

                        console.log("  this.invDetails ", this.invDetails)

                    }, 100);

                },
                (error) => {
                    console.error(error);
                });

        },

        LoadWorkOrderById: function () {
            var url_string = location.href;
            var url = new URL(url_string);
            var wrkId = url.searchParams.get("wrkId");


            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/WorkOrder/work-orders-by-id?WrkId=" + wrkId;

            axios.get(clientURL, config).then(
                (result) => {
                    setTimeout(() => {
                        // this.invDetails = result.data;
                        this.AssignInfo(result.data);
                    }, 100);

                },
                (error) => {
                    console.error(error);
                });
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

       
        formatCurrency: function (Crn) {
            let dollarUS = Intl.NumberFormat("en-US", {
                style: "currency",
                currency: "USD",
            });
      
            return dollarUS.format(Crn);

        }
      
    }


});