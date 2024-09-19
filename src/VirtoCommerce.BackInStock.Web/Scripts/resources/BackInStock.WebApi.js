angular.module('VirtoCommerce.BackInStock')
.factory('BackInStock.WebApi', ['$resource', function ($resource) {
    return $resource('api/backInStock', {}, {
        search: { method: 'POST', url: 'api/backInStock/search' },
        update: { method: 'POST' },
        delete: { method: 'DELETE' },
    });
    }]);
