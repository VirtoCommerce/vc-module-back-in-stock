angular.module('VirtoCommerce.BackInStock')
    .controller('VirtoCommerce.BackInStock.subscriptionWidgetController', [
        '$scope', 'platformWebApp.bladeNavigationService', 'VirtoCommerce.BackInStock.subscriptionApi',
        function ($scope, bladeNavigationService, subscriptionApi) {
            let blade = $scope.blade;
            let filter = $scope.filter = blade.filter || {};
            let customerId = $scope.blade.currentEntityId;

            function init() {
                $scope.subscriptions = null;
                $scope.subscriptionsCount = 0;

                refresh();
            }

            function refresh() {
                $scope.loading = true;

                const criteria = angular.extend(filter, {
                    memberId: customerId,
                    skip: 0,
                    take: 100,
                });

                subscriptionApi.search(criteria, function (data) {
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
                    controller: 'VirtoCommerce.BackInStock.subscriptionListController',
                    template: 'Modules/$(VirtoCommerce.BackInStock)/Scripts/blades/back-in-stock-subscription-list.html',
                    filter: { memberId: customerId }
                };

                bladeNavigationService.showBlade(newBlade, blade);
            };

            $scope.$watch('blade.currentEntityId', function (id) {
                if (id) {
                    refresh();
                }
            });

            init();
        }
    ]);
