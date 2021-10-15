var num_add = document.getElementById("num-add");
var num_minus = document.getElementById("num-minus");
var input_num = document.getElementById("input-num");

num_add.onclick = function () {

    input_num.value = parseInt(input_num.value) + 1;

}


num_minus.onclick = function () {

    if (input_num.value <= 0) {
        input_num.value = 0;
    } else {

        input_num.value = parseInt(input_num.value) - 1;

    }
}


