// using System.Threading;
// using System.Threading.Tasks;
// using AutoMapper;
// using ProjectIndustries.Sellify.App.Identity.Services;
// using ProjectIndustries.Sellify.App.WebHooks;
// using ProjectIndustries.Sellify.Core.WebHooks.Services;
//
// namespace ProjectIndustries.Dashboards.App.WebHooks.LicenseKeys
// {
//   public class LicenseKeyAssociationChangeWebHookMapper : WebHookMapperBase<LicenseKeyAssociationChangeWebHookDataBase>
//   {
//     private readonly IUserRefProvider _userRefProvider;
//
//     public LicenseKeyAssociationChangeWebHookMapper(IMapper mapper, IWebHookPayloadFactory webHookPayloadFactory,
//       IWebHookBindingRepository webHookBindingRepository, IUserRefProvider userRefProvider)
//       : base(mapper, webHookPayloadFactory, webHookBindingRepository)
//     {
//       _userRefProvider = userRefProvider;
//     }
//
//     public override bool CanMap(object @event) => @event is LicenseKeyAssociationChange;
//
//     protected override async ValueTask InitializeAsync(LicenseKeyAssociationChangeWebHookDataBase data,
//       CancellationToken ct)
//     {
//       await _userRefProvider.InitializeAsync(data.User, ct);
//     }
//   }
// }