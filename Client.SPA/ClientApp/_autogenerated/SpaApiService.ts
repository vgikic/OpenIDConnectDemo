import fetchHelper from "../fetchHelper";
// NOTE: the baseUrl must be an existing ambient variable (usually injected via a <script> tag in _Layout.cshtml)
declare var baseUrl : string;

import { IUserDto } from './UserDto';

export class SpaService {
    
    public static Url_GetClientSpaProtectedApiDataAsync = () => baseUrl + `api/SpaApi/getClientSpaProtectedApiDataAsync`;
    
    public static Url_LogoutAsync = () => baseUrl + `api/SpaApi/logoutAsync`;
    
    public static Url_RenewTokensAsync = () => baseUrl + `api/SpaApi/renewTokensAsync`;
    
 
    public static async getClientSpaProtectedApiDataAsync<T>( showSpinner = true, requestInit?: RequestInit) : Promise<T> {
        return fetchHelper.Get<T>(SpaService.Url_GetClientSpaProtectedApiDataAsync(), showSpinner, requestInit);
    }
    public static async logoutAsync<T>( showSpinner = true, requestInit?: RequestInit) : Promise<T> {
        return fetchHelper.Get<T>(SpaService.Url_LogoutAsync(), showSpinner, requestInit);
    }
    public static async renewTokensAsync<T>( showSpinner = true, requestInit?: RequestInit) : Promise<T> {
        return fetchHelper.Get<T>(SpaService.Url_RenewTokensAsync(), showSpinner, requestInit);
    }     
}

