using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var sizeX = lines[0].Length;
var sizeY = lines.Length;

var map = new char[sizeX, sizeY];

for (var i = 0; i < sizeY; i++)
for (var j = 0; j < sizeX; j++)
{
    map[j, i] = lines[i][j];
}

var sum = 0;
for (var i = 0; i < sizeX; i++)
for (var j = 0; j < sizeY; j++)
{
    var location = new Location(i, j);
    if (map.At(location) == '0')
        sum += AnalyzeTrailhead('0', location).Count;
}

Console.WriteLine(sum);

HashSet<Location> AnalyzeTrailhead(char currentHeight, Location currentLocation)
{
    if (currentHeight == '9')
        return [currentLocation];
    return Enum.GetValues<Direction>().Select(dir => currentLocation.Move(dir))
        .Where(loc => !loc.IsOob(sizeX, sizeY))
        .Where(loc => map.At(loc) == currentHeight + 1)
        .SelectMany(loc => AnalyzeTrailhead((char)(currentHeight + 1), loc))
        .ToHashSet();
}


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