function loadsummernote() {
    $('#compose-record').summernote();
}

function loadMedicalRecord(patId, phyId) {

    $('#spinn-loader').show();

    var thisPage = $('#currPage').val();
    console.log(thisPage);


    $.ajax({
        type: "Get",
        url: '/Doctor/PatientMedicalRecord/MedicalHistory',
        data: { id: patId, phyid: phyId},
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
        url: '/Doctor/PatientMedicalRecord/GetRecordChart',
        contentType: 'application/html;charset=utf8',
        data: {recordNo:recordno},
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



//var newmedicalrecord = document.getElementById('addPatientMedicalRecord');
//    newmedicalrecord.addEventListener('click', function (event) {
//        event.preventDefault();
//        event.stopPropagation();


//        const patientId = document.getElementById('patientid').value;

//        $.ajax({
//            type: 'Get',
//            url: '/Doctor/PatientMedicalRecord/New_MedicalRecord',
//            data: { id: patientId},
//            contentType: 'application/html;charset=utf8',
//            datatype: 'html',
//            cache: false,
//            success: function (result) {

//                var modal = $('#modal-createPhyRecord');
//                modal.find('.modal-body').html(result);

//                loadsummernote();

//                modal.modal({
//                        backdrop: 'static',
//                        keyboard: false
//                    },
//                    'show');

//            },
//            error: function (xhr, ajaxOptions, thrownError) {
//                //Swal.fire('Error adding record!', 'Please try again', 'error');
//            }
//        });




//    });

(function ($) {


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

                //get_physician(patient.PatientId);
                
                window.location.href = patMedicalRecord.getMedicalRecordByDoc.replace("patid", patient.PatientId).replace("phid", $('#hdnLogPhyId').val());
            });
    }



    autocompleteName();


})(jQuery);


$(document).on('click', '.post #modifychart', function (e) {
    e.preventDefault();
    e.stopPropagation();

    alert($(this).closest('.post').attr('data-postId'));


});


$(document).on('click', '.post #viewchart', function (e) {
    e.preventDefault();
    e.stopPropagation();

    //var recordNo = $(this).closest('.post').attr('data-postId');

    var pageSelected = $('.pagination').find('li.active').next().text();

    console.log(parseInt(pageSelected) - 1);


    loadChart($(this).closest('.post').attr('data-postId'));

});


$(document).on('click', '#medication', function (e) {
    e.preventDefault();

    $.ajax({
        type: 'Get',
        url: '/Doctor/PatientMedicalRecord/CreateMedication',
        data: { recNo: $('#recNo').val() },
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


$(document).on('click', '#modalClose', function () {

    $("#modal-createMedication").modal("hide");
});



$(document).on('click', '#save-docmedication', function(e) {
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

                var formUrl = $('#form_docAddMedication').attr('action');
                var form = $('[id*=form_docAddMedication]');

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


$(document).on('click', '#return_to_list', function(e) {
    e.preventDefault();
    e.stopPropagation();

    loadMedicalRecord($('#patientid').val(), $('#phyid').val());
});