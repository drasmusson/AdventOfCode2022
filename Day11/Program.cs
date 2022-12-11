// https://adventofcode.com/2022/day/11

using System.Data;

var input = File.ReadAllLines("Day11.txt");

//PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    List<Monkey> monkeys = ParseMonkeys(input);

    RunRounds(20, monkeys);

    var mostActive = monkeys.OrderByDescending(x => x.MbLevel).Take(2).Select(y => y.MbLevel).ToList();
    var totalMb = mostActive.Aggregate((x, y) => x * y);
    Console.WriteLine($"Part one: {totalMb}");
}

void PartTwo(string[] input)
{
    List<Monkey> monkeys = ParseMonkeys(input);

    RunRounds2(10000, monkeys);

    var mostActive = monkeys.OrderByDescending(x => x.MbLevel).Take(2).Select(y => y.MbLevel).ToList();
    var totalMb = mostActive.Aggregate((x, y) => x * y);
    Console.WriteLine($"Part one: {totalMb}");
}

void RunRounds(int rounds, List<Monkey> monkeys)
{
    for (int i = 0; i < rounds; i++)
        Round(monkeys);
}

void Round(List<Monkey> monkeys)
{
    foreach (var monkey in monkeys)
    {
        if (monkey.Items.Any())
            monkey.TakeTurn(monkeys);

    }
}

void RunRounds2(int rounds, List<Monkey> monkeys)
{
    for (int i = 0; i < rounds; i++)
        Round2(monkeys);
}

void Round2(List<Monkey> monkeys)
{
    foreach (var monkey in monkeys)
    {
        if (monkey.Items.Any())
            monkey.TakeTurn2(monkeys);

    }
}

List<Monkey> ParseMonkeys(string[] input)
{
    List<Monkey> monkeys = new List<Monkey>();

    for (int i = 0; i < input.Length; i+=7)
    {
        var segment = input.Skip(i).Take(6).ToList();

        monkeys.Add(new Monkey(segment));
    }
    return monkeys;
}

class Monkey
{
    public int Nr { get; }
    public long MbLevel { get; private set; }
    public List<long> Items { get; set; }
    private string Operation { get; }
    private Func<long, long> Test { get; }

    public Monkey(List<string> input)
    {
        Nr = int.Parse(input[0].Split(' ')[1][0].ToString());
        Items = input[1].Split("Starting items: ")[1].Split(", ").Select(x => long.Parse(x)).ToList();
        Operation = input[2].Split("Operation: new = ")[1];

        var divisor = int.Parse(input[3].Split("Test: divisible by ")[1]);
        var tMonkey = int.Parse(input[4].Split("If true: throw to monkey ")[1]);
        var fMonkey = int.Parse(input[5].Split("If false: throw to monkey ")[1]);
        Test = (x) =>
        {
            return x % divisor == 0 ? tMonkey : fMonkey;
        };
    }

    public void TakeTurn(List<Monkey> monkeys)
    {
        foreach (var item in Items)
        {
            MbLevel++;

            var newWorryLevel = InspectItem(item);
            newWorryLevel = (long)Math.Floor(newWorryLevel / 3.0);

            var monkey = Test(newWorryLevel);

            monkeys.First(x => x.Nr == monkey).Items.Add(newWorryLevel);
        }
        Items.Clear();
    }

    public void TakeTurn2(List<Monkey> monkeys)
    {
        foreach (var item in Items)
        {
            MbLevel++;

            var newWorryLevel = InspectItem(item);

            var monkey = Test(newWorryLevel);

            monkeys.First(x => x.Nr == monkey).Items.Add(newWorryLevel);
        }
        Items.Clear();
    }

    private long InspectItem(long item)
    {
        var operation = Operation.Replace("old", item.ToString());

        var opParts = operation.Split(' ');
        var x = long.Parse(opParts[0]);
        var y = long.Parse(opParts[2]);

        switch (opParts[1])
        {
            case "+":
                return x + y;
            case "-":
                return x - y;
            case "*":
                return x * y;
            case "/":
                return x / y;
            default:
                break;
        }
        return 0;
    }
}