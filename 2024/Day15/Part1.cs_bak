var sections = File.ReadAllText("input.txt").Split("\n\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

var lines = sections[0].Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

var sizeX = lines[0].Length;
var sizeY = lines.Length;

var map = new char[sizeX, sizeY];

var robot = new Location(0, 0);
for (var i = 0; i < sizeY; i++)
for (var j = 0; j < sizeX; j++)
{
    map[j, i] = lines[i][j];
    if (map[j, i] == '@')
        robot = new Location(j, i);
}

var commands = sections[1].Replace("\n", "");

foreach (var command in commands)
{
    var direction = command switch
    {
        '^' => Direction.North,
        '>' => Direction.East,
        'v' => Direction.South,
        '<' => Direction.West,
        _ => throw new Exception("filter ur commands better, u scrub")
    };

    var facingLocation = robot.Move(direction);
    var facingBlock = map.At(facingLocation);

    if (facingBlock == '#')
        continue;

    if (facingBlock == 'O')
    {
        while (facingBlock == 'O')
        {
            facingLocation = facingLocation.Move(direction);
            facingBlock = map.At(facingLocation);
        }
        if (facingBlock == '#')
            continue;

        while (facingLocation != robot)
        {
            var previousLocation = facingLocation.Move(direction.Opposite());
            map.At(facingLocation) = map.At(previousLocation);
            facingLocation = previousLocation;
        }

        map.At(robot) = '.';
        robot = robot.Move(direction);
    }
    else
    {
        map.At(facingLocation) = '@';
        map.At(robot) = '.';
        robot = facingLocation;
    }
}

var sum = 0;
for (var x = 0; x < sizeX; x++)
for (var y = 0; y < sizeY; y++)
{
    var location = new Location(x, y);
    if (map.At(location) == 'O')
        sum += 100 * y + x;
}

Console.WriteLine(sum);

enum Direction
{
    North,
    West,
    South,
    East,
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

    public static Direction Opposite(this Direction direction) => direction switch
    {
        Direction.North => Direction.South,
        Direction.East => Direction.West,
        Direction.South => Direction.North,
        Direction.West => Direction.East,

        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };
}
