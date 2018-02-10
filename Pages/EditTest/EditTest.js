$(document).ready(function () {
    var counter = 0;
    var counter1 = 0;

    $("#addprocedure").on("click", function () {
        var newRow = $("<tr>");
        var cols = "";

        cols += '<td><input type="text" class="form-control" name="Procedure' + counter + '"/></td>';
        cols += '<td><input type="text" class="form-control" name="Arg1' + counter + '"/></td>';
        cols += '<td><input type="text" class="form-control" name="Arg2' + counter + '"/></td>';
        cols += '<td><input type="text" class="form-control" name="Arg3' + counter + '"/></td>';
        cols += '<td><input type="text" class="form-control" name="Arg4' + counter + '"/></td>';
        cols += '<td><input type="text" class="form-control" name="Arg5' + counter + '"/></td>';
        cols += '<td><input type="text" class="form-control" name="Arg6' + counter + '"/></td>';
        cols += '<td><input type="text" class="form-control" name="Arg7' + counter + '"/></td>';
        cols += '<td><input type="text" class="form-control" name="Arg8' + counter + '"/></td>';

        cols += '<td><input type="button" class="ibtnDel btn btn-md btn-danger "  value="Delete"></td>';
        newRow.append(cols);
        $("#proceduretable").append(newRow);
        counter++;
    });

    $("#addrequirement").on("click", function () {
        var newRow = $("<tr>");
        var cols = "";

        cols += '<td><input type="text" class="form-control" name="RequirementID' + counter1 + '"/></td>';
        cols += '<td><textarea class="form-control" rows="5" id="information' + counter1 + '"/></td>';

        cols += '<td><input type="button" class="ibtnDel btn btn-md btn-danger "  value="Delete"></td>';
        newRow.append(cols);
        $("#requirementstable").append(newRow);
        counter1++;
    });

    $("table.order-list").on("click", ".ibtnDel", function (event) {
        $(this).closest("tr").remove();
        counter -= 1
    });


});
