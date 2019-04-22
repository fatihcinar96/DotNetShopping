function statusButtonClick(sender, orderId) {
    var shippingCode = '';
    if (sender.value === '3') {
        shippingCode = $('#ShippingCode').val();
    }
    var dataToPost = {
        orderId: orderId,
        status: sender.value,
        shippingCode: shippingCode
    };

    $.post('/Order/UpdateStatus', dataToPost)
        .done(function (response, status, jqxhr) {
            if (response['Success'] === true) {
                location.reload();
            } else {
                alert(response['Error']);
            }
        })
        .fail(function (jqxhr, status, error) {
            // this is the ""error"" callback
            alert('post fail Error!');
        });

};


function saveButtonClick(orderId) {
    var shippingCode = $('#ShippingCode').val();

    var dataToPost = {
        orderId: orderId,
        status: $('#OrderStatus').val(),
        shippingCode: shippingCode
    };

    $.post('/Order/UpdateStatus', dataToPost)
        .done(function (response, status, jqxhr) {
            if (response['Success'] === true) {
                location.reload();
            } else {
                alert(response['Error']);
            }
        })
        .fail(function (jqxhr, status, error) {
            alert(error);
        });
};
