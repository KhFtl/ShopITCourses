$(document).ready(function () {
    CKEDITOR.replace('Description',
        {
            filebrowserUploadUrl: "/Ckeditor/Upload",
            filebrowserBrowseUrl: "/Ckeditor/FileBrowser",
            //filebrowserWindowWidth: '1000',
            //filebrowserWindowHeight: '700'
        });
});