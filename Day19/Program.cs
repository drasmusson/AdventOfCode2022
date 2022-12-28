// https://adventofcode.com/2022/day/19

using System.Text.RegularExpressions;

var input = File.ReadAllLines("Day19.txt");

PartOne(input);

void PartOne(string[] input)
{
    var blueprints = ParseBlueprints(input);
}

List<Blueprint> ParseBlueprints(string[] input)
{
    var blueprints = new List<Blueprint>();

    foreach (var line in input)
    {
        var nums = Regex.Matches(line, @"\d+").Select(x => int.Parse(x.Value)).ToArray();

        blueprints.Add(new Blueprint(
            nums[0],
            new Robot(Unit.Ore, new List<(Unit, int)> { (Unit.Ore, nums[1]) }),
            new Robot(Unit.Clay, new List<(Unit, int)> { (Unit.Ore, nums[2]) }),
            new Robot(Unit.Obsidian, new List<(Unit, int)> { (Unit.Ore, nums[3]), (Unit.Clay, nums[4]) }),
            new Robot(Unit.Geode, new List<(Unit, int)> { (Unit.Ore, nums[5]), (Unit.Obsidian, nums[6]) })
            )
        );
    }
    return blueprints;
}

class Blueprint
{
    public int Id { get; private set; }
    public Robot[] Robots { get; private set; }

    public Blueprint(int id, params Robot[] robots)
    {
        Id = id;
        Robots = robots;
    }
}

record Robot(
    Unit Production,
    List<(Unit, int)> Cost
);

enum Unit
{
    Ore,
    Clay,
    Obsidian,
    Geode
}