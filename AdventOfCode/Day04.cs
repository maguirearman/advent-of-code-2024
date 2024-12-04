using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;


public class Day04 : BaseDay
{
    private readonly string[] _input;

    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    static int search2D(char[][] grid, int row, int col, string word)
    {
        int m = grid.Length;
        int n = grid[0].Length;
        int matchCount = 0;

        // return false if the given coordinate does not match first letter of word
        if (grid[row][col] != word[0])
            return 0;

        int len = word.Length;

        // x and y are used to set the direction in which we search for word
        int[] x = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] y = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int dir = 0; dir < 8; dir++)
        {
            int currX = row, currY = col, k;

            //check the direction
            for (k = 1; k < len; k++)
            {
                currX += x[dir];
                currY += y[dir];

                //if out of bounds or mismatch
                if (currX < 0 || currX >= m || currY < 0 || currY >= n || grid[currX][currY] != word[k])
                    break;
            }

            //if the entire word matches in this direction
            if (k == len)
                matchCount++;
        }

        // if word is not found in any direction, return count
        return matchCount;
    }

    static bool search2DIsX(char[][] grid, int row, int col)
    {
        int m = grid.Length;
        int n = grid[0].Length;

        // return false if the given coordinate does not match first letter of word
        if (grid[row][col] != 'A')
            return false;

        // dir 1 is checking one set of diagonals
        int[] dir1x = { -1, 1 };
        int[] dir1y = { -1, 1 };

        // dir 2 is checking the other set of diagonals
        int[] dir2x = { -1, 1 };
        int[] dir2y = { 1, -1 };

        bool found1M = false, found1S = false;

        // check dir 1 diagonals to see if one is M and the other is S
        for (int i = 0; i < 2; i++)
        {
            int newX = row + dir1x[i];
            int newY = col + dir1y[i];

            //check we're within grid bounds
            if (newX < 0 || newX >= m || newY < 0 || newY >= n)
                return false;

            if (grid[newX][newY] == 'M')
                found1M = true;
            else if (grid[newX][newY] == 'S')
                found1S = true;
            else
                return false;
        }

        bool found1 = found1M && found1S;

        bool found2M = false, found2S = false;

        // check dir 2 diagonals to see if one is M and the other is S
        for (int i = 0; i < 2; i++)
        {
            int newX = row + dir2x[i];
            int newY = col + dir2y[i];

            //check we're within grid bounds
            if (newX < 0 || newX >= m || newY < 0 || newY >= n)
                return false;

            if (grid[newX][newY] == 'M')
                found2M = true;
            else if (grid[newX][newY] == 'S')
                found2S = true;
            else
                return false;
        }

        bool found2 = found2M && found2S;


        return found1 && found2;
    }

    public override ValueTask<string> Solve_1()
    {
        //create grid of chars
        char[][] grid = new char[_input.Length][];
        int rows = _input.Length;
        int cols = _input[0].Length;
        int count = 0;

        //populate the grid
        for (int i = 0; i < rows; i++)
        {
            grid[i] = _input[i].ToCharArray(); 
        }

        //count up all of the words in the input
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (search2D(grid, i, j, "XMAS") > 0)
                {
                    count = count + search2D(grid, i, j, "XMAS");
                }
            }
        }

        return new(count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        //create grid of chars
        char[][] grid = new char[_input.Length][];
        int rows = _input.Length;
        int cols = _input[0].Length;
        int count = 0;

        //populate the grid
        for (int i = 0; i < rows; i++)
        {
            grid[i] = _input[i].ToCharArray();
        }


        //count up all of the words in the input
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (search2DIsX(grid, i, j))
                {
                    count++;
                }
            }
        }

        return new(count.ToString());
    }
}
