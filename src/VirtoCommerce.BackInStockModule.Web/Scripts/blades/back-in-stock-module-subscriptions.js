angular.module('VirtoCommerce.BackInStockModule')
    .controller('VirtoCommerce.BackInStockModule.backInStockSubscriptionsListController',
        ['$scope', 'VirtoCommerce.BackInStockModule.subscriptions.webApi', 'platformWebApp.bladeUtils', 'uiGridConstants',
            'platformWebApp.uiGridHelper', 'platformWebApp.authService', 'virtoCommerce.catalogModule.items',
            function ($scope, subscriptionsApi, bladeUtils, uiGridConstants, uiGridHelper, authService, catalogItems) {
                $scope.gridOptions = {};
                $scope.uiGridConstants = uiGridConstants;
                $scope.setGridOptions = function (gridOptions) {
                    uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                        uiGridHelper.bindRefreshOnSortChanged($scope);
                    });
                    bladeUtils.initializePagination($scope);
                };


                let blade = $scope.blade;
                blade.title = 'Back In Stock';
                blade.headIcon = 'fa fa-comments';
                blade.headIcon = 'fa fa-comments';
                blade.title = 'Rating and Reviews';

                blade.toolbarCommands = [
                    {
                        name: "platform.commands.refresh", icon: 'fa fa-refresh',
                        executeMethod: blade.refresh,
                        canExecuteMethod: function () {
                            return true;
                        }
                    }
                ];

                const filter = $scope.filter = blade.filter || {};
                filter.criteriaChanged = function () {
                    if ($scope.pageSettings.currentPage > 1) {
                        $scope.pageSettings.currentPage = 1;
                    } else {
                        blade.refresh();
                    }
                };

                blade.refresh = function () {
                    blade.isLoading = true;
                    subscriptionsApi.search(angular.extend(filter, {
                        searchPhrase: filter.keyword ? filter.keyword : undefined,
                        sort: uiGridHelper.getSortExpression($scope),
                        skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                        take: $scope.pageSettings.itemsPerPageCount
                    }), function (data) {
                        data.results.forEach((item => {
                            catalogItems.get({ id: item.productId, respGroup: 1 }, (catalogItem) => {
                                item.productName = catalogItem.name
                            });
                        }));
                        blade.isLoading = false;
                        $scope.pageSettings.totalItems = data.totalCount;
                        blade.currentEntities = data.results;
                    });
                }
            }]);
