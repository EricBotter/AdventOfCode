using System.Text.RegularExpressions;

var numberRegex = NumberRegex();

var lines = File.ReadAllLines("input.txt");

var reports = lines.Select(line => numberRegex.Matches(line).Select(match => int.Parse(match.Value)).ToList());

var sum = 0;
foreach (var report in reports)
{
    List<int> diffs = [];
    for (var i = 1; i < report.Count; i++)
    {
        diffs.Add(report[i] - report[i - 1]);
    }

    sum += diffs.All(x => x is <= -1 and >= -3) || diffs.All(x => x is >= 1 and <= 3) ? 1 : 0;
}

Console.WriteLine(sum);

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();
}