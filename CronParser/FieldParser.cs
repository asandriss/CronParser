namespace CronParser;

public static class FieldParser
{
   public static List<int> Parse(string expression, int min, int max)
   {
      if(string.IsNullOrWhiteSpace(expression))
         throw new ArgumentException("Expression cannot be null or empty.", nameof(expression));

      var values = new SortedSet<int>();

      foreach (var part in expression.Split(','))
      {
         var partValues = ParsePart(part.Trim(), min, max);
         values.UnionWith(partValues);
      }

      return values.ToList();
   }

   private static IEnumerable<int> ParsePart(string part, int min, int max)
   {
      if (part == "*")
         return Range(min, max);

      if (part.Contains('-'))
         return ParseRange(part, min, max);

      if (part.Contains('/'))
         return ParseStep(part, min, max);
      
      return [0];
   }

   private static IEnumerable<int> ParseStep(string part, int min, int max)
   {
      var values = part.Split('/');
      if (values.Length != 2)
         throw new FormatException($"Invalid step expressions provided: {part}");

      var basePart = values[0];
      var step = int.Parse(values[1]);

      return Range(min, max).Where(v => (v - min) % step == 0);
   }

   private static IEnumerable<int> ParseRange(string part, int min, int max)
   {
      var values = part.Split('-').Select(int.Parse).ToArray();
      if (values.Length != 2)
         throw new FormatException($"Invalid range provided {part}");

      int start = values[0];
      int end = values[1];

      if (start < min || end > max)
         throw new ArgumentOutOfRangeException($"Range {part} out of bounds [{min}-{max}]");

      return Range(start, end);
   }

   private static List<int> Range(int start, int end)
   {
      return Enumerable.Range(start, end - start + 1).ToList();
   }
}