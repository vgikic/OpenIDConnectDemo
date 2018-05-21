import fetchHelper from "../fetchHelper";
// NOTE: the baseUrl must be an existing ambient variable (usually injected via a <script> tag in _Layout.cshtml)
declare var baseUrl : string;

import { IUserDto } from './UserDto';

export class SampleDataService {
    
    public static Url_WeatherForecasts = () => baseUrl + `api/SampleData/weatherForecasts`;
    
    public static Url_Logout = () => baseUrl + `api/SampleData/logout`;
    
    public static Url_RenewTokens = () => baseUrl + `api/SampleData/renewTokens`;
    
 
    public static async weatherForecasts<T>( showSpinner = true, requestInit?: RequestInit) : Promise<T> {
        return fetchHelper.Get<T>(SampleDataService.Url_WeatherForecasts(), showSpinner, requestInit);
    }
    public static async logout<T>( showSpinner = true, requestInit?: RequestInit) : Promise<T> {
        return fetchHelper.Get<T>(SampleDataService.Url_Logout(), showSpinner, requestInit);
    }
    public static async renewTokens<T>( showSpinner = true, requestInit?: RequestInit) : Promise<T> {
        return fetchHelper.Get<T>(SampleDataService.Url_RenewTokens(), showSpinner, requestInit);
    }     
}

