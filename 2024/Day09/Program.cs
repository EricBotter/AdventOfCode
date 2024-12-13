
using System.Collections;

var line = File.ReadAllText("input.txt").Trim().Select(c => int.Parse(c.ToString())).ToList();

var zippedLine = line.Chunk(2);

var drive = new List<DiskFile>();
var gaps = new List<Gap>();

var id = 0;
var position = 0;
foreach (var chunk in zippedLine)
{
    drive.Add(new DiskFile(id, position, chunk[0]));

    position += chunk[0];

    if (chunk.Length == 2)
    {
        gaps.Add(new Gap(position, chunk[1]));
        position += chunk[1];
    }

    id++;
}

for (var i = drive.Count - 1; i >= 0; i--)
{
    var file = drive[i];
    for (var j = 0; j < gaps.Count; j++)
    {
        var gap = gaps[j];
        if (gap.Length >= file.Length)
        {
            drive[i] = file with { Position = gap.Position };
            gaps[j] = new Gap(gap.Position + file.Length, gap.Length - file.Length);
            break;
        }

        if (gaps[j].Position >= file.Position)
        {
            break;
        }
    }
}

var sum = 0L;
foreach (var file in drive)
{
    for (var i = 0; i < file.Length; i++)
    {
        sum += file.Id * (file.Position + i);
    }
}



Console.WriteLine(sum);

record DiskFile(int Id, int Position, int Length);
record Gap(int Position, int Length);
