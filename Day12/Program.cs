// https://adventofcode.com/2022/day/12

var input = File.ReadAllLines("Day12.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var map = ParseElevationPoints(input);
    var start = map.Values.First(x => x is Start);

    var answer = Search(map, start);

    Console.WriteLine($"Part one: {answer}");
}

void PartTwo(string[] input)
{
    var map = ParseElevationPoints(input);
    var lowestPoints = GetLowestPoints(map.Values).ToList();
    var pathLengths = SearchFromPoints(map, lowestPoints);

    var answer = pathLengths.Where(x => x > -1).Min();
    Console.WriteLine($"Part two: {answer}");
}

IEnumerable<int> SearchFromPoints(Dictionary<Coord, ElevationPoint> map, List<ElevationPoint> lowestPoints)
{
    foreach (var point in lowestPoints)
    {
        ResetMap(map);
        yield return Search(map, point);
    }
}

void ResetMap(Dictionary<Coord, ElevationPoint> map)
{
    foreach (var elevationPoint in map.Values)
        elevationPoint.Reset();
}

IEnumerable<ElevationPoint> GetLowestPoints(Dictionary<Coord, ElevationPoint>.ValueCollection values)
{
    return values.Where(x => x.Height == 0);
}

int Search(Dictionary<Coord, ElevationPoint> map, ElevationPoint start)
{
    var queue = new Queue<ElevationPoint>();

    queue.Enqueue(start);
    start.Visited = true;

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();

        if (current is End)
            return current.StepsFromStart;

        var neighbours = GetLowerNeighbours(map, current).ToList();

        foreach (var neighbour in neighbours)
        {
            if (!neighbour.Visited)
            {
                queue.Enqueue(neighbour);
                neighbour.Visited = true;
                neighbour.StepsFromStart = current.StepsFromStart + 1;
            }
        }
    }
    //This should never be reached, it means the end was not found
    return -1;
}

IEnumerable<ElevationPoint> GetLowerNeighbours(Dictionary<Coord, ElevationPoint> points, ElevationPoint current)
{
    foreach (var neighbourCoord in current.Coord.GetNeighbouringCoords())
        if (points.TryGetValue(neighbourCoord, out var neighbour) && (neighbour.Height <= current.Height + 1))
            yield return neighbour;
}

Dictionary<Coord, ElevationPoint> ParseElevationPoints(string[] input)
{
    var elevationPoints = new Dictionary<Coord, ElevationPoint>();

    for (int y = 0; y < input.Length; y++)
        for (int x = 0; x < input[y].Length; x++)
        {
            var coord = new Coord(x, y);
            var elevationPoint = CreateElevationPoint(coord, input[y][x]);

            elevationPoints.Add(coord, elevationPoint);
        }

    return elevationPoints;
}

ElevationPoint CreateElevationPoint(Coord coord, char heightChar)
{
    switch (heightChar)
    {
        case 'S':
            return new Start(coord, heightChar);
        case 'E':
            return new End(coord, heightChar);
        default:
            return new ElevationPoint(coord, heightChar);
    }
}

record Coord(int X, int Y)
{
    internal IEnumerable<Coord> GetNeighbouringCoords()
    {
        yield return this with { X = X - 1 };
        yield return this with { X = X + 1 };
        yield return this with { Y = Y - 1 };
        yield return this with { Y = Y + 1 };
    }
}

class ElevationPoint
{
    public Coord Coord { get; }
    public bool Visited { get; set; }
    public int Height { get; }
    public int StepsFromStart { get; set; }

    public ElevationPoint(Coord coord, char heightChar)
    {
        Coord = coord;
        Height = GetHeightValueForChar(heightChar);
    }
    public void Reset()
    {
        Visited = false;
        StepsFromStart = 0;
    }

    private int GetHeightValueForChar(char heightChar)
    {
        var heights = "abcdefghijklmnopqrstuvwxyz";

        switch (heightChar)
        {
            case 'S':
                return heights.IndexOf('a');
            case 'E':
                return heights.IndexOf('z');
            default:
                return heights.IndexOf(heightChar);
        }
    }
}

class Start : ElevationPoint
{
    public Start(Coord coord, char heightChar) : base(coord, heightChar)
    {
    }
}

class End : ElevationPoint
{
    public End(Coord coord, char heightChar) : base(coord, heightChar)
    {
    }
}