

function loadsummernote() {
    $('#compose-record').summernote();
}

function loadMedicalRecord(patId, phyId) {

    $('#spinn-loader').show();


    $.ajax({
        type: "Get",
        url: '/PatientRecord/Load_MedicalRecordPartialView',
        data: { patientid: patId, phyid: phyId },
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            $('#medicalrecord').html(result);

        },
        error: function (xhr, ajaxOptions, thrownError) {

            Swal.fire('Error retrieving record!', 'Please try again', 'error');


        }


    }).done(function () {

        setTimeout(function () {

            $('#spinn-loader').hide();

        }, 1000);

    });


}


function loadChart(recordno) {
    
    $('#spinn-loader').show();

    $.ajax({
        type: 'Get',
        url: '/PatientRecord/GetRecordChart',
        contentType: 'application/html;charset=utf8',
        data: { recordNo:recordno},
        datatype: 'html',
        cache: false,
        success: function (result) {

            $('#medicalrecord').html(result);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            Swal.fire('Error on retrieving record!', 'Please try again', 'error');
        }


    }).done(function () {

        setTimeout(function () {

            $('#spinn-loader').hide();

        }, 1000);


    });
}


var getAge = function (dob) {
    var b_day = new Date(dob);
    var today = new Date();
    var d = moment(b_day).format('MM/DD/YYYY');
    var bday = new Date(d);
    console.log(bday);
    var age = today.getFullYear() - bday.getFullYear();
    var mo = today.getMonth() - bday.getMonth();

    if (mo < 0 || mo === 0 && (today.getDate() < bday.getDate())) {

        age--;
    }

    return age;
}



function get_physician(patId) {

    $.ajax({
        type: "Get",
        url: '/PatientRecord/GetAllPhysicianforPatient',
        data: { patientId: patId },
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            $('#patient-physList').html(result);

        },
        error: function (xhr, ajaxOptions, thrownError) {
            Swal.fire('Error retrieving record!', 'Please try again', 'error');


        }
    });



}



$(function () {


    $('#get-age').html(getAge($('#patient-info').val()));


    function autocompleteName() {

        let patients = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('PatientName'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: '/Patient/GetPatientAutoComplete?query=%QUERY',
                wildcard: '%QUERY'
            }
        });


        $('#search-patient').typeahead({
            highlight: true
        },
            {
                name: 'patients',
                display: 'PatientName',
                source: patients
            }
        ).on("typeahead:selected typeahead:autocompleted",
            function (e, patient) {
                e.preventDefault();

                get_physician(patient.PatientId);

            });
    }



    autocompleteName();

    //console.log($('#patient-info').val());

});

$(document).on('click', '#addActivity', function (e) {

    e.preventDefault();
    e.stopPropagation();

    const patientId = $('#patientid').val();
    const phyId = $('#phyid').val();


    $.ajax({
        type: 'Get',
        url: '/PatientRecord/AddRecord',
        data: { patientId: patientId, physicianId: phyId },
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            var modal = $('#modal-createRecord');
            modal.find('.modal-body').html(result);

            loadsummernote();

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


$(document).on('click', '#save-record', function (e) {
    e.preventDefault();
    e.stopPropagation();

    const patientId = $('#patientid').val();
    const phyId = $('#phyid').val();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm saving Medical Record..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

        if (result.value) {

            var formUrl = $('#formPatientRecord').attr('action');
            var form = $('[id*=formPatientRecord]');

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

                            $("#modal-createRecord").modal('hide');

                            toast.fire({
                                type: 'success',
                                title: 'Record Succesfully Added.'
                            });
                        }


                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error adding record!', 'Please try again', 'error');
                    }

                }).done(function () {

                    loadMedicalRecord(patientId, phyId, function () {

                    });


                });

            }

        }
    }

    );

});


$(document).on('click', '.patient-record', function (e) {
    e.preventDefault();
    e.stopPropagation();

    window.location.href = '/PatientRecord/MedicalHistory/' + $(this).attr('data-patientId') + '/' + $(this).closest('tr').attr('data-phyId');
});


$(document).on('click', '.post #modifychart', function (e) {
    e.preventDefault();
    e.stopPropagation();

    alert('sadsadas');

});


$(document).on('click', '.post #viewchart', function (e) {
    e.preventDefault();
    e.stopPropagation();

    $('#spinn-loader').show();

    $.ajax({
        type: 'Get',
        url: '/PatientRecord/GetRecordChart',
        contentType: 'application/html;charset=utf8',
        data: { recordNo: $(this).parents('.post').attr('data-postId')},
        datatype: 'html',
        cache: false,
        success: function (result) {

            $('#medicalrecord').html(result);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            Swal.fire('Error on retrieving record!', 'Please try again', 'error');
        }


    }).done(function () {

        setTimeout(function () {

            $('#spinn-loader').hide();

        }, 1000);


    });

});

$(document).on('click', '#return_to_list', function(e) {

    e.preventDefault();
   
    loadMedicalRecord($('#patientid').val(), $('#phyid').val());

});


$(document).on('click', '#medication', function(e) {
    e.preventDefault();

    $.ajax({
        type: 'Get',
        url: '/PatientRecord/CreateMedication',
        data: { recNo:$('#recNo').val()},
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            var modal = $('#modal-createMedication');
            modal.find('.modal-body').html(result);

            $('#compose-medication').summernote({
                height: 350   //set editable area's height
                //codemirror: { // codemirror options
                //    theme: 'monokai'
                //}
            });

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


$(document).on('click', '#save-medication', function (e) {
    e.preventDefault();
    e.stopPropagation();

    const recordNo = $('#recNo').val();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm saving Medication Record..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

            if (result.value) {

                var formUrl = $('#formAddMedication').attr('action');
                var form = $('[id*=formAddMedication]');

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

                                $("#modal-createMedication").modal('hide');

                                toast.fire({
                                    type: 'success',
                                    title: 'Record Succesfully Updated.'
                                });
                            }


                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            Swal.fire('Error adding record!', 'Please try again', 'error');
                        }

                    }).done(function () {

                        loadChart(recordNo);
                    });

                }

            }
        }

    );

});

$(document).on('click', '#modalClose', function () {

    $("#modal-createMedication").modal("hide");
});
