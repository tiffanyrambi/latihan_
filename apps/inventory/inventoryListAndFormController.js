mainProject.controller("inventoryListAndFormController",
    function ($scope, $state, $stateParams, inventoryService) {

        initializeValues();


        $scope.delete = function (index) {
            //$scope.inventories.splice(index, 1);

            $scope.inventoriesTampung.splice($scope.inventoriesTampung.indexOf($scope.inventories[index]), 1);
            $scope.filteredData($scope.myValue);

        };

        $scope.submit = function () {
            //$scope.inventories.push($scope.inventoryMiniForm);
            //resetInventoryMiniForm();

            $scope.inventoriesTampung.push($scope.inventoryMiniForm);
            $scope.filteredData($scope.myValue);
            resetInventoryMiniForm();

        } 

        $scope.edit = function (index) {
            //var inv = $scope.inventories[index];
            //$scope.inventoryMiniForm = {
            //    inventoryCode: inv.inventoryCode,
            //    inventoryName: inv.inventoryName,
            //    inventoryQty: parseInt(inv.inventoryQty),
            //    inventoryPrice: parseInt(inv.inventoryPrice),
            //}
            //$scope.inventories.splice(index, 1);

            var inv = $scope.inventoriesTampung[$scope.inventoriesTampung.indexOf($scope.inventories[index])];
            $scope.inventoryMiniForm = {
                inventoryCode: inv.inventoryCode,
                inventoryName: inv.inventoryName,
                inventoryQty: parseInt(inv.inventoryQty),
                inventoryPrice: parseInt(inv.inventoryPrice),
            }
            $scope.inventoriesTampung.splice($scope.inventoriesTampung.indexOf($scope.inventories[index]), 1);
            $scope.filteredData($scope.myValue);
        }


        function initializeValues() {
            $scope.inventories = [
                {
                    inventoryCode: "STARTER_DUMMY",
                    inventoryName: "STARTER_DUMMY",
                    inventoryQty: "1",
                    inventoryPrice: "10000",
                },
                {
                    inventoryCode: "STARTER_DUMMY2",
                    inventoryName: "STARTER_DUMMY2",
                    inventoryQty: "2",
                    inventoryPrice: "20000",
                },
                {
                    inventoryCode: "STARTER_DUMMY3",
                    inventoryName: "STARTER_DUMMY3",
                    inventoryQty: "31",
                    inventoryPrice: "30000",
                },
                {
                    inventoryCode: "STARTER_DUMMY4",
                    inventoryName: "STARTER_DUMMY4",
                    inventoryQty: "42",
                    inventoryPrice: "40000",
                },
            ];

            $scope.inventoriesTampung = $scope.inventories;
            resetInventoryMiniForm();

           
            //getInventory();
        }

        function resetInventoryMiniForm() {
            $scope.inventoryMiniForm = {
                inventoryCode: null,
                inventoryName: null,
                inventoryQty: null,
                inventoryPrice: null,
            };
        }



        // filter
        $scope.filteredData = function (myValue) {
            //console.log($scope.inventories);

            //if (myValue.length !== 0) {
            //    var dataFilter = $scope.inventories.filter((inventory) =>
            //        inventory.inventoryQty.toString().includes(myValue) ||
            //        inventory.inventoryPrice.toString().includes(myValue) ||
            //        inventory.inventoryCode.toLowerCase().includes(myValue.toLowerCase()) ||
            //        inventory.inventoryName.toLowerCase().includes(myValue.toLowerCase())
            //    );
            //    $scope.inventories = dataFilter;
            //} else {
            //    $scope.inventories = $scope.inventoriesTampung;
            //}

            if (myValue.length !== 0) {
               var dataFilter = $scope.inventoriesTampung.filter((inventory) =>
                    inventory.inventoryQty.toString().includes(myValue) ||
                    inventory.inventoryPrice.toString().includes(myValue) ||
                    inventory.inventoryCode.toLowerCase().includes(myValue.toLowerCase()) ||
                    inventory.inventoryName.toLowerCase().includes(myValue.toLowerCase())
                );
                $scope.inventories = dataFilter;
            } else {
                $scope.inventories = $scope.inventoriesTampung;
            }

            console.log($scope.inventoriesTampung);

           
            
        };


    });