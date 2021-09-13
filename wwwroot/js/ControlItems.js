function addItemCard() {

    $("#addItemCard").modal("show");
}
function onCloseModal() {

    $("#addItemCard").modal("hide");
}
function onDeleteModel() {
    $("#providertitle").val("");
    $("#description").val("");
    $("#bigimg").val("");
    $("#smallimg").val("");
    $("#discountselect").val("choose");
    $("#addItemCard").modal("hide");
}
var routeURL = location.protocol + "//" + location.host;


$.ajax({
    url: routeURL + '/api/AccountSettings/GetCardData',
    type: 'GET',
    dataType: 'JSON',
    contentType: 'application/json; charset=utf-8',
    dataType: "json",
    success: function (response) {

        if (response.status === 1) {
		
            var obj = response.dataeum;
            jQuery.each(obj, function (i, val) {
                /*$("#id" + obj[i].id).val(obj[i].id);*/
                $("#titlecard_" + obj[i].id).append(obj[i].title);
                $("#descriptioncard_" + obj[i].id).append(obj[i].description);
                $("#discountcard_" + obj[i].id).append(obj[i].discount);
                $("#getbigimgcard_" + obj[i].id).attr({
                    alt: obj[i].title,
                    src: obj[i].bigImgUrl
            });
                $("#samllimgcard_" + obj[i].id).attr({
                    alt: obj[i].title,
                    src: obj[i].smallImgUrl
            });
                
               
            });
           
        }

    },
    async: false
});



function onAddItem() {

    var requestData = {
        
        Title: $(providertitle).val(),
        Description: $(description).val(),
        BigImgUrl: $(bigimg).val(),
        smallImgUrl: $(smallimg).val(),
        Id: parseInt($(id).val()),
        discount: $(discountselect).val()
    };
    
    $.ajax({
        url: routeURL + '/api/AccountSettings/PostNewItemMethod',
        type: 'POST',
        data: JSON.stringify(requestData),
        contentType: "application/json",
        dataType: "json",
        success: function (response) {

            if (response.status === 1) {

                $.notify("Data Changed Successfully", "Sucess")
                location.reload(true);
            }
            if (response.status === 0) {

                $.notify("Error", "error")

            }


        },
        error: function (xhr) {
            $.notify("Error", "error");
        }

    });
}


function onDeleteCard(var_id) {

    var id = parseInt(var_id);
    $.ajax({
        url: routeURL + '/api/AccountSettings/DeleteCardItem/' + id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.status === 1) {

                $.notify("Item Removed Successfully", "success");
                location.reload(true);
            }


        },
        error: function (xhr) {
            $.notify("Error", "error");

        }

    });
}

function onEditCard(var_id) {
    //get the data of modal

    var id = parseInt(var_id);
    $("#providertitle").val($('#titlecard_' + id).text());
    $("#description").val($('#descriptioncard_' + id).text());
    var discountget = $('#discountcard_' + id).text();
    $("#bigimg").val($("#getbigimgcard_" + id).attr('src'));
    $("#smallimg").val($("#samllimgcard_" + id).attr('src'));
   /* var images = $("#getbigimgcard_" + id).attr('src');*/
    switch (discountget) {
        case "5%":
            $("#discountselect").val('5%')
            break;
        case "10%":
            $("#discountselect").val('10%')
            break;
        case "15%":
            $("#discountselect").val('15%')
            break;
        case "20%":
            $("#discountselect").val('20%')
            break;
        case "25%":
            $("#discountselect").val('25%')
            break;
        case "30%":
            $("#discountselect").val('30%')
            break;
        case "50%":
            $("#discountselect").val('50%')
            break;
        case "70%":
            $("#discountselect").val('70%')
            break;
        case "80%":
            $("#discountselect").val('80%')
            break;
        case "90%":
            $("#discountselect").val('90%')
            break;
        case "100%":
            $("#discountselect").val('100%')
            break;
    }
    addItemCard();
    $("#btnConfirm").hide();
    $("#btnDelete").hide();
    $("#ClosemOdel").hide();
    $('#btnEdit').removeAttr('hidden');
    $('#edititem').removeAttr('hidden');
    $('#edititem').click(function (reset) {

        $("#providertitle").val("");
        $("#description").val("");
        $("#bigimg").val("");
        $("#smallimg").val("");
        $("#discountselect").val("choose");
        $("#btnConfirm").show();
        $("#btnDelete").show();
        $("#ClosemOdel").show();
        $("#btnEdit").attr("hidden", true);
        $("#edititem").attr("hidden", true);
        $("#addItemCard").modal("hide");

    });
    $('#btnEdit').click(function updateModalItem() {
        var x = $("#discountselect").val();
        var requestData = {
           
            Title: $("#providertitle").val(),
            Description: $("#description").val(),
            BigImgUrl: $("#bigimg").val(),
            smallImgUrl: $("#smallimg").val(),
            discount: $("#discountselect").val()
         

        };
        $.ajax({
            url: routeURL + '/api/AccountSettings/updateModalItem/' + id,
            type: 'POST',
            data: JSON.stringify(requestData),
            contentType: "application/json",
            dataType: "json",
            success: function (response) {
                if (response.status === 1) {

                    $.notify("Item Removed Successfully", "success");
                    location.reload(true);
                }


            },
            error: function (xhr) {
                $.notify("Error", "error");

            }

        });


    });
}


