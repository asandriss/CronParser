using CronParser;

try
{
    if (args.Length != 1)
    {
        var helpText = """
                     Usage: CronParser <cron expression>

                     Standard cron expression format:
                       minute       0-59
                       hour         0-23
                       day of month 1-31
                       month        1-12
                       day of week  0-6 (0 = Sunday)
                       
                     Example: CronParser "*/15 0 1,15 * 1-5 /usr/bin/find"
                       This will schedule the command to run every 15 minutes at hour 0 on the 1st and 15th of each month, Monday to Friday.
                     """;
        
        Console.WriteLine(helpText);
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