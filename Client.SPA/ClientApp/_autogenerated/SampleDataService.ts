import fetchHelper from "../fetchHelper";
// NOTE: the baseUrl must be an existing ambient variable (usually injected via a <script> tag in _Layout.cshtml)
declare var baseUrl : string;


export class SampleDataService {
    
    public static Url_WeatherForecasts = () => baseUrl + `api/SampleData/weatherForecasts`;
    
 
    public static async weatherForecasts<T>( showSpinner = true, requestInit?: RequestInit) : Promise<T> {
        return fetchHelper.Get<T>(SampleDataService.Url_WeatherForecasts(), showSpinner, requestInit);
    }     
}

