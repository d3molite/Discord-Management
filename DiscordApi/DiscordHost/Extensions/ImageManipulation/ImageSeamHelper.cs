using OpenCvSharp;

namespace DiscordApi.DiscordHost.Extensions.ImageManipulation;

public enum SeamDirection
{
    Y,
    X
}

public class ImageSeamHelper
{
    // X GOES RIGHT (COLS)
    // Y GOES DOWN (ROWS)
    
    private int _colShifts;
    private int _rowShifts;

    public Mat Source;
    public Mat Weighted;

    public ImageSeamHelper(Mat source, Mat weighted)
    {
        Source = source;
        Weighted = weighted;
    }
    
    public Mat ScaleX()
    {
        var cols = GetColSeams();
        cols.SeamList = cols.SeamList.OrderBy(x => x.AverageEnergy).ToList();

        var slice = cols.SeamList.GetRange(0, cols.SeamList.Count / 5);

        foreach (var seam in slice)
        {
            ShiftColToLeft(seam);
        }

        return Source;
    }

    public Mat ScaleY()
    {
        var rows = GetRowSeams();
        rows.SeamList = rows.SeamList.OrderBy(x => x.AverageEnergy).ToList();

        var slice = rows.SeamList.GetRange(0, rows.SeamList.Count / 5);

        foreach (var seam in slice)
        {
            ShiftRowToTop(seam);
        }

        return Source;
    }

    private Seams GetRowSeams()
    {
        Seams rows = new();

        foreach (var row in Enumerable.Range(0, Source.Rows))
        {
            rows.SeamList.Add(GetRowSeam(row));
        }

        return rows;
    }
    
    private Seams GetColSeams()
    {
        Seams cols = new();

        foreach (var col in Enumerable.Range(0, Source.Cols))
        {
            cols.SeamList.Add(GetColSeam(col));
        }

        return cols;
    }

    private void ShiftColToLeft(Seam seam)
    {
        var input = new Mat<Vec3b>(Source);
        var inputGray = new Mat<byte>(Weighted);

        var indexer = input.GetIndexer();
        var indexerGray = inputGray.GetIndexer();

        foreach (var position in seam.Pixels)
        {
            for (var col = position.ColumnX; col > _colShifts; col--)
            {
                if (col - 1 < _colShifts) continue;

                var currentPixel = indexer[position.RowY, col];
                var currentPixelGray = indexerGray[position.RowY, col];

                var pixelAbove = indexer[position.RowY, col - 1];
                var pixelAboveGray = indexerGray[position.RowY, col - 1];

                indexer[position.RowY, col] = pixelAbove;
                indexer[position.RowY, col - 1] = currentPixel;
                
                indexerGray[position.RowY, col] = pixelAboveGray;
                indexerGray[position.RowY, col - 1] = currentPixelGray;
            }
        }

        _colShifts++;

        Source = input;
        Weighted = inputGray;
    }

    private void ShiftRowToTop(Seam seam)
    {
        var input = new Mat<Vec3b>(Source);
        var inputGray = new Mat<byte>(Weighted);

        var indexer = input.GetIndexer();
        var indexerGray = inputGray.GetIndexer();

        foreach (var position in seam.Pixels)
        {
            for (var row = position.RowY; row > _rowShifts; row--)
            {
                if (row - 1 < _rowShifts) continue;

                var currentPixel = indexer[row, position.ColumnX];
                var currentPixelGray = indexerGray[row, position.ColumnX];

                var pixelAbove = indexer[row - 1, position.ColumnX];
                var pixelAboveGray = indexerGray[row - 1, position.ColumnX];

                indexer[row, position.ColumnX] = pixelAbove;
                indexer[row - 1, position.ColumnX] = currentPixel;
                
                indexerGray[row, position.ColumnX] = pixelAboveGray;
                indexerGray[row - 1, position.ColumnX] = currentPixelGray;
            }
        }

        _rowShifts++;

        Source = input;
        Weighted = inputGray;
    }
    
    private void PaintSeam(Seam seam)
    {
        var input = new Mat<Vec3b>(Source);
        var indexer = input.GetIndexer();

        foreach (var pixel in seam.Pixels)
        {
            indexer[pixel.RowY, pixel.ColumnX] = new Vec3b(255, 0, (byte)seam.HighestEnergy);
        }

        Source = input;
    }

    private Seam GetRowSeam(int row)
    {
        var seam = new Seam();

        for (var column = _colShifts + 1; column < Source.Cols; column++)
        {
            var indexer = new Mat<byte>(Weighted).GetIndexer();
            var value = (int)indexer[row, column];

            seam.Pixels.Add(new Pixel(column, row, value));
        }

        return seam;
    }
    
    private Seam GetColSeam(int col)
    {
        var seam = new Seam();

        for (var row = _rowShifts + 1; row < Source.Rows; row++)
        {
            var indexer = new Mat<byte>(Weighted).GetIndexer();
            var value = (int)indexer[row, col];

            seam.Pixels.Add(new Pixel(col, row, value));
        }

        return seam;
    }

    public Mat Resize(Mat squooshed)
    {
        var size = squooshed.Size();
        squooshed = squooshed[_rowShifts, squooshed.Rows, _colShifts, squooshed.Cols];
        Cv2.Resize(squooshed, squooshed, size);
        return squooshed;
    }

    public class Pixel
    {
        public Pixel(int columnX, int rowY, int value = 0)
        {
            ColumnX = columnX;
            RowY = rowY;
            Energy = value;
        }

        public int ColumnX { get; set; }
        public int RowY { get; set; }
        public int Energy { get; set; }
    }

    public class Seam
    {
        public Seam()
        {
            Pixels = new List<Pixel>();
        }

        public List<Pixel> Pixels { get; set; }
        public double AverageEnergy => Pixels.DistinctBy(x => x.Energy).Average(x => x.Energy);
        public double HighestEnergy => Pixels.Max(x => x.Energy);
    }

    public class Seams
    {
        public Seams()
        {
            SeamList = new List<Seam>();
        }

        public List<Seam> SeamList { get; set; }
    }
}