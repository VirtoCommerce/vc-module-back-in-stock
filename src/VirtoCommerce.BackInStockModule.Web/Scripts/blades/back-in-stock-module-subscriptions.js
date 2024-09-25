angular.module('VirtoCommerce.BackInStockModule')
    .controller('VirtoCommerce.BackInStockModule.backInStockSubscriptionsController',
        ['$scope', 'VirtoCommerce.BackInStockModule.subscriptions.webApi',
        function ($scope, subscriptionsApi) {
        var blade = $scope.blade;
        blade.title = 'Back In Stock';

        blade.refresh = function () {
            blade.isLoading = true;
            subscriptionsApi.search(angular.extend(filter, {
                searchPhrase: filter.keyword ? filter.keyword : undefined,
                sort: uiGridHelper.getSortExpression($scope),
                skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                take: $scope.pageSettings.itemsPerPageCount
            }), function (data) {
                blade.isLoading = false;
                $scope.pageSettings.totalItems = data.totalCount;
                blade.currentEntities = data.results;
            });
        }
    }]);
