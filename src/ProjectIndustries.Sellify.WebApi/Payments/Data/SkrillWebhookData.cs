using System;
using Microsoft.AspNetCore.Mvc;

namespace ProjectIndustries.Sellify.WebApi.Payments.Data
{
  public class SkrillWebhookData
  {
    [FromForm(Name = "pay_to_email")] public string PayToEmail { get; set; } = null!;
    [FromForm(Name = "mb_transaction_id")] public string TransactionId { get; set; } = null!;
    [FromForm(Name = "md5sig")] public string Md5Signature { get; set; } = null!;
    [FromForm(Name = "merchant_id")] public string MerchantID { get; set; } = null!;
    [FromForm(Name = "mb_currency")] public string Currency { get; set; } = null!;
    [FromForm(Name = "mb_amount")] public string Amount { get; set; } = null!;
    [FromForm(Name = "firstname")] public string FirstName { get; set; } = null!;
    [FromForm(Name = "lastname")] public string LastName { get; set; } = null!;
    [FromForm(Name = "pay_from_email")] public string Email { get; set; } = null!;
    [FromForm(Name = "status")] public SkrillStatus Status { get; set; }
    [FromForm(Name = "orderId")] public Guid OrderId { get; set; }
  }
}