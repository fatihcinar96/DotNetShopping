$(document).ready(function () {
    $('.searchInput').select2({
        ajax: {
            url: '/Api/Search',
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    keyword: params.term
                };
            },
            processResults: function (data, params) {
                return {
                    results: data.results
                };
            },
            cache: true
        },
        placeholder: 'Search for a product',
        escapeMarkup: function (markup) { return markup; },
        minimumInputLength: 1,
        templateResult: formatRepo,
        templateSelection: formatRepoSelection
    });
    $('.searchInput').on('select2:select', function (e) {
        //alert('selected' + $('.searchInput').val());
        window.location = '/p/' + $('.searchInput').val();
    });
});

function formatRepo(data) {
    if (data.loading) {
        return data.text;
    }

    var markup = '<div class="search-item">' +
        '<div class="search-item-image"><img src="/ProductImage/' + data.image + '-1.jpg" /></div>' +
        '<div class="search-item-data">' +
        '<div class="search-item-title">' + data.text + '</div>' +
        '<div class="search-item-price">Only $' + data.UnitPrice + '</div>' +
        '<div class="search-item-stock">' + data.Stock + ' in stock</div>' +
        '</div>';
    return markup;
}

function formatRepoSelection(data) {
    return data.text;
}

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