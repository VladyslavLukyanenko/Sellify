using System;
using System.Linq;
using Newtonsoft.Json;

namespace ProjectIndustries.Sellify.Core.Collections
{
  public class PageRequest
  {
    public const int MaxLimit = 100;
    public const int MinLimit = 5;

    private int? _limit;

    public PageRequest(PageRequest request)
      : this(request.PageIndex, request.Limit)
    {
      // ReSharper disable once VirtualMemberCallInConstructor
      OrderBy = request.OrderBy;
    }

    public PageRequest(int pageIndex, int limit)
    {
      PageIndex = pageIndex;
      Limit = limit;
    }

    public PageRequest()
    {
    }

    public int PageIndex { get; set; }
    public virtual string? OrderBy { get; set; }

    public int Limit
    {
      get => _limit.GetValueOrDefault(MinLimit);
      set
      {
        var sanitizedValue = SanitizeLimitValue(value);
        _limit = sanitizedValue;
      }
    }

    [JsonIgnore] public int Offset => Limit * PageIndex;

    [JsonIgnore] public bool IsOrdered => !string.IsNullOrWhiteSpace(OrderBy);

    protected virtual int SanitizeLimitValue(int value)
    {
      return Math.Min(Math.Max(value, MinLimit), MaxLimit);
    }

    public PageRequest Clone()
    {
      return (PageRequest) MemberwiseClone();
    }

    public bool IsOrderedBy(string order)
    {
      return IsOrdered && OrderBy!.Contains(order, StringComparison.InvariantCultureIgnoreCase);
    }

    public void RemoveOrderingBy(string order)
    {
      if (!IsOrdered)
      {
        return;
      }

      var tokens = OrderBy!.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
      var propNames = tokens
        .Where(propName => !propName.StartsWith(order, StringComparison.InvariantCultureIgnoreCase));

      OrderBy = string.Join(", ", propNames);
    }
  }
}