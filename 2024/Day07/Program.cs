using System.Text.RegularExpressions;

Func<string, List<long>> parseLineToNumberList =
    line => NumberRegex().Matches(line).Select(match => long.Parse(match.Value)).ToList();

var calibrations = File.ReadAllLines("input.txt").Select(parseLineToNumberList).Select(Calibration.FromInts);

var sum = calibrations.Where(calibration => calibration.IsValid()).Sum(calibration => calibration.Target);

Console.WriteLine(sum);

record Calibration(long Target, List<long> Operands)
{
    public static Calibration FromInts(List<long> values) => new(values[0], values[1..]);

    public bool IsValid()
    {
        return InternalIsValid(Operands[0], Operands[1..]);

        bool InternalIsValid(long currentValue, List<long> remainingValues)
        {
            if (currentValue > Target)
                return false;
            if (remainingValues.Count == 0)
                return currentValue == Target;

            return InternalIsValid(currentValue + remainingValues[0], remainingValues[1..])
                   || InternalIsValid(currentValue * remainingValues[0], remainingValues[1..]);
        }
    }
}

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();
}