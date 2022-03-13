const app = new Vue({
    el: '#archiveDiv',
    beforeMount() {
        this.loadAllWorkOrder();
    },
    data: {
        errors: [],
        workOrders: [],
        wrkS: '0'
    },
    methods: {

        submitInvoice: function () {
            this.errors = [];
        },
       
        loadAllWorkOrder: function () {

            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/WorkOrder/load-work-orders/archived";

            axios.get(clientURL, config).then(
                (result) => {

                    $("#wrkArchived").dataTable().fnDestroy();

                    this.workOrders = result.data;

                    setTimeout(() => {
                        $('#wrkArchived').DataTable({
                            "scrollY": "500px",
                            "scrollCollapse": true,
                            "paging": false
                        });
                    }, 100);

                    console.log(" datata ", this.workOrders);

                },
                (error) => {
                    console.error(error);
                }
            );

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