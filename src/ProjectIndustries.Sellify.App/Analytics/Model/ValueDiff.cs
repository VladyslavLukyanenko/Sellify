using System;

namespace ProjectIndustries.Sellify.App.Analytics.Model
{
  public class ValueDiff<T> where T : struct
  {
    public static ValueDiff<int> CreateInt32(int curr, int? prev) => new()
    {
      Current = curr,
      Previous = prev,
      ChangePercents = CalculatePercents(curr, prev)
    };

    public static ValueDiff<float> CreateSingle(float curr, float? prev) => new()
    {
      Current = curr,
      Previous = prev,
      ChangePercents = CalculatePercents((decimal) curr, prev == null ? null : (decimal) prev)
    };

    public static ValueDiff<decimal> CreateDecimal(decimal curr, decimal? prev) => new()
    {
      Current = decimal.Round(curr, 2),
      Previous = prev == null ? null : decimal.Round(prev.Value, 2),
      ChangePercents = CalculatePercents(curr, prev)
    };

    private static float? CalculatePercents(decimal curr, decimal? prev)
    {
      if (prev == null)
      {
        return null;
      }

      return curr == 0
        ? curr == prev ? 0 : -100
        : (float) Math.Round((curr - prev.Value) / curr * 100, 1);
    }

    public T Current { get; set; }
    public T? Previous { get; set; }

    public float? ChangePercents { get; set; }
  }
}