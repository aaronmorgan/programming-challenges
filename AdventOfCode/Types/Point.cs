﻿namespace AdventOfCode.Types;

/// <summary>
/// Generic Point type for, points.
/// </summary>
public readonly struct Point(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Point other && X == other.X && Y == other.Y;
}