angular.module('BackInStock')
    .controller('BackInStock.helloWorldController', ['$scope', 'BackInStock.webApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'BackInStock';

        blade.refresh = function () {
            api.get(function (data) {
                blade.title = 'BackInStock.blades.backInStock.title';
                blade.data = data.results;
                blade.isLoading = false;
            });
        };

        blade.refresh();
    }]);
