
const app = new Vue({
    el: '#addEmploye',
    beforeMount() {
        this.loadAllRoles();
    },
    data: {
        errors: [],
        firstName: null,
        lastName: null,
        email: null,
        phone: null,
        password: null,
        confirmPassword:'',
        role: '0',
        roles: []
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
                    firstName: this.firstName,
                    lastName: this.lastName,
                    email: this.email,
                    phone: this.phone,
                    password: this.password,
                    role: this.role
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

            if (!this.firstName) {
                this.errors.push("First name is required.");
            }

            if (!this.firstName) {
                this.errors.push("Last name is required.");
            }

            if (!this.email) {
                this.errors.push('Email is required.');
            } else if (!this.validateEmail(this.email)) {
                this.errors.push('Valid email is required.');
            }

            if (!this.phone) {
                this.errors.push("Phone number is required.");
            }

            if (!this.password) {
                this.errors.push("Password is required.");
            }

            if (!this.confirmPassword) {
                this.errors.push("Confirm password is required.");
            }

            if (this.password != this.confirmPassword) {
                this.errors.push("Confirm password is not matching.");
            }
            

            if (!this.errors.length) {
                return true;
            }
        },

        handlePhoneInput: function (e) {
            var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
            this.phone = '(' + x[1] + ') ' + x[2] + '-' + x[3];
        },
        clearAll: function () {
            this.firstName = "";
            this.lastName = "";
            this.email = "";
            this.phone = "";
            this.password = "";
            this.confirmPassword = "";
            this.role = "0";
        },
        validateEmail() {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email)) {
                return true;
            } else {
                return false;
            }
        },

    loadAllRoles: function () {
            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/employee/all-role";

            axios.get(clientURL, config).then(result => {
                console.log(result.data);
                this.roles = result.data;
            }, error => {
                console.error(error);
            });

        }

    }
})




