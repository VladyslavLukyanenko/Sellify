namespace ProjectIndustries.Sellify.Core.Orders.Services
{
  public class UpdateOrCreateCustomerCommand
  {
    public UpdateOrCreateCustomerCommand()
    {
    }

    public UpdateOrCreateCustomerCommand(string customerEmail, string? firstName, string? lastName)
    {
      CustomerEmail = customerEmail;
      FirstName = firstName;
      LastName = lastName;
    }
    
    public string CustomerEmail { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
  }
}