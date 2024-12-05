using System.Text.RegularExpressions;

var numberRegex = NumberRegex();

Func<string, List<int>> parseNumbersInLine =
    line => numberRegex.Matches(line).Select(match => match.Value).Select(int.Parse).ToList();

var blocks = File.ReadAllText("input.txt").Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

var rules = blocks[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(parseNumbersInLine).Select(Rule.FromLine).ToList();
var updates = blocks[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(parseNumbersInLine).ToList();

var sum = updates.Where(update => rules.All(rule => rule.Valid(update)))
    .Sum(update => update[(update.Count - 1) / 2]);

Console.WriteLine(sum);


record Rule(int Before, int After)
{
    public static Rule FromLine(List<int> line) => new(line[0], line[1]);

    public bool Valid(List<int> update)
    {
        if (update.Contains(Before) && update.Contains(After))
            return update.IndexOf(Before) < update.IndexOf(After);
        return true;
    }
}


partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();
}