angular.module('BackInStock')
    .factory('BackInStock.webApi', ['$resource', function ($resource) {
        return $resource('api/back-in-stock');
    }]);
