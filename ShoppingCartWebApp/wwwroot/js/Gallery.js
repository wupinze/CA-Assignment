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

        //GetCartCount();
        ProductIntoCartProduct(target.id, (target.classList[0] + " " + target.classList[1]));
        GetCartCount();
    }
}


function GetCartCount() {
    let xhr = new XMLHttpRequest();

    xhr.open("GET", "/Gallery/CartCount")
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");
    xhr.send()
    
    xhr.onreadystatechange = function () {
        
        if (this.readyState == XMLHttpRequest.DONE) {
            

            let data = this.responseText;
            //console.log(data)

            let elem = document.getElementById("count");
            elem.innerHTML = data;
        }
    }
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
                window.location.reload(true);
            }
        }
    }

    //package as Json object
    let product = {
        ProductId: productId,
        ProductName: productName,
    };

    alert("Added " + productName + " to Cart")

    xhr.send(JSON.stringify(product));
}

