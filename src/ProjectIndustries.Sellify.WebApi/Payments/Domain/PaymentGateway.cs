using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.WebApi.Payments.Domain
{
  public class PaymentGateway : Enumeration
  {
    public static readonly PaymentGateway Stripe = new(1, nameof(Stripe));
    public static readonly PaymentGateway Skrill = new(2, nameof(Skrill));
    public static readonly PaymentGateway PayPal = new(3, nameof(PayPal));

    public PaymentGateway(int id, string name)
      : base(id, name)
    {
      
    }
  }
}