using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

public class Day12 : BaseDay
{
    private readonly string[] _input;

    public Day12()
    {
        _input = File.ReadAllLines(InputFilePath);
    }



    public override ValueTask<string> Solve_1()
    {
        int rows = _input.Length;
        int cols = _input[0].Length;

        char[,] grid = new char[rows, cols];
        bool[,] visited = new bool[rows, cols];

        //build the grid
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = _input[i][j];
            }
        }

        // up, down, left, right
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        List<(char plantType, int area, int perimeter)> regions = new List<(char, int, int)>();
        
        //use dfS to calculate area and perimeter
        (int area, int perimeter) calculate (int x, int y, char plantType)
        {
            //current cell is marked as visited
            visited[x, y] = true;
            int area = 1;
            int perimeter = 0;

            //check all 4 neighbors
            for (int d = 0; d < 4; d++)
            {
                int nx = x + dx[d];
                int ny = y + dy[d];

                //bounds check
                if (nx < 0 || nx >= rows || ny < 0 || ny >= cols || grid[nx, ny] != plantType)
                {
                    //if oob or diff type, add to perimeter
                    perimeter++;
                }
                else if (!visited[nx, ny])
                {
                    //recursively call on neighbor if it is in region
                    var (subArea, subPerimeter) = calculate(nx, ny, plantType);
                    //add calculated area and perimeter
                    area += subArea;
                    perimeter += subPerimeter;
                }
            }

            return (area, perimeter);
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (!visited[i, j])
                {
                    char plantType = grid[i, j];
                    var (area, perimeter) = calculate(i, j, plantType);
                    regions.Add((plantType, area, perimeter));
                }
            }
        }

        int sum = 0;
        foreach (var region in regions)
        {
            int areaPerimeterProduct = region.area * region.perimeter;
            sum += areaPerimeterProduct;
            Console.WriteLine($"Plant Type: {region.plantType}, Area: {region.area}, Perimeter: {region.perimeter}, Area * Perimeter: {areaPerimeterProduct}");
        }


        return new(sum.ToString());
    }


    private (int area, int turns) ExploreRegionWithTurns(char[,] grid, bool[,] visited, int startX, int startY)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        char plantType = grid[startX, startY];

        // Directions to traverse neighbors (Up, Down, Left, Right)
        var directions = new (int dx, int dy)[]
        {
        (-1, 0), // Up
        (1, 0),  // Down
        (0, -1), // Left
        (0, 1)   // Right
        };

        // BFS/DFS to trace the region boundary and count "turns"
        Queue<(int, int, (int, int))> queue = new Queue<(int, int, (int, int))>();
        queue.Enqueue((startX, startY, (-1, -1))); // Start with initial position
        visited[startX, startY] = true;

        int area = 0;
        int turns = 0;
        (int, int) currentDirection = (-1, -1);

        while (queue.Count > 0)
        {
            var (x, y, prevDirection) = queue.Dequeue();
            area++;

            // Explore all neighbors to find external transitions
            foreach (var dir in directions)
            {
                int newX = x + dir.dx;
                int newY = y + dir.dy;

                if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
                {
                    if (grid[newX, newY] == plantType && !visited[newX, newY])
                    {
                        visited[newX, newY] = true;
                        queue.Enqueue((newX, newY, dir));

                        if (currentDirection != (-1, -1) && dir != currentDirection)
                        {
                            // Detected a turn
                            turns++;
                        }

                        currentDirection = dir; // Update current movement direction
                    }
                }
            }
        }

        return (area, turns);
    }

    public override ValueTask<string> Solve_2()
    {
        int rows = _input.Length;
        int cols = _input[0].Length;

        char[,] grid = new char[rows, cols];
        bool[,] visited = new bool[rows, cols];

        //build the grid
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = _input[i][j];
            }

        }
        int totalPrice = 0;

        // Explore all regions
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (!visited[i, j])
                {
                    var (area, turns) = ExploreRegionWithTurns(grid, visited, i, j);
                    if (area > 0 && turns > 0)
                    {
                        int regionPrice = area * turns;
                        totalPrice += regionPrice;
                        Console.WriteLine($"Region Plant Type: {grid[i, j]}, Area: {area}, Turns: {turns}, Total Price: {regionPrice}");
                    }
                }
            }
        }

        return new(totalPrice.ToString());
    }
}
