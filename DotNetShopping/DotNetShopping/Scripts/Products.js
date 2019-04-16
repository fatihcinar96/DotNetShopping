$(document).ready(function () {

    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() > $(document).height() - 100) {

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

