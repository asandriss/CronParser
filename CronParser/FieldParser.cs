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
      
      if (part.Contains('/'))
         return ParseStep(part, min, max);
 
      if (part.Contains('-'))
         return ParseRange(part, min, max);
     
      return [ParseSingle(part, min, max)];
   }

   private static IEnumerable<int> ParseStep(string part, int min, int max)
   {
      var values = part.Split('/');
      if (values.Length != 2)
         throw new FormatException($"Invalid step expressions provided: {part}");

      var basePart = values[0];
      var step = int.Parse(values[1]);

      var baseRange = basePart == "*" 
         ? Range(min, max).ToArray() 
         : ParsePart(basePart, min, max).ToArray();

      return baseRange.Where(v => (v - baseRange.Min()) % step == 0);
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
   
   private static int ParseSingle(string part, int min, int max)
   {
      if (!int.TryParse(part, out var value))
         throw new FormatException($"Invalid value: '{part}' is not a number.");

      if (value < min || value > max)
         throw new ArgumentOutOfRangeException($"Value '{value}' out of range [{min}-{max}].");

      return value;
   }

   private static IEnumerable<int> Range(int start, int end)
   {
      return Enumerable.Range(start, end - start + 1);
   }
}