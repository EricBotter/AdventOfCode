var directionMap = new Dictionary<Direction, Direction>
{
    { Direction.North, Direction.East },
    { Direction.East, Direction.South },
    { Direction.South, Direction.West },
    { Direction.West, Direction.North }
};

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

var sum = 0;

for (var i = 0; i < sizeX; i++)
for (var j = 0; j < sizeY; j++)
{
    Console.WriteLine($"testing {i}, {j}");

    if (map[i, j] != '.')
        continue;

    map[i, j] = '#';

    if (Run())
        sum++;

    map[i, j] = '.';
}

Console.WriteLine(sum);


bool Run()
{
    var knownLocations = new MultiValueDictionary<Location, Direction>();
    var position = initialPosition;
    var direction = Direction.North;

    while (!position.IsOob(sizeX, sizeY))
    {
        var checkPosition = position.Move(direction);

        if (knownLocations.ContainsKey(checkPosition) && knownLocations[checkPosition].Contains(direction))
            return true;

        if (!checkPosition.IsOob(sizeX, sizeY) && map.At(checkPosition) == '#')
        {
            direction = directionMap[direction];
            continue;
        }

        position = checkPosition;
        if (!position.IsOob(sizeX, sizeY))
        {
            knownLocations.Add(position, direction);
        }
    }

    return false;
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
        if (!this.TryGetValue(key, out values)) {
            values = new List<Value>();
            this.Add(key, values);
        }
        values.Add(value);
    }
}

static class Exts
{
    public static ref T At<T>(this T[,] source, Location location) => ref source[location.X, location.Y];
}