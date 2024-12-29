using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var robots = lines.Select(ParseNumbersInLine)
    .Select(nums => new Robot(new Location(nums[0], nums[1]), new Offset(nums[2], nums[3])))
    .ToList();

robots.ForEach(robot => robot.BuildLoop());

var count = 0;
while (true)
{
    count++;
    robots.ForEach(robot => robot.Move(8258));

    var map = robots.GroupBy(robot => robot.Location)
        .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());

    for (var i = 0; i < SizeY; i++)
    {
        for (var j = 0; j < SizeX; j++)
        {
            if (map.TryGetValue(new Location(j, i), out var value))
                Console.Write(value);
            else
                Console.Write('.');
        }
        Console.WriteLine();
    }

    Console.ReadLine();
    Console.Clear();

    // var mapX = robots.GroupBy(robot => robot.Location.X)
    //     .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());
    // var mapY = robots.GroupBy(robot => robot.Location.Y)
    //     .ToDictionary(grouping => grouping.Key, grouping => grouping.Count());
    //
    // if (mapX.Count == 89 && mapY.Count == 88)
    // {
    //     Console.WriteLine(mapX.Count);
    //
    //     Console.WriteLine(count);
    //     Console.ReadLine();
    //     Console.Clear();
    // }
}

class Robot(Location location, Offset velocity)
{
    public Location Location { get; private set; } = location;
    private Offset Velocity { get; } = velocity;

    private readonly List<Location> _loop = [];
    private int _loopPos = 0;

    public void BuildLoop()
    {
        var current = Location;
        do
        {
            _loop.Add(current);
            current = current.Move(Velocity);
        } while (current != Location);
    }

    public Robot Move(int times)
    {
        _loopPos = (_loopPos + times) % _loop.Count;
        Location = _loop[_loopPos];
        return this;
    }
}

readonly record struct Offset(int X, int Y)
{
}

readonly record struct Location(int X, int Y)
{
    public Location Move(Offset offset) =>
        new((X + offset.X + Program.SizeX) % Program.SizeX, (Y + offset.Y + Program.SizeY) % Program.SizeY);
}

partial class Program
{
    [GeneratedRegex("-?[0-9]+")]
    private static partial Regex NumberRegex();

    public const int SizeX = 101;
    public const int SizeY = 103;

    private static List<int> ParseNumbersInLine(string line) =>
        NumberRegex().Matches(line).Select(match => match.Value).Select(int.Parse).ToList();
}
