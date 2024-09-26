angular.module('VirtoCommerce.BackInStockModule')
    .controller('VirtoCommerce.BackInStockModule.backInStockSubscriptionsWidgetController',
        ['$scope', 'VirtoCommerce.BackInStockModule.subscriptions.webApi', 'platformWebApp.bladeNavigationService', 'uiGridConstants',
            'platformWebApp.uiGridHelper', 'platformWebApp.bladeUtils',
            function ($scope, subscriptionsApi, bladeNavigationService, uiGridConstants, uiGridHelper, bladeUtils) {
                let blade = $scope.blade;
                let filter = $scope.filter = blade.filter || {};
                let customerId = "1eb2fa8ac6574541afdb525833dadb46";//$scope.blade.currentEntityId;

                function init() {
                    $scope.gridOptions = {};
                    $scope.uiGridConstants = uiGridConstants;
                    $scope.setGridOptions = function (gridOptions) {
                        uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                            uiGridHelper.bindRefreshOnSortChanged($scope);
                        });
                        bladeUtils.initializePagination($scope);
                    };
                    $scope.loading = false;
                    $scope.subscriptions = null;
                    $scope.subscriptionsCount = 0;

                    subscriptionsApi.search(angular.extend(filter, {
                        userId: customerId,
                        skip: 0,
                        take: 100,
                    }), function (data) {
                        $scope.subscriptions = data?.results ?? null;
                        $scope.subscriptionsCount = $scope.subscriptions == null ? 0 : data.totalCount;
                        $scope.loading = false;
                    });
                }

                function refresh() {
                    $scope.loading = true;
                    subscriptionsApi.search(angular.extend(filter, {
                        userId: customerId,
                        skip: 0,
                        take: 100,
                    }), function (data) {
                        $scope.subscriptions = data?.results ?? null;
                        $scope.subscriptionsCount = $scope.subscriptions == null ? 0 : data.totalCount;
                        $scope.loading = false;
                    });
                }

                $scope.openBlade = function () {
                    if ($scope.loading) {
                        return;
                    }

                    let newBlade = {
                        id: 'backInStockModuleSubscriptions',
                        title: 'VirtoCommerce.BackInStockModule.blades.subscriptions.title',
                        controller: 'VirtoCommerce.BackInStockModule.backInStockSubscriptionsListController',
                        template: 'Modules/$(VirtoCommerce.BackInStockModule)/Scripts/blades/back-in-stock-module-subscriptions.tpl.html',
                        filter: {entityIds: [customerId], entityType: 'Customer'}
                    };

                    bladeNavigationService.showBlade(newBlade, blade);

                };

                $scope.$watch("blade.currentEntityId", function (id) {
                    if (id) {
                        refresh();
                    }
                });

                init();

            }
        ]);
