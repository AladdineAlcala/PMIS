var $selectedObject;
var $tableUsers;


(function($) {

   

    function addUserRole(selectedobj) {

        if (selectedobj.hasClass('selected')) {

            var $this = selectedobj;
            //var id = $this.attr('data-userId');


            $.ajax({
                type: 'Get',
                url: '/Account/AddRoleUser',
                contentType: 'application/html;charset=utf8',
                data: { id: $this.attr('data-userId')},
                datatype: 'html',
                cache: false,
                success: function (result) {

                    var modal = $('#modal-addroles');

                    modal.find('.modal-body').html(result);

                    $('#sncdoc').hide();
                   
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


           
        }

    }



    var addnewuser = function () {

        $.ajax({
            type: 'Get',
            url:'/Account/RegisterUser',
            contentType: 'application/html;charset=utf8',
            datatype: 'html',
            cache: false,
            success: function (result) {

                var modal = $('#modal-adduser');
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

    };

    var removeUser = function (selectedobj) {

        var $this = selectedobj;


        Swal.fire({
            title: "Are You Sure ?",
            text: "Confirm removing user..",
            type: "question",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes,Proceed operation..!'

        }).then((result) => {

            if (result.value) {

             
                    $.ajax({
                        type: 'Post',
                        url: '/Account/RemoveUser',
                        data: { id: $this.attr('data-userId')},
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
                            Swal.fire('Error adding record!', 'Please try again', 'error');
                        }

                    }).done(function () {


                        $('#tbl-users').DataTable().ajax.reload();

                    
                        $tableUsers.button(0).enable();
                        $tableUsers.button(1).disable();
                        $tableUsers.button(2).disable();
                        $tableUsers.button(3).disable();
                    });
            
              
            }

        });



    };

    var modifyUser = function (selectedobj) { throw new Error("Not implemented"); };

    if ($.fn.DataTable.isDataTable('#tbl-users')) {

        $('#tbl-users').dataTable().fnDestroy();
        $('#tbl-users').dataTable().empty();

    }

        $tableUsers = $('#tbl-users').DataTable({
            "paging": true,
            "searching": true,
            "ordering": true,
            "info": true,
            "autoWidth": false,
            "language": {
                'processing':
                    '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
            },

            "dom": "<'##tbl-userstop.row'<'col-sm-6'B><'col-sm-6'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'#tbl-usersbottom.row'<'col-sm-5'l><'col-sm-7'p>>",
            "ajax": {
                "url": '/Account/GetAllUsers',
                "type": "Get",
                "datatype": "json"
            },
            "columns": [
                {
                    "data":null

                },
                {
                    "data": "username"

                },
                {
                    "data": "email"

                },
                { "data": "roles", "className": "text-capitalize" }

                //{
                //    "data": null,
                //    "className": "text-center",
                //    "render": function () {

                //        return '<div class="btn-group btn-group-sm">' +
                //                '<a href="#" class="btn btn-warning" id="view-profile"><i class="fas fa-eye"></i></a>' +
                //                '<a href="#" class="btn btn-success" id="edit-patient"><i class="fas fa-edit"></i></a>' +
                //                '<a href="#" class="btn btn-danger" id="remove-patient"><i class="fas fa-trash"></i></a>' +
                //                '</div>'
                //            ;
                //    }
                //}
            ],
            "columnDefs": [
                {
                    'targets': 0,
                    'searchable': false,
                    'orderable': false,
                    'width': '5%',
                    'className': 'select-checkbox',
                    'data': null,
                    'defaultContent': ''
                },
                { 'width': '25%', 'searchable': false, 'orderable': false, 'targets': 1 },
                { 'width': '20%', 'searchable': false, 'orderable': false, 'targets': 2 },
                { 'width': '20%', 'searchable': true, 'orderable': true, 'targets': 3 }
            
            ],
            createdRow: function(row, data, dataIndex) {
                $(row).attr('data-userId', data.userId);
            },
            "order": [["1", "asc"]],

            select: {
                style: 'os',
                selector: 'td:first-child'
                 },
        buttons: [
            {
                text: '<i class="fas fa-user-plus fa-md fa-fw"></i>',
                className: 'btn btn-default btn-flat btnAddUser',
                titleAttr: 'New User Profile',
                action: function () {
                   
                    addnewuser();

                }
            },
            {
                text: '<i class="fas fa-user-edit fa-md fa-fw"></i>',
                className: 'btn btn-default btn-flat btnModifyUser',
                titleAttr: 'Remove User',
                action: function () {
                  
                    modifyUser($selectedObject);

                }
            },
            {
                text: '<i class="fas fa-user-times fa-md fa-fw"></i>',
                className: 'btn btn-default btn-flat btnRemoveUser',
                titleAttr: 'Remove User',
                action: function () {
                   
                    removeUser($selectedObject);

                }
            },
            {
                text: '<i class="fad fa-users-cog fa-md fa-fw"></i>',
                className: 'btn btn-default btn-flat btnAddRoles',
                titleAttr: 'Add User Roles',
                action: function () {
                    addUserRole($selectedObject);
                }
            }
        ]

    });


        $('#tbl-users tbody').on('click', 'tr', function () {

        $selectedObject = null;

        //alert('sadasd');

        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');

            $tableUsers.button(0).enable();
            $tableUsers.button(1).disable();
            $tableUsers.button(2).disable();
            $tableUsers.button(3).disable();
        }

        else {
            $tableUsers.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');

            $tableUsers.button(0).disable();
            $tableUsers.button(1).enable();
            $tableUsers.button(2).enable();
            $tableUsers.button(3).enable();

            $selectedObject = $(this);
        }


    });


    

})(jQuery);

$(document).on('click', '#registerUser', function (e) {

    e.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm saving user..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

        if (result.value) {

            var formUrl = $('#registUser').attr('action');
            var form = $('[id*=registUser]');

            //console.log(form);

            $.validator.unobtrusive.parse(form);
            form.validate();


            if (form.valid()) {

                //console.log(formUrl);

                $.ajax({
                    type: 'Post',
                    url: formUrl,
                    data: form.serialize(),
                    datatype: 'json',
                    cache: false,
                    success: function (data) {

                        if (data.success) {

                            $("#modal-adduser").modal('hide');

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


                    $('#tbl-users').DataTable().ajax.reload();

                });

            }

        }

    });


});


$(document).on('change', '#' + 'syncopt', function(e) {

    $('#sncdoc').hide();


    if ($(this).find("option:selected").text() === 'doctor') {

        $('#sncdoc').show();
    }
});

$(document).on('click', '#add_roletouser', function (e) {

    e.preventDefault();

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm saving role..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

        if (result.value) {

            var formUrl = $('#formaddroletoUser').attr('action');
            var form = $('[id*=formaddroletoUser]');

            //console.log(form);

            $.validator.unobtrusive.parse(form);
            form.validate();


            if (form.valid()) {

                //console.log(formUrl);

                $.ajax({
                    type: 'Post',
                    url: formUrl,
                    data: form.serialize(),
                    datatype: 'json',
                    cache: false,
                    success: function (data) {

                        if (data.success) {

                            $("#modal-addroles").modal('hide');

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


                    $('#tbl-users').DataTable().ajax.reload();

                });

            }

        }

    });

});



document.getElementById('btn-saveUserProfile').addEventListener('click',
    function (e) {
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
                            success: function (data) {
                                if (data.success) {

                                    //toastr.info('New Record Succesfully Added');
                                    toast.fire({
                                        type: 'info',
                                        title: ' Record Succesfully Updated.'
                                    });


                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                Swal.fire('Error adding record!', 'Please try again', 'error');
                            }

                        }).done(function () {
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
    function (e) {
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
                            success: function (data) {
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
                            error: function (xhr, ajaxOptions, thrownError) {
                                Swal.fire('Error adding record!', 'Please try again', 'error');
                            }

                        }).done(function () {
                            //setTimeout(function () {

                            //    window.location.href = patientProfile.indexPatientProfile;


                            //}, 2000);


                        });

                    }

                }
            }
        );
    });