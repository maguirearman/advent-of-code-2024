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

    private int a; // Number of times Button A is pressed
    private int b; // Number of times Button B is pressed
    private int minCost = int.MaxValue; // To track the minimum token cost across all machines
    private bool solvable = false;
    private int totalTokens = 0;

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

        int prizesWon = 0;
        foreach (var machine in clawMachines)
        {
            Console.WriteLine($"Attempted solve for prize X={machine.PrizeX}, Y={machine.PrizeY}");

            if (SolveLinearSystem(machine))
            {
                prizesWon++;
                Console.WriteLine($"Tokens spent for this machine: {minCost}");
            }
        }

        Console.WriteLine($"\nTotal prizes won: {prizesWon}");
        Console.WriteLine($"Minimum tokens spent: {totalTokens}");

        return new(totalTokens.ToString());
    }
    private bool SolveLinearSystem(ClawMachine machine)
    {
        double[,] A = {
        { machine.AX, machine.BX },
        { machine.AY, machine.BY }
    };

        double[] B = {
        machine.PrizeX,
        machine.PrizeY
    };


        if (IsSystemSolvable(A))
        {
            try
            {
                var inverseA = InvertMatrix(A);
                double[] solution = MultiplyMatrixVector(inverseA, B);

                // Clamp and round the solution
                int computedA = ClampToValidRange((int)Math.Round(solution[0]));
                int computedB = ClampToValidRange((int)Math.Round(solution[1]));

                int cost = 3 * computedA + computedB;

                if (cost < minCost)
                {
                    minCost = cost;
                    a = computedA;
                    b = computedB;
                    totalTokens = minCost + totalTokens;
                    solvable = true;
                    Console.WriteLine($"Optimal presses found: A={computedA}, B={computedB}");
                    Console.WriteLine($"Token cost: {cost}");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error solving system: {ex.Message}");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Linear system is not solvable.");
            return false;
        }
    }

    private bool IsSystemSolvable(double[,] A)
    {
        double determinant = A[0, 0] * A[1, 1] - A[1, 0] * A[0, 1];
        Console.WriteLine($"Determinant: {determinant}");
        return Math.Abs(determinant) > 1e-10; // Ensure determinant is valid
    }

    private double[] SolveMatrixEquation(double[,] A, double[] B)
    {
        try
        {
            double[,] inverseMatrix = InvertMatrix(A);
            return MultiplyMatrixVector(inverseMatrix, B);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error solving the system: {ex.Message}");
            return null;
        }
    }

    private double[,] InvertMatrix(double[,] A)
    {
        double determinant = A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0];
        double invDet = 1.0 / determinant;

        return new double[,]
        {
        { A[1, 1] * invDet, -A[0, 1] * invDet },
        { -A[1, 0] * invDet, A[0, 0] * invDet }
        };
    }

    private double[] MultiplyMatrixVector(double[,] matrix, double[] vector)
    {
        return new double[]
        {
        matrix[0, 0] * vector[0] + matrix[0, 1] * vector[1],
        matrix[1, 0] * vector[0] + matrix[1, 1] * vector[1]
        };
    }

    private int ClampToValidRange(int value)
    {
        return Math.Clamp(value, 0, 100_000_000);
    }
}

