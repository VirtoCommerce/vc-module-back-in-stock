angular.module('BackInStockModule')
    .controller('BackInStockModule.helloWorldController', ['$scope', 'BackInStockModule.webApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'BackInStockModule';

        blade.refresh = function () {
            api.get(function (data) {
                blade.title = 'BackInStockModule.blades.hello-world.title';
                blade.data = data.result;
                blade.isLoading = false;
            });
        };

        blade.refresh();
    }]);
