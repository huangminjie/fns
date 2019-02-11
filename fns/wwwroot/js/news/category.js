$(function () {
    search();
});
function add(){
    var category=prompt("请输入要添加的类别","")
    if (category!= null && category != "")
    {
        var data = {
            name: category
        };
        $.ajax({
            url: '/News/SaveCategory',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            success: function (data, status) {
                if (data.ok) {
                    search();
                }
                else {
                    alert(data.message);
                }
            }
        })
    }
}
function search() {
    $.ajax({
        url: '/News/GetCategoryList',
        type: 'GET',
        success: function (data, status) {
            if (data.ok) {
                $("tbody").empty();
                data.resData.forEach((item) => {
                    var tr = `
                    <tr>
                        <td>${item.id}</td>
                        <td>${item.name}</td>
                        <td style="display: flex;">
                            <a href="javascript:void(0);" onclick="remove(${item.id})"><i class="fa fa-remove fa-fw"></i></a>
                        </td>
                    </tr>
                    `;
                    $("tbody").append(tr);
                });
            }
            else {
                alert(data.message);
            }
        }
    })
}
function remove(id) {
    var r = confirm("确认删除?");
    if (r == true) {
        $.ajax({
            url: '/News/DeleteCategory',
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ id: id }),
            success: function (data, status) {
                if (data.ok) {
                    search();
                }
                else {
                    alert(data.message);
                }
            }
        })
    }
}