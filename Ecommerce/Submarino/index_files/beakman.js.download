function B2Wab(pageAgent) {
    this.convertBaseURL = "https://beakman-server-v1.b2w.io/convert";
    this.pageAgent = (pageAgent ? pageAgent : new B2WabPageAgent());

    this.convert = function(parameters) {
		if (parameters['client_id'] == undefined || parameters['experiment_name'] == undefined) {
			throw new Error("'client_id' and 'experiment_name' are mandatory parameters.");
		}

		url = this.convertBaseURL + '?experiment='+parameters['experiment_name']+'&clientId='+parameters['client_id'];
		console.log('request [PUT] /convert to : '+ url);
		this.pageAgent.putRequest(url);
    };

  	this.convertAll = function(parameters) {
    console.log('beakmanjs - convert products on Cart');
		for (var data of parameters) {
			this.convert(data);
		}
	};

};

function B2WabPageAgent() {
    this.timeout = 1000;

	this.getRequest = function(endpoint) {
		if (endpoint == undefined || endpoint.length == 0) {
			throw new Error("endpoint cannot be undefined or empty");
		}

		var xhttp = new XMLHttpRequest();
		var is_async = true;
		xhttp.open("GET", endpoint, is_async);
		xhttp.send();
	};

	this.putRequest = function(endpoint, bodyRequestData) {
		if (endpoint == undefined || endpoint.length == 0) {
			throw new Error("endpoint cannot be undefined or empty");
		}

		var xhttp = new XMLHttpRequest();
		var is_async = true;
		xhttp.open("PUT", endpoint, is_async);
		xhttp.send("{}");
	};

	this.getSessionID = function() {
		var name = "B2W-SID";
		var value = "; " + document.cookie;
		var parts = value.split("; " + name + "=");
		if (parts.length == 2) {
			return parts.pop().split(";").shift();
		}
	};

};
