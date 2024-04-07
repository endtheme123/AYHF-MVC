var dataTable;

function loadDataTable(status) {
    console.log("good");
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + status },
        "columns": [
            { data: 'id', "width": "25%" },
            { data: 'name', "width": "25%" },
            { data: 'phoneNumber', "width": "25%" },
            { data: 'applicationUser.email', "width": "10%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Detail 
                        </a>
                        
                    </div>`
                },
                "width": "25%"
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
                }
            })
        }
    });
}

$(document).ready(function () {
    console.log("good");
    var url = window.location.search;
    if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("shipping")) {
                loadDataTable("shipping");
            }
            else {
                if (url.includes("packaging")) {
                    loadDataTable("packaging");
                }
                else {
                    if (url.includes("pending")) {
                        loadDataTable("pending");
                    }
                    else {
                        if (url.includes("packed")) {
                            loadDataTable("packed");
                        }
                        else {
                            loadDataTable("all");
                        }
                    }
                }
            }
        }
    }
    
})
