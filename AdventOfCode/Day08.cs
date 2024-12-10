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

        foreach (char freq in antennas.Keys)
        {
            {
                List<(int x, int y)> positions = antennas[freq];


                for (int i = 0; i < positions.Count; i++)
                {
                    for (int j = i + 1; j < positions.Count; j++)
                    {
                        var (x1, y1) = positions[i];
                        var (x2, y2) = positions[j];

                        Console.WriteLine($"Checking pair: ({x1},{y1}) and ({x2},{y2})");

                        int dx = x2 - x1;
                        int dy = y2 - y1;

                        Console.WriteLine($"dx: {dx}, dy: {dy}");

                        // Check if the pair satisfies the twice-as-far rule
                        if ((dx == 2 * dy) || (dy == 2 * dx))
                        {
                            Console.WriteLine("Pair satisfies twice-as-far condition.");

                            int midX = (x1 + x2) / 2;
                            int midY = (y1 + y2) / 2;

                            Console.WriteLine($"Midpoint calculated: ({midX}, {midY})");

                            if (midX >= 0 && midX < _input[0].Length && midY >= 0 && midY < _input.Length)
                            {
                                Console.WriteLine($"Adding to antinodePositions: ({midX}, {midY})");
                                antinodePositions.Add((midX, midY));
                            }
                            else
                            {
                                Console.WriteLine($"Midpoint out of bounds: ({midX}, {midY})");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Pair does NOT satisfy twice-as-far condition.");
                        }
                    }
                }

            }
        }
        return new(antinodePositions.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new();
    }
}