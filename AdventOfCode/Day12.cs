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


    public override ValueTask<string> Solve_2()
    {
        int rows = _input.Length;
        int cols = _input[0].Length;

        char[,] grid = new char[rows, cols];
        Dictionary<char, List<(int x, int y)>> plantRegions = new Dictionary<char, List<(int x, int y)>>();

        //build the grid
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = _input[i][j];
            }
        }

        //build the grid
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                char plantType = _input[i][j];

                if (!plantRegions.ContainsKey(plantType))
                {
                    plantRegions[plantType] = new List<(int x, int y)>();
                }

                plantRegions[plantType].Add((i, j));
            }
        }

        bool IsCorner(int x, int y, char plantType)
        {
            // Directions: Left, Right, Up, Down
            (int dx, int dy)[] directions =
            {
            (-1, 0), (1, 0), (0, -1), (0, 1)
        };

            int neighborCount = 0;

            foreach (var (dx, dy) in directions)
            {
                int neighborX = x + dx;
                int neighborY = y + dy;

                if (IsWithinBounds(neighborX, neighborY) && grid[neighborX, neighborY] == plantType)
                {
                    neighborCount++;
                }
            }

            return neighborCount == 1 || neighborCount == 3; // A corner is defined by having 2 or more neighbors
        }

        bool IsWithinBounds(int x, int y)
        {
            // Assuming the plant map is 10x10 or adjust accordingly
            return x >= 0 && x < 10 && y >= 0 && y < 10;
        }

        // Calculate area and number of sides for a region
        (int area, int numberOfSides) CalculateRegionProperties(List<(int x, int y)> coordinates, char plantType)
        {
            int area = coordinates.Count;
            int numberOfSides = 0;

            foreach (var (x, y) in coordinates)
            {
                if (IsCorner(x, y, plantType))
                {
                    Console.WriteLine($"Coordinate ({x}, {y}) is a corner for plant type '{plantType}'");
                    numberOfSides++;
                }
            }

            return (area, numberOfSides);
        }


        int sum = 0;
        foreach (var region in plantRegions)
        {
            var (area, numberOfSides) = CalculateRegionProperties(region.Value, region.Key);
            int areaSideProduct = area * numberOfSides;
            sum += areaSideProduct;
            Console.WriteLine($"Plant Type: {region.Key}, Area: {area}, Sides: {numberOfSides}, Area * Sides: {areaSideProduct}");
        }

        return new(sum.ToString());
    }
}
