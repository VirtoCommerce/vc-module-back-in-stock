angular.module('VirtoCommerce.BackInStock')
    .controller('VirtoCommerce.BackInStock.subscriptionWidgetController', [
        '$scope', 'platformWebApp.bladeNavigationService', 'VirtoCommerce.BackInStock.subscriptionApi',
        function ($scope, bladeNavigationService, subscriptionApi) {
            const blade = $scope.blade;
            const memberId = blade.currentEntityId;
            $scope.subscriptionsCount = 0;

            function refresh() {
                $scope.loading = true;

                const criteria = {
                    memberId: memberId,
                    take: 0,
                };

                subscriptionApi.search(criteria, function (data) {
                    $scope.subscriptionsCount = data.totalCount;
                    $scope.loading = false;
                });
            }

            $scope.openBlade = function () {
                if ($scope.loading) {
                    return;
                }

                const newBlade = {
                    id: 'backInStockSubscriptions',
                    controller: 'VirtoCommerce.BackInStock.subscriptionListController',
                    template: 'Modules/$(VirtoCommerce.BackInStock)/Scripts/blades/back-in-stock-subscription-list.html',
                    memberId: memberId
                };

                bladeNavigationService.showBlade(newBlade, blade);
            };

            $scope.$watch('blade.currentEntityId', function (id) {
                if (id) {
                    refresh();
                }
            });
        }
    ]);
