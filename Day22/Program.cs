// https://adventofcode.com/2022/day/22

var input = File.ReadAllLines("Day22.txt");

PartOne(input);

void PartOne(string[] input)
{
    var board = new Board(input.TakeWhile(x => !string.IsNullOrEmpty(x)).ToArray());
    var instructions = input.Skip(input.ToList().IndexOf("") + 1).First();
}

record Coord(int X, int Y);

class Board
{
    public Dictionary<Coord, char>  Map { get; }
    public Coord Start => GetStart();

    public Board(string[] input)
    {
        Map = InitMap(input);
    }

    private Coord GetStart()
    {
        var minX = Map.Keys.Where(k => k.Y == 1).Min(c => c.X);
        return new Coord(minX, 1);
    }

    private Dictionary<Coord, char> InitMap(string[] input)
    {
        var map = new Dictionary<Coord, char>();
        for (int y = 0; y < input.Length; y++)
        {
            var row = input[y];
            for (int x = 0; x < row.Length; x++)
            {
                var space = row[x];
                if (space == '.' || space == '#')
                {
                    var coord = new Coord(x + 1, y + 1);
                    map[coord] = space;
                }
            }
        }
        return map;
    }
}

enum Direction
{
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3
}