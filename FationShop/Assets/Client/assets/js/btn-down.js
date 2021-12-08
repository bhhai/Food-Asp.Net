// Btn down

var downBtns = document.querySelectorAll(".new__title");
var itemView = document.querySelectorAll(".new__list");

var itemHidden = function(item) {
    itemView[item].classList.toggle('item__active');
}

downBtns.forEach((downBtn, i) => {
    downBtn.addEventListener("click", () => {
        itemHidden(i);
    })
})