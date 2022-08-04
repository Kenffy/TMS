var dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/Task",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "requestDate","width": "20%"},
            { "data": "totalOfHours", "width": "10%" },
            { "data": "priority.name", "width": "15%" },
            { "data": "status", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return ` <div class="text-center">
                                <a href="/Admin/Task/details?id=${data}" class="btn btn-warning text-white" style="cursor:pointer; width:50px;">
                                    <i class="fas fa-info"></i>
                                </a>
                                <a href="/Admin/Task/upsert?id=${data}" class="btn btn-success text-white" style="cursor:pointer; width:50px;">
                                    <i class="far fa-edit"></i>
                                </a>
                                <a class="btn btn-info text-white" style="cursor:pointer; width:50px;" onclick=Close('/api/Task/'+${data})>
                                    <i class="fas fa-lock"></i>
                                </a>
                                <a class="btn btn-danger text-white" style="cursor:pointer; width:50px;" onclick=Delete('/api/Task/'+${data})>
                                    <i class="far fa-trash-alt"></i>
                                </a>
                    </div>`;
                }, "width": "25%"
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%",
        "order": [[2, "asc"]]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function Close(url) {
    swal({
        title: "Are you sure you want to Close?",
        text: "You will not be able to modify this task!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'POST',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}