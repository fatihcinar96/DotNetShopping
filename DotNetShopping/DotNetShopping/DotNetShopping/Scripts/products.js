$(document).ready(function () {
    $('#slider-range').slider({
        range: true,
        min: 0,
        max: 1000,
        values: [minValue, maxValue],
        slide: function (event, ui) {
            $('#amount').val('$' + ui.values[0] + ' - $' + ui.values[1]);
            $('.RangeLink').attr('href', '/Home/Products?Category=' + category + '&min=' + ui.values[0] + '&max=' + ui.values[1] + '&Brand=' + brand);
            $('.RangeLink').show(200);
        }
    });
    $('#amount').val('$' + $('#slider-range').slider('values', 0) +
        ' - $' + $('#slider-range').slider('values', 1));
    var page = 1;
    var count = 12;
    var loadMore = true;
    $(window).on("scroll", function () {
        var scrollHeight = $(document).height();
        var scrollPosition = $(window).height() + $(window).scrollTop();
        if ((scrollHeight - scrollPosition) / scrollHeight === 0) {
            //when scroll to botttom of the page
            //alert('Load More Now!');
            if (loadMore) {
                var dataToPost = {
                    Page: page,
                    Count: count,
                    Category: category,
                    Brand: brand,
                    min: minValue,
                    max: maxValue
                };
                $.post('/Api/Products', dataToPost)
                    .done(function (response, status, jqxhr) {
                        if (response !== '') {
                            page++;
                            $('.ProductList').append(response);
                        }
                        else {
                            loadMore = false;
                        }
                    })
                    .fail(function (jqxhr, status, error) {
                        //this is the ""error"" callback
                        alert('Error!');
                    });
            }
        }
    })
});
