// https://adventofcode.com/2022/day/19

using System.Text.RegularExpressions;

var input = File.ReadAllLines("Day19.txt");

PartOne(input);

void PartOne(string[] input)
{
    var blueprints = ParseBlueprints(input);

    
}

int GetMaxGeodes(int timeleft, Blueprint blueprint)
{
    var seenStates = new HashSet<State>();
    var queue = new Queue<State>();

    queue.Enqueue(
        new State(
            timeleft,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
        ));

    var currentBest = 0;

    while (queue.TryDequeue(out var state))
    {
        currentBest = currentBest > state.Geode ? currentBest : state.Geode;

        if (state.Timeleft == 0) continue;

        queue.Enqueue(state);
    }
}

List<Blueprint> ParseBlueprints(string[] input)
{
    var blueprints = new List<Blueprint>();

    foreach (var line in input)
    {
        var nums = Regex.Matches(line, @"\d+").Select(x => int.Parse(x.Value)).ToArray();

        blueprints.Add(new Blueprint(
            nums[0],
            new Robot(Unit.Ore, new List<Material> { new Material(Unit.Ore, nums[1]) }),
            new Robot(Unit.Clay, new List<Material> { new Material(Unit.Ore, nums[2]) }),
            new Robot(Unit.Obsidian, new List<Material> { new Material(Unit.Ore, nums[3]), new Material(Unit.Clay, nums[4]) }),
            new Robot(Unit.Geode, new List<Material> { new Material(Unit.Ore, nums[5]), new Material(Unit.Obsidian, nums[6]) })
            )
        );
    }
    return blueprints;
}

record State(
    int Timeleft,
    int Ore,
    int Clay,
    int Obsidian,
    int Geode,
    int OreRobots,
    int ClayRobots,
    int ObsidianRobots,
    int GeodeRobots)
{
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

record Robot(Unit Production, List<Material> Cost);

record Material(Unit Unit, int Quantity);

enum Unit
{
    Ore,
    Clay,
    Obsidian,
    Geode
}