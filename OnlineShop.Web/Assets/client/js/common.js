var common = {
    init: function () {
        common.registerEvents();
    },

    registerEvents: function () {
        $("#txtSearch").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Product/GetListProductByName",
                    dataType: "json",
                    data: {
                        keyword: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtSearch").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtSearch").val(ui.item.label);
                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<a>" + item.label + "</a>")
                .appendTo(ul);
        };

        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));

            $.ajax({
                url: '/ShoppingCart/Add',
                data: {
                    productId: productId
                },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        //alert('Thêm sản phẩm thành công.');
                        toastr.success('Thông báo', 'Sản phẩm đã được thêm vào giỏ hàng', { timeOut : 3000 })
                        $("#CountcartItem").text(response.Counter + " Items")
                    } else {
                        toastr.success('Thông báo', response.message, { timeOut: 3000 })
                        //alert(response.message);
                    }
                }
            });
        });
    }
}
common.init();