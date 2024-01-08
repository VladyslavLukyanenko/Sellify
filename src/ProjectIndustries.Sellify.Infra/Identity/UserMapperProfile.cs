using AutoMapper;
using ProjectIndustries.Sellify.App.Identity.Domain;
using ProjectIndustries.Sellify.App.Identity.Model;

namespace ProjectIndustries.Sellify.Infra.Identity
{
  public class UserMapperProfile : Profile
  {
    public UserMapperProfile()
    {
      CreateMap<ApplicationUser, UserData>()
        .ForMember(_ => _.IsEmailConfirmed, _ => _.MapFrom(q => q.EmailConfirmed))
        .ForMember(_ => _.Name, _ => _.MapFrom(q => q.FirstName + " " + q.LastName))
        .ForMember(_ => _.IsLockedOut, _ => _.MapFrom(o => o.LockoutEnd.HasValue))
        .ForMember(_ => _.Roles, _ => _.Ignore());
    }
  }
}