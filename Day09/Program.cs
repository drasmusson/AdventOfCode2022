// https://adventofcode.com/2022/day/9

var input = File.ReadAllLines("Day09.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var instructions = input.Select(x => (Direction: x.Split(' ')[0], Distance: int.Parse(x.Split(' ')[1]))).ToList();

    var rope = new Rope(2);

    foreach (var instruction in instructions)
        rope.Move(instruction);

    Console.WriteLine($"Part one: {rope.Visited.Count()}");
}

void PartTwo(string[] input)
{
    var instructions = input.Select(x => (Direction: x.Split(' ')[0], Distance: int.Parse(x.Split(' ')[1]))).ToList();

    var rope = new Rope(10);

    foreach (var instruction in instructions)
        rope.Move(instruction);

    Console.WriteLine($"Part two: {rope.Visited.Count()}");
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
    public Knot Head { get; internal set; }
    public HashSet<Coord> Visited { get; }

    public Rope(int ropeLength)
    {
        Visited = new HashSet<Coord> { new Coord(0, 0) };
        Head = AddKnotsToRope(ropeLength);
    }

    private Knot AddKnotsToRope(int ropeLength)
    {
        var knots = new List<Knot>();

        for (int i = 0; i < ropeLength; i++)
        {
            var knot = new Knot(null, null);

            if (knots.Any())
            {
                knot.Parent = knots[i - 1];
            }
            knots.Add(knot);
        }
        for (int i = 0; i < knots.Count - 1; i++)
        {
            knots[i].Child = knots[i + 1];
        }
        return knots[0];
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

        var tailCoord = Head.Child?.Move();
        if (tailCoord != null) 
        {
            Visited.Add(tailCoord);
        }
    }

    private void MoveHead(string direction)
    {
        switch (direction)
        {
            case "R":
                Head.Position = Head.Position.RightNeighbour();
                break;
            case "L":
                Head.Position = Head.Position.LeftNeighbour();
                break;
            case "U":
                Head.Position = Head.Position.TopNeighbour();
                break;
            case "D":
                Head.Position = Head.Position.BottNeighbour();
                break;
            default:
                break;
        }
    }
}

class Knot
{
    public Coord Position { get; internal set; }
    public Knot? Parent { get; set; }
    public Knot? Child { get; set; }
    public Knot(Knot? child, Knot? parent)
    {
        Child = child;
        Position = new Coord(0, 0);
        Parent = parent;
    }

    internal Coord? Move()
    {
        if (IsTouchingParent())
            return null;

        if (Position.X - Parent?.Position.X < -1 || (Position.X - Parent?.Position.X == -1 && Position.Y != Parent?.Position.Y))
            Position = Position.RightNeighbour();
        if (Position.X - Parent?.Position.X > 1 || (Position.X - Parent?.Position.X == 1 && Position.Y != Parent?.Position.Y))
            Position = Position.LeftNeighbour();
        if (Position.Y - Parent?.Position.Y < -1 || (Position.Y - Parent?.Position.Y == -1 && Position.X != Parent?.Position.X))
            Position = Position.TopNeighbour();
        if (Position.Y - Parent?.Position.Y > 1 || (Position.Y - Parent?.Position.Y == 1 && Position.X != Parent?.Position.X))
            Position = Position.BottNeighbour();

        if (Child is null)
            return Position;

        return Child.Move();
    }

    private bool IsTouchingParent()
    {
        if (Position == Parent?.Position)
            return true;

        if (Position == Parent?.Position.BottNeighbour()
            || Position == Parent?.Position.BottRightNeighbour()
            || Position == Parent?.Position.BottLeftNeighbour()
            || Position == Parent?.Position.LeftNeighbour()
            || Position == Parent?.Position.RightNeighbour()
            || Position == Parent?.Position.TopLeftNeighbour()
            || Position == Parent?.Position.TopNeighbour()
            || Position == Parent?.Position.TopRightNeighbour())
            return true;

        return false;
    }
}