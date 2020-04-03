class Appointment {
    constructor(id, name) {
        this.patientid = id;
        this.patientname = name;
    }
}

(function ($) {

    var elementTable = document.getElementById('tableAppoint');

    var appointmenthub = $.connection.appointmenthub;

    //$.connection.hub.start().done(function () {
    //    alert("Connected" + $.connection.hub.id);
    //});

    $.connection.hub.logging = true;


    appointmenthub.client.displayAppointment = function(model) {

        var outputModel = JSON.parse(model);
        let appoint = '';
        let i = 0;
        debugger;

        var objcount= Object.keys(outputModel).length;

        if (objcount > 0) {

            outputModel.forEach(function(el) {
                i += 1;

                appoint += '<tr data-patNo="' +
                    el.No +
                    '">' +
                    '<td>' +
                    i +
                    '</td>' +
                    '<td>' +
                    el.PatientName +
                    '</td>' +
                    '<td>' +
                    el.Stat +
                    '</td>' +
                    '<td class="text-center">' +
                    '<button class="btn btn-info btn-sm" data-patientId="' +
                    el.PatientNo +
                    '" id="view_medicalrecords">' +
                    '<i class="fal fa-file-medical-alt"></i>' +
                    '</button>' +
                    '</td>' +
                    '</tr>';
            });
        } else {
            appoint += '<tr>' +
                '<td colspan="4" class="text-center bg-gradient-danger"> No Record Found</td>' +
                '</tr>';
        }

       

        elementTable.innerHTML = appoint;

    }

    $.connection.appointmenthub.connection.start();




})(jQuery);