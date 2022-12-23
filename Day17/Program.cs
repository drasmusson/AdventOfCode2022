// https://adventofcode.com/2022/day/17

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

void PartTwo(string[] input)
{
    var rockDropper = new RockDropper(input[0]);
    var answer = rockDropper.DropRocks(1000000000000);
    Console.WriteLine($"Part two: {answer}");
}

class RockDropper
{
    public Chamber Chamber { get; private set; }
    private string JetPattern { get; set; }
    private long Iterations { get; set; }
    private List<int> PreviousJetIndexes { get; set; }
    private (int Height, long RockCount) RepeatSection { get; set; }
    private int? RepeatIndex { get; set; }
    private int CurrentJetIndex { get; set; }
    private bool JetWrapped { get; set; }
    private (int Height, long RockCount) Bottom { get; set; }
    private long RocksToDrop { get; set; }
    public RockDropper(string jetPattern)
    {
        Chamber = new Chamber();
        JetPattern = jetPattern;
        Iterations = 0;
        PreviousJetIndexes = new List<int>();
    }

    public long DropRocks(long rocks)
    {
        RocksToDrop = rocks;

        for (long i = 0; i < rocks; i++)
        {
            var rockType = i % 5;
            var rock = CreateRock((int)rockType, GetNewStartingCoord());
            JetWrapped = false;
            DropRock(rock);

            // this works for test input but my input wrappes at rock type 2 every time.
            if (JetWrapped && (i + 1) % 5 == 0)
            {
                if (RepeatIndex is null)
                {
                    if (PreviousJetIndexes.Contains(CurrentJetIndex))
                    {
                        Bottom = (Chamber.HighestPoint.Y, i);
                        RepeatIndex = CurrentJetIndex;
                    }
                    else
                    {
                        PreviousJetIndexes.Add(CurrentJetIndex);
                    }
                }
                else if (CurrentJetIndex == RepeatIndex)
                {
                    RepeatSection = (Chamber.HighestPoint.Y - Bottom.Height, i - Bottom.RockCount);
                    rocks = i + (rocks - Bottom.RockCount) % RepeatSection.RockCount;
                }
            }
        }

        var restHeight = Chamber.HighestPoint.Y - Bottom.Height - RepeatSection.Height;
        var repeats = RepeatSection.RockCount > 0 ? ((RocksToDrop - Bottom.RockCount) / RepeatSection.RockCount) : 0;

        long height = Bottom.Height + repeats * RepeatSection.Height + restHeight;
        return height;
    }

    void DropRock(Rock rock)
    {
        var rockIsFalling = true;

        while (rockIsFalling)
        {
            CurrentJetIndex = (int)(Iterations % JetPattern.Length);
            if (CurrentJetIndex == 0)
                JetWrapped = true;

            var direction = JetPattern[CurrentJetIndex];

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
            Iterations++;
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
    public Coord HighestPoint => Map.Keys.Any() ? Map.Keys.OrderByDescending(x => x.Y).First() : new Coord(0,0);

    public Chamber()
    {
        Map = new Dictionary<Coord, string>();

        //for (int i = 0; i < 7; i++)
        //{
        //    var coord = new Coord(i, 0);
        //    Map[coord] = "-";
        //}
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
        if (coord.Y < 1) return true;

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