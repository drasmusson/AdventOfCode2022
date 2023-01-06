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

    GetAndSetNumber(monkeys, root);

    var path = GetPathToHumn(monkeys, root, "");

    Console.WriteLine($"Part one: {path}");
}

void SetNumbersForAllButHumnAndParents(List<Monkey> monkeys, Monkey root)
{
    var humnFree = monkeys.Where(x => x.Name == root.Formula.MonkeyOne || x.Name == root.Formula.MonkeyTwo).First(m => !DoesMonkeyReachHumn(monkeys, m));

    var rootNumber = GetAndSetNumber(monkeys, humnFree);

    var b = monkeys.First(x => x.Formula != null && (x.Formula.MonkeyOne == "humn" || x.Formula.MonkeyTwo == "humn"));
    var humnBrother = b.Formula.MonkeyOne == "humn" ? b.Formula.MonkeyTwo : b.Formula.MonkeyOne;

    var p2 = GetAndSetNumber(monkeys, monkeys.First(x => x.Name == humnBrother));
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

long? GetAndSetNumber(List<Monkey> monkeys, Monkey monkey)
{
    if (monkey.Name == "humn") return null;
    if (monkey.Number.HasValue) return monkey.Number.Value;

    long? number = 0;

    var leftMonkeyNumber = GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne));
    var rightMonkeyNumber = GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo));

    if (leftMonkeyNumber is null || rightMonkeyNumber is null)
    {
        return null;
    }
    switch (monkey.Formula.Modifier)
    {
        case "+":
            number = GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne)) + GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo)).Value;
            break;
        case "-":
            number = GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne)) - GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo));
            break;
        case "/":
            number = GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne)) / GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo));
            break;
        case "*":
            number = GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne)) * GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo));
            break;
        default:
            break;
    }
    monkey.Number = number;
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

bool DoesMonkeyReachHumn(List<Monkey> monkeys, Monkey monkey)
{
    if (monkey.Name == "humn") return true;
    if (monkey.Number.HasValue) return false;

    var p1 = DoesMonkeyReachHumn(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne));
    var p2 = DoesMonkeyReachHumn(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo));

    return p1 || p2;
}

class Monkey
{
    public string Name { get; }
    public long? Number { get; set; }
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