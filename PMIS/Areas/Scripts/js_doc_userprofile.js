
document.getElementById('btn-saveUserProfile').addEventListener('click',
    function(e) {
        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Saving Profile",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Save it!',
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

                if (result.value) {

                    var formUrl = $('#formUpdateProfileInfo').attr('action');
                    var form = $('[id*=formUpdateProfileInfo]');

                    $.validator.unobtrusive.parse(form);
                    form.validate();

                    if (form.valid()) {

                        $.ajax({
                            type: 'Post',
                            url: formUrl,
                            data: form.serialize(),
                            datatype: 'json',
                            cache: false,
                            success: function(data) {
                                if (data.success) {

                                    //toastr.info('New Record Succesfully Added');
                                    toast.fire({
                                        type: 'info',
                                        title: ' Record Succesfully Updated.'
                                    });


                                } 
                            },
                            error: function(xhr, ajaxOptions, thrownError) {
                                Swal.fire('Error adding record!', 'Please try again', 'error');
                            }

                        }).done(function() {
                            //setTimeout(function () {

                            //    window.location.href = patientProfile.indexPatientProfile;


                            //}, 2000);


                        });

                    }

                }
            }
        );
    });


document.getElementById('btn-saveUserSecurity').addEventListener('click',
    function(e) {
        e.preventDefault();

        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm Changing User Password",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Save it!',
            //closeOnConfirm: true, closeOnCancel: true
        }).then((result) => {

                if (result.value) {

                    var formUrl = $('#formUpdatePass').attr('action');
                    var form = $('[id*=formUpdatePass]');

                    $.validator.unobtrusive.parse(form);
                    form.validate();

                    if (form.valid()) {

                        $.ajax({
                            type: 'Post',
                            url: formUrl,
                            data: form.serialize(),
                            datatype: 'json',
                            cache: false,
                            success: function(data) {
                                if (data.success) {

                                    //toastr.info('New Record Succesfully Added');
                                    toast.fire({
                                        type: 'info',
                                        title: ' Record Succesfully Updated.'
                                    });

                                    document.getElementById('oldpass').value = '';
                                    document.getElementById('newpass').value = '';
                                    document.getElementById('confirmpass').value = '';
                                }

                                else {
                                    

                                    Swal.fire('Failed', data.errmsg, 'error');
                                    //console.log('asdasds');
                                    //console.log(data.errmsg);

                                }
                            },
                            error: function(xhr, ajaxOptions, thrownError) {
                                Swal.fire('Error adding record!', 'Please try again', 'error');
                            }

                        }).done(function() {
                            //setTimeout(function () {

                            //    window.location.href = patientProfile.indexPatientProfile;


                            //}, 2000);


                        });

                    }

                }
            }
        );
    });


document.getElementById("UpImage").onchange = function () {
    alert(this.value);
   
};
