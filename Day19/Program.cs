// https://adventofcode.com/2022/day/19

using System.Text.RegularExpressions;

var input = File.ReadAllLines("Day19.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var blueprints = ParseBlueprints(input);
    foreach (var blueprint in blueprints)
    {
        blueprint.Max = GetMaxGeodes(24, blueprint);
    }
    var answer = blueprints.Sum(b => b.Max * b.Id);

    Console.WriteLine($"Part one: {answer}");
}

void PartTwo(string[] input)
{
    var blueprints = input.Length == 2 ? ParseBlueprints(input) : ParseBlueprints(input[..3]);

    foreach (var blueprint in blueprints)
    {
        blueprint.Max = GetMaxGeodes(32, blueprint);
    }
    var answer = blueprints.Select(x => x.Max).Aggregate((a, b) => a * b);

    Console.WriteLine($"Part two: {answer}");
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
            1,
            0,
            0,
            0
        ));

    var currentBest = 0;

    while (queue.TryDequeue(out var state))
    {
        currentBest = currentBest > state.Geode ? currentBest : state.Geode;

        if (state.Timeleft == 0) continue;
        if (BestPossibleGeodeScore(state) < currentBest) continue;
        if (currentBest - state.Geode >= 3) continue;

        if (state.Ore >= blueprint.GeodeRobotCost.Ore && state.Obsidian >= blueprint.GeodeRobotCost.Obsidian)
        {
            var newState = state.Increase().AddRobot(blueprint, Robot.Geode);
            if (!seenStates.Contains(newState))
            {
                queue.Enqueue(newState);
            }
            seenStates.Add(newState);
        }
        else
        {
            if (state.Ore >= blueprint.ObsidianRobotCost.Ore && state.Clay >= blueprint.ObsidianRobotCost.Clay && state.ObsidianRobots < blueprint.GeodeRobotCost.Obsidian)
            {
                var newState = state.Increase().AddRobot(blueprint, Robot.Obsidian);
                if (!seenStates.Contains(newState))
                {
                    queue.Enqueue(newState);
                }
                seenStates.Add(newState);
            }
            else
            {
                if (state.Ore >= blueprint.OreRobotCost && state.OreRobots < blueprint.MostOreExpensive())
                {
                    var newState = state.Increase().AddRobot(blueprint, Robot.Ore);
                    if (!seenStates.Contains(newState))
                    {
                        queue.Enqueue(newState);
                    }
                    seenStates.Add(newState);
                }

                if (state.Ore >= blueprint.ClayRobotCost && state.ClayRobots < blueprint.ObsidianRobotCost.Clay)
                {
                    var newState = state.Increase().AddRobot(blueprint, Robot.Clay);
                    if (!seenStates.Contains(newState))
                    {
                        queue.Enqueue(newState);
                    }
                    seenStates.Add(newState);
                }

                var newS = state.Increase();
                queue.Enqueue(newS);
            }
        }
    }
    return currentBest;
}

int BestPossibleGeodeScore(State state)
{
    return state.Geode + state.GeodeRobots * state.Timeleft + (state.Timeleft * (state.Timeleft - 1)) / 2;
}

List<Blueprint> ParseBlueprints(string[] input)
{
    var blueprints = new List<Blueprint>();

    foreach (var line in input)
    {
        var nums = Regex.Matches(line, @"\d+").Select(x => int.Parse(x.Value)).ToArray();

        blueprints.Add(new Blueprint(
            nums[0],
            nums[1],
            nums[2],
            (nums[3], nums[4]),
            (nums[5], nums[6])
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
    public State AddRobot(Blueprint blueprint, Robot robot) => robot switch
    {
        Robot.Ore => this with { Ore = Ore - blueprint.OreRobotCost, OreRobots = OreRobots + 1 },
        Robot.Clay => this with { Ore = Ore - blueprint.ClayRobotCost, ClayRobots = ClayRobots + 1 },
        Robot.Obsidian => this with { Ore = Ore - blueprint.ObsidianRobotCost.Ore, Clay = Clay - blueprint.ObsidianRobotCost.Clay, ObsidianRobots = ObsidianRobots + 1 },
        Robot.Geode => this with { Ore = Ore - blueprint.GeodeRobotCost.Ore, Obsidian = Obsidian - blueprint.GeodeRobotCost.Obsidian, GeodeRobots = GeodeRobots + 1 },
        _ => throw new NotImplementedException()
    };


    public State Increase()
    {
        return this with
        {
            Timeleft = Timeleft - 1,
            Ore = Ore + OreRobots,
            Clay = Clay + ClayRobots,
            Obsidian = Obsidian + ObsidianRobots,
            Geode = Geode + GeodeRobots
        };
    }
}

class Blueprint
{
    public int Max { get; set; }
    public int Id { get; }
    public int OreRobotCost { get; }
    public int ClayRobotCost { get; }
    public (int Ore, int Clay) ObsidianRobotCost { get; }
    public (int Ore, int Obsidian) GeodeRobotCost { get; }

    public Blueprint(int id, int oreRobotCost, int clayRobotCost, (int Ore, int Clay) obsidianRobotCost, (int Ore, int Obsidian) geodeRobotCost)
    {
        Id = id;
        OreRobotCost = oreRobotCost;
        ClayRobotCost = clayRobotCost;
        ObsidianRobotCost = obsidianRobotCost;
        GeodeRobotCost = geodeRobotCost;
    }

    public int MostOreExpensive() 
    {
        var ints = new List<int> { OreRobotCost, ClayRobotCost, ObsidianRobotCost.Ore, GeodeRobotCost.Ore };

        return ints.Max();
    }
}

//record Robot(Unit Production, List<Material> Cost);

//record Material(Unit Unit, int Quantity);

enum Robot
{
    Ore,
    Clay,
    Obsidian,
    Geode
}