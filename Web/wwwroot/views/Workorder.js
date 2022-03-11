
const app = new Vue({
    el: '#workOrder',
    mounted() {
        $("#wrkProject").select2();
    },
    beforeMount() {
        this.loadAllProject();
        this.loadAllcompany();
        this.loadAllWorkOrder();
    },
    data: {
        errors: [],
        name :null,
        consecutiveWork: null,
        budget:'',
        startDate: '',
        endDate:'',
        project:'0',
        description: null,
        comapny:'0',
        seen: false,
        isName: false,
        projects: [],
        workOrders: [],
        companies: []

    },
    methods: {

        showHide: function () {

            this.seen = !this.seen;

            if (this.seen) {
                setTimeout(() => {
                    $("#wrkCompany").select2();
                    $("#wrkProject").select2();
                }, 150);
            }
        },
        SubmitWorkOrder: function (e) {

            console.log($("#wrkCompany").select2("val"), " eee ---- ", $("#wrkProject").select2('val'));

            this.project = $("#wrkProject").select2("val");
            this.comapny = $("#wrkCompany").select2("val");

            if (this.isValidFrom()) {

                const config = { headers: { 'Content-Type': 'application/json' } };
                var base_url = window.location.origin;
                const clientURL = base_url + "/api/workOrder/store-work-order";

                const wrkData =
                {
                    name: this.name,
                    consecutiveWork: this.consecutiveWork,
                    originalBudget: (this.budget.substring(1)).replace(",",""),
                    startDate: this.startDate,
                    endDate: this.endDate,
                    projectId: this.project ,
                    companyId: this.comapny,
                    oTDescription: this.description
                };

                axios.post(clientURL, wrkData, config)
                    .then(response => {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'Record has been added successfully!',
                            showConfirmButton: false,
                            timer: 1500
                        })
                        this.clearAll();
                        this.loadAllWorkOrder();
                    }).catch(errors => {
                        Swal.fire({
                            position: 'top-end',
                            title: 'Error!',
                            text: 'Something went wrong.' + errorThrown.errorMessage,
                            icon: 'error',
                            confirmButtonText: 'Ok',
                        })
                    });
            }
        },
        isValidFrom: function () {
            this.errors = [];

            if ($("#wrkProject").select2("val") == "0") {
                this.errors.push("Project is required.");
            }

            if ($("#wrkCompany").select2("val") =="0") {
                this.errors.push("Company is required.");
            }
           

            if (!this.consecutiveWork) {
                this.errors.push("Contact name is required.");
            }

            if (!this.budget) {
                this.errors.push("Budget is required.");
            }

            if (!this.startDate) {
                this.errors.push('Start date is required.');
            }

            if (!this.endDate) {
                this.errors.push('End date is required.');
            }

            if (!this.errors.length) {
                return true;
            }
        },

        handleUserInput: function (e) {
            var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
            this.phone = '(' + x[1] + ') ' + x[2] + '-' + x[3];
        },
        clearAll: function () {
            this.project = "0";
            this.comapny = "0";
            this.startDate = "";
            this.consecutiveWork = "";
            this.description = "";
            this.budget =""
        },
        validateEmail(email) {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email)) {
                return true;
            } else {
                return false;
            }
        },
        loadAllProject: function () {

            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/project/load-active-projects";

            axios.get(clientURL, config).then(result => {
                this.projects = result.data;
            }, error => {
                console.error(error);
            });

        },
        loadAllcompany: function () {

            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/company/all-companies";

            axios.get(clientURL, config).then(result => {
                this.companies = result.data;
               
            }, error => {
                console.error(error);
            });

        },
        loadAllWorkOrder: function () {
            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/WorkOrder/load-approved-orders";

            axios.get(clientURL, config).then(result => {
                this.workOrders = result.data;
                
                setTimeout(() => {
                    $('#allWorkOrder').DataTable({
                        "scrollY": "500px",
                        "scrollCollapse": true,
                        "paging": false
                    });
                }, 100);

             

            }, error => {
                console.error(error);
            });
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

            this.budget = dollarUS.format(this.budget);
        },
        showCompanyByClient: function (client) {

            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/company/all-companies-client?guid=" + client.id;

            $("#allCompany").modal("show");

            axios.get(clientURL, config).then(result => {

                console.log(result.data);
                this.companies = result.data;
            }, error => {
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
})





