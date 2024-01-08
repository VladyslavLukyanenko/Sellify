//==============================================================================
//
//  This file was auto-generated by a tool using the PayPal REST API schema.
//  More information: https://developer.paypal.com/docs/api/
//
//==============================================================================
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PayPal.Api
{
    /// <summary>
    /// A refund transaction. This is the resource that is returned on GET /refund
    /// <para>
    /// See <a href="https://developer.paypal.com/docs/api/">PayPal Developer documentation</a> for more information.
    /// </para>
    /// </summary>
    public class DetailedRefund : Refund
    {
        /// <summary>
        /// free-form field for the use of clients
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "custom")]
        public string custom { get; set; }

        /// <summary>
        /// invoice number to track this payment
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "invoice_number")]
        public new string invoice_number { get; set; }

        /// <summary>
        /// Amount refunded to payer of the original transaction, in the current Refund call
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "refund_to_payer")]
        public Currency refund_to_payer { get; set; }

        /// <summary>
        /// List of external funding that were refunded by the Refund call. Each external_funding unit should have a unique reference_id
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "refund_to_external_funding")]
        public List<ExternalFunding> refund_to_external_funding { get; set; }

        /// <summary>
        /// Transaction fee refunded to original recipient of payment.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "refund_from_transaction_fee")]
        public Currency refund_from_transaction_fee { get; set; }

        /// <summary>
        /// Amount subtracted from PayPal balance of the original recipient of payment, to make this refund.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "refund_from_received_amount")]
        public Currency refund_from_received_amount { get; set; }

        /// <summary>
        /// Total amount refunded so far from the original purchase. Say, for example, a buyer makes $100 purchase, the buyer was refunded $20 a week ago and is refunded $30 in this transaction. The gross refund amount is $30 (in this transaction). The total refunded amount is $50.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "total_refunded_amount")]
        public Currency total_refunded_amount { get; set; }
    }
}
