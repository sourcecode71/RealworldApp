const app = new Vue({
    el: '#invPay',
    beforeMount() {
        selectedWrkId = "0";
        this.loadAllWorkOrder();
        this.loadAllInvoice();
       
    },
    data: {
        errors: [],
        workOrders: [],
        wrkS: '0',
        isWrkVisiblity: false,
        seen: false,
        seenInvDetails: false,
        partialBill: '',
        invoicebill: '',
        invoiceNumber : '',
        invoiceDate: '',
        remark: '',
        allInv: [],
        filterWrk: [],
        inv:''

    },
    methods: {

        submitInvoice: function () {
            this.errors = [];
            if (this.isValidInvFrom()) {
                const config = { headers: { "Content-Type": "application/json" } };
                var base_url = window.location.origin;
                const clientURL = base_url + "/api/invoice/save-invoice";

                const wrkData = {
                    workOrderId: selectedWrkId,
                   // partialBill: this.partialBill.substring(1).replace(",", ""),
                    partialBill: this.invoicebill.substring(1).replace(",", ""),
                    invoiceBill: this.invoicebill.substring(1).replace(",", ""),
                    invoiceNumber: this.invoiceNumber,
                    invoiceDate: this.invoiceDate,
                    remarks: this.remark
                };

                axios
                    .post(clientURL, wrkData, config)
                    .then((response) => {

                        console.log(" response ", response.data);

                        if (response.data == "Success") {
                            Swal.fire({
                                position: "top-end",
                                icon: "success",
                                title: "Record has been added successfully!",
                                showConfirmButton: false,
                                timer: 1500,
                            });

                            this.clearAll();
                            this.loadAllInvoice();
                        } else {
                            Swal.fire({
                                position: "top-end",
                                title: "Error!",
                                text: "Still the budget is not approved/waitting for approval.",
                                icon: "error",
                                confirmButtonText: "Ok",
                            });
                        }
                        

                    })
                    .catch((errors) => {

                        Swal.fire({
                            position: "top-end",
                            title: "Error!",
                            text: "Something went wrong." + errors.errorMessage,
                            icon: "error",
                            confirmButtonText: "Ok",
                        });


                    });
            }

        },
        loadAllInvoice: function () {
            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/invoice/load-all-invoice";

            axios.get(clientURL, config).then(
                (result) => {

                    $("#invAllLogs").dataTable().fnDestroy();

                    this.allInv = result.data;
                    setTimeout(() => {
                        $('#invAllLogs').DataTable({
                            "scrollY": "500px",
                            "scrollCollapse": true,
                            "paging": false,
                            columns: [
                                { "width": "3%" },
                                { "width": "9%" },
                                { "width": "25%" },
                                { "width": "10%" },
                                { "width": "10%" },
                                { "width": "9%" },
                                { "width": "8%" },
                                { "width": "12%" },
                            ]
                        });
                    }, 100);
                },
                (error) => {
                    console.error(error);
                }
            );
        },
        loadAllWorkOrder: function () {

            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/WorkOrder/load-approved-orders";

            axios.get(clientURL, config).then(
                (result) => {
                    

                    window.sessionStorage.setItem("wrk", JSON.stringify(result.data));
                    this.workOrders = result.data;
                    this.setupControl()

                },
                (error) => {
                    console.error(error);
                }
            );

        },

        showInvoiceDetails: function (inv) {
            this.seenInvDetails = true;
            this.inv = inv;
            console.log(" invoice details ", inv);

        },


        isValidInvFrom: function () {
            if (selectedWrkId == "0") {
                this.errors.push("Please select a work order");
            }

            //if (!this.partialBill) {
            //    this.errors.push("Partial bill is reequired");
            //}

            if (!this.invoicebill) {
                this.errors.push("Invoice bill is reequired");
            }

            if (!this.invoiceNumber) {
                this.errors.push("Invoice number is reequired");
            }

            if (!this.invoiceDate) {
                this.errors.push("Invoice Date is reequired");
            }

            if (!this.errors.length) {
                return true;
            } else {
                return false;
            }

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
                    selectedWrkId = id;

                    var wrkOD = JSON.parse(window.sessionStorage.getItem("wrk"));
                    this.filterWrk = wrkOD.filter(p => p.id == id);
                    this.isWrkVisiblity = this.filterWrk.length > 0;
                });
        },

        wrkProgress: function (wrk) {

            console.log(wrk, " wrk --- ", wrk.budgetHour);

            if (wrk.budgetHour != 0) {
                var wrkPC = (wrk.spentHour / wrk.budgetHour).toFixed(2) + "%";
                return wrkPC;
            } else {
                return "0%";
            }
        },

        clearAll: function () {
            this.wrkS = '0';
            this.seenInvDetails = false;
            this.partialBill = '';
            this.invoicebill = '';
            this.invoiceNumber = '';
            this.invoiceDate = '';
        },
        showHide: function () {
            this.seen = !this.seen;
            this.seenInvDetails = false;
            if (this.seen) {
                setTimeout(() => {
                    app.setupControl();
                }, 150);
            }
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