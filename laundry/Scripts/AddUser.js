$(document).ready(function () {

    //===============================================================================================================================================================
    //==========================================================Adding New Users, applying username, passwords and roles to users ===================================
    //===============================================================================================================================================================
    //---all these events will be applied on Index.schtml, from tbl-Account controll

    //Filling the userlist
    $.ajax({
        type: "Get",
        url: "/tbl_Account/UsrList",
        data: "",
        contentType: "application/json; charset=utf-8",
        datatype: "jsondata",
        async: "true",
        success: function (response) {
            //var msg = eval('(' + response.d + ')');
            var msg = response;
            // remove table if it exists
            if ($('#tblUsrLst').length != 0) {
                $("#tblUsrLst").remove();
            }
            var table = '<table class="tblUserLst table table-responsive" id=tblResult>' +
                        '<thead> ' +
                            '<tr>' +
                                '<th></th>' +
                                '<th>Username</th>' +
                                '<th>Firt Name</th>' +
                                '<th>Last Name</th>' +
                                '<th>Role</ht>' +
                                '<th>Branch</th>' +
                                '<th></th>' +
                            '</tr' +
                        '</thead>  ' +
                        '<tbody>';
            for (var i = 0; i <= (msg.length - 1) ; i++) {
                var row = "<tr>";
                row += '<td style="background-color:#eee;">' + (i + 1) + '</td>';
                row += '<td>' + msg[i].Username + '</td>';
                row += '<td>' + msg[i].FName + '</td>';
                row += '<td>' + msg[i].LName + '</td>';
                row += '<td>' + msg[i].Role + '</td>';
                row += '<td>' + msg[i].BrName + '</td>';
                row += '<td><img src="/Content/Images/edit.png" title="Edit record." onclick="bindRecordToEdit(' + msg[i].AID + ')" style="width:16px;" /> <a id="hlkdel" href="#"  data-delid=' + msg[i].AID + '> <img src="/Content/Images/delete.png" title="Delete record." style="width:16px;" /></a></td>';
                row += '</tr>';
                table += row;
            }
            table += '</tbody></table>';
            //adding table to the Div with id#divuserlst
            $('#divuserlst').html(table);
            $("#divuserlst").slideDown("slow");
        },
        error: function (response) {
            alert(response.status + ' ' + response.statusText);
        }
    });

    //Pagniation (going next and previous
    //$(document).on("click", "#Previous", function () {

    //    if (CalculateAndSetPage("Previous"))
    //        $("#nasser").submit();

    //});
    //$(document).on("click", "#Next", function () {
    //    if (CalculateAndSetPage("Next"))
    //    $("#nasser").submit();
    //});

    //function CalculateAndSetPage(movingType) {

    //    var currentPage = parseInt($("#CurrentPage").val());
    //    alert(currentPage);
    //    var lastPage = parseInt($("#LastPage").val());

    //    if (currentPage == 1 && movingType == "Previous")
    //        return false;

    //    if (currentPage == lastPage && movingType == "Next")
    //        return false;

    //    if (movingType == "Previous")
    //        currentPage--;

    //    else if (movingType == "Next") {
    //        currentPage++;
    //    }
    //    else
    //        alert("some thing is wrong");

    //    $("#CurrentPage").val(currentPage);
    //    return true;
    //}


    //Create new User
    $("#flip3").click(function (e) {
        e.preventDefault();
        $("#panel3").slideToggle("slow");
    });

    //add new user
    $(document).on("click", "#btnAddUser", function () {
        //var username = $("#username").val();
        //var password = $("#password").val();
        //var FName = $("#FName").val();
        //var LName = $("#LName").val();
        //var CDate = $("#CDate").val();
        //var Branch = $("#Branch").val();

        var data = {
            username: $("#username").val(),
            password: $("#password").val(),
            FName: $("#FName").val(),
            LName: $("#LName").val(),
            Role: $("#Role").val(),
            Branch: $("#Branch").val()
        }

        $.ajax({
            type: "Get",
            url: "/tbl_Account/AddNewUser",
            data: data,
            contentType: "application/json; charset=utf-8",
            datatype: "jsondata",
            async: "true",
            success: function (response) {
                //var msg = eval('(' + response.d + ')');
                var msg = response;
                // remove table if it exists
                if ($('#tblUsrLst').length != 0) {
                    $("#tblUsrLst").remove();
                }
                var table = '<table class="tblUserLst table table-responsive" id=tblResult>' +
                            '<thead> ' +
                                '<tr>' +
                                    '<th></th>' +
                                    '<th>Username</th>' +
                                    '<th>Firt Name</th>' +
                                    '<th>Last Name</th>' +
                                    '<th>Role</th>' +
                                    '<th>Branch</th>' +
                                    '<th></th>' +
                                '</tr' +
                            '</thead>  ' +
                            '<tbody>';
                for (var i = 0; i <= (msg.length - 1) ; i++) {
                    var row = "<tr>";
                    row += '<td style="background-color:#eee;">' + (i + 1) + '</td>';
                    row += '<td>' + msg[i].Username + '</td>';
                    row += '<td>' + msg[i].FName + '</td>';
                    row += '<td>' + msg[i].LName + '</td>';
                    row += '<td>' + msg[i].Role + '</td>';
                    row += '<td>' + msg[i].BrName + '</td>';
                    row += '<td><img src="/Content/Images/edit.png" title="Edit record." onclick="bindRecordToEdit(' + msg[i].AID + ')" style="width:16px;" /> <a id="hlkdel" href="#"  data-delid=' + msg[i].AID + '> <img src="/Content/Images/delete.png" title="Delete record." style="width:16px;" /></a></td>';
                    row += '</tr>';
                    table += row;
                }
                table += '</tbody></table>';
                //adding table to the Div with id#divuserlst
                $('#divuserlst').html(table);
                $("#divuserlst").slideDown("slow");
                $("#panel3").slideToggle("slow");
                clearfields();
            },
            error: function (response) {
                alert(response.status + ' ' + response.statusText);
            }
        });

    });

    //Clear fields
    function clearfields() {
        $("#username").val("");
        $("#password").val("");
        $("#FName").val("");
        $("#LName").val("");
        $("#Role").val("");
        $("#Branch").val("");
    }

    //Delete user

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
        $.ajax({
            type: "Get",
            url: "/tbl_Account/DelUsers",
            data: data,
            contentType: "application/json; charset=utf-8",
            datatype: "jsondata",
            async: "true",
            success: function (response) {
                //var msg = eval('(' + response.d + ')');
                var msg = response;
                // remove table if it exists
                if ($('#tblUsrLst').length != 0) {
                    $("#tblUsrLst").remove();
                }
                var table = '<table class="tblUserLst table table-responsive" id=tblResult>' +
                            '<thead> ' +
                                '<tr>' +
                                    '<th>No.</th>' +
                                    '<th>Username</th>' +
                                    '<th>Firt Name</th>' +
                                    '<th>Last Name</th>' +
                                    '<th>Role</th>' +
                                    '<th>Branch</th>' +
                                    '<th></th>' +
                                '</tr' +
                            '</thead>  ' +
                            '<tbody>';
                for (var i = 0; i <= (msg.length - 1) ; i++) {
                    var row = "<tr>";
                    row += '<td style="background-color:#eee;">' + (i + 1) + '</td>';
                    row += '<td>' + msg[i].Username + '</td>';
                    row += '<td>' + msg[i].FName + '</td>';
                    row += '<td>' + msg[i].LName + '</td>';
                    row += '<td>' + msg[i].Role + '</td>';
                    row += '<td>' + msg[i].BrName + '</td>';
                    row += '<td><img src="/Content/Images/edit.png" title="Edit record." onclick="bindRecordToEdit(' + msg[i].AID + ')" style="width:16px;" /> <a id="hlkdel" href="#"  data-delid=' + msg[i].AID + '> <img src="/Content/Images/delete.png" title="Delete record." style="width:16px;" /></a></td>';
                    row += '</tr>';
                    table += row;
                }
                table += '</tbody></table>';
                //adding table to the Div with id#divuserlst
                $('#divuserlst').html(table);
                $("#divuserlst").slideDown("slow");
                clearfields();
            },
            error: function (response) {
                alert(response.status + ' ' + response.statusText);
            }
        });
    });

});
