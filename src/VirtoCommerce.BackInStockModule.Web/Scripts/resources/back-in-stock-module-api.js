angular.module('BackInStockModule')
    .factory('BackInStockModule.webApi', ['$resource', function ($resource) {
        return $resource('api/back-in-stock-module');
    }]);
