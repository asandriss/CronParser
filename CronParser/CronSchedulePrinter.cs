namespace CronParser;

public static class CronSchedulePrinter
{
   public static void Print(CronSchedule schedule, TextWriter writer)
   {
      PrintField(writer, "minute", schedule.Minutes);
      PrintField(writer, "hour", schedule.Hours);
      PrintField(writer, "day of month", schedule.DaysOfMonth);
      PrintField(writer, "month", schedule.Months);
      PrintField(writer, "day of week", schedule.DaysOfWeek);
      if(schedule.Year.Count > 0)
         PrintField(writer, "year", schedule.Year );
      
      writer.WriteLine($"{ "command",-14}{schedule.Command}");
   }

   private static void PrintField(TextWriter writer, string fieldName, IList<int> values)
   {
      writer.WriteLine($"{fieldName, -14}{string.Join(" ", values)}");
   }
}