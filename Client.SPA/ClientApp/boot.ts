import 'bootstrap';
import * as ko from 'knockout';
import './webpack-component-loader';

import * as bootstrap from "bootstrap";
import './css/site.scss';

import { createBrowserHistory } from 'history';
import AppRootComponent from './components/app-root/app-root';
import { vm } from "./main-vm";

//import fetchHelper from './fetchHelper';

//var baseUrl = (window as any).baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
//var baseApiUrl = (window as any).baseApiUrl = document.getElementsByTagName('baseApiUrl')[0].getAttribute('href');
//fetchHelper.API_ACCESS_TOKEN = (window as any).apiAccessToken;
//const basename = baseUrl.substring(0, baseUrl.length - 1); // History component needs no trailing slash


ko.components.register('app-root', AppRootComponent);
ko.applyBindings(vm);


// Basic hot reloading support. Automatically reloads and restarts the Knockout app each time
// you modify source files. This will not preserve any application state other than the URL.
if (module.hot) {
    module.hot.accept();
    module.hot.dispose(() => ko.cleanNode(document.body));
}
