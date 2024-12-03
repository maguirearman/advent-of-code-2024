using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }


    public override ValueTask<string> Solve_1()
    {
        int result = 0;
        List<int> num1 = new List<int>();
        List<int> num2 = new List<int>();

        //regex to match mul(number1,number2)
        string mulPattern = @"mul\((\d{1,3}),(\d{1,3})\)";
        Regex mulRegex = new Regex(mulPattern);

        //find matches
        MatchCollection matches = mulRegex.Matches(_input);

        foreach (Match match in matches)
        {
            if (match.Success)
            {
                num1.Add(int.Parse(match.Groups[1].Value));
                num2.Add(int.Parse(match.Groups[2].Value));
            }
        }

        for (int i = 0; i < num1.Count(); i++)
        {
            result = result + num1[i] * num2[i];
        }


        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int result = 0;
        List<int> mulPositions = new List<int>();
        List<int> doPositions = new List<int>();
        List<int> dontPositions = new List<int>();

        //regex to match mul(number1,number2)
        string mulPattern = @"mul\((\d{1,3}),(\d{1,3})\)";
        Regex mulRegex = new Regex(mulPattern);

        //find matches
        MatchCollection mulMatches = mulRegex.Matches(_input);

        //store position of each mul
        foreach (Match mulMatch in mulMatches)
        {
            mulPositions.Add(mulMatch.Index);
        }

        //regex to match do() or don't()
        string doPattern = @"do(n\'t)?\(\)";
        Regex doRegex = new Regex(doPattern);

        //find matches
        MatchCollection doMatches = doRegex.Matches(_input);

        foreach (Match doMatch in doMatches)
        {
            if (doMatch.Groups[1].Success)  
            {
                //store the position of don't()
                dontPositions.Add(doMatch.Index);  
            }
            else
            {
                //store the position of do()
                doPositions.Add(doMatch.Index); 
            }
        }


        //for each mul(), determine if it is enabled or disabled
        bool mulEnabled = true; 

        foreach (int mulPosition in mulPositions)
        {
            //find the closest do() or don't() before the current mul()
            int closestDoPosition = -1;
            int closestDontPosition = -1;

            //find the closest do() before mulPosition
            foreach (int doPos in doPositions)
            {
                if (doPos <= mulPosition) 
                {
                    closestDoPosition = doPos;
                }
                else
                {
                    break;
                }
            }

            //find the closest don't() before mulPosition
            foreach (int dontPos in dontPositions)
            {
                if (dontPos <= mulPosition) 
                {
                    closestDontPosition = dontPos;
                }
                else
                {
                    break;
                }
            }

            //decide if mul() should be enabled or disabled based on the closest do() or don't()
            if (closestDontPosition > closestDoPosition)  
            {
                mulEnabled = false;
            }
            else
            {
                mulEnabled = true;
            }

            //if mulEnabled is true, calculate the mul result
            if (mulEnabled)
            {
                //find the corresponding numbers from the mul() match
                var match = Regex.Match(_input.Substring(mulPosition), mulPattern);
                int num1 = int.Parse(match.Groups[1].Value);
                int num2 = int.Parse(match.Groups[2].Value);
                result += num1 * num2;
            }
        }

        return new(result.ToString());
    }
}
