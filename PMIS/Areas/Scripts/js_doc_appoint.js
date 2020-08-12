﻿
var count;
var appoint_date;
var phyId = null;


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


function getAppointment(id,appointmentdate) {
    $.ajax({
        type: 'Get',
        url: '/Doctor/DocAppointment/GetAppointmentByDoctor',
        ajaxasync: true,
        data: { id: id, appointmentDate: appointmentdate },
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            $('#tableAppoint').html(result);
        }
    });
}



(function ($) {
    
    var appointdate = moment(new Date()).format('YYYY-MM-DD HH:mm');

    getAppointment($('#hdn_docuserId').val(), appointdate);

    appointcounter($('#hdn_docuserId').val(), appointdate);


})(jQuery);

$(document).on('click', '#btnconsultationserve', function (e) {

    e.preventDefault();
    var appointdate = moment(new Date()).format('YYYY-MM-DD HH:mm');

    $('#spinn-loader').show();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm serve appointment..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

            if (result.value) {

                $.ajax({
                    type: 'Post',
                    url: '/Doctor/DocAppointment/ServeAppointment',
                    data: { apptId: ($(this).closest('tr').attr('data-apptNo')) },
                    datatype: 'json',
                    cache: false,
                    success: function (data) {

                        if (data.success) {

                            toast.fire({
                                type: 'success',
                                title: 'Record Succesfully Updated.'
                            });
                        }



                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error adding record!', 'Please try again', 'error');
                    }

                }).done(function (data) {

                    $('#tableAppoint').load(data.url);

                    setTimeout(function () {

                        $('#spinn-loader').hide();

                    }, 1000);

                    appointcounter($('#hdn_docuserId').val(), appointdate);
                });

            }
            else if (result.dismiss === Swal.DismissReason.cancel) {
                setTimeout(function () {

                    $('#spinn-loader').hide();

                }, 1000);
            }
        }

    );
});

$(document).on('click', '#view_medicalrecords', function (e) {

    e.preventDefault();
    e.stopPropagation();

    //console.log($('#hdn_docuserId').val());

    window.location.href = docAppoint.getPatientMedicalHistory.replace("patid", $(this).attr('data-patientId')).replace("phid", $('#hdn_docuserId').val());
});