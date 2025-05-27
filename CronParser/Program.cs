// See https://aka.ms/new-console-template for more information

using CronParser;

try
{
    if (args.Length != 1)
    {
        Console.WriteLine("Usage: CronParser \"<cron expression>\"");
        return;
    }

    var schedule = CronScheduleFactory.Parse(args[0]);
    CronSchedulePrinter.Print(schedule, Console.Out);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Environment.Exit(1);
}