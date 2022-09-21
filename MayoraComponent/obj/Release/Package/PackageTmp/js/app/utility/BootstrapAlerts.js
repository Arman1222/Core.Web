(function () {

	var alertService = {
		showAlert: showAlert,
		success: success,
		info: info,
		warning: warning,
		error: error
	}; 

	window.alerts = alertService;

	var alertContainer = $(".alert-container");

	var template = _.template("<div class='alert <%= alertClass %> alert-dismissable'>" +
		"<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>" +
		"<%= message %>" +
		"</div>");

	function showAlert(alert, element) {
	    var alertElement = $(template(alert));

	    if (element != undefined)
	    {
	        element.append(alertElement); //untuk error di form
	    }
	    else {
	        alertContainer.append(alertElement); //untuk error di global / index 
	    }

		//window.setTimeout(function () {
		//	alertElement.fadeOut();
		//}, 5000);
	}

	function success(message, element) {
	    showAlert({ alertClass: "alert-success", message: message }, element);
	}

	function info(message, element) {
	    showAlert({ alertClass: "alert-info", message: message }, element);
	}

	function warning(message, element) {
	    showAlert({ alertClass: "alert-warning", message: message }, element);
	}

	function error(message, element) {	  
	    showAlert({ alertClass: "alert-danger", message: message }, element);
	}

})();
