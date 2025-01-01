var lines = File.ReadAllLines("input.txt");

var sizeX = lines[0].Length;
var sizeY = lines.Length;

var map = new char[sizeX, sizeY];

var start = new Location(0, 0);
var end = new Location(0, 0);

for (var i = 0; i < sizeY; i++)
for (var j = 0; j < sizeX; j++)
{
    map[j, i] = lines[i][j];
    if (map[j, i] == 'S')
    {
        start = new Location(j, i);
        map[j, i] = '.';
    }
    if (map[j, i] == 'E')
    {
        end = new Location(j, i);
        map[j, i] = '.';
    }
}

var pastWeights = new Dictionary<(Location, Direction), int>();

var activePaths = new LinkedList<Path>();
activePaths.AddLast(new Path(start, Direction.East, 0, []));

var finalPaths = new List<Path>();

while (activePaths.Count > 0)
{
    var current = activePaths.First!.Value;
    activePaths.RemoveFirst();

    if (!PathWeightExists(current))
    {
        if (current.Loc == end)
        {
            finalPaths.Add(current);
            continue;
        }

        if (map.At(current.Loc.Move(current.Dir)) != '#')
        {
            activePaths.AddLast(current.MoveForward());
        }

        if (map.At(current.Loc.Move(current.Dir.RotateRight())) != '#')
        {
            activePaths.AddLast(current.RotateRight());
        }

        if (map.At(current.Loc.Move(current.Dir.RotateLeft())) != '#')
        {
            activePaths.AddLast(current.RotateLeft());
        }
    }
}

var bestScore = pastWeights.Where(pair => pair.Key.Item1 == end).Min(pair => pair.Value);
var seats = finalPaths.Where(path => path.Score == bestScore).SelectMany(path => path.Previous).Append(end).ToHashSet();

foreach (var seat in seats)
{
    map.At(seat) = 'O';
}

for (var i = 0; i < sizeY; i++)
{
    for (var j = 0; j < sizeX; j++)
    {
        Console.Write(map[j, i]);
    }
    Console.WriteLine();
}

var count = seats.Count;
Console.WriteLine(count);

return;

bool PathWeightExists(Path path)
{
    if (pastWeights.TryGetValue((path.Loc, path.Dir), out var score))
    {
        if (score < path.Score)
            return true;
    }

    pastWeights[(path.Loc, path.Dir)] = path.Score;
    return false;
}

enum Direction
{
    North,
    West,
    South,
    East,
}

readonly record struct Path(Location Loc, Direction Dir, int Score, List<Location> Previous)
{
    public Path MoveForward() => new(Loc.Move(Dir), Dir, Score + 1, Previous.Append(Loc).ToList());

    public Path RotateRight() => new(Loc, Dir.RotateRight(), Score + 1000, Previous.Append(Loc).ToList());

    public Path RotateLeft() => new(Loc, Dir.RotateLeft(), Score + 1000, Previous.Append(Loc).ToList());
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
}

static class Exts
{
    public static ref T At<T>(this T[,] source, Location location) => ref source[location.X, location.Y];

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
}
