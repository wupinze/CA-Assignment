let selected = [];

window.onload = function () {
    /*setup event listeners for product add to cart*/
    let elems = document.getElementsByClassName("a_product");
    for (let i = 0; i < elems.length; i++) {
        elems[i].addEventListener('click', OnAddClick);
    }
}

//On "Add to Cart" click
function OnAddClick(event) {
    let target = event.currentTarget;

    ProductIntoCartProduct(target.id);
}

//Product goes into cart
function ProductIntoCartProduct(productId, productName) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Gallery/AddProductToCart");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        if (this.readyState == XMLttpRequest.DONE) {
            if (this.status != 200) {
                return;
            }

            let data = JSON.parse(this.responseText);
            if (data.status == "success") {
                window.location.href = "/ViewCart/getCartViewList";

            }
        }
    }

    //package as Json object
    let product = {
        //ProductIds: []
        ProductId: productId,
        ProductName: productName
    };

    let count = selected.push(product)
    /*for (let key of Object.keys(selected)) {
        data.ProductIds.push(key)
    }*/
    let elem = document.getElementById("count");
    elem.innerHTML = count;

    alert("Added " + productName + " " + product.ProductName + " " + productId + " to Cart")

    xhr.send(JSON.stringify(product));
}