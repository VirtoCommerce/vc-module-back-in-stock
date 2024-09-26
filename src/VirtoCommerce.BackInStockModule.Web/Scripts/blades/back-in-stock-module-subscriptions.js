angular.module('VirtoCommerce.BackInStockModule')
    .controller('VirtoCommerce.BackInStockModule.backInStockSubscriptionsListController',
        ['$scope', 'VirtoCommerce.BackInStockModule.subscriptions.webApi', 'platformWebApp.bladeUtils', 'uiGridConstants',
            'platformWebApp.uiGridHelper', 'platformWebApp.authService', 'VirtoCommerce.BackInStockModule.entityTypesResolverService',
            function ($scope, subscriptionsApi, bladeUtils, uiGridConstants, uiGridHelper, authService, entityTypesResolverService) {
                $scope.gridOptions = {};
                $scope.uiGridConstants = uiGridConstants;
                $scope.setGridOptions = function (gridOptions) {
                    uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                        uiGridHelper.bindRefreshOnSortChanged($scope);
                    });
                    bladeUtils.initializePagination($scope);
                };
                $scope.entityTypesList = entityTypesResolverService.objects.map(x => x.entityType);


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
                const bladeNavigationService = bladeUtils.bladeNavigationService;
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
                        blade.isLoading = false;
                        $scope.pageSettings.totalItems = data.totalCount;
                        blade.currentEntities = data.results;
                    });
                }

                blade.selectNode = function (data) {
                    $scope.selectedNodeId = data.id;

                    /*if (!authService.checkPermission('customerReviews:update')) {
                        return;
                    }*/

                    const newBlade = {
                        id: 'backInStockModuleSubscriptions',
                        currentEntityId: data.id,
                        currentEntity: angular.copy(data),
                        title: 'Back in stock subscriptions',
                        controller: 'VirtoCommerce.BackInStockModule.backInStockSubscriptionsController',
                        template: 'Modules/$(VirtoCommerce.BackInStockModule)/Scripts/blades/back-in-stock-module-subscriptions.tpl.html',
                        filter: {entityIds: [customerId], entityType: 'Customer'}
                    };

                    bladeNavigationService.showBlade(newBlade, blade);
                }
            }]);
