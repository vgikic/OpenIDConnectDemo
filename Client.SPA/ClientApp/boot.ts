import './css/site.scss';
import 'bootstrap';
import * as ko from 'knockout';
import { createBrowserHistory } from 'history';
import './webpack-component-loader';
import AppRootComponent from './components/app-root/app-root';
import fetchHelper from './fetchHelper';

var baseUrl = (window as any).baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
var baseApiUrl = (window as any).baseApiUrl = document.getElementsByTagName('baseApiUrl')[0].getAttribute('href');
fetchHelper.API_ACCESS_TOKEN = (window as any).apiAccessToken;
const basename = baseUrl.substring(0, baseUrl.length - 1); // History component needs no trailing slash


// Load and register the <app-root> component
ko.components.register('app-root', AppRootComponent);

// Tell Knockout to start up an instance of your application
ko.applyBindings({ history: createBrowserHistory({ basename }), basename });

// Basic hot reloading support. Automatically reloads and restarts the Knockout app each time
// you modify source files. This will not preserve any application state other than the URL.
if (module.hot) {
    module.hot.accept();
    module.hot.dispose(() => ko.cleanNode(document.body));
}
