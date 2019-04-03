$(document).ready(function () {
    $("#BillingCountryId").change(function () {
        var countryId = this.value;
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

                    }
                    else {
                        $('#BillingStateId').append($("<option />").val(0).text('No State'));
                        $('#BillingStateId').prop("disabled", true);
                        fillCities($('#BillingCityId'), countryId, 0);
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
    });

    //////

    $('#PaymentMethodId').change(function () {
        var discount = $(this).find(':selected').attr('data-discount');
        var hidden = parseInt($('#hiddenPayment').val());  
        var discPrice = (hidden * discount) / 100; 
        var lastPrice = hidden - discPrice;
        if ($(this).val() > 0) {
            if ($(this).val() == 1 || $(this).val() == 2) {
                $('.payment-credit-cart').show(200);
            } else {
                $('.payment-credit-cart').hide(200);
            }

            if ($(this).val() == 6) {
                $('.payment-paypal').show(200);
            } else {
                $('.payment-paypal').hide(200);
            }
           
            $('#PaymentDiscount').html('$' + lastPrice);
            $('.PaymentDiscount').show(200);
            $('#PaymentInfo').html($(this).find(':selected').attr('data-info'));
        } else {
            $('.PaymentDiscount').hide(200);
            $('#PaymentDiscount').html('');
            $('#PaymentInfo').hide(200);
            $('#PaymentInfo').html('');
        }
        
    })




    $("#ShippingMethodId").change(function () {
        var cost = $(this).find(':selected').attr('data-cost');
        $('#ShippingCost').html('$' + cost);
    })


    $("#ShippingCountryId").change(function () {
        var countryId = this.value;
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
                    }
                    else {
                        $('#ShippingStateId').append($("<option />").val(0).text('No State'));
                        $('#ShippingStateId').prop("disabled", true);
                        fillCities($('#ShippingCityId'), countryId, 0);
                    }
                    fillShippingMethods(countryId);
                })
                .fail(function (jqxhr, status, error) {
                    // this is the ""error"" callback
                    alert('Error!');
                });
        }
        else {
            $('#ShippingStateId').append($("<option />").val(0).text('Select Country'));
            $('#ShippingStateId').prop("disabled", false);
        }


    });
   
});



$("#BillingStateId").change(function () {
    var countryId = $('#BillingCountryId').val();
    var stateId = this.value;
    if (stateId > 0 && countryId > 0) {
        fillCities($('#BillingCityId'),countryId, stateId);
    }
    else {
        $('#BillingCityId').append($("<option />").val(0).text('Select Country/State'));
    }
});
function fillCities(city,countryId, stateId) {

    var dataToPost = {
        CountryId: countryId,
        StateId: stateId
    };
    //alert('PostData:' + countryId + ' ' + stateId);
    $.post('/Api/GetCitiesFor', dataToPost)
        .done(function (response, status, jqxhr) {
            if (response['Cities'].length > 0) {
                city.empty();
                city.append($("<option />").val(0).text('Select City'));
                $.each(response['Cities'], function () {
                    city.append($("<option />").val(this.CityId).text(this.Name));
                });
            }
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
            $('#PaymentMethodId').append($("<option />").val(0).text("Select Payment Method"));
            $.each(response['PaymentMethods'], function () {

                $('#PaymentMethodId').append($("<option />").val(this.PaymentMethodId).text(this.Name).attr('data-discount', this.PaymentDiscount).attr('data-info',this.PaymentInfo));
            })
        })
}
function fillShippingMethods(countryId, weight) {
    var weight = parseFloat($('#weight').val().replace(',', '.'));
    
    var dataToPost = {
        CountryId: countryId
    };
    $.post('/Api/GetShippingMethods', dataToPost)
        .done(function (response, status, jqxhr) {
            $('#ShippingMethodId').empty();
            $('#ShippingMethodId').append($("<option />").val(0).text('Select ShippingMethod'));
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
                } else if (weight > 2.5) {
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

