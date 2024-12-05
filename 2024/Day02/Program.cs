using System.Text.RegularExpressions;

var numberRegex = NumberRegex();

var lines = File.ReadAllLines("input.txt");

var reports = lines.Select(line => numberRegex.Matches(line).Select(match => int.Parse(match.Value)).ToList());

var sum = 0;
var count = 1;
foreach (var report in reports)
{
    var reportValid = IsReportValid(report);
    Console.WriteLine($"Report {count++} is valid: {reportValid}");
    sum += reportValid ? 1 : 0;
}

Console.WriteLine(sum);
return;


bool IsReportValid(List<int> report)
{
    if (IsSubReportValid(report, true) || IsSubReportValid(report, false))
        return true;

    for (var i = 0; i < report.Count; i++)
    {
        List<int> test = [..report];
        test.RemoveAt(i);
        if (IsSubReportValid(test, true) || IsSubReportValid(test, false))
            return true;
    }

    return false;
}

bool IsSubReportValid(List<int> report, bool increasing)
{
    if (report.Count <= 1)
        return true;

    if (increasing)
    {
        if (report[1] - report[0] is >= 1 and <= 3)
            return IsSubReportValid(report[1..], increasing);
    }
    else
    {
        if (report[0] - report[1] is >= 1 and <= 3)
            return IsSubReportValid(report[1..], increasing);
    }

    return false;
}

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();
}