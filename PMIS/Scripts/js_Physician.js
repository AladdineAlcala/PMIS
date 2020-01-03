
const toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 5000
});


$(document).ready(function () {

    var createPhysician = function() {

        $.ajax({
            type:'Get',
            url:physician.createphysician,
            contentType: 'application/html;charset=utf8',
            datatype: 'html',
            cache: false,
            success: function (result) {

                var modal = $('#modal-createphysician');
                modal.find('.modal-body').html(result);

                modal.modal({
                        backdrop: 'static',
                        keyboard: false
                    },
                    'show');
            },
            error: function (xhr, ajaxOptions, thrownError) {
                Swal.fire('Error adding record!', 'Please try again', 'error');
            }
        });

    };


    if ($.fn.DataTable.isDataTable('#table-physician')) {

        $('#table-physician').dataTable().fnDestroy();
        $('#table-physician').dataTable().empty();

    }

    var physicianTable = $('#table-physician').dataTable({

        "paging": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "language": {

            'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
        },

        "dom": "<'#tablePhysiciantop.row'<'col-sm-6'B><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'#tablePhysiciantop.row'<'col-sm-5'l><'col-sm-7'p>>"

        ,

        "ajax":
            {
            "url": physician.getallphysician,
            "type": "GET",
            "datatype": "json"
            }
        ,
        "columns":
            [
                { "data": "PhysId", "className": "text-center" },
                { "data": "PhysName", "className": "text-capitalize" },
                { "data": "PhysAbr", "className": "text-uppercase" },
                {
                    "data": null,
                    "className": "text-center",
                    "render": function () {

                        return '<div class="btn-group btn-group-sm">' +
                                '<a href="#" class="btn btn-warning"><i class="fas fa-eye"></i></a>' +
                                '<a href="#" class="btn btn-success"><i class="fas fa-edit"></i></a>' +
                                '<a href="#" class="btn btn-danger" id="remove-phycisian"><i class="fas fa-trash"></i></a>' +
                                '</div>'
                            ;
                    }
                }
            ]
        ,
        "columnDefs":
            [
                    { 'width': '10%', 'targets': 0 },
                    {
                        'targets': 3,
                        'searchable': false,
                        'orderable': false,
                        'width': '12%'

                    }   
                 
            ]
        ,
        
        createdRow: function (row, data, dataIndex) {
            $(row).attr('data-phyId', data.PhysId);
        }
        ,
        buttons: [
            {
                text: '<i class="fal fa-plus-square fa-md fa-fw"></i>',
                className: 'btn btn-success btn-flat btnAddPatient',
                titleAttr: 'New Doctor Profile',
                action: function () {

                    createPhysician();
                }
            }
        ]
    });



});

$(document).on('click','#btn-savePhysician',function(e) {

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm saving physician..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

        if (result.value) {

            var formUrl = $('#create-physician').attr('action');
            var form = $('[id*=create-physician]');

            console.log(form);

            $.validator.unobtrusive.parse(form);
            form.validate();


            if (form.valid()) {

                //console.log(formUrl);

                $.ajax({
                    type:'Post',
                    url: formUrl,
                    data: form.serialize(),
                    datatype: 'json',
                    cache: false,
                    success: function(data) {

                        if (data.success) {

                            $("#modal-createphysician").modal('hide');

                            toast.fire({
                                type: 'info',
                                title: 'Record Succesfully Added.'
                            });
                        }


                    },
                    error: function(xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error adding record!', 'Please try again', 'error');
                    }

                }).done(function() {

                  
                    $('#table-physician').DataTable().ajax.reload();

                });

            }
           
        }

    });


});


$(document).on('click', '#remove-phycisian', function (e) {

    var id = $(this).closest('tr').attr('data-phyId');

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Removing This Physician..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed for removing!'

    }).then((result) => {

        if (result.value) {

            $.ajax({
                type: "post",
                url:physician.removephysician,
                ajaxasync: true,
                data: {id: id },
                cache: false,
                success: function (data) {

                    if (data.success) {

                        toast.fire({
                            type: 'info',
                            title: 'Record Succesfully Deleted.'
                        });

                    }
                }

            }).done(function () {

                $('#table-physician').DataTable().ajax.reload();
            });
        }

    });
});