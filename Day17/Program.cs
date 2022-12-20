﻿// https://adventofcode.com/2022/day/17

var input = File.ReadAllLines("Day17.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var rockDropper = new RockDropper(input[0]);
    rockDropper.DropRocks(2022);
    var answer = rockDropper.Chamber.HighestPoint.Y;
    Console.WriteLine($"Part one: {answer}");
}

//void PartTwo(string[] input)
//{
//    var rockDropper = new RockDropper(input[0]);
//    rockDropper.DropRocks(1000000000000);
//    var answer = rockDropper.Chamber.HighestPoint.Y;
//    Console.WriteLine($"Part two: {answer}");
//}

class RockDropper
{
    public Chamber Chamber { get; private set; }
    private string JetPattern { get; set; }
    private int Iteration { get; set; }
    public RockDropper(string jetPattern)
    {
        Chamber = new Chamber();
        JetPattern = jetPattern;
        Iteration = 0;
    }

    public void DropRocks(long rocks)
    {
        for (int i = 0; i < rocks; i++)
        {
            var rock = CreateRock(i % 5, GetNewStartingCoord());

            DropRock(rock);
        }
    }

    void DropRock(Rock rock)
    {
        var rockIsFalling = true;

        while (rockIsFalling)
        {
            var direction = JetPattern[Iteration % JetPattern.Length];

            rock.MoveInDirection(direction);
            if (Chamber.IsIntersecting(rock.Body))
                rock.UndoLastMove();

            rock.MoveInDirection('V');
            if (Chamber.IsIntersecting(rock.Body))
            {
                rock.UndoLastMove();
                Chamber.AddRock(rock.Body);
                rockIsFalling = false;
            }
            Iteration++;
        }
    }

    Coord GetNewStartingCoord()
    {
        var highestCoord = Chamber.HighestPoint;
        return Chamber.HighestPoint with { X = 2, Y = highestCoord.Y + 4 };
    }

    Rock CreateRock(int type, Coord startCoord) => (type) switch
    {
        0 => new Line(startCoord),
        1 => new Plus(startCoord),
        2 => new L(startCoord),
        3 => new I(startCoord),
        4 => new Square(startCoord),
        _ => throw new NotImplementedException()
    };

}

public record Coord(int X, int Y);

class Chamber
{
    public Dictionary<Coord, string> Map { get; private set; }
    public Coord HighestPoint => Map.Keys.OrderByDescending(x => x.Y).First();

    public Chamber()
    {
        Map = new Dictionary<Coord, string>();

        for (int i = 0; i < 7; i++)
        {
            var coord = new Coord(i, 0);
            Map[coord] = "-";
        }
    }

    public bool IsIntersecting(List<Coord> rock)
    {
        return rock.Any(Map.ContainsKey) ||
            rock.Any(IsOnEdge);
    }

    public void AddRock(List<Coord> rock)
    {
        foreach (var coord in rock)
            Map[coord] = "#";
    }

    bool IsOnEdge(Coord coord)
    {
        if (coord.X < 0) return true;
        if (coord.X > 6) return true;

        return false;
    }
}

abstract class Rock
{
    public List<Coord> Body { get; internal set; }
    private List<Coord> LastPosition { get; set; }

    public void MoveInDirection(char direction)
    {
        switch (direction)
        {
            case '>':
                MoveRight();
                break;
            case '<':
                MoveLeft();
                break;
            case 'V':
                MoveDown();
                break;
            default:
                break;
        }
    }

    public void UndoLastMove()
    {
        Body = LastPosition;
    }

    private void MoveDown()
    {
        LastPosition = Body;

        var newPosition = new List<Coord>();

        foreach (var coord in Body)
            newPosition.Add(coord with { Y = coord.Y - 1 });

        Body = newPosition;
    }

    private void MoveLeft()
    {
        LastPosition = Body;

        var newPosition = new List<Coord>();

        foreach (var coord in Body)
            newPosition.Add(coord with { X = coord.X - 1 });

        Body = newPosition;
    }

    private void MoveRight()
    {
        LastPosition = Body;

        var newPosition = new List<Coord>();

        foreach (var coord in Body)
            newPosition.Add(coord with { X = coord.X + 1 });

        Body = newPosition;
    }
}

class Line : Rock
{
    public Line(Coord startCoord)
    {
        Body = new List<Coord>
        {
            startCoord,
            startCoord with { X = startCoord.X + 1 },
            startCoord with { X = startCoord.X + 2 },
            startCoord with { X = startCoord.X + 3 }
        };
    }
}

class Plus : Rock
{
    public Plus(Coord startCoord)
    {
        Body = new List<Coord>
        {
            startCoord with { X = startCoord.X + 1 },
            startCoord with { Y = startCoord.Y + 1 },
            startCoord with { X = startCoord.X + 1, Y = startCoord.Y + 1 },
            startCoord with { X = startCoord.X + 1, Y = startCoord.Y + 2 },
            startCoord with { X = startCoord.X + 2, Y = startCoord.Y + 1 },
        };
    }
}

class L : Rock
{
    public L(Coord startCoord)
    {
        Body = new List<Coord>
        {
            startCoord,
            startCoord with { X = startCoord.X + 1 },
            startCoord with { X = startCoord.X + 2 },
            startCoord with { X = startCoord.X + 2, Y = startCoord.Y + 1},
            startCoord with { X = startCoord.X + 2, Y = startCoord.Y + 2},
        };
    }
}

class I : Rock
{
    public I(Coord startCoord)
    {
        Body = new List<Coord>
        {
            startCoord,
            startCoord with { Y = startCoord.Y + 1 },
            startCoord with { Y = startCoord.Y + 2 },
            startCoord with { Y = startCoord.Y + 3 },
        };
    }
}

class Square : Rock
{
    public Square(Coord startCoord)
    {
        Body = new List<Coord>
        {
            startCoord,
            startCoord with { X = startCoord.X + 1 },
            startCoord with { Y = startCoord.Y + 1 },
            startCoord with { X = startCoord.X + 1, Y = startCoord.Y + 1 }
        };
    }
}