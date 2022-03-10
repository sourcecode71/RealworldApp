
const app = new Vue({
    el: '#hourLog',
    beforeMount() {
        this.LoadAllHourLogs();
    },
    data: {
        errors: [],
        logs:[],
        isSeenEmp: false
    },
    methods: {

        submitForm: function (e) {
            this.errors = [];

            if (this.isValidFrom()) {
                const config = { headers: { 'Content-Type': 'application/json' } };
                var base_url = window.location.origin;
                const clientURL = base_url + "/api/employee/register";

                const clientData =
                {
                
                };

                axios.post(clientURL, clientData, config)
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
        isValidFrom: function () {
            this.errors = [];
        },

        handlePhoneInput: function (e) {
            var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
            this.phone = '(' + x[1] + ') ' + x[2] + '-' + x[3];
        },
        clearAll: function () {
           
        },
        validateEmail() {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email)) {
                return true;
            } else {
                return false;
            }
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

        }

       

    }
})




