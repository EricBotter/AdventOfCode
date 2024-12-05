using System.Text.RegularExpressions;

var numberRegex = NumberRegex();

var lines = File.ReadAllLines("input.txt");

var reports = lines.Select(line => numberRegex.Matches(line).Select(match => int.Parse(match.Value)).ToList());

var sum = 0;
var i = 1;
foreach (var report in reports)
{
    var reportValid = IsReportValid(report);
    Console.WriteLine($"Report {i++} is valid: {reportValid}");
    sum += reportValid ? 1 : 0;
}

Console.WriteLine(sum);
return;


bool IsReportValid(List<int> report)
    => IsRecursiveReportValid(report, false, true) || IsRecursiveReportValid(report, true, true);


bool IsRecursiveReportValid(List<int> report, bool increasing, bool errorTolerated)
{
    if (report.Count <= 1)
        return true;

    if (increasing)
    {
        if (report[1] - report[0] is >= 1 and <= 3)
            return IsRecursiveReportValid(report[1..], increasing, errorTolerated);
    }
    else
    {
        if (report[0] - report[1] is >= 1 and <= 3)
            return IsRecursiveReportValid(report[1..], increasing, errorTolerated);
    }

    if (!errorTolerated) return false;

    return IsRecursiveReportValid(report[1..], increasing, false)
        || IsRecursiveReportValid(report[2..].Prepend(report[0]).ToList(), increasing, false);
}

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();
}