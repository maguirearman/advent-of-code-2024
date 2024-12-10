using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day10 : BaseDay
{
    private readonly string[] _input;

    public Day10()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public int CalculateTrailheadScore((int row, int col) trailhead, int[,] grid)
    {
        var visitedNines = new HashSet<(int, int)>();
        int startRow = trailhead.row;
        int startCol = trailhead.col;

        //use depth first search to explore the grid
        void DFS(int row, int col, int currentHeight)
        {
            // check bounds and if it can move
            if (row < 0 || row >= grid.GetLength(0) || col < 0 || col >= grid.GetLength(1) || grid[row, col] != currentHeight)
            {
                return;
            }

            //if we've reached a 9, add it to the set
            if (currentHeight == 9)
            {
                visitedNines.Add((row, col));
                return;
            }

            // recursively call DFS on all four possible directions
            DFS(row + 1, col, currentHeight + 1); // down
            DFS(row - 1, col, currentHeight + 1); // up
            DFS(row, col + 1, currentHeight + 1); // right
            DFS(row, col - 1, currentHeight + 1); // left
        }

        //start the DFS from the trailhead
        DFS(startRow, startCol, 0);

        return visitedNines.Count;
    }

    public int CalculateTrailheadRating((int row, int col) trailhead, int[,] grid)
    {
        var distinctTrails = new HashSet<string>();
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // depth first search storing path
        void DFS(int row, int col, int currentHeight, string path)
        {
            // bounds and validity check
            if (row < 0 || row >= rows || col < 0 || col >= cols || grid[row, col] != currentHeight)
            {
                return;
            }

            // add the current coordinate to the path string
            path += $"->{row},{col}";

            // if we reach a height of 9, store the path as a distinct trail
            if (currentHeight == 9)
            {
                distinctTrails.Add(path);
                return;
            }

            // recursively call DFS on all four possible directions
            DFS(row + 1, col, currentHeight + 1, path); // down
            DFS(row - 1, col, currentHeight + 1, path); // up
            DFS(row, col + 1, currentHeight + 1, path); // right
            DFS(row, col - 1, currentHeight + 1, path); // left
        }

        //start the DFS from the trailhead
        DFS(trailhead.row, trailhead.col, 0, "");

        return distinctTrails.Count;
    }

    public override ValueTask<string> Solve_1()
    {
        int rows = _input.Length;
        int cols = _input[0].Length;
        int[,] grid = new int[rows, cols];
        HashSet<(int, int)> trailheads = new HashSet<(int, int)>();

        // store everything in 2D array
        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = 0; j < _input[i].Length; j++)
            {
                //store char as int
                grid[i, j] = _input[i][j] - '0';
                if (grid[i, j] == 0)
                {
                    trailheads.Add((i, j));
                }
            }
        }

        int totalScore = 0;
        foreach (var trailhead in trailheads)
        {
            totalScore += CalculateTrailheadScore(trailhead, grid);
        }

        return new(totalScore.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int rows = _input.Length;
        int cols = _input[0].Length;
        int[,] grid = new int[rows, cols];
        HashSet<(int, int)> trailheads = new HashSet<(int, int)>();

        // store everything in 2D array
        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = 0; j < _input[i].Length; j++)
            {
                //store char as int
                grid[i, j] = _input[i][j] - '0';
                if (grid[i, j] == 0)
                {
                    trailheads.Add((i, j));
                }
            }
        }

        int totalScore = 0;
        foreach (var trailhead in trailheads)
        {
            totalScore += CalculateTrailheadRating(trailhead, grid);
        }

        return new(totalScore.ToString());
    }
}