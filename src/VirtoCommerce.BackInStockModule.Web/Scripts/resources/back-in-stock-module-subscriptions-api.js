angular.module('VirtoCommerce.BackInStockModule')
    .factory('VirtoCommerce.BackInStockModule.subscriptions.webApi', ['$resource', function ($resource) {
        return $resource('api/back-in-stock-module/subscription', {}, {
            search: {method: 'POST', url: 'api/back-in-stock-module/subscription/search'},
            update: {method: 'POST'},
            delete: {method: 'DELETE'},
        });

    }]);
