using Shouldly;

namespace CronParser.Tests;

public class CronScheduleTest
{
    [Fact]
    public void CronSchedulePrinter_ShouldPrint_CurrectlyFormattedOutput()
    {
        var schedule = new CronSchedule(
            [0, 15, 30, 45],
            [0],
            [1, 15],
            Enumerable.Range(1, 12).ToList(),
            [1, 2, 3, 4, 5],
            "/usr/bin/find"
            );

        using var sw = new StringWriter();
        CronSchedulePrinter.Print(schedule, sw);

        var actual = sw.ToString().Trim();
        actual.ShouldContain("minute        0 15 30 45");
        actual.ShouldContain("command       /usr/bin/find");
    }
}