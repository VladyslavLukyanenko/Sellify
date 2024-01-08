// using NodaTime;
// using ProjectIndustries.Dashboards.App.Identity.Model;
// using ProjectIndustries.Dashboards.App.Products.Model;
//
// namespace ProjectIndustries.Dashboards.App.WebHooks.LicenseKeys
// {
//   public abstract class LicenseKeyAssociationChangeWebHookDataBase : WebHookDataBase
//   {
//     public long Id { get; set; }
//     public PlanRef Plan { get; set; } = null!;
//     public ProductRef Product { get; set; } = null!;
//     public UserRef? User { get; set; }
//     public Instant? Expiry { get; set; }
//     public string Value { get; set; } = null!;
//   }
// }