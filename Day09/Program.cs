// https://adventofcode.com/2022/day/9

var input = File.ReadAllLines("Day09.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var instructions = input.Select(x => (Direction: x.Split(' ')[0], Distance: int.Parse(x.Split(' ')[1]))).ToList();

    var rope = new Rope();

    foreach (var instruction in instructions)
        rope.Move(instruction);

    Console.WriteLine($"Part one: {rope.Visited.Count()}");
}

void PartTwo(string[] input)
{
    var instructions = input.Select(x => (Direction: x.Split(' ')[0], Distance: int.Parse(x.Split(' ')[1]))).ToList();

    var rope = new Rope();

    foreach (var instruction in instructions)
        rope.Move(instruction);

    Console.WriteLine($"Part one: {rope.Visited.Count()}");
}

record Coord(int X, int Y)
{
    public Coord RightNeighbour() => this with { X = X + 1 };
    public Coord LeftNeighbour() => this with { X = X - 1 };
    public Coord TopNeighbour() => this with { Y = Y + 1 };
    public Coord BottNeighbour() => this with { Y = Y - 1 };
    public Coord BottLeftNeighbour() => this with { X = X - 1, Y = Y - 1 };
    public Coord BottRightNeighbour() => this with { X = X + 1, Y = Y - 1 };
    public Coord TopRightNeighbour() => this with { X = X + 1, Y = Y + 1 };
    public Coord TopLeftNeighbour() => this with { X = X - 1, Y = Y + 1 };
}

class Rope
{
    public Coord Head { get; internal set; }
    public Coord Tail { get; internal set; }
    public HashSet<Coord> Visited { get; }

    public Rope()
    {
        Head = new Coord(0, 0);
        Tail = new Coord(0, 0);
        Visited = new HashSet<Coord> { new Coord(0, 0) };
    }

    internal void Move((string Direction, int Distance) instruction)
    {
        for (int i = 0; i < instruction.Distance; i++)
        {
            MoveInDirection(instruction.Direction);
        }
    }

    private void MoveInDirection(string direction)
    {
        MoveHead(direction);

        if(!HeadAndTailIsTouching())
            MoveTail();

        Visited.Add(Tail);
    }

    private void MoveHead(string direction)
    {
        switch (direction)
        {
            case "R":
                Head = Head.RightNeighbour();
                break;
            case "L":
                Head = Head.LeftNeighbour();
                break;
            case "U":
                Head = Head.TopNeighbour();
                break;
            case "D":
                Head = Head.BottNeighbour();
                break;
            default:
                break;
        }
    }

    private void MoveTail()
    {
        if (Tail.X - Head.X < -1 || (Tail.X - Head.X == -1 && Tail.Y != Head.Y))
            Tail = Tail.RightNeighbour();
        if (Tail.X - Head.X > 1 || (Tail.X - Head.X == 1 && Tail.Y != Head.Y))
            Tail = Tail.LeftNeighbour();
        if (Tail.Y - Head.Y < -1 || (Tail.Y - Head.Y == -1 && Tail.X != Head.X))
            Tail = Tail.TopNeighbour();
        if (Tail.Y - Head.Y > 1 || (Tail.Y - Head.Y == 1 && Tail.X != Head.X))
            Tail = Tail.BottNeighbour();
    }

    private bool HeadAndTailIsTouching()
    {
        if (Head == Tail)
            return true;

        if (Tail == Head.BottNeighbour()
            || Tail == Head.BottRightNeighbour()
            || Tail == Head.BottLeftNeighbour()
            || Tail == Head.LeftNeighbour()
            || Tail == Head.RightNeighbour()
            || Tail == Head.TopLeftNeighbour()
            || Tail == Head.TopNeighbour()
            || Tail == Head.TopRightNeighbour())
            return true;

        return false;
    }
}

class LargeRope
{
    public Coord Head { get; internal set; }
    public Knot ChildKnot { get; internal set; }
    public HashSet<Coord> Visited { get; }

    public LargeRope()
    {
        Head = new Coord(0, 0);
        Visited = new HashSet<Coord> { new Coord(0, 0) };

        var prevKnot = new Knot(null);

        for (int i = 0; i < 10; i++)
        {
            var newKnot = new Knot(prevKnot); 
            ChildKnot = newKnot;
            prevKnot = newKnot;
        }
    }

    internal void Move((string Direction, int Distance) instruction)
    {
        for (int i = 0; i < instruction.Distance; i++)
        {
            MoveInDirection(instruction.Direction);
        }
    }

    private void MoveInDirection(string direction)
    {
        MoveHead(direction);

        if (!HeadAndTailIsTouching())
            MoveTail();

        Visited.Add(Tail);
    }

    private void MoveHead(string direction)
    {
        switch (direction)
        {
            case "R":
                Head = Head.RightNeighbour();
                break;
            case "L":
                Head = Head.LeftNeighbour();
                break;
            case "U":
                Head = Head.TopNeighbour();
                break;
            case "D":
                Head = Head.BottNeighbour();
                break;
            default:
                break;
        }
    }

    private void MoveTail()
    {
        if (Tail.X - Head.X < -1 || (Tail.X - Head.X == -1 && Tail.Y != Head.Y))
            Tail = Tail.RightNeighbour();
        if (Tail.X - Head.X > 1 || (Tail.X - Head.X == 1 && Tail.Y != Head.Y))
            Tail = Tail.LeftNeighbour();
        if (Tail.Y - Head.Y < -1 || (Tail.Y - Head.Y == -1 && Tail.X != Head.X))
            Tail = Tail.TopNeighbour();
        if (Tail.Y - Head.Y > 1 || (Tail.Y - Head.Y == 1 && Tail.X != Head.X))
            Tail = Tail.BottNeighbour();
    }

    private bool HeadAndTailIsTouching()
    {
        if (Head == Tail)
            return true;

        if (Tail == Head.BottNeighbour()
            || Tail == Head.BottRightNeighbour()
            || Tail == Head.BottLeftNeighbour()
            || Tail == Head.LeftNeighbour()
            || Tail == Head.RightNeighbour()
            || Tail == Head.TopLeftNeighbour()
            || Tail == Head.TopNeighbour()
            || Tail == Head.TopRightNeighbour())
            return true;

        return false;
    }
}

class Knot
{
    public Coord Position { get; }
    public Knot? Child { get; set; }
    public Knot(Knot? child)
    {
        Child = child;
        Position = new Coord(0, 0);
    }
}