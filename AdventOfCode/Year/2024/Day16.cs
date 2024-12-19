using Common.Types;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day16
{
    
    // see Day23_Part1_ALongWalk
    [Theory]
    [InlineData("Day16DevelopmentTesting1.txt", 7036)]
//    [InlineData("Day14.txt", 101, 103, 100, 221278720)] // too low 221278720
    public void Day14_Reindeer_Maze(string filename, int expectedAnswer)
    {
        char[,] maze = InputParser.ReadAllChars("2024/" + filename);

        int result = 0;

        (int y, int x) startPosition = (0, 0);
        (int y, int x) exitPosition = (0, 0);

        // Locate the start and exit nodes.
        for (int row = 0; row < maze.GetLength(0); row++)
        {
            for (int column = 0; column < maze.GetLength(1); column++)
            {
                if (maze[row, column] == 'E') exitPosition = (row, column);
                if (maze[row, column] == 'S')
                {
                    startPosition = (row, column);
                }
            }
        }

        List<(int, int)> allPaths = [];
        
        Stack<(int y, int x)> queue = new();
        queue.Push(startPosition);

        while (queue.Count > 0)
        {
            (int y, int x) current = queue.Pop();

            foreach (var (dr, dc) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
            {
                if (maze[dr, dc] == '#') continue;

                if (maze[dr, dc] == '.')
                {
                    queue.Push(current);
                }
            }
        }

        Assert.Equal(expectedAnswer, result);
    }
}