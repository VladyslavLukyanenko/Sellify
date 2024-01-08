﻿// using AutoMapper;
// using ProjectIndustries.Dashboards.App.Identity.Model;
// using ProjectIndustries.Dashboards.App.Products.Model;
// using ProjectIndustries.Dashboards.Core.Products.Events.LicenseKeys;
//
// namespace ProjectIndustries.Dashboards.App.WebHooks.LicenseKeys
// {
//   public class LicenseKeyAssociationChangeMappingProfile : Profile
//   {
//     public LicenseKeyAssociationChangeMappingProfile()
//     {
//       CreateMap<LicenseKeyAssociationChange, LicenseKeyAssociationChangeWebHookDataBase>()
//         .ForMember(_ => _.Plan, _ => _.MapFrom(o => new PlanRef {Id = o.PlanId}))
//         .ForMember(_ => _.Product, _ => _.MapFrom(o => new ProductRef {Id = o.ProductId}))
//         .ForMember(_ => _.User, _ => _.MapFrom(o => o.UserId.HasValue ? new UserRef {Id = o.UserId.Value} : null))
//         .ForMember(_ => _.Dashboard, _ => _.MapFrom(o => new DashboardRef {Id = o.DashboardId}))
//         .ForMember(_ => _.Type, _ => _.Ignore());
//
//       CreateMap<LicenseKeyPurchased, LicenseKeyPurchasedWebHookData>()
//         .IncludeBase<LicenseKeyAssociationChange, LicenseKeyAssociationChangeWebHookDataBase>()
//         .ForMember(_ => _.IsTrial, _ => _.MapFrom(o => o.TrialEndsAt.HasValue));
//     }
//   }
// }