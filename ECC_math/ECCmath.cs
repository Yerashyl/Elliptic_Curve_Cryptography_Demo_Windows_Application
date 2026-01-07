public static class ECCMath
{
    public static int p = 97;
    public static int a = 2;

    static int ModInverse(int k)
    {
        k = (k % p + p) % p;
        for (int i = 1; i < p; i++)
            if ((k * i) % p == 1)
                return i;
        throw new Exception("No inverse");
    }

    public static PointEC Add(PointEC P, PointEC Q)
    {
        if (P.IsInfinity) return Q;
        if (Q.IsInfinity) return P;

        if (P.X == Q.X && P.Y != Q.Y)
            return PointEC.Infinity();

        int lambda;
        if (P.X == Q.X && P.Y == Q.Y)
        {
            lambda = (3 * P.X * P.X + a) * ModInverse(2 * P.Y) % p;
        }
        else
        {
            lambda = (Q.Y - P.Y) * ModInverse(Q.X - P.X) % p;
        }

        int xr = (lambda * lambda - P.X - Q.X) % p;
        int yr = (lambda * (P.X - xr) - P.Y) % p;

        xr = (xr + p) % p;
        yr = (yr + p) % p;

        return new PointEC(xr, yr);
    }

    public static PointEC ScalarMult(int k, PointEC G)
    {
        PointEC R = PointEC.Infinity();
        PointEC P = G;

        while (k > 0)
        {
            if ((k & 1) == 1)
                R = Add(R, P);

            P = Add(P, P);
            k >>= 1;
        }
        return R;
    }
}
