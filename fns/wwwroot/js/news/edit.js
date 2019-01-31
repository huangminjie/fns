var serverUrl = 'http://47.99.103.201:8011';
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
            insertImg(url);
            picsList.push(url);
            console.log("customInsert");
            console.log(picsList);
        });
    }
}
editorNews.customConfig.linkImgCallback = function (url) {
    picsList.push(url);// url 即插入图片的地址
    console.log("linkImgCallback");
    console.log(picsList);
}
editorNews.create();
editorNews.txt.html($("#txtNewsDetail").val());

//加载预览页面
$("#divNewsPreview").html($("#txtNewsDetail").val());

function returnNewsList() {
    $("#divEditNews").html("");
    $("#divNewsList").css("display", "");
    editorNews.txt.clear();
}

function submitNews() {
    console.log(picsList);
    $.ajax({
        url: '/News/SaveNews',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "Title": $("#fld-title").val(), "Content": editorNews.txt.html(), "DoRef": $("#fld-doref").val(), "PicUrlList": picsList }),
        async: true,
        success: function (data, status) {
            $("#divEditNews").html(data);
            $("#divNewsList").css("display", "none");
        }
    }); //ajax

}