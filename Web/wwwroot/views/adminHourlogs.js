
const app = new Vue({
    el: '#hourLog',
    beforeMount() {

        wrkIdSelect = "0";

       // this.LoadAllHourLogs();
        this.LoadActiveWorkOrder();
        this.loadAllEmpoyee();
        this.Select2Setup();
    },
    data: {
        errors: [],
        logs: [],
        workOrders: [],
        wrkId: '0',
        empId:'0',
        spentHour: '',
        comments: '',
        hourLogDate: '',
        hrsEmp: [],
        isSeenEmp: false
    },
    methods: {

        SubmitHourLogs: function () {
            this.errors = [];

            if (this.isValidFrom()) {
                const config = { headers: { 'Content-Type': 'application/json' } };
                var base_url = window.location.origin;
                var hrsUrl = base_url + "/api/company/admin/save-hour-log";

                const hrsData =
                {
                    workOrderId: this.wrkId,
                    empId: this.empId,
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
                        this.LoadHourLogForEmpWrk();
                        this.clearAll();


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
            const clientURL = base_url + "/api/WorkOrder/load-work-orders/active";

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

        LoadHourLogForEmpWrk: function () {

            if (this.empId !=0  && this.wrkId !=0) {
                const config = { headers: { 'Content-Type': 'application/json' } };
                var base_url = window.location.origin;
                const clientURL = base_url + "/api/Company/admin-wrk-hour-log?wrkId=" + this.wrkId + "&empId=" + this.empId;

                axios.get(clientURL, config).then(result => {
                    console.log(result.data)
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
            }

        

        },
        loadAllEmpoyee: function () {
            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/Employee/load-hourlogs-emp";

            axios.get(clientURL, config).then(
                (result) => {
                    this.hrsEmp = result.data;
                },
                (error) => {
                    console.error(error);
                }
            );
        },

        Select2Setup: function () {
            setTimeout(() => {
                $("#wrkOrderId").select2({}).on('change', function (e) {
                    var id = $("#wrkOrderId option:selected").val();
                    app.wrkId = id;
                    app.LoadHourLogForEmpWrk();
                });


                $("#employeeId").select2({}).on('change', function (e) {
                    var id = $("#employeeId option:selected").val();
                    app.empId = id;
                    app.LoadHourLogForEmpWrk();
                });
                

            }, 100);
        },
        isValidFrom: function () {
            this.errors = [];

          
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




