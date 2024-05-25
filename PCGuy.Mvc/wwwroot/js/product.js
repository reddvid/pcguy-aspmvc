$(document).ready(function () {
    loadDataTable();
});

var dataTable;

function loadDataTable() {
    dataTable = $('#dataTable').DataTable({
        "ajax": {url: '/admin/product/getall'},
        "columns": [
            {data: 'name'},
            {data: 'price', render: $.fn.dataTable.render.number(',', '.', 2, '₱')},
            {data: 'subcategory.name'},
            {data: 'subcategory.category.name'},
            {
                data: 'id',
                render: function (data) {
                    return ` <div class="w-75 btn-group" role="group">
                        <a class="link-info mx-2" href="/admin/product/upsert?id=${data}">Edit</a>
                        <a class="link-danger ms-2" style="cursor:pointer;" onclick="Delete('/admin/product/delete/${data}')">Delete</a>
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
