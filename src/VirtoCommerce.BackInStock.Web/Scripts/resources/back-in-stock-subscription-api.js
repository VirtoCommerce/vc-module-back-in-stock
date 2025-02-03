angular.module('VirtoCommerce.BackInStock')
    .factory('VirtoCommerce.BackInStock.subscriptionApi', ['$resource', function ($resource) {
        return $resource('api/back-in-stock/subscriptions', {}, {
            search: { method: 'POST', url: 'api/back-in-stock/subscriptions/search' },
        });
    }]);
