// https://adventofcode.com/2022/day/21

var input = File.ReadAllLines("Day21.txt");

//PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var monkeys = ParseMonkeys(input);

    var root = monkeys.First(x => x.Name == "root");

    var rootNumber = GetNumber(monkeys, root);

    Console.WriteLine($"Part one: {rootNumber}");
}

void PartTwo(string[] input)
{
    var monkeys = ParseMonkeys(input);

    var root = monkeys.First(x => x.Name == "root");

    var path = GetPathToHumn(monkeys, root, "");

    Console.WriteLine($"Part one: {path}");
}

List<Monkey> ParseMonkeys(string[] input)
{
    var monkeys = new List<Monkey>();

    foreach (var line in input)
    {
        var nameSplit = line.Split(": ");

        if (long.TryParse(nameSplit[1], out var number))
        {
            monkeys.Add(new Monkey(nameSplit[0], number));
            continue;
        }
        var formula = new Formula(nameSplit[1][5].ToString(), nameSplit[1].Substring(0, 4), nameSplit[1].Substring(nameSplit[1].Length - 4));
        monkeys.Add(new Monkey(nameSplit[0], null, formula));
    }

    return monkeys;
}

long GetNumber(List<Monkey> monkeys, Monkey monkey)
{
    if (monkey.Number.HasValue) return monkey.Number.Value;

    var number = 0L;

    switch (monkey.Formula.Modifier)
    {
        case "+":
            number = GetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne)) + GetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo));
            break;
        case "-":
            number = GetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne)) - GetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo));
            break;
        case "/":
            number = GetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne)) / GetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo));
            break;
        case "*":
            number = GetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne)) * GetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo));
            break;
        default:
            break;
    }
    return number;
}

string GetPathToHumn(List<Monkey> monkeys, Monkey monkey, string path)
{
    if (monkey.Name == "humn") return path += ", " + monkey.Name;
    if (monkey.Number.HasValue) return "";

    path += ", " + monkey.Name;
    var p1 = GetPathToHumn(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne), path);
    var p2 = GetPathToHumn(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo), path);

    if (p1 != "") return p1;

    return p2;
}

class Monkey
{
    public string Name { get; }
    public long? Number { get; }
    public Formula? Formula { get; }
    public string Path { get; set; }

    public Monkey(string name, long? number, Formula? formula = null)
    {
        Name = name;
        Number = number;
        Formula = formula;
    }
}

class Formula
{
    public string Modifier { get; }
    public string MonkeyOne { get; }
    public string MonkeyTwo { get; }

    public Formula(string modifier, string monkeyOne, string monkeyTwo)
    {
        Modifier = modifier;
        MonkeyOne = monkeyOne;
        MonkeyTwo = monkeyTwo;
    }
}