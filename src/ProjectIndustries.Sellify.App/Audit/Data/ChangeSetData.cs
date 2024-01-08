using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using ProjectIndustries.Sellify.App.Identity.Model;

namespace ProjectIndustries.Sellify.App.Audit.Data
{
  public class ChangeSetData
  {
    private IList<ChangeSetEntryRefData> _entries = new List<ChangeSetEntryRefData>();
    public Guid Id { get; set; }
    public string Label { get; set; } = null!;
    public Instant Timestamp { get; set; }

    public UserRef UpdatedBy { get; set; } = null!;

    public IList<ChangeSetEntryRefData> Entries
    {
      get => _entries;
      // ReSharper disable once ConstantConditionalAccessQualifier
      set => _entries = value?.OrderByDescending(_ => _.CreatedAt).ToList() ?? new List<ChangeSetEntryRefData>();
    }
  }
}