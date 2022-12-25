// https://adventofcode.com/2022/day/18

var input = File.ReadAllLines("Day18.txt");

PartOne(input);
Parttwo(input);

void PartOne(string[] input)
{
    var cubes = ParseToCubes(input).ToList();

    CompareCubes(cubes, cubes);

    var touchedFaces = cubes.SelectMany(x => x.Faces).Count();
    var answer = cubes.Count() * 6 - touchedFaces;
    Console.WriteLine($"Part one: {answer}");
}

void Parttwo(string[] input)
{
    var coords = ParseToCoords(input).ToList();
    var map = GetMap(coords);
    Surround(map, new Coord(coords.Min(x => x.X), coords.Min(x => x.Y), coords.Min(x => x.Z)));
    var answer = map.Values.Where(s => s is Lava).Sum(x => x.Surfaces);
    Console.WriteLine($"Part two: {answer}");
}

void Surround(Dictionary<Coord, Space> map, Coord start)
{
    var queue = new Queue<Space>();

    queue.Enqueue(map[start]);
    map[start].Visited = true;

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();

        var neighbours = GetNeighbours(current, map);

        foreach (var neighbour in neighbours)
        {
            var space = map[neighbour];
            space.Surfaces++;

            if (!space.Visited)
            {
                queue.Enqueue(space);
                space.Visited = true;
            }
        }
    }
}

IEnumerable<Coord> GetNeighbours(Space current, Dictionary<Coord, Space> map)
{
    foreach (var neighbourCoord in current.GetNeighbours())
        if (map.TryGetValue(neighbourCoord, out var neighbour))
            yield return neighbour.Coord;
}

Dictionary<Coord, Space> GetMap(List<Coord> coords)
{
    var map = new Dictionary<Coord, Space>();

    var minX = coords.Min(c => c.X) - 1;
    var maxX = coords.Max(c => c.X) + 1;
    var minY = coords.Min(c => c.Y) - 1;
    var maxY = coords.Max(c => c.Y) + 1;
    var minZ = coords.Min(c => c.Z) - 1;
    var maxZ = coords.Max(c => c.Z) + 1;

    for (int x = minX; x <= maxX; x++)
        for (int y = minY; y <= maxY; y++)
            for (int z = minZ; z <= maxZ; z++)
            {
                var coord = new Coord(x, y, z);
                var isLava = coords.Any(x => x == coord);
                map[coord] = isLava ? new Lava(coord) : new Air(coord);
            }
    return map;
}

void CompareCubes(List<Cube> cubes, List<Cube> other)
{
    foreach (var cubeA in cubes)
    {
        foreach (var cubeB in other)
        {
            if (cubeA.AreEqual(cubeB))
                continue;

            // check if A's front face is touching B's back face
            if (cubeA.X + cubeA.Size == cubeB.X && cubeA.Y == cubeB.Y && cubeA.Z == cubeB.Z)
            {
                cubeA.Faces.Add(Face.Front);
            }

            // check if A's back face is touching B's front face
            if (cubeA.X == cubeB.X + cubeB.Size && cubeA.Y == cubeB.Y && cubeA.Z == cubeB.Z)
            {
                cubeA.Faces.Add(Face.Back);
            }

            // check if A's top face is touching B's bottom face
            if (cubeA.X == cubeB.X && cubeA.Y == cubeB.Y + cubeB.Size && cubeA.Z == cubeB.Z)
            {
                cubeA.Faces.Add(Face.Top);
            }

            // check if A's bottom face is touching B's top face
            if (cubeA.X == cubeB.X && cubeA.Y + cubeA.Size == cubeB.Y && cubeA.Z == cubeB.Z)
            {
                cubeA.Faces.Add(Face.Bottom);
            }

            // check if A's left face is touching B's right face
            if (cubeA.X == cubeB.X && cubeA.Y == cubeB.Y && cubeA.Z + cubeA.Size == cubeB.Z)
            {
                cubeA.Faces.Add(Face.Left);
            }

            // check if A's right face is touching B's left face
            if (cubeA.X == cubeB.X && cubeA.Y == cubeB.Y && cubeA.Z == cubeB.Z + cubeB.Size)
            {
                cubeA.Faces.Add(Face.Right);
            }
        }
    }
}

IEnumerable<Coord> ParseToCoords(string[] input)
{
    foreach (var line in input)
    {
        var xyz = line.Split(',').Select(x => int.Parse(x)).ToArray();
        yield return new Coord(xyz[0], xyz[1], xyz[2]);
    }
}

IEnumerable<Cube> ParseToCubes(string[] input)
{
    foreach (var line in input)
    {
        var xyz = line.Split(',').Select(x => int.Parse(x)).ToArray();
        yield return new Cube(xyz[0], xyz[1], xyz[2]);
    }
}

record Coord(int X, int Y, int Z)
{
    public IEnumerable<Coord> GetNeighbours()
    {
        yield return this with { X = X - 1 };
        yield return this with { X = X + 1 };
        yield return this with { Y = Y - 1 };
        yield return this with { Y = Y + 1 };
        yield return this with { Z = Z - 1 };
        yield return this with { Z = Z + 1 };
    }
}

abstract class Space
{
    public bool Visited { get; set; }
    public int Surfaces { get; set; }
    public Coord Coord { get; }
    public virtual IEnumerable<Coord> GetNeighbours() => new List<Coord>();

    public Space(Coord coord)
    {
        Coord = coord;
    }
}

class Air : Space
{
    public override IEnumerable<Coord> GetNeighbours() => Coord.GetNeighbours();
    public Air(Coord coord) : base(coord) { }
}

class Lava : Space
{
    public Lava(Coord coord) : base(coord) { }
}

class Cube
{
    public HashSet<Face> Faces { get; set; }
    public int X => Coord.X;
    public int Y => Coord.Y;
    public int Z => Coord.Z;
    public int Size { get; private set; } = 1;
    private Coord Coord { get; set; }

    public Cube(int x, int y, int z)
    {
        Faces = new HashSet<Face>();
        Coord = new Coord(x, y, z);
    }

    public bool AreEqual(Cube other) => X == other.X && Y == other.Y && Z == other.Z;
}

enum Face
{
    Front,
    Back,
    Top,
    Bottom,
    Left,
    Right
}