public struct PointEC
{
    public int X;
    public int Y;
    public bool IsInfinity;

    public PointEC(int x, int y)
    {
        X = x;
        Y = y;
        IsInfinity = false;
    }

    public static PointEC Infinity()
    {
        return new PointEC { IsInfinity = true };
    }
}
