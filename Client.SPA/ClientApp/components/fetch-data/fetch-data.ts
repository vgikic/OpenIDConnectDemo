import * as ko from 'knockout';
import 'isomorphic-fetch';
import { SampleDataService } from '../../_autogenerated/SampleDataService';

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

class FetchDataViewModel {
    public forecasts = ko.observableArray<WeatherForecast>();

    constructor() {
        this.getData();
    }

    private getData = async () => {
        var test = await SampleDataService.weatherForecasts();
    }
    private logout = async () => {
        await SampleDataService.logout();
        window.location.href = "/";
    }
}

export default { viewModel: FetchDataViewModel, template: require('./fetch-data.html') };
