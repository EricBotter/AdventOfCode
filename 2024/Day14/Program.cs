using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var robots = lines.Select(ParseNumbersInLine)
    .Select(nums => new Robot(new Location(nums[0], nums[1]), new Offset(nums[2], nums[3])))
    .ToList();

robots.ForEach(robot => robot.BuildLoop());

var movedRobots = robots
    .Select(robot => robot.Move(100))
    .ToList();

var output = movedRobots.Select(robot => robot.Location switch
    {
        ( < SizeX/2, < SizeY/2 ) => 1,
        ( > SizeX/2, < SizeY/2 ) => 2,
        ( < SizeX/2, > SizeY/2 ) => 3,
        ( > SizeX/2, > SizeY/2 ) => 4,
        _ => -1
    })
    .Where(quadrant => quadrant != -1)
    .GroupBy(x => x)
    .Aggregate(1, (i, j) => i * j.Count());

Console.WriteLine(output);

readonly struct Robot(Location location, Offset velocity)
{
    public Location Location { get; private init; } = location;
    private Offset Velocity { get; } = velocity;

    private readonly List<Location> _loop = [];

    public void BuildLoop()
    {
        var current = Location;
        do
        {
            _loop.Add(current);
            current = current.Move(Velocity);
        } while (current != Location);
    }

    public Robot Move(int times) => this with { Location = _loop[times % _loop.Count] };
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
