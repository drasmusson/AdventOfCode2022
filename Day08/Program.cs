// https://adventofcode.com/2022/day/8

var input = File.ReadAllLines("Day08.txt");

PartOne(input);

void PartOne(string[] input)
{
    var xLength = input[0].Length;
    var yLength = input.Length;

    for (int y = 0; y < yLength; y++)
    {
        for (int x = 0; x < xLength; x++)
        {

        }
    }
}

Console.WriteLine("Hello, World!");

public record Coord(int X, int Y);

public class Tree
{
    public bool Visible { get; set; }
    public int Height { get; }

    public Tree(int height)
    {
        Height = height;
        Visible = false;
    }
}