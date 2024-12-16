var lines = File.ReadAllLines("input.txt");

var sizeX = lines[0].Length;
var sizeY = lines.Length;

var map = new char[sizeX, sizeY];
var zoneMap = new int[sizeX, sizeY];
var sidesMap = new int[sizeX, sizeY];

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
var sides = new Dictionary<int, int>();

for (var i = 1; i <= zoneIdx; i++)
{
    areas[i] = 0;
    perimeters[i] = 0;
    sides[i] = 0;
}

for (var x = 0; x < sizeX; x++)
for (var y = 0; y < sizeY; y++)
{
    var currentLocation = new Location(x, y);
    var currentZone = zoneMap.At(currentLocation);

    areas[currentZone]++;
    perimeters[currentZone] += 4 - AjdacentTo(currentLocation, sizeX, sizeY)
        .Where(loc => zoneMap.At(loc) == currentZone)
        .Count(loc => zoneMap.At(loc) == zoneMap.At(currentLocation));

    var currentSides = 4 - AjdacentTo(currentLocation, sizeX, sizeY)
        .Where(loc => zoneMap.At(loc) == currentZone)
        .Count(loc => zoneMap.At(loc) == zoneMap.At(currentLocation));

    // from: https://github.com/seapagan/aoc-2024/blob/1c219172f9ca5ef834d19474684636230f353e96/12/main.py#L61
    var northLocation = currentLocation.Move(Direction.North);
    var southLocation = currentLocation.Move(Direction.South);
    var westLocation = currentLocation.Move(Direction.West);
    var eastLocation = currentLocation.Move(Direction.East);

    //check west location
    if (!westLocation.IsOob(sizeX, sizeY) && zoneMap.At(westLocation) == currentZone)
    {
        var westNorth = westLocation.Move(Direction.North);

        if (!westNorth.IsOob(sizeX, sizeY) && zoneMap.At(westNorth) != currentZone
                                           && zoneMap.At(northLocation) != currentZone)
        {
            currentSides--;
        }
        if (westNorth.IsOob(sizeX, sizeY) && northLocation.IsOob(sizeX, sizeY))
        {
            currentSides--;
        }

        var westSouth = westLocation.Move(Direction.South);

        if (!westSouth.IsOob(sizeX, sizeY) && zoneMap.At(westSouth) != currentZone
                                           && zoneMap.At(southLocation) != currentZone)
        {
            currentSides--;
        }
        if (westSouth.IsOob(sizeX, sizeY) && southLocation.IsOob(sizeX, sizeY))
        {
            currentSides--;
        }
    }

    //check north location
    if (!northLocation.IsOob(sizeX, sizeY) && zoneMap.At(northLocation) == currentZone)
    {
        var northWest = northLocation.Move(Direction.West);

        if (!northWest.IsOob(sizeX, sizeY) && zoneMap.At(northWest) != currentZone
                                           && zoneMap.At(westLocation) != currentZone)
        {
            currentSides--;
        }
        if (northWest.IsOob(sizeX, sizeY) && westLocation.IsOob(sizeX, sizeY))
        {
            currentSides--;
        }

        var northEast = northLocation.Move(Direction.East);

        if (!northEast.IsOob(sizeX, sizeY) && zoneMap.At(northEast) != currentZone
                                           && zoneMap.At(eastLocation) != currentZone)
        {
            currentSides--;
        }
        if (northEast.IsOob(sizeX, sizeY) && eastLocation.IsOob(sizeX, sizeY))
        {
            currentSides--;
        }
    }

    sidesMap.At(currentLocation) = currentSides;
    sides[currentZone] += currentSides;
}

var sum = 0;
for (var i = 1; i <= zoneIdx; i++)
{
    sum += areas[i] * sides[i];
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