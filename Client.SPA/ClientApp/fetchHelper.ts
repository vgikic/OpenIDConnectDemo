﻿import * as $ from "jquery";

export default class fetchHelper {

    public static EVENT_TARGET_ID = "fetch-error-event-target"; // Id of HTML element on which event listener is attached
    public static ERROR_EVENT_NAME = "fetch-error-event";


    /**
    * Loader DOM element, depends on html having an element with class _spinnerElementClassName
    */

    private static dlg: JQuery;
    private static overlay: JQuery;
    private static spinnerCount = 0;
    private static lastTimeoutHandle;
    private static _spinnerElementClassName = ".main-loader";
    private static _overlayElementClassName = ".overlay";

    private static createHeaders = (): Headers => new Headers({
        'Content-Type': "application/json"
    });

    private static createRequestOptions = (method: string): RequestInit => {
        return {
            method: method,
            credentials: 'include',
            headers: fetchHelper.createHeaders(),
            cache: 'default',
            
        }
    }

    public static async Get<T>(url: string, showSpinner = true, requestInit?: RequestInit): Promise<T> {
        try {
            const options = fetchHelper.getOptions('GET', requestInit);
            return await fetchHelper.getResponseAsync(options, url, showSpinner);
        }
        catch (error) {
            return fetchHelper.onError(error, showSpinner);
        }
    }

    public static async Post<T>(url: string, data: Object, showSpinner = true, requestInit?: RequestInit): Promise<T> {
        try {
            const options = fetchHelper.getOptions('POST', requestInit);
            options.body = JSON.stringify(data);
            const result = await fetchHelper.getResponseAsync(options, url, showSpinner);
            return result;
        }
        catch (error) {
            return fetchHelper.onError(error, showSpinner);
        }
    }

    public static async Put<T>(url: string, data: Object, showSpinner = true, requestInit?: RequestInit): Promise<T> {
        try {
            const options = fetchHelper.getOptions('PUT', requestInit);
            options.body = JSON.stringify(data);
            return await fetchHelper.getResponseAsync(options, url, showSpinner);
        }
        catch (error) {
            return fetchHelper.onError(error, showSpinner);
        }
    }

    public static async Delete<T>(url: string, showSpinner = true, requestInit?: RequestInit): Promise<T> {
        try {
            const options = fetchHelper.getOptions('DELETE', requestInit);
            return await fetchHelper.getResponseAsync(options, url, showSpinner);
        }
        catch (error) {
            return fetchHelper.onError(error, showSpinner);
        }
    }

    private static getOptions = (method: string, options?: RequestInit): RequestInit => options || fetchHelper.createRequestOptions(method);

    private static responseHandlerAsync = async (response: Response): Promise<any> => {

        if (response.ok) {
            return await response.json().catch(() => Promise.resolve({ error: false })) //parsing of response body as json failed, server might have returned null 
        }
        else {
            const statusResult: IResponseStatus = { status: response.status, text: response.statusText };
            const errorResponse: IErrorResponse = {
                status: statusResult,
                modelState: [],
                error: true
            };
            const modelState = await response.json()
                .catch((e) => // Internal Server Error (500), there is no modelstate, (note: .NET CORE returns HTML as response on Internal Server Error so parsing it as JSON throws an error)
                    Promise.reject(errorResponse)
                );

            errorResponse.modelState = modelState;
            return Promise.reject(errorResponse);
        }
    }

    private static getResponseAsync = async (options: RequestInit, url: string, showSpinner: boolean) => {

        fetchHelper.showLoadingSpinner(showSpinner);

        const response = await fetch(url, options);

        const result =
            await fetchHelper
                .responseHandlerAsync(response)
                .catch((er: IErrorResponse) => Promise.reject(er));

        fetchHelper.hideLoadingSpinner(showSpinner);

        return result;
    }


    public static showLoadingSpinner = (showSpinner: boolean) => {

        if (showSpinner) {

            if (fetchHelper.dlg === undefined) {
                fetchHelper.dlg = $(fetchHelper._spinnerElementClassName);
                fetchHelper.overlay = $(fetchHelper._overlayElementClassName)
            }

            fetchHelper.spinnerCount++;

            if (fetchHelper.lastTimeoutHandle === undefined) {       //  important: set handle only if undefined
                fetchHelper.lastTimeoutHandle = setTimeout(() => {
                    fetchHelper.dlg.addClass("visible");
                    fetchHelper.overlay.addClass('visible');
                }, 100);
            }
        }
    }

    public static hideLoadingSpinner = (showSpinner: boolean) => {

        if (showSpinner) {
            if (fetchHelper.dlg === undefined) {
                fetchHelper.dlg = $(fetchHelper._spinnerElementClassName);
            }

            if (--fetchHelper.spinnerCount <= 0) {
                if (fetchHelper.lastTimeoutHandle) {
                    clearTimeout(fetchHelper.lastTimeoutHandle);
                }
                fetchHelper.lastTimeoutHandle = undefined;
                fetchHelper.dlg.removeClass("visible");
                fetchHelper.overlay.removeClass('visible');
            }
        }
    }

    private static dispatchErrorEvent = (errorResponse: IErrorResponse) => {

        const event = document.createEvent('Event') as IErrorEvent;
        event.errorResponse = errorResponse;
        event.initEvent(fetchHelper.ERROR_EVENT_NAME, true, true);

        const target = document.getElementById(fetchHelper.EVENT_TARGET_ID);
        if (target)
            target.dispatchEvent(event);
    }

    private static onError = (error, showSpinner: boolean) => {
        fetchHelper.dispatchErrorEvent(error);
        fetchHelper.hideLoadingSpinner(showSpinner);
        return Promise.resolve(error);
    }

}
interface IResponseStatus {
    status: number,
    text: string
}

export interface IErrorResponse {
    status: IResponseStatus,
    modelState: {},
    error: boolean
}

export interface IErrorEvent extends Event {
    errorResponse: IErrorResponse
}