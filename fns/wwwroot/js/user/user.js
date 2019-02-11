$(function () {
    search(1);
});
function search(pi) {
    var data = {
        pi: pi,
        ps: 10
    }
    $.ajax({
        url: '/User/GetList',
        type: 'GET',
        data: JSON.stringify(data),
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
                $('#paginationUserList.pagination').jqPaginator('option', {
                    totalCounts: data.resData.total,
                    pageSize: data.resData.ps,
                    currentPage: data.resData.pi,
                    onPageChange: function (pi, type) {

                    }
                });
                //$('#pagination').jqPaginator('option', {
                //    currentPage: pi,
                //    totalCounts: data.resData.total
                //});
            }
            else {
                alert(data.message);
            }
        }
    })
}
