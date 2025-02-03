angular.module('VirtoCommerce.BackInStock')
    .controller('VirtoCommerce.BackInStock.subscriptionListController', [
        '$scope', 'uiGridConstants', 'platformWebApp.uiGridHelper', 'platformWebApp.bladeUtils', 'platformWebApp.bladeNavigationService',
        'VirtoCommerce.BackInStock.subscriptionApi',
        function ($scope, uiGridConstants, uiGridHelper, bladeUtils, bladeNavigationService, subscriptionApi) {
            const blade = $scope.blade;
            blade.title = 'BackInStock.blades.subscriptions.title';
            blade.headIcon = 'fa fa-comments';

            blade.refresh = function () {
                blade.isLoading = true;

                const criteria = angular.extend(filter, {
                    memberId: blade.memberId,
                    sort: uiGridHelper.getSortExpression($scope),
                    skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                    take: $scope.pageSettings.itemsPerPageCount
                });

                subscriptionApi.search(criteria, async function (data) {
                    $scope.pageSettings.totalItems = data.totalCount;
                    blade.currentEntities = data.results;
                    blade.isLoading = false;
                });
            };

            blade.selectNode = function (node) {
                var newBlade = {
                    id: "backInStockProductDetails",
                    controller: 'virtoCommerce.catalogModule.itemDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html',
                    itemId: node.productId
                };
                bladeNavigationService.showBlade(newBlade, blade);
            };

            blade.toolbarCommands = [
                {
                    name: 'platform.commands.refresh',
                    icon: 'fa fa-refresh',
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
                }
                else {
                    blade.refresh();
                }
            };

            $scope.gridOptions = {};
            $scope.uiGridConstants = uiGridConstants;

            $scope.setGridOptions = function (gridOptions) {
                uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                    uiGridHelper.bindRefreshOnSortChanged($scope);
                });
                bladeUtils.initializePagination($scope);
            };
        }]);
