// https://adventofcode.com/2022/day/22

using System.Data;

var input = File.ReadAllLines("Day22.txt");

PartOne(input);

void PartOne(string[] input)
{
    var board = new Board(input.TakeWhile(x => !string.IsNullOrEmpty(x)).ToArray());
    var instructions = input.Skip(input.ToList().IndexOf("") + 1).First();
}

Coord MoveFinger(Finger finger, Board board, int steps)
{
    for (int i = 0; i < steps; i++)
    {
        switch (finger.Direction)
        {
            case Direction.Right:
                break;
            case Direction.Down:
                break;
            case Direction.Left:
                break;
            case Direction.Up:
                break;
            default:
                break;
        }
    }

    return new Coord(0, 0);
}

record Coord(int X, int Y);

class Finger
{
    public Coord Position { get; set; }
    public Direction Direction { get; private set; }

    public Finger(Coord start)
    {
        Position = start;
        Direction = Direction.Right;
    }

    public void Turn(string input)
    {
        var directionInt = 0;
        if (input == "R")
            directionInt = ((int)Direction + 1 + 4) % 4;
        if (input == "L")
            directionInt = ((int)Direction - 1 + 4) % 4;

        Direction = (Direction)directionInt;
    }
}

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