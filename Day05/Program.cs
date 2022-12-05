// https://adventofcode.com/2022/day/5

var input = File.ReadAllLines("Day05.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    Dictionary<int, List<string>> crates;
    List<Instruction> instructions;
    ParseCratesAndInstructions(input, out crates, out instructions);

    foreach (var instruction in instructions)
    {
        var liftedCrates = crates[instruction.Source].Take(instruction.Quantity).ToList();
        crates[instruction.Source].RemoveRange(0, instruction.Quantity);

        liftedCrates.Reverse();
        crates[instruction.Destination].InsertRange(0, liftedCrates);
    }

    var answer = string.Join("", Enumerable.Range(1, crates.Count()).Select(i => crates[i].First()));

    Console.WriteLine($"Part one: {answer}");
}

void PartTwo(string[] input)
{
    Dictionary<int, List<string>> crates;
    List<Instruction> instructions;
    ParseCratesAndInstructions(input, out crates, out instructions);

    foreach (var instruction in instructions)
    {
        var liftedCrates = crates[instruction.Source].Take(instruction.Quantity).ToList();
        crates[instruction.Source].RemoveRange(0, instruction.Quantity);

        crates[instruction.Destination].InsertRange(0, liftedCrates);
    }

    var answer = string.Join("", Enumerable.Range(1, crates.Count()).Select(i => crates[i].First()));

    Console.WriteLine($"Part two: {answer}");
}

void ParseCratesAndInstructions(string[] input, out Dictionary<int, List<string>> crates, out List<Instruction> instructions)
{
    crates = new Dictionary<int, List<string>>();
    instructions = new List<Instruction>();
    foreach (var line in input)
    {
        if (IsCrateInstructionSeparation(line))
        {
            continue;
        }

        if (IsInstructionInput(line))
        {
            instructions.Add(ParseInstruction(line));
            continue;
        }

        if (IsCrateInput(line))
        {
            AddCratesFromLine(crates, line);
            continue;
        }
    }
}

bool IsCrateInput(string line) => line.Contains('[');

bool IsInstructionInput(string line) => !string.IsNullOrWhiteSpace(line) && line[0] == 'm';

bool IsCrateInstructionSeparation(string line) => string.IsNullOrWhiteSpace(line) || int.TryParse(line.Replace(" ", ""), out _);

static void AddCratesFromLine(Dictionary<int, List<string>> crates, string line)
{
    var currentCol = 0;

    for (int i = 0; i < line.Length; i += 4)
    {
        currentCol++;

        var linePart = line.Substring(i, 3);

        if (!String.IsNullOrWhiteSpace(linePart))
        {
            var crateInput = linePart[1].ToString();

            if (!crates.ContainsKey(currentCol))
                crates.Add(currentCol, new List<string> { crateInput });
            else
                crates[currentCol].Add(crateInput);
        }
    }
}

Instruction ParseInstruction(string line)
{
    var split = line.Split(' ');
    return new Instruction
    {
        Quantity = int.Parse(split[1]),
        Source = int.Parse(split[3]),
        Destination = int.Parse(split[5]),
    };
}

public record Instruction
{
    public int Quantity { get; init; }
    public int Source { get; init; }
    public int Destination { get; init; }
}