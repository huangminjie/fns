var serverUrl = 'https://localhost:5001';
var picsList = [];
//实例化编辑器
var E = window.wangEditor;
var editorNews = new E('#divNewsContent #editor');
// 上传图片到服务器
editorNews.customConfig.uploadImgServer = serverUrl + '/picture/uploadpicture';
// 将图片大小限制为 5M
editorNews.customConfig.uploadImgMaxSize = 5 * 1024 * 1024;
// 限制一次最多上传 1 张图片
editorNews.customConfig.uploadImgMaxLength = 5;
// 附加参数
editorNews.customConfig.uploadImgParams = {
    type: 'news'
};
//上传文件的参数名
//editorNews.customConfig.uploadFileName = 'file';
//超时时间
editorNews.customConfig.uploadImgTimeout = 30000;
editorNews.customConfig.uploadImgHooks = {
    customInsert: function (insertImg, result, editor) {
        result.resData.forEach(url => {
            insertImg(serverUrl + url);
        });
    }
}
editorNews.create();
editorNews.txt.html('@Model.content');

function returnNewsList() {
    $("#divEditNews").html("");
    $("#divNewsList").css("display", "");
    editorNews.txt.clear();
}

function submitNews() {
    console.log($("#fld-title").val());
    console.log($("#fld-doref").val());
    $.ajax({
        url: '/News/SaveNews',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "Title": $("#fld-title").val(), "Content": editorNews.txt.html(), "DoRef": $("#fld-doref").val() }),
        async: true,
        success: function (data, status) {
            $("#divEditNews").html(data);
            $("#divNewsList").css("display", "none");
        }
    }); //ajax

}