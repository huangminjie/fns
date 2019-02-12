$(function () {
    search(0);
});
function search(index) {
    var data = {
        pi: index === 0?1:index,
        ps: 10
    }
    $.ajax({
        url: '/User/GetList',
        type: 'GET',
        data: data,
        success: function (data, status) {
            if (data.ok) {
                $("tbody").empty();
                data.resData.data.forEach((item) => {
                    var tr = `
                    <tr>
                        <td>${item.id}</td>
                        <td>${item.name}</td>
                        <td>
                            <img src="${item.avatar}" style="height: 60px;width: 60px;" />
                        </td>
                        <td>${item.gender}</td>
                        <td>${item.status}</td>
                        <td>${item.insDt}</td>
                        <td>${item.birthday}</td>
                        <td style="display: flex;">
                            <a href="javascript:void(0);" onclick="remove(${item.id})"><i class="fa fa-remove fa-fw"></i></a>
                        </td>
                    </tr>
                    `;
                    $("tbody").append(tr);
                });
                if(data.resData.total > 0){
                    if(index === 0){
                        $('#paginationUserList.pagination').jqPaginator({
                            totalCounts: data.resData.total,
                            pageSize: data.resData.ps,
                            currentPage: data.resData.pi,
                            onPageChange: function (num, type) {
                                search(num);
                            }
                        });
                    }
                    else{
                        $('#paginationUserList.pagination').jqPaginator('option', {
                            currentPage: index
                        });
                    }
                }
            }
            else {
                alert(data.message);
            }
        }
    })
}
function remove(id){
    var r = confirm("确认删除?");
    if (r == true) {
        $.ajax({
            url: '/User/Delete',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                id:id
            }),
            success: function (data, status) {
                if (data.ok) {
                    search(1);
                }
                else {
                    alert(data.message);
                }
            }
        })
    }
}