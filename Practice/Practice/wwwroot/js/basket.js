$(document).ready(function () {
    $(document).on("click", ".delete-product", function (e) {
        e.preventDefault();
        let decodedCookie = decodeURIComponent(document.cookie.indexOf("Id"));
        console.log(decodedCookie)

        //let c;
        //for (var i = 0; i < decodedCookie.length; i++) {
        //   c=decodedCookie[0];
        //}
        console.log(c)
        $.ajax({
            type: "Get",
            url: "basket/delete/id",
            success: function (res) {
                console.log(res)
            }
        })

    })
})