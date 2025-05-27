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
        var expected = new List<int>() { 1, 2, 3 };
        
        actual.ShouldBeEquivalentTo(expected);
    }
}