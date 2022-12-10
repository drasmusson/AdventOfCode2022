// https://adventofcode.com/2022/day/10

using System.Text;

var input = File.ReadAllLines("Day10.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var instructions = ParseToInstructions(input).ToList();
    var targetCycles = new List<int>
    {
        20, 60, 100, 140, 180, 220
    };

    var signalStrengths = new List<int>();
    foreach (var targetCycle in targetCycles)
    {
        var X = RunProgram(instructions, targetCycle);
        signalStrengths.Add(targetCycle * X);
    }
    var answer = signalStrengths.Sum();
    Console.WriteLine($"Part one: {answer}");
}

void PartTwo(string[] input)
{
    var instructions = ParseToInstructions(input).ToList();
    
    var printer = new Printer();
    printer.PrintToScreen(instructions);

    Console.WriteLine("Part two:");
    foreach (var line in printer.Screen)
    {
        Console.WriteLine(line);
    }
}

int RunProgram(List<Instruction> instructions, int targetCycles)
{
    var cycles = 1;
    var X = 1;

    foreach (var instruction in instructions)
    {
        cycles += instruction.Cycle;

        if (cycles <= targetCycles)
            X += instruction.V;

        if (cycles > targetCycles)
            break;
    }
    return X;
}

IEnumerable<Instruction> ParseToInstructions(string[] input)
{
    foreach (var line in input)
        yield return new Instruction(line);
}

record Sprite(int Value)
{
    public bool Equals(int value) => Value == value || Value == value + 1 || Value == value - 1;
    public static Sprite operator +(Sprite sprite, int value) => new Sprite(sprite.Value + value);
}

class Printer
{
    public List<string> Screen { get; private set; }

    public Printer()
    {
        Sprite = new Sprite(1);
        PrntLocation= 0;
        Screen = new List<string>();
        Cycle = 0;
        Line = new StringBuilder();
    }

    private Sprite Sprite { get; set; }
    private int PrntLocation { get; set; }
    private int Cycle { get; set; }
    private StringBuilder Line { get; set; }

    internal void PrintToScreen(List<Instruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            PrintLineToScreen(instruction);
            if (Cycle == 240)
                break;
            Sprite += instruction.V;
        }
    }

    private void PrintLineToScreen(Instruction instruction)
    {
        for (int i = 0; i < instruction.Cycle; i++)
        {
            if (Sprite.Equals(PrntLocation))
                Line.Append("#");
            else
                Line.Append(".");

            PrntLocation++;
            Cycle++;
            if (Cycle % 40 == 0)
                ResetForNewRow();
        }
    }

    private void ResetForNewRow()
    {
        PrntLocation = 0;
        Screen.Add(Line.ToString());
        Line = new StringBuilder();
    }
}

class Instruction
{
    public int Cycle { get; } = 1;
    public int V { get; } = 0;

    public Instruction(string input)
    {
        var type = input.Substring(0, 4);

        if (type == "addx")
        {
            V = int.Parse(input.Split(' ')[1]);
            Cycle = 2;
        }
    }
}