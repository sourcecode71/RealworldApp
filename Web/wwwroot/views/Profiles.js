const app = new Vue({
    el: '#profile',
    beforeMount() {
        this.loadMyProfile();
    },
    data: {
        errors: [],
        currentPassword: '',
        newPassword: '',
        confirmPassword: '',
        role: '',
        profile:''
    },
    methods: {
        loadMyProfile: function () {
            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/profile/my-profile";

            axios.get(clientURL, config).then(
                (result) => {
                    this.profile = result.data;

                    console.log(" profile ", this.profile);
                 
                },
                (error) => {
                    console.error(error);
                }
            );

        },
        submitChangePassword: function () {
            this.errors = [];
            if (this.isValidInvFrom()) {
                const config = { headers: { "Content-Type": "application/json" } };
                var base_url = window.location.origin;
                const clientURL = base_url + "/api/profile/change-password";

                const profile = {
                    password: this.currentPassword,
                    confirmPassword: this.confirmPassword
                };

                axios
                    .Put(clientURL, profile, config)
                    .then((response) => {

                        if (response.data == "Success") {
                            Swal.fire({
                                position: "top-end",
                                icon: "success",
                                title: "Record has been updated successfully!",
                                showConfirmButton: false,
                                timer: 1500,
                            });

                            this.clearAll();
                            this.loadAllInvoice();
                        }
                        else {
                            Swal.fire({
                                position: "top-end",
                                title: "Error!",
                                text: "Fail to update.",
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
        isNumber: function (evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46) {
                evt.preventDefault();;
            } else {
                return true;
            }
        },

        Select2Setup: function () {
            setTimeout(() => {
                $("#employeeId").select2({}).on('change', function (e) {
                    var id = $("#employeeId option:selected").val();
                    app.empId = id;
                });


            }, 100);
        },

        isValidInvFrom: function () {

            if (!this.currentPassword) {
                this.errors.push("Current password is required.");
            }

            if (!this.newPassword) {
                this.errors.push("New password is required.");
            }

            if (!this.confirmPassword) {
                this.errors.push("Confirm password is required.");
            }

            if (this.newPassword != this.confirmPassword) {
                this.errors.push("Confirm password is not matching.");
            }

            if (!this.errors.length) {
                return true;
            }
        }


  }


});