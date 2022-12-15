// https://adventofcode.com/2022/day/13

using System;

var input = File.ReadAllLines("Day13.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var pairs = GetPairs(input);
    var items = ParsePairs(pairs);

    var indexes = ComparePairs(items);
    var answer = indexes.Sum();
    Console.WriteLine($"Part one: {answer}");
}

void PartTwo(string[] input)
{
    var inputList = input.ToList();

    inputList.RemoveAll(x => string.IsNullOrWhiteSpace(x));

    var divider1 = ParseListItem("[[2]]", out _);
    var divider2 = ParseListItem("[[6]]", out _);

    var items = inputList.Select(x => ParseListItem(x, out _)).ToList();
    items.Add(divider1);
    items.Add(divider2);

    var a = items.ToArray();
    Array.Sort(a, Compare);
    Array.Reverse(a);

    var i1 = Array.IndexOf(a, divider1);
    var i2 = Array.IndexOf(a, divider2);
    Console.WriteLine($"Part one: {(i1 + 1) * (i2 + 1)}");
}

int Compare(ListItem a, ListItem b)
{
    var cr = a.CompareListItems(b);

    if (cr == CompareResult.Equal || cr == CompareResult.Correct) return 1;

    return -1;
}

List<int> ComparePairs(List<ListItem> items)
{
    var result = new List<int>();
    for (int i = 0; i < items.Count; i+=2)
    {
        var cr = items[i].CompareListItems(items[i + 1]);
        if (cr == CompareResult.Correct) result.Add(i / 2 + 1);
    }
    return result;
}

List<ListItem> ParsePairs(List<(string Left, string Right)> pairs)
{
    var listItems = new List<ListItem>();

    foreach (var pair in pairs)
    {
        listItems.Add(ParseListItem(pair.Left, out _));
        listItems.Add(ParseListItem(pair.Right, out _));
    }
    return listItems;
}

ListItem ParseListItem(string input, out string rest)
{
    var result = new List<Item>();
    input = CutFirstChar(input);
    var end = false;
    while(input.Length > 0 && !end)
    {
        var c = input[0];
        switch (c)
        {
            case ']':
                end = true;
                break;
            case ',':
                input = CutFirstChar(input);
                break;
            default:
                result.Add(ParseItem(input, out input));
                break;
        }
    }
    rest = CutFirstChar(input);

    return new ListItem(result);
}

NumberItem ParseNumberItem(string input, out string rest)
{
    var i = input.IndexOfAny(new char[] {',',']'});
    rest = input.Substring(i);
    var value = int.Parse(input[..i]);
    return new NumberItem(value);
}

Item ParseItem(string input, out string rest) => input[0] switch
{
    '[' => ParseListItem(input, out rest),
    _ => ParseNumberItem(input, out rest)
};

string CutFirstChar(string input) => input.Length > 0 ? input.Substring(1, input.Length - 1) : "";

List<(string Left, string Right)> GetPairs(string[] input)
{
    var list = new List<(string Left, string Right)>();

    for (int i = 0; i < input.Length; i += 3)
    {
        var pair = (input[i], input[i + 1]);
        list.Add(pair);
    }
    return list;
}

record NumberItem(int Value) : Item
{
    public ListItem ToListItem()
    {
        return new ListItem(new List<Item> { this });
    }

    public CompareResult CompareNumberItems(NumberItem otherNum) =>
        (this.Value, otherNum.Value) switch
        {
            (var a, var b) when a == b => CompareResult.Equal,
            (var a, var b) when a < b => CompareResult.Correct,
            (var a, var b) when a > b => CompareResult.Incorrect,
            _ => throw new InvalidOperationException()
        };
}

record ListItem(List<Item> Items) : Item
{
    public override string ToString()
    {
        return $"ListItem. Count: {Items.Count}";
    }

    public CompareResult CompareListItems(ListItem otherList)
    {
        var iter = Items.Count < otherList.Items.Count ? Items.Count : otherList.Items.Count;

        for (int i = 0; i < iter; i++)
        {
            var cr = Items[i].CompareTo(otherList.Items[i]);
            if (cr != CompareResult.Equal)
                return cr;
        }

        if (Items.Count < otherList.Items.Count) return CompareResult.Correct;
        if (Items.Count > otherList.Items.Count) return CompareResult.Incorrect;

        return CompareResult.Equal;
    }
}

record Item
{
    public CompareResult CompareTo(Item otherNum) =>
        (this, otherNum) switch
        {
            (ListItem left, ListItem right) => left.CompareListItems(right),
            (NumberItem left, NumberItem right) => left.CompareNumberItems(right),
            (ListItem left, NumberItem right) => left.CompareListItems(right.ToListItem()),
            (NumberItem left, ListItem right) => left.ToListItem().CompareListItems(right),
            _ => throw new InvalidOperationException()
        };
}

enum CompareResult
{
    Equal,
    Correct,
    Incorrect
}