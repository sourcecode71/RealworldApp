
  const app = new Vue({
      el: '#app',
      beforeMount() {
          this.loadAllClients();
          
    },
    data: {
            errors: [],
            name: null,
            contactName: '',
            email: null,
            phone: '',
            sndPhone:'',
            address: null,
            seen: false,
            clients: [],
            companies:[]

        },
      methods: {
       
     submitForm: function (e) {
           
       if(this.isValidFrom())
       {

        const config = {headers: {'Content-Type': 'application/json' } };
        var base_url = window.location.origin;
        const clientURL = base_url + "/api/Company/save-client";

           const clientData =
           {
            name : this.name,
            address : this.address,
            contactName:this.contactName,
               email: this.email,
               phone: this.sndPhone != null ? this.phone + "," + this.sndPhone : this.phone
           };

         axios.post(clientURL, clientData, config)
                .then(response =>{
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Record has been added successfully!',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    this.clearAll();
                    this.loadAllClients();

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

        if (!this.name) {
             this.errors.push("Name is required.");
        }

        if (!this.contactName) {
            this.errors.push("Contact name is required.");
        }

        if (!this.phone) {
          this.errors.push("Phone number is required.");
        }

       /* if (!this.email) {
        this.errors.push('Email is required.');
        } else if (!this.validateEmail(this.email)) {
        this.errors.push('Valid email is required.');
        } */

            if (!this.errors.length)
            {
                  return true;
            }
     },
        
     
        loadAllClients: function () {

                const config = { headers: { 'Content-Type': 'application/json' } };
                var base_url = window.location.origin;
                const clientURL = base_url + "/api/company/all-clients";

                axios.get(clientURL, config).then(result => {
                    $("#allClients").dataTable().fnDestroy();

                    console.log(" client --", result);

                    setTimeout(() => {
                        this.clients = result.data;
                    }, 100);
                    
                    setTimeout(() => {
                        $('#allClients').DataTable({
                            "scrollY": "750px",
                            "scrollCollapse": true,
                            "paging": false,
                            columns: [
                                { "width": "3%" },
                                { "width": "9%" },
                                { "width": "12%" },
                                { "width": "12%" },
                                { "width": "12%" },
                                { "width": "37%" },
                                { "width": "15%" }
                            ]
                        });
                    }, 500);

                }, error => {
                    console.error(error);
                });

          },
        showCompanyByClient: function (client) {

            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/company/all-companies-client?guid=" + client.id;

            $("#allCompany").modal("show");

            axios.get(clientURL, config).then(result => {
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

          },
     
      handleUserInput: function (e) {
          var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
          this.phone = '(' + x[1] + ') ' + x[2] + '-' + x[3];
          },

         
    handlesndPhoneInput: function (e) {
        var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
        this.sndPhone = '(' + x[1] + ') ' + x[2] + '-' + x[3];
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
          }
      }
  })


                    
