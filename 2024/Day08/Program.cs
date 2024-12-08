using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var sizeX = lines[0].Length;
var sizeY = lines.Length;

var map = new char[sizeX, sizeY];

List<Antenna> antennae = [];
for (var i = 0; i < sizeY; i++)
for (var j = 0; j < sizeX; j++)
{
    map[j, i] = lines[i][j];
    if (map[j, i] != '.')
    {
        antennae.Add(new Antenna(map[j, i], new Location(j, i)));
    }
}

HashSet<Location> validLocations = [];
foreach (var antennaGroup in antennae.GroupBy(antenna => antenna.Frequency))
{
    var newAntinodes = antennaGroup.SelectMany(a1 => antennaGroup.Where(a2 => a2 != a1).Select(a2 => (a1, a2)))
        .Select(tuple => tuple.a1.GetAntiNodesFor(tuple.a2))
        .SelectMany(tuple => new[] { tuple.Item1, tuple.Item2 })
        .Where(location => !location.IsOob(sizeX, sizeY));

    foreach (var antinode in newAntinodes)
    {
        validLocations.Add(antinode);
    }
}

Console.WriteLine(validLocations.Count);

record Location(int X, int Y)
{
    public bool IsOob(int maxX, int maxY) => X < 0 || Y < 0 || X >= maxX || Y >= maxY;
}

record Antenna(char Frequency, Location Location)
{
    public (Location, Location) GetAntiNodesFor(Antenna antenna)
    {
        return (
            new Location(Location.X - (antenna.Location.X - Location.X), Location.Y - (antenna.Location.Y - Location.Y)),
            new Location(antenna.Location.X - (Location.X - antenna.Location.X), antenna.Location.Y - (Location.Y - antenna.Location.Y))
        );
    }
}

public class MultiValueDictionary<Key, Value> : Dictionary<Key, List<Value>>
{
    public void Add(Key key, Value value)
    {
        List<Value> values;
        if (!this.TryGetValue(key, out values))
        {
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

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();

    private static List<long> ParseLineToNumberList(string line) =>
        NumberRegex().Matches(line).Select(match => long.Parse(match.Value)).ToList();
}