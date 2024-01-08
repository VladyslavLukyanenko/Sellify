using System;
using System.Collections.Generic;
using NodaTime;
using ProjectIndustries.Sellify.Core.WebHooks;

namespace ProjectIndustries.Sellify.App.WebHooks.Data
{
  [WebHookType("fakewebhook")]
  public class FakeWebhookData : WebHookDataBase
  {
    public string Name { get; set; } = null!;
    public int Value { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public Instant Timestamp { get; set; }
    public decimal Money { get; set; }
    public double? RandomNum { get; set; }
    public List<int> Primitives { get; set; } = new();

    public List<ComplexValue> ComplexValues { get; set; } = new();

    public Dictionary<string, string> Dict { get; set; } = new();

    public FakeInnerData InnerData { get; set; } = new();
  }

  public class ComplexValue
  {
    public Guid Id { get; set; }
    public double RandomValue { get; set; }
    public DateTimeOffset Time { get; set; }
  }

  public class FakeInnerData
  {
    public Guid Id { get; set; }
    public Guid? NotEmptyId { get; set; }
    public Guid? EmptyId { get; set; }
    public string BirthDay { get; set; } = null!;
    public string Gender { get; set; } = null!;
  }

}