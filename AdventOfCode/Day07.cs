using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly string[] _input;

    public Day07()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<long> results = new List<long>();
        List<long[]> operands = new List<long[]>();

        //parse each line
        for (int i = 0; i < _input.Length; i++)
        {
            var parts = _input[i].Split(':');
            if (parts.Length == 2)
            {
                results.Add(long.Parse(parts[0].Trim()));

                var operandArray = parts[1].Trim().Split(' ').Select(long.Parse).ToArray();
                operands.Add(operandArray);
            }
        }

        long sum = 0;
        //check if valid and add up sum
        for (int i = 0; i < results.Count; i++)
        {
            bool canAchieve = CanMatchTarget(results[i], operands[i]);

            if (canAchieve)
            {
                sum += results[i];
            }
        }

        return new(sum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        List<long> results = new List<long>();
        List<long[]> operands = new List<long[]>();

        //parse each line
        for (int i = 0; i < _input.Length; i++)
        {
            var parts = _input[i].Split(':');
            if (parts.Length == 2)
            {
                results.Add(long.Parse(parts[0].Trim()));

                var operandArray = parts[1].Trim().Split(' ').Select(long.Parse).ToArray();
                operands.Add(operandArray);
            }
        }

        long sum = 0;
        //check if valid and add up sum
        for (int i = 0; i < results.Count; i++)
        {
            bool canAchieve = CanMatchTargetWithConcat(results[i], operands[i]);

            if (canAchieve)
            {
                sum += results[i];
            }
        }

        return new(sum.ToString());
    }

    private bool CanMatchTarget(long target, long[] numbers)
    {
        int numOperators = numbers.Length - 1;
        int totalCombinations = (int)Math.Pow(2, numOperators);


        for (int combo = 0; combo < totalCombinations; combo++)
        {
            long result = numbers[0];
            for (int j = 0; j < numOperators; j++)
            {
                // determine the operator for this position (using binary bit shifting)
                bool isAddition = (combo & (1 << j)) != 0;
                if (isAddition)
                    result += numbers[j + 1];
                else
                    result *= numbers[j + 1];

                // early termination if the result exceeds the target
                if (result > target)
                    break;
            }

            if (result == target)
                return true;
        }
        return false;
    }

    private bool CanMatchTargetWithConcat(long target, long[] numbers)
    {
        int numOperators = numbers.Length - 1;
        //bit shifting for 3 operators
        int totalCombinations = (int)Math.Pow(3, numOperators); 

        for (int combo = 0; combo < totalCombinations; combo++)
        {
            long result = numbers[0];
            int currentCombo = combo;

            for (int j = 0; j < numOperators; j++)
            {
                int operatorType = currentCombo % 3; 
                currentCombo /= 3; //shift to the next operator choice for the next position

                switch (operatorType)
                {
                    case 0: //multiplication
                        result *= numbers[j + 1];
                        break;
                    case 1: //addition
                        result += numbers[j + 1];
                        break;
                    case 2: //concatenation
                        result = ConcatenateNumbers(result, numbers[j + 1]);
                        break;
                }

                //early termination if the result exceeds the target
                if (result > target)
                    break;
            }

            if (result == target)
                return true;
        }

        return false;
    }

    private long ConcatenateNumbers(long a, long b)
    {
        string concatenatedString = $"{a}{b}";
        return long.Parse(concatenatedString);
    }


}
