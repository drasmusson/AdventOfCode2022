// https://adventofcode.com/2022/day/20

var input = File.ReadAllLines("Day20.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var originalOrder = GetNumbers(input);
    var currentOrder = originalOrder.Select(x => x).ToList();
    Decrypt(originalOrder, currentOrder);
    var answer = GetAnswer(originalOrder, currentOrder);

    Console.WriteLine($"Part one: {answer}");
}

void PartTwo(string[] input)
{
    var originalOrder = GetNumbers(input, 811589153);
    var currentOrder = originalOrder.Select(x => x).ToList();

    for (int i = 0; i < 10; i++)
    {
        Decrypt(originalOrder, currentOrder);
    }
    var answer = GetAnswer(originalOrder, currentOrder);

    Console.WriteLine($"Part two: {answer}");
}

List<Number> GetNumbers(string[] input, long? decryptionKey = null)
{
    if (decryptionKey is null)
        return input.Select(x => new Number(long.Parse(x))).ToList();

    return input.Select(x => new Number(long.Parse(x) * decryptionKey.Value)).ToList();
}

void Decrypt(List<Number> originalOrder, List<Number> currentOrder)
{
    foreach (var number in originalOrder)
    {
        var index = currentOrder.IndexOf(number);
        MoveNumber(currentOrder, index);
    }
}

static long GetAnswer(List<Number> originalOrder, List<Number> currentOrder)
{
    var zeroIndex = currentOrder.IndexOf(originalOrder.First(x => x.Value == 0));

    var answer = currentOrder[(1000 + zeroIndex) % currentOrder.Count].Value +
        currentOrder[(2000 + zeroIndex) % currentOrder.Count].Value +
        currentOrder[(3000 + zeroIndex) % currentOrder.Count].Value;
    return answer;
}

void MoveNumber(List<Number> list, int oldIndex)
{
    var temp = list[oldIndex];
    int newIndex = (int)((oldIndex + temp.Value) % (list.Count - 1));

    if (newIndex <= 0 && oldIndex + temp.Value != 0)
    {
        newIndex = list.Count - 1 + newIndex;
    }

    list.RemoveAt(oldIndex);
    list.Insert(newIndex, temp);
}

class Number
{
    public long Value { get; }

    public Number(long value)
    {
        Value = value;
    }
}