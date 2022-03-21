const app = new Vue({
    el: '#invPay',
    beforeMount() {
        selectedWrkId = "0";
        this.loadAllInvoice();
        this.loadPaidInvoice();
       
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
        inv: '',
        payDate: '',
        payAmount: '',
        paidAll :[],
        invPay :'',

    },
    methods: {

        submitPayment: function () {
            this.errors = [];

            if (this.isValidInvFrom()) {
                const config = { headers: { "Content-Type": "application/json" } };
                var base_url = window.location.origin;
                const clientURL = base_url + "/api/invoice/pay-bill";

                const wrkData = {
                    workOrderId: this.invPay.workOrderId,
                    payAmount: this.payAmount.substring(1).replace(",", ""),
                    invoiceId: this.invPay.id,
                    invoiceNo: this.invPay.invoiceNumber,
                    payDate: this.payDate,
                    remarks: this.remark
                };

                axios
                    .post(clientURL, wrkData, config)
                    .then((response) => {

                        if (response.data) {
                            Swal.fire({
                                position: "top-end",
                                icon: "success",
                                title: "Record has been added successfully!",
                                showConfirmButton: false,
                                timer: 1500,
                            });

                            this.clearAll();
                            this.loadAllInvoice();
                            this.loadPaidInvoice();
                            $("#invPayment").modal("hide");
                        } else {
                            Swal.fire({
                                position: "top-end",
                                title: "Error!",
                                text: "Payment fail.",
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
            const clientURL = base_url + "/api/invoice/load-pending-invoice";

            axios.get(clientURL, config).then(
                (result) => {

                    $("#invAllLogs").dataTable().fnDestroy();

                    this.allInv = result.data;
                    setTimeout(() => {
                        $('#invAllLogs').DataTable({
                            "scrollY": "300px",
                            "scrollCollapse": true,
                            "paging": false,
                            "searching": false,
                            "info": false
                        });
                    }, 100);
                },
                (error) => {
                    console.error(error);
                }
            );
        },

        loadPaidInvoice: function () {
            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/invoice/load-all-payment";

            axios.get(clientURL, config).then(
                (result) => {

                    $("#paidLogs").dataTable().fnDestroy();

                    this.paidAll = result.data;

                    console.log(" paid bill ", this.paidAll);

                    setTimeout(() => {
                        $('#paidLogs').DataTable({
                            "scrollY": "500px",
                            "scrollCollapse": true,
                            "paging": false,
                            columns: [
                                { "width": "3%" },
                                { "width": "9%" },
                                { "width": "8%" },
                                { "width": "13%" },
                                { "width": "10%" },
                                { "width": "9%" },
                                { "width": "8%" },
                                { "width": "9%" },
                                { "width": "9%" },
                            ]
                        });
                    }, 500);
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

        PayInvoiceBill: function(inv) {
            console.log(" Invoice bill ", inv);

            $("#invPayment").modal("show");
            this.invPay = inv;
        },


        isValidInvFrom: function () {
            

            if (!this.payAmount) {
                this.errors.push("Payment  bill is reequired");
            }

            if (!this.payDate) {
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

        formatPayAmount: function (e) {
            let dollarUS = Intl.NumberFormat("en-US", {
                style: "currency",
                currency: "USD",
            });
            var dblBudget = this.payAmount.replace(",", "");
            this.payAmount = this.payAmount.charAt(0) == "$" ? dollarUS.format(dblBudget.substring(1)) : dollarUS.format(dblBudget);
        },

        clearAll: function () {
            this.invPay = '';
            this.payAmount = '';
            this.payDat = '';
            this.invoiceDate = '';
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