// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


function tableFix()
{
    var rows = $("#myTable tr");
    var columns = $("#myTable tr th");
    var rowCount = rows.length;
    var colCount = columns.length;

    while (rowCount < 4) {
        addRow();
        rowCount = $("#myTable tr").length;
    }

    while (colCount < 3) {
        addCol();
        colCount = $("#myTable tr th").length;
    }

    while (rowCount > 4) {
        var row = rows.eq(`${rowCount - 1}`)
        button = row.find(".close");
        button.trigger("click");
        rowCount = $("#myTable tr").length;
    }

    while (colCount > 3) {
        var row = rows.eq(0)
        buttons = row.find(".close");
        button = buttons.last();
        button.trigger("click");
        colCount = $("#myTable tr th").length;
    }
}


$(function () {
    $("#generateExample").click(function () {

        tableFix();

        $("#initialState").val("q0");
        $("#inputWord").val("bba");

        var letters = $("input[name='letters']");
        var states = $("input[name='states']");
        var formulas = $("input[name='formulas']");
        var checkBoxes = $("input[type = 'checkbox']");

        letters.eq(0).val("a");
        letters.eq(1).val("b");
        states.eq(0).val("q0");
        states.eq(1).val("q1");
        formulas.eq(0).val("q1");
        formulas.eq(1).val("q0 and not q1");
        formulas.eq(2).val("q0");
        formulas.eq(3).val("not q0");
        checkBoxes.eq(1).prop('checked', true);





        //$("#initialState").val("s");
        //$("#inputWord").val("ab");

        //var letters = $("input[name='letters']");
        //var states = $("input[name='states']");
        //var formulas = $("input[name='formulas']");
        //var checkBoxes = $("input[type = 'checkbox']");

        //letters.eq(0).val("a");
        //letters.eq(1).val("b");
        //states.eq(0).val("s");
        //states.eq(1).val("q");
        //formulas.eq(0).val("q");
        //formulas.eq(1).val("false");
        //formulas.eq(2).val("false");
        //formulas.eq(3).val("p or s");
        //formulas.eq(4).val("s");
        //formulas.eq(5).val("false");


        //checkBoxes.eq(0).prop('checked', true);

    })
})

$(function () {
    $("#form").submit(function () {
        event.preventDefault();

        $.ajax({
            type: "POST",
            url: 'Home/Tree5',
            data: $("form").serialize(),
            success: function (data) {
                $("#computationArea").html(data);
            }
        })
    })
})


function addRow() {

    var colCount = $("#myTable tr th").length;
    var rowCount = $("#myTable tr").length;
    var formulasCount = (colCount - 1) * (rowCount - 1);
    var extraCell = 0;
    var table = document.getElementById("myTable");
    var row = document.createElement("TR");


    for (let i = 0; i <= colCount; i++) {
        var td = document.createElement("TD");

        if (i == 0) {
            var html = `<div CLASS="input-group mb-3"> <div CLASS="input-group-prepend"> <div CLASS="input-group-text"> <input TYPE="checkbox" name="isFinal" VALUE="true">  <input TYPE="hidden" NAME="isFinal" VALUE="false"> </div></div> <input CLASS="form-control" PLACEHOLDER="state" TYPE="text" name="states" required> </div>`;
            td.className = "bg-light";
            td.innerHTML = html;
        }

        else if (i == colCount) {
            var html = `<button type="button" class="close" aria-label="Close" onClick="delRow(this)">
                                                <span aria-hidden="true">&times;</span>
                                                </button>`
            td.className = "border-0";
            td.style = "min-width: 10px; padding: 6px";
            td.innerHTML = html;
        }

        else {
            var html = `<input CLASS="form-control" PLACEHOLDER="formula" TYPE="text" name="formulas" required>`;
            td.innerHTML = html;
        }

        row.appendChild(td);
    }
    table.appendChild(row);
}



function delRow(r) {

    var ind = r.parentNode.parentNode.rowIndex;
    var table = document.getElementById("myTable");
    table.deleteRow(ind);
}

function delCol(c) {
    var ind = c.closest("td").cellIndex;
    $("tr").each(function (j, row) {
        $(row).children(`td:eq(${ind}),th:eq(${ind})`).remove()
    });
}

function addCol() {
    var colCount = $("#myTable tr th").length;
    $("#myTable tr").each((i, row) => {

        if (i == 0) {
            var cell = document.createElement("td");
            cell.className = "border-0";
            cell.style.padding = "6px";
            cell.style.textAlign = "center";

            var closeButton = `<button type="button" class="close" onClick="delCol(this)">
                                                                            <span aria-hidden="true">&times;</span>
                                                                        </button>`;
            cell.innerHTML = closeButton;
            row.appendChild(cell);
        }

        else if (i == 1) {
            var cell = document.createElement("th")
            var headCell = $("#headCell").html()
            $(cell).html(headCell)
            row.appendChild(cell);
        }

        else if (i == 2) {
            var cell = document.createElement("td");
            var bodyCell = $("#bodyCell").html();
            $(cell).html(bodyCell);
            row.appendChild(cell);
        }

        else if (i > 2) {
            var cell = document.createElement("td");
            var bodyCell = $("#bodyCell").html();
            $(cell).html(bodyCell);
            row.appendChild(cell);
            row.insertBefore(cell, row.children[colCount]);
        }
    });
}