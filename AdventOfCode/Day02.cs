using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string[] _input;

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath);
    }


    public override ValueTask<string> Solve_1()
    {
        int safeReports = 0;

        for (int i = 0; i < _input.Length; i++)
        {
            string currentLine = _input[i];
            List<int> currentReport = currentLine.Split(' ').Select(int.Parse).ToList();

            //check if the report is strictly increasing
            bool allIncreasing = currentReport.Zip(currentReport.Skip(1), (a, b) => b > a).All(isIncreasing => isIncreasing);

            //check if the report is strictly decreasing
            bool allDecreasing = currentReport.Zip(currentReport.Skip(1), (a, b) => b < a).All(isDecreasing => isDecreasing);

            //if it's either strictly increasing or strictly decreasing, proceed to check distances
            if (allIncreasing || allDecreasing)
            {
                bool isSafe = true;

                //check the distance between each adjacent pair
                for (int j = 0; j < currentReport.Count - 1; j++)
                {
                    int distance = Math.Abs(currentReport[j + 1] - currentReport[j]);
                    if (distance < 1 || distance > 3)
                    {
                        isSafe = false;
                        break;
                    }
                }

                if (isSafe)
                {
                    safeReports++;
                }
            }
        }


        return new(safeReports.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int safeReports = 0;

        for (int i = 0; i < _input.Length; i++)
        {
            string currentLine = _input[i];
            List<int> currentReport = currentLine.Split(' ').Select(int.Parse).ToList();

            //check if the report is strictly increasing
            bool allIncreasing = currentReport.Zip(currentReport.Skip(1), (a, b) => b > a).All(isIncreasing => isIncreasing);

            //check if the report is strictly decreasing
            bool allDecreasing = currentReport.Zip(currentReport.Skip(1), (a, b) => b < a).All(isDecreasing => isDecreasing);

            //if the report is strictly increasing or strictly decreasing, check distances
            bool isSafe = (allIncreasing || allDecreasing) && currentReport.Zip(currentReport.Skip(1), (a, b) => Math.Abs(b - a)).All(dist => dist >= 1 && dist <= 3);

            if (isSafe)
            {
                safeReports++;
            }
            else
            {
                // try removing each level and check if the report becomes safe
                bool canBeMadeSafe = false;
                for (int j = 0; j < currentReport.Count; j++)
                {
                    //remove the level at index j
                    List<int> modifiedReport = currentReport.Where((val, index) => index != j).ToList();

                    //check if the modified report is strictly increasing or strictly decreasing
                    bool modifiedAllIncreasing = modifiedReport.Zip(modifiedReport.Skip(1), (a, b) => b > a).All(isIncreasing => isIncreasing);
                    bool modifiedAllDecreasing = modifiedReport.Zip(modifiedReport.Skip(1), (a, b) => b < a).All(isDecreasing => isDecreasing);

                    //check if the distance between adjacent levels in the modified report is within the allowed range
                    bool modifiedIsSafe = (modifiedAllIncreasing || modifiedAllDecreasing) && modifiedReport.Zip(modifiedReport.Skip(1), (a, b) => Math.Abs(b - a)).All(dist => dist >= 1 && dist <= 3);

                    if (modifiedIsSafe)
                    {
                        canBeMadeSafe = true;
                        break; 
                    }
                }

                if (canBeMadeSafe)
                {
                    safeReports++;
                }
            }
        }




        return new(safeReports.ToString());
    }
}
