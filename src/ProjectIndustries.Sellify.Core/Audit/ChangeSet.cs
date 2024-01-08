using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Core.Audit
{
  public class ChangeSet : Entity<Guid>, IAggregateRoot
  {
    private readonly List<ChangeSetEntry> _entries = new List<ChangeSetEntry>();

    private ChangeSet()
    {
    }

    public ChangeSet(string label, string updatedBy)
    {
      Timestamp = SystemClock.Instance.GetCurrentInstant();
      Label = label;
      UpdatedBy = updatedBy;
    }

    public Instant Timestamp { get; private set; }
    public string Label { get; private set; } = null!;
    public string UpdatedBy { get; private set; } = null!;


    public IReadOnlyList<ChangeSetEntry> Entries => _entries.AsReadOnly();

    public void AddEntry(ChangeSetEntry changeSetEntry)
    {
      var prev = _entries.FirstOrDefault(_ => _.SameChangesAs(changeSetEntry));
      if (prev != null)
      {
        _entries.Remove(prev);
      }

      _entries.Add(changeSetEntry);
    }

    public bool IsEmpty()
    {
      return !_entries.Any();
    }
  }
}