var $selectedObject;
var $prescriptionList;

var newprescription = function () {
    // console.log(this);
    $.ajax({
        type: 'Get',
        url: '/Prescription/CreatePrescription',
        contentType: 'application/html;charset=utf8',
        datatype: 'html',
        cache: false,
        success: function (result) {

            console.log(result);

            var modal = $('#modal-createprescription');
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

var onremovePrescription = ()=> {
       
    var pId = $selectedObject.closest('tr').attr('data-prscId');

    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm Removing This Prescription..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Proceed for removing!'

    }).then((result) => {

        if (result.value) {

            $.ajax({
                type: "post",
                url: '/Prescription/RemovePrescription',
                ajaxasync: true,
                data: { id: pId },
                cache: false,
                success: function (data) {

                    if (data.success) {

                        toast.fire({
                            type: 'success',
                            title: 'Record Succesfully Deleted.'
                        });

                    }
                }

            }).done(function () {

                $('#tblePrescription').DataTable().ajax.reload();
            });
        }

    });

};



(function ($) {

    if ($.fn.DataTable.isDataTable('#tblePrescription')) {

        $('#tblePrescription').dataTable().fnDestroy();
        $('#tblePrescription').dataTable().empty();

    }


    $prescriptionList = $('#tblePrescription').DataTable({
        "paging": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "language": {

            'processing': '<i class="fa fa-spinner fa-spin fa-3x fa-fw"></i><span class="sr-only"> Loading data Pls. wait....</span>'
        },

        "dom": "<'#tablePrescriptiontop.row'<'col-sm-6'B><'col-sm-6'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'#tablePrescriptionbottom.row'<'col-sm-5'l><'col-sm-7'p>>"

        ,

        
        "ajax":
        {
            "url": '/Prescription/GetAllPrescriptionsAsync/',
            "type": "GET",
            "datatype": "json"
        }
        ,
       
        "columns":
        [
            { "data": null },
            { "data": "Id", "className": "text-center" },
            { "data": "PrescriptionCatDetails", "className": "text-capitalize" },
            { "data": "PrescriptionDetails", "className": "text-capitalize" },
            { "data": "PrescUnit", "className": "text-capitalize" }
            //{
            //    "data": null,
            //    "className": "text-center",
            //    "render": function () {

            //        return '<div class="btn-group btn-group-sm">' +
            //                '<a href="#" class="btn btn-warning"><i class="fas fa-eye"></i></a>' +
            //                '<a href="#" class="btn btn-success"><i class="fas fa-edit"></i></a>' +
            //                '<a href="#" class="btn btn-danger" id="remove-prescription"><i class="fas fa-trash"></i></a>' +
            //                '</div>'
            //            ;
            //    }
            //}
        ]
        ,
        "columnDefs":
        [
            {
                'targets': 0,
                'searchable': false,
                'orderable': false,
                'width': '5%',
                'className': 'select-checkbox',
                'data': null,
                'defaultContent': ''
            },
            { 'width': '10%', 'targets': 1 },
            { 'width': '20%', 'targets': 2 },
            {
                'targets': 4,
                'searchable': false,
                'orderable': false,
                'width': '15%'

            }
           
                 
        ]
        ,
        
        createdRow: function (row, data, dataIndex) {
            $(row).attr('data-prscId', data.Id);
        }
        ,
        "order": [[ 1, "asc" ]],

        select: {
            style: 'os',
            selector: 'td:first-child'

        }
        ,
        buttons: [
            {
                text: '<i class="fa fa-plus-square fa-fw"></i>',
                className: 'btn btn-primary btnAddCustomer',
                titleAttr: 'Add a new record',
                action: function () {

                    
                    newprescription();
                }
            },
            
            {
                text: '<i class="fa fa-edit fa-fw"></i>',
                className: 'btn btn-primary btnModifyCustomer',
                titleAttr: 'Modify record',
                action: function () {

                   

                }, enabled: false
            },
            {
                text: '<i class="fa fa-trash fa-fw"></i>',
                className: 'btn btn-primary btnRemoveCustomer',
                titleAttr: 'Remove record',
                action: function () {
                   
                    onremovePrescription();

                }, enabled: false
            }
        ]
    });

    $('#tblePrescription tbody').on('click', 'tr', function () {

        $selectedObject = null;


        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');

            $prescriptionList.button(0).enable();
            $prescriptionList.button(1).disable();
            $prescriptionList.button(2).disable();

        }

        else {
            $prescriptionList.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');

            $prescriptionList.button(0).disable();
            $prescriptionList.button(1).enable();
            $prescriptionList.button(2).enable();


            $selectedObject = $(this);
        }


    });


})(jQuery);





$(document).on('click', '#btn-savePrescription',(e)=> {
    e.preventDefault();



    Swal.fire({
        title: "Are You Sure ?",
        text: "Confirm saving Med. Prescription..",
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes,Proceed operation..!'

    }).then((result) => {

        if (result.value) {

            var formUrl = $('#formprescription').attr('action');
            var form = $('[id*=formprescription]');

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

                            $("#modal-createprescription").modal('hide');

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


                    $('#tblePrescription').DataTable().ajax.reload();

                });

            }

        }

    });

});

$(document).on('click', '#remove-prescription', function(e) {
    e.preventDefault();

    
});