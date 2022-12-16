// https://adventofcode.com/2022/day/15

var input = File.ReadAllLines("Day15.txt");

//PartOne(input);
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
	Console.WriteLine($"Part two: {9}");
}

Coord FindBeacon(List<Sensor> sensors)
{
	foreach (var sensor in sensors)
	{
		foreach (var edgeCoord in sensor.GetEdgeCoords(0, 20))
		{
			if (NotInRange(sensors, edgeCoord))
			{
				return edgeCoord;
			}
		}
	}
	return new Coord(0, 0);
}

bool NotInRange(List<Sensor> sensors, Coord coord)
{
	var withinRange = true;

	foreach (var sensor in sensors)
	{
		if (!sensor.IsCoordWithinRange(coord))
		{
            withinRange = false;
		}
		else
		{
			withinRange = true;
		}
	}
	return withinRange;
}

long CalculateTuningFrequency(Coord beacon)
{
	return beacon.X * 4000000 + beacon.Y;
}

//Coord Search(List<Coord> coordsToSearchFor, List<Sensor> sensors)
//{
//	var coordFound = false;
//	var beacon = new Coord(0, 0);
//	while (!coordFound)
//	{
//		var coordToSearchFor = coordsToSearchFor.First();

//        foreach (var sensor in sensors)
//        {
//            if (!sensor.CoveredCoords.Contains(coordToSearchFor)) 
//			{
//				beacon = coordToSearchFor;
//				coordFound = true;
//			}
//			else
//			{
//				coordsToSearchFor.Remove(coordToSearchFor);
//				coordFound = false;
//				break;
//			}
//        }
//	}
//	return beacon;
//}

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

//List<Coord> GetAllPossibleCoords(int min, int max)
//{
//    var coords = new List<Coord>();

//    for (int x = min; x <= max; x++)
//    {
//        for (int y = min; y <= max; y++)
//        {
//			coords.Add(new Coord(x, y));
//        }
//    }
//	return coords;
//}

record Coord(int X, int Y)
{
	public int ManhattanDistanceFromCoord(Coord other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
}

class Sensor
{
	public Coord ClosestBeacon { get; private set; }
	//public List<Coord> CoveredCoords { get; private set; }
	private Coord Position { get; set; }
	private int Range { get; set; }
	public Sensor(int sensorX, int sensorY, int beaconX, int beaconY)
	{
		Position = new Coord(sensorX, sensorY);
        ClosestBeacon = new Coord(beaconX, beaconY);
		Range = Position.ManhattanDistanceFromCoord(ClosestBeacon);
		//CoveredCoords = GetCoveredCoords();
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

	public List<Coord> GetEdgeCoords(int min, int max)
	{
		var edgeCoords = new List<Coord>();

		var edgeDistance = Range + 1;

		for (int i = -edgeDistance; i <= edgeDistance; i++)
		{
			var y1 = Position.Y - edgeDistance;
			var y2 = Position.Y + edgeDistance;
			var x = Position.X + i;

            if (y1 >= min && y1 <= max &&
				y2 >= min && y2 <= max &&
                x >= min && x <= max)
			{
				edgeCoords.Add(new Coord(x, y1));
				edgeCoords.Add(new Coord(x, y2));
			}
		}

		for (int i = -edgeDistance; i < edgeDistance; i++)
		{
            var x1 = Position.X - edgeDistance;
            var x2 = Position.X + edgeDistance;
            var y = Position.Y + i;

            if (x1 >= min && x1 <= max &&
                x2 >= min && x2 <= max &&
                y >= min && y <= max)
            {
                edgeCoords.Add(new Coord(x1, y));
                edgeCoords.Add(new Coord(x2, y));
            }
        }

		return edgeCoords;
	}

    public bool IsCoordWithinRange(Coord other)
    {
		return Position.ManhattanDistanceFromCoord(other) <= Range;
    }


    //List<Coord> GetCoveredCoords()
    //{
    //       var coordinates = new List<Coord>();
    //	for (int x = Position.X - Range; x <= Position.X + Range; x++)
    //	{
    //		for (int y = Position.Y - Range; y <= Position.Y + Range; y++)
    //		{
    //			var coordsoTest = new Coord(x, y);
    //			if (IsWithinRange(coordsoTest))
    //			{
    //                   coordinates.Add(coordsoTest);
    //               }
    //           }
    //       }

    //       return coordinates;
    //   }

}