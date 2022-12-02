// https://adventofcode.com/2022/day/2

var games = File.ReadAllLines("Day02.txt");

PartOne(games);
PartTwo(games);

void PartOne(string[] games)
{
	var total = 0;

	foreach (var gameString in games)
	{
		var game = (PlayParse(gameString[0]), PlayParse(gameString[2]));
		var playScore = GetPlayScore(game.Item2);
        var outcome = PlayGame(game);
        var roundResult = GetOutcomeScore(outcome);

        var totalRoundScore = playScore + roundResult;
		total += totalRoundScore;
	}

	Console.WriteLine("Part one: " + total);
}

void PartTwo(string[] games)
{
	var total = 0;

	foreach (var gameString in games)
	{
		var game = (PlayParse(gameString[0]), OutcomeParse(gameString[2]));
		var play = GetPlay(game);
		var playScore = GetPlayScore(play);
		var roundResult = GetOutcomeScore(game.Item2);

		var totalRoundScore = playScore + roundResult;
		total += totalRoundScore;
	}

	Console.WriteLine("Part two: " + total);
}

static Outcome PlayGame((Play p1, Play p2) game) => game switch
{
	{ p1: Play.Rock, p2: Play.Rock } => Outcome.Draw,
	{ p1: Play.Rock, p2: Play.Paper } => Outcome.Win,
	{ p1: Play.Rock, p2: Play.Scissor } => Outcome.Loss,

	{ p1: Play.Paper, p2: Play.Rock } => Outcome.Loss,
	{ p1: Play.Paper, p2: Play.Paper } => Outcome.Draw,
	{ p1: Play.Paper, p2: Play.Scissor } => Outcome.Win,

    { p1: Play.Scissor, p2: Play.Rock } => Outcome.Win,
    { p1: Play.Scissor, p2: Play.Paper } => Outcome.Loss,
    { p1: Play.Scissor, p2: Play.Scissor } => Outcome.Draw
};

static int GetOutcomeScore(Outcome outcome) => outcome switch
{
    Outcome.Loss => 0,
    Outcome.Draw => 3,
    Outcome.Win => 6
};

static Play GetPlay((Play p1, Outcome outcome) gameInstructions)
{
	switch (gameInstructions.outcome)
	{
		case Outcome.Loss:
			return GetLosingPlay(gameInstructions.p1);
        case Outcome.Draw:
            return GetDrawPlay(gameInstructions.p1);
        case Outcome.Win:
            return GetWinningPlay(gameInstructions.p1);
        default:
			return default;
	}
}

static Play GetLosingPlay(Play p1) => p1 switch
{
	Play.Rock => Play.Scissor,
	Play.Paper => Play.Rock,
	Play.Scissor => Play.Paper
};

static Play GetDrawPlay(Play p1) => p1 switch
{
    Play.Rock => Play.Rock,
    Play.Paper => Play.Paper,
    Play.Scissor => Play.Scissor
};

static Play GetWinningPlay(Play p1) => p1 switch
{
    Play.Rock => Play.Paper,
    Play.Paper => Play.Scissor,
    Play.Scissor => Play.Rock
};

static int GetPlayScore(Play v)
{
    switch (v)
	{
		case Play.Rock:
			return 1;
		case Play.Paper:
			return 2;
		case Play.Scissor:
			return 3;
		default: return 0;
	}
}
static Play PlayParse(char input) => input switch
{
	'A' => Play.Rock,
	'B' => Play.Paper,
	'C' => Play.Scissor,

	'X' => Play.Rock,
	'Y' => Play.Paper,
	'Z' => Play.Scissor
};

static Outcome OutcomeParse(char input) => input switch
{
    'X' => Outcome.Loss,
    'Y' => Outcome.Draw,
    'Z' => Outcome.Win
};

enum Play
{
	Rock,
	Paper,
	Scissor
}

enum Outcome
{
	Win,
	Loss,
	Draw
}