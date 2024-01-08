namespace ProjectIndustries.Sellify.WebApi.Payments.Data
{
  public enum SkrillStatus
  {
    /// <summary>
    /// Whenever a chargeback is received by Skrill, a ‘-3’ status is posted in the
    /// status_url and an email is sent to the primary email address linked to the
    /// Merchant’s account. Skrill also creates a new debit transaction to debit the
    /// funds from your merchant account.
    /// </summary>
    Chargeback = -3,

    /// <summary>
    /// This status is typically sent when the customer tries to pay via Credit Card or
    /// Direct Debit but our provider declines the transaction. It can also be sent if the
    /// transaction is declined by Skrill’s internal fraud engine for example:
    /// failed_reason_code 54 - Failed due to internal security restrictions.
    /// </summary>
    Failed = -2,

    /// <summary>
    /// Pending transactions can either be cancelled manually by the sender in their
    /// online Skrill Digital Wallet account history or they will auto-cancel after 14 days
    /// if still pending.
    /// </summary>
    Cancelled = -1,

    /// <summary>
    /// Sent when the customers pays via an offline bank transfer option. Such
    /// transactions will auto-process if the bank transfer is received by Skrill.
    /// Note: We strongly recommend that you do not process the order or transaction
    /// in your system upon receipt of this status from Skrill.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Sent when the transaction is processed and the funds have been received
    /// in your Skrill account.
    /// </summary>
    Processed = 2
  }
}