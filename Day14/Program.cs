// https://adventofcode.com/2022/day/14

using System.Runtime.CompilerServices;

var input = File.ReadAllLines("Day14.txt");

PartOne(input);

void PartOne(string[] inputs)
{
    var map = new Dictionary<Coord, string>();

    foreach (var input in inputs)
    {
        var split = input.Split(" -> ");

        for (int i = 0; i < split.Length - 1; i++)
        {
            var c1 = new Coord(int.Parse(split[i].Split(',')[0]), int.Parse(split[i].Split(',')[1]));
            var c2 = new Coord(int.Parse(split[i + 1].Split(',')[0]), int.Parse(split[i + 1].Split(',')[1]));

            var line = GetLine(c1, c2).ToList();
        }
    }
}
IEnumerable<Coord> GetLine(Coord c1, Coord c2)
{
    if (c1.X == c2.X)
    {
        var c1First = c1.Y < c2.Y;
        var s = c1First ? c1.Y : c2.Y;
        var e = c1First ? c2.Y : c1.Y;
        return Enumerable.Range(s, e - s + 1).Select(i => new Coord(c1.X, i));
    }
    if (c1.Y == c2.Y)
    {
        var c1First = c1.X < c2.X;
        var s = c1First ? c1.X : c2.X;
        var e = c1First ? c2.X : c1.X;
        return Enumerable.Range(s, e - s + 1).Select(i => new Coord(i, c1.Y));
    }
    return new List<Coord>();
}

record Coord(int X, int Y);
