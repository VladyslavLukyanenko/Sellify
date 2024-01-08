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
import { OrderStatus } from './order-status.model';
import { PurchasedProductData } from './purchased-product-data.model';


export interface OrderRowData { 
    id?: string;
    createdAt?: string;
    customerEmail?: string | null;
    invoiceEmail?: string | null;
    customerName?: string | null;
    status?: OrderStatus;
    totalPrice?: number;
    purchasedProduct?: PurchasedProductData;
}
