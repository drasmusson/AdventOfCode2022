// https://adventofcode.com/2022/day/7

var input = File.ReadAllLines("Day07.txt").ToList();

PartOne(input);
PartTwo(input);

void PartOne(List<string> input)
{
    var mainDir = ParseDir(input);
    var allDirs = mainDir.GetChildDirs();

    var smallDirs = allDirs.Where(x => x.Size <= 100000).ToList();
    var smallDirsSizes = smallDirs.Select(x => x.Size).Sum();
    
    Console.WriteLine($"Part one: {smallDirsSizes}");
}

void PartTwo(List<string> input)
{
    Dir mainDir = ParseDir(input);
    var spaceToBeCleared = 30000000 - (70000000 - mainDir.Size);
    var allDirs = mainDir.GetChildDirs();

    var sizeToRemove = allDirs.Where(x => x.Size >= spaceToBeCleared).Min(x => x.Size);

    Console.WriteLine($"Part two: {sizeToRemove}");
}

Dir ParseDir(List<string> input)
{
    var currentDir = new Dir("/", null);
    var isDisplayMode = false;
    for (int i = 1; i < input.Count; i++)
    {
        var cmdLine = input[i];
        
        if (CmdLineIsLsCommand(cmdLine))
        {
            isDisplayMode = true;
            continue;
        }

        if (isDisplayMode && CmdLineIsCommand(cmdLine))
            isDisplayMode = false;

        if (isDisplayMode)
        {
            if (CmdLineIsDir(cmdLine))
                currentDir.AddDir(new Dir(cmdLine.Split("dir ")[1], currentDir));

            if (CmdLineIsDoc(cmdLine))
            {
                var docSplit = cmdLine.Split(" ");
                var size = long.Parse(docSplit[0]);
                currentDir.AddSizeToThisAndParents(size);
            }
        }

        if (CmdLineIsCdCommand(cmdLine))
        {
            if (CmdLineIsMoveOut(cmdLine))
                currentDir = currentDir.GetParentDir();
            else
                currentDir = currentDir.GetDir(cmdLine.Split("cd ")[1]);
        }
    }
    var mainDir = currentDir.GetOutermostDir();
    return mainDir;
}

bool CmdLineIsMoveOut(string cmdLine) => cmdLine == "$ cd ..";

bool CmdLineIsCdCommand(string cmdLine) => cmdLine.StartsWith("$ cd");

bool CmdLineIsCommand(string cmdLine) => cmdLine[0] == '$';

bool CmdLineIsDoc(string cmdLine) => long.TryParse(cmdLine.Split(" ")[0], out _);

bool CmdLineIsDir(string cmdLine) => cmdLine.StartsWith("dir");

bool CmdLineIsLsCommand(string cmdLine) => cmdLine == "$ ls";

public class Dir
{
    public List<Dir> Dirs { get; }
    public Dir? ParentDir { get; }
    public string Name { get; }
    public long Size { get; internal set;  }

    public Dir(string name, Dir? parentFolder)
    {
        Dirs = new List<Dir>();
        Name = name;
        ParentDir = parentFolder;
    }

    internal void AddDir(Dir dir)
    {
        Dirs.Add(dir);
    }

    internal Dir GetDir(string dirName)
    {
        return Dirs.FirstOrDefault(x => x.Name == dirName);
    }

    internal Dir GetParentDir()
    {
        return ParentDir;
    }

    internal void AddSizeToThisAndParents(long size)
    {
        Size += size;

        if (ParentDir is null)
            return;

        ParentDir.AddSizeToThisAndParents(size);
    }

    internal Dir GetOutermostDir()
    {
        if (ParentDir == null)
            return this;

        return ParentDir.GetOutermostDir();
    }

    internal List<Dir> GetChildDirs()
    {
        var dirs = Dirs;
        var childDirs = Dirs.SelectMany(x => x.GetChildDirs()).ToList();
        dirs.AddRange(childDirs);
        return dirs;
    }
}