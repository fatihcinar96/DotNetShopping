function loadShoppingCart()
{
    $.post('/Api/GetShoppingCart')
        .done(function (response, status, jqxhr) {
            // this is the "success" callback
            displayShoppingCart(response['Cart']);
        })
        .fail(function (jqxhr, status, error) {
            // this is the ""error"" callback
            alert('Error!');
        });
}

function displayShoppingCart(cart) {
    var qty = 0;
    var totalPrice = 0;
    var cartContent = '';
    var cartTotalContent = '';

    for (var i = 0; i < cart.length; i++)
    {
        qty += parseInt(cart[i]['Quantity']);
        totalPrice += parseInt(cart[i]['Quantity']) * parseFloat(cart[i]['UnitPrice']);
        cartContent += getCartContent(cart[i]);
    }
    //alert('Adet:' + qty);
    $('#cartQty').html(qty);
    if (qty > 0) {
        cartTotalContent = getCartTotalContent(totalPrice);
    }
    else {
        cartContent = '<div class=\'cartItem\'>Your cart is empty.</div>';
    }
    $('#cartContent').html(cartContent + cartTotalContent);
}
function getCartContent(item) {
    var content = '<div class=\'cartItem\'>' +
        '<img src=\'../../ProductImage/' + item['PhotoName'] + '-1.jpg\'>' +
        '<div class=\'cartItemName\'>' + item['VariantName'] + ' ' + item['ProductName'] + '</div>' +
        '<div class=\'cartQuantity\'>' + item['Quantity'] + ' x $' + item['UnitPrice'] + '</div>' +
        '<i class=\'cartRemove glyphicon glyphicon-remove-circle\' onclick=\'removeCart(' + item['VariantId'] + ');\' title=\'Delete\'></i>' +
        '</div>';
    return content;
}
function getCartTotalContent(totalPrice) {
    var content = '<div class=\'cartTotal\'>Total: $' + totalPrice.toFixed(2) + '</div>' +
        '<div class=\'cartButtons\'>' +
        '<a class=\'btn btn-default\' href=\'../../Checkout/Cart\'><i class="icon-basket"></i>View Cart</a>' +
        '<a class=\'btn btn-default\' href=\'\../../Checkout/Checkout\'><i class="icon-right-thin"></i>Checkout</a>' +
        '<div class=\'clearer\'></div>' +
        '</div >';
    return content;
}
function removeCart(variantId) {
    //alert(variantId + ' removed!');
    var dataToPost = {
        VariantId: variantId
    };
    $.post('/Api/RemoveCart', dataToPost)
        .done(function (response, status, jqxhr) {
            // this is the "success" callback
            //alert(response['UserId']);
            displayShoppingCart(response['Cart']);
        })
        .fail(function (jqxhr, status, error) {
            // this is the ""error"" callback
            alert('Error!');
        });
}
function addToCart(variantId, qty) {
    var dataToPost = {
        VariantId: variantId,
        Qty: qty
    };
    $.post('/Api/AddToCart', dataToPost)
        .done(function (response, status, jqxhr) {
            // this is the "success" callback
            //alert(response['UserId']);
            displayShoppingCart(response['Cart']);
        })
        .fail(function (jqxhr, status, error) {
            // this is the ""error"" callback
            alert('Error!');
        });
}