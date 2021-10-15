let selected = []

window.onload = function () {
    /*setup event listeners for product add to cart*/

    let elems = document.getElementsByClassName("add-to-cart");
    for (let i = 0; i < elems.length; i++) {
        elems[i].addEventListener('click', OnAddClick);
    }

    //On "Add to Cart" click
    function OnAddClick(event) {
        let target = event.currentTarget;

        //GetCartCount(target.id, (target.classList[0] + " " + target.classList[1]));
        ProductIntoCartProduct(target.id, (target.classList[0] + " " + target.classList[1]));
        GetCartCount();
    }
}

function GetCartCount() {
    let xhr = new XMLHttpRequest();

    xhr.open("GET", "/Gallery/CartCount")
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.send()
    console.log("test")
    xhr.onreadystatechange = function () {
        console.log("TEST")
        if (this.readyState == XMLHttpRequest.DONE) {
            console.log("YEE")

            let data = this.responseText;
            console.log(data)

            //ProductIntoCartProduct(productId, productName);
            let elem = document.getElementById("count");
            let count = parseInt(data) + parseInt(1);
            elem.innerHTML = count;
        }
    }
    console.log("end")
}

//Product goes into cart
function ProductIntoCartProduct(productId, productName) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Gallery/AddProductToCart");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status != 200) {
                return;
            }

            let data = JSON.parse(this.responseText);
            if (data.status == "success") {
                //window.location.href = "/ViewCart/getCartViewList";
            }
        }
    }
    //let data = GetCartCount();
    //package as Json object
    let product = {
        //ProductinCart: []
        ProductId: productId,
        ProductName: productName,
        //CartNum: data
    };

    /*let count = selected.push(product)
    for (let key of Object.keys(selected)) {
        data.ProductIds.push(key)
    }
    let elem = document.getElementById("count");
    elem.innerHTML = count;*/

    alert("Added " + productName + " to Cart");

    xhr.send(JSON.stringify(product));
}


function search_product() {

    let input = document.getElementById('searchbar').value
    if (input == null) {
        input = "";
    }
    input = input.toLowerCase();
    let x = document.getElementsByClassName('card');
    for (i = 0; i < x.length; i++) {
        if (!x[i].innerHTML.toLowerCase().includes(input)) {
            x[i].style.display = "none";
        }
        else {
            x[i].style.display = "card";
        }
    }
}

