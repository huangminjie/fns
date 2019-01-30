function openModal(data) {
    if (data) {
        $("#modalTitle").html("修改");
    }
    else {
        $("#modalTitle").html("新增");
    }
    $('#myModal').modal('show');
    console.log(123);
}
function save() {
    var redirectUrl = $("#redirectUrl").val();
    var duration = $("#duration").val();

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
    formData.append('type', 'splash');
    var serverUrl = 'https://localhost:5001';
    $.ajax({
        url: serverUrl + '/picture/uploadpicture',
        type: 'POST',
        cache: false,
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            console.log(data)
        },
        error: function (data) {
            this.alert("上传失败");
        }
    })
}
