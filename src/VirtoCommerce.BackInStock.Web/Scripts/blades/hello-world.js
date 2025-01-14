angular.module('BackInStock')
    .controller('BackInStock.helloWorldController', ['$scope', 'BackInStock.webApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'BackInStock';

        blade.refresh = function () {
            api.get(function (data) {
                blade.title = 'BackInStock.blades.hello-world.title';
                blade.data = data.result;
                blade.isLoading = false;
            });
        };

        blade.refresh();
    }]);
