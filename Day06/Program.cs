// https://adventofcode.com/2022/day/6
var input = File.ReadAllText("Day06.txt");

PartOne(input);
PartTwo(input);

void PartOne(string input)
{
	var answer = 0;
    var chunkSize = 4;
	for (int i = 0; i < input.Length; i++)
	{
        var signalPart = input.Skip(i).Take(chunkSize);

        if (signalPart.Distinct().Count() == signalPart.Count())
		{
			answer = i + chunkSize;
			break;
		}
    }
    Console.WriteLine($"Part one: {answer}");
}

void PartTwo(string input)
{
    var answer = 0;
    var chunkSize = 14;

    for (int i = 0; i < input.Length; i++)
    {
        var signalPart = input.Skip(i).Take(chunkSize);

        if (signalPart.Distinct().Count() == signalPart.Count())
        {
            answer = i + chunkSize;
            break;
        }
    }
    Console.WriteLine($"Part two: {answer}");
}