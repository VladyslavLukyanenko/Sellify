using System;

namespace ProjectIndustries.Sellify.Core.WebHooks
{
  [AttributeUsage(AttributeTargets.Class)]
  public class WebHookTypeAttribute : Attribute
  {
    public WebHookTypeAttribute(string name)
    {
      Name = name;
    }
    public string Name { get; }
  }
}