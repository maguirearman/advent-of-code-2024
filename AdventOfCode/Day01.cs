using System.Collections.Immutable;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string[] _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }


    public override ValueTask<string> Solve_1()
    {
        //initialize arrays for each list
        int[] leftList = new int[_input.Length];
        int[] rightList = new int[_input.Length];
        int totalDistance = 0;

        //parse and create lists
        for (int i = 0; i < _input.Length; i++)
        {
            string currentLine = _input[i];
            string[] parts = currentLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            leftList[i] = int.Parse(parts[0]);
            rightList[i] = int.Parse(parts[1]);
        }

        //sort lists
        Array.Sort(leftList);
        Array.Sort(rightList);

        //calculate distance and add to total
        for (int i = 0; i < _input.Length; i++)
        {
            int distance = Math.Abs(leftList[i] - rightList[i]);
            totalDistance += distance;
        }


        return new(totalDistance.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        //initialize arrays for each list
        int[] leftList = new int[_input.Length];
        int[] rightList = new int[_input.Length];
        int similarityScore = 0;

        //parse and create lists
        for (int i = 0; i < _input.Length; i++)
        {
            string currentLine = _input[i];
            string[] parts = currentLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            leftList[i] = int.Parse(parts[0]);
            rightList[i] = int.Parse(parts[1]);
        }

        //sort lists
        Array.Sort(leftList);
        Array.Sort(rightList);

        int leftNumber = 0;
        for (int i = 0; i < _input.Length; i++)
        {
            leftNumber = leftList[i];
            int leftNumberCount = 0;
            for (int j = 0; j < _input.Length; j++)
            {
                if (rightList[j] == leftNumber)
                {
                    leftNumberCount++;
                }
            }
            similarityScore = similarityScore + leftNumber * leftNumberCount;
        }

        return new(similarityScore.ToString());
    }
}
