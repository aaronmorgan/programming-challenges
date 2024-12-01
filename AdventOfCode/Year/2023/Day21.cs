// using System.Collections.Concurrent;
// using System.Net;
// using AdventOfCode.Extensions;
// using AdventOfCode.Utilities;
// using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
//
// namespace AdventOfCode._2023;
//
// public class Day21
// {
//     // Part 2: go through all possible combinations of x m a s 1-4000 and identify which are accepted and return the score.
//     [Theory]
//     [InlineData("Day21DevelopmentTesting1.txt", 6, 16)]
//     // [InlineData("Day21.txt", 397061)]
//     public void Day19_Part1_StepCounter(string filename, int maxSteps, int expectedAnswer)
//     {
//         var input = InputParser.ReadAllLines("2023/" + filename).ToArray();
//         char[,] arr = new char[input.Length, input[0].Length];
//
//         Point? startLocation = null;
//
//         // Convert the input file into a 2D char array.
//         for (int i = 0; i < input.Length; i++)
//         {
//             var line = input[i];
//
//             for (int j = 0; j < line.Length; j++)
//             {
//                 arr[i, j] = line[j];
//
//                 if (line[j] == 'S')
//                 {
//                     startLocation = new Point(j, i);
//                 }
//             }
//         }
//
//         Assert.NotNull(startLocation);
//
//         int result = 0;
//
//         // Point searchStart = new Point(startLocation.X - maxSteps, startLocation.Y - maxSteps);
//         //
//         // for (int i = searchStart.Y; i <= input.Length - startLocation.Y  +maxSteps; i++)
//         // {
//         //     var line = input[i];
//         //
//         //     for (int j = searchStart.X; j <= startLocation.X + maxSteps; j++)
//         //     {
//         //         if (arr[i, j] == '#') continue;
//         //     }
//         // }     
//
//         Stack<Point> queue = new();
//         queue.Push(new Point(startLocation.Y, startLocation.X, startLocation.stepsRemaining));
//
//         var directions = new[]
//         {
//             (-1, 0),
//             (1, 0),
//             (0, -1),
//             (0, 1)
//         };
//
//         while (queue.Count > 0)
//         {
//             var item = queue.Pop();
//
//             foreach (var a in directions)
//                 {
//                     doit(a, item, maxSteps);
//                     }
//             //
//             // doit(Direction.Up, item, maxSteps);
//             // doit(Direction.Right, item, maxSteps);
//             // doit(Direction.Down, item, maxSteps);
//             // doit(Direction.Left, item, maxSteps);
//         }
//
//         doit(Direction.Up, startLocation, maxSteps);
//
//
//         arr.WriteToFile("C:/temp/Day21_before.txt");
//
//
//         return;
//
//         void doit((int row, int col) direction, Point currentLocation, int stepsRemaining)
//         {
//             switch (direction)
//             {
//                 // case Direction.Up: { currentLocation.Y--; break; }
//                 // case Direction.Right: { currentLocation.Y--; break; }
//                 // case Direction.Down: { currentLocation.Y--; break; }
//                 // case Direction.Left: { currentLocation.Y--; break; }
//             }
//
//             if (maxSteps % 2 == 0 && stepsRemaining % 2 == 0)
//             {
//             }
//             else
//             {
//                 switch (arr[currentLocation.Y, currentLocation.X])
//                 {
//                     case '.':
//                     {
//                         arr[currentLocation.Y, currentLocation.X] = 'O';
//                         break;
//                     }
//                     case '#':
//                     default:
//                     {
//                         return;
//                     }
//                 }
//
//                 // if (arr[currentLocation.Y, currentLocation.X] == '.')
//                 // {
//                 // }
//             }
//
//             //     if (direction == Direction.Up) doit(Direction.Up, new Point(currentLocation.X, currentLocation.Y - 1), stepsRemaining--);
//         }
//
//         arr.WriteToFile("C:/temp/Day21_after.txt");
//
//
//         Assert.Equal(expectedAnswer, result);
//     }
//
//     private record Point(int X, int Y, int stepsRemaining = 0);
//
//     // private enum Direction
//     // {
//     //     Up,
//     //     Right,
//     //     Down,
//     //     Left
//     // }
// }