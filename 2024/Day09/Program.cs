// See https://aka.ms/new-console-template for more information

var line = File.ReadAllText("input.txt").Trim().Select(c => int.Parse(c.ToString())).ToList();

var zippedLine = line.Chunk(2);

var drive = new List<int>();
var id = 0;
foreach (var chunk in zippedLine)
{
    for (var i = 0; i < chunk[0]; i++)
    {
        drive.Add(id);
    }

    if (chunk.Length == 2)
    {
        for (var i = 0; i < chunk[1]; i++)
        {
            drive.Add(-1);
        }
    }

    id++;
}

var rightIdx = drive.Count - 1;
for (var leftIdx = 0; rightIdx > leftIdx; leftIdx++)
{
    if (drive[leftIdx] == -1)
    {
        drive[leftIdx] = drive[rightIdx--];
        while (drive[rightIdx] == -1)
        {
            rightIdx--;
        }
    }
}

drive = drive[..(rightIdx+1)];

var sum = 0L;
for (var i = 0; i < drive.Count; i++)
{
    if (drive[i] != -1)
    {
        sum += drive[i] * i;
    }
}

Console.WriteLine(sum);
