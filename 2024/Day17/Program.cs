using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");

var a = ParseNumbersInLine(lines[0])[0];
var b = ParseNumbersInLine(lines[1])[0];
var c = ParseNumbersInLine(lines[2])[0];

var program = ParseNumbersInLine(lines[4]).ToList();
var ip = 0;

List<int> output = [];
while (ip >= 0 && ip < program.Count - 1)
{
    var opcode = program[ip];
    var operand = program[ip + 1];

    switch (opcode)
    {
        case 0: // adv
            a /= 1 << ComboOperand(operand);
            break;
        case 1: // bxl
            b ^= operand;
            break;
        case 2: // bst
            b = ComboOperand(operand) & 7;
            break;
        case 3: // jnz
            if (a != 0)
            {
                ip = operand;
                continue;
            }
            break;
        case 4: // bxc
            b ^= c;
            break;
        case 5: // out
            output.Add(ComboOperand(operand) & 7);
            break;
        case 6: // bdv
            b = a / (1 << ComboOperand(operand));
            break;
        case 7: // cdv
            c = a / (1 << ComboOperand(operand));
            break;
    }

    ip += 2;
}

Console.WriteLine(string.Join(',', output));

return;

int ComboOperand(int operand)
{
    return operand switch
    {
        >= 0 and <= 3 => operand,
        4 => a,
        5 => b,
        6 => c,
        _ => throw new Exception("combo operand failed")
    };
}

static class Exts
{
    public static void Deconstruct(this int[] program, out int opcode, out int operand)
    {
        opcode = program[0];
        operand = program[1];
    }
}

partial class Program
{
    [GeneratedRegex("[0-9]+")]
    private static partial Regex NumberRegex();

    private static List<int> ParseNumbersInLine(string line) =>
        NumberRegex().Matches(line).Select(match => match.Value).Select(int.Parse).ToList();
}
