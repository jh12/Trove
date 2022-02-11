namespace Trove.DataModels.Metadata;

public class Size
{
    public int Height { get; init; }
    public int Width { get; init; }

    public Size()
    {
    }

    public Size(int height, int width)
    {
        Height = height;
        Width = width;
    }
}