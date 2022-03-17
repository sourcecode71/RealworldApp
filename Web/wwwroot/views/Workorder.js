const app = new Vue({
  el: "#workOrder",
  mounted() {},
    beforeMount() {
    comapnyId = "0";
    projectId = "0";
    this.loadAllProject();
    this.loadAllcompany();
    this.loadAllWorkOrder();
    this.LoadEmployee();
    
  },
  data: {
      errors: [],
      wrk:'',
    name: null,
    consecutiveWork: null,
    budget: "",
    startDate: "",
    endDate: "",
    project: "0",
    description: null,
    comapny: "0",
    seen: false,
    isName: false,
    showDetails: false,
      hrsDetails: false,
      IsInv: true,
    projects: [],
    workOrders: [],
    companies: [],
    engineers: [],
    engErrors: [],
    peHours: "",
    drawing: "0",
    engineer: "0",
    drawings: [],
    drawhours: "",
    drwErrors: [],
    allEmp: [],
    allEng: [],
    allDrw: [],
    projectYear: "",
    projectNo: "",
    consProject: "",
      wrkNo: '',
    wrkBudgetNo:'',
    wrkStartDate: '',
    wrkEndDate: '',
    wrkConsWork: '',
    bhours: '',
    ahours: '',
    sbudget: '',
      abudget: '',
      clientName: '',
      companyName: '',
      description: '',
      pageName: 'Work Order',
      invDetails: [],
      assignEmp: [],
      empHrsDetails :[]
  },
  methods: {

      SubmitWorkOrder: function (e) {
      this.errors = [];
      if (this.isValidFrom()) {
        const config = { headers: { "Content-Type": "application/json" } };
        var base_url = window.location.origin;
        const clientURL = base_url + "/api/workOrder/store-work-order";

        const wrkData = {
          name: this.name,
          consecutiveWork: this.consecutiveWork,
          originalBudget: this.budget.substring(1).replace(",", ""),
          startDate: this.startDate,
          endDate: this.endDate,
          projectId: projectId,
          companyId: comapnyId,
          week: this.pweek,
          engineers: this.engineers,
          drawings: this.drawings,
          oTDescription: this.description,
        };

        axios
          .post(clientURL, wrkData, config)
          .then((response) => {
            Swal.fire({
              position: "top-end",
              icon: "success",
              title: "Record has been added successfully!",
              showConfirmButton: false,
              timer: 1500,
            });
            this.clearAll();
            this.loadAllWorkOrder();
          })
          .catch((errors) => {
            Swal.fire({
              position: "top-end",
              title: "Error!",
              text: "Something went wrong." + errorThrown.errorMessage,
              icon: "error",
              confirmButtonText: "Ok",
            });
          });
      }
    },

      loadAllProject: function () {
      const config = { headers: { "Content-Type": "application/json" } };
      var base_url = window.location.origin;
      const clientURL = base_url + "/api/project/load-active-projects";

      axios.get(clientURL, config).then(
        (result) => {
          this.projects = result.data;
        },
        (error) => {
          console.error(error);
        }
      );
    },

      loadAllcompany: function () {
      const config = { headers: { "Content-Type": "application/json" } };
      var base_url = window.location.origin;
      const clientURL = base_url + "/api/company/all-companies";

      axios.get(clientURL, config).then(
        (result) => {
          this.companies = result.data;
        },
        (error) => {
          console.error(error);
        }
      );
    },

   loadAllWorkOrder: function () {

      const config = { headers: { "Content-Type": "application/json" } };
      var base_url = window.location.origin;
      const clientURL = base_url + "/api/WorkOrder/load-approved-orders";

      axios.get(clientURL, config).then(
        (result) => {

              $("#allWorkOrder").dataTable().fnDestroy();

              setTimeout(() => {
                  this.workOrders = result.data;
              }, 100);

          setTimeout(() => {
            $("#allWorkOrder").DataTable({
              scrollY: "500px",
              scrollCollapse: true,
              paging: false,
            });
          }, 500); 

        },
        (error) => {
          console.error(error);
        }
      );
      },

      LoadEmployee: function () {
      const config = { headers: { "Content-Type": "application/json" } };
      var base_url = window.location.origin;
      const clientURL = base_url + "/api/Employee/all-active-employee";

      axios.get(clientURL, config).then(
        (result) => {
          this.allEmp = result.data;
          this.allEng = result.data.filter((p) => p.role == "Engineering");
          this.allDrw = result.data.filter((p) => p.role == "Drawing");
        },
        (error) => {
          console.error(error);
        });
      },

    AddEnginer: function () {
      this.engErrors = [];
      let eng = this.engineer;

      let existEng = this.engineers.filter((v) => v.id == eng.id).length;

      this.validateEngineerHours();

      if (existEng == 0 && this.engErrors.length == 0) {
        let engD = {
          id: eng.id,
          name: eng.name,
          email: eng.email,
          hour: this.peHours,
        };

        this.engineers.push(engD);
        this.peHours = "";
        this.engineer = "00";
      } else if (existEng > 0) {
        Swal.fire({
          position: "top-end",
          title: "Error!",
          text: "Already added the engineer!.",
          icon: "error",
          confirmButtonText: "Ok",
        });
      }
    },
    RemoverEngineer: function (id) {
      this.engineers = this.engineers.filter(function (el) {
        return el.id != id;
      });
    },

    AddDrawing: function () {
      this.drwErrors = [];
      let drw = this.drawing;

      let existdrw = this.drawings.filter((v) => v.id == drw.id).length;

      this.validateDrawingHours();

      if (existdrw == 0 && this.drwErrors.length == 0) {
        let drwMan = {
          id: drw.id,
          name: drw.name,
          email: drw.email,
          hour: this.drawhours,
        };

        this.drawings.push(drwMan);
        this.drawhours = "";
        this.drawing = "00";
      } else if (existdrw > 0) {
        Swal.fire({
          position: "top-end",
          title: "Error!",
          text: "Already added the drawing man!.",
          icon: "error",
          confirmButtonText: "Ok",
        });
      }
    },
    RemoveDrwaing: function (id) {
      this.drawings = this.drawings.filter(function (el) {
        return el.id != id;
      });
      },


      AssignInfo: function (wrk) {

          this.wrk = wrk;

          this.projectYear = wrk.projectYear;
          this.projectNo = wrk.projectNo;
          this.consProject = wrk.projectName;

          this.wrkNo = wrk.workOrderNo;
          this.wrkStartDate = wrk.startDateStr;
          this.wrkEndDate = wrk.endDateStr;
          this.wrkBudgetNo = wrk.wrkBudgetNo;
          this.wrkConsWork = wrk.consecutiveWork;
          this.bhours = wrk.budgetHour;
          this.ahours = wrk.spentHour;
          this.sbudget = wrk.originalBudget;
          this.abudget = wrk.approvedBudget;
          this.clientName = wrk.clientName;
          this.companyName = wrk.companyName;
          this.description = wrk.otDescription;
      },

     ShowWorkOrder: function (wrk) {

         this.wrk = wrk;

        this.projectYear = wrk.projectYear;
        this.projectNo = wrk.projectNo;
        this.consProject = wrk.projectName;

         this.wrkNo = wrk.workOrderNo;
         this.wrkStartDate = wrk.startDateStr;
         this.wrkEndDate = wrk.endDateStr;
         this.wrkBudgetNo = wrk.wrkBudgetNo;
         this.wrkConsWork = wrk.consecutiveWork;
         this.bhours = wrk.budgetHour;
         this.ahours = wrk.spentHour;
         this.sbudget = wrk.originalBudget;
         this.abudget = wrk.approvedBudget;
         this.clientName = wrk.clientName;
         this.companyName = wrk.companyName;
         this.description = wrk.otDescription;

          this.showDetails = true;
          this.seen = false;



      },

      ShowWorkOrderDetails: function () {
          this.hrsDetails = !this.hrsDetails;
          this.loadAllWorkOrder();
          this.pageName = "Work Order";
         

      },

      showFullBudget: function (wrk, tp) {
          this.IsInv = true;

          $("#allWorkOrder").dataTable().fnDestroy();
          this.hrsDetails = !this.hrsDetails;
          this.pageName = this.hrsDetails ? "Invoice Details" : "Work Order";

          const config = { headers: { "Content-Type": "application/json" } };
          var base_url = window.location.origin;
          const clientURL = base_url + "/api/invoice/work-order/load-invoices?wrkId=" + wrk.id;

          axios.get(clientURL, config).then(
              (result) => {

                  $("#invDetailsOrder").dataTable().fnDestroy();

                  setTimeout(() => {
                      this.invDetails = result.data;
                  }, 100);

                  setTimeout(() => {
                      $("#invDetailsOrder").DataTable({
                          scrollY: "500px",
                          scrollCollapse: true,
                          paging: false,
                      });
                  }, 500);

              },
              (error) => {
                  console.error(error);
              });


      },

      showFullHrs: function (wrk, tp) {

          this.IsInv = false;

          $("#allWorkOrder").dataTable().fnDestroy();
          $("#invDetailsOrder").dataTable().fnDestroy();
          this.hrsDetails = !this.hrsDetails;
          this.pageName = this.hrsDetails ? "HRS Details" : "Work Order";


          this.LoadHoursLogSummery(wrk.id);
          this.LoadHoursLogDetails(wrk.id);

      },

      LoadHoursLogSummery: function (wrkId) {
          const config = { headers: { "Content-Type": "application/json" } };
          var base_url = window.location.origin;
          const wrkURL = base_url + "/api/Employee/workorder/emp-hour-summery?wrkId=" + wrkId;

          axios.get(wrkURL, config).then(
              (result) => {
                  this.assignEmp = result.data;

                  console.log(" Assing emp ", this.assignEmp);
              },
              (error) => {
                  console.error(error);
              });

      },


      LoadHoursLogDetails: function (wrkId) {
          const config = { headers: { "Content-Type": "application/json" } };
          var base_url = window.location.origin;
          const wrkURL = base_url + "/api/Employee/workorder/emp-hour-details?wrkId=" + wrkId;

          axios.get(wrkURL, config).then(
              (result) => {

                  $("#empHrsDetails").dataTable().fnDestroy();

                  setTimeout(() => {
                      this.empHrsDetails = result.data;
                  }, 100);

                  setTimeout(() => {
                      $("#empHrsDetails").DataTable({
                          scrollY: "500px",
                          scrollCollapse: true,
                          paging: false,
                      });
                  }, 500);



                  //empHrsDetails
              },
              (error) => {
                  console.error(error);
              });

      },

    validateEngineerHours: function () {
      if (!this.engineer || this.engineers.id == "00") {
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
    isValidFrom: function () {
      this.errors = [];

      if (!projectId || projectId == "0") {
        this.errors.push("Project is required.");
      }

      if (!comapnyId || comapnyId == "0") {
        this.errors.push("Company is required.");
      }

      if (!this.consecutiveWork) {
        this.errors.push("Work order name is required.");
      }

      if (!this.budget) {
        this.errors.push("Budget is required.");
      }

      if (!this.startDate) {
        this.errors.push("Start date is required.");
      }

      if (!this.endDate) {
        this.errors.push("End date is required.");
        }

        if (this.engineers.length == 0 && this.peHours) {
              this.errors.push("Project add engineer's hour");
          }

        if (this.drawings.length == 0 && this.drawhours) {
              this.errors.push("Project add drwaing's hour");
          } 

      if (!this.errors.length) {
        return true;
      }
    },

    handleUserInput: function (e) {
      var x = e.target.value
        .replace(/\D/g, "")
        .match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
      this.phone = "(" + x[1] + ") " + x[2] + "-" + x[3];
    },
    clearAll: function () {
      this.project = "0";
      this.comapny = "0";
      this.startDate = "";
      this.endDate = "";
      this.consecutiveWork = "";
      this.description = "";
        this.budget = "";
        this.engineers = [],
        this.engineer = '0';
        this.peHours = '';
        this.drawing = '0';
        this.drawings = []
    },
    validateEmail(email) {
      if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(this.email)) {
        return true;
      } else {
        return false;
      }
    },

    isNumber: function (evt) {
      evt = evt ? evt : window.event;
      var charCode = evt.which ? evt.which : evt.keyCode;
      if (
        charCode > 31 &&
        (charCode < 48 || charCode > 57) &&
        charCode !== 46
      ) {
        evt.preventDefault();
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

      showHide: function () {
          this.seen = !this.seen;
          this.showDetails = false;
      if (this.seen) {
        setTimeout(() => {
          $("#wrkCompany")
            .select2()
            .on("change", function (e) {
              var id = $("#wrkCompany option:selected").val();
              comapnyId = id;
            });

          $("#wrkProject")
            .select2()
            .on("change", function (e) {
              var id = $("#wrkProject option:selected").val();
              projectId = id;
            });
        }, 150);
      }
    },
  },
});
