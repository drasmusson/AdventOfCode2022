// https://adventofcode.com/2022/day/4

var input = File.ReadAllLines("Day04.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var result = 0;

    foreach (var line in input)
    {
        var pairs = line.Split(',');
        var range1 = ConvertToRange(pairs[0]);
        var range2 = ConvertToRange(pairs[1]);

        var intersection = range1.Intersect(range2).Count();

        if (intersection == range1.Count() || intersection == range2.Count())
            result++;
    }
    Console.WriteLine($"Part one: {result}");
}

void PartTwo(string[] input)
{
    var result = 0;

    foreach (var line in input)
    {
        var pairs = line.Split(',');
        var range1 = ConvertToRange(pairs[0]);
        var range2 = ConvertToRange(pairs[1]);

        var intersection = range1.Intersect(range2).Count();

        if (intersection > 0)
            result++;
    }
    Console.WriteLine($"Part two: {result}");
}

static IEnumerable<int> ConvertToRange(string intervalString)
{
    var interval = intervalString.Split('-').Select(x => int.Parse(x)).ToArray();
    return Enumerable.Range(interval[0], interval[1] + 1 - interval[0]);
}