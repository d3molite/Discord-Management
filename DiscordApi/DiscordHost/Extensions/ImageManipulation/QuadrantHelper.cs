using OpenCvSharp;

namespace DiscordApi.DiscordHost.Extensions.ImageManipulation;

public class QuadrantHelper
{
    private readonly int _halfHeight;
    private readonly int _halfWidth;
    private readonly int _height;
    private readonly Point[] _masks;

    private readonly Point[] _points;
    private readonly int _width;

    public QuadrantHelper(int width, int height)
    {
        _width = width;
        _height = height;

        _halfHeight = height / 2;
        _halfWidth = width / 2;

        _points = InitializePoints();
        _masks = InitializeMasks();
    }

    public Point Center => _points[0];

    public Point[] GetMaskTriangle(int quadrant)
    {
        switch (quadrant)
        {
            default:
            case 1:
                return new[] { _masks[0], _masks[1], _masks[2] };
            case 2:
                return new[] { _masks[0], _masks[2], _masks[3] };
            case 3:
                return new[] { _masks[1], _masks[2], _masks[3] };
            case 4:
                return new[] { _masks[0], _masks[1], _masks[3] };
            case 5:
                return new[] { _masks[2], _masks[3], _masks[0] };
            case 6:
                return new[] { _masks[2], _masks[0], _masks[1] };
            case 7:
                return new[] { _masks[2], _masks[0], _masks[1] };
            case 8:
                return new[] { _masks[3], _masks[1], _masks[2] };
        }
    }

    public static FlipMode GetFlipMode(int slice)
    {
        switch (slice)
        {
            default:
            case 1:
            case 2:
                return FlipMode.X;
            case 3:
            case 4:
                return FlipMode.Y;
            case 5:
            case 6:
                return FlipMode.X;
            case 7:
            case 8:
                return FlipMode.Y;
        }
    }

    public Rect GetBoundingBox(int quadrant)
    {
        switch (quadrant)
        {
            default:
            case 1:
            case 2:
                return new Rect(_halfWidth, 0, _halfWidth, _halfHeight);
            case 3:
            case 4:
                return new Rect(_halfWidth, _halfHeight, _halfWidth, _halfHeight);
            case 5:
            case 6:
                return new Rect(0, _halfHeight, _halfWidth, _halfHeight);
            case 7:
            case 8:
                return new Rect(0, 0, _halfWidth, _halfHeight);
        }
    }

    public static int Normalize(int input, int max = 8)
    {
        if (input > max)
        {
            return input % max;
        }

        return input;
    }

    private Point[] InitializePoints()
    {
        return new[]
        {
            new Point(_halfWidth, _halfHeight), // 0
            new Point(_halfWidth, 0), // 1
            new Point(_width, 0), // 2
            new Point(_width, _halfHeight), // 3
            new Point(_width, _height), // 4
            new Point(_halfWidth, _height), // 5
            new Point(0, _height), // 6
            new Point(0, _halfHeight), // 7
            new Point(0, 0) // 8
        };
    }

    private Point[] InitializeMasks()
    {
        return new[]
        {
            new Point(0, _halfHeight), // Bottom Left
            new Point(0, 0), // Top Left
            new Point(_halfWidth, 0), // Top Right
            new Point(_halfWidth, _halfHeight) // Bottom Right
        };
    }
}