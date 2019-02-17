var serverUrl = $("#txtServerPath").val();

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
// 隐藏“网络图片”tab
editorNews.customConfig.showLinkImg = false;
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
            //picsList.push(url);
        });
    }
}
editorNews.customConfig.linkImgCallback = function (url) {
    //picsList.push(url);// url 即插入图片的地址
}

editorNews.customConfig.onchange = function (html) {
    //加载预览页面
    $("#divNewsPreview").html(html);//$("#txtNewsDetail").val());
}
editorNews.create();
editorNews.txt.html($("#txtNewsDetail").val());


function returnNewsList() {
    $("#divEditNews").html("");
    $("#divNewsList").css("display", "");
    editorNews.txt.clear();
}

function submitNews() {

    if ($("#cid").val() == "") {
        alert("类目不能为空！");
        return;
    }
    if ($("#fld-title").val() == "") {
        alert("标题不能为空！");
        return;
    }
    if ($("#fld-auth").val() == "") {
        alert("作者不能为空！");
        return;
    }


    var content = editorNews.txt.html();
    if ($("#fld-context-0").is(":checked")) {
        if (editorNews.txt.html().trim() == "<p><br></p>") {
            alert("内容不能为空！");
            return;
        }
    } else {
        if ($("#fld-doref").val().trim() == "") {
            alert("新闻外链不能为空！");
            return;
        }
    }
    if (content == "<p><br></p>") { content = "";}

    //由于我们只允许本地上传图片至服务器，目前所有我们添加的图片格式为<img src="/upload/news/" style="max-width:100%;">
    $.each($('#divNewsContent #editor').find("img"), function (i, val) {
        var imgSrc = $(val).attr("src");
        if (imgSrc.indexOf("/upload/news/") != -1)
            picsList.push(imgSrc);
    }); 

    $.ajax({
        url: '/News/SaveNews',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ "id": $("#fld-id").val(), "cid": $("#cid").val(), "title": $("#fld-title").val(), "auth": $("#fld-auth").val(), "content": content, "doRef": $("#fld-doref").val(), "picUrlList": picsList }),
        async: true,
        success: function (data, status) {
            refreshNews();
        }
    }); //ajax

}

function refreshNews() {    
    $("#divNewsList").parent().load("/News/Lists");
}


function loadPreView() {
    $("#preTitle").html($("#fld-title").val());
    $("#preAuthor").html($("#fld-auth").val());
    $("#divNewsPreview").html($("#txtNewsDetail").val());
}
$(function () {

    $("#fld-title").bind("change", function () {
        $("#preTitle").html($(this).val());
    });
    $("#fld-auth").bind("change", function () {
        $("#preAuthor").html($(this).val());
    });

    $("#fld-context-0").bind("click", function () {
        if ($("#fld-doref").val().length > 0) {
            if (confirm("您的操作将清空已添加的新闻外链，是否继续？")) {
                $("#fld-context-0").prop("checked", "checked");
                $("#fld-context-1").removeProp("checked");
                $("#fld-doref").val("");
                $("#divNewsContent").css("display", "block");
                $("#divPreview").css("display", "block");
                $("#divNewsDoRef").css("display", "none");
            }
            else {
                $("#fld-context-0").removeProp("checked");
                $("#fld-context-1").prop("checked", "checked");
            }
        }
        else {
            $("#fld-context-0").prop("checked", "checked");
            $("#fld-context-1").removeProp("checked");
            $("#fld-doref").val("");
            $("#divNewsContent").css("display", "block");
            $("#divPreview").css("display", "block");
            $("#divNewsDoRef").css("display", "none");
        }
    });

    $("#fld-context-1").bind("click", function () {
        if (editorNews.txt.html() != "<p><br></p>") {
            if (confirm("您的操作将清空已添加的新闻内容，是否继续？")) {
                $("#fld-context-0").removeProp("checked");
                $("#fld-context-1").prop("checked", "checked");
                editorNews.txt.html("");
                $("#divNewsDoRef").css("display", "block");
                $("#divNewsContent").css("display", "none");
                $("#divPreview").css("display", "none");
                $("#preTitle").html($("#fld-title").val());
                $("#preAuthor").html($("#fld-auth").val());
                $("#divNewsPreview").html("");
            }
            else {
                $("#fld-context-0").prop("checked", "checked");
                $("#fld-context-1").removeProp("checked");
            }
        }
        else {
            $("#fld-context-0").removeProp("checked");
            $("#fld-context-1").prop("checked", "checked");
            editorNews.txt.html("");
            $("#divNewsDoRef").css("display", "block");
            $("#divNewsContent").css("display", "none");
            $("#divPreview").css("display", "none");
            $("#preTitle").html($("#fld-title").val());
            $("#preAuthor").html($("#fld-auth").val());
            $("#divNewsPreview").html("");
        }
    });

    console.log("isInnerNews  "+isInnerNews);
    if (isInnerNews == "True") {
        loadPreView();
        $("#fld-context-0").prop("checked", "checked");
        $("#fld-context-0").click();
    }
    else {
        $("#fld-context-1").prop("checked", "checked");
        $("#fld-context-1").click();
    }
})