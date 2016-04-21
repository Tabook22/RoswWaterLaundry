$(document).ready(function () {

    //===============================================================================================================================================================
    //==========================================================Adding New Bill =====================================================================================
    //===============================================================================================================================================================
    //---all these events will be applied on Bill.schtml, from Bill Controll

    //search for users, and choosing the right search method

        $("#custtel").prop("disabled", true);
        $("#custcode").prop("disabled", true);
        $("#cstId").prop("disabled", true);
        $(document).on("click", "#rdoption", function () {
            var getOption = $(this).val();

            switch (getOption) {
                case 'cTel':
                    $("#custtel").prop('disabled', false);
                    $("#custcode").prop("disabled", true);
                    $("#custcode").val("");
                    $("#cstId").prop("disabled", true);
                    $("#cstId").val("");
                    break;
                case 'cCode':
                    $("#custtel").prop('disabled', true);
                    $("#custtel").val("");
                    $("#custcode").prop("disabled", false);
                    $("#cstId").prop("disabled", true);
                    $("#cstId").val("");
                    break;
                case 'cstId':
                    $("#custtel").prop('disabled', true);
                    $("#custtel").val("");
                    $("#custcode").prop("disabled", true);
                    $("#custcode").val("");
                    $("#cstId").prop("disabled", false);
                    break;
            }
        });

    //this function is used to solve the JSON date issue
    function ToJavaScriptDate(value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        return dt.getDate() + "/" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
    }

    // This will delete the tempTable to make sure there is no data in the temp table before adding a new bill
    $.ajax({
        type: "GET",
        url: "/Bills/DelAllTemp",
        data: "",
        data: "{}",
        contentType: "application/json; charset=utf-8",
        datatype: "jsondata",
        async: "true",
        success: function (response) {

        },
        error: function (response) {
            alert(response.status + ' ' + response.statusText);
        }
    });

    // this will fill the table of the items which were selected by the customers

    //Allow only numbers in the quntity field
    $("#Qyt").keypress(function (e) {
        //if the letter is not digit then display error and don't type anything
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            //display error message
            $("#errmsg").html("Digits Only").show().fadeOut("slow");
            return false;
        }
    });

    // Getting the Custtomer Id
    $("#findCst").on("click", function (e) {
        e.preventDefault();
        var getTel = $("#custtel").val();
        var getCode=  $("#custcode").val();
        var getName = $("#cstId").val();

        var getOption =$("input[name=rdoption]:checked").val() //Other fields values

        if (getOption == "cTel") {
            var data = {
                sOption:getTel,
                sType: "cTel"
            }
        }else if (getOption == "cCode") {
            var data = {
                sOption:getCode,
                sType: "cCode"
            }
        } else {
                var data = {
                    sOption:getName,
                    sType: "cstId"
                }
        }

        $.ajax({
            type: "GET",
            url: "/Customers/getCustDetails",
            data: data,
            contentType: "application/json; charset=utf-8",
            datatype: "jsondata",
            async: "true",
            success: function (response) {
                //var msg = eval('(' + response.d + ')');
                var msg = response;
                var cName = response.CustName;
                $("#CustId").val(response.CustId);
                $("#cstname").text(cName);
                $("#custcode").val(response.CustId);
                $("#custtel").val(response.Tel);
                $('#cstId option[value="' + response.CustId + '"]').attr('selected', true)
            },
            error: function (response) {
                alert("Please check the inputs of the customers")
                //alert(response.status + ' ' + response.statusText);
            }
        });
    });

    // Getting the Itme Quantity and costs
    $('#ItemId').change(function () {
        //getting the item id
        var getT = $("#ItemId option:selected").text();

        if (getT === "--Select Item--") {
            alert(getT);
            $("#Qyt").val('');
            $("#Cost").val('');
            $("#totalCost").text('');
            return false;
        }
        var id = $(this).val();
        $("#Qyt").val(''); //clear quantity
        //getting the url location for ajax call
        var url = '@Url.Action("getItemCosts", "Items")' + '?id=' + id;
        $("#Qyt").focus().css('background-color', '#ffff00');
        $.ajax({
            type: "POST",
            url: url,
            data: "{}",
            contentType: "application/json; charset=utf-8",
            datatype: "jsondata",
            async: "true",
            success: function (response) {
                //var msg = eval('(' + response.d + ')');
                //var msg = response;
                // var cName = "Customer Name: " + response.CustName + " ---ID: " + response.CustId;
                $("#Cost").val(response.Price.toFixed(3));
            },
            error: function (response) {
                alert(response.status + ' ' + response.statusText);
            }
        });
    });

    //Calaculationg Total Cost
    $('#Qyt').keyup(function () {
        //getting the item id
        var id = $(this).val();
        var qt = $("#Cost").val();
        //getting the url location for ajax call
        var getTotals = id * qt;
        $("#totalCost").text(getTotals.toFixed(3));//to add more digital places

    });

    //check to see if all the fileds are not empty,if there is any field is empty this function will return false
    function CheckFields() {
        //customer details
        // check to see if there is a value in the customer tel
        var csttel = $("#custtel").val();
        var custId1 = $("#CustId").val();
        var itemId1 = $('#getItmId').val();
        var Qyt1 = $('#Qyt').val();
        var cost1 = $("#totalCost").html();

        if (csttel.length == 0 || custId1.length == 0 || itemId1.length == 0 || Qyt1.length == 0 || cost1.length == 0) {
            alert("Please check the input fields before adding new item to the list");
            return false;
        }

    }

    // Add new bill temprory
    $('#btnAddTemp').on("click", function (e) {
        e.preventDefault();
        var valid = CheckFields();
        if (valid == false) {
            return false;
        }

        //var custId = $("#CustId").val();
        //var itemId = $('#ItemId').val();
        //var qyt = $('#Qyt').val();
        //var cost = $("#totalCost").text();

        var data = {
            custId: $("#CustId").val(),
            itemId: $('#getItmId').val(),
            qyt: $('#Qyt').val(),
            cost: $("#totalCost").text(),
            MId: $("#itmMId").val(),
            SId: $("#itmSId").val()
        };

        // var url = '@Url.Action("AddTemp", "Bills")' + "?custId=" + custId + "&itemId=" + itemId + "&qyt=" + qyt + "&cost=" + cost;
        $.ajax({
            type: "Get",
            url: "/Bills/AddTemp",
            data: data,
            contentType: "application/json; charset=utf-8",
            datatype: "jsondata",
            async: "true",
            success: function (response) {
                //var msg = eval('(' + response.d + ')');
                var msg = response;
                // remove table if it exists
                if ($('#tblResult').length != 0) {
                    $("#tblResult").remove();
                }
                var table = '<table class="tblResult table table-responsive" id=tblResult>' +
                 '<thead> ' +
                 '<tr>' +
                 '<th style="display:none;"></th>' +
                // '<th></th>' +
                 //'<th> </th>' +
                 '<th>Item Code</th>' +
                 '<th>Description</th>' +
                 //'<th></th>' +
                 '<th>Qyt</th>' +
                 '<th>Cost</th>' +
                 '<th></th>' +
                 '</thead>  ' +
                 '<tbody>';

                for (var i = 0; i <= (msg.length - 1) ; i++) {
                    var row = "<tr>";
                    row += '<td style="background-color:red;display:none;">' + msg[i].Id + '</td>';
                    //row += '<td>' + msg[i].CustId + '</td>';
                    //row += '<td>' + msg[i].CustName + '</td>';
                    row += '<td>' + msg[i].ItemId + '</td>';
                    row += '<td>' + msg[i].ItemName + '</td>';
                    //row += '<td>' + ToJavaScriptDate(msg[i].Date) + '</td>';
                    row += '<td>' + msg[i].Qyt + '</td>';
                    row += '<td class="price">' + msg[i].Cost + '</td>';
                    row += '<td><img src=' + editImgUrl + ' title="Edit record." onclick="bindRecordToEdit(' + msg[i].Id + ')" style="width:16px;display:none;" /> <a id="hlkdel" href="#"  data-delid=' + msg[i].Id + '> <img src=' + deleteImgUrl + ' title="Delete record." style="width:16px;" /></a></td>';

                    row += '</tr>';
                    table += row;
                }
                table += '</tbody></table>';

                $('#cstitems').html(table);
                $("#cstitems").slideDown("slow");

                var sum = 0;
                // iterate through each td based on class and add the values
                $(".price").each(function () {

                    var value = $(this).text();
                    // add only if the value is number
                    if (!isNaN(value) && value.length != 0) {
                        sum += parseFloat(value);
                    }

                });
                $("#tAmount").text("Total Amount (R.O)= " + sum.toFixed(2));
            },
            error: function (response) {
                alert(response.status + ' ' + response.statusText);
            }
        });

    });

    // delete item from the list

    $(document).on("click", "#hlkdel", function (e) {
        e.preventDefault();
        var getDelId = $(this).data("delid");
        //=== Show confirmation alert to user before delete a record.
        var ans = confirm("Are you sure to delete a record?");
        var data = {
            id: getDelId
        };
        // var url = '@Url.Action("DeleteTemp", "Bills")' + '?id=' + ;
        //=== If user pressed Ok then delete the record else do nothing.
        if (ans == true) {
            $.ajax({
                type: "Get",
                url: "/Bills/DeleteTemp",
                data: data, // data: "{id:'" + id + "'}",
                contentType: "application/json; charset=utf-8",
                datatype: "jsondata",
                async: "true",
                success: function (response) {
                    //var msg = eval('(' + response.d + ')');
                    var msg = response;
                    // remove table if it exists
                    if ($('#tblResult').length != 0) {
                        $("#tblResult").remove();
                    }
                    var table = '<table class="tblResult table table-responsive" id=tblResult>' +
            '<thead> ' +
            '<tr>' +
            '<th style="display:none;"></th>' +
           // '<th></th>' +
            //'<th> </th>' +
            '<th>Item Code</th>' +
            '<th>Description</th>' +
            //'<th></th>' +
            '<th>Qyt</th>' +
            '<th>Cost</th>' +
            '<th></th>' +
            '</thead>  ' +
            '<tbody>';

                    for (var i = 0; i <= (msg.length - 1) ; i++) {
                        var row = "<tr>";
                        row += '<td style="background-color:red;display:none;">' + msg[i].Id + '</td>';
                        //row += '<td>' + msg[i].CustId + '</td>';
                        //row += '<td>' + msg[i].CustName + '</td>';
                        row += '<td>' + msg[i].ItemId + '</td>';
                        row += '<td>' + msg[i].ItemName + '</td>';
                        //row += '<td>' + ToJavaScriptDate(msg[i].Date) + '</td>';
                        row += '<td>' + msg[i].Qyt + '</td>';
                        row += '<td class="price">' + msg[i].Cost + '</td>';
                        row += '<td><img src=' + editImgUrl + ' title="Edit record." onclick="bindRecordToEdit(' + msg[i].Id + ')" style="width:16px;display:none;" /> <a id="hlkdel" href="#"  data-delid=' + msg[i].Id + '> <img src=' + deleteImgUrl + ' title="Delete record." style="width:16px;" /></a></td>';

                        row += '</tr>';
                        table += row;
                    }
                    table += '</tbody></table>';


                    $('#cstitems').html(table);
                    $("#cstitems").slideDown("slow");

                    //get the total sum
                    var sum = 0;
                    // iterate through each td based on class and add the values
                    $(".price").each(function () {

                        var value = $(this).text();
                        // add only if the value is number
                        if (!isNaN(value) && value.length != 0) {
                            sum += parseFloat(value);
                        }

                    });
                    $("#tAmount").text("Total Amount (R.O)= " + sum.toFixed(2));
                },
                error: function (response) {
                    alert(response.status + ' ' + response.statusText);
                }
            });
        }

    });

    function deleteRecord(id) {
        alert("الحمد لله رب العالمين")
        //=== Show confirmation alert to user before delete a record.
        var ans = confirm("Are you sure to delete a record?");
        var url = '@Url.Action("DeleteTemp", "Bills")' + '?id=' + id;;
        //=== If user pressed Ok then delete the record else do nothing.
        if (ans == true) {
            $.ajax({
                type: "POST",
                url: url,
                data: '', // data: "{id:'" + id + "'}",
                contentType: "application/json; charset=utf-8",
                datatype: "jsondata",
                async: "true",
                success: function (response) {
                    //var msg = eval('(' + response.d + ')');
                    var msg = response;
                    // remove table if it exists
                    if ($('#tblResult').length != 0) {
                        $("#tblResult").remove();
                    }
                    var table = '<table class="tblResult table table-responsive" id=tblResult>' +
                        '<thead> ' +
                        '<tr>' +
                        '<th style="display:none;">Id</th>' +
                        '<th>CustId</th>' +
                        '<th> CustName</th>' +
                        '<th>ItemId</th>' +
                        '<th>ItemName</th>' +
                        '<th>Date</th>' +
                        '<th>Qyt</th>' +
                        '<th>Cost</th>' +
                        '<th>Actions</th>' +
                        '</thead>  ' +
                        '<tbody>';

                    for (var i = 0; i <= (msg.length - 1) ; i++) {
                        var row = "<tr>";
                        row += '<td style="background-color:red;display:none;">' + msg[i].Id + '</td>';
                        row += '<td>' + msg[i].CustId + '</td>';
                        row += '<td>' + msg[i].CustName + '</td>';
                        row += '<td>' + msg[i].ItemId + '</td>';
                        row += '<td>' + msg[i].ItemName + '</td>';
                        row += '<td>' + ToJavaScriptDate(msg[i].Date) + '</td>';
                        row += '<td>' + msg[i].Qyt + '</td>';
                        row += '<td class="price">' + msg[i].Cost + '</td>';
                        row += '<td><img src="@Url.Content("~/Content/Images/edit.png")" title="Edit record." onclick="bindRecordToEdit(' + msg[i].Id + ')" style="width:16px;display:none;" />  <img src="@Url.Content("~/Content/Images/delete.png")" onclick="deleteRecord(' + msg[i].Id + ')" title="Delete record." style="width:16px;" /></td>';

                        row += '</tr>';
                        table += row;
                    }
                    table += '</tbody></table>';

                    $('#cstitems').html(table);
                    $("#cstitems").slideDown("slow");

                    //get the total sum
                    var sum = 0;
                    // iterate through each td based on class and add the values
                    $(".price").each(function () {

                        var value = $(this).text();
                        // add only if the value is number
                        if (!isNaN(value) && value.length != 0) {
                            sum += parseFloat(value);
                        }

                    });
                    $("#tAmount").text("Total Amount (R.O)= " + sum.toFixed(2));
                },
                error: function (response) {
                    alert(response.status + ' ' + response.statusText);
                }
            });
        }
    }







});