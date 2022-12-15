// https://adventofcode.com/2022/day/14

var input = File.ReadAllLines("Day14.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] inputs)
{
    var cave = new Cave(inputs, 1);
    PourSandUntilFlowing(cave);

    var answer = cave.Map.Values.Count(x => x == "o");
    Console.WriteLine($"Part one: {answer}");
}

void PartTwo(string[] inputs)
{
    var cave = new Cave(inputs, 2);
    PourSandUntilFull(cave);

    var answer = cave.Map.Values.Count(x => x == "o");
    Console.WriteLine($"Part one: {answer}");
}

void PourSandUntilFlowing(Cave cave)
{
    var isFlowing = false;
    while (!isFlowing)
    {
        isFlowing = PourGrainUntilStopOrFlowing(cave, new Grain(cave.SandSource));
    }
}

void PourSandUntilFull(Cave cave)
{
    var isFull = false;
    while (!isFull)
    {
        isFull = PourGrainUntilStop(cave, new Grain(cave.SandSource));
    }
}

bool PourGrainUntilStopOrFlowing(Cave cave, Grain grain)
{
    while (!grain.IsAtRest && !cave.IsOutOfBounds(grain.CurrentPos))
    {
        grain.FallThroughCave(cave);
    }
    if (!cave.IsOutOfBounds(grain.CurrentPos))
    {
        cave.Add(grain.CurrentPos, "o");
        return false;
    }
    return true;
}

bool PourGrainUntilStop(Cave cave, Grain grain)
{
    while (!grain.IsAtRest)
    {
        grain.FallThroughCave(cave);
    }
    if (!cave.IsFull)
    {
        cave.Add(grain.CurrentPos, "o");
        return false;
    }
    return true;
}

record Coord(int X, int Y)
{
    public IEnumerable<Coord> GetFallCorrdsInPriorityOrder()
    {
        yield return this with { Y = Y + 1 };
        yield return this with { X = X - 1, Y = Y + 1 };
        yield return this with { X = X + 1, Y = Y + 1 };
    }
    public IEnumerable<Coord> GetLine(Coord c)
    {
        if (X == c.X)
        {
            var thisFirst = Y < c.Y;
            var s = thisFirst ? Y : c.Y;
            var e = thisFirst ? c.Y : Y;
            return Enumerable.Range(s, e - s + 1).Select(i => new Coord(X, i));
        }
        if (Y == c.Y)
        {
            var thisFirst = X < c.X;
            var s = thisFirst ? X : c.X;
            var e = thisFirst ? c.X : X;
            return Enumerable.Range(s, e - s + 1).Select(i => new Coord(i, Y));
        }
        return new List<Coord>();
    }
}

class Grain
{
    public Coord CurrentPos { get; private set; }
    public bool IsAtRest { get; private set; }

    public Grain(Coord source)
    {
        CurrentPos = source;
    }

    public void FallThroughCave(Cave cave)
    {
        foreach (var fallCoord in CurrentPos.GetFallCorrdsInPriorityOrder())
        {
            if (cave.CoordIsEmpty(fallCoord))
            {
                FallTo(fallCoord);
                return;
            }
        }
        IsAtRest = true;
    }

    void FallTo(Coord coord)
    {
        CurrentPos = coord;
    }
}

class Cave
{
    public Dictionary<Coord, string> Map { get; private set; }
    public Coord SandSource { get; private set; }
    public bool IsFull => Map.TryGetValue(SandSource, out _);
    private (int Xmin, int Xmax, int Ymin, int Ymax) Boundaries { get; set; }
    private int FloorYCoord { get; set; }
    private int Part { get; set; }
    public Cave(string[] inputs, int part)
    {
        Map = new Dictionary<Coord, string>();
        ParseCave(inputs);
        SetBoundaries();
        SetFloor();
        Part = part;
        SandSource = new Coord(500, 0);
    }

    public bool CoordIsEmpty(Coord coord) => Part == 1 ? CoordIsEmptyV1(coord) : CoordIsEmptyV2(coord);

    private bool CoordIsEmptyV1(Coord coord)
    {
        return !Map.ContainsKey(coord);
    }

    private bool CoordIsEmptyV2(Coord coord)
    {
        if (coord.Y == FloorYCoord) return false;
        return !Map.ContainsKey(coord);
    }

    public void Add(Coord coord, string value)
    {
        if (!Map.ContainsKey(coord))
            Map.Add(coord, value);
        else
            Map[coord] = value;
    }

    public bool IsOutOfBounds(Coord coord)
    {
        if (coord.X > Boundaries.Xmax) return true;
        if (coord.X < Boundaries.Xmin) return true;
        if (coord.Y > Boundaries.Ymax) return true;
        if (coord.Y < Boundaries.Ymin) return true;

        return false;
    }

    private void SetFloor()
    {
        FloorYCoord = Boundaries.Ymax + 2;
    }

    private void SetBoundaries()
    {
        var xMin = Map.Keys.Min(x => x.X);
        var xMax = Map.Keys.Max(x => x.X);
        var yMin = 0;
        var yMax = Map.Keys.Max(x => x.Y);

        Boundaries = (xMin, xMax, yMin, yMax);
    }

    private void ParseCave(string[] inputs)
    {
        foreach (var input in inputs)
        {
            var split = input.Split(" -> ");

            for (int i = 0; i < split.Length - 1; i++)
            {
                var c1 = new Coord(int.Parse(split[i].Split(',')[0]), int.Parse(split[i].Split(',')[1]));
                var c2 = new Coord(int.Parse(split[i + 1].Split(',')[0]), int.Parse(split[i + 1].Split(',')[1]));

                var line = c1.GetLine(c2).ToList();

                foreach (var coord in line)
                    Add(coord, "#");
            }
        }
    }
}
