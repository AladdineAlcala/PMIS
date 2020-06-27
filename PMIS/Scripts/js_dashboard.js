




(function($) {
    const appointmentCount = function () {
        $.ajax({
            type: "Get",
            url: '/Dashboard/GetAppointments',
            contentType: 'application/html;charset=utf8',
            ajaxasync: true,
            datatype: 'html',
            cache: false,
            success: function (result) {

                $('#appointment').html(result);

            },
            error: function (xhr, ajaxOptions, thrownError) {
                Swal.fire('Error retrieving record!', 'Please try again', 'error');


            }
        });

    }

 

    const loggedPhysician = function () {
        $.ajax({
            type: "Get",
            url: '/Dashboard/GetLoggedPhysician',
            contentType: 'application/html;charset=utf8',
            datatype: 'html',
            cache: false,
            success: function (result) {

                $('#loggedphycisian').html(result);

            },
            error: function (xhr, ajaxOptions, thrownError) {
                Swal.fire('Error retrieving record!', 'Please try again', 'error');


            }
        });

    }

    
    const registeredPatients = function () {
        $.ajax({
            type: "Get",
            url: '/Dashboard/GetRegisteredPatients',
            contentType: 'application/html;charset=utf8',
            ajaxasync: true,
            datatype: 'html',
            cache: false,
            success: function (result) {

                $('#registeredPatients').html(result);

            },
            error: function (xhr, ajaxOptions, thrownError) {
                Swal.fire('Error retrieving record!', 'Please try again', 'error');


            }
        });

    }

    const getallMedicineCount = function () {
        $.ajax({
            type: "Get",
            url: '/Dashboard/GetMedicines',
            contentType: 'application/html;charset=utf8',
            ajaxasync: true,
            datatype: 'html',
            cache: false,
            success: function (result) {

                $('#medicines').html(result);

            },
            error: function (xhr, ajaxOptions, thrownError) {
                Swal.fire('Error retrieving record!', 'Please try again', 'error');


            }
        });

    }


    appointmentCount();

    loggedPhysician();
    registeredPatients();
    getallMedicineCount();

})(jQuery);