$(document).ready(function () {

    $("#slider-range").slider({
        range: true,
        min: 0,
        max: 1000,
        values: [minValue, maxValue],
    slide: function (event, ui) {
        $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
        $(".RangeLink").attr("href", '/Home/Products?Category=@ViewBag.SelectedCategory&min=' + ui.values[0] + '&max=' + ui.values[1] + '&Brand=@ViewBag.Brand');
        $(".RangeLink").show(200);
    }
});
$("#amount").val("$" + $("#slider-range").slider("values", 0) +
    " - $" + $("#slider-range").slider("values", 1));

    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 200) {

            var dataToPost = {
                min: minValue,
                max: maxValue,
                count: ++count,
                BrandId: brandId,
                Category: category
            };
            $.post('/Api/LoadMoreProducts', dataToPost)
                .done(function (response, status, jqxhr) {
                    $(".ProductList").append(response);
                })
                .fail(function (error, status, jqxhr) {
                    alert("something went wrong");
                })
        }
    });
})

