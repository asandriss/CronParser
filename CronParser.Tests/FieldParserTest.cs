using Shouldly;

namespace CronParser.Tests;

public class FieldParserTest
{
    [Fact]
    public void ParseStar_ShouldReturn_AllValuesMinToMax()
    {
        var actual = FieldParser.Parse("*", 1, 3);
        var expected = new List<int> { 1, 2, 3 };
        
        actual.ShouldBeEquivalentTo(expected);
    }
    
    [Theory]
    [InlineData("5", 0, 59, new[] { 5 })]
    [InlineData("0", 0, 0, new[] { 0 })]
    [InlineData("59", 0, 59, new[] { 59 })]
    public void ParseSingleValue_ShouldReturn_SingleValue(string expression, int min, int max, int[] expected)
    {
        var actual = FieldParser.Parse(expression, min, max).ToArray();
        actual.ShouldBeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("1-3", 1, 60, new[] { 1, 2, 3 })]
    [InlineData("5-5", 1, 60, new[] { 5 })]
    [InlineData("1-7", 1, 60, new[] {1, 2, 3, 4, 5, 6, 7 })]
    [InlineData("1-1", 1, 60, new[] { 1 })]
    [InlineData("30-31", 1, 31, new[] { 30, 31 })]
    public void ParseRange_ShouldReturn_ValuesInRange(string expression, int min, int max, int[] expected)
    {
        var actual = FieldParser.Parse(expression, min, max).ToArray();
        actual.ShouldBeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("1-", 1, 60)]
    [InlineData("-5", 1, 60)]
    [InlineData("a-b", 1, 60)]
    public void ParseRange_ShouldThrow_OnInvalidRangeFormat(string expression, int min, int max)
    {
        Assert.Throws<FormatException>(() => FieldParser.Parse(expression, min, max).ToArray());
    }

    [Theory]
    [InlineData("5-1", 1, 60)]
    [InlineData("0-5", 1, 60)]
    [InlineData("1-61", 1, 60)]
    public void ParseRange_ShouldThrow_OnOutOfBoundsRange(string expression, int min, int max)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => FieldParser.Parse(expression, min, max).ToArray());
    }
 
    [Theory]
    [InlineData("*/2", 0, 5, new[] {0, 2, 4})]
    [InlineData("*/5", 0, 20, new[] {0, 5, 10, 15, 20})]
    [InlineData("*/1", 0, 3, new[] {0, 1, 2, 3})] 
    [InlineData("*/10", 10, 40, new[] {10, 20, 30, 40})]
    [InlineData("10-20/3", 0, 59, new[] {10, 13, 16, 19})]
    [InlineData("5-15/5", 0, 59, new[] {5, 10, 15})]
    [InlineData("1-5/10", 0, 59, new[] {1})]
    [InlineData("1-1/1", 0, 59, new[] {1})]
    [InlineData("0-59/20", 0, 59, new[] {0, 20, 40})]
    public void ParseSteps_ShouldReturn_CorrectValues(string expression, int min, int max, int[] expected)
    {
        var actual = FieldParser.Parse(expression, min, max).ToArray();
        
        actual.ShouldBeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("1,2,3", 0, 59, new[] {1,2,3})]
    [InlineData("1,2,3,2,2,2,1,3", 0, 59, new[] {1,2,3})]
    [InlineData("1-3,2-5", 0, 59, new[] {1,2,3,4,5})]
    [InlineData("1-3,1-3,1-3", 0, 59, new[] {1,2,3})]
    [InlineData("1-3,5-6", 0, 59, new[] {1,2,3,5,6})]
    [InlineData("5,4,3,2,1", 0, 59, new[] {1,2,3,4,5})]
    [InlineData("10,10,10,10", 0, 59, new[] {10})]
    [InlineData("*/20,0-5", 0, 59, new[] {0,1,2,3,4,5,20,40})]
    [InlineData("1-5,*/20,*/30", 0, 59, new[] {0,1,2,3,4,5,20,30,40})]
    public void MultipeCombinationsOfCommaSeparatedValues_ShouldReturn_SortedSetOfValuesWithoutDuplicates(string expression, int min, int max, int[] expected)
    {
        var actual = FieldParser.Parse(expression, min, max).ToArray();
        
        actual.ShouldBeEquivalentTo(expected);
    }

    [Fact]
    public void Parse_ShouldThrow_OnMixedExpressionWithInvalidPart()
    {
        Assert.Throws<FormatException>(() => FieldParser.Parse("1-3,failHere", 0, 10));
    }
}