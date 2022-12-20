// https://adventofcode.com/2022/day/17

using System.Linq;
using System.Security.Cryptography.X509Certificates;

var input = File.ReadAllLines("Day17.txt");

PartOne(input);

void PartOne(string[] input)
{
    var chamber = new Chamber();
}

Block CreateBlock(int type, Coord startCoord) => (type) switch
{
    1 => new Line(startCoord),
    2 => new Plus(startCoord),
    3 => new L(startCoord),
    4 => new I(startCoord),
    5 => new Square(startCoord),
};

public record Coord(int X, int Y);

class Chamber
{
    public Dictionary<Coord, string> Map { get; private set; }

    public Chamber()
    {
        Map = new Dictionary<Coord, string>();

        for (int i = 0; i < 7; i++)
        {
            var coord = new Coord(i, 0);
            Map[coord] = "-";
        }
    }

    public bool IsOnEdge(Coord coord)
    {
        if (coord.X < 0) return true;
        if (coord.X > 6) return true;

        return false;
    }
}

abstract class Block
{
    public List<Coord> Body { get; set; }
}

class Line : Block
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

class Plus : Block
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

class L : Block
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

class I : Block
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

class Square : Block
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