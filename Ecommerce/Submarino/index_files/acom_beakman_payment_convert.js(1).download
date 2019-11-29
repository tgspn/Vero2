/////////////////////////////////////
//
// Script to use AB testing on payment page
//
/////////////////////////////////////////

var b2w = new B2Wab();
var pageAgent = new B2WabPageAgent();

/* build list of parameters dict to each product aton basket */
var convertAllProducts = function(productsOnCart, brand) {
	var productsRequestParameters = [];
	for(var prod of productsOnCart) {
		var storeName = 'bbox-exp01__'+brand;
		var experiment = storeName + "__" + parseInt(prod.product.department.replace("1000",""));
		var client_id = prod.additionalInfo.buyboxToken;
		var data =  {'client_id' : client_id, 'experiment_name' : experiment};
		try{
			productsRequestParameters.push(data);
		}catch(e){
			//do nothing
		}
	}
	b2w.convertAll(productsRequestParameters);
}

var findBrand = function(url) {
	if(url.indexOf("submarino") != -1){
		return "SUBA"
	} else if (url.indexOf("americanas") != -1){
		return "ACOM"
	} else if (url.indexOf("shoptime") != -1){
		return "SHOP"
	}
}

document.addEventListener('payment:screen:submit', function(event) {
	var brand = findBrand(event.srcElement.URL);
	convertAllProducts(event.detail.paymentState.cart.lines, brand);
	return true;
});

/*document.addEventListener('page:loadx', function(event) {
	var brand = findBrand(event);
	if(event.detail.page.type == 'thankyou'){
		convertAllProducts(event.detail.cart.lines, brand);
	}
	return true;
});
*/