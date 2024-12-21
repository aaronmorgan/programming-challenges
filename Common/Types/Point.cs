namespace Common.Types;

/// <summary>
/// Generic Point type for, points.
/// </summary>
public readonly struct Point(int x, int y) : IEquatable<Point>
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Point other && X == other.X && Y == other.Y;
    
    /// <inheritdoc />
    public bool Equals(Point other)
    {
        return X == other.X && Y == other.Y;
    }

    public static bool operator ==(Point left, Point right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Point left, Point right)
    {
        return !(left == right);
    }
    
    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
}
