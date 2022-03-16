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
        projectNo: '',
        consProject: '',
        projectYear: '',
        wrkNo: '',
        wrkConsWork: '',
        wrkBudgetNo:'',
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
        clientName: '',
        assignEmp:[],
    },
    methods: {

        submitInvoice: function () {
            this.errors = [];
        },
       
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
                                { "width": "2%" },
                                { "width": "5%" },
                                { "width": "11%" },
                                { "width": "9%" },
                                { "width": "9%" },
                                { "width": "20%" },
                                { "width": "10%" },
                                { "width": "12%" },
                                { "width": "12%" },
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
        },

        ShowAllInvoice: function (wrk) {
            this.AssignInfo(wrk);
            this.allInv = true;
            this.allHrs = false;
            this.LoadHoursLogSummery(wrk.id);

        },

        ShowAllHRS: function (wrk) {
            this.AssignInfo(wrk);
            this.allInv = false;
            this.allHrs = true;

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


        isValidInvFrom: function () {
           
        },

        isNumber: function (evt) {
            evt = evt ? evt : window.event;
            var charCode = evt.which ? evt.which : evt.keyCode;
            if (
                charCode > 31 &&
                (charCode < 48 || charCode > 57) &&
                charCode !== 46
            ) {
                evt.preventDefault();
            } else {
                return true;
            }
        },
        formatNumberInvBill: function (e) {
            let dollarUS = Intl.NumberFormat("en-US", {
                style: "currency",
                currency: "USD",
            });
            var dblBudget = this.invoicebill.replace(",", "");
            this.invoicebill = this.invoicebill.charAt(0) == "$" ? dollarUS.format(dblBudget.substring(1)) : dollarUS.format(dblBudget);
        },

        formatNumberPartialBill: function (e) {
            let dollarUS = Intl.NumberFormat("en-US", {
                style: "currency",
                currency: "USD",
            });
            var dblBudget = this.partialBill.replace(",", "");
            this.partialBill = this.partialBill.charAt(0) == "$" ? dollarUS.format(dblBudget.substring(1)) : dollarUS.format(dblBudget);
            this.invoicebill = this.partialBill;
        },

        setupControl: function () {
            $("#wrkOrderId")
                .select2()
                .on("change", function (e) {
                    var id = $("#wrkOrderId option:selected").val();
                   
                });
        },

        clearAll: function () {
            this.wrkS = '0';
            this.isSelectWrk = false;
            this.partialBill = '';
            this.invoicebill = '';
            this.invoiceNumber = '';
            this.invoiceDate = '';
        }
    }


});