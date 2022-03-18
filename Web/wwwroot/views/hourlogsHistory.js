
const app = new Vue({
    el: '#hourLog',
    beforeMount() {

        wrkIdSelect = "0";

        this.LoadAllHourLogsSummery();
        this.Select2Setup();
    },
    data: {
        errors: [],
        workOrders: [],
        wrkId: '0',
        empId:'0',
        spentHour: '',
        comments: '',
        hourLogDate: '',
        hrsEmp: [],
        empHrsSummery: [],
        empHrs: [],
        isSeenEmp: false
    },
    methods: {

        SubmitHourLogs: function () {
            this.errors = [];

           
            
        },
       
       
        LoadAllHourLogsSummery: function () {
            const config = { headers: { 'Content-Type': 'application/json' } };
            var base_url = window.location.origin;
            const clientURL = base_url + "/api/Employee/all-hour-log/summery";

            axios.get(clientURL, config).then(result => {

                $("#empHourLogSummery").dataTable().fnDestroy();

                console.log(" hours --- logo ", result.data)

                this.empHrsSummery = result.data;

                setTimeout(() => {
                    $('#empHourLogSummery').DataTable({
                        "scrollY": "500px",
                        "scrollCollapse": true,
                        "paging": false,
                        "columns": [
                            { "width": "2%" },
                            { "width": "15%" },
                            { "width": "7%" },
                            { "width": "5%" },
                            { "width": "15%" },
                            { "width": "8%" },
                            { "width": "8%" },
                            { "width": "12%" }
                        ],
                        "footerCallback": function (row, data, start, end, display) {
                            var api = this.api();
                            var intVal = function (i) {
                                return typeof i === 'string' ?
                                    i.replace(/[\$,]/g, '') * 1 :
                                    typeof i === 'number' ?
                                        i : 0;
                            };

                            total = api
                                .column(6)
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                }, 0);

                            pageTotal = api
                                .column(6, { page: 'current' })
                                .data()
                                .reduce(function (a, b) {
                                    return intVal(a) + intVal(b);
                                }, 0);

                            $(api.column(6).footer()).html(
                                '   ' + pageTotal + ''
                            );
                        }
                    });
                }, 100);

            }, error => {
                console.error(error);
            });

        },

        ShowAllHRS: function (logs) {
            this.LoadEmpWiseHrs(logs.empId);


        },

        LoadEmpWiseHrs: function (empid) {
            const config = { headers: { "Content-Type": "application/json" } };
            var base_url = window.location.origin;
            const wrkURL = base_url + "/api/Employee/emp-wise-hour?empId=" + empid;

            axios.get(wrkURL, config).then(
                (result) => {
                    $("#empHourLog").dataTable().fnDestroy();
                    setTimeout(() => {
                        this.empHrs = result.data;

                    }, 100);
                    setTimeout(() => {
                        $('#empHourLog').DataTable({
                            "scrollY": "230px",
                            "scrollCollapse": true,
                            "paging": false,
                            searching: false,
                        });
                    }, 500);
                  

                },
                (error) => {
                    console.error(error);
                });
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
        }
    
       

    }
})




