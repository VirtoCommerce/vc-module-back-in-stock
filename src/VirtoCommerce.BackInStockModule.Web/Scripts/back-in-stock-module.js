// Call this to register your module to main application
const moduleName = 'VirtoCommerce.BackInStockModule';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['$state',
        'platformWebApp.widgetService',
        'platformWebApp.authService',
        'VirtoCommerce.BackInStockModule.entityTypesResolverService',
        'virtoCommerce.catalogModule.items',
        function ($state, widgetService, authService, entityTypesResolverService, catalogItems) {
            let customerBackInStockSubscriptionsWidget = {
                controller: 'VirtoCommerce.BackInStockModule.backInStockSubscriptionsController',
                template: 'Modules/$(VirtoCommerce.BackInStockModule)/Scripts/widgets/back-in-stock-widget.tpl.html',
                isVisible: true,
            };

            widgetService.registerWidget(customerBackInStockSubscriptionsWidget, 'customerDetail1');

            function checkPermissionToReadPrices() {
                return authService.checkPermission('BackInStockModule:access');
            }

            //Product entityType resolver
            entityTypesResolverService.registerType({
                entityType: 'Product',
                description: 'VirtoCommerce.BackInStockModule.blades.product-detail.description',
                fullTypeName: 'VirtoCommerce.CatalogModule.Core.Model.CatalogProduct',
                icon: 'fas fa-shopping-bag',
                entityIdFieldName: 'itemId',
                detailBlade: {
                    controller: 'virtoCommerce.catalogModule.itemDetailController',
                    template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
                },
                getEntity: function (entityId, setEntityCallback) {
                    catalogItems.get({id: entityId, respGroup: 1}, (data) => {
                        setEntityCallback(data.name, data.imgSrc);
                    });
                },
                knownChildrenTypes: []
            });
        }
    ]);
