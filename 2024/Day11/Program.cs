using System.Text.RegularExpressions;

var cache = new Dictionary<(long, long), long>();


var line = File.ReadAllText("input.txt").Trim();

var numbers = ParseNumbersInLine(line);

var sum = numbers.Sum(x => StonesForNumber(x, 75));

Console.WriteLine(sum);

long StonesForNumber(long n, long iterationsLeft)
{
    if (cache.TryGetValue((n, iterationsLeft), out var value))
        return value;

    if (iterationsLeft == 0)
        return 1;

    if (n == 0)
        return CacheOp(StonesForNumber(1, iterationsLeft - 1));

    var length = n.ToString().Length;
    if (length % 2 == 0)
    {
        var limit = Math.Pow(10, length / 2);
        return CacheOp(StonesForNumber((long)(n / limit), iterationsLeft - 1) + StonesForNumber((long)(n % limit), iterationsLeft - 1));
    }

    return CacheOp(StonesForNumber(n * 2024, iterationsLeft - 1));

    long CacheOp(long result)
    {
        cache[(n, iterationsLeft)] = result;
        return result;
    }
}

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();

    private static List<long> ParseNumbersInLine(string line) =>
        NumberRegex().Matches(line).Select(match => match.Value).Select(long.Parse).ToList();
}