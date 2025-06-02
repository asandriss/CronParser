namespace CronParser;

public static class CronScheduleFactory
{
   public static CronSchedule Parse(string expression)
   {
      List<int> year = [];
      string command = string.Empty;
      
      if (string.IsNullOrWhiteSpace(expression))
         throw new ArgumentException("Cron expression cannot be empty");
      
      var parts = expression.Split(' ', 6, StringSplitOptions.RemoveEmptyEntries);

      if (parts.Length != 6)
         throw new ArgumentException("Cron expression must contain 5 time fields and a command");

      if (!parts[5].StartsWith('.') && !parts[5].StartsWith('/'))
      {
         year = FieldParser.Parse(parts[5].Split(' ').First(), 2000, 3000);
         command = string.Join(' ', parts[5].Split(' ').Skip(1).ToArray());
      }

      return new CronSchedule(
         minutes: FieldParser.Parse(parts[0], 0, 59),
         hours: FieldParser.Parse(parts[1], 0, 23),
         daysOfMonth: FieldParser.Parse(parts[2], 1, 31),
         months: FieldParser.Parse(parts[3], 1, 12),
         daysOfWeek: FieldParser.Parse(parts[4], 0, 6),
         year: year,
         command: command == String.Empty ? parts[5] : command
      );
   }
}