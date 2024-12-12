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

    public Dictionary<string, long> Blink(Dictionary<string, long> currentDict)
    {
        var newDict = new Dictionary<string, long>();

        foreach (var entry in currentDict)
        {
            string stone = entry.Key;
            long count = entry.Value;

            //transform the stone using cached or computed result
            var transformedStones = memorization.GetOrAdd(stone, ProcessStone);

            //accumulate the counts for each transformed stone
            foreach (var transformedStone in transformedStones)
            {
                if (!newDict.ContainsKey(transformedStone))
                {
                    newDict[transformedStone] = 0;
                }
                newDict[transformedStone] += count;
            }
        }

        return newDict;
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
        // Parse input into an initial dictionary
        var stones = _input.Split(' ').GroupBy(s => s)
                           .ToDictionary(g => g.Key, g => (long)g.Count());

        int blinks = 25;

        for (int i = 0; i < blinks; i++)
        {
            stones = Blink(stones);
        }

        long totalStones = stones.Values.Sum();
        return new(totalStones.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        // Parse input into an initial dictionary
        var stones = _input.Split(' ').GroupBy(s => s)
                           .ToDictionary(g => g.Key, g => (long)g.Count());

        int blinks = 75;

        for (int i = 0; i < blinks; i++)
        {
            stones = Blink(stones);
        }

        long totalStones = stones.Values.Sum();
        return new(totalStones.ToString());
    }
}
