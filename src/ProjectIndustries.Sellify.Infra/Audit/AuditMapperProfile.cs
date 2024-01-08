using AutoMapper;
using ProjectIndustries.Sellify.App.Audit.Data;
using ProjectIndustries.Sellify.App.Identity.Model;
using ProjectIndustries.Sellify.Core.Audit;

namespace ProjectIndustries.Sellify.Infra.Audit
{
  public class AuditMapperProfile : Profile
  {
    public AuditMapperProfile()
    {
      CreateMap<ChangeSet, ChangeSetData>()
        .ForMember(_ => _.UpdatedBy, _ => _.MapFrom(o => new UserRef {Id = o.UpdatedBy}))
        //.ForMember(_ => _.Facility, _ => _.MapFrom(o => new FacilityRef {Id = o.FacilityId.GetValueOrDefault()}))
        ;

      CreateMap<ChangeSetEntry, ChangeSetEntryRefData>();
    }
  }
}