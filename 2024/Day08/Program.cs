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
        .SelectMany(tuple => tuple.a1.GetAntiNodesFor(tuple.a2, sizeX, sizeY));

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
    public IEnumerable<Location> GetAntiNodesFor(Antenna antenna, int sizeX, int sizeY)
    {
        var frequency = new Location(antenna.Location.X - Location.X, antenna.Location.Y - Location.Y);

        var currentFrequency = Location;
        while (!currentFrequency.IsOob(sizeX, sizeY))
        {
            yield return currentFrequency;
            currentFrequency = new Location(currentFrequency.X - frequency.X, currentFrequency.Y - frequency.Y);
        }

        currentFrequency = new Location(Location.X + frequency.X, Location.Y + frequency.Y);
        while (!currentFrequency.IsOob(sizeX, sizeY))
        {
            yield return currentFrequency;
            currentFrequency = new Location(currentFrequency.X + frequency.X, currentFrequency.Y + frequency.Y);
        }
    }
}