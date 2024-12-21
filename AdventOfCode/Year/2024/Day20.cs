using System.Diagnostics;
using Common.Algorithms;
using Common.Types;
using Common.Utilities;

namespace AdventOfCode.Year._2024;

public class Day20
{
    [Theory]
    [InlineData("Day20DevelopmentTesting1.txt", 2,0, 14)]
    [InlineData("Day20DevelopmentTesting1.txt", 4,0, 14)]
    [InlineData("Day20DevelopmentTesting1.txt", 8,0, 4)]
    [InlineData("Day20DevelopmentTesting1.txt", 40,0, 1)]
    [InlineData("Day20DevelopmentTesting1.txt", 64, 0,1)]
     [InlineData("Day20.txt", 100,100, 0)]
    public void Day20_Race_Condition(string filename, int savedPicoSeconds, int minimumThreshold, int expectedResult)
    {
        char[,] input = InputParser.ReadAllChars("2024/" + filename);

        var startPosition = new Point(0, 0);
        var endPosition = new Point(0, 0);

        for (var row = 0; row < input.GetLength(0); row++)
        {
            for (var col = 0; col < input.GetLength(1); col++)
            {
                if (input[row, col] == 'S')
                {
                    startPosition = new Point(col, row);
                }

                if (input[row, col] == 'E')
                {
                    endPosition = new Point(col, row);
                }
            }
        }

        List<Point> path = BreadthFirstSearch.GenericBfs(input, startPosition, endPosition);

        List<Point> tail = new List<Point>();

        Dictionary<int, int> counts = new Dictionary<int, int>();
        Point newStartPos = new Point(0, 0);

            int t = 0;

        foreach (var p in path)
        {
            tail.Add(p);
            
            if (tail.Count <= minimumThreshold) continue;

            for (var index = 0; index < tail.Count - minimumThreshold; index++)
            {
                var q = tail[index];
                // Have we passed a cell that's just on the other side of a wall?
                if (q.X == p.X - 2 && p.Y == q.Y ||
                    q.X == p.X + 2 && p.Y == q.Y ||
                    q.Y == p.Y - 2 && p.X == q.X ||
                    q.Y == p.Y + 2 && p.X == q.X)
                {
                    int x = 0, y = 0;

                    Point a = new Point(0, 0);

                    if (p.X == q.X)
                    {
                        x = p.X;
                        if (p.Y > q.Y) y = p.Y - 1;
                        if (p.Y < q.Y) y = p.Y + 1;
                    }
                    else
                    {
                        y = p.Y;
                        if (p.X > q.X) x = p.X - 1;
                        if (p.X < q.X) x = p.X + 1;
                    }

                    // We're only interested in connected cells with one wall between them.
                    if (input[y, x] != '#') continue;

                    //    char[,] c = new char[input.GetLength(1), input.GetLength(0)];
                    var c = input.Clone() as char[,];

                    c[y, x] = '.';
                    var u = tail[index+1];
                    c[u.Y, u.X] = '#';

                    
                    // ConsoleUtilities.DrawToConsole(c, (y, x));

                    newStartPos = q;

                    var d = BreadthFirstSearch.GenericBfs(c, startPosition, endPosition);
                    var z = path.Count - d.Count;
                    
                    if (!counts.TryAdd(z, 1))
                    {
                        counts[z] += 1;
                    }

                       Console.WriteLine($"{z}: {counts[z]}");
                }
            }
            
            
            t++;
        }

        int result = 0;

    //    Assert.Equal(expectedResult, counts[savedPicoSeconds]);
        
        var e = 0;
        
        foreach (var a in counts){
            if (a.Key >= 100) e++;
            }
        
Assert.Equal(expectedResult, e);
    }
}
