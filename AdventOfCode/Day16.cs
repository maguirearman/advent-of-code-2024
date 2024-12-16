using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day16 : BaseDay
{
    private readonly string[] _input;

    public Day16()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    enum Direction { North, East, South, West }

    public override ValueTask<string> Solve_1()
    {
        char[][] maze = new char[_input.Length][];
        (int y, int x) startPos = (-1, -1);
        (int y, int x) endPos = (-1, -1);

        for (int y = 0; y < _input.Length; y++)
        {
            
            maze[y] = _input[y].ToCharArray();

            for (int x = 0; x < maze[y].Length; x++)
            {
                // identify start and end positions
                if (maze[y][x] == 'S')
                {
                    startPos = (y, x);
                }
                else if (maze[y][x] == 'E')
                {
                    endPos = (y, x);
                }
            }
        }

        Direction direction = Direction.East;

        (int dy, int dx)[] moves = {
            (-1, 0), // North
            (0, 1),  // East
            (1, 0),  // South
            (0, -1)  // West
        };
        int lowestScore = 100000000;

        PriorityQueue<(int score, int y, int x, Direction dir), int> priorityQueue = new();

        HashSet<(int y, int x, Direction dir)> visited = new();

        //add starting state to priority queue
        priorityQueue.Enqueue((score: 0, y: startPos.y, x: startPos.x, dir: Direction.East), 0);

        while (priorityQueue.Count > 0)
        {
            //pop the state with the lowest score
            var (score, y, x, dir) = priorityQueue.Dequeue();

            //avoid reprocessing already visited states
            if (!visited.Add((y, x, dir))) continue;

            //if we've reached the end, store the lowest score
            if ((y, x) == endPos)
            {
                if (score < lowestScore)
                {
                    lowestScore = score;
                }
            }

            //attempt to move forward
            (int dy, int dx) move = moves[(int)dir];
            int newY = y + move.dy;
            int newX = x + move.dx;

            if (newY >= 0 && newY < maze.Length &&
                newX >= 0 && newX < maze[0].Length &&
                maze[newY][newX] != '#')
            {
                priorityQueue.Enqueue((score + 1, newY, newX, dir), score + 1);
            }

            // rotating clockwise
            Direction clockwise = (Direction)(((int)dir + 1) % 4);
            priorityQueue.Enqueue((score + 1000, y, x, clockwise), score + 1000);

            // rotating counterclockwise
            Direction counterclockwise = (Direction)(((int)dir + 3) % 4);
            priorityQueue.Enqueue((score + 1000, y, x, counterclockwise), score + 1000);
        }

        return new(lowestScore.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        char[][] maze = new char[_input.Length][];
        (int y, int x) startPos = (-1, -1);
        (int y, int x) endPos = (-1, -1);

        for (int y = 0; y < _input.Length; y++)
        {

            maze[y] = _input[y].ToCharArray();

            for (int x = 0; x < maze[y].Length; x++)
            {
                // identify start and end positions
                if (maze[y][x] == 'S')
                {
                    startPos = (y, x);
                }
                else if (maze[y][x] == 'E')
                {
                    endPos = (y, x);
                }
            }
        }


        int[] dy = { -1, 0, 1, 0 };
        int[] dx = { 0, 1, 0, -1 };

        //BFS Queue
        Queue<(int y, int x)> queue = new Queue<(int, int)>();
        HashSet<(int, int)> visited = new HashSet<(int, int)>();
        Dictionary<(int, int), int> distances = new Dictionary<(int, int), int>();

        //start BFS from the starting position
        queue.Enqueue(startPos);
        visited.Add(startPos);
        distances[startPos] = 0;

        //bfs to compute shortest path distances
        while (queue.Count > 0)
        {
            var (y, x) = queue.Dequeue();

            for (int dir = 0; dir < 4; dir++)
            {
                int newY = y + dy[dir];
                int newX = x + dx[dir];

                if (newY >= 0 && newY < maze.Length &&
                    newX >= 0 && newX < maze[0].Length &&
                    maze[newY][newX] != '#' &&
                    !visited.Contains((newY, newX)))
                {
                    queue.Enqueue((newY, newX));
                    visited.Add((newY, newX));
                    distances[(newY, newX)] = distances[(y, x)] + 1;
                }
            }
        }

        //backtrack from the end position and find all tiles on the shortest paths
        HashSet<(int, int)> uniqueTiles = new HashSet<(int, int)>();
        void DFS((int y, int x) currentPos, HashSet<(int, int)> pathVisited)
        {
            if (currentPos == endPos)
            {
                //if we reached the end, mark all tiles in the current path
                foreach (var tile in pathVisited)
                {
                    uniqueTiles.Add(tile);
                }
                return;
            }

            pathVisited.Add(currentPos);

            for (int dir = 0; dir < 4; dir++)
            {
                int newY = currentPos.y + dy[dir];
                int newX = currentPos.x + dx[dir];
                var nextPos = (newY, newX);

                //move only if it's part of the shortest path sequence
                if (distances.ContainsKey(nextPos) &&
                    distances[nextPos] == distances[currentPos] + 1 &&
                    maze[newY][newX] != '#')
                {
                    DFS(nextPos, new HashSet<(int, int)>(pathVisited));
                }
            }

            pathVisited.Remove(currentPos);
        }

        //start DFS to gather unique tiles from all shortest paths
        DFS(startPos, new HashSet<(int, int)>());




        return new(uniqueTiles.Count.ToString());
    }
}
