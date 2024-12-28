using Common.Types;

namespace Common.Utilities;

public static class ConsoleUtilities
{
    // TODO: Create 'setup Console' function that returns the console configuration as an object. Then DrawToConsole only needs to receive that
    // information and draw the data, rather than recreate the console situation on each call.

    /// <summary>
    /// 
    /// </summary>
    /// <param name="array">The 2d char array to draw.</param>
    /// <param name="location">The current location being processed as an overlay icon.</param>
    /// <param name="path">Draws the path as an overlay on the grid.</param>
    /// <param name="delayMs">The draw delay period in millisecons.</param>
    /// <param name="iconColor"></param>
    public static void DrawToConsole(
        this char[,] array,
        Point location = default,
        List<(int y, int x)>? path = default,
        int delayMs = 25,
        ConsoleColor iconColor = ConsoleColor.DarkYellow)
    {
        Console.Clear();
        Console.CursorVisible = false;

        Console.WindowWidth = Math.Min(Math.Max(80, array.GetLength(1)), Console.LargestWindowWidth);
        Console.WindowHeight = Math.Min(Math.Max(80, array.GetLength(0)), Console.LargestWindowHeight);

        var startRow = 0;
        var startColumn = 0;
        var endRow = array.GetLength(0);
        var endColumn = array.GetLength(1);

        // If the array is still too large for the console window draw what we can with the 'location' at the center.
        if (Console.WindowHeight < array.GetLength(0) || Console.WindowWidth < array.GetLength(1))
        {
            startRow = Math.Max(0, location.Y - 20);
            endRow = Math.Max(0, location.Y + 20);
            startColumn = Math.Max(0, location.X - 20);
            endColumn = Math.Max(0, location.X + 20);
        }

        // SetBufferSize() is Windows only.
        // if (OperatingSystem.IsWindows())
        // {
        //     Console.SetBufferSize(Math.Max(Console.WindowWidth, endColumn - startColumn),
        //         Math.Max(Console.WindowHeight, endRow - startRow));
        // }

        for (var row = startRow; row < endRow; row++)
        {
            for (var col = startColumn; col < endColumn; col++)
            {
                if (row < 0 || row >= array.GetLength(0) || col < 0 || col >= array.GetLength(1)) continue;

                Console.SetCursorPosition(col - startColumn, row - startRow);
                Console.Write(array[row, col]);
            }
        }

        if (location != default)
        {
            DrawIconAtPosition(location.X - startColumn, location.Y - startRow, iconColor);
        }

        if (path != null && path.Count != 0)
        {
            var originalLeft = Console.CursorLeft;
            var originalTop = Console.CursorTop;
            var originalColor = Console.ForegroundColor;


            Console.ForegroundColor = iconColor;
            foreach ((int y, int x) point in path)
            {
                Console.SetCursorPosition(point.y, point.x);
                Console.Write('\u2588');
            }

            Console.SetCursorPosition(originalLeft, originalTop);
            Console.ForegroundColor = originalColor;
        }

        Task.Delay(delayMs).Wait();
    }

    private static void DrawIconAtPosition(int x, int y, ConsoleColor color)
    {
        var originalLeft = Console.CursorLeft;
        var originalTop = Console.CursorTop;
        var originalColor = Console.ForegroundColor;

        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;

        Console.Write('\u2588');

        Console.SetCursorPosition(originalLeft, originalTop);
        Console.ForegroundColor = originalColor;
    }
}