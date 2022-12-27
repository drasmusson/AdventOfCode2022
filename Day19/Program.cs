// https://adventofcode.com/2022/day/19

var input = File.ReadAllLines("../Input/Day19.txt");

PartOne(input);

void PartOne(string[] input)
{
    
}

record Robot(
    List<(Unit, int)> Cost,
    (Unit, int) Production
);

enum Unit
{
    Ore,
    Clay,
    Obsidian,
    Geode
}