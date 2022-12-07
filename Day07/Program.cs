// https://adventofcode.com/2022/day/7

var input = File.ReadAllLines("Day07.txt").ToList();

PartOne(input);

void PartOne(List<string> input)
{
    input.RemoveAt(0);

    var currentDir = new Dir("/", null);
    var isDisplayMode = false;
    foreach (var cmdLine in input)
	{
        if (CmdLineIsLsCommand(cmdLine))
        {
            isDisplayMode = true;
            continue;
        }

        if (isDisplayMode && CmdLineIsCommand(cmdLine))
            isDisplayMode = false;

        if(isDisplayMode)
        {
            if (CmdLineIsDir(cmdLine))
                currentDir.AddDir(new Dir(cmdLine.Split("dir ")[1], currentDir));

            if (CmdLineIsDoc(cmdLine))
            {
                var docSplit = cmdLine.Split(" ");
                currentDir.AddDoc(new Doc(long.Parse(docSplit[0]), docSplit[1]));
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
    var allDirs = mainDir.GetChildDirs();

    var smallDirs = allDirs.Where(x => x.Size <= 100000).ToList();
    var smallDirsSizes = smallDirs.Select(x => x.Size).Sum();
    
    Console.Write($"Part one: {smallDirsSizes}");
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
    public List<Doc> Docs { get; }
    public Dir? ParentDir { get; }
    public string Name { get; }
    public long Size
    {
        get
        {
            var dirSize = GetChildDirs().Select(x => x.Size).Sum();
            var docSize = Docs.Select(x => x.Size).Sum();
            return docSize + dirSize;
        }
    }

    public Dir(string name, Dir? parentFolder)
    {
        Dirs = new List<Dir>();
        Docs = new List<Doc>();
        Name = name;
        ParentDir = parentFolder;
    }

    internal void AddDir(Dir dir)
    {
        Dirs.Add(dir);
    }
    internal void AddDoc(Doc doc)
    {
        Docs.Add(doc);
    }

    internal Dir GetDir(string dirName)
    {
        return Dirs.FirstOrDefault(x => x.Name == dirName);
    }

    internal Dir GetParentDir()
    {
        return ParentDir;
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

public class Doc
{
    public long Size { get; }
    public string Name { get; }

    public Doc(long size, string name)
    {
        Size = size;
        Name = name;
    }
}