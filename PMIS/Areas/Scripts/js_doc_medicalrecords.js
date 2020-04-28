﻿var $selected;


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
        error: function (thrownError) {

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
        error: function (thrownError) {
            Swal.fire('Error on retrieving record!', 'Please try again', 'error');
        }


    }).done(function () {

        setTimeout(function () {

            $('#spinn-loader').hide();

        }, 1000);


    });
}


const patientMedPrescrioption = (recNo) => {

    $.ajax({
        type: 'Get',
        url: '/Doctor/PatientMedicalRecord/GetMedPrescription',
        contentType: 'application/html;charset=utf8',
        data: { recordNo: recNo },
        datatype: 'html',
        cache: false,
        success: function(result) {

            $('#med_prescription').html(result);
        },
        error: function(ajaxOptions, thrownError) {
            Swal.fire('Error on retrieving record!', 'Please try again', 'error');
        }


    });

};

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

   // console.log(parseInt(pageSelected) - 1);


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



$(document).on('click', '#save-docmedication',(e)=> {
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


$(document).on('click', '#return_to_list',(e)=>{
    e.preventDefault();
    e.stopPropagation();

    loadMedicalRecord($('#patientid').val(), $('#phyid').val());
});


$(document).on('click', '#removeMedication', function(e) {
    e.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm removing Medication..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

            if (result.value) {

                $.ajax({
                    type: 'Post',
                    url: '/Doctor/PatientMedicalRecord/RemoveMedication',
                    data: { medNo: $('#medicationNo').val() },
                    async: true,
                    datatype: 'json',
                    cache: false,
                    success: function (data) {

                        if (data.success) {

                            toast.fire({
                                type: 'success',
                                title: 'Record Succesfully Removed.'
                            });
                        }


                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        Swal.fire('Error removing record!', 'Please try again', 'error');
                    }

                }).done(function () {

             

                  //  patientMedPrescrioption($('#recNo').val());
                });


            }

        }


    );

});


$(document).on('click','#prescription',event => {
    event.preventDefault();
    event.stopPropagation();

    $.ajax({
        type: 'Get',
        url: '/Doctor/PatientMedicalRecord/MedicalPrescription',
        data: { recNo: $('#recNo').val() },
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            var modal = $('#modal-prescription');
            modal.find('.modal-body').html(result);

            

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


$(document).on('click', '#btn-saveDoctorsPrescription', function(e) {
    e.preventDefault();

    const recordNo = $('#addMedRecNo').val();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm saving Record..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

            if (result.value) {

                var formUrl = $('#form-addDorctorPrescription').attr('action');
                var form = $('[id*=form-addDorctorPrescription]');

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

                                $("#modal-prescription").modal('hide');

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

                        patientMedPrescrioption(recordNo);
                    });

                }

            }
        }

    );



});

//remove doctors prescription

$(document).on('click', '#btn-prescriptionremove', function (e) {

    e.preventDefault();

    $("#tble-docPrescription tbody tr").each(function () {
       
        var tr = $(this);

        tr.find("td").each(function () {

           // var chk = $(this).find(":checkbox").prop("checked");
          
            if ($(this).find(":checkbox").prop("checked")) {

                var selectedid = $(this).closest('tr').find("input:checked").val();

                Swal.fire({
                    title: "Are You Sure ?",
                    text: "Confirm removing Record..",
                    type: "question",
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes,Proceed operation..!'

                }).then((result) => {

                        if (result.value) {

                            $.ajax({
                                type: 'Post',
                                url: '/Doctor/PatientMedicalRecord/RemoveDoctorsPrescription',
                                data: { id: selectedid },
                                async:true,
                                datatype: 'json',
                                cache: false,
                                success: function (data) {

                                    if (data.success) {

                                        toast.fire({
                                            type: 'success',
                                            title: 'Record Succesfully Removed.'
                                        });
                                    }


                                },
                                error: function (xhr, ajaxOptions, thrownError) {
                                    Swal.fire('Error removing record!', 'Please try again', 'error');
                                }

                            }).done(function () {

                                $('button.btn-x').prop("disabled", true);
                                $('#prescription').prop("disabled", false);

                              //  $(this).closest('tr').remove();

                                patientMedPrescrioption($('#recNo').val());
                            });


                        }

                    }


                );
               
            }

        });

    });

});


$(document).on('click', '#tble-docPrescription > tbody >tr', function(e) {

    if ($(e.target).is(':checkbox')) {


        $selected = $(e.target);
        var $this = $(e.target);

        $this.prop('checked')
            ? $('button.btn-x').prop("disabled", false)
            : $('button.btn-x').prop("disabled", true);

        $this.prop('checked')
            ? $('#prescription').prop("disabled", true)
            : $('#prescription').prop("disabled", false);

    }
});


