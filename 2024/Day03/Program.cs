using System.Text.RegularExpressions;

var numberRegex = NumberRegex();
var mulRegex = MulRegex();
var doRegex = DoRegex();
var dontRegex = DontRegex();

var input = File.ReadAllText("input.txt");

var mulMatches = mulRegex.Matches(input);
var doMatches = doRegex.Matches(input);
var dontMatches = dontRegex.Matches(input);

var allMatches = mulMatches.Concat(doMatches).Concat(dontMatches).OrderBy(match => match.Index);

var mulEnable = true;
var sum = 0;
foreach (var match in allMatches)
{
    mulEnable = match.Value switch
    {
        "don't()" => false,
        "do()" => true,
        _ => mulEnable
    };

    if (mulEnable && match.Value.StartsWith("mul"))
    {
        var operands = numberRegex.Matches(match.Value);
        sum += int.Parse(operands[0].Value) * int.Parse(operands[1].Value);
    }
}

Console.WriteLine(sum);

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();

    [GeneratedRegex(@"mul\([0-9]+,[0-9]+\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"do\(\)")]
    private static partial Regex DoRegex();

    [GeneratedRegex(@"don't\(\)")]
    private static partial Regex DontRegex();
}