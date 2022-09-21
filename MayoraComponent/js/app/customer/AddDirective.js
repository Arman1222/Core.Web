(function () {
    "use strict";

    window.app.directive('addCustomer', addCustomer);

    function addCustomer() {
        return {
            templateUrl: MyApplication.rootPath  + '/customer/template/add.tmpl.cshtml',
            controller: controller,
            controllerAs: 'vm'
        }
    }    

    controller.$inject = ['$scope', 'customerService', 'customertemenosService', 'imageclassService','applicationusersService', 'alerts', 'Upload', '$timeout','MessageBox','blockUI'];
    function controller($scope, customerService, customertemenosService, imageclassService, applicationusersService, alerts, Upload, $timeout, MessageBox, blockUI) {
        var vm = this;
        vm.param = {searchtext: 'a'}
        //customerService.getAll().then(function (response) {
        //    vm.customertemenosGetPage = response.data;
        //});
        vm.mmm = {asa: ''};
        if ( vm.mmm === null || !(vm.mmm instanceof Array)) {
            vm.mmm = [];
        }
        vm.customertemenosGetPage = customertemenosService.getPage;
        vm.userInfo = {
                mDate: '1967-10-07'            
        };
        vm.testUploadfile = '89139ac6-61f5-4962-a6bd-d4aee9c3a88e-6 Info Perusahaan.JPG';
        vm.action = function () {
            vm.setImage();
        }
        vm.blur = function () {
            console.log("blur function active");
        }
        vm.change = function () {
            console.log("Change function active");
        }
        vm.tinymceOptions = {
            plugins: 'print textcolor advlist autolink lists link image charmap print preview hr anchor pagebreak searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking save table contextmenu directionality emoticons template paste textcolor colorpicker textpattern imagetools table',
            toolbar: "insertfile undo redo | styleselect | bold italic underline forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image"
        };

        vm.onUploadFileChange = function () {
            alert("Change");
        }

        vm.minDate = new Date();
        vm.minDate.setFullYear(2016, 1, 10);
         
        vm.maxDate = new Date();
        vm.maxDate.setFullYear(2100, 2, 10);

        vm.selectedMonthTest = new Date();

        vm.save = save;
        vm.title = "Add New Customer";
       
        vm.customer = {
            startTime: new Date()            
        };
        vm.applicationusersGetPage = applicationusersService.getPage;        
        vm.imageclassPicklist = imageclassService.picklist;
        vm.imageclassLoadAll = imageclassService.loadAll;
        vm.imageclassCombobox = {};

        applicationusersService.loadAll()
            .then(function (response) {
                var appUser = response;
            });

        imageclassService.combobox().then(function (data) {
            vm.imageclassCombobox = data;            
        });

        vm.gridOptions = {};

        vm.onFinishUpload = function () {
            customerService.uploadData(vm.gridOptions.data);
        }

        vm.testError = function () {           
            customerService.testError()
            .then(function (response) {
                MessageBox.show(response);
            })
            .catch(function (response) {
                MessageBox.show(response);                
            });
        }

        // Area Multi Complate

        vm.skillsList = [
            { id: 1, name: "Java" },
            { id: 2, name: "C" },
            { id: 3, name: "C++" },
            { id: 4, name: "Core Java" },
            { id: 5, name: "JavaScript" },
            { id: 6, name: "PHP" },
            { id: 7, name: "HTML" },
            { id: 8, name: "CSS" },
            { id: 9, name: "Angular Js" },
            { id: 10, name: "Bootstrap" }
        ];

        vm.selectItemCallback = function (item) {
            vm.selectedItem = item;
            console.log(item);
        };

        vm.removeItemCallback = function (item) {
            vm.removedItem = item;
            console.log(item);
        };
        // Area Multi Complate

        //vm.isNameNotValid = function (name) {
        //    if (vm.form != undefined && typeof vm.form[name] !== 'undefined') {
        //        if (vm.customer.name != 'Admin') {
        //            vm.form[name].$setValidity("server", false);
        //            return true;
        //        } else {
        //            vm.form[name].$setValidity("server", true);
        //        }
        //    }           
        //    return false;
        //}

        vm.isNameNotValid = function (name) {
                if (vm.customer.name != 'Admin') {                    
                    return true;
                } 
            return false;
        }

        //vm.isImageClassNotValid = function (name) {
        //    if (vm.form != undefined && typeof vm.form[name] !== 'undefined') {
        //        if (vm.selectedImageclass == undefined) {
        //            vm.form[name].$setValidity("server", false);
        //            return true;
        //        } else {
        //            vm.form[name].$setValidity("server", true);
        //        }
        //    }            
        //    return false;
        //}

        vm.isImageClassComboboxNotValid = function (name) {
            if (vm.selectedImageclassCombobox == undefined) {
                return true;
            }             
            return false;
        }
        vm.isImageClassPicklistNotValid = function (name) {
            if (vm.selectedImageclassPicklist == undefined) {
                return true;
            }
            return false;
        }

        //vm.isNominalNotValid = function (name) {
        //    if (vm.form != undefined && typeof vm.form[name] !== 'undefined') {
        //        if (vm.customer.nominal <= 0) {
        //            vm.form[name].$setValidity("server", false);
        //            return true;
        //        } else {
        //            vm.form[name].$setValidity("server", true);
        //        }
        //    }
        //    return false;
        //}

        vm.isNominalNotValid = function (name) {
            if (vm.customer.nominal <= 0) {
               return true;
            } 
            return false;
        }

        vm.uploadDataChange = function () {
            var modelUpload = vm.modelUpload;
            var uploadData = vm.uploadData;
        }

        vm.loadFile = function ($event) {
            if (event.fromElement.files.length == 0) {
                return (false);   // leave in case no file was chosen
            };
            alasql('SELECT * FROM FILE(?,{headers:true})', [event], function (data) {
                $scope.eData = data;     //  eData contains the JSON representation of the Excel sheet rows
            });
        };

        vm.uploadFile = function ($event) {
            var files = event.target.files;
            alasql('SELECT * FROM FILE(?,{headers:true})', [event], function (data) {
                $scope.eData = data;     //  eData contains the JSON representation of the Excel sheet rows
            });
        };
       
        vm.uploadFiles = function (files, errFiles) {
            vm.files = files;
            vm.errFiles = errFiles;
            angular.forEach(files, function (file) {
                file.upload = Upload.upload({
                    url: MyApplication.rootPath + 'customer/upload',
                    data: { file: file }
                });

                file.upload.then(function (response) {
                    $timeout(function () {
                        file.result = response.data;
                    });
                }, function (response) {
                    if (response.status > 0)
                        $scope.errorMsg = response.status + ': ' + response.data;
                }, function (evt) {
                    file.progress = Math.min(100, parseInt(100.0 *
                                             evt.loaded / evt.total));
                });
            });
        }        

        vm.onSelectImageClassCombobox = function () {
            console.log('selectedImageClassCombobox = ' + JSON.stringify(vm.selectedImageclassCombobox, null, 4));
            vm.customer.imageClassId = vm.selectedImageclassCombobox.imageClassId;
            vm.maxDate.setFullYear(2016, 10, 10);
        }

        vm.onSelectImageClassPicklist = function () {
            console.log('selectedImageClassPicklist = ' + JSON.stringify(vm.selectedImageclassPicklist, null, 4));
            vm.customer.imageClassId = vm.selectedImageclassPicklist.imageClassId;
        }

        vm.onChangeCustomerType = function () {
            console.log('onChangeCustomerType = ' + JSON.stringify(vm.customer.customerType, null, 4));
            vm.maxDate.setFullYear(2016, 5, 10);
        }

        vm.onBlurCustomerType = function () {
            console.log('onBlurCustomerType = ' + JSON.stringify(vm.customer.customerType, null, 4));          
        }

        vm.onChangeName = function () {
            console.log('onChangeName = ' + JSON.stringify(vm.customer.name, null, 4));
        }

        vm.onBlurName = function () {
            console.log('onBlurName = ' + JSON.stringify(vm.customer.name, null, 4));
        }

        vm.onChangeAddress = function () {
            console.log('onChangeAddress = ' + JSON.stringify(vm.customer.address, null, 4));
        }

        vm.onBlurAddress = function () {
            console.log('onBlurAddress = ' + JSON.stringify(vm.customer.address, null, 4));
        }

        vm.onChangeEmail = function () {
            console.log('onChangeEmail = ' + JSON.stringify(vm.customer.email, null, 4));
        }

        vm.onBlurEmail = function () {
            console.log('onBlurEmail = ' + JSON.stringify(vm.customer.email, null, 4));
        }

        vm.onChangePhone = function () {
            console.log('onChangePhone = ' + JSON.stringify(vm.customer.phone, null, 4));
        }

        vm.onBlurPhone = function () {
            console.log('onBlurPhone = ' + JSON.stringify(vm.customer.phone, null, 4));
        }

        vm.onChangeTerminationDate = function () {
            var aaa = vm.testDate;
            console.log('onChangeTerminationDate = ' + JSON.stringify(vm.customer.terminationDate, null, 4));
        }

        vm.onBlurTerminationDate = function () {
            console.log('onBlurTerminationDate = ' + JSON.stringify(vm.customer.terminationDate, null, 4));
        }

        vm.onChangeStartTime = function () {
            console.log('onChangeStartTime = ' + JSON.stringify($scope.vm.customer.startTime, null, 4));
        }

        vm.onBlurStartTime = function () {
            console.log('onBlurStartTime = ' + JSON.stringify(vm.customer.startTime, null, 4));
        }

        vm.onChangeNumberTest = function () {
            console.log('onChangeNumberTest = ' + JSON.stringify(vm.selectedNumberTest, null, 4));
        }

        vm.onChangeTotal = function () {
            console.log('onChangeTotal = ' + JSON.stringify(vm.customer.total, null, 4));
        }

        vm.onChangeDateTest = function () {
            console.log('onChangeDateTest = ' + JSON.stringify(vm.selectedDateTest, null, 4));
        }

        vm.onChangeMonthTest = function () {
            console.log('onChangeMonthTest = ' + JSON.stringify(vm.selectedMonthTest, null, 4));
        }

        function save() {            
            //blockUI.start('Saving...');
            MessageBox.confirm("Save", "Save Data", function () {
                 
                customerService.add(vm.customer)
                        .then(function (data) {
                            //Close the modal
                            $scope.$close();
                        })
                        .catch(function (response) {
                            if (response.data.errorMessage) {
                                vm.message.error("There was a problem saving the issue: <br/>" + response.data.errorMessage + "<br/>Please try again.");
                            } else {
                                vm.message.error("There was a problem saving the issue. Please try again.");
                            }
                        })
                        .finally(function () {
                            //blockUI.stop();
                        });

            });            
          
        }
    }
})();