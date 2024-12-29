using System.Text.RegularExpressions;

var arcadeInput = File.ReadAllText("input.txt").Split("\n\n");

var arcades = arcadeInput.Select(ParseNumbersInLine).Select(ints =>
    new Arcade(new Offset(ints[0], ints[1]), new Offset(ints[2], ints[3]), new Location(10000000000000 + ints[4], 10000000000000 + ints[5])));

var sum = arcades
    .Select(arcade => arcade.Solve())
    .Where(cost => cost != -1)
    .Sum();

Console.WriteLine(sum);

record Arcade(Offset ButtonA, Offset ButtonB, Location Prize)
{
    public long Solve()
    {
        // got help from: https://blog.jverkamp.com/2024/12/13/aoc-2024-day-13-cramerinator
        // apologies for these variable names...
        var axpyaypx = ButtonA.X * Prize.Y - ButtonA.Y * Prize.X;
        var axbybxay = ButtonA.X * ButtonB.Y - ButtonB.X * ButtonA.Y;

        if (axpyaypx % axbybxay != 0)
            return -1;

        var b = axpyaypx / axbybxay;

        var pxbbx = Prize.X - b * ButtonB.X;
        if (pxbbx % ButtonA.X != 0)
            return -1;

        var a = pxbbx / ButtonA.X;

        return a * 3 + b;
    }
}

readonly record struct Offset(long X, long Y)
{
}

readonly record struct Location(long X, long Y)
{
    public Location Move(Offset offset) => new(X + offset.X, Y + offset.Y);

    public double DistanceFrom(Location other) =>
        Math.Sqrt(Math.Pow(Math.Abs(X - other.X), 2) + Math.Pow(Math.Abs(Y - other.Y), 2));

    public bool IsOob(long maxX, long maxY) => X >= maxX || Y >= maxY;
}

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();

    private static List<long> ParseNumbersInLine(string line) =>
        NumberRegex().Matches(line).Select(match => match.Value).Select(long.Parse).ToList();
}
