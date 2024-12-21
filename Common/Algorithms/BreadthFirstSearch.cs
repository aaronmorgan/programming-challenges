using Common.Types;

namespace Common.Algorithms;

public class BreadthFirstSearch
{
    /// <summary>
    /// Standard BFS that looks up, right, down and left. It takes a 2d char array and two <see cref="Point"/>
    /// </summary>
    public static List<Point> GenericBfs(char[,] array, Point startPos, Point endPos)
    {
        Queue<(Point pos, List<Point> path)> queue = new();
        HashSet<Point> seen = [];

        queue.Enqueue((startPos, []));

        while (queue.Count > 0)
        {
            var (currentPos, currentPath) = queue.Dequeue();

            // We've been on this vertex before, from this direction.
            if (!seen.Add(currentPos)) continue;

            // Check we're at the end position.
            if (currentPos == endPos)
            {
                return currentPath;
            }

            foreach (var (dr, dc) in new[] { (-1, 0), (0, 1), (1, 0), (0, -1) })
            {
                int y = currentPos.Y + dr;
                int x = currentPos.X + dc;

                // Are we looking around at an already traversed position?
                if (seen.Contains(new Point(x, y))) continue;

                // Bounds checking.
                if (y < 0 || y > array.GetLength(0) - 1 ||
                    x < 0 || x > array.GetLength(1) - 1) continue;

                if (array[y, x] == '#') continue;

                // Queue the next adjacent position and the direction we're moving to get there.
                queue.Enqueue((new Point(x, y), [..currentPath, new Point(x, y)]));
            }
        }

        return default!;
    }
}
