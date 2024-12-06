Dictionary<Direction, Direction> directionMap = new Dictionary<Direction, Direction>
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

var position = new Location(0, 0);
var direction = Direction.North;
for (var i = 0; i < sizeY; i++)
for (var j = 0; j < sizeX; j++)
{
    map[j, i] = lines[i][j];
    if (map[j, i] == '^')
    {
        position = new Location(j, i);
        map[j, i] = 'X';
    }
}

while (!position.IsOob(sizeX, sizeY))
{
    var checkPosition = position.Move(direction);

    if (!checkPosition.IsOob(sizeX, sizeY) && map.At(checkPosition) == '#')
    {
        direction = directionMap[direction];
        continue;
    }

    position = checkPosition;
    if (!position.IsOob(sizeX, sizeY))
    {
        map.At(position) = 'X';
    }
}

var sum = 0;
for (var i = 0; i < sizeY; i++)
for (var j = 0; j < sizeX; j++)
{
    if (map[j, i] == 'X')
    {
        sum++;
    }
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