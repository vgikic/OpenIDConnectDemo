import * as ko from 'knockout';
import { Route, Router } from '../../router';
import { vm } from '../../main-vm';

class AppRootViewModel {
    private router: Router;
    private currentRoute: KnockoutObservable<Route>;
    private

    constructor() {
        this.router = vm.router;

        this.currentRoute = this.router.currentRoute;
    }

    private componentName = ko.pureComputed(() => {
        var currentRoute = this.router.currentRoute() || { component: 'home-page' };
        return currentRoute.component;
    });

    // To support hot module replacement, this method unregisters the router and KO components.
    // In production scenarios where hot module replacement is disabled, this would not be invoked.
    private dispose() {
        this.router.dispose();

        // TODO: Need a better API for this
        Object.getOwnPropertyNames((<any>ko).components._allRegisteredComponents).forEach(componentName => {
            ko.components.unregister(componentName);
        });
    }
}

export default { viewModel: AppRootViewModel, template: require('./app-root.html') };
