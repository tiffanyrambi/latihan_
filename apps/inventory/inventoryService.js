mainProject.factory("inventoryService", function ($http) {
    //var getInventories = function (itemPerPage, pageNumber) {
    //    return $http.get("api/Inventory?itemPerPage=" + itemPerPage + "&pageNumber=" + pageNumber);
    //}
    var getInventories = function () {
        return $http.get("api/Inventory");
    }

    var getInventory = function (id) {
        return $http.get("api/Inventory?id=" + id);
    }

    var editInventory = function (inventory) {
        return $http.put("api/Inventory", inventory);
    }

    var saveInventory = function (inventory) {
        return $http.post("api/Inventory", inventory);
    }

    var deleteInventory = function (id) {
        return $http.delete("api/Inventory?id=" + id);
    }

    var inventorySimple = function (ajaxUrl) {
        return $http.get(ajaxUrl);
    };


    return {
        saveInventory: saveInventory,
        getInventory: getInventory,
        getInventories: getInventories,
        editInventory: editInventory,
        deleteInventory: deleteInventory,
        inventorySimple: inventorySimple
    }
});