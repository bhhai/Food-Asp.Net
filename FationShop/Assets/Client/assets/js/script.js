var slideIndex = 1;
showSlides(slideIndex);


// Thumbnail image controls
function currentSlide(n) {
    showSlides(slideIndex = n);
}

function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    var dots = document.getElementsByClassName("dot");
    if (n > slides.length) { slideIndex = 1 }
    if (n < 1) { slideIndex = slides.length }
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[slideIndex - 1].style.display = "block";
    dots[slideIndex - 1].className += " active";
}









// Modal

var popupViews = document.querySelectorAll('.product__modal');
var popupBtns = document.querySelectorAll('.icon__hover3');
var closeBtns = document.querySelectorAll('.modal__close');
var overlay = document.querySelector(".product__overlay");

//javascript for quick view button
var popup = function (popupClick) {
    popupViews[popupClick].classList.add('modal__active');
    overlay.classList.add("modal__active");
    document.getElementsByTagName("body")[0].style.overflow = "hidden";

    // Change img in popup
    var childrens = popupViews[popupClick].querySelector(".modal__change-img").children;
    var imgMain = popupViews[popupClick].querySelector(".pro__main-im");
    var imgDiv = popupViews[popupClick].querySelectorAll(".modal__ch");

    imgDiv.forEach((item) => {
        item.addEventListener("click", function (event) {
            imgMain.src = event.target.src;
        })
    })

}

popupBtns.forEach((popupBtn, i) => {
    popupBtn.addEventListener("click", () => {
        popup(i);
    });
});


// overlay button
overlay.addEventListener("click", () => {
    overlay.classList.remove("modal__active");
    popupViews.forEach((popupView) => {
        popupView.classList.remove('modal__active');
        overlay.classList.remove("modal__active");
        document.getElementsByTagName("body")[0].style.overflow = "auto";
    });
})

//javascript for close button
closeBtns.forEach((closeBtn) => {
    closeBtn.addEventListener("click", () => {
        popupViews.forEach((popupView) => {
            popupView.classList.remove('modal__active');
            overlay.classList.remove("modal__active");
            document.getElementsByTagName("body")[0].style.overflow = "auto";
        });
    });
});




var slider = document.getElementById('brand-slider');
var btnRight = document.getElementById('btn-right');
var btnLeft = document.getElementById('btn-left');

btnRight.addEventListener('click', function () {
    slider.scrollLeft += slider.scrollWidth / 8;
})

btnLeft.addEventListener('click', function () {
    slider.scrollLeft -= slider.scrollWidth / 8;
})