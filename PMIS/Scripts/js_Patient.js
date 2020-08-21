



function take_snapshot() {
    // take snapshot and get image data
    Webcam.snap(function (data_uri) {
     
        const file = {
            id: $('#patient-Id').val(),
            base64Image: data_uri
        }

        saveSnapShot(file);

    });


    Webcam.reset();

    $("#modal-picsnapshot").modal('hide');
}




const saveSnapShot = (file) => {

    let formData = new FormData();
    formData.append("patId",file.id);
    formData.append("base64Image", file.base64Image);

    $.ajax({
        url: '/Patient/SaveProfileImage',
        type: 'Post',
        data: formData,
        processData: false,
        contentType: false,
        success:function(data) {
            if (data.success) {
                console.log(data);
                //// display results in page
                  document.getElementById('img-portrait').innerHTML =
                '<img id="profilePrev" class="img-thumbnail img-fluid" src="/Content/Images/' + data.randomFileName + '"/>';
            }
        }
    });

}

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

$(document).ready(function () {

    var patId = $('#patient-Id').val();
    var dateofbirth = $('#dob').val();

    $('span.get-age').html(getAge(dateofbirth));

    var createPatientProfile = function createnewPatientProfile() {

        $('ul.nav-sidebar a.patient').addClass('active');


        window.location.href = '/Patient/CreatePatientProfile';

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
            "url": '/Patient/GetAllPatientsAsync',
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
                            '<a href="#" class="btn btn-warning" id="view-profile"><i class="fas fa-eye"></i></a>' +
                            '<a href="#" class="btn btn-success" id="edit-patient"><i class="fas fa-edit"></i></a>' +
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

                $('#spinn-loader').show();

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

                    }).done(function (data) {

                        setTimeout(function () {


                            $('#spinn-loader').hide();

                           // window.location.href = patientProfile.indexPatientProfile;
                           
                            

                        }, 1000);
                     
                        window.location.href = '/Patient/ViewProfile/' + data.patId;
                    });

                }

                }
            }

           
        );


    });


    //$('#modal-picsnapshot').on('hidden.bs.modal',
    //    function() {
         
    //        Webcam.reset();

    //    });


    $('#btnupdateprofileclose').on('click', event=> {
        event.preventDefault();
        event.stopPropagation();

        window.location.href = '/Patient/Index';

    });


    var patientAppointmentList = (patId) => {

        $.ajax({
            type: 'Get',
            url: '/Patient/GetAllAppointmentByPatient',
            ajaxasync: true,
            data: { patientId: patId },
            contentType: 'application/html;charset=utf8',
            datatype: 'html',
            cache: false,
            success: function (result) {

                $('#appointment-info').html(result);
            }
        });
    }

    if (patId != null) {

        patientAppointmentList(patId);

       
    }

    if ($.fn.DataTable.isDataTable('#table-appointmentList')) {

        $('#table-appointmentList').dataTable().fnDestroy();
        $('#table-appointmentList').dataTable().empty();

    }


    var tableAppointmentList = $('#table-appointmentList').DataTable({
        "paging": true,
        "searching": false,
        "ordering": false,
        //"info": false,
        "serverSide": false
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
                                type: 'success',
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


$(document).on('click','#edit-patient',function(e) {
    e.preventDefault();
    e.stopPropagation();

    var id = $(this).closest('tr').attr('data-patientId');

    window.location.href = "/Patient/EditPatient/" + id;


});

$(document).on('click', '#btn-updatepatientprofile', function(e) {
    e.preventDefault();
    e.stopPropagation();

    var ele = $('#patientInfo-TabContent');

    var activeform = ele.closest('div').find('.show.active').find('form');

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Updating Information on this Patient..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed operation!'

    }).then((result) => {

            if (result.value) {

                var formUrl = activeform.attr('action');
                var id = activeform.attr('id');
                var form = $('[id*=' + id + ']');

                console.log(form);

                $.validator.unobtrusive.parse(form);

                form.validate();

                if (form.valid()) {

                    $.ajax({
                        type: 'Post',
                        url: formUrl,
                        data: form.serialize(),
                        datatype: 'json',
                        cache: false,
                        success: function (data) {

                            if (data.success) {

                                toast.fire({
                                    type: 'success',
                                    title: 'Record Succesfully Updated.'
                                });

                            } else {

                                Swal.fire('Unable to update record!', 'Please try again', 'info');
                            }


                        }
                        ,
                        error: function (xhr, ajaxOptions, thrownError) {

                            Swal.fire('Error adding record!', 'Please try again or check record', 'error');
                        }

                    }).done(function () {

                        window.location.href = "/Patient/Index";


                    }); //end ajax
                }

            }

        }
    );

});

$(document).on('click', '#view-profile', function(e) {
    e.preventDefault();

    window.location.href = '/Patient/ViewProfile/' + $(this).closest('tr').attr('data-patientId');

});


document.getElementById('profilepicbtn').addEventListener('click', function(e) {
    e.preventDefault();

    $.ajax({
        type: 'Get',
        url: '/Patient/ProfileImageModal',
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            var modal = $('#modal-picsnapshot');
            modal.find('.modal-body').html(result);

            Webcam.set({
                width: 420,
                height: 320,
                dest_width: 640,
                dest_height: 480,
                image_format: 'jpeg',
                jpeg_quality: 90,
                force_flash: false,
                flip_horiz: true,
                fps: 45
            });
            Webcam.attach('#picsnap');

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




});

$(document).on('click', '#capture-profileImage', function (e) {
    e.preventDefault();
    take_snapshot();
});


$(document).on('click', '#view-medicalinfo', function (e) {
    e.preventDefault();

    alert('sds');
});