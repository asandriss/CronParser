using CronParser;
using JetBrains.Annotations;
using Shouldly;

namespace CronParser.Tests;

[TestSubject(typeof(FieldParser))]
public class FieldParserTest
{
    [Fact]
    public void ParseStar_ShouldReturn_AllValuesMinToMax()
    {
        var actual = FieldParser.Parse("*", 1, 3);
        var expected = new List<int> { 1, 2, 3 };
        
        actual.ShouldBeEquivalentTo(expected);
    }

    [Fact]
    public void ParseRange_ShouldReturn_ValuesInRange()
    {
        var actual = FieldParser.Parse("1-3", 1, 60);
        var expected = new List<int> { 1, 2, 3 };
        
        actual.ShouldBeEquivalentTo(expected);
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
}