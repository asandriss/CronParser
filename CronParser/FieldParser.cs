namespace CronParser;

public static class FieldParser
{
   public static List<int> Parse(string expression, int min, int max)
   {
      if(string.IsNullOrWhiteSpace(expression))
         throw new ArgumentException("Expression cannot be null or empty.", nameof(expression));

      if (expression == "*")
         return Range(min, max);

      return [0];
   }

   private static List<int> Range(int start, int end)
   {
      return Enumerable.Range(start, end - start + 1).ToList();
   }
}