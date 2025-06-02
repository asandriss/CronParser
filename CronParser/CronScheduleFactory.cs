namespace CronParser;

public static class CronScheduleFactory
{
   public static CronSchedule Parse(string expression)
   {
      if (string.IsNullOrWhiteSpace(expression))
         throw new ArgumentException("Cron expression cannot be empty");
      
      var parts = expression.Split(' ', 7, StringSplitOptions.RemoveEmptyEntries);

      if (parts.Length < 6)
         throw new ArgumentException("Cron expression must contain 5 time fields and a command");

      if (parts[5].StartsWith('.') || parts[5].StartsWith('/'))
      {
         var newExpression = $"{parts[0]} {parts[1]} {parts[2]} {parts[3]} {parts[4]} * {parts[5]}";
         return Parse(newExpression);
      }

      return new CronSchedule(
         minutes: FieldParser.Parse(parts[0], 0, 59),
         hours: FieldParser.Parse(parts[1], 0, 23),
         daysOfMonth: FieldParser.Parse(parts[2], 1, 31),
         months: FieldParser.Parse(parts[3], 1, 12),
         daysOfWeek: FieldParser.Parse(parts[4], 0, 6),
         year: FieldParser.Parse(parts[5], 2000, 3000),
         command: parts[6]
      );
   }
}