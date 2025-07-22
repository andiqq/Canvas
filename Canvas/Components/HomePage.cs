namespace MauiReactor.Startup.Components;

internal class HomePage : Component
{
    private readonly Shadow myShadow = new Shadow()
        .Brush(Colors.Black)
        .Offset(20, 20)
        .Radius(40)
        .Opacity(80);

    public override VisualNode Render()
        => ContentPage(
            Timer()
                .Interval(10)
                .IsEnabled(true)
                .OnTick(Invalidate),
            GraphicsView()
                .Drawable(new ClockView())
                .Shadow(myShadow)
            );
}

internal class ClockView : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeLineCap = LineCap.Round;
        canvas.FillColor = Colors.Gray;

// Translation and scaling
        canvas.Translate(dirtyRect.Center.X, dirtyRect.Center.Y);
        float scale = Math.Min(dirtyRect.Width / 200f, dirtyRect.Height / 200f);
        canvas.Scale(scale, scale);

// Hour and minute marks
        for (int angle = 0; angle < 360; angle += 6)
        {
            canvas.FillCircle(0, -90, angle % 30 == 0 ? 4 : 2);
            canvas.Rotate(6);
        }

        var now = DateTime.Now;

// Hour hand
        canvas.StrokeSize = 15;
        canvas.SaveState();
        canvas.Rotate(30 * now.Hour + now.Minute / 2f);
        canvas.DrawLine(0, 0, 0, -50);
        canvas.RestoreState();

// Minute hand
        canvas.StrokeSize = 10;
        canvas.SaveState();
        canvas.Rotate(6 * now.Minute + now.Second / 10f);
        canvas.DrawLine(0, 0, 0, -70);
        canvas.RestoreState();

// Second hand
        canvas.StrokeSize = 2;
        canvas.SaveState();
        canvas.Rotate(6 * (now.Millisecond + now.Second * 1000f) / 1000f);
        canvas.DrawLine(0, 10, 0, -80);
        canvas.RestoreState();
    }
}
