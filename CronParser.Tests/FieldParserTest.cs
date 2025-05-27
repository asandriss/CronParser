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

    [Fact]
    public void ParseSteps_ShouldReturn_CorrectValues()
    {
        var actual = FieldParser.Parse("*/2", 0, 5);
        var expected = new List<int> { 0, 2, 4 };
        
        actual.ShouldBeEquivalentTo(expected);
    }
}