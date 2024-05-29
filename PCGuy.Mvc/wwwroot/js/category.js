$(document).ready(function () {
    loadDataTable();
});

let dataTable;

function loadDataTable() {
    dataTable = $('#dataTable').DataTable({
        "ajax": {url: '/admin/category/getall'},
        "columns": [
            {data: 'name'},
            {
                data: 'id',
                render: function (data) {
                    return ` <div class="w-75 btn-group" role="group">
                        <a class="link-info mx-2" href="/admin/category/upsert?id=${data}">Edit</a>
                        <a class="link-danger ms-2" style="cursor:pointer;" onclick="Delete('/admin/category/delete/${data}')">Delete</a>
                    </div>`
                }
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                },
                error: function (data) {
                    dataTable.ajax.reload();
                    toastr.error(data.message);
                }
            })
        }
    });
}
