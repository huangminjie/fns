var bannerList = [];
$(function () {
    search();
});
function search() {
    $.ajax({
        url: '/Banner/GetList',
        type: 'GET',
        success: function (data, status) {
            if (data.ok) {
                $("tbody").empty();
                bannerList = data.resData;
                data.resData.forEach((item) => {
                    var tr = `
                    <tr>
                        <td>${item.id}</td>
                        <td>
                            <img src="${item.picUrl}" style="height: 60px;width: 60px;" />
                        </td>
                        <td>${item.linkUrl}</td>
                        <td>${item.type == 1 ?"应用内":"应用外"}</td>
                        <td style="display: flex;">
                            <a href="javascript:void(0);" onclick="openModal(${item.id})"><i class="fa fa-pencil fa-fw"></i></a>
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
function openModal(id) {
    if (id) {
        var data = bannerList.find(o => o.id == id);
        $("#modalTitle").html("修改");
        $("#id").val(data.id);
        $("#linkUrl").val(data.linkUrl);
        $("#type").val(data.type);
        $("#picUrl").val(data.picUrl);
        $("#preview").prop("src", data.picUrl);
    }
    else {
        $("#id").val('');
        $("#linkUrl").val('');
        $("#type").val('1');
        $("#picUrl").val('');
        $("#preview").prop("src", null);
        $("#modalTitle").html("新增");
    }
    $('#myModal').modal('show');
}
function save() {
    var id = $("#id").val();
    var linkUrl = $("#linkUrl").val();
    var type = $("#type").val();
    var picUrl = $("#picUrl").val();
    if (picUrl === '') {
        alert("请选择图片!");
        return false;
    }
    var data = {
        id: id,
        linkUrl: linkUrl,
        type: type,
        picUrl: picUrl
    };
    $.ajax({
        url: '/Banner/Save',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        success: function (data, status) {
            if (data.ok) {
                search();
                $('#myModal').modal('hide');
            }
            else {
                alert(data.message);
            }
        }
    })
}
function imgPreview(fileDom) {
    if (window.FileReader) {
        var reader = new FileReader();
    } else {
        alert("您的设备不支持图片预览功能，如需该功能请升级您的设备！");
    }
    //获取文件
    var file = fileDom.files[0];
    var imageType = /^image\//;
    //是否是图片
    if (!imageType.test(file.type)) {
        alert("请选择图片！");
        return;
    }
    //读取完成
    reader.onload = function (e) {
        //获取图片dom
        var img = document.getElementById("preview");
        //图片路径设置为读取的图片
        img.src = e.target.result;
    };
    reader.readAsDataURL(file);
    var formData = new FormData();
    formData.append('file', file);
    formData.append('type', 'banner');
    $.ajax({
        url: '/Picture/UploadPicture',
        type: 'POST',
        cache: false,
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.ok) {
                var url = data.resData[0];
                $("#picUrl").val(url);
            }
        },
        error: function (data) {
            this.alert("上传失败");
        }
    });
}
function remove(id) {
    var r = confirm("确认删除?");
    if (r == true) {
        $.ajax({
            url: '/Banner/Delete',
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


