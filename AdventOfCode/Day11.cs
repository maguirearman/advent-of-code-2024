using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day11 : BaseDay
{
    private readonly string _input;

    public Day11()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    static ConcurrentDictionary<string, List<string>> memorization = new ConcurrentDictionary<string, List<string>>();

    static List<string> Blink(List<string> stones)
    {
        // Group stones to reduce redundant processing
        var groupedStones = stones.GroupBy(s => s).ToDictionary(g => g.Key, g => g.Count());
        ConcurrentBag<string> newStones = new ConcurrentBag<string>();

        Parallel.ForEach(groupedStones, stoneGroup =>
        {
            string stone = stoneGroup.Key;
            int count = stoneGroup.Value;

            //skip stabilized values
            if (stone == "1")
            {
                for (int i = 0; i < count; i++)
                    newStones.Add("1");
                return;
            }

            //get or compute the transformation
            List<string> result = memorization.GetOrAdd(stone, ProcessStone);

            //add the result to the output for each occurrence
            foreach (var res in result)
            {
                for (int i = 0; i < count; i++)
                {
                    newStones.Add(res);
                }
            }
        });

        return newStones.ToList();
    }

    static List<string> ProcessStone(string stone)
    {
        if (stone == "0")
        {
            return new List<string> { "1" };
        }
        else if (stone.Length % 2 == 0)
        {
            int middle = stone.Length / 2;
            string left = stone.Substring(0, middle).TrimStart('0');
            string right = stone.Substring(middle).TrimStart('0');

            return new List<string>
            {
                string.IsNullOrEmpty(left) ? "0" : left,
                string.IsNullOrEmpty(right) ? "0" : right
            };
        }
        else
        {
            long number = long.Parse(stone);
            return new List<string> { (number * 2024).ToString() };
        }
    }

    public override ValueTask<string> Solve_1()
    {
        List<string> stones = _input.Split(' ').ToList();
        int blinks = 25;

        for (int i = 0; i < blinks; i++)
        {
            stones = Blink(stones);
        }
        return new(stones.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        List<string> stones = _input.Split(' ').ToList();
        int blinks = 75;

        for (int i = 0; i < blinks; i++)
        {
            stones = Blink(stones);
            Console.WriteLine($"After {i + 1} blink(s): {stones.Count} stones");
        }

        return new(stones.Count.ToString());
    }
}
