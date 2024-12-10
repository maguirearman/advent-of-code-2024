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
        StringBuilder step1 = new StringBuilder();
        int fileId = 0;

        // Parse the input string into file blocks and free spaces
        for (int i = 0; i < _input.Length; i += 2)
        {
            int fileLength = int.Parse(_input[i].ToString());
            int freeSpaceLength = (i + 1) < _input.Length ? int.Parse(_input[i + 1].ToString()) : 0;

            // Append file blocks using the fileId
            step1.Append(new string(fileId.ToString()[0], fileLength));
            fileId++;

            // Append free space blocks after file blocks
            if (freeSpaceLength > 0)
            {
                step1.Append(new string('.', freeSpaceLength));
            }
        }

        Console.WriteLine("Initial map: " + step1.ToString());
        char[] map = step1.ToString().ToCharArray();

        // Perform the left-right swapping until all gaps are cleared
        while (true)
        {
            int leftMostDot = Array.IndexOf(map, '.');
            int rightMostNumber = Array.FindLastIndex(map, c => char.IsDigit(c));

            if (leftMostDot == -1 || rightMostNumber == -1 || leftMostDot > rightMostNumber)
            {
                break;
            }

            // Swap the leftmost dot with the rightmost number
            Console.WriteLine($"Swapping index {leftMostDot} ('.') with index {rightMostNumber} ('{map[rightMostNumber]}')");
            (map[leftMostDot], map[rightMostNumber]) = (map[rightMostNumber], map[leftMostDot]);
        }

        Console.WriteLine("Final map after compacting: " + new string(map));

        // Calculate checksum by summing index * fileID at each non-dot index
        long checksum = 0;
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] != '.')
            {
                checksum += i * (map[i] - '0');
                Console.WriteLine($"Adding to checksum: index {i}, file ID {map[i]}, current checksum = {checksum}");
            }
        }

        Console.WriteLine("Final checksum: " + checksum);

        return new ValueTask<string>(checksum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new();
    }
}
