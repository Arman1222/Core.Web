(function () {
    'use strict';

    window.app.controller('CustomerController', CustomerController);

    CustomerController.$inject = ['$scope', '$modal', 'customerService', 'alerts', 'customertemenosService'];
    function CustomerController($scope, $modal, customerService, alerts, customertemenosService) {
        var vm = this;
        vm.add = add;
        vm.detail = detail;
        vm.edit = edit;
        vm.customersFunction = customerService.getPage;
        vm.customersGetAll = customerService.getAll;
        vm.customerstemenosGetPage = customertemenosService.getPage;
        vm.message = alerts;
        vm.loading = true;
        var params___ = {
            namaSP: 'sp_percobaan',
            params_: []
        }

        vm.refreshData2 = function () {
            params___.params_ = [{ name: 'pUserID', value: 'ichwan', size: 20 }, { name: 'pPassword', value: 'ichwan', size: 50 }]
            debugger;
            customerService.ExecStoreProcedure(params___).then(
            function (response) {
                vm.customerArray = response.data[0];
            });
        }
        vm.refreshData2();

        vm.onBlurEditedCell = function (val) {
            alert("merubah : " + val.oldValue + " menjadi " + val.newValue + " pada Column " + val.colName);
            console.log(val);
        }
        //throw new Error("test");
        vm.exportWord = function () {
            customerService.exportWord()
                .then(function (response) {
                    var file = _base64toBlob(response.data, response.contentType); //new Blob(response.data, { type: response.contentType });
                    var fileURL = URL.createObjectURL(file);
                    window.open(fileURL);
                });
        }

        vm.exportPdf = function () {
            customerService.exportPdf()
                .then(function (response) {
                    var file = _base64toBlob(response.data, response.contentType); //new Blob(response.data, { type: response.contentType });
                    var fileURL = URL.createObjectURL(file);
                    window.open(fileURL);
                });
        }

        vm.rowStyle = function (entity) {
            if (entity && entity.customerType === 'Corporate') {
                return { background: 'green' };
            }
            return {};
        }

        vm.onLoadTableCustomer =
            function () {
                return new Promise(function (resolve, reject) {
                    var a = 10
                    if (a == 10)
                        resolve(a);
                    else
                        reject(a);
                })
                .then(function (e) {
                    console.log('done', e);
                })
                .catch(function (e) {
                    console.log('catch: ', e);
                });
            };


        $scope.$watch('vm.totalItems', function (value) {
            if (value) {
                console.log(value);
                var totalItems = vm.totalItems;
            }
        });

        $scope.$watch('vm.response', function (value) {
            if (value) {
                console.log(value);
                var response = vm.response;
            }
        });

        vm.tableActions = {
            onEdit: edit,
            onDetail: detail,
            onDelete: function (value) {
                alert('Delete : ' + value);
            },
            onClick: function (value) {
                //alert('Click : ' + value);
            },
            onDblClick: function (value) {
                //alert('DblClick : ' + value);
            }
        };

        function add() {
            var modalInstance = $modal.open({
                windowClass: 'form-modal-window-1200',
                template: '<add-customer />'
            });

            modalInstance.result.then(function () {
                vm.refreshData();
            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
            });
        }

        function edit(data) {
            var modalInstance = $modal.open({
                template: '<edit-customer data="data" />',
                scope: angular.extend($scope.$new(true), { data: data })
            });

            modalInstance.result.then(function () {
                vm.refreshData();
            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
            });
        }

        function detail(data) {
            $modal.open({
                template: '<details-customer data="data" />',
                scope: angular.extend($scope.$new(true), { data: data })
            });
        }
        vm.gridOptionsaaa = {
            enableRowSelection: true,
            enableSelectAll: true,
            selectionRowHeaderWidth: 35,
            rowHeight: 35,
            showGridFooter: true,
            multiSelect: true,
            columnDefs: [
              { name: 'id' },
              { name: 'name' },
              { name: 'age', displayName: 'Age (not focusable)', allowCellFocus: false },
              { name: 'address.city' }
            ],
            data: [{
                "id": 0,
                "guid": "de3db502-0a33-4e47-a0bb-35b6235503ca",
                "isActive": false,
                "balance": "$3,489.00",
                "picture": "http://placehold.it/32x32",
                "age": 30,
                "name": "Sandoval Mclean",
                "gender": "male",
                "company": "Zolavo",
                "email": "sandovalmclean@zolavo.com",
                "phone": "+1 (902) 569-2412",
                "address": {
                    "street": 317,
                    "city": "Blairstown",
                    "state": "Maine",
                    "zip": 390
                },
                "about": "Fugiat velit laboris sit est. Amet eu consectetur reprehenderit proident irure non. Adipisicing mollit veniam enim veniam officia anim proident excepteur deserunt consectetur aliquip et irure. Elit aliquip laborum qui elit consectetur sit proident adipisicing.\r\n",
                "registered": "1991-02-21T23:02:31+06:00",
                "friends": [
                    {
                        "id": 0,
                        "name": "Rosanne Barrett"
                    },
                    {
                        "id": 1,
                        "name": "Nita Chase"
                    },
                    {
                        "id": 2,
                        "name": "Briggs Stark"
                    }
                ]
            },
            {
                "id": 1,
                "guid": "9f507483-5ecc-4af4-800f-349306820585",
                "isActive": false,
                "balance": "$2,407.00",
                "picture": "http://placehold.it/32x32",
                "age": 22,
                "name": "Nieves Mack",
                "gender": "male",
                "company": "Oulu",
                "email": "nievesmack@oulu.com",
                "phone": "+1 (812) 535-2614",
                "address": {
                    "street": 155,
                    "city": "Cherokee",
                    "state": "Kentucky",
                    "zip": 4723
                },
                "about": "Culpa anim anim nulla deserunt dolor exercitation eu in anim velit. Consectetur esse cillum ea esse ullamco magna do voluptate sit ut cupidatat ullamco. Et consequat eu excepteur do Lorem aute est quis proident irure.\r\n",
                "registered": "1989-07-26T15:52:15+05:00",
                "friends": [
                    {
                        "id": 0,
                        "name": "Brewer Maxwell"
                    },
                    {
                        "id": 1,
                        "name": "Ayala Franks"
                    },
                    {
                        "id": 2,
                        "name": "Hale Nichols"
                    }
                ]
            },
            {
                "id": 2,
                "guid": "58c66190-15be-4e75-9b09-183599403241",
                "isActive": false,
                "balance": "$3,409.00",
                "picture": "http://placehold.it/32x32",
                "age": 20,
                "name": "Terry Clay",
                "gender": "female",
                "company": "Freakin",
                "email": "terryclay@freakin.com",
                "phone": "+1 (965) 462-3681",
                "address": {
                    "street": 124,
                    "city": "Wright",
                    "state": "Pennsylvania",
                    "zip": 8002
                },
                "about": "Exercitation exercitation adipisicing eu cupidatat reprehenderit laborum incididunt reprehenderit Lorem anim. Velit aliquip dolore qui excepteur dolor non occaecat aute et. Consectetur anim veniam irure ea id aliqua amet. Nostrud tempor ullamco velit labore consequat aute nostrud nostrud veniam cupidatat amet nostrud quis. Qui exercitation eiusmod esse eu officia officia Lorem Lorem ullamco voluptate excepteur fugiat nulla et. Ea ipsum ut do culpa labore non duis commodo sit. Id sint dolor ipsum consectetur nostrud nulla consectetur esse deserunt.\r\n",
                "registered": "2000-12-02T22:19:28+06:00",
                "friends": [
                    {
                        "id": 0,
                        "name": "Etta Hawkins"
                    },
                    {
                        "id": 1,
                        "name": "Zamora Barlow"
                    },
                    {
                        "id": 2,
                        "name": "Lynette Vinson"
                    }
                ]
            },
            {
                "id": 3,
                "guid": "0a1b0539-73ec-473a-846a-71a58e04551c",
                "isActive": false,
                "balance": "$3,567.00",
                "picture": "http://placehold.it/32x32",
                "age": 21,
                "name": "Bishop Carr",
                "gender": "male",
                "company": "Digirang",
                "email": "bishopcarr@digirang.com",
                "phone": "+1 (860) 463-2942",
                "address": {
                    "street": 824,
                    "city": "Homeworth",
                    "state": "Oklahoma",
                    "zip": 5215
                },
                "about": "Nulla ullamco sint exercitation minim ea sunt. Excepteur minim tempor velit in. Proident id reprehenderit nisi officia in anim elit laboris aute sint amet voluptate. Deserunt et nostrud magna eu esse ea adipisicing non quis sint fugiat consectetur enim sint. Magna elit mollit eiusmod non voluptate sunt.\r\n",
                "registered": "2012-10-15T19:03:24+05:00",
                "friends": [
                    {
                        "id": 0,
                        "name": "Young Gentry"
                    },
                    {
                        "id": 1,
                        "name": "Dean Lopez"
                    },
                    {
                        "id": 2,
                        "name": "Mccray Bradford"
                    }
                ]
            },
            {
                "id": 4,
                "guid": "f82261a1-71d0-4d96-aeb6-03e300112f18",
                "isActive": true,
                "balance": "$1,931.00",
                "picture": "http://placehold.it/32x32",
                "age": 33,
                "name": "Hatfield Hudson",
                "gender": "male",
                "company": "Quonata",
                "email": "hatfieldhudson@quonata.com",
                "phone": "+1 (981) 476-2966",
                "address": {
                    "street": 853,
                    "city": "Bynum",
                    "state": "Rhode Island",
                    "zip": 3382
                },
                "about": "In fugiat elit ipsum qui occaecat elit enim eu labore. Esse incididunt adipisicing nostrud veniam proident duis ex aute sit id. Exercitation occaecat nisi incididunt ut esse nostrud pariatur. Consectetur culpa minim deserunt minim proident consectetur incididunt enim duis adipisicing pariatur proident.\r\n",
                "registered": "2000-09-05T10:41:58+05:00",
                "friends": [
                    {
                        "id": 0,
                        "name": "Munoz Sharp"
                    },
                    {
                        "id": 1,
                        "name": "Louella Vaughn"
                    },
                    {
                        "id": 2,
                        "name": "Cleveland Parker"
                    }
                ]
            },
            {
                "id": 5,
                "guid": "3eeb5290-1357-4c8b-8ca3-ea9f01521928",
                "isActive": false,
                "balance": "$2,215.00",
                "picture": "http://placehold.it/32x32",
                "age": 29,
                "name": "Madge Wilkerson",
                "gender": "female",
                "company": "Mixers",
                "email": "madgewilkerson@mixers.com",
                "phone": "+1 (947) 551-2199",
                "address": {
                    "street": 374,
                    "city": "Springdale",
                    "state": "Minnesota",
                    "zip": 7453
                },
                "about": "Officia laboris laborum dolore ad minim ad mollit et excepteur adipisicing do non nostrud officia. Anim in exercitation dolor cupidatat deserunt. Commodo excepteur aliqua consequat do. Aliquip incididunt quis sunt cillum reprehenderit consequat.\r\n",
                "registered": "2005-12-16T01:13:09+06:00",
                "friends": [
                    {
                        "id": 0,
                        "name": "Tabatha Mclaughlin"
                    },
                    {
                        "id": 1,
                        "name": "Letitia Evans"
                    },
                    {
                        "id": 2,
                        "name": "Greta Sykes"
                    }
                ]
            },
            {
                "id": 6,
                "guid": "29bc47e3-5275-49be-b9cf-95853f1c5801",
                "isActive": true,
                "balance": "$3,623.00",
                "picture": "http://placehold.it/32x32",
                "age": 30,
                "name": "Harrell Gaines",
                "gender": "male",
                "company": "Namebox",
                "email": "harrellgaines@namebox.com",
                "phone": "+1 (902) 410-2375",
                "address": {
                    "street": 639,
                    "city": "Jackpot",
                    "state": "Virginia",
                    "zip": 4822
                },
                "about": "Magna non sit laboris amet Lorem occaecat tempor aute cillum ut dolore dolor pariatur. Amet consequat id consequat id esse aliquip. Irure anim ex veniam aliquip magna aute velit qui duis minim.\r\n",
                "registered": "1998-08-08T13:08:45+05:00",
                "friends": [
                    {
                        "id": 0,
                        "name": "Beatriz Lancaster"
                    },
                    {
                        "id": 1,
                        "name": "Cora Lawrence"
                    },
                    {
                        "id": 2,
                        "name": "Elva Pate"
                    }
                ]
            },
            {
                "id": 7,
                "guid": "7e7aba67-7562-4bea-9a16-86108f41b4b4",
                "isActive": true,
                "balance": "$2,731.00",
                "picture": "http://placehold.it/32x32",
                "age": 23,
                "name": "Christensen Wall",
                "gender": "male",
                "company": "Elentrix",
                "email": "christensenwall@elentrix.com",
                "phone": "+1 (985) 594-3954",
                "address": {
                    "street": 510,
                    "city": "Vandiver",
                    "state": "Colorado",
                    "zip": 5384
                },
                "about": "Est quis nostrud elit sint commodo consectetur ea ullamco tempor voluptate veniam reprehenderit. Elit Lorem aliqua dolore commodo officia labore. Cupidatat proident qui ullamco in cillum.\r\n",
                "registered": "1992-06-19T22:03:28+05:00",
                "friends": [
                    {
                        "id": 0,
                        "name": "Olsen Rosario"
                    },
                    {
                        "id": 1,
                        "name": "Janelle Mcintosh"
                    },
                    {
                        "id": 2,
                        "name": "Dorothy Gallegos"
                    }
                ]}]
        };

        vm.gridOptionsaaa.multiSelect = true;

    }
})();