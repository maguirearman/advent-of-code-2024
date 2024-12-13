using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day13 : BaseDay
{
    private readonly string _input;

    public Day13()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    class ClawMachine
    {
        public int AX { get; set; }
        public int AY { get; set; }
        public int BX { get; set; }
        public int BY { get; set; }
        public long PrizeX { get; set; }
        public long PrizeY { get; set; }
    }

    public override ValueTask<string> Solve_1()
    {
        var clawMachines = new List<ClawMachine>();
        var regex = new Regex(@"Button A: X\+(\d+), Y\+(\d+).*?Button B: X\+(\d+), Y\+(\d+).*?Prize: X=(\d+), Y=(\d+)", RegexOptions.Singleline);

        //parse input into claw machine objects
        foreach (Match match in regex.Matches(_input))
        {
            clawMachines.Add(new ClawMachine
            {
                AX = int.Parse(match.Groups[1].Value),
                AY = int.Parse(match.Groups[2].Value),
                BX = int.Parse(match.Groups[3].Value),
                BY = int.Parse(match.Groups[4].Value),
                PrizeX = long.Parse(match.Groups[5].Value),
                PrizeY = long.Parse(match.Groups[6].Value)
            });
        }

        int totalTokens = 0;
        int prizesWon = 0;

        //solve for each claw machine
        for (int i = 0; i < clawMachines.Count; i++)
        {
            var machine = clawMachines[i];
            int minCost = int.MaxValue;
            int bestA = 0, bestB = 0;
            bool solvable = false;

            //go through all combinations of clicks under 100
            for (int a = 0; a <= 100; a++)
            {
                for (int b = 0; b <= 100; b++)
                {
                    //if the coordinates are on the prize
                    if (a * machine.AX + b * machine.BX == machine.PrizeX &&
                        a * machine.AY + b * machine.BY == machine.PrizeY)
                    {
                        //calc cost and see if it is the cheapest so far
                        int cost = 3 * a + b;
                        if (cost < minCost)
                        {
                            minCost = cost;
                            bestA = a;
                            bestB = b;
                            solvable = true;
                        }
                    }
                }
            }
            if (solvable)
            {
                prizesWon++;
                totalTokens += minCost;
                Console.WriteLine($"Machine {i + 1}: Solvable with cost {minCost} tokens (A: {bestA}, B: {bestB})");
            }

        }

        Console.WriteLine($"\nTotal prizes won: {prizesWon}");
        Console.WriteLine($"Minimum tokens spent: {totalTokens}");

        return new(totalTokens.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var clawMachines = new List<ClawMachine>();
        var regex = new Regex(@"Button A: X\+(\d+), Y\+(\d+).*?Button B: X\+(\d+), Y\+(\d+).*?Prize: X=(\d+), Y=(\d+)", RegexOptions.Singleline);

        //parse input into claw machine objects
        foreach (Match match in regex.Matches(_input))
        {
            clawMachines.Add(new ClawMachine
            {
                AX = int.Parse(match.Groups[1].Value),
                AY = int.Parse(match.Groups[2].Value),
                BX = int.Parse(match.Groups[3].Value),
                BY = int.Parse(match.Groups[4].Value),
                PrizeX = long.Parse(match.Groups[5].Value),
                PrizeY = long.Parse(match.Groups[6].Value)
            });
        }

        // Add 10^13 to the X and Y positions of all prizes
        foreach (var machine in clawMachines)
        {
            machine.PrizeX += 10000000000000;
            machine.PrizeY += 10000000000000;
        }

        int totalTokens = 0;
        int prizesWon = 0;

        //solve for each claw machine
        for (int i = 0; i < clawMachines.Count; i++)
        {
            var machine = clawMachines[i];

            int minCost = int.MaxValue;
            int bestA = 0, bestB = 0;
            bool solvable = false;

            long x1 = machine.AX, y1 = machine.AY;
            long x2 = machine.BX, y2 = machine.BY;
            long px = machine.PrizeX, py = machine.PrizeY;

            // Extended GCD for X and Y
            var gcdX = ExtendedGcd(x1, x2, out long sX, out long tX);
            var gcdY = ExtendedGcd(y1, y2, out long sY, out long tY);

            // Check if the solution is possible
            if (px % gcdX != 0 || py % gcdY != 0)
            {
                solvable = false;
            }

            // Scale solutions for target prize positions
            sX *= px / gcdX;
            tX *= px / gcdX;
            sY *= py / gcdY;
            tY *= py / gcdY;

            // Precompute step sizes for k
            long stepX = x2 / gcdX;
            long stepY = y2 / gcdY;

            // Find the bounds for valid `k`
            long kMinX = (long)Math.Ceiling(-(double)sX / stepX);
            long kMaxX = (long)Math.Floor((double)tX / stepX);
            long kMinY = (long)Math.Ceiling(-(double)sY / stepY);
            long kMaxY = (long)Math.Floor((double)tY / stepY);

            long kMin = Math.Max(kMinX, kMinY);
            long kMax = Math.Min(kMaxX, kMaxY);

            if (kMin > kMax)
            {
                solvable = false;
            }



            // Combine the solutions for X and Y
            for (long k = kMin; k <= kMax; k++)
            {
                long aX = sX + k * stepX;
                long bX = tX - k * (x1 / gcdX);

                long aY = sY + k * stepY;
                long bY = tY - k * (y1 / gcdY);

                if (aX >= 0 && bX >= 0 && aY >= 0 && bY >= 0 && aX == aY && bX == bY)
                {
                    int cost = (int)(3 * aX + bX);
                    minCost = cost;
                }
            }

            if (solvable)
            {
                prizesWon++;
                totalTokens += minCost;
                Console.WriteLine($"Machine {i + 1}: Solvable with cost {minCost} tokens (A: {bestA}, B: {bestB})");
            }

        }

        Console.WriteLine($"\nTotal prizes won: {prizesWon}");
        Console.WriteLine($"Minimum tokens spent: {totalTokens}");

        return new(totalTokens.ToString());
    }

    private static long ExtendedGcd(long a, long b, out long x, out long y)
    {
        if (b == 0)
        {
            x = 1;
            y = 0;
            return a;
        }

        long gcd = ExtendedGcd(b, a % b, out long x1, out long y1);
        x = y1;
        y = x1 - (a / b) * y1;
        return gcd;
    }
}

