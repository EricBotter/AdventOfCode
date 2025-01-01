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



enum Direction
{
    North,
    West,
    South,
    East,
}

readonly record struct Offset(int X, int Y)
{
}

readonly record struct Location(int X, int Y)
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

    public Location Move(Offset offset) => new(X + offset.X, Y + offset.Y);

    public bool IsOob(int maxX, int maxY) => X < 0 || Y < 0 || X >= maxX || Y >= maxY;
}

public class MultiValueDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>> where TKey : notnull
{
    public void Add(TKey key, TValue value) {
        List<TValue> values;
        if (!TryGetValue(key, out values)) {
            values = new List<TValue>();
            Add(key, values);
        }
        values.Add(value);
    }
}

static class Exts
{
    public static ref T At<T>(this T[,] source, Location location) => ref source[location.X, location.Y];

    public static Direction Opposite(this Direction direction) => direction switch
    {
        Direction.North => Direction.South,
        Direction.East => Direction.West,
        Direction.South => Direction.North,
        Direction.West => Direction.East,

        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };

    public static Direction RotateRight(this Direction direction) => direction switch
    {
        Direction.North => Direction.East,
        Direction.East => Direction.South,
        Direction.South => Direction.West,
        Direction.West => Direction.North,

        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };

    public static Direction RotateLeft(this Direction direction) => direction switch
    {
        Direction.North => Direction.West,
        Direction.East => Direction.North,
        Direction.South => Direction.East,
        Direction.West => Direction.South,

        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };

    public static IEnumerable<T> ToEnumerable<T>(this T[,] source, int sizeX, int sizeY) =>
        Enumerable.Range(0, sizeX)
            .SelectMany(x => Enumerable.Range(0, sizeY).Select(y => (x, y)))
            .Select(t => source[t.x, t.y]);
}

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();

    private static List<int> ParseNumbersInLine(string line) =>
        NumberRegex().Matches(line).Select(match => match.Value).Select(int.Parse).ToList();

    private static IEnumerable<Direction> Directions() => Enum.GetValues<Direction>();

    private static IEnumerable<Location> AjdacentTo(Location loc, int sizeX, int sizeY) =>
        Directions()
            .Select(loc.Move)
            .Where(l => !l.IsOob(sizeX, sizeY));

}
