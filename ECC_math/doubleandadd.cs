Queue<char> bits;
PointEC P, R;

void StartAnimation(int d)
{
    bits = new Queue<char>(Convert.ToString(d, 2));
    P = G;
    R = PointEC.Infinity();
    timer.Start();
}

void timer_Tick(object sender, EventArgs e)
{
    if (bits.Count == 0)
    {
        timer.Stop();
        return;
    }

    char bit = bits.Dequeue();

    // Double
    P = ECCMath.Add(P, P);

    if (bit == '1')
        R = ECCMath.Add(R, P);

    pictureBox.Invalidate();
}
