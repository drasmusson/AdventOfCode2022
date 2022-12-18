// https://adventofcode.com/2022/day/16

var input = File.ReadAllLines("Day16.txt");

PartOne(input);
PartTwo(input);


void PartOne(string[] input)
{
    var valves = ParseValves(input);

    foreach (var valve in valves)
        valve.SetShortestPathToValves(valves);

    var answer = CalculateMaxPressure(30, valves.Where(x => x.ReleasePressure > 0).ToList(), valves.First(x => x.Name == "AA"));

    Console.WriteLine($"Part one: {answer}");
}

void PartTwo(string[] input)
{
    var valves = ParseValves(input);

    foreach (var valve in valves)
        valve.SetShortestPathToValves(valves);

    var startValve = valves.First(x => x.Name == "AA");
    var answer = CalculateMaxPressureForTwo(new[] { 26, 26 }, valves.Where(x => x.ReleasePressure > 0).ToList(), new[] { startValve, startValve });

    Console.WriteLine($"Part two: {answer}");
}

int CalculateMaxPressure(int timeLeft, List<Valve> valvesToUse, Valve start)
{
    var maxPressure = 0;

    foreach (var valve in valvesToUse)
    {
        // TimeLeft = time it takes to go to turned off valve + 1 minute for the time it takes to turn on a valve.
        var newTimeLeft = timeLeft - start.ShortestPathsToValves[valve.Name] - 1;

        if (newTimeLeft > 0)
        {
            var pressure = newTimeLeft * valve.ReleasePressure + CalculateMaxPressure(newTimeLeft, valvesToUse.Where(x => x.Name != valve.Name).ToList(), valve);
            if (pressure > maxPressure)
                maxPressure = pressure;
        }
    }
    return maxPressure;
}

int CalculateMaxPressureForTwo(int[] timesLeft, List<Valve> valvesToUse, Valve[] currentValves)
{
    var maxPressure = 0;
    var worker = timesLeft[0] > timesLeft[1] ? 0 : 1;

    var currentValve = currentValves[worker];

    foreach (var valve in valvesToUse)
    {
        // TimeLeft = time it takes to go to turned off valve + 1 minute for the time it takes to turn on a valve.
        var newTimeLeft = timesLeft[worker] - currentValve.ShortestPathsToValves[valve.Name] - 1;

        if (newTimeLeft > 0)
        {
            var times = new[] { newTimeLeft, timesLeft[1 - worker] };
            var valves = new[] { valve, currentValves[1 - worker] };
            var pressure = newTimeLeft * valve.ReleasePressure + CalculateMaxPressureForTwo(times, valvesToUse.Where(x => x.Name != valve.Name).ToList(), valves);
            if (pressure > maxPressure)
                maxPressure = pressure;
        }
    }
    return maxPressure;
}

Dictionary<string, int> GetShortestPathToTarget(List<Valve> valves, Valve startValve)
{
    var queue = new Queue<Valve>();

    queue.Enqueue(startValve);
    var visited = new HashSet<string>();
    var distances = new Dictionary<string, int>();
    distances[startValve.Name] = 0;

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();

        foreach (var neighbour in current.ConnectedValves)
        {
            if (!distances.ContainsKey(neighbour))
            {
                distances[neighbour] = distances[current.Name] + 1;
                queue.Enqueue(valves.First(x => x.Name == neighbour));
            }
        }
    }
    return distances;
}

List<Valve> ParseValves(string[] input)
{
    var valves = new List<Valve>();

    foreach (var line in input)
    {
        var name = line.Split(' ')[1];
        var rate = int.Parse(line.Split('=')[1].Split(';')[0]);
        var split = line.Split("leads to valve ");

        var connectedValves = new List<string>();
        if (split.Length > 1)
        {
            connectedValves.Add(split[1]);
        }
        else
        {
            connectedValves.AddRange(line.Split("lead to valves ")[1].Split(", ").Select(x => x).ToList());
        }

        valves.Add(new Valve(name, rate, connectedValves));
    }
    return valves;
}

class Valve
{
    public string Name { get; private set; }
    public int ReleasePressure { get; private set; }
    public List<string> ConnectedValves { get; private set; }
    public Dictionary<string, int> ShortestPathsToValves { get; set; }

    public Valve(string name, int releasePressure, List<string> connectedValves)
    {
        Name = name;
        ReleasePressure = releasePressure;
        ConnectedValves = connectedValves;
        ShortestPathsToValves = new Dictionary<string, int>();
    }

    public void SetShortestPathToValves(List<Valve> valves)
    {
        var queue = new Queue<Valve>();

        queue.Enqueue(this);
        var visited = new HashSet<string>();
        var distances = new Dictionary<string, int>();
        distances[Name] = 0;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbour in current.ConnectedValves)
            {
                if (!distances.ContainsKey(neighbour))
                {
                    distances[neighbour] = distances[current.Name] + 1;
                    queue.Enqueue(valves.First(x => x.Name == neighbour));
                }
            }
        }

        distances.Remove(Name);
        ShortestPathsToValves = distances;
    }
}