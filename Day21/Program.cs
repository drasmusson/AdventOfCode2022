// https://adventofcode.com/2022/day/21

var input = File.ReadAllLines("Day21.txt");

PartOne(input);
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
    monkeys.First(x => x.Name == "humn").Number = null;
    var root = monkeys.First(x => x.Name == "root");

    GetAndSetNumber(monkeys, root);

    var path = GetHumnPath(monkeys, root, new List<Operation>());
    var startValue = path.First().LeftValue == null ? path.First().RightValue : path.First().LeftValue;
    var answer = EvaluateFormula(path.Skip(1).ToList(), startValue.Value);
    Console.WriteLine($"Part two: {answer}");
}

long EvaluateFormula(List<Operation> values, long startValue)
{
    var humn = startValue;

    foreach (var operation in values)
    {
        if (operation.Operator == "humn") return humn;
        humn = operation.CalculateInverse(humn);
    }
    return humn;
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

    switch (monkey.Formula.Operator)
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
        return null;

    switch (monkey.Formula.Operator)
    {
        case "+":
            number = GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyOne)) + GetAndSetNumber(monkeys, monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo));
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

List<Operation> GetHumnPath(List<Monkey> monkeys, Monkey monkey, List<Operation> formula)
{
    if (monkey.Name == "humn")
    {
        formula.Add(new Operation(null, "humn", null));
        return formula;
    }

    var monkey1 = monkeys.First(x => x.Name == monkey.Formula.MonkeyOne);
    var monkey2 = monkeys.First(x => x.Name == monkey.Formula.MonkeyTwo);

    if (monkey1.Number == null)
    {
        formula.Add(new Operation(null, monkey.Formula.Operator, monkey2.Number));
        GetHumnPath(monkeys, monkey1, formula);
    }
    else
    {
        formula.Add(new Operation(monkey1.Number, monkey.Formula.Operator, null));
        GetHumnPath(monkeys, monkey2, formula);
    }

    return formula;
}

class Operation
{
    public long? LeftValue { get; }
    public string Operator { get; }
    public long? RightValue { get; }

    public Operation(long? leftValue, string @operator, long? rightValue)
    {
        LeftValue = leftValue;
        Operator = @operator;
        RightValue = rightValue;
    }

    public long CalculateInverse(long a)
    {
        var b = LeftValue is null ? RightValue.Value : LeftValue.Value;

        if (LeftValue != null) return CalculateRightInverse(a, b);
        return CalculateLeftInverse(a, b);
    }

    private long CalculateLeftInverse(long a, long right)
    {
        switch (Operator)
        {
            case "+":
                return a - right;
            case "-":
                return a + right;
            case "*":
                return a / right;
            case "/":
                return a * right;
            default:
                return a;
        }
    }

    private long CalculateRightInverse(long a, long left)
    {
        switch (Operator)
        {
            case "+":
                return a - left;
            case "-":
                return left - a;
            case "*":
                return a / left;
            case "/":
                return left * a;
            default:
                return a;
        }
    }
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
    public string Operator { get; }
    public string MonkeyOne { get; }
    public string MonkeyTwo { get; }

    public Formula(string modifier, string monkeyOne, string monkeyTwo)
    {
        Operator = modifier;
        MonkeyOne = monkeyOne;
        MonkeyTwo = monkeyTwo;
    }
}