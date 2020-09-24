

function loadsummernote() {
    $('#compose-record').summernote({
        height: 350
    });
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

            $('#patmedicalrecord').html(result);

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
        data: {recordNo:recordno},
        datatype: 'html',
        cache: false,
        success: function (result) {

            $('#patmedicalrecord').html(result);
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


$(function () {


    $('#get-age').html(getAge($('#patient-info').val()));
    //console.log($('#compose-medicalrecord').val().length);



    autocompleteName();

    //console.log($('#patient-info').val());

});

$(document).on('click', '#close-modal-patrecord', function (e) {
    e.preventDefault();
    e.stopPropagation();

    $("#modal-createRecord").modal('hide');


});


$(document).on('click', '#addActivity', function (e) {

    e.preventDefault();
    e.stopPropagation();

    const patientId = $('#patientid').val();
    const phyId = $('#phyid').val();
    const appno = $('#appointmentNo').val();

    $.ajax({
        type: 'Get',
        url: '/PatientRecord/AddRecord',
        data: { patientId: patientId, physicianId: phyId, appNo: appno },
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

                    loadMedicalRecord(patientId, phyId);


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


   // alert($(this).closest('.post').attr('data-postId'));

    $.ajax({
        type: 'Get',
        url: '/PatientRecord/ModifyPatientRecord',
        data: { recordNo: $(this).closest('.post').attr('data-postId') },
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            var modal = $('#modal-modifyRecord');
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


$(document).on('click', '#update-record', function (e) {
    e.preventDefault();
    e.stopPropagation();

    const patientId = $('#patientid').val();
    const phyId = $('#phyid').val();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Updating Medical Record..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

            if (result.value) {

                var formUrl = $('#formModifyPatientRecord').attr('action');
                var form = $('[id*=formModifyPatientRecord]');

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

                                $("#modal-modifyRecord").modal('hide');

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

                        loadMedicalRecord(patientId, phyId);


                    });

                }

            }
        }

    );

});


//$(document).on('click', '.post #viewchart', function (e) {
//    e.preventDefault();
//    e.stopPropagation();

//    var pageSelected = $('.pagination').find('li.active').next().text();

//    loadChart($(this).closest('.post').attr('data-postId'));

//    var addActivity = document.getElementById('addActivity');
//    addActivity.style.visibility = "hidden";

//});


$(document).on('click', '.post #viewchart', function (event) {
    event.preventDefault();
    event.stopPropagation();
    $('#spinn-loader').show();

    var recordno = $(this).closest('.post').attr('data-postId');


    $.ajax({
        type: 'Get',
        url: '/PatientRecord/GetMedicalChartDetailAsync',
        data: { recNo: recordno },
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            var modal = $('#modal-viewmedicalchartdetail');
            modal.find('.modal-body').html(result);

            $('#compose-medicalrecord').summernote({
                height: 450   //set editable area's height
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
    }).done(function () {


        setTimeout(function () {

            $('#spinn-loader').hide();

        }, 1000);
    });


});


$(document).on('click', '#return_to_list', function(e) {

    e.preventDefault();
   
    loadMedicalRecord($('#patientid').val(), $('#phyid').val());
    var addActivity = document.getElementById('addActivity');
    addActivity.style.visibility = "visible";
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



$(document).on('click', '.post #removechart', function (e) {
    e.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Removing Record..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

            if (result.value) {
             
               //var recordno = $(this).closest('.post').attr('data-postId');

                $.ajax({
                    type: 'Post',
                    url: '/PatientRecord/RemovePatientRecord',
                    data: { recordId: $(this).closest('.post').attr('data-postId')},
                    datatype: 'json',
                    cache: false,
                    success: function (data) {

                        if (data.success) {

                            $("#modal-createMedication").modal('hide');

                            toast.fire({
                                type: 'success',
                                title: 'Record Succesfully Deleted.'
                            });
                        }


                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error adding record!', 'Please try again', 'error');
                    }

                }).done(function () {

                    loadMedicalRecord($('#patientid').val(), $('#phyid').val());
                });


            }
        }

    );
});

$(document).on('click', '#printPrescription', function (e) {
    e.preventDefault();

    var medRecordNo = document.getElementById('recNo').value;
    
    var url = "/Prescription/PrintPrescript?medRecNo=" + medRecordNo;
    window.open(url,"_blank");
});

$(document).on('click', '#view-profile', function (e) {
    e.preventDefault();

    window.location.href = '/Patient/ViewProfile/' + $('#patientid').val();

});


$(document).on("hidden.bs.modal", '#modal-modifyRecord', function (e) {
    e.preventDefault();

    $('#spinn-loader').show();

    setTimeout(function () {

        $('#spinn-loader').hide();

    }, 1000);

});