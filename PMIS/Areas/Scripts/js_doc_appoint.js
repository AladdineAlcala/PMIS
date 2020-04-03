
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



(function ($) {
    


})(jQuery);

//document.getElementById("view_medicalrecords").addEventListener('click',function() {

//    //e.preventDefault();
//    //e.stopPropagation();

//       alert($(this).attr('data-patientId'));

//    //window.location.href = docAppoint.getPatientMedicalHistory.replace("patid", $(this).attr('data-patientId')).replace("phid",2);
//});

$(document).on('click', '#view_medicalrecords', function (e) {

    e.preventDefault();
    e.stopPropagation();

    //console.log($('#hdn_docuserId').val());

    window.location.href = docAppoint.getPatientMedicalHistory.replace("patid", $(this).attr('data-patientId')).replace("phid", $('#hdn_docuserId').val());
});