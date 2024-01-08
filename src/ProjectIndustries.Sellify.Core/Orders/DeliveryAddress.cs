namespace ProjectIndustries.Sellify.Core.Orders
{
  public class DeliveryAddress
  {
    public string CountryId { get; private set; } = null!;
    public string AddressLine1 { get; private set; } = null!;
    public string AddressLine2 { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string PostalCode { get; private set; } = null!;
  }
}