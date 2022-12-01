// https://adventofcode.com/2022/day/1

PartOne();
PartTwo();

void PartOne()
{
    var lines = File.ReadAllLines("Day01.txt");

    var highestCountYet = 0;
    var currentCount = 0;

    foreach (var calories in lines)
    {
        if (calories == "")
        {
            if (currentCount > highestCountYet)
                highestCountYet = currentCount;

            currentCount = 0;
        }
        else
        {
            var caloriesInt = int.Parse(calories);
            currentCount += caloriesInt;
        }
    }
    
    Console.WriteLine("Part one: " + highestCountYet);
}

void PartTwo()
{
    var lines = File.ReadAllLines("Day01.txt");

    var totalCalorieCounts = new List<int>();
    var currentCount = 0;

    foreach (var calories in lines)
    {
        if (calories == "")
        {
            totalCalorieCounts.Add(currentCount);
            currentCount = 0;
        }
        else
        {
            var caloriesInt = int.Parse(calories);
            currentCount += caloriesInt;
        }
    }

    totalCalorieCounts.Sort();
    totalCalorieCounts.Reverse();

    var top3 = totalCalorieCounts.Take(3).Sum();
    Console.WriteLine("Part two: " + top3);
}


