/**
 * Sellify API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: v1
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { WebhookRowData } from './webhook-row-data.model';


export interface WebhookRowDataIPagedList { 
    readonly isFirst?: boolean;
    readonly isLast?: boolean;
    readonly pageIndex?: number;
    readonly limit?: number;
    readonly count?: number;
    readonly totalElements?: number;
    readonly totalPages?: number;
    readonly isEmpty?: boolean;
    readonly content?: Array<WebhookRowData> | null;
}

