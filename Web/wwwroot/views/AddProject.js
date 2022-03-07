const app = new Vue({
    el: '#addProject',
    beforeMount() {
        this.loadAllClients();
    },
    data: {
        errors: [],
        name: '',
        budget: '',
        pweek: '',
        client:'0',
        workorderDesc: '',
        engineer: '0',
        peHours:'',
        clients: [],
        engineers: [],
        engErrors: [],
        drawing: '0',
        drawings: [],
        drawhours: '',
        drwErrors: [],

    },
    methods: {
      
        loadAllClients: function () {

            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/company/all-clients";

            axios.get(clientURL, config).then(result => {
                this.clients = result.data;
            }, error => {
                console.error(error);
            });

        },
        AddEnginer: function () {
            this.engErrors = [];
            let eng = this.engineer;

            let existEng = this.engineers.filter(v => v.id == eng.id).length;

            this.validateEngineerHours();

            if (existEng == 0 && this.engErrors.length==0) {
                let engD = {
                    id: eng.id,
                    name: eng.contactName,
                    email: eng.email,
                    hour: this.peHours
                }

                this.engineers.push(engD);
                this.peHours = "";
                this.engineer = "00";
            } else if (existEng > 0) {
                Swal.fire({
                    position: 'top-end',
                    title: 'Error!',
                    text: 'Already added the engineer!.',
                    icon: 'error',
                    confirmButtonText: 'Ok',
                })
            }
         

        },
        RemoverEngineer: function (id) {
            this.engineers = this.engineers.filter(function (el) { return el.id != id; });
        },

        AddDrawing: function () {

            this.drwErrors = [];
            let drw = this.drawing;

            let existdrw = this.drawings.filter(v => v.id == drw.id).length;

            this.validateDrawingHours();

            if (existdrw == 0 && this.drwErrors.length == 0) {
                let drwMan = {
                    id: drw.id,
                    name: drw.contactName,
                    email: drw.email,
                    hour: this.drawhours
                }

                this.drawings.push(drwMan);
                this.drawhours = "";
                this.drawing = "00";
            } else if (existdrw > 0) {
                Swal.fire({
                    position: 'top-end',
                    title: 'Error!',
                    text: 'Already added the drawing man!.',
                    icon: 'error',
                    confirmButtonText: 'Ok',
                })
            }

        },
        RemoveDrwaing: function (id) {
            this.drawings = this.drawings.filter(function (el) { return el.id != id; });
        },
        SubmitProject: function () {

            
            let clId = this.client.id;

            let project = {
                name: this.name,
                client: clId,
                budegt: this.budget,
                week: this.pweek,
                engineer: this.engineers,
                drawings: this.drawings,
                remarks: this.remarks
            }
            this.validateProject();

            if (this.errors.length == 0)
            {

                const config = { headers: { 'Content-Type': 'application/json' } };
                var base_url = window.location.origin;
                const clientURL = base_url + "/api/project/createProject";

                const clientData =
                {
                    name: this.name,
                    address: this.address,
                    contactName: this.contactName,
                    email: this.email,
                    phone: this.phone
                };

                console.log(" kaka  --- ", clientData);

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
        validateEngineerHours: function () {

            if (!this.engineer || this.engineers.id=="00") {
                this.engErrors.push("Please select the engineer");
            }
            if (!this.peHours) {
                this.engErrors.push("Please add the budgeted hours");
            }
        },

        validateDrawingHours: function () {
            if (!this.drawing || this.drawing.id == "01") {
                this.drwErrors.push("Please select the drawing man");
            }
            if (!this.drawhours) {
                this.drwErrors.push("Please add the budgeted hours");
            }
        },

        validateProject: function () {
            if (!this.name) {
                this.errors.push("Project name is required");
            }

            if (!this.client) {
                this.errors.push("Client name is required");
            }

            if (!this.budget) {
                this.errors.push("Project budget is required");
            }


            if (!this.pweek) {
                this.errors.push("Project budgeted time is required");
            }

            if (this.engineers.length ==0) {
                this.errors.push("Project engineer is required");
            }

            if (this.drawings.length == 0) {
                this.errors.push("Project drwaing man required");
            }
        }
    },

   

});