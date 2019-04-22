$(document).ready(function () {
    testValues();

    $("#BillingCountryId").change(function () {
        billingCountryChanged(0, 0);
    });
    $("#BillingStateId").change(function () {
        var countryId = $('#BillingCountryId').val();
        var stateId = this.value;
        if (stateId > 0 && countryId > 0) {
            fillCities($('#BillingCityId'), countryId, stateId, 0, false);
        }
        else {
            $('#BillingCityId').append($("<option />").val(0).text('Select Country/State'));
        }
    });
    $("#ShippingCountryId").change(function () {
        shippingCountryChanged(0, 0);
    });
    $("#ShippingStateId").change(function () {
        var countryId = $('#ShippingCountryId').val();
        var stateId = this.value;
        if (stateId > 0 && countryId > 0) {
            fillCities($('#ShippingCityId'), countryId, stateId, 0, false);
        }
        else {
            $('#ShippingCityId').append($("<option />").val(0).text('Select Country/State'));
        }
    });
    $("#ShippingMethodId").change(function () {
        if ($(this).val() > 0) {
            var cost = $(this).find(':selected').attr('data-cost');
            $('#ShippingCost').html('$' + cost);
            $('#shipping-total').html('$' + cost);
            $('#shippingbox6').html('Shipping: '+$('#ShippingMethodId option:selected').text() + ' $' + cost);
        }
        else {
            $('#ShippingCost').html('');
        }
    });
    $("#PaymentMethodId").change(function () {

        var paymentMethodId = parseInt($(this).val());
        if (paymentMethodId > 0) {
            if (paymentMethodId === 1 || paymentMethodId === 2) {
                $('.payment-creditcard').show(200);
                $('.payment-paypal').hide(200);
            }
            else {
                $('.payment-creditcard').hide(200);
                if (paymentMethodId === 6) {
                    $('.payment-paypal').show(200);
                }
                else {
                    $('.payment-paypal').hide(200);
                }
            }

            var totalPrice = parseFloat($('#totalPrice').val().replace(',', '.'));
            var cost = parseFloat($('#ShippingMethodId').find(':selected').attr('data-cost').replace(',', '.'));
            var discount = parseFloat($(this).find(':selected').attr('data-discount').replace(',', '.'));
            //alert('totalPrice:' + totalPrice + ' discount:' + discount + ' cost:' + cost);
            var grandTotal = 0;
            if (discount > 0) {
                grandTotal = cost + totalPrice - discount;
                $('#PaymentDiscount').html('$' + amount.toFixed(2));
                $('#discount-total').html('$' + amount.toFixed(2));
                $('#grand-total').html('$' + grandTotal.toFixed(2));
                $('.PaymentDiscount').show(200);
            }
            else {
                $('.PaymentDiscount').hide(200);
                $('#discount-total').html('$0.00');
                grandTotal = cost + totalPrice;
                $('#grand-total').html('$' + grandTotal.toFixed(2));
            }
        }
        else {
            $('.payment-creditcard').hide(200);
            $('.payment-paypal').hide(200);
            $('.PaymentDiscount').hide(200);
            $('#PaymentDiscount').html('');
            $('#discount-total').html('$0.00');
            $('#grand-total').html('$0.00');
        }
        $('#PaymentInfo').html($(this).find(':selected').attr('data-info'));
        //$('#myform').validate().resetForm();
    });
    $('#PlaceOrder').click(function () {
        if ($('#conditions').prop('checked')) {
            return true;
        }
        else {
            alert('You have to agree terms and conditions to place an order.');
            return false;
        }
    });
});
function billingCountryChanged(selectedStateId, selectedCityId) {
    var countryId = $("#BillingCountryId").val();
    //alert('countryId:' + countryId);
    $('#BillingStateId').empty();
    $('#BillingCityId').empty();

    if (countryId > 0) {
        var dataToPost = {
            CountryId: countryId
        };
        $.post('/Api/GetStatesFor', dataToPost)
            .done(function (response, status, jqxhr) {
                // this is the "success" callback
                if (response['States'].length > 0) {
                    $('#BillingCityId').append($("<option />").val(0).text('Select Country/State'));
                    $('#BillingStateId').prop("disabled", false);
                    $('#BillingStateId').append($("<option />").val(0).text('Select State'));
                    $.each(response['States'], function () {
                        $('#BillingStateId').append($("<option />").val(this.StateId).text(this.Name));
                    });
                    if (selectedStateId > 0) {
                        $('#BillingStateId').val(selectedStateId);
                        fillCities($('#BillingCityId'), countryId, selectedStateId, selectedCityId, false);
                    }
                }
                else {
                    $('#BillingStateId').append($("<option />").val(0).text('No State'));
                    $('#BillingStateId').prop("disabled", true);
                    fillCities($('#BillingCityId'), countryId, 0, 0, false);
                }
            })
            .fail(function (jqxhr, status, error) {
                // this is the ""error"" callback
                alert('Error!');
            });
        fillPaymentMethods(countryId);
    }
    else {
        $('#BillingStateId').append($("<option />").val(0).text('Select Country'));
        $('#BillingStateId').prop("disabled", false);
    }
}
function shippingCountryChanged(selectedStateId, selectedCityId) {
    var countryId = $('#ShippingCountryId').val();
    //alert('countryId:' + countryId);
    $('#ShippingStateId').empty();
    $('#ShippingCityId').empty();

    if (countryId > 0) {
        var dataToPost = {
            CountryId: countryId
        };
        $.post('/Api/GetStatesFor', dataToPost)
            .done(function (response, status, jqxhr) {
                // this is the "success" callback
                if (response['States'].length > 0) {
                    $('#ShippingCityId').append($("<option />").val(0).text('Select Country/State'));
                    $('#ShippingStateId').prop("disabled", false);
                    $('#ShippingStateId').append($("<option />").val(0).text('Select State'));
                    $.each(response['States'], function () {
                        $('#ShippingStateId').append($("<option />").val(this.StateId).text(this.Name));
                    });
                    if (selectedStateId > 0) {
                        $('#ShippingStateId').val(selectedStateId);
                        fillCities($('#ShippingCityId'), countryId, selectedStateId, selectedCityId, true);
                    }
                }
                else {
                    $('#ShippingStateId').append($("<option />").val(0).text('No State'));
                    $('#ShippingStateId').prop("disabled", true);
                    fillCities($('#ShippingCityId'), countryId, 0, selectedCityId, true);
                }
            })
            .fail(function (jqxhr, status, error) {
                // this is the ""error"" callback
                alert('Error!');
            });
        fillShippingMethods(countryId);
    }
    else {
        $('#ShippingStateId').append($("<option />").val(0).text('Select Country'));
        $('#ShippingStateId').prop("disabled", false);
    }
}
function fillCities(city, countryId, stateId, selectedCityId, fillShippingNeeded) {
    var dataToPost = {
        CountryId: countryId,
        StateId: stateId
    };
    //alert('select city:' + selectedCityId);
    //alert('PostData:' + countryId + ' ' + stateId);
    $.post('/Api/GetCitiesFor', dataToPost)
        .done(function (response, status, jqxhr) {
            if (response['Cities'].length > 0) {
                city.empty();
                city.append($("<option />").val(0).text('Select City'));
                $.each(response['Cities'], function () {
                    city.append($("<option />").val(this.CityId).text(this.Name));
                });
                if (selectedCityId > 0) {
                    city.val(selectedCityId);
                    if (fillShippingNeeded) {
                        fillShippingBox();
                    }
                }
            }
        })
        .fail(function (jqxhr, status, error) {
            // this is the ""error"" callback
            alert('Error!');
        });
}
function fillShippingMethods(countryId) {
    var weight = parseFloat($('#weight').val().replace(',', '.'));
    //alert('weight:' + weight);
    var dataToPost = {
        CountryId: countryId
    };
    $.post('/Api/GetShippingMethods', dataToPost)
        .done(function (response, status, jqxhr) {
            $('#ShippingMethodId').empty();
            $('#ShippingMethodId').append($("<option />").val('').text('Select ShippingMethod'));
            $.each(response['ShippingMethods'], function () {
                var cost = 0;
                if (weight <= 0.5) {
                    cost = this.CostHalf;
                } else if (weight <= 1) {
                    cost = this.CostOne;
                } else if (weight <= 1.5) {
                    cost = this.CostOneHalf;
                } else if (weight <= 2) {
                    cost = this.CostTwo;
                } else if (weight > 2) {
                    cost = this.CostTwoHalf;
                }
                $('#ShippingMethodId').append($("<option />").val(this.ShippingMethodId).text(this.Name).attr('data-cost', cost));
            });
        })
        .fail(function (jqxhr, status, error) {
            // this is the ""error"" callback
            alert('Error!');
        });
}
function fillPaymentMethods(countryId) {
    var dataToPost = {
        CountryId: countryId
    };
    $.post('/Api/GetPaymentMethods', dataToPost)
        .done(function (response, status, jqxhr) {
            $('#PaymentMethodId').empty();
            $('#PaymentMethodId').append($("<option />").val('').text('Select PaymentMethod'));
            $.each(response['PaymentMethods'], function () {
                $('#PaymentMethodId').append($("<option />").val(this.PaymentMethodId).text(this.Name).attr('data-discount', this.PaymentDiscount).attr('data-info', this.PaymentInfo));
            });
        })
        .fail(function (jqxhr, status, error) {
            // this is the ""error"" callback
            alert('Error!');
        });
}
function billingNextClick() {
    $('#myform').validate({ // initialize the plugin
        // rules & options
    });
    if ($('#myform').valid()) {
        var value = parseInt($('input:radio[name=billingradio]:checked').val());
        //alert('value:' + value);
        $('#checkout-step-billing').hide(200);
        if (value === 1) {
            $('#checkout-step-shipping').show(200);
        }
        else {
            copyAddress();
            $('#checkout-step-shipping_method').show(200);
        }
        fillBillingBox();
    }
}
function shippingNextClick() {
    $('#myform').validate({ // initialize the plugin
        // rules & options
    });
    if ($('#myform').valid()) {
        $('#checkout-step-shipping').hide(200);
        $('#checkout-step-shipping_method').show(200);
        fillShippingBox();
    }
}
function shippingMethodNextClick() {
    $('#myform').validate({ // initialize the plugin
        // rules & options
    });
    if ($('#myform').valid()) {
        $('#checkout-step-shipping_method').hide(200);
        $('#checkout-step-payment').show(200);

    }
}
function paymentNextClick() {
    $('#myform').validate({ // initialize the plugin
        // rules & options

    });
    if ($('#myform').valid()) {
        $('#checkout-step-payment').hide(200);
        $('#checkout-step-review').show(200);
    }
}
function billingTitleClick() {
    $('#checkout-step-billing').show(200);
    $('#checkout-step-shipping').hide(200);
    $('#checkout-step-shipping_method').hide(200);
    $('#checkout-step-payment').hide(200);
    $('#checkout-step-review').hide(200);
}
function shippingTitleClick() {
    $('#checkout-step-billing').hide(200);
    $('#checkout-step-shipping').show(200);
    $('#checkout-step-shipping_method').hide(200);
    $('#checkout-step-payment').hide(200);
    $('#checkout-step-review').hide(200);
}
function shippingMethodTitleClick() {
    $('#checkout-step-billing').hide(200);
    $('#checkout-step-shipping').hide(200);
    $('#checkout-step-shipping_method').show(200);
    $('#checkout-step-payment').hide(200);
    $('#checkout-step-review').hide(200);
}
function paymentTitleClick() {
    $('#checkout-step-billing').hide(200);
    $('#checkout-step-shipping').hide(200);
    $('#checkout-step-shipping_method').hide(200);
    $('#checkout-step-payment').show(200);
    $('#checkout-step-review').hide(200);
}
function reviewClick() {
    $('#checkout-step-billing').hide(200);
    $('#checkout-step-shipping').hide(200);
    $('#checkout-step-shipping_method').hide(200);
    $('#checkout-step-payment').hide(200);
    $('#checkout-step-review').show(200);
}
function copyAddress() {
    $('#ShippingFirstName').val($('#BillingFirstName').val());
    $('#ShippingLastName').val($('#BillingLastName').val());
    $('#ShippingCompany').val($('#BillingCompany').val());
    $('#ShippingStreet1').val($('#BillingStreet1').val());
    $('#ShippingStreet2').val($('#BillingStreet2').val());
    $('#ShippingCountryId').val($('#BillingCountryId').val());
    shippingCountryChanged($('#BillingStateId').val(), $('#BillingCityId').val());
    $('#ShippingZip').val($('#BillingZip').val());
    $('#ShippingTelephone').val($('#BillingTelephone').val());
}
function testValues() {
    $('#BillingFirstName').val('Fatih');
    $('#BillingLastName').val('Cinar');
    $('#BillingCompany').val('Microsoft');
    $('#BillingStreet1').val('CincinSt');
    $('#BillingStreet2').val('Esenler');
    $('#BillingCountryId').val(1);
    billingCountryChanged(2, 8);
    $('#BillingZip').val('34200');
    $('#BillingTelephone').val('5070728003');
    $('#BillingEmail').val('fatihcinar96@gmail.com');
}
function fillBillingBox() {
    $('#billingbox1').html($('#BillingFirstName').val() + ' ' + $('#BillingLastName').val());
    $('#billingbox2').html($('#BillingCompany').val());
    $('#billingbox3').html($('#BillingStreet1').val() + ', ' + $('#BillingStreet2').val());
    $('#billingbox4').html($('#BillingCityId option:selected').text() + ', ' + $('#BillingStateId option:selected').text() + ', ' + $('#BillingCountryId option:selected').text());
    $('#billingbox5').html('Zip: ' + $('#BillingZip').val() + ' Phone: ' + $('#BillingTelephone').val());
    $('#billingbox6').html($('#BillingEmail').val());
    $('#billingbox').show(200);
}
function fillShippingBox() {
    $('#shippingbox1').html($('#ShippingFirstName').val() + ' ' + $('#ShippingLastName').val());
    $('#shippingbox2').html($('#ShippingCompany').val());
    $('#shippingbox3').html($('#ShippingStreet1').val() + ', ' + $('#ShippingStreet2').val());
    $('#shippingbox4').html($('#ShippingCityId option:selected').text() + ', ' + $('#ShippingStateId option:selected').text() + ', ' + $('#ShippingCountryId option:selected').text());
    $('#shippingbox5').html('Zip: ' + $('#ShippingZip').val() + ' Phone: ' + $('#ShippingTelephone').val());
    $('#shippingbox').show(200);
}