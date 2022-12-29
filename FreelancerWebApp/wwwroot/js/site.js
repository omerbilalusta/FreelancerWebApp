// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function randomNumber(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min
}

const STAR_COUNT = 100
let result = ""
for (let i = 0; i < STAR_COUNT; i++) {
    result += `${randomNumber(-50, 50)}vw ${randomNumber(-50, 50)}vh ${randomNumber(0, 3)}px ${randomNumber(0, 3)}px #fff,`
}
console.log(result.substring(0, result.length - 1))


let slideIndex = 1;
/*showSlides(slideIndex);*/

// Next/previous controls
function plusSlides(n) {
    showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);
}

//function showSlides(n) {
//    let i;
//    let slides = document.getElementsByClassName("mySlides");
//    let dots = document.getElementsByClassName("dot");
//    if (n > slides.length) { slideIndex = 1 }
//    if (n < 1) { slideIndex = slides.length }
//    for (i = 0; i < slides.length; i++) {
//        slides[i].style.display = "none";
//    }
//    for (i = 0; i < dots.length; i++) {
//        dots[i].className = dots[i].className.replace(" active", "");
//    }
//    slides[slideIndex - 1].style.display = "block";
//    dots[slideIndex - 1].className += " active";
//}

function fill() {
    const targetElement = document.getElementById(targetDeliverFilePath);
    const fromElement = document.getELementById(fromtDeliverFilePath)
    targetElement.value = fromElement.value;
    console.log("asd");
}



    //function showModal() {
    //    $('#modalContent').modal('show');
    //}

    //function hideModal() {
    //    $('#modalContent').modal('hide');
    //}

    //$('#btnShowModal').click(function () {
    //    // İş ID'sini alın
    //    var jobId = $(this).data('id');

    //// İş detaylarını sunucudan çekin
    //loadJobDetails(jobId);
    //console.log("asd");
    //// Modal pop-up'ı açın
    //showModal();
    //});
    //$('#btnCloseModal').click(function () {
    //    console.log("hayfdaa");
    //hideModal();
    //});

    //function loadJobDetails(jobId) {
    //    $.ajax({
    //        url: '/jobs/details/' + jobId, // Sunucudan verileri çekmek için göndereceğiniz isteğin URL'si
    //        type: 'GET', // İsteğin tipi (GET, POST, vb.)
    //        success: function (response) { // İsteğin başarılı olması durumunda yapılacak işlemler
    //            // İş detaylarını sayfanın içine yükleyin
    //        },
    //        error: function (error) { // İsteğin başarısız olması durumunda yapılacak işlemler
    //            // Hata mesajı gösterin
    //        }
    //    });
    //}

    //function showJobDetails(job) {
    //    $('#modalContent .modal-title').text(job.title); // İş başlığını göster
    //$('#modalContent .modal-body').html(job.description); // İş açıklamasını göster
    //}

