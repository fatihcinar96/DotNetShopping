function loadShoppingCart(){

    $.post('Api/GetShoppingCart')
        .done(function (response, status, jqxhr) {
            displayShoppingCart(response['Cart']);

        })
        .fail(function (jqxhr, status, error) {
            alert('Error!');
        });
}

function displayShoppingCart(cart) {
    var qty = 0;
    for (var i = 0; i < cart.length; i++) {
        qty += cart[i]['Quantity'];
    }
    $('#cartQty').html(qty);
}

function addToCart(variantId,qty) {
    var dataToPost = {
        VariantId: variantId,
        Qty: qty
    };
    $.post('/Api/AddToCart', dataToPost)
        .done(function (response, status, jqxhr) {
            // this is the "success" callback
            //alert(response['userId']);
        })
        .fail(function (jqxhr, status, error) {
            // this is the ""error"" callback
            alert('Error!');
        });
}