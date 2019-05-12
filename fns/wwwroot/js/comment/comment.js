$(function () {
    search(0);
});
function search(index) {
    var data = {
        pi: index === 0 ? 1 : index,
        ps: 10
    };
    $.ajax({
        url: '/Comment/GetList',
        type: 'GET',
        data: data,
        success: function (data, status) {
            if (data.ok) {
                $("tbody").empty();
                data.resData.data.forEach((item) => {
                    var tr = `
                    <tr>
                        <td>${item.id}</td>
                        <td>${item.user}</td>
                        <td>${item.news}</td>
                        <td>${item.content}</td>
                        <td>${item.status}</td>
                        <td>${item.insDt}</td>
                        <td style="display: flex;">
                            <a href="javascript:void(0);" onclick="changeStatus(${item.id},'${item.status}')">变更状态</a>
                        </td>
                    </tr>
                    `;
                    $("tbody").append(tr);
                });
                if (data.resData.total > 0) {
                    if (index === 0) {
                        $('#paginationUserList.pagination').jqPaginator({
                            totalCounts: data.resData.total,
                            pageSize: data.resData.ps,
                            currentPage: data.resData.pi,
                            onPageChange: function (num, type) {
                                search(num);
                            }
                        });
                    }
                    else {
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
    });
}
function changeStatus(id,status) {
    var text = "";
    var isNormal = true;
    if (status === "正常") {
        text = "确认标记该评论为违规？";
        isNormal = false;
    }
    else {
        text = "确认标记该评论为正常？";
        isNormal = true;
    }
    var r = confirm(text);
    if (r === true) {
        $.ajax({
            url: '/Comment/Audit',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                id: id,
                isNormal: isNormal
            }),
            success: function (data, status) {
                if (data.ok) {
                    alert("操作成功！");
                    search(1);
                }
                else {
                    alert(data.message);
                }
            }
        });
    }
}