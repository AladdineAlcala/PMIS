$(document).ready(function() {
    
    if ($.fn.DataTable.isDataTable('#table-schedules')) {

        $('#table-schedules').dataTable().fnDestroy();
        $('#table-schedules').dataTable().empty();

    }

    var tableschedules = $('#table-schedules').DataTable({
        
                            "paging": true,
                            "searching": false,
                            "ordering": false,
                            "info": false,
                            
                            "columnDefs":
                            [
                                { 'width': '5%', 'targets': 0 },
                             
                                {
                                    'targets': 4,
                                    'searchable': false,
                                    'orderable': false,
                                    'width': '12%'
             
                                }
                            ]
                            

                            });
});