// https://adventofcode.com/2022/day/15

var input = File.ReadAllLines("Day15.txt");

PartOne(input);
PartTwo(input);

void PartOne(string[] input)
{
    var sensors = ParseToSensors(input);
	var coordsOnRow = sensors.SelectMany(x => x.GetCoordsOnRowWithinRange(2000000)).ToHashSet();
	var beacons = sensors.Select(x => x.ClosestBeacon);
    var answer = coordsOnRow.Except(beacons).Count();
	Console.WriteLine($"Part one: {answer}");
}

void PartTwo(string[] input)
{
	var sensors = ParseToSensors(input);
	var beacon = FindBeacon(sensors);
	var answer = CalculateTuningFrequency(beacon);
	Console.WriteLine($"Part two: {answer}");
}

Coord FindBeacon(List<Sensor> sensors)
{
	foreach (var sensor in sensors)
	{
		foreach (var edgeCoord in sensor.GetEdgeCoords(0, 4000000))
		{
			if (!InRange(sensors, edgeCoord))
				return edgeCoord;
		}
	}
	return new Coord(0, 0);
}

bool InRange(List<Sensor> sensors, Coord coord)
{
	foreach (var sensor in sensors)
	{
		if (sensor.IsCoordWithinRange(coord))
			return true;
	}
	return false;
}

long CalculateTuningFrequency(Coord beacon)
{
	return (long)beacon.X * (long)4000000 + (long)beacon.Y;
}

List<Sensor> ParseToSensors(string[] input)
{
	var sensors = new List<Sensor>();

	foreach (var line in input)
	{
		var split = line.Split("=");
		var sensorX = int.Parse(split[1].Split(",")[0]);
		var sensorY = int.Parse(split[2].Split(":")[0]);
		var beaconX = int.Parse(split[3].Split(",")[0]);
		var beaconY = int.Parse(split[4]);

		sensors.Add(new Sensor(sensorX, sensorY, beaconX, beaconY));
    }
	return sensors;
}

record Coord(int X, int Y)
{
	public int ManhattanDistanceFromCoord(Coord other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
}

class Sensor
{
	public Coord ClosestBeacon { get; private set; }
	public int Range { get; private set; }
	private Coord Position { get; set; }
	public Sensor(int sensorX, int sensorY, int beaconX, int beaconY)
	{
		Position = new Coord(sensorX, sensorY);
        ClosestBeacon = new Coord(beaconX, beaconY);
		Range = Position.ManhattanDistanceFromCoord(ClosestBeacon);
	}

	public List<Coord> GetCoordsOnRowWithinRange(int row)
	{
		var minX = Position.X - Range;
		var maxX = Position.X + Range;

		var coords = new List<Coord>();

		for (int i = minX; i <= maxX; i++)
		{
			var coordToTest = new Coord(i, row);
			if (IsCoordWithinRange(coordToTest)) coords.Add(coordToTest);
		}
		return coords;
	}

	public IEnumerable<Coord> GetEdgeCoords(int min, int max)
	{
		var outerEdgeDist = Range + 1;

		int minx = Position.X - outerEdgeDist;
		int maxx = Position.X + outerEdgeDist;

        for (int i = minx; i <= maxx; i++)
        {
			var x = i;
			var y1 = Position.Y + (outerEdgeDist - Math.Abs(i - Position.X));
			var y2 = Position.Y - (outerEdgeDist - Math.Abs(i - Position.X));

			if (x >= min && x <= max &&
				y1 >= min && y1 <= max)
			{
				yield return new Coord(x, y1);
			}
			if (x >= min && x <= max &&
				y2 >= min && y2 <= max)
			{
				yield return new Coord(x, y2);
			}
        }
    }

    public bool IsCoordWithinRange(Coord other)
    {
		return Position.ManhattanDistanceFromCoord(other) <= Range;
    }
}