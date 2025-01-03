﻿List<Direction> directions =
[
    Direction.NorthEast, Direction.SouthEast, Direction.SouthWest, Direction.NorthWest
];
List<List<char>> validDiagonals =
[
    ['M', 'M', 'S', 'S'],
    ['M', 'S', 'S', 'M'],
    ['S', 'S', 'M', 'M'],
    ['S', 'M', 'M', 'S']
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
for (var i = 1; i < sizeX - 1; i++)
{
    for (var j = 1; j < sizeY - 1; j++)
    {
        var currentLocation = new Location(i, j);

        if (map.At(currentLocation) == 'A')
        {
            var characters = directions
                .Select(direction => currentLocation.Move(direction))
                .Select(location => map.At(location))
                .ToList();

            if (validDiagonals.Any(diagonal => diagonal.SequenceEqual(characters)))
                sum++;
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