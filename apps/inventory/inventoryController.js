mainProject.controller("inventoryController",
    function ($scope, $state, $stateParams, inventoryService) {

        initializeValues();
        if ($stateParams.id) {
            inventoryService.getInventory($stateParams.id).then(
                function (onSuccess) {
                    $scope.inventory = onSuccess.data;


                    // -----------------------------------------------
                    // Extract and store inventory IDs for later use
                    $scope.inventoryIds = onSuccess.data.map(function (inventory) {
                        return inventory.InventoryId;
                    });
                    // --------------------------------------------------------



                    });
                }, function (onError) {
                    alert(onError.data.message);
                });
        }


        $scope.submit = function (inventory) {
            if ($stateParams.id) {
                inventoryService.editInventory(inventory).then(
                    function (onSuccess) {
                        $state.go("inventoryList");
                        alert(onSuccess.data.message);
                    }, function (onError) {



                        alert(onError.data.message);
                    })
            } else {
                inventoryService.saveInventory(inventory).then(
                    function (onSuccess) {
                        $state.go("inventoryList");
                        alert(onSuccess.data.message);
                    }, function (onError) {
                        alert(onError.data.message);
                    })
            }
        }


        function initializeValues() {
            $scope.inventories = [];
            $scope.inventory = {
                inventoryQty: 0,
                inventoryPrice: 0
            };
        }

    });