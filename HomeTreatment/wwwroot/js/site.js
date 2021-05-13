// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

//Write your JavaScript code.
$("#services").click(function () {
    $('html, body').animate({
        scrollTop: $("#servicesInfo").offset().top
    }, 1000);
});

$("#logoImg").click(function () {
    window.location.href = '/Home'
});


jQueryAjaxPost = form =>{
    $.ajax({
        type: 'POST',
        url: form.action,
        data: new FormData(form),
        contentType: false,
        processData: false,
        success: function (res) {
            console.log(res);
        },
        error: function (err) {
            console.log(err);
        }
    });

    return false;
}

//function myFunction() {
//    var input, filter, table, tr, td, i, txtValue;
//    input = document.getElementById("myInput");
//    filter = input.value.toUpperCase();
//    table = document.getElementById("table");
//    tr = table.getElementsByTagName("tr");
//    for (i = 0; i < tr.length; i++) {
//        td = tr[i].getElementsByTagName("td")[0];
//        if (td) {
//            txtValue = td.textContent || td.innerText;
//            if (txtValue.toUpperCase().indexOf(filter) > -1) {
//                tr[i].style.display = "";
//            } else {
//                tr[i].style.display = "none";
//            }
//        }
//    }
//}