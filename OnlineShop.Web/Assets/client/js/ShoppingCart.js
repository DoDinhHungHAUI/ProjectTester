
var cart = {
    init: function () {
        cart.loadData();
        cart.registerEvent();

    },

    registerEvent: function () {
        $('.btnAddToCart').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.addItem(productId);
        });

        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            cart.deleteItem(productId);
        });

        $('.txtQuantity').off('keyup').on('keyup', function () {
            var quantity = parseInt($(this).val());
            var productid = parseInt($(this).data('id'));
            var price = parseFloat($(this).data('price'));
            if (isNaN(quantity) == false) {

                var amount = quantity * price;

                $('#amount_' + productid).text(numeral(amount).format('0,0'));
            }
            else {
                $('#amount_' + productid).text(0);
            }

            $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0') + "Vnđ");
            cart.updateAll();
        });

        $('#btnContinue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/Home/Index";
        });

        $('#btnDeleteAll').off('click').on('click', function (e) {
            e.preventDefault();
            cart.deleteAll();
        });

        $('#btnCheckout').off('click').on('click', function (e) {
            e.preventDefault();
            $('#divCheckout').show();
        });

        $('#chkUserLoginInfo').off('click').on('click', function () {
            if ($(this).prop('checked'))
                cart.getLoginUser();
            else {
                $('#txtName').val('');
                $('#txtAddress').val('');
                $('#txtEmail').val('');
                $('#txtPhone').val('');
            }

        });
        $('#btnCreateOrder').off('click').on('click', function (e) {
            e.preventDefault();
            ////var isValid = $('#frmPayment').valid();
            ////if (isValid) {
            ////    cart.createOrder();
            ////}

            cart.createOrder();

        });

    },

    createOrder: function () {
        var order = {
            CustomerName: $('#txtName').val(),
            CustomerAddress: $('#txtAddress').val(),
            CustomerEmail: $('#txtEmail').val(),
            CustomerMobile: $('#txtPhone').val(),
            CustomerMessage: $('#txtMessage').val(),
            PaymentMethod: "Thanh toán tiền mặt",
            Status: false
        }

        $.ajax({
            url: '/ShoppingCart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {
                if (response.status) {
                    $('#divCheckout').hide();
                    cart.deleteAll();

                    setTimeout(function () {
                        $('#cartContent').html('Cảm ơn bạn đã đặt hàng thành công. Chúng tôi sẽ liên hệ sớm nhất.');
                    }, 2000);
                }
            }
        })


    },

    getLoginUser: function () {
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('#txtName').val(user.FullName);
                    $('#txtAddress').val(user.Address);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhone').val(user.PhoneNumber);
                }
            }
        });
    },

    updateAll: function () {
        var cartList = [];
        $.each($('.txtQuantity'), function (i, item) {
            cartList.push({
                ProductId: $(item).data('id'),
                Quantity: $(item).val()
            });
        });

        $.ajax({
            url: '/ShoppingCart/Update',
            type: 'POST',
            data: {
                cartData: JSON.stringify(cartList)
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    cart.loadData();
                    $("#CountcartItem").text(response.Counter + " Items")
                    console.log('Update ok');
                }
            }
        });

    },

    getTotalOrder: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },

    addItem: function (productId) {
       
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
                    toastr.success('Thông báo', 'Sản phẩm đã được thêm vào giỏ hàng', { timeOut: 3000 })
                    $("#CountcartItem").text(response.Counter + " Items")
                } else {
                    //alert(response.message);
                    toastr.success('Thông báo', response.message, { timeOut: 3000 })
                }
       
            }
        });
    },

    deleteAll: function (productId) {
        $.ajax({
            url: '/ShoppingCart/DeleteAll',
            type: 'POST',
            dataType: 'json',
            success: function (respose) {
                if (respose.status) {
                    cart.loadData();
                    $("#CountcartItem").text(response.Counter + " Items")
                }
            }
        })
    },

    deleteItem: function (productId) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                productId: productId
            },
            type: 'POST',
            dataType: 'Json',
            success: function (response) {
                if (response.status) {

                    cart.loadData();
                    $("#CountcartItem").text(response.Counter + " Items")
                }
            }
        });
    },


    loadData: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template = $('#tplCart').html();
                    var html = '';
                    var data = res.data;

                    var searchRegExp = /,/gi;
                    var replaceWith = '.';

                  
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductId: item.ProductId,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: item.Product.Price,
                            PriceF: numeral(item.Product.Price).format('0,0').replace(searchRegExp, replaceWith),
                            Quantity: item.Quantity,
                            Amount: numeral(item.Quantity * item.Product.Price).format('0,0').replace(searchRegExp, replaceWith)
                        });
                    });
                    $("#CountcartItem").text(res.Counter + " Items")

                    $('#cartBody').html(html);

                    if (html == '') {
                        $('#cartContent').html('Không có sản phẩm nào trong giỏ hàng');
                    }

                    $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0').replace(searchRegExp, replaceWith) + "Vnđ");
                   
                    cart.registerEvent();
                }
            }
        })
    }


}
cart.init()


















