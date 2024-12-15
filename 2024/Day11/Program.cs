﻿using System.Text.RegularExpressions;

var line = File.ReadAllText("input.txt").Trim();

var numbers = ParseNumbersInLine(line);

List<long> newNumbers = [];

for (var i = 0; i < 25; i++)
{
    // Console.WriteLine($"Iteration {i}");
    foreach (var n in numbers)
    {
        if (n == 0)
        {
            // Console.WriteLine("  0 to 1");
            newNumbers.Add(1);
        }
        else
        {
            var length = n.ToString().Length;
            if (length % 2 == 0)
            {
                // Console.Write($"  {n} to be split to ");

                var limit = Math.Pow(10, length / 2);
                newNumbers.Add((int)(n / limit));
                newNumbers.Add((int)(n % limit));
                // Console.WriteLine($"{newNumbers[^2]} and {newNumbers[^1]}");
            }
            else
            {
                newNumbers.Add(n * 2024);
                // Console.WriteLine($"  {n} to {newNumbers[^1]}");
            }
        }
    }

    numbers = [..newNumbers];
    newNumbers.Clear();
}

Console.WriteLine(numbers.Count);


partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();

    private static List<long> ParseNumbersInLine(string line) =>
        NumberRegex().Matches(line).Select(match => match.Value).Select(long.Parse).ToList();
}