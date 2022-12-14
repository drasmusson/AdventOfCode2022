// https://adventofcode.com/2022/day/13

var input = File.ReadAllLines("Day13.txt");

PartOne(input);

void PartOne(string[] input)
{
    var pairs = GetPairs(input);

    var indexes = SolveProblem(pairs);

    var answer = indexes.Sum();
    Console.WriteLine($"Part one: {answer}");
}

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

List<int> SolveProblem(List<(string Left, string Right)> pairs)
{
    var indexes = new List<int>();

    for (int i = 0; i < pairs.Count; i++)
    {
        if (Compare(pairs[i].Left, pairs[i].Right).Value)
            indexes.Add(i + 1);
    }

    return indexes;
}

bool ComparePair((string Left, string Right) pair)
{
    var originalPair = pair;

    if (IsList(pair.Left) && IsList(pair.Right))
    {

    }
    return true;
}
bool? Compare(string left, string right)
{
    var leftIsList = IsList(left);
    var rightIsList = IsList(right);

    if (leftIsList && rightIsList)
    {
        var subpackageLeft = GetFirstSubPackage(left);
        var subpackageRight = GetFirstSubPackage(right);

        var compareResult = Compare(GetFirstSubPackage(left), GetFirstSubPackage(right));
        if (compareResult != null) return compareResult;

        var lSubIndex = left.IndexOf(subpackageLeft);
        var rSubIndex = right.IndexOf(subpackageRight);

        left = left.Remove(lSubIndex - 1, subpackageLeft.Length + 2);
        right = right.Remove(rSubIndex - 1, subpackageRight.Length + 2);
    }

    if (leftIsList && !rightIsList)
    {
        var rC = ConvertToList(right);
        var subpackageLeft = GetFirstSubPackage(left);
        var subpackageRight = GetFirstSubPackage(right);

        var compareResult = Compare(GetFirstSubPackage(left), GetFirstSubPackage(right));
        if (compareResult != null) return compareResult;

        var lSubIndex = left.IndexOf(subpackageLeft);
        var rSubIndex = right.IndexOf(subpackageRight);

        left = left.Remove(lSubIndex - 1, subpackageLeft.Length + 2);
        right = right.Remove(rSubIndex - 1, subpackageRight.Length + 2);
    }

    if (!leftIsList && rightIsList)
    {
        var lC = ConvertToList(left);
        var subpackageLeft = GetFirstSubPackage(left);
        var subpackageRight = GetFirstSubPackage(right);

        var compareResult = Compare(GetFirstSubPackage(left), GetFirstSubPackage(right));
        if (compareResult != null) return compareResult;

        var lSubIndex = left.IndexOf(subpackageLeft);
        var rSubIndex = right.IndexOf(subpackageRight);

        left = left.Remove(lSubIndex - 1, subpackageLeft.Length + 2);
        right = right.Remove(rSubIndex - 1, subpackageRight.Length + 2); ;
    }

    var leftList = left.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
    var rightList = right.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
    while (leftList.Any() && rightList.Any())
    {
        var lC = leftList.First();
        var rC = rightList.First();

        if (IsList(lC) || IsList(rC))
        {
            left = String.Join(",", leftList);
            right = String.Join(",", rightList);

            var compareResult = Compare(left, right);
            if (compareResult != null) return compareResult;
        }

        var l = int.Parse(leftList.First());
        var r = int.Parse(rightList.First());

        if (l < r) return true;

        if (l > r) return false;

        leftList.RemoveAt(0);
        rightList.RemoveAt(0);
    }

    if (!leftList.Any() && !rightList.Any()) return null;

    return rightList.Any();
}

string ConvertToList(string input)
{
    return string.IsNullOrEmpty(input) ? $"[]" : $"[{input[0]}]";
}

string GetFirstSubPackage(string package)
{
    if (string.IsNullOrEmpty(package) || package[0] != '[') return package;

    var startBrackets = 0;
    var i = 0;
    do
    {
        var c = package[i];

        switch (c)
        {
            case '[':
                startBrackets++;
                break;
            case ']':
                startBrackets--;
                break;
            default:
                break;
        }
        i++;
    } while (startBrackets != 0);

    return package.Substring(1, i - 2);
}

static string RemoveOuterBrackets(string package) => package.Substring(1, package.Length - 2);

static bool IsList(string package) => !string.IsNullOrEmpty(package) && package[0] == '[';


