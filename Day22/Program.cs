// https://adventofcode.com/2022/day/22

using System.Data;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("Day22.txt");

PartOne(input);

void PartOne(string[] input)
{
    var board = new Board(input.TakeWhile(x => !string.IsNullOrEmpty(x)).ToArray());
    var finger = new Finger(board.Start);
    var instructionString = input.Skip(input.ToList().IndexOf("") + 1).First();
    var pattern = @"(\d+)|(\D+)";
    var instructions = Regex.Split(instructionString, pattern).Where(x => !string.IsNullOrEmpty(x)).ToList();
    var finalPosition = MoveFingerAcrossBoard(board, finger, instructions);
    var result = (1000 * finalPosition.Y) + (4 * finalPosition.X) + finger.Direction;

    Console.Write($"Part one: {result}");
}

Coord MoveFingerAcrossBoard(Board board, Finger finger, List<string> instructions)
{
    foreach (var instruction in instructions)
    {
        if (IsMoveInstruction(instruction))
        {
            finger.Position = MoveAcrossBoard(board, finger.Direction, finger.Position, int.Parse(instruction));
        }
        else
        {
            finger.Turn(instruction);
        }
    }

    return finger.Position;
}

bool IsMoveInstruction(string instruction) => int.TryParse(instruction, out var _);

Coord MoveAcrossBoard(Board board, Direction direction, Coord start, int steps)
{
    var result = new Coord(0, 0);

    switch (direction)
    {
        case Direction.Right:
            result = MoveRight(board, start, steps);
            break;
        case Direction.Down:
            result = MoveDown(board, start, steps);
            break;
        case Direction.Left:
            result = MoveLeft(board, start, steps);
            break;
        case Direction.Up:
            result = MoveUp(board, start, steps);
            break;
        default:
            break;
    }
    return result;
}

Coord MoveRight(Board board, Coord start, int steps)
{
    var prev = start;

    for (int i = 0; i < steps; i++)
    {
        var next = prev with { X = prev.X + 1 };
        if (board.IsCoordOb(next))
        {
            var col = board.Map.Keys.Where(x => x.Y == next.Y);
            next = col.First(c => c.X == col.Min(v => v.X));
        }

        if (board.Map[next] == '#')
            return prev;

        prev = next;
    }

    return prev;
}

Coord MoveLeft(Board board, Coord start, int steps)
{
    var prev = start;

    for (int i = 0; i < steps; i++)
    {
        var next = prev with { X = prev.X - 1 };
        if (board.IsCoordOb(next))
        {
            var col = board.Map.Keys.Where(x => x.Y == next.Y);
            next = col.First(c => c.X == col.Max(v => v.X));
        }

        if (board.Map[next] == '#')
            return prev;

        prev = next;
    }

    return prev;
}

Coord MoveDown(Board board, Coord start, int steps)
{
    var prev = start;

    for (int i = 0; i < steps; i++)
    {
        var next = prev with { Y = prev.Y + 1 };
        if (board.IsCoordOb(next))
        {
            var row = board.Map.Keys.Where(x => x.X == next.X);
            next = row.First(c => c.Y == row.Min(v => v.Y));
        }

        if (board.Map[next] == '#')
            return prev;

        prev = next;
    }

    return prev;
}

Coord MoveUp(Board board, Coord start, int steps)
{
    var prev = start;

    for (int i = 0; i < steps; i++)
    {
        var next = prev with { Y = prev.Y - 1 };
        if (board.IsCoordOb(next))
        {
            var row = board.Map.Keys.Where(x => x.X == next.X);
            next = row.First(c => c.Y == row.Max(v => v.Y));
        }

        if (board.Map[next] == '#')
            return prev;

        prev = next;
    }

    return prev;
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
    public Dictionary<Coord, char> Map { get; }
    public Coord Start => GetStart();

    public Board(string[] input)
    {
        Map = InitMap(input);
    }

    public bool IsCoordOb(Coord coord)
    {
        return !Map.ContainsKey(coord);
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