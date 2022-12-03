// https://adventofcode.com/2022/day/3

var rucksacks = File.ReadAllLines("Day03.txt");

PartOne(rucksacks);
PartTwo(rucksacks);

void PartOne(string[] rucksacks)
{
    var compartments = rucksacks.Select(x => (x.Substring(0, x.Length / 2), x.Substring(x.Length / 2, x.Length / 2)));

    var reuccuring = compartments.SelectMany(x => x.Item1.Intersect(x.Item2));

    var prioSum = reuccuring.Select(x => GetPrioprityValue(x)).Sum();

    Console.WriteLine("Part one: " + prioSum);
}

void PartTwo(string[] rucksacks)
{
    var groups = from i in Enumerable.Range(0, rucksacks.Length) group rucksacks[i] by i / 3;
    
    var badges = new List<char>();

    foreach (var group in groups)
    {
        var ar = group.ToArray();
        var firstMatch = ar[0].Intersect(ar[1]);
        var badge = firstMatch.Intersect(ar[2]);
        badges.Add(badge.First());
    }
    var prioSum = badges.Select(x => GetPrioprityValue(x)).Sum();

    Console.WriteLine("Part two: " + prioSum);
}

int GetPrioprityValue(char v)
{
    var prioList = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    return prioList.IndexOf(v) + 1;
}
