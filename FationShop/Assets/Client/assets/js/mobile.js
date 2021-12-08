// Nav Moblie


var bar = document.querySelector(".popup__mobile");
var overlayMob = document.querySelector(".nav__overlay");

function Bar() {
    bar.classList.add("moblie__active");
    overlayMob.classList.add("moblie__active");
    document.getElementsByTagName("body")[0].style.overflow = "hidden";
}

function BarClose() {
    bar.classList.remove("moblie__active");
    overlayMob.classList.remove("moblie__active");
    document.getElementsByTagName("body")[0].style.overflow = "auto";
}

function MenuCap2() {
    var item = document.querySelector(".list__bar");
    item.classList.toggle("item__active")
}