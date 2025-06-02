using Shouldly;

namespace CronParser.Tests;

public class CronScheduleTest
{
    [Fact]
    public void CronSchedulePrinterWithoutAYear_ShouldRender_FullCorrectTable()
    {
        var schedule = new CronSchedule(
            [0, 15, 30, 45],
            [0],
            [1, 15],
            Enumerable.Range(1, 12).ToList(),
            [1, 2, 3, 4, 5],
            year: [],
            "/usr/bin/find"
        );

        using var sw = new StringWriter();
        CronSchedulePrinter.Print(schedule, sw);

        var expected = string.Join(Environment.NewLine, [
            "minute        0 15 30 45",
            "hour          0",
            "day of month  1 15",
            "month         1 2 3 4 5 6 7 8 9 10 11 12",
            "day of week   1 2 3 4 5",
            "command       /usr/bin/find"
        ]);

        sw.ToString().Trim().ShouldBe(expected);
    }

    [Fact]
    public void CronSchedulePrinterWitASpecificYear_ShouldRender_FullCorrectTable()
    {
        var schedule = new CronSchedule(
            [0, 15, 30, 45],
            [0],
            [1, 15],
            Enumerable.Range(1, 12).ToList(),
            [1, 2, 3, 4, 5],
            year: [2005],
            "/usr/bin/find"
        );

        using var sw = new StringWriter();
        CronSchedulePrinter.Print(schedule, sw);

        var expected = string.Join(Environment.NewLine, [
            "minute        0 15 30 45",
            "hour          0",
            "day of month  1 15",
            "month         1 2 3 4 5 6 7 8 9 10 11 12",
            "day of week   1 2 3 4 5",
            "year          2005",
            "command       /usr/bin/find"
        ]);

        sw.ToString().Trim().ShouldBe(expected);
    }

    [Fact]
    public void CronScheduleFactory_ShouldParse_ValidExpression()
    {
        var input = "*/15 0 1,15 * 1-5 /usr/bin/find";
        var schedule = CronScheduleFactory.Parse(input);

        schedule.Minutes.ShouldBe([0, 15, 30, 45]);
        schedule.Hours.ShouldBe([0]);
        schedule.DaysOfMonth.ShouldBe([1, 15]);
        schedule.Months.ShouldBe(Enumerable.Range(1, 12).ToList());
        schedule.DaysOfWeek.ShouldBe([1, 2, 3, 4, 5]);
        schedule.Command.ShouldBe("/usr/bin/find");
    }
    
    [Fact]
    public void CronScheduleFactory_ShouldParse_AYearParameter()
    {
        var input = "*/15 0 1,15 * 1-5 2005 /usr/bin/find";
        var schedule = CronScheduleFactory.Parse(input);

        schedule.Minutes.ShouldBe([0, 15, 30, 45]);
        schedule.Hours.ShouldBe([0]);
        schedule.DaysOfMonth.ShouldBe([1, 15]);
        schedule.Months.ShouldBe(Enumerable.Range(1, 12).ToList());
        schedule.DaysOfWeek.ShouldBe([1, 2, 3, 4, 5]);
        schedule.Year.ShouldBe([2005]);
        schedule.Command.ShouldBe("/usr/bin/find");
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("* * * * *")]
    public void CronScheduleFactory_ShouldThrow_OnInvalidFormat(string expression)
    {
        Assert.Throws<ArgumentException>(() => CronScheduleFactory.Parse(expression));
    }
}