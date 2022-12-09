// https://adventofcode.com/2022/day/8

var input = File.ReadAllLines("Day08.txt");


PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var treePatch = ParseInputToPatch(input);

    var topEdgeCoords = treePatch.GetTopEdge();
    foreach (var coord in topEdgeCoords)
    {
        LookDownUntilEdgeFromCoord(treePatch, coord);
    }

    var leftEdge = treePatch.GetLeftEdge();
    foreach (var coord in leftEdge)
    {
        LookRightUntilEdgeFromCoord(treePatch, coord);
    }

    var bottomEdge = treePatch.GetBottomEdge();
    foreach (var coord in bottomEdge)
    {
        LookUpUntilEdgeFromCoord(treePatch, coord);
    }

    var rightEdge = treePatch.GetRightEdge();
    foreach (var coord in rightEdge)
    {
        LookLeftUntilEdgeFromCoord(treePatch, coord);
    }
    
    var answer = treePatch.Map.Count(y => y.Value.Visible);

    Console.WriteLine($"Part one: {answer}");
}

void PartTwo(string[] input)
{
    var treePatch = ParseInputToPatch(input);

    foreach (var coord in treePatch.Map.Keys)
    {
        var up = GetUpViewDistansFromCoord(treePatch, coord);
        var left = GetLeftViewDistansFromCoord(treePatch, coord);
        var right = GetRightViewDistansFromCoord(treePatch, coord);
        var down = GetDownViewDistansFromCoord(treePatch, coord);

        treePatch.Map[coord].ViewingScore = up * left * right * down;
    }

    treePatch.SetOuteredgesTo(0);

    var maxViewScore = treePatch.Map.Values.Max(x => x.ViewingScore);

    Console.WriteLine($"Part two: {maxViewScore}");
}

int GetUpViewDistansFromCoord(TreePatch treePatch, Coord coord)
{
    var viewDistance = 0;
    var viewHeight = treePatch.Map[coord].Height;

    coord = coord.GetUpCoord();
    while (treePatch.Map.TryGetValue(coord, out var tree))
    {
        viewDistance++;

        if (viewHeight <= tree.Height)
            return viewDistance;

        coord = coord.GetUpCoord();
    }
    return viewDistance;
}

int GetLeftViewDistansFromCoord(TreePatch treePatch, Coord coord)
{
    var viewDistance = 0;
    var viewHeight = treePatch.Map[coord].Height;

    coord = coord.GetLeftCoord();
    while (treePatch.Map.TryGetValue(coord, out var tree))
    {
        viewDistance++;

        if (viewHeight <= tree.Height)
            return viewDistance;

        coord = coord.GetLeftCoord();
    }
    return viewDistance;
}


int GetRightViewDistansFromCoord(TreePatch treePatch, Coord coord)
{
    var viewDistance = 0;
    var viewHeight = treePatch.Map[coord].Height;

    coord = coord.GetRightCoord();
    while (treePatch.Map.TryGetValue(coord, out var tree))
    {
        viewDistance++;

        if (viewHeight <= tree.Height)
            return viewDistance;

        coord = coord.GetRightCoord();
    }
    return viewDistance;
}
int GetDownViewDistansFromCoord(TreePatch treePatch, Coord coord)
{
    var viewDistance = 0;
    var viewHeight = treePatch.Map[coord].Height;

    coord = coord.GetDownCoord();
    while (treePatch.Map.TryGetValue(coord, out var tree))
    {
        viewDistance++;

        if (viewHeight <= tree.Height)
            return viewDistance;

        coord = coord.GetDownCoord();
    }
    return viewDistance;
}

void LookUpUntilEdgeFromCoord(TreePatch treePatch, Coord coord)
{
    treePatch.Map[coord].Visible = true;

    var highestTree = 0;
    while (treePatch.Map.TryGetValue(coord, out var tree))
    {
        if (tree.Height > highestTree)
        {
            tree.Visible = true;
            highestTree = tree.Height;
        }
        coord = coord with { Y = coord.Y - 1 };
    }
}

void LookDownUntilEdgeFromCoord(TreePatch treePatch, Coord coord)
{
    treePatch.Map[coord].Visible = true;

    var highestTree = 0;
    while (treePatch.Map.TryGetValue(coord, out var tree))
    {
        if (tree.Height > highestTree)
        {
            tree.Visible = true;
            highestTree = tree.Height;
        }
        coord = coord with { Y = coord.Y + 1 };
    }
}

void LookRightUntilEdgeFromCoord(TreePatch treePatch, Coord coord)
{
    treePatch.Map[coord].Visible = true;

    var highestTree = 0;
    while (treePatch.Map.TryGetValue(coord, out var tree))
    {
        if (tree.Height > highestTree)
        {
            tree.Visible = true;
            highestTree = tree.Height;
        }
        coord = coord with { X = coord.X + 1 };
    }
}

void LookLeftUntilEdgeFromCoord(TreePatch treePatch, Coord coord)
{
    treePatch.Map[coord].Visible = true;

    var highestTree = 0;
    while (treePatch.Map.TryGetValue(coord, out var tree))
    {
        if (tree.Height > highestTree)
        {
            tree.Visible = true;
            highestTree = tree.Height;
        }
        coord = coord with { X = coord.X - 1 };
    }
}
TreePatch ParseInputToPatch(string[] input)
{
    var treePatch = new TreePatch();

    var xLength = input[0].Length;
    var yLength = input.Length;

    for (int y = 0; y < yLength; y++)
    {
        for (int x = 0; x < xLength; x++)
        {
            var coord = new Coord(x, y);
            var treeHeight = int.Parse(input[y][x].ToString());

            treePatch.AddTreeToCoordinate(coord, new Tree(treeHeight));
        }
    }
    return treePatch;
}

public record Coord(int X, int Y)
{
    public Coord GetUpCoord() => this with { Y = Y - 1 };
    public Coord GetLeftCoord() => this with { X = X - 1 };
    public Coord GetRightCoord() => this with { X = X + 1 };
    public Coord GetDownCoord() => this with { Y = Y + 1 };
}

public class Tree
{
    public bool Visible { get; set; }
    public int Height { get; }
    public long ViewingScore { get; set; }
    public Tree(int height)
    {
        Height = height;
        Visible = false;
    }
}

public class TreePatch
{
    public Dictionary<Coord, Tree> Map { get; }

    public TreePatch()
    {
        Map = new Dictionary<Coord, Tree>();
    }

    internal void AddTreeToCoordinate(Coord coord, Tree tree)
    {
        Map.Add(coord, tree);
    }

    internal List<Coord> GetTopEdge()
    {
        return Map.Select(x => x.Key).Where(c => c.Y == 0).ToList();
    }

    internal List<Coord> GetLeftEdge()
    {
        return Map.Select(x => x.Key).Where(c => c.X == 0).ToList();
    }

    internal List<Coord> GetRightEdge()
    {
        var highestX = Map.Select(x => x.Key).Max(c => c.X);

        return Map.Select(x => x.Key).Where(c => c.X == highestX).ToList();
    }

    internal List<Coord> GetBottomEdge()
    {
        var highestY = Map.Select(x => x.Key).Max(c => c.Y);

        return Map.Select(x => x.Key).Where(c => c.Y == highestY).ToList();
    }

    internal void SetOuteredgesTo(int v)
    {
        var outerBotCoords = GetBottomEdge();

        foreach (var outerCoord in outerBotCoords)
            Map[outerCoord].ViewingScore = v;

        var outerTopCoords = GetTopEdge();

        foreach (var outerCoord in outerBotCoords)
            Map[outerCoord].ViewingScore = v;

        var outerLeftCoords = GetLeftEdge();

        foreach (var outerCoord in outerLeftCoords)
            Map[outerCoord].ViewingScore = v;

        var outerRightCoords = GetRightEdge();

        foreach (var outerCoord in outerRightCoords)
            Map[outerCoord].ViewingScore = v;
    }
}