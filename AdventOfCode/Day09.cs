using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string _input;

    public Day09()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        List<int> map = new List<int>();
        int fileId = 0;

        //parse input string into blocks
        for (int i = 0; i < _input.Length; i += 2)
        {
            int fileLength = int.Parse(_input[i].ToString());
            int freeSpaceLength = (i + 1) < _input.Length ? int.Parse(_input[i + 1].ToString()) : 0;

            //add file blocks
            for (int j = 0; j < fileLength; j++)
            {
                map.Add(fileId);
            }

            //add free space blocks
            for (int k = 0; k < freeSpaceLength; k++)
            {
                map.Add(-1); //uses -1 to represent free spaces
            }

            fileId++;
        }

        Console.WriteLine("Initial map: " + string.Join(",", map));

        // step 2 compact disc shift
        map = CompactAndShift(map);

        Console.WriteLine("Final map after compacting: " + string.Join(",", map));

        //calculate the checksum by summing index * fileID at valid indices
        long checksum = 0;
        for (int i = 0; i < map.Count; i++)
        {
            if (map[i] != -1)
            {
                checksum += i * map[i];
                Console.WriteLine($"Adding index {i} with fileID {map[i]} to checksum, current checksum = {checksum}");
            }
        }

        return new ValueTask<string>(checksum.ToString());
    }

    private List<int> CompactAndShift(List<int> map)
    {
        while (true)
        {
            //leftmost .
            int leftMostGapIndex = map.IndexOf(-1); 
            //rightmost #
            int rightMostNumberIndex = map.FindLastIndex(x => x != -1);

            if (leftMostGapIndex == -1 || rightMostNumberIndex == -1 || leftMostGapIndex >= rightMostNumberIndex)
            {
                break; //no more valid swaps left
            }

            //swap
            map[leftMostGapIndex] = map[rightMostNumberIndex];
            map[rightMostNumberIndex] = -1;
        }

        return map;
    }
    private List<int> CompactFilesIteratively(List<int> map)
    {
        while (true)
        {
            var rightmostFile = FindRightmostFile(map);
            if (rightmostFile == null)
                break;

            int fileSize = rightmostFile.Item2;
            int fileStartIndex = rightmostFile.Item1;

            var leftmostFreeIndex = FindLeftmostFreeSpace(map, fileSize);
            if (leftmostFreeIndex != -1)
            {
                Console.WriteLine($"Moving file starting at index {fileStartIndex} of size {fileSize} to position starting at index {leftmostFreeIndex}");
                MoveFile(map, fileStartIndex, leftmostFreeIndex, fileSize);
            }
            else
            {
                Console.WriteLine($"No left space found for file starting at index {fileStartIndex}");
                break;
            }
        }

        return map;
    }

    /// <summary>
    /// Finds the rightmost file in the map.
    /// </summary>
    private Tuple<int, int> FindRightmostFile(List<int> map)
    {
        for (int i = map.Count - 1; i >= 0; i--)
        {
            if (map[i] != -1) // Found a file
            {
                int fileId = map[i];
                int length = 0;

                while (i - length >= 0 && map[i - length] == fileId)
                {
                    length++;
                }

                Console.WriteLine($"Found rightmost file starting at index {i - length + 1} of length {length}");
                return new Tuple<int, int>(i - length + 1, length);
            }
        }

        return null; // No file found
    }

    /// <summary>
    /// Finds the leftmost free space that can fit the given file size.
    /// </summary>
    private int FindLeftmostFreeSpace(List<int> map, int fileSize)
    {
        int index = 0;

        while (index < map.Count)
        {
            if (map[index] == -1)
            {
                // Check if there are enough contiguous -1's to fit the file
                int freeSpaceCount = 0;

                while (index + freeSpaceCount < map.Count && map[index + freeSpaceCount] == -1)
                {
                    freeSpaceCount++;
                    if (freeSpaceCount == fileSize)
                    {
                        Console.WriteLine($"Found leftmost free space at index {index} that can fit size {fileSize}");
                        return index;
                    }
                }

                index += freeSpaceCount;
            }
            else
            {
                index++;
            }
        }

        Console.WriteLine("No leftmost free space found that can fit the file.");
        return -1;
    }

    /// <summary>
    /// Moves a file block to a new location in the map.
    /// </summary>
    private void MoveFile(List<int> map, int oldStartIndex, int newStartIndex, int fileSize)
    {
        for (int i = 0; i < fileSize; i++)
        {
            map[oldStartIndex + i] = -1; // Clear the original space
            map[newStartIndex + i] = map[oldStartIndex + i];
        }

        Console.WriteLine($"Moved file from index {oldStartIndex} to index {newStartIndex}");
    }

    public override ValueTask<string> Solve_2()
    {
        List<int> map = new List<int>();
        int fileId = 0;

        //parse input string into blocks
        for (int i = 0; i < _input.Length; i += 2)
        {
            int fileLength = int.Parse(_input[i].ToString());
            int freeSpaceLength = (i + 1) < _input.Length ? int.Parse(_input[i + 1].ToString()) : 0;

            //add file blocks
            for (int j = 0; j < fileLength; j++)
            {
                map.Add(fileId);
            }

            //add free space blocks
            for (int k = 0; k < freeSpaceLength; k++)
            {
                map.Add(-1); //uses -1 to represent free spaces
            }

            fileId++;
        }

        Console.WriteLine("Initial map: " + string.Join(",", map));

        // step 2 //new sorting algo
        //move rightmost file to left most space if possible
        map = CompactFilesIteratively(map);

        Console.WriteLine("Final map after compacting: " + string.Join(",", map));

        //calculate the checksum by summing index * fileID at valid indices
        long checksum = 0;
        for (int i = 0; i < map.Count; i++)
        {
            if (map[i] != -1)
            {
                checksum += i * map[i];
                Console.WriteLine($"Adding index {i} with fileID {map[i]} to checksum, current checksum = {checksum}");
            }
        }

        return new ValueTask<string>(checksum.ToString());
    }
}
