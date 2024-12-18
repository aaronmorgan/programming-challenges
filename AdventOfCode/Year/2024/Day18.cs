﻿using AdventOfCode.Extensions;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day18
{
    /// <summary>
    /// Uses a simple BFS keeping track of the current path to determine the shortest route to the exit point.
    /// </summary>
    [Theory]
    [InlineData("Day18DevelopmentTesting1.txt", 7, 7, 12, 22)]
    [InlineData("Day18.txt", 71, 71, 1024, 280)]
    public void Day18_Part1_RAM_Run(string filename, int width, int height, int kbData, int expectedAnswer)
    {
        string[] input = InputParser.ReadAllLines("2024/" + filename).ToArray();
        char[,] grid = new char[width, height];

        for (var row = 0; row < grid.GetLength(0); row++)
        {
            for (var col = 0; col < grid.GetLength(1); col++)
            {
                grid[row, col] = '.';
            }
        }

        for (int i = 0; i < kbData; i++)
        {
            string s = input[i];
            int[] pos = s.Split(',').Select(int.Parse).ToArray();

            // Data is given to us as x y, flip to store as y x.
            grid[pos[1], pos[0]] = '#';
        }

        grid.DrawToConsole();

        Queue<((int y, int x) pos, List<(int y, int x)> path)> queue = new();
        HashSet<(int y, int x)> seen = [];

        queue.Enqueue(((0, 0), [(0, 0)]));
        List<(int y, int x)> path = [];

        while (queue.Count > 0)
        {
            var (currentPos, currentPath) = queue.Dequeue();

            // We've been on this vertex before, from this direction.
            if (!seen.Add(currentPos)) continue;

            // Check we're at the end position.
            if (currentPos.x == grid.GetLength(0) - 1 && currentPos.y == grid.GetLength(1) - 1)
            {
                path = currentPath;
                break;
            }

            foreach (var (dr, dc) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
            {
                int y = currentPos.y + dr;
                int x = currentPos.x + dc;

                // Are we looking around at an already traversed position?
                if (seen.Contains((y, x))) continue;

                // Bounds checking.
                if (y < 0 || y > grid.GetLength(0) - 1 || x < 0 || x > grid.GetLength(1) - 1) continue;

                if (grid[y, x] == '#') continue;

                // Queue the next adjacent position and the direction we're moving to get there.
                queue.Enqueue(((y, x), new List<(int row, int col)>(currentPath)
                {
                    (y, x)
                }));
            }
        }

        Assert.Equal(expectedAnswer, path.Count - 1);
    }
}