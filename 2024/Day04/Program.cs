List<Direction> directions =
[
    Direction.North, Direction.East, Direction.South, Direction.West,
    Direction.NorthWest, Direction.NorthEast, Direction.SouthWest, Direction.SouthEast
];

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
{
    for (var j = 0; j < sizeY; j++)
    {
        var currentLocation = new Location(i, j);

        if (map.At(currentLocation) == 'X')
        {
            foreach (var direction in directions)
            {
                var indexLocation = currentLocation.Move(direction);
                if (indexLocation.IsOob(sizeX, sizeY) || map.At(indexLocation) != 'M')
                    continue;

                indexLocation = indexLocation.Move(direction);
                if (indexLocation.IsOob(sizeX, sizeY) || map.At(indexLocation) != 'A')
                    continue;

                indexLocation = indexLocation.Move(direction);
                if (indexLocation.IsOob(sizeX, sizeY) || map.At(indexLocation) != 'S')
                    continue;

                Console.WriteLine($"XMAS found: {currentLocation}, {direction}");
                sum++;
            }
        }
    }
}

Console.WriteLine(sum);

enum Direction
{
    North,
    West,
    South,
    East,
    NorthWest,
    NorthEast,
    SouthWest,
    SouthEast
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
            Direction.NorthWest => new Location(X - 1, Y - 1),
            Direction.NorthEast => new Location(X + 1, Y - 1),
            Direction.SouthWest => new Location(X - 1, Y + 1),
            Direction.SouthEast => new Location(X + 1, Y + 1),

            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    public bool IsOob(int maxX, int maxY) => X < 0 || Y < 0 || X >= maxX || Y >= maxY;
}

static class Exts
{
    public static ref T At<T>(this T[,] source, Location location) => ref source[location.X, location.Y];
}