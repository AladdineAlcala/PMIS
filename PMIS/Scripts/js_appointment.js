//$(function() { 

//});

var count;
var appoint_date;
var phyId = null;


function callCalender() {
    var x = 5;
    var doc = 'JLV';
    let date = new Date();
    var d = date.getDate(),
        m = date.getMonth(),
        y = date.getFullYear();

    var Calendar = FullCalendar.Calendar;
    var calendarEl = document.getElementById('app-calendar');

    var calendar = new Calendar(calendarEl, {

        plugins: ['bootstrap', 'dayGrid'],
        timeZone: 'UTC',
        defaultView: 'dayGridMonth',

        events: [
            {
                title: `${doc} : (${x})`,
                start: new Date(y, m, d),
                backgroundColor: '#f56954', //red
                borderColor: '#f56954' //red
            }
            ,
            {
                title: `${doc} : (${x})`,
                start: new Date(y, m, d + 1),
                backgroundColor: '#f56954', //red
                borderColor: '#f56954' //red
            }
            ,
            {
                title: `${doc} : (${x})`,
                start: new Date(y, m, d + 1),
                backgroundColor: '#f56954', //red
                borderColor: '#f56954' //red
            }
            ,
            {
                title: `${doc} : (${x})`,
                start: new Date(y, m, d + 1),
                backgroundColor: '#f56954', //red
                borderColor: '#f56954' //red
            }


        ]
        ,
        contentHeight: 450,
        aspectRatio: 1.5,
        displayEventTime: false
    });

    calendar.render();
}

function appointcounter(id, appointdate) {

    //console.log(id);
    //console.log(appointdate);


    $.ajax({
        type: 'Get',
        url: '/Appointment/GetAppointCount',
        ajaxasync: true,
        data: { id: id, appdate: appointdate },
        dataType: 'json',
        cache: false,
        success: function (data) {
            count = 0;
            //debugger;

            if (data !== null) {
                count = data;
                $('#appoint-count').html(data);
            } else {

                $('#appoint-count').html(0);
            }
        }
    });
}

function loadAppointmentDateSetting() {

    let dateNow = new Date();

    $('#datetimepicker4').datetimepicker({

        defaultDate: dateNow,
        enabledHours: false,
        format: "MMM-DD-YYYY",
        locale: moment().local('en'),
        minDate: dateNow

    });

    $('#datetimepicker5').datetimepicker({

        defaultDate: dateNow,
        enabledHours: false,
        format: "MMM-DD-YYYY",
        locale: moment().local('en'),
        minDate: dateNow

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


    $('#patientname').typeahead({
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

            $('#patientId').val(patient.PatientId);

            console.log(patient.PatientId);

        });
}





(function ($) {

    appoint_date = moment(new Date($('#appiont-date').html())).format('YYYY-MM-DD HH:mm');

    appointcounter(null, appoint_date);

    ////Custom date validation overide for date formats
    $.validator.methods.date = function (value, element) {

        return this.optional(element) || moment(value, "MMM-DD-YYYY", true).isValid();
    }

    //callCalender();


    $('#add-appointment').prop("disabled", true);


    document.getElementsByClassName('appointOptions').selectedIndex = 0;


    $(document).on('click', "#add-appointment", function (e) {
        e.preventDefault();

        $.ajax({
            type: 'Get',
            url: '/Appointment/CreateAppointment',
            contentType: 'application/html;charset=utf8',
            datatype: 'html',
            cache: false,
            success: function(result) {

                var modal = $('#modal-createAppointment');
                modal.find('.modal-body').html(result);


                let patients = new Bloodhound({
                    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('PatientName'),
                    queryTokenizer: Bloodhound.tokenizers.whitespace,
                    remote: {
                        url: '/Patient/GetPatientAutoComplete?query=%QUERY',
                        wildcard: '%QUERY'
                    }
                });


                $('#patientname').typeahead({
                                highlight: true
                            },
                            {
                                name: 'patients',
                                display: 'PatientName',
                                source: patients
                            }


                        ).on("typeahead:selected typeahead:autocompleted",
                            function(e, patient) {
                                e.preventDefault();
                      
                                $('#patientId').val(patient.PatientId);

                       

                            });
            
                      

                loadAppointmentDateSetting();

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

})(jQuery);





$(document).on('click', '#btn-saveAppointment', function (e) {
    e.preventDefault();

    var ele = $('#appointment-TabContent');

    var appoint = ele.closest('div').find('.show.active').find('form');
    //const pId = $('#select-doctor').val();
    //alert(appoint.attr('action'));

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm saving Appointment..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

        if (result.value) {

            var formUrl = appoint.attr('action');
            var id = appoint.attr('id');
            var form = $('[id*=' + id + ']');

            //console.log(form);

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

                            $('#modal-createAppointment').modal('hide');

                            toast.fire({
                                type: 'success',
                                title: 'Record Succesfully Added.'
                            });

                        } else {

                            Swal.fire('Unable to add record!', 'Please try again or check if has already appointment', 'info');
                        }


                    }
                    ,
                    error: function (xhr, ajaxOptions, thrownError) {

                        Swal.fire('Error adding record!', 'Please try again or check record', 'error');
                    }

                }).done(function (data) {

                    if (data.success) {

                        //reload table appointment
                        $('#tableAppoint').load(data.url);

                        //refresh counter
                        appointcounter(phyId, appoint_date);

                    }


                }); //end ajax
            }

        }
    }
    );

});


$(document).on('change', '#select-doctor', function (e) {
    e.preventDefault();
    e.stopPropagation();


    var norows = document.getElementById('no-rows-alert');
    var hasrows = false;

    phyId = $(this).val();

    appoint_date = moment(new Date($('#appiont-date').html())).format('YYYY-MM-DD HH:mm');

    if ($(this).val() !== "") {

        $.ajax({
            type: "Get",
            url: appointment.getAppointmentbyDoctorId,
            data: { id: phyId, appdate: appoint_date },
            contentType: 'application/html;charset=utf8',
            datatype: 'html',
            cache: false,
            success: function (result) {


                $('#tableAppoint').html(result);

                appointcounter(phyId, appoint_date);

                hasrows = true;

            },
            error: function (xhr, ajaxOptions, thrownError) {
                Swal.fire('Error retrieving record!', 'Please try again', 'error');


            }
        });

        $('#add-appointment').prop("disabled", false);

    } else {

        $('#tableAppoint').children().remove();

        norows.style.display = hasrows ? 'none' : 'inherit';
        if (!hasrows) {
            setTimeout(function () {
                norows.style.display = 'none';
            }, 1500);
        }

        $('#appoint-count').html(0);


        $('#add-appointment').prop("disabled", true);
    }





    //$('#tableAppiont').load(data.url);


});

$(document).on('click', '#btn-medicalrecords', function (e) {
    e.preventDefault();
    e.stopPropagation();


    window.location.href = '/PatientRecord/MedicalHistory/' + $(this).attr('data-patientId') + '/' + $('#select-doctor').val();

});


$(document).on('click', '#btnconsultationoption', function (e) {
    e.preventDefault();
    e.stopPropagation();

    const appointNo = $(this).closest('tr').attr('data-patNo');
 
    $.ajax({
        type: 'Get',
        url:'/Appointment/AppointmentOptions',
        contentType: 'application/html;charset=utf8',
        data: {appno:appointNo},
        datatype: 'html',
        cache: false,
        success: function (result) {

            var modal = $('#modal-appointoptions');
            modal.find('.modal-body').html(result);


            let patients = new Bloodhound({
                datumTokenizer: Bloodhound.tokenizers.obj.whitespace('PatientName'),
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                remote: {
                    url: '/Patient/GetPatientAutoComplete?query=%QUERY',
                    wildcard: '%QUERY'
                }
            });


            $('#patientname').typeahead({
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

                    $('#patientId').val(patient.PatientId);

                    //console.log(patient.PatientId);

                });


            $('#replacement').hide();

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

$(document).on('change', '.appointOptions', function(e) {
    e.preventDefault();

    $('#replacement').hide();

    if ($(this).val() === "Replace") {
        $('#replacement').show();
    }

});


$(document).on('click', '#btn-saveAppointmentOption', function (e) {
    e.preventDefault();


    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm saving Appointment Option..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

            if (result.value) {

                var formUrl = $('#form-appointoption').attr('action');
                var form = $('[id*=form-appointoption]');

                //console.log(form);

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

                                $('#modal-appointoptions').modal('hide');

                                toast.fire({
                                    type: 'success',
                                    title: 'Record Succesfully Modified.'
                                });

                            } else {

                                Swal.fire('Unable to add record!', 'Please try again or check if has already appointment', 'info');
                            }


                        }
                        ,
                        error: function (xhr, ajaxOptions, thrownError) {

                            Swal.fire('Error adding record!', 'Please try again or check record', 'error');
                        }

                    }).done(function (data) {

                        if (data.success) {

                            //console.log(data.url);
                            //reload table 
                            //reload table appointment
                            $('#tableAppoint').load(data.url);

                            //refresh counter
                            appointcounter(phyId, appoint_date);

                        }


                    }); //end ajax
                }

            }
        }
    );

    
});
