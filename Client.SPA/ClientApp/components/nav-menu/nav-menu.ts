import * as ko from 'knockout';
import { Route, Router } from '../../router';
import { vm } from '../../main-vm';

class NavMenuViewModel {
    public currentRoute: KnockoutObservable<Route>;
    private routes: Array<Route>;

    constructor() {
        this.currentRoute = vm.router.currentRoute;
        this.routes = vm.routes.filter(r => r.visible);
    }
}

export default { viewModel: NavMenuViewModel, template: require('./nav-menu.html') };
