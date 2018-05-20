import fetchHelper from "../fetchHelper";
// NOTE: the baseUrl must be an existing ambient variable (usually injected via a <script> tag in _Layout.cshtml)
declare var baseApiUrl : string;


export class DataService {
    
    public static Url_GetDataAsync = () => baseApiUrl + `api/Data/getDataAsync`;
    
    public static Url_GetPolicyProtectedDataAsync = () => baseApiUrl + `api/Data/getPolicyProtectedDataAsync`;
    
    public static Url_GetCustomRequirementProtectedDataAsync = (id: number) => baseApiUrl + `api/Data/getCustomRequirementProtectedDataAsync/${id}`;
    
 
    public static async getDataAsync<T>( showSpinner = true, requestInit?: RequestInit) : Promise<T> {
        return fetchHelper.Get<T>(DataService.Url_GetDataAsync(), showSpinner, requestInit);
    }
    public static async getPolicyProtectedDataAsync<T>( showSpinner = true, requestInit?: RequestInit) : Promise<T> {
        return fetchHelper.Get<T>(DataService.Url_GetPolicyProtectedDataAsync(), showSpinner, requestInit);
    }
    public static async getCustomRequirementProtectedDataAsync<T>(id : number, showSpinner = true, requestInit?: RequestInit) : Promise<T> {
        return fetchHelper.Get<T>(DataService.Url_GetCustomRequirementProtectedDataAsync(id), showSpinner, requestInit);
    }     
}

