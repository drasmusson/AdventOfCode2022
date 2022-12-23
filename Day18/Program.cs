// https://adventofcode.com/2022/day/18

var input = File.ReadAllLines("Day18.txt");

PartOne(input);

void PartOne(string[] input)
{
    var cubes = ParsetoCubes(input).ToList();

    CompareCubes(cubes);

    var touchedFaces = cubes.SelectMany(x => x.Faces).Count();
    var answer = cubes.Count() * 6 - touchedFaces;
    Console.WriteLine($"Part one: {answer}");
}

void CompareCubes(List<Cube> cubes)
{
    foreach (var cubeA in cubes)
    {
        foreach (var cubeB in cubes)
        {
            if (cubeA.AreEqual(cubeB))
                continue;

            // check if A's front face is touching B's back face
            if (cubeA.X + cubeA.Size == cubeB.X && cubeA.Y == cubeB.Y && cubeA.Z == cubeB.Z)
            {
                cubeA.Faces[Face.Front] = true;
            }

            // check if A's back face is touching B's front face
            if (cubeA.X == cubeB.X + cubeB.Size && cubeA.Y == cubeB.Y && cubeA.Z == cubeB.Z)
            {
                cubeA.Faces[Face.Back] = true;
            }

            // check if A's top face is touching B's bottom face
            if (cubeA.X == cubeB.X && cubeA.Y == cubeB.Y + cubeB.Size && cubeA.Z == cubeB.Z)
            {
                cubeA.Faces[Face.Top] = true;
            }

            // check if A's bottom face is touching B's top face
            if (cubeA.X == cubeB.X && cubeA.Y + cubeA.Size == cubeB.Y && cubeA.Z == cubeB.Z)
            {
                cubeA.Faces[Face.Bottom] = true;
            }

            // check if A's left face is touching B's right face
            if (cubeA.X == cubeB.X && cubeA.Y == cubeB.Y && cubeA.Z + cubeA.Size == cubeB.Z)
            {
                cubeA.Faces[Face.Left] = true;
            }

            // check if A's right face is touching B's left face
            if (cubeA.X == cubeB.X && cubeA.Y == cubeB.Y && cubeA.Z == cubeB.Z + cubeB.Size)
            {
                cubeA.Faces[Face.Right] = true;
            }
        }
    }
}

IEnumerable<Cube> ParsetoCubes(string[] input)
{
    foreach (var line in input)
    {
        var xyz = line.Split(',').Select(x => int.Parse(x)).ToArray();
        yield return new Cube(xyz[0], xyz[1], xyz[2]);
    }
}

class Cube
{
    public Dictionary<Face, bool> Faces { get; set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Z { get; private set; }
    public int Size { get; private set; } = 1;

    public Cube(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
        Faces = new Dictionary<Face, bool>();
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