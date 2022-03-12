
const app = new Vue({
    el: '#hourLog',
    beforeMount() {

        wrkIdSelect = "0";

        this.LoadAllHourLogs();
        this.LoadActiveWorkOrder();
        this.Select2Setup();
    },
    data: {
        errors: [],
        logs: [],
        workOrders: [],
        wrkId: '0',
        spentHour: '',
        comments: '',
        hourLogDate:'',
        isSeenEmp: false
    },
    methods: {

        SubmitHourLogs: function () {
            this.errors = [];

            if (this.isValidFrom()) {
                const config = { headers: { 'Content-Type': 'application/json' } };
                var base_url = window.location.origin;
                var hrsUrl = base_url + "/api/company/save-hour-log";

                const hrsData =
                {
                    workOrderId: wrkIdSelect,
                    spentHour: this.spentHour,
                    remarks: this.comments,
                    spentDate: this.hourLogDate
                };

                axios.post(hrsUrl, hrsData, config)
                    .then(response => {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'Record has been added successfully!',
                            showConfirmButton: false,
                            timer: 1500
                        })
                        this.clearAll();
                        this.LoadAllHourLogs();

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
     

        LoadActiveWorkOrder: function () {
            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/WorkOrder/load-approved-orders";

            axios.get(clientURL, config).then(
                (result) => {
                    this.workOrders = result.data;
                    console.log(" noto ", this.workOrders);
                   

                },
                (error) => {
                    console.error(error);
                }
            );
        },
       
        LoadAllHourLogs: function () {
            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/Company/load-hour-log";

            axios.get(clientURL, config).then(result => {
                console.log(result.data )
                this.logs = result.data;

                setTimeout(() => {
                    $('#empHourLog').DataTable({
                        "scrollY": "500px",
                        "scrollCollapse": true,
                        "paging": false
                    });
                }, 100);

            }, error => {
                console.error(error);
            });

        },

        Select2Setup: function () {
            setTimeout(() => {
                $("#wrkOrderId").select2({}).on('change', function (e) {
                    var id = $("#wrkOrderId option:selected").val();
                    wrkIdSelect = id;
                    console.log(" selected data ", wrkIdSelect)
                });;

            }, 100);
        },
        isValidFrom: function () {
            this.errors = [];

            if (wrkIdSelect == "0") {
                this.errors.push(" Please select the work order ");
            }

            if (!this.spentHour) {
                this.errors.push(" Please enter spent hours");
            }

            if (!this.hourLogDate) {
                this.errors.push(" Please select spent Date");
            }

            if (this.errors.length > 0) {
                return false;
            } else {
                return true;
            }
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
        clearAll: function () {
            this.workOrderId = '',
            this.spentHour = '';
            this.spentDate = '';
            this.comments = '';

        },
        validateEmail() {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email)) {
                return true;
            } else {
                return false;
            }
        },

       

    }
})




