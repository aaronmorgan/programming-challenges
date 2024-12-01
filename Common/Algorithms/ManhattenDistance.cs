using Common.Types;

namespace Common.Algorithms;

public class ManhattenDistance
{
    /// <summary>
    /// Calculate Manhattan distance between two points in 2D space.
    /// </summary>
    public static int Calculate2D(Point p1, Point p2)
    {
        return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
    }
}