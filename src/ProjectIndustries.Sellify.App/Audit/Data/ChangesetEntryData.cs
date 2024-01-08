using System.Collections.Generic;

namespace ProjectIndustries.Sellify.App.Audit.Data
{
  public class ChangesetEntryPayloadData
  {
    public IDictionary<string, string?> Current { get; set; } = new Dictionary<string, string?>();
    public IDictionary<string, string?>? Previous { get; set; } = new Dictionary<string, string?>();
  }
}