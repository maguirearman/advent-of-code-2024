using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string[] _input;

    public Day06()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    static (int, int) TurnRight((int, int) currentDir)
    {
        if (currentDir == (-1, 0)) return (0, 1); //up -> right
        if (currentDir == (0, 1)) return (1, 0);  //right -> down
        if (currentDir == (1, 0)) return (0, -1); //down -> left
        if (currentDir == (0, -1)) return (-1, 0); //left -> up
        throw new InvalidOperationException("Invalid direction given");
    }

    private bool LeadsToInfiniteLoop(char[,] grid, (int, int) startingPosition, (int, int) initialDirection)
    {
        var visited = new HashSet<((int, int), (int, int))>();
        (int, int) currentPosition = startingPosition;
        (int, int) currentDirection = initialDirection;

        while (true)
        {
            if (visited.Contains((currentPosition, currentDirection)))
            {
               
                return true;
            }

            visited.Add((currentPosition, currentDirection));

            int newRow = currentPosition.Item1 + currentDirection.Item1;
            int newCol = currentPosition.Item2 + currentDirection.Item2;

            if (newRow < 0 || newRow >= grid.GetLength(0) || newCol < 0 || newCol >= grid.GetLength(1))
            {
                return false;
            }

            if (grid[newRow, newCol] == '#')
            {
                currentDirection = TurnRight(currentDirection);
            }
            else
            {
                currentPosition = (newRow, newCol);
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        char[,] grid = new char[_input.Length, _input[0].Length];

        // store everything in 2D array
        for (int i = 0; i < _input.Length; i++)
        {
            for (int j =0; j < _input[i].Length; j++)
            {
                grid[i , j] = _input[i][j];
            }
        }

        (int, int)? guardCoordinates = null;
        List<(int, int)> objects = new List<(int, int)>();
        var directionMap = new Dictionary<char, (int, int)>
        {
            { '^', (-1, 0) },
            { '>', (0, 1) },
            { '<', (0, -1) },
            { 'v', (1, 0) }
        };
        (int, int) currentDirection = (0, 0);


        //find starting position, direction, and locations of objects
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (directionMap.ContainsKey(grid[i , j]))
                {
                    guardCoordinates = (i, j);
                    currentDirection = directionMap[grid[i , j]];
                    grid[i, j] = '.';

                } 
                else if (grid[i, j] == '#')
                {
                    objects.Add((i, j));
                }
            }
        }

        var visited = new HashSet<(int, int)>();
        (int row, int col) currentPosition = guardCoordinates.Value;

        while (true)
        {
            visited.Add(currentPosition);

            //calculate next position
            (int nextRow, int nextCol) = (currentPosition.row + currentDirection.Item1, currentPosition.col + currentDirection.Item2);

            // if the guard walks out of bounds, break
            if (nextRow < 0 || nextRow >= grid.GetLength(0) || nextCol < 0 || nextCol >= grid.GetLength(1))
            {
                break;
            }

            // Check if the next position is an object
            if (grid[nextRow, nextCol] == '#')
            {
                // Turn right: Update currentDelta to the next direction in the sequence
                currentDirection = TurnRight(currentDirection);
            }
            else
            {
                // Move to the next position
                currentPosition = (nextRow, nextCol);
            }
        }
        int count = visited.Count;
        return new(count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var directionMap = new Dictionary<char, (int, int)>
        {
            { '^', (-1, 0) },  
            { '>', (0, 1) },   
            { '<', (0, -1) }, 
            { 'v', (1, 0) }    
        };

        char[,] grid = new char[_input.Length, _input[0].Length];

        // store everything in 2D array
        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = 0; j < _input[i].Length; j++)
            {
                grid[i, j] = _input[i][j];
            }
        }

        (int, int)? guardCoordinates = null;
        (int, int) currentDirection = (0, 0);


        //find starting position, direction, and locations of objects
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (directionMap.ContainsKey(grid[i, j]))
                {
                    guardCoordinates = (i, j);
                    currentDirection = directionMap[grid[i, j]];
                    grid[i, j] = '.';
                    
                }
            }
        }

        int count = 0;
        int totalTestPositions = 0;
        
        // Simulate adding `#` at every walkable location and check for infinite loops
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j] == '.')
                {

                    totalTestPositions++;
                    // Simulate with a wall at this position
                    grid[i, j] = '#';

                    if (LeadsToInfiniteLoop(grid, guardCoordinates.Value, currentDirection))
                    {
                        count++;
                    }

                    // Restore the original map after testing
                    grid[i, j] = '.';
                }
            }
        }

        return new(count.ToString());
    }
}
