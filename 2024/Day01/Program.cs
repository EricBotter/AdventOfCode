using System.Text.RegularExpressions;

List<int> firstList = [];
List<int> secondList = [];

var lines = File.ReadAllLines("input.txt");
var numberRegex = NumberRegex();

foreach (var lineMatches in lines.Select(line => numberRegex.Matches(line)))
{
    firstList.Add(int.Parse(lineMatches[0].Value));
    secondList.Add(int.Parse(lineMatches[1].Value));
}

firstList.Sort();
secondList.Sort();

var sum = firstList.Sum(num => num * secondList.Count(x => x == num));

Console.WriteLine(sum);

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();
}