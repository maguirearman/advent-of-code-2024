using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day08 : BaseDay
{
    private readonly string[] _input;

    public Day08()
    {
        _input = File.ReadAllLines(InputFilePath);
    }
    public (int x, int y) WalkAntinode((int x, int y) start, (int x, int y) other)
    {
        int dx = other.x - start.x;
        int dy = other.y - start.y;

        //scale movement by 2 (double the distance)
        return (start.x + 2 * dx, start.y + 2 * dy);
    }

    bool IsWithinBounds((int x, int y) pos, string[] map)
    {
        return pos.x >= 0 && pos.x < map[0].Length && pos.y >= 0 && pos.y < map.Length;
    }

    public override ValueTask<string> Solve_1()
    {
        Dictionary<char, List<(int x, int y)>> antennas = new Dictionary<char, List<(int x, int y)>>();

        //store frequencies in antenna dict
        for (int y = 0; y < _input.Length; y++)
        {
            for (int x = 0; x < _input[y].Length; x++)
            {
                char ch = _input[y][x];

                //skip empty cells
                if (ch == '.') continue;

                //initialize the list for a new frequency if it doesn't exist
                if (!antennas.ContainsKey(ch))
                {
                    antennas[ch] = new List<(int x, int y)>();
                }

                //add the antenna's position
                antennas[ch].Add((x, y));
            }
        }

        // Debugging: print the parsed antennas
        foreach (var kvp in antennas)
        {
            Console.WriteLine($"Frequency '{kvp.Key}': Positions {string.Join(", ", kvp.Value)}");
        }

        //set to store unique antinode positions
        HashSet<(int x, int y)> antinodePositions = new HashSet<(int x, int y)>();

        foreach (var kvp in antennas)
        {
            List<(int x, int y)> positions = kvp.Value;

            for (int i = 0; i < positions.Count; i++)
            {
                for (int j = i + 1; j < positions.Count; j++)
                {
                    var point1 = positions[i];
                    var point2 = positions[j];

                    Console.WriteLine($"Checking antenna pair: ({point1.x}, {point1.y}) - ({point2.x}, {point2.y})");

                    //walk along diagonal direction (point1 -> point2)
                    var antinode1 = WalkAntinode(point1, point2);
                    if (IsWithinBounds(antinode1, _input))
                    {
                        Console.WriteLine($"Adding antinode at: ({antinode1.x}, {antinode1.y})");
                        antinodePositions.Add(antinode1);
                    }

                    //walk along diagonal direction (point2 -> point1) — compute the reverse walk
                    var antinode2 = WalkAntinode(point2, point1);
                    if (IsWithinBounds(antinode2, _input))
                    {
                        Console.WriteLine($"Adding antinode at: ({antinode2.x}, {antinode2.y})");
                        antinodePositions.Add(antinode2);
                    }
                }
            }
        }

        //create a visual map with dimensions matching the input
        char[,] visualMap = new char[_input.Length, _input[0].Length];

        //initialize the map with the input's characters directly
        for (int y = 0; y < _input.Length; y++)
        {
            for (int x = 0; x < _input[y].Length; x++)
            {
                visualMap[y, x] = _input[y][x];
            }
        }


        return new(antinodePositions.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new();
    }
}