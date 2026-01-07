void DrawCurve(Graphics g, PointEC? highlight = null, PointEC? result = null)
{
    g.Clear(Color.White);

    foreach (var pt in curvePoints)
    {
        g.FillEllipse(Brushes.LightGray,
            pt.X * scale, pt.Y * scale, 5, 5);
    }

    if (highlight.HasValue)
        g.FillEllipse(Brushes.Red,
            highlight.Value.X * scale,
            highlight.Value.Y * scale, 8, 8);

    if (result.HasValue)
        g.FillEllipse(Brushes.Blue,
            result.Value.X * scale,
            result.Value.Y * scale, 8, 8);
}
