var _generateTable;
//http://stackoverflow.com/questions/3914483/how-to-define-a-new-global-function-in-javascript
(function () {
    _generateTable = function (data) {
        // Code here...
        var tablesResult = [];
        tablesResult.rows = data;
        if (data.hasOwnProperty("0")) {
            tablesResult.cols = Object.keys(data["0"]); 
        } else {
            tablesResult.cols = [];
        }
        return tablesResult;
    };
}());

var _copyObject;
//http://stackoverflow.com/questions/6620296/copying-javascript-object-attributes
(function () {
    //helper function to clone a given object instance
    _copyObject = function(srcObj, destObj) {        
        for (var key in srcObj) {
            //copy all the fields
            destObj[key] = srcObj[key];
        }        
    }
}());

var _getParameterByName;
//http://stackoverflow.com/questions/901115/how-can-i-get-query-string-values-in-javascript
(function () {
    _getParameterByName = function (name, url) {
        if (!url) url = window.location.href;
        name = name.replace("/[\[\]]/g", "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    };
}());


var _guid;
//http://stackoverflow.com/questions/105034/create-guid-uuid-in-javascript
(function () {
    _guid = function (data) {
        // Code here...
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
          s4() + '-' + s4() + s4() + s4();        
    };
}());

var _findIndexArrByAttr;
//http://stackoverflow.com/questions/7176908/how-to-get-index-of-object-by-its-property-in-javascript
(function () {
    _findIndexArrByAttr = function (array,attr,value) {
        for (var i = 0; i < array.length; i += 1) {
            if (array[i][attr] === value) {
                return i;
            }
        }
    };
}());

var _removeArray;
//http://stackoverflow.com/questions/10024866/remove-object-from-array-using-javascript
(function () {
    _removeArray = function (arrList, propName, propValue) {

        var indexArr = _findIndexArrByAttr(arrList, propName, propValue);
        //http://stackoverflow.com/questions/5767325/remove-a-particular-element-from-an-array-in-javascript
        if (indexArr > -1) {
            arrList.splice(indexArr, 1);
        }

        //return arrList.filter(function (val) {
        //    return val[propName] !== propValue;
        //});
    };  
}());

var _base64toBlob;
//http://stackoverflow.com/questions/16245767/creating-a-blob-from-a-base64-string-in-javascript
(function () {
    _base64toBlob = function (base64Data, contentType) {
        contentType = contentType || '';
        var sliceSize = 1024;
        var byteCharacters = atob(base64Data);
        var bytesLength = byteCharacters.length;
        var slicesCount = Math.ceil(bytesLength / sliceSize);
        var byteArrays = new Array(slicesCount);

        for (var sliceIndex = 0; sliceIndex < slicesCount; ++sliceIndex) {
            var begin = sliceIndex * sliceSize;
            var end = Math.min(begin + sliceSize, bytesLength);

            var bytes = new Array(end - begin);
            for (var offset = begin, i = 0 ; offset < end; ++i, ++offset) {
                bytes[i] = byteCharacters[offset].charCodeAt(0);
            }
            byteArrays[sliceIndex] = new Uint8Array(bytes);
        }
        return new Blob(byteArrays, { type: contentType });
    };
}());

(function () {
	'use strict';	

	Object.defineProperty(Array.prototype, 'count', {
		get: function () { return this.length; }
	});

	if (Array.prototype.addRange) return;

	Array.prototype.addRange = function (target) {
		this.push.apply(this, target);
	};
    //http://jsfiddle.net/johnpapa/ejafg/
	Array.prototype.inArray = function (searchFor, propertySearch) {
	    var searchArr = [];
	    var retVal = -1;
	    var self = this;
	    for (var index = 0; index < self.length; index++) {
	        var item = self[index];
	        if (propertySearch !== undefined && propertySearch !== null && item.hasOwnProperty(propertySearch) && typeof item[propertySearch] === 'string') {
	            //if (item[property].toLowerCase() === searchFor.toLowerCase()) {
	            if (item[propertySearch].toLowerCase().indexOf(searchFor.toLowerCase()) > -1) {
	                //retVal = index;
	                //return retVal;	              
	                //how-to-add-an-object-into-an-array
	                //http://stackoverflow.com/questions/6254050/how-to-add-an-object-into-an-array
	                searchArr.push(item);
	            }
	        } else {
	            for (var propertyName in item) {
	                if (item.hasOwnProperty(propertyName)) {
	                    if (propertyName !== 'guid' && typeof item[propertyName] === 'string' && item[propertyName].toLowerCase().indexOf(searchFor.toLowerCase()) > -1) {
	                        //how-to-add-an-object-into-an-array
	                        //http://stackoverflow.com/questions/6254050/how-to-add-an-object-into-an-array
	                        searchArr.push(item);
							break;
	                    }
	                }
	            }
	        }
	    };
	    return searchArr;
	};

})();

var _jsonToCsvConverter;
//http://stackoverflow.com/questions/28967782/ui-grid-using-external-pagination-and-exporting-data
(function () {
    _jsonToCsvConverter = function (JSONData, ReportTitle, ShowLabel) {
        //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
        var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

        var CSV = '';
        //Set Report title in first row or line

        //CSV += ReportTitle + '\r\n\n';

        //This condition will generate the Label/Header
        if (ShowLabel) {
            var row = "";

            //This loop will extract the label from 1st index of on array
            for (var index in arrData[0]) {

                //Now convert each value to string and comma-seprated
                if (index !== '$$hashKey')
                    row += index + ',';
            }

            row = row.slice(0, -1);

            //append Label row with line break
            CSV += row + '\r\n';
        }

        //1st loop is to extract each row
        for (var i = 0; i < arrData.length; i++) {
            var row = "";

            //2nd loop will extract each column and convert it in string comma-seprated
            for (var index in arrData[i]) {
                row += '"' + arrData[i][index] + '",';
            }

            row.slice(0, row.length - 1);

            //add a line break after each row
            CSV += row + '\r\n';
        }

        if (CSV == '') {
            alert("Invalid data");
            return;
        }

        //Generate a file name
        var fileName = "";// "MyReport_";
        //this will remove the blank-spaces from the title and replace it with an underscore
        fileName += ReportTitle.replace(/ /g, "_");

        //Initialize file format you want csv or xls
        var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

        // Now the little tricky part.
        // you can use either>> window.open(uri);
        // but this will not work in some browsers
        // or you will not get the correct file extension    

        //this trick will generate a temp <a /> tag
        var link = document.createElement("a");
        link.href = uri;

        //set the visibility hidden so it will not effect on your web-layout
        link.style = "visibility:hidden";
        link.download = fileName + ".csv";

        //this part will append the anchor tag and remove it after automatic click
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}());