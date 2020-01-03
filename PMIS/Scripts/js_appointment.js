//$(function() { 

//});
var count;

const toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 5000
});


function appointcounter(id) {

    $.ajax({
        type: 'Get',
        url: '/Appointment/GetAppointCount',
        ajaxasync: true,
        data: { id: id },
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

function AutocompleteName() {

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
}





(function ($) {

 
    $('#add-appointment').prop("disabled", true);
   appointcounter(null);

    ////Custom date validation overide for date formats
    $.validator.methods.date = function (value, element) {

        return this.optional(element) || moment(value, "MMM-DD-YYYY", true).isValid();
    }


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
                title: 'All Day Event',
                start: new Date(y, m, 1),
                backgroundColor: '#f56954', //red
                borderColor: '#f56954' //red
            }


        ]
        ,
        contentHeight: 450,
        aspectRatio: 1.5
    });

    calendar.render();




})(jQuery);


$(document).on('click', '#add-appointment', function (e) {
    e.preventDefault();

    $.ajax({
        type: 'Get',
        url: appointment.newAppointment,
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            var modal = $('#modal-createAppointment');
            modal.find('.modal-body').html(result);

            AutocompleteName();

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


$(document).on('click', '#btn-saveAppointment', function (e) {
    e.preventDefault();

    var ele = $('#appointment-TabContent');

    var appoint = ele.closest('div').find('.show.active').find('form');

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

                        $('#modal-createAppointment').modal('hide');

                        toast.fire({
                            type: 'info',
                            title: 'Record Succesfully Added.'
                        });


                    }
                    ,
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error adding record!', 'Please try again', 'error');
                    }

                }).done(function (data) {

                    //$('#tableAppoint').load(data.url);
                    //reload table appointment

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
    var phyId = null;
    var hasrows = false;
    phyId = $(this).val();

    if ($(this).val() !== "") {

        $.ajax({
            type: "Get",
            url: appointment.getAppointmentbyDoctorId,
            data: { id: phyId },
            contentType: 'application/html;charset=utf8',
            datatype: 'html',
            cache: false,
            success: function (result) {


                $('#tableAppoint').html(result);

                appointcounter(phyId);

                hasrows = true;

            },
            error: function (xhr, ajaxOptions, thrownError) {
                Swal.fire('Error retrieving record!', 'Please try again', 'error');


            }
        });

        $('#add-appointment').prop("disabled", false);

    } else {

        $('#tableAppoint').children().remove();

        norows.style.display = hasrows?'none' : 'inherit';
        if (!hasrows) {
            setTimeout(function () {
                norows.style.display = 'none';
            },1500);
        }

        $('#appoint-count').html(0);


        $('#add-appointment').prop("disabled", false);
    }





    //$('#tableAppiont').load(data.url);


});