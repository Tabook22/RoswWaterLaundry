$(document).ready(function () {
    $(".collapsible").on("click", function (e) {
        e.preventDefault();

        var this1 = $(this); // Get Click item 
        var data = {
            pid: $(this).attr('pid')
        };
        var isLoaded = $(this1).attr('data-loaded'); // Check data already loaded or not
        if (isLoaded == "false") {
            $(this1).addClass("loadingP");   // Show loading panel
            $(this1).removeClass("itmcollapse");

            // Now Load Data Here 
            $.ajax({
                url: "/Treeview/GetSubMenu",
                type: "GET",
                data: data,
                dataType: "json",
                success: function (d) {
                    $(this1).removeClass("loadingP");

                    if (d.length > 0) {

                        var $ul = $("<ul></ul>");
                        $.each(d, function (i, ele) {
                            $ul.append(
                                    $("<li></li>").append(
                              
                                        "<span class='subItmcollapse subCollapsible' data-loaded='false' pid='" + ele.Id + "'>&nbsp;</span>"+
                                        "<span>"+ele.catName +"</span>" 
                                    )
                                )
                        });

                        $(this1).parent().append($ul);
                        $(this1).addClass('itmcollapse');
                        $(this1).toggleClass('itmcollapse expand');
                        $(this1).closest('li').children('ul').slideDown();
                    }
                    else {
                        // no sub menu
                        $(this1).css({ 'dispaly': 'inline-block', 'width': '15px' });
                    }

                    $(this1).attr('data-loaded', true);
                },
                error: function () {
                    alert("Error!");
                }
            });
        }
        else {
            // if already data loaded
            $(this1).toggleClass("itmcollapse expand");
            $(this1).closest('li').children('ul').slideToggle();
        }

    });

    //Items list in Item Sub Category
    $(document).on("click",".subCollapsible", function (e) {
        e.preventDefault();
        var this1 = $(this); // Get Click item 
        var data = {
            pid: $(this).attr('pid')
        };
        var isLoaded = $(this1).attr('data-loaded'); // Check data already loaded or not
        if (isLoaded == "false") {
            $(this1).addClass("loadingP");   // Show loading panel
            $(this1).removeClass("itmcollapse");

            // Now Load Data Here 
            $.ajax({
                url: "/Treeview/GetSubMenu2",
                type: "GET",
                data: data,
                dataType: "json",
                success: function (d) {
                    $(this1).removeClass("loadingP");
                    if (d.length > 0) {
                        var $ul = $("<ul></ul>");
                        $.each(d, function (i, ele) {
                            $ul.append(
                                    $("<li></li>").append(
                                        "<span class='icollapse iCollapsible' data-loaded='false' pid='" + ele.ItemId + "'>&nbsp;</span>" + "<img id='itmImg' src='" + ele.itemImg + "'/>" + "<br/>"+
                                         "<input type='button' value='" + ele.ItemName + "' title='" + ele.ItemName + "' class='btn btn-danger itmName' data-itmid='" + ele.ItemId + "' style='border-radius: 1px;min-width:10rem;' />"
                                    )
                                )
                        });

                        $(this1).parent().append($ul);
                        $(this1).addClass('subItmcollapse');
                        $(this1).toggleClass('subItmcollapse expand');
                        $(this1).closest('li').children('ul').slideDown();
                    }
                    else {
                        // no sub menu
                        $(this1).css({ 'dispaly': 'inline-block', 'width': '15px' });
                    }

                    $(this1).attr('data-loaded', true);
                },
                error: function () {
                    alert("Error!");
                }
            });
        }
        else {
            // if already data loaded
            $(this1).toggleClass("itmcollapse expand");
            $(this1).closest('li').children('ul').slideToggle();
        }

    });

    // On click on Item
    // Getting the Itme Quantity and costs---------------------------------------------------------------------------------------------------------------------------------
    $(document).on("click", ".itmName", function (e) {
        e.preventDefault();
        //getting the item id
        var data = {
            id: $(this).data("itmid")
        };
        $("#Qyt").val(''); //clear quantity
        $("#Qyt").focus().css('background-color', '#ffff00');
        $.ajax({
            type: "GET",
            url: "/Items/getItemCosts",
                data: data,
                dataType: "json",
                success: function (response) {
                    //var msg = eval('(' + response.d + ')');
                    //var msg = response;
                    // var cName = "Customer Name: " + response.CustName + " ---ID: " + response.CustId;
                    $("#Cost").val(response.Price.toFixed(3));
                    $('#getItmId').val(response.ItemId); //store item id in a hidden field to used it after when we save the bill details
                    $('#itmMId').val(response.ItemId); //store item main category in a hidden field to used it after when we save the bill details
                    $('#itmSId').val(response.ItemId); //store item sub category in a hidden field to used it after when we save the bill details

                },
                error: function (response) {
                    alert(response.status + ' ' + response.statusText);
                }
            });
        });
});
