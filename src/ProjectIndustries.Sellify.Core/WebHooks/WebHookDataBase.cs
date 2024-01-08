using System;

namespace ProjectIndustries.Sellify.Core.WebHooks
{
  public abstract class WebHookDataBase
  {
    public Guid StoreId { get; set; }
    public string Type => WebHookDataInspector.GetType(this);
  }
}