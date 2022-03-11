
const app = new Vue({
    el: '#projectAct',
    beforeMount() {
        this.LoadActiveWrkBudget();

    },
    data: {
        errors: [],
        appStatus: 0,
        isApproved: true,
        appBudget: null,
        comment: null,
        bLabel:"Approved Budget",
        seen: false,
        appComment:null,
        wrkBudgets: [],
        selectedWrk: null,
        companies: []

    },
    methods: {

        submitForm: function (e) {

            
        },
        
        LoadActiveWrkBudget: function () {
            var PmName = "NA";
            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/project/project-activities/budget-load?PmName=" + PmName;

            axios.get(clientURL, config).then(result => {

                this.wrkBudgets = result.data;

                console.log("  this.wrkBudgets ", this.wrkBudgets);


                setTimeout(() => {
                    $('#allClients').DataTable({
                        "scrollY": "500px",
                        "scrollCollapse": true,
                        "paging": false
                    });
                }, 100);



            }, error => {
                console.error(error);
            });

        },
        isValidFrom: function () {
            this.errors = [];

            if (!this.errors.length) {
                return true;
            }
        },

        handleUserInput: function (e) {
            var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
            this.phone = '(' + x[1] + ') ' + x[2] + '-' + x[3];
        },
        clearAll: function () {
            this.name = "";
            this.email = "";
            this.phone = "";
            this.address = "";
            this.contactName = "";
        },
        validateEmail(email) {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email)) {
                return true;
            } else {
                return false;
            }
        },
        loadAllClients: function () {

         

        },
        approvalPop: function (wrk) {
            console.log(" work worder ", wrk);
            this.selectedWrk = wrk;



            if (wrk.approvalStatus == 0) {
                $("#exampleModal").modal("show");

            } else {
                var msg = wrk.approvalStatus  == 1 ? "Already the budget is approved" : "Already the budget is refused";

                Swal.fire({
                    position: 'top-end',
                    title: 'Info!',
                    text: msg,
                    icon: 'info',
                    showConfirmButton: false,
                    timer: 2000
                });
            }
        },
        onChangeStatus: function (status) {
            console.log(" status ", status);
            this.bLabel = status == 1 ? " Approved Budget" : "Change Budget";
            if (status == 2) {
                this.isApproved = false;
            } else {
                this.isApproved = true;
            }
            
        },
        approvalSubmit: function () {

            console.log(" value ", this.appStatus);

           // console.log(" this budget ", this.selectedWrk);

        },
        isNumber: function (evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46) {
                evt.preventDefault();;
            } else {
                return true;
            }
        },
        formatNumber: function (e) {
            let dollarUS = Intl.NumberFormat("en-US", {
                style: "currency",
                currency: "USD",
            });


            this.appBudget = dollarUS.format(this.appBudget);
        },

    }
})




