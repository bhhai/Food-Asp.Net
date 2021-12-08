var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnUpdate').off('click').on('click', function () {
            window.location.href = "/";
            console.log("clicked");
            var listProduct = $('#quantity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    Product: {
                        ID: $(item).data('id')
                    }
                })
            })
        });

        $.ajax({
            url: '/Cart/Update',
            data: { cartModel: JSON.stringify(cartList) },
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status == true) {
                    window.location.href = "/gio-hang";
                }
            }
        })
    }
}

cart.init();

console.log("alo")
