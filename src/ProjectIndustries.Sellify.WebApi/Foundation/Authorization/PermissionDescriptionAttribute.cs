using System;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Authorization
{
  [AttributeUsage(AttributeTargets.Field)]
  public class PermissionDescriptionAttribute : Attribute
  {
    public PermissionDescriptionAttribute(string description)
    {
      Description = description;
    }

    public string Description { get; }
  }
}