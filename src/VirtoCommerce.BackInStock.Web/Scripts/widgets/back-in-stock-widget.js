angular.module('VirtoCommerce.BackInStock')
    .controller('VirtoCommerce.BackInStock.backInStockSubscriptionsWidgetController',
        ['$scope', 'VirtoCommerce.BackInStock.subscriptions.webApi', 'platformWebApp.bladeNavigationService',
            function ($scope, subscriptionsApi, bladeNavigationService) {
                let blade = $scope.blade;
                let filter = $scope.filter = blade.filter || {};
                let customerId = $scope.blade.currentEntityId;

                function init() {
                    $scope.loading = false;
                    $scope.subscriptions = null;
                    $scope.subscriptionsCount = 0;

                    subscriptionsApi.search(angular.extend(filter, {
                        memberId: customerId,
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
                        memberId: customerId,
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
                        id: 'backInStockSubscriptions',
                        title: 'BackInStock.blades.subscriptions.title',
                        controller: 'VirtoCommerce.BackInStock.backInStockSubscriptionsListController',
                        template: 'Modules/$(VirtoCommerce.BackInStock)/Scripts/blades/back-in-stock-subscriptions.html',
                        filter: { memberId: customerId }
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
