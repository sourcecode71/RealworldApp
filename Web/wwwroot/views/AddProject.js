const app = new Vue({
    el: '#addProject',
    beforeMount() {
        sClientId = "0";
        this.loadAllClients();
        this.LoadEmployee();
        this.LoadAllActiveProjects();
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
        allEmp: [],
        allEng: [],
        allDrw: [],
        allProjects: [],
        wrksBYP : [],
        hideNow: false,
        seen: false

    },
    methods: {
      
    
        AddEnginer: function () {
            this.engErrors = [];
            let eng = this.engineer;

            let existEng = this.engineers.filter(v => v.id == eng.id).length;

            this.validateEngineerHours();

            if (existEng == 0 && this.engErrors.length==0) {
                let engD = {
                    id: eng.id,
                    name: eng.name,
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

            console.log(" drw ", drw);

            let existdrw = this.drawings.filter(v => v.id == drw.id).length;

            this.validateDrawingHours();

            if (existdrw == 0 && this.drwErrors.length == 0) {
                let drwMan = {
                    id: drw.id,
                    name: drw.name,
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

            this.errors = [];
            
          
            this.validateProject();

            if (this.errors.length == 0)
            {

                const config = { headers: { 'Content-Type': 'application/json' } };
                var base_url = window.location.origin;
                const clientURL = base_url + "/api/project/create-new-project";

                let project = {
                    name: this.name,
                    client: sClientId,
                    budget: (this.budget.substring(1)).replace(",",''),
                    week: this.pweek,
                    engineers: this.engineers,
                    drawings: this.drawings,
                    description: this.workorderDesc
                }

                axios.post(clientURL, project, config)
                    .then(response => {
                        Swal.fire({
                            position: 'top-end',
                            icon: 'success',
                            title: 'Record has been added successfully!',
                            showConfirmButton: false,
                            timer: 1500
                        })
                        this.clearAll();
                        this.LoadAllActiveProjects();
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

       /************ Loading Work ****************/

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
        LoadEmployee: function () {
            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/Employee/all-active-employee";

            axios.get(clientURL, config).then(result => {
                this.allProjects = result.data;
                this.allEng = result.data.filter(p => p.role == "Engineering");
                this.allDrw = result.data.filter(p => p.role == "Drawing");
            }, error => {
                console.error(error);
            });

        },

        LoadAllActiveProjects: function () {
            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/project/load-active-projects";

            axios.get(clientURL, config).then(result => {

                $("#allProject").dataTable().fnDestroy();

                setTimeout(() => {
                    this.allProjects = result.data;

                }, 100);

                setTimeout(() => {
                    $("#allProject").DataTable({
                        scrollY: "500px",
                        scrollCollapse: true,
                        paging: false,
                    });
                }, 100);
              
            }, error => {
                console.error(error);
            });
        },

        showWorkOrderByProject: function (prj) {


            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const wrkURL = base_url + "/api/WorkOrder/work-orders/by-project?PrId=" + prj.id;

            axios.get(wrkURL, config).then(result => {
                this.wrksBYP = result.data;
                $("#allWorkOrder").modal("show");

            }, error => {
                console.error(error);
            });



        },

        /************ clerical  Work ****************/

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


            var dblBudget = this.budget.replace(",", "");
            this.budget = this.budget.charAt(0) == "$" ? dollarUS.format(dblBudget.substring(1)) : dollarUS.format(dblBudget);
        },
        validateEngineerHours: function () {

         /*   if (!this.engineer || this.engineers.id=="00") {
                this.engErrors.push("Please select the engineer");
            }
            if (!this.peHours) {
                this.engErrors.push("Please add the budgeted hours");
            } */
        },
        validateDrawingHours: function () {
          /*  if (!this.drawing || this.drawing.id == "01") {
                this.drwErrors.push("Please select the drawing man");
            }
            if (!this.drawhours) {
                this.drwErrors.push("Please add the budgeted hours");
            } */
        },
        validateProject: function () {
            if (!this.name) {
                this.errors.push("Project name is required");
            }

            if (sClientId == "0") {
                this.errors.push("Client name is required");
            }

            if (!this.budget) {
                this.errors.push("Project budget is required");
            }


            if (!this.pweek) {
                this.errors.push("Project budgeted time is required");
            }

            /*if (this.engineers.length ==0) {
                this.errors.push("Project engineer is required");
            }

            if (this.drawings.length == 0) {
                this.errors.push("Project drwaing man required");
            } */
        },
        clearAll: function () {
            this.errors = [];
            this.name = '';
            this.budget = '';
            this.pweek = '';
            this.client='0';
            this.workorderDesc = '';
            this.engineers=[],
            this.engineer = '0';
            this.peHours= '';
            this.drawing = '0';
            this.drawings=[]
        },
        addNewDivVisibility() {
            this.seen = !this.seen;

            if (this.seen)
                setTimeout(() => {
                    $("#pClient").select2({

                    }).on('change', function (e) {
                        var id = $("#pClient option:selected").val();
                        sClientId = id;
                        console.log(" selected client id ", sClientId)
                    });

                }, 200);
        },
    },

   

});