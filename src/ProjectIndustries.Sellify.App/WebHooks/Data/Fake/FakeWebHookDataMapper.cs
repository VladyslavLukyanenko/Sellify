using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NodaTime;
using ProjectIndustries.Sellify.Core;
using ProjectIndustries.Sellify.Core.Events;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Core.WebHooks.Services;

namespace ProjectIndustries.Sellify.App.WebHooks.Data.Fake
{
  public class FakeWebHookTriggerEvent : DomainEvent, IStoreBoundEntity
  {
    public Guid StoreId { get; } = Guid.Parse("b49930dc-9ce4-41f5-a09e-16a3dce9a61b");
  }

  public class FakeWebHookDataMapper : IWebHookPayloadMapper
  {
    public bool CanMap(object @event) => @event is FakeWebHookTriggerEvent;

    public ValueTask<WebHookDataBase?> MapAsync(object @event, CancellationToken ct = default)
    {
      var data = new FakeWebhookData
      {
        Name = "Andrii",
        Value = 30,
        Money = 33M,
        Timestamp = SystemClock.Instance.GetCurrentInstant(),
        RandomNum = new Random().NextDouble(),
        DayOfWeek = DateTime.Now.DayOfWeek,
        Primitives = new List<int> {1, 2, 3, 4, 5},
        Dict = new Dictionary<string, string>
        {
          ["first"] = "1",
          ["second"] = "2"
        },
        ComplexValues = new List<ComplexValue>
        {
          new()
          {
            Id = Guid.NewGuid(),
            Time = DateTimeOffset.Now,
            RandomValue = new Random().NextDouble()
          },
          new()
          {
            Id = Guid.NewGuid(),
            Time = DateTimeOffset.Now,
            RandomValue = new Random().NextDouble()
          },
          new()
          {
            Id = Guid.NewGuid(),
            Time = DateTimeOffset.Now,
            RandomValue = new Random().NextDouble()
          }
        },
        InnerData = new FakeInnerData
        {
          Id = Guid.NewGuid(),
          NotEmptyId = Guid.NewGuid(),
          Gender = "M",
          BirthDay = "1990-09-22"
        }
      };

      return ValueTask.FromResult<WebHookDataBase?>(data);
    }
  }
}