// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//$(document).ready(function () {
//    $('.preloader').show()
//    $(window).on('load', function () {
//        $('.preloader').hide()
//    });
//});

document.addEventListener("DOMContentLoaded", function () {
    console.log("DOMContentLoaded");
    document.querySelector(".preloader").style.display = "none";
});

document.onreadystatechange = function () {
    if (document.readyState === "complete") {
        console.log("readystate complete");
        document.querySelector(".preloader").style.display = "none";
    }
};