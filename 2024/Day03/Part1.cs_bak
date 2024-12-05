using System.Text.RegularExpressions;

var numberRegex = NumberRegex();
var mulRegex = MulRegex();

var input = File.ReadAllText("input.txt");

var matches = mulRegex.Matches(input);

var mults = matches.Select(match =>
{
    var operands = numberRegex.Matches(match.Value);
    return (int.Parse(operands[0].Value), int.Parse(operands[1].Value));
});

var sum = mults.Sum(mult => mult.Item1 * mult.Item2);

Console.WriteLine(sum);


partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();

    [GeneratedRegex(@"mul\([0-9]+,[0-9]+\)")]
    private static partial Regex MulRegex();

}