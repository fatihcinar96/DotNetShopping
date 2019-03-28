function loadShoppingCart() {

    $.post('/Api/GetShoppingCart')
        .done(function (response, status, jqxhr) {
            displayShoppingCart(response['Cart']);

        })
        .fail(function (jqxhr, status, error) {
            //alert(error);
        });
}

function displayShoppingCart(cart) {
    var qty = 0;
    var cartContent = "";
    var totalPrice = 0;
    var cartTotalContent = '';
    for (var i = 0; i < cart.length; i++) {
        qty += parseInt(cart[i]['Quantity']);
        totalPrice += parseInt(cart[i]['Quantity']) * parseFloat(cart[i]['UnitPrice']);
        cartContent += getCartContent(cart[i]);
    }
    $('#cartQty').html(qty);
    cartTotalContent = getCartTotalContent(totalPrice);
    $('#cartContent').html(cartContent + cartTotalContent);
}

function addToCart(variantId, qty) {
    var dataToPost = {
        VariantId: variantId,
        Qty: qty
    };
    $.post('/Api/AddToCart', dataToPost)
        .done(function (response, status, jqxhr) {
            // this is the "success" callback
            //alert(response['userId']);
            displayShoppingCart(response['Cart']);

        })
        .fail(function (jqxhr, status, error) {
            // this is the ""error"" callback
            //alert('Error!');
        });
}

function getCartContent(item) {
    
    var content = '<div class=\'cartItem\'>' + '<img src=\'../../ProductImage/' + item['PhotoName'] + '-1.jpg\'>' +
        '<div class=\'cartItemName\'>' + item['VariantName'] + ' ' + item['ProductName'] + '</div>'
        + '<div class=\'cartQuantity\'>' + item['Quantity'] + 'X' + '</div>'
        + '<div class=\'cartPrice\'>' + item['UnitPrice'] + + ' ' + '$' + '</div>' +
        '<i class= \'cartRemove glyphicon glyphicon-remove-circle\' onclick =\'removeCart(' + item['VariantId'] + ');\'></i>'
        + '</div>';
    return content;
}

function getCartTotalContent(totalPrice) {
    var content = '<div class=\'cartTotal\'>Total:' + totalPrice + ' $' + '</div>' +
        '<div class=\'cartButtons\'>' +
        '<a class= \'btn btn-default\' href= \'../../Checkout/Cart\'><i class="icon-basket"></i>View Cart</a>' +
        '<a class= \'btn btn-default\' href= \'../../Checkout/Checkout\'><i class= "icon-right-thin"></i>Checkout</a>' +
        '<div class=\'clearer\'></div>' + '</div>';
    return content;
}

function removeCart(variantId) {
    var dataToPost = {
        VariantId: variantId
    };
    $.post('/Api/RemoveCart', dataToPost)
        .done(function (response, status, jqxhr) {
            // this is the "success" callback
            //alert(response['userId']);
            displayShoppingCart(response['Cart']);

        })
        .fail(function (jqxhr, status, error) {
            // this is the ""error"" callback
            alert(error);
        });
}


