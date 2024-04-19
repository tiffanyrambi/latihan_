mainProject.controller("inventoryListController",
    function ($scope, $state, $stateParams, inventoryService) {

        initializeValues();


        $scope.delete = function (id) {
            inventoryService.deleteInventory(id).then(
                function (onSuccess) {
                    alert("berhasil hapus");
                    $scope.getInventory();
                })
        };


        function initializeValues() {
            $scope.inventories = [];

            //// push another data
            //$scope.inventories.push(
            //    {
            //        inventoryCode: "g",
            //        inventoryName: "gibs",
            //        inventoryQty: 5,
            //        inventoryPrice: 4300
            //    },
            //    {
            //        inventoryCode: "h",
            //        inventoryName: "hippy",
            //        inventoryQty: 6,
            //        inventoryPrice: 4200
            //    },
            //    {
            //        inventoryCode: "h",
            //        inventoryName: "hippy",
            //        inventoryQty: 6,
            //        inventoryPrice: 4200
            //    }
            //);

            // FILTER data => qty > 3
            //$scope.inventories = $scope.inventories.filter((inventory) => inventory.inventoryQty > 3);

            // FILTER bcd, yg dilihat hanya huruf pertama dari string inventoryCode
            //$scope.inventories = $scope.inventories.filter((inventory) => inventory.inventoryCode.charAt(0) !== "b" && inventory.inventoryCode.charAt(0) !== "c" && inventory.inventoryCode.charAt(0) !== "d"  );

            // SPLICE --> masukin data di antara e dan f

            //$scope.inventories.splice(7, 0,
            //    {
            //        "inventoryCode": "aa",
            //        "inventoryName": "inserted",
            //        "inventoryQty": 7,
            //        "inventoryPrice": 7
            //    }
            //);

            //$scope.addedData = 
            //    {
            //        "inventoryCode": "inserted",
            //        "inventoryName": "inserted",
            //        "inventoryQty": 7,
            //        "inventoryPrice": 7
            //    };

            //$scope.inventories = $scope.inventories.toSpliced(6,0,$scope.addedData);

            // INDEXOF & LOOPING --> nilai quantity = nilai index

            //for (let i = 0; i < $scope.inventories.length; i++) {
            //    //$scope.inventories[i].inventoryQty = i + 1;
            //    $scope.inventories[i].inventoryQty = $scope.inventories.indexOf($scope.inventories[i]) + 1;
            //};

            //$scope.itemPerPage = 5;
            //$scope.pageNumber = 1;

            getInventory();
        }


        // NG-CHANGE & FILTER values (inventoryCode, inventoryName, inventoryQty, inventoryPrice)
        $scope.myFunc = function (myValue) {
             initializeValues();
            if (myValue.length !== 0) {
                $scope.inventories = $scope.inventories.filter((inventory) =>
                    inventory.inventoryQty.toString().includes(myValue) ||
                    inventory.inventoryPrice.toString().includes(myValue) ||
                    inventory.inventoryCode.includes(myValue) ||
                    inventory.inventoryName.includes(myValue)
                );
            } 
            
        };


        //var hitungQty = false;
        //var hitungPrice = false;
        //var hitungName = false;
        //var hitungCode = false;

        // SORT TABLE

        // sort qty
        //$scope.sortQty = function () {

        //    if (hitungQty == false) {
        //        // asc
        //        $scope.inventories = $scope.inventories.sort((a, b) => a.inventoryQty - b.inventoryQty);
        //    } else {
        //        // desc
        //        $scope.inventories = $scope.inventories.sort((b, a) => a.inventoryQty - b.inventoryQty);
        //    }
        //    hitungQty = !hitungQty;
        //};

        // sort price
        //$scope.sortPrice = function () {

        //    if (hitungPrice == false) {
        //        // asc
        //        $scope.inventories = $scope.inventories.sort((a, b) => a.inventoryPrice - b.inventoryPrice);
        //    } else {
        //        // desc
        //        $scope.inventories = $scope.inventories.sort((b, a) => a.inventoryPrice - b.inventoryPrice);
        //    }
        //    hitungPrice = !hitungPrice;
        //};

        // sort name
        //$scope.sortName = function () {

        //    if (hitungName == false) {
        //        $scope.inventories.sort((a, b) => {
        //            var nameA = a.inventoryName.toUpperCase(); // ignore upper and lowercase
        //            var nameB = b.inventoryName.toUpperCase(); // ignore upper and lowercase
        //            if (nameA < nameB) {
        //                return -1;
        //            }
        //            if (nameA > nameB) {
        //                return 1;
        //            }

        //            // names must be equal
        //            return 0;
        //        });
        //    } else {
        //        $scope.inventories.sort((a, b) => {
        //            var nameA = a.inventoryName.toUpperCase(); // ignore upper and lowercase
        //            var nameB = b.inventoryName.toUpperCase(); // ignore upper and lowercase
        //            if (nameA > nameB) {
        //                return -1;
        //            }
        //            if (nameA < nameB) {
        //                return 1;
        //            }

        //            // names must be equal
        //            return 0;
        //        });
        //    }
        //    hitungName = !hitungName;
           
        //}

        // sort code 
        //$scope.sortCode = function () {

        //    if (hitungCode == false) {
        //        $scope.inventories.sort((a, b) => {
        //            var nameA = a.inventoryCode.toUpperCase(); // ignore upper and lowercase
        //            var nameB = b.inventoryCode.toUpperCase(); // ignore upper and lowercase
        //            if (nameA < nameB) {
        //                return -1;
        //            }
        //            if (nameA > nameB) {
        //                return 1;
        //            }

        //            // names must be equal
        //            return 0;
        //        });
        //    } else {
        //        $scope.inventories.sort((a, b) => {
        //            var nameA = a.inventoryCode.toUpperCase(); // ignore upper and lowercase
        //            var nameB = b.inventoryCode.toUpperCase(); // ignore upper and lowercase
        //            if (nameA > nameB) {
        //                return -1;
        //            }
        //            if (nameA < nameB) {
        //                return 1;
        //            }

        //            // names must be equal
        //            return 0;
        //        });
        //    }

        //    hitungCode = !hitungCode;

        //}
        

        //function f(num) {
        //    return num + 1;
        //}
        //function s(num) {
        //    num = num + 1;
        //    return;
        //}

        function getInventory() {
            //inventoryService.getInventories($scope.itemPerPage, $scope.pageNumber).then(
            inventoryService.getInventories().then(
                function (onSuccess) {
                    //$scope.inventories = db.inventories.orderby.skip().take()
                    $scope.inventories = onSuccess.data;

                    var capitalizedData = $scope.inventories.map(function (item) {
                        var capitalizedInventory = {};
                        capitalizedInventory.InventoryCode = item.inventoryCode; // Assuming you want to capitalize the first letter of each property
                        capitalizedInventory.InventoryName = item.inventoryName;
                        capitalizedInventory.InventoryQty = item.inventoryQty;
                        capitalizedInventory.BackOrderSalesOrder = item.backOrderSalesOrder;
                        capitalizedInventory.InventoryQtyOutgoing = item.inventoryQtyOutgoing;
                        capitalizedInventory.InventoryQtyFinal = item.inventoryQtyFinal;
                        capitalizedInventory.InventoryPrice = item.inventoryPrice;
                        //capitalizedInventory.InventoryId = item.inventoryId;
                        return capitalizedInventory;
                    });

                    //new addition

                    //my function is inferior...i'll use the one from stackexchange

                    //var tempArray = [];
                    //for (inv of $scope.inventories) {
                    //    tempArray.push(inv);
                    //    if (tempArray.length >= $scope.itemPerPage) {
                    //        $scope.inventoriesSplitIntoPages.push(tempArray);
                    //        tempArray = [];
                    //    }
                    //}
                    //$scope.getInventory();


                    //for (let i = 0; i < $scope.inventories.length; i += $scope.itemPerPage) {
                    //    const chunk = $scope.inventories.slice(i, i + $scope.itemPerPage);
                    //    $scope.inventoriesSplitIntoPages.push(chunk)
                    //}

                    //console.log($scope.inventories);
                    //console.log($scope.inventoriesSplitIntoPages);


                }, function (onError) {
                    $message = onError.data.message;
                });
        }

        $scope.getInventory = function () {
            getInventory();

        };

    });