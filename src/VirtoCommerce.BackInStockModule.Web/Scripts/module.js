// Call this to register your module to main application
var moduleName = 'BackInStockModule';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider',
        function ($stateProvider) {
            $stateProvider
                .state('workspace.BackInStockModuleState', {
                    url: '/BackInStockModule',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        'platformWebApp.bladeNavigationService',
                        function (bladeNavigationService) {
                            var newBlade = {
                                id: 'blade1',
                                controller: 'BackInStockModule.helloWorldController',
                                template: 'Modules/$(VirtoCommerce.BackInStockModule)/Scripts/blades/hello-world.html',
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
                path: 'browse/BackInStockModule',
                icon: 'fa fa-cube',
                title: 'BackInStockModule',
                priority: 100,
                action: function () { $state.go('workspace.BackInStockModuleState'); },
                permission: 'BackInStockModule:access',
            };
            mainMenuService.addMenuItem(menuItem);
        }
    ]);
