using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var sizeX = lines[0].Length;
var sizeY = lines.Length;

var map = new char[sizeX, sizeY];

var initialPosition = new Location(0, 0);
for (var i = 0; i < sizeY; i++)
for (var j = 0; j < sizeX; j++)
{
    map[j, i] = lines[i][j];
    if (map[j, i] == '^')
    {
        initialPosition = new Location(j, i);
        map[j, i] = 'X';
    }
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

public class MultiValueDictionary<Key, Value> : Dictionary<Key, List<Value>> {
    public void Add(Key key, Value value) {
        List<Value> values;
        if (!TryGetValue(key, out values)) {
            values = new List<Value>();
            Add(key, values);
        }
        values.Add(value);
    }
}

static class Exts
{
    public static ref T At<T>(this T[,] source, Location location) => ref source[location.X, location.Y];
}

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();

    public static List<int> ParseNumbersInLine(string line) =>
        NumberRegex().Matches(line).Select(match => match.Value).Select(int.Parse).ToList();
}
