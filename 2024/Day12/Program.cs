var lines = File.ReadAllLines("input.txt");

var sizeX = lines[0].Length;
var sizeY = lines.Length;

var map = new char[sizeX, sizeY];
var zoneMap = new int[sizeX, sizeY];

for (var i = 0; i < sizeY; i++)
for (var j = 0; j < sizeX; j++)
{
    map[j, i] = lines[i][j];
}

var zoneIdx = 0;
for (var x = 0; x < sizeX; x++)
for (var y = 0; y < sizeY; y++)
{
    var currentLocation = new Location(x, y);
    if (zoneMap.At(currentLocation) != 0)
        continue;

    zoneMap.At(currentLocation) = ++zoneIdx;

    List<Location> zone = [currentLocation];
    while (zone.Count > 0)
    {
        var toAdd = AjdacentTo(zone[0], sizeX, sizeY)
            .Where(loc => zoneMap.At(loc) == 0)
            .Where(loc => map.At(loc) == map.At(currentLocation))
            .ToList();
        toAdd.ForEach(loc => zoneMap.At(loc) = zoneMap.At(zone[0]));
        zone.AddRange(toAdd);
        zone.RemoveAt(0);
    }
}

var areas = new Dictionary<int, int>();
var perimeters = new Dictionary<int, int>();

for (var i = 1; i <= zoneIdx; i++)
{
    areas[i] = 0;
    perimeters[i] = 0;
}

for (var x = 0; x < sizeX; x++)
for (var y = 0; y < sizeY; y++)
{
    var currentLocation = new Location(x, y);
    var currentZone = zoneMap.At(currentLocation);

    areas[currentZone]++;
    perimeters[currentZone] += 4 - AjdacentTo(currentLocation, sizeX, sizeY)
        .Where(loc => zoneMap.At(loc) == currentZone)
        .Count(loc => map.At(loc) == map.At(currentLocation));
}

var sum = 0;
for (var i = 1; i <= zoneIdx; i++)
{
    sum += areas[i] * perimeters[i];
}

Console.WriteLine(sum);

enum Direction
{
    North,
    West,
    South,
    East,
}

record Location(int X, int Y)
{
    public Location Move(Direction direction) =>
        direction switch
        {
            Direction.North => this with { Y = Y - 1 },
            Direction.East => this with { X = X + 1 },
            Direction.South => this with { Y = Y + 1 },
            Direction.West => this with { X = X - 1 },

            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    public bool IsOob(int maxX, int maxY) => X < 0 || Y < 0 || X >= maxX || Y >= maxY;
}

static class Exts
{
    public static ref T At<T>(this T[,] source, Location location) => ref source[location.X, location.Y];
}

partial class Program
{
    private static IEnumerable<Direction> Directions() => Enum.GetValues<Direction>();

    private static IEnumerable<Location> AjdacentTo(Location loc, int sizeX, int sizeY) => Directions()
        .Select(loc.Move)
        .Where(l => !l.IsOob(sizeX, sizeY));
}