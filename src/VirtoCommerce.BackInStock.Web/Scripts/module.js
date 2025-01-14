// Call this to register your module to main application
var moduleName = 'BackInStock';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider',
        function ($stateProvider) {
            $stateProvider
                .state('workspace.BackInStockState', {
                    url: '/BackInStock',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        'platformWebApp.bladeNavigationService',
                        function (bladeNavigationService) {
                            var newBlade = {
                                id: 'blade1',
                                controller: 'BackInStock.helloWorldController',
                                template: 'Modules/$(VirtoCommerce.BackInStock)/Scripts/blades/hello-world.html',
                                isClosingDisabled: true,
                            };
                            bladeNavigationService.showBlade(newBlade);
                        }
                    ]
                });
        }
    ])
    .run(['platformWebApp.mainMenuService', '$state',
        function (mainMenuService, $state) {
            //Register module in main menu
            var menuItem = {
                path: 'browse/BackInStock',
                icon: 'fa fa-cube',
                title: 'BackInStock',
                priority: 100,
                action: function () { $state.go('workspace.BackInStockState'); },
                permission: 'BackInStock:access',
            };
            mainMenuService.addMenuItem(menuItem);
        }
    ]);
