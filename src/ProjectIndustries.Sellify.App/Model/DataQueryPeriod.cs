using CSharpFunctionalExtensions;
using NodaTime;
using NodaTime.Text;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.App.Model
{
  public abstract class DataQueryPeriod : Enumeration
  {
    public static readonly DataQueryPeriod Yearly = new YearlyDataQueryPeriod(1, nameof(Yearly));
    public static readonly DataQueryPeriod Monthly = new MonthlyDataQueryPeriod(2, nameof(Monthly));
    public static readonly DataQueryPeriod AllTime = new AllTimeDataQueryPeriod(3, nameof(AllTime));

    private DataQueryPeriod(int id, string name)
      : base(id, name)
    {
    }

    public abstract Result<(Instant? StartOfPrev, Instant? Start, Instant? End)> GetTotalRange(string rawStart,
      Offset offset);

    public abstract string GetGroupingUnit(LocalDate date);
    public abstract bool IsPrevious(LocalDate? start, LocalDate date);

    private class AllTimeDataQueryPeriod : DataQueryPeriod
    {
      public AllTimeDataQueryPeriod(int id, string name) : base(id, name)
      {
      }

      public override Result<(Instant? StartOfPrev, Instant? Start, Instant? End)> GetTotalRange(string rawStart,
        Offset offset) => (null, null, null);

      public override string GetGroupingUnit(LocalDate date) => LocalDatePattern.Iso.Format(date);

      public override bool IsPrevious(LocalDate? start, LocalDate date) => false;
    }

    private class YearlyDataQueryPeriod : DataQueryPeriod
    {
      public YearlyDataQueryPeriod(int id, string name)
        : base(id, name)
      {
      }

      public override Result<(Instant? StartOfPrev, Instant? Start, Instant? End)> GetTotalRange(string rawStart,
        Offset offset)
      {
        if (!int.TryParse(rawStart, out var year))
        {
          return Result.Failure<(Instant? StartOfPrev, Instant? Start, Instant? End)>("Can't parse start year");
        }

        var startTime = new OffsetDateTime(new LocalDateTime(year, 1, 1, 00, 00), offset);
        var startOfPrevTime = startTime.With((LocalDate date) => date.Minus(Period.FromYears(1)));
        var endTime = new OffsetDateTime(new LocalDateTime(year, 12, 31, 23, 59, 59, 999), offset);

        return (startOfPrevTime.ToInstant(), startTime.ToInstant(), endTime.ToInstant());
      }

      public override string GetGroupingUnit(LocalDate date) => LocalDatePattern.Iso.Format(date);

      public override bool IsPrevious(LocalDate? start, LocalDate date) =>
        start.HasValue && start.Value.Year < date.Year;
    }

    private class MonthlyDataQueryPeriod : DataQueryPeriod
    {
      public MonthlyDataQueryPeriod(int id, string name) : base(id, name)
      {
      }

      public override Result<(Instant? StartOfPrev, Instant? Start, Instant? End)> GetTotalRange(string rawStart,
        Offset offset)
      {
        var parseResult = YearMonthPattern.Iso.Parse(rawStart);
        if (!parseResult.Success)
        {
          return Result.Failure<(Instant? StartOfPrev, Instant? Start, Instant? End)>("Can't parse start month");
        }

        var startDate = parseResult.Value;
        var startMonth = startDate.Month;
        var startYear = startDate.Year;

        var startTime = new OffsetDateTime(new LocalDateTime(startYear, startMonth, 1, 00, 00), offset);
        var startOfPrevTime = startTime.With((LocalDate date) => date.Minus(Period.FromMonths(1)));

        var endDay = startTime.Calendar.GetDaysInMonth(startYear, startMonth);
        var endTime = new OffsetDateTime(new LocalDateTime(startYear, startMonth, endDay, 23, 59, 59, 999), offset);

        return (startOfPrevTime.ToInstant(), startTime.ToInstant(), endTime.ToInstant());
      }

      public override string GetGroupingUnit(LocalDate date) => LocalDatePattern.Iso.Format(date);

      public override bool IsPrevious(LocalDate? start, LocalDate date) =>
        start.HasValue &&
        (start.Value.Year == date.Year
          ? start.Value.Month < date.Month
          : start.Value.Year < date.Year);
    }
  }
}