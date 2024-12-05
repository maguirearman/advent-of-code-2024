using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdventOfCode;
public class Day05 : BaseDay
{
    private readonly string[] _input;

    public Day05()
    {
        _input = File.ReadAllLines(InputFilePath);
    }


    public override ValueTask<string> Solve_1()
    {
        Dictionary<int,List<int>> rules = new Dictionary<int,List<int>>();
        List<int[]> orders = new List<int[]>();
        int finalSum = 0;

        //parse input
        foreach (var currentLine in _input) {
            //if line is a rule, populate map
            if (currentLine.Contains("|"))
            {
                string[] parts = currentLine.Split('|');
                int key = int.Parse(parts[0]);
                int value = int.Parse(parts[1]);

                //check if the key exists; if not, initialize the list
                if (!rules.ContainsKey(key))
                {
                    rules[key] = new List<int>();
                }

                //add the value to the list for the key
                rules[key].Add(value);
            }
            //if a line is an order, add to list of int arrays
            else if (currentLine.Contains(","))
            {
 
                int[] order = currentLine.Split(',').Select(num => int.Parse(num)).ToArray();
                orders.Add(order);
            }
        }

        //check if each order follows rules
        foreach (var order in orders)
        {
            bool isValid = true;

            for (int i = 0; i < order.Length; i++)
            {
                int currentNum1 = order[i];
                if (rules.ContainsKey(currentNum1))
                {
                    List<int> listofNum2s = rules[currentNum1];
                    foreach (int currentNum2 in listofNum2s)
                    {
                        if (order.Contains(currentNum2))
                        {
                            int currentNum1Index = Array.IndexOf(order, currentNum1);
                            int currentNum2Index = Array.IndexOf(order, currentNum2);
                            if (currentNum1Index > currentNum2Index)
                            {
                                isValid = false;
                                break;
                            }
                        }
                    }
                }
            }


            if (isValid)
            {
                int middleIndex = order.Length / 2;
                int middleNumber = order[middleIndex];

                finalSum += middleNumber;
            } else { 
            
            }

        }


        return new(finalSum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<int, List<int>> rules = new Dictionary<int, List<int>>();
        List<int[]> orders = new List<int[]>();
        int finalSum = 0;

        //parse input
        foreach (var currentLine in _input)
        {
            //if line is a rule, populate map
            if (currentLine.Contains("|"))
            {
                string[] parts = currentLine.Split('|');
                int key = int.Parse(parts[0]);
                int value = int.Parse(parts[1]);

                //check if the key exists; if not, initialize the list
                if (!rules.ContainsKey(key))
                {
                    rules[key] = new List<int>();
                }

                //add the value to the list for the key
                rules[key].Add(value);
            }
            //if a line is an order, add to list of int arrays
            else if (currentLine.Contains(","))
            {

                int[] order = currentLine.Split(',').Select(num => int.Parse(num)).ToArray();
                orders.Add(order);
            }
        }


        //check if each order follows rules and resort if it fails
        foreach (int[] order in orders)
        {
            bool isValid = true;
            bool swapped;

            do
            {
                swapped = false;
                for (int i = 0; i < order.Length; i++)
                {
                    int currentNum1 = order[i];
                    if (rules.ContainsKey(currentNum1))
                    {
                        List<int> listofNum2s = rules[currentNum1];
                        foreach (int currentNum2 in listofNum2s)
                        {
                            if (order.Contains(currentNum2))
                            {
                                int currentNum1Index = Array.IndexOf(order, currentNum1);
                                int currentNum2Index = Array.IndexOf(order, currentNum2);
                                //if it breaks the rule
                                if (currentNum1Index > currentNum2Index)
                                {
                                    //swap currentNum1 and currentNum2
                                    (order[currentNum1Index], order[currentNum2Index]) =
                                        (order[currentNum2Index], order[currentNum1Index]);
                                    isValid = false;
                                    swapped = true;
                                }
                            }
                        }
                    }
                }
            } while (swapped);

            //count up invalid middle numbers
            if (!isValid)
            {
                int middleIndex = order.Length / 2;
                int middleNumber = order[middleIndex];
                finalSum += middleNumber;
            }

        }

        return new(finalSum.ToString());
    }
}
