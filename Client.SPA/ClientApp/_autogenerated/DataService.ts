import fetchHelper from "../fetchHelper";
// NOTE: the baseUrl must be an existing ambient variable (usually injected via a <script> tag in _Layout.cshtml)
declare var baseApiUrl : string;


export class DataService {
    
    public static Url_GetDataAsync = () => baseApiUrl + `api/Data/getDataAsync`;
    
 
    public static async getDataAsync<T>( showSpinner = true, requestInit?: RequestInit) : Promise<T> {
        return fetchHelper.Get<T>(DataService.Url_GetDataAsync(), showSpinner, requestInit);
    }     
}

