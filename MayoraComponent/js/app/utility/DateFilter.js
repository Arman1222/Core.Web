(function() {
	window.app.filter('parseDate', parseDate);

	function parseDate() {
		return function(input) {
			if (typeof input != 'string' || input.indexOf('/Date') === -1) return input;

			return new Date(parseInt(input.substr(6))); 
		}
	}

    //http://www.angulartutorial.net/2014/04/date-filtering-and-formatting-in.html
	window.app.filter('dateFormat', dateFormat);
	function dateFormat($filter) {
	    return function (input) {	        
	        if (input == null) { return ""; }

	        var _date = null;
	        if (input instanceof Date) {
	            _date = $filter('date')(input, 'dd/MM/yyyy');
	        } else {
	            _date = $filter('date')(new Date(input), 'dd/MM/yyyy');
	        }

	        return _date.toUpperCase();	        
	    };
	};

	window.app.filter('dateTimeFormat', dateTimeFormat);
	function dateTimeFormat($filter) {
	    return function (input) {
	        if (input == null) { return ""; }

	        var _date = null;
	        if (input instanceof Date) {
	            _date = $filter('date')(input, 'dd/MM/yyyy HH:mm:ss');
	        } else {
	            _date = $filter('date')(new Date(input), 'dd/MM/yyyy HH:mm:ss');
	        }       
                        
	        return _date.toUpperCase();
	    };
	};

	window.app.filter('timeFormat', function ($filter) {
	    return function (input) {
	        if (input == null) { return ""; }

	        var _date = $filter('date')(new Date(input), 'HH:mm:ss');

	        return _date.toUpperCase();

	    };
	});

})();