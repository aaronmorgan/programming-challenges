using Common.Types;

namespace Common.Algorithms;

public static class BreadthFirstSearch
{
    /// <summary>
    /// Standard BFS that takes a start and end position and returns a List of <see cref="Point"/> from start to
    /// end, i.e. the path.
    /// </summary>
    public static List<Point> FindPath(char[,] array, Point startPos, Point endPos, char obstacle = '#')
    {
        Queue<(Point pos, List<Point> path)> queue = new();
        HashSet<Point> seen = [];

        queue.Enqueue((startPos, []));

        while (queue.Count > 0)
        {
            var (currentPos, currentPath) = queue.Dequeue();

            // We've been on this position before, from this direction.
            if (!seen.Add(currentPos)) continue;

            // Check we're at the end position.
            if (currentPos == endPos)
            {
                return currentPath;
            }

            foreach (var (dc, dr) in new[] { (0, -1), (1, 0), (0, 1), (-1, 0) })
            {
                var p = new Point(currentPos.X + dc, currentPos.Y + dr);

                // Are we looking around at an already traversed position?
                if (seen.Contains(p)) continue;

                // Bounds checking.
                if (p.X < 0 || p.X > array.GetLength(1) - 1 || p.Y < 0 || p.Y > array.GetLength(0) - 1) continue;

                // We've strayed onto a position of a different type.
                if (array[p.Y, p.X] == obstacle) continue;

                // Queue the next adjacent position and the direction we're moving to get there.
                queue.Enqueue((p, [..currentPath, p]));
            }
        }

        return default!;
    }

    /// <summary>
    /// Works like a 'flood fill search' algorithm. Returns a List of <see cref="Point"/> locations that are touching
    /// the startPos location and are of the same char type. 
    /// </summary>
    public static List<Point> GetConnectedCells(char[,] array, Point startPos, char type)
    {
        HashSet<Point> connectedCells =
        [
            startPos
        ];

        Queue<(Point pos, List<Point> region)> queue = new();
        HashSet<Point> seen = [];

        queue.Enqueue((startPos, []));

        while (queue.Count > 0)
        {
            var (currentPos, region) = queue.Dequeue();

            // We've been on this position before.
            if (!seen.Add(currentPos)) continue;

            foreach (var (dc, dr) in new[] { (0, -1), (1, 0), (0, 1), (-1, 0) })
            {
                var p = new Point(currentPos.X + dc, currentPos.Y + dr);

                if (seen.Contains(p)) continue;

                // Bounds checking.
                if (p.X < 0 || p.X > array.GetLength(1) - 1 || p.Y < 0 || p.Y > array.GetLength(0) - 1) continue;

                // We've strayed onto a position of a different type.
                if (array[p.Y, p.X] != type) continue;

                // Queue the next adjacent position and the direction we're moving to get there.
                queue.Enqueue((p, [..region, p]));

                connectedCells.Add(p);
            }
        }

        return connectedCells.ToList();
    }
}