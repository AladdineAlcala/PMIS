

const toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 5000
});


$(document).ready(function () {

    var getAge = function (dob) {

        //debugger;

        var today = new Date();
        var d = moment.utc(dob).format('MM/DD/YYYY');
        var bday = new Date(d);

        var age = today.getFullYear() - bday.getFullYear();
        var mo = today.getMonth() - bday.getMonth();

        if (mo < 0 || mo === 0 && (today.getDate() < bday.getDate())) {

            age--;
        }

        return age;
    }

    var createPatientProfile = function createnewPatientProfile() {

        $('ul.nav-sidebar a.patient').addClass('active');

        window.location.href = patientProfile.createPatientProfile;

    }


    if ($.fn.DataTable.isDataTable('#tablePatient')) {

        $('#tablePatient').dataTable().fnDestroy();
        $('#tablePatient').dataTable().empty();

    }

    var patientTable = $('#tablePatient').DataTable({
        "paging": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "language": {

            'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
        },

        "dom": "<'#tablePatientop.row'<'col-sm-6'B><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'#tablePatientop.row'<'col-sm-5'l><'col-sm-7'p>>"

        ,
        "ajax": {
            "url": patientProfile.getPatientlist,
            "type": "Get",
            "datatype":"json"
        }
        ,
        "columns": [
            {
                "data": "PatientId",
                "className": "text-center"
            },
            {
                "data":"Lastname",
                "className": "text-capitalize",
                "render":function(data, type, patient) {
                    if (patient.Middle === null) {
                        return patient.Lastname + ' ,' + patient.Firstname;
                    } else {
                        return patient.Lastname + ' ,' + patient.Firstname + ' '+ patient.Middle;
                    }
                }
            },
            {
                "data": "Municipality",
                "className": "text-capitalize",
                "render":function(data, type, address) {
                    if (address.AddStreetBrgy !== null) {
                        return address.AddStreetBrgy + ' ,' + address.Municipality + ' ,' + address.Province;
                    } else {
                        return address.Municipality + ' ,' + address.Province;
                    }
                }
            },
            { "data": "Gender", "className": "text-capitalize" },
            {
                "data": "DateofBirth",
                "className":"text-center",
                "render": function (data) {

                    return getAge(data);
                }
            },

            {
                "data": "ContactCell"
             
            },
            {
                "data": null,
                "className": "text-center",
                "render": function () {

                    return '<div class="btn-group btn-group-sm">' +
                            '<a href="#" class="btn btn-warning"><i class="fas fa-eye"></i></a>' +
                            '<a href="#" class="btn btn-success"><i class="fas fa-edit"></i></a>' +
                            '<a href="#" class="btn btn-danger" id="remove-patient"><i class="fas fa-trash"></i></a>' +
                            '</div>'
                            ;
                }
            }
        ]
        ,
        "columnDefs": [
            { 'width': '8%', 'searchable': true, 'orderable': true, 'targets': 0 },
            { 'width': '5%', 'searchable': false, 'orderable': false, 'targets': 4 },
            { 'width': '15%', 'searchable': false, 'orderable': false, 'targets': 5 },
            {
                'targets': 6,
                'searchable': false,
                'orderable': false,
                'width': '12%'
             
            }
          
          
        ]
        ,
        createdRow: function (row, data, dataIndex) {
            $(row).attr('data-patientId', data.PatientId);
        },

        buttons: [
                    {
                        text: '<i class="fas fa-users-medical fa-md fa-fw"></i>',
                        className: 'btn btn-success btn-flat btnAddPatient',
                        titleAttr: 'New Patient Profile',
                        action: function () {

                            createPatientProfile();

                        }
                    }
                ]

    });

   
    $('div #savepatientinfo').on('click', function (e) {
        e.preventDefault();

        var form = $(this).closest('form');

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Saving Patient Details..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Save it!',
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

            if (result.value) {

                var formUrl = $('#form-patient').attr('action');
                $.validator.unobtrusive.parse(form);
                form.validate();

                if (form.valid()) {

                    $.ajax({
                        type: 'Post',
                        url: formUrl,
                        data: form.serialize(),
                        datatype: 'json',
                        cache: false,
                        success:function(data) {
                            if (data.success) {

                                //toastr.info('New Record Succesfully Added');
                                toast.fire({
                                    type: 'info',
                                    title: 'New Record Succesfully Added.'
                                });

                               
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error adding record!', 'Please try again', 'error');
                        }

                    }).done(function () {
                        setTimeout(function () {

                            window.location.href = patientProfile.indexPatientProfile;
                           

                        }, 2000);
                     
                      
                    });

                }

                }
            }

           
        );


    });


   


});

$(document).on('click', '#remove-patient', function(e) {

    var id = $(this).closest('tr').attr('data-patientId');

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Removing This Patient..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed for removing!'

    }).then((result) => {
        
            if (result.value) {

                $.ajax({
                    type: "post",
                    url: patientProfile.removePatientProfile,
                    ajaxasync: true,
                    data: { patientId: id },
                    cache: false,
                    success:function(data) {
                        
                        if (data.success) {
                            
                            toast.fire({
                                type: 'info',
                                title: 'Record Succesfully Deleted.'
                            });

                        }
                    }

                }).done(function () {

                    $('#tablePatient').DataTable().ajax.reload();
                });
            }
            
    });
});