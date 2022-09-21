var _convertDateStringsToDates;
//http://aboutcode.net/2013/07/27/json-date-parsing-angularjs.html
(function () {
    _convertDateStringsToDates = function (input) {
        // Code here...
        var regexIso8601 = /^(\d{4}|\+\d{6})(?:-(\d{2})(?:-(\d{2})(?:T(\d{2}):(\d{2}):(\d{2})(\.(\d{1,}))?(Z|([\-+])(\d{2}):(\d{2}))?)?)?)?$/;
        // Ignore things that aren't objects.
        if (typeof input !== "object") return input; 

        for (var key in input) {
            if (!input.hasOwnProperty(key)) continue;

            var value = input[key];
            var match;
            // Check for string properties which look like dates.
            if (typeof value === "string" && (match = value.match(regexIso8601))) {
                //var milliseconds = Date.parse(match[0]);               
                //if (!isNaN(milliseconds)) {
                //    input[key] = new Date(milliseconds);
                //}
                //http://stackoverflow.com/questions/2587345/javascript-date-parse
                var dateParse = new Date(match[1], match[2] - 1, match[3], match[4], match[5], match[6]);
                if(_isValidDate(dateParse)){
                    input[key] = dateParse;
                }                
            } else if (typeof value === "object") {
                // Recurse into object
                _convertDateStringsToDates(value);
            }
        }

    };
}());

//http://stackoverflow.com/questions/1353684/detecting-an-invalid-date-date-instance-in-javascript
var _isValidDate;
(function () {
    _isValidDate = function (d) {
        if (Object.prototype.toString.call(d) !== "[object Date]")
            return false;
        return !isNaN(d.getTime());
    }
}());
//http://stackoverflow.com/questions/7809156/why-datepicker-parsedate-is-not-working
var _parseDate;
(function () {
    _parseDate = function (format, dateStr) {
        if (dateStr === undefined || dateStr === "")
            return "";
        var date = null;
        try {
            date = $.datepicker.parseDate(format, dateStr);
        } catch (e) { console.log("Error Convert to Date : " + e); }
        return date;
    }
}());

var _parseDateToString;
(function () {
    _parseDateToString = function (srcFormat, destFormat, dateStr) {
        if (dateStr === undefined || dateStr === "")
            return "";
        var date = _parseDate(srcFormat, dateStr);
        if(date)
            return $.datepicker.formatDate(destFormat, date);
        return "";
    }
}());
//http://stackoverflow.com/questions/16229494/converting-excel-date-serial-number-to-date-using-javascript/16233621
var _parseExcelDateToJSDate;
(function () {
    _parseExcelDateToJSDate = function (dateStr5Digit) {
        if (dateStr5Digit === undefined || dateStr5Digit === "")
            return "";

        var utc_days = Math.floor(dateStr5Digit - 25569);
        var utc_value = utc_days * 86400;
        var date_info = new Date(utc_value * 1000);

        var fractional_day = dateStr5Digit - Math.floor(dateStr5Digit) + 0.0000001;

        var total_seconds = Math.floor(86400 * fractional_day);

        var seconds = total_seconds % 60;

        total_seconds -= seconds;

        var hours = Math.floor(total_seconds / (60 * 60));
        var minutes = Math.floor(total_seconds / 60) % 60;

        return new Date(date_info.getFullYear(), date_info.getMonth(), date_info.getDate(), hours, minutes, seconds);
    }
}());