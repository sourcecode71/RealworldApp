
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
            phone:'',
            address: null,
            seen: false,
            clients :[]

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
            email : this.email,
            phone : this.phone
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

        if (!this.email) {
        this.errors.push('Email is required.');
        } else if (!this.validateEmail(this.email)) {
        this.errors.push('Valid email is required.');
        }

            if (!this.errors.length)
            {
                  return true;
            }
     },
        
        handleUserInput: function (e)
        {
            var replacedInput = e.target.value.replace(/\D/g, '').match(/(\d{0, 3})(\d{0, 3})(\d{0, 4})/);
            this.phone = !replacedInput[2] ? replacedInput[1] : '(' + replacedInput[1] + ') ' + replacedInput[2] + (replacedInput[3] ? '-' + replacedInput[3] : '');
        },
        clearAll: function ()
        {
            this.name = "";
            this.email = "";
            this.phone ="";
            this.address ="";
            this.contactName ="";
        },
        validateEmail(email) {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email)) {
                return true;
            } else {
                return false;
            }
          },
        loadAllClients: function () {

                const config = { headers: { 'Content-Type': 'application/json' } };
                var base_url = window.location.origin;
                const clientURL = base_url + "/api/company/all-clients";

                axios.get(clientURL, config).then(result => {

                    console.log(result.data);
                    this.clients = result.data;

                    setTimeout(() => {
                        $('#example').DataTable({
                            "scrollY": "500px",
                            "scrollCollapse": true,
                            "paging": false
                        });
                    }, 100);

               
                
                }, error => {
                    console.error(error);
                });

          },
          showDetails: function (client) {
              console.log("client ", client);
              $("#myModal").modal("show");
          }

      }
  })


   
                    
