using BenchmarkDotNet.Attributes;
using InterviewAssignement.GeometricUtils;

namespace InterviewAssignement;

[MemoryDiagnoser]
public class ShapeBenchmark
{
    private readonly GeometricCalculator _calculator = new(new GeometricParser());

    // Example queries for benchmarking
    private const string RectangleQuery = "side 10 side 900";
    private const string TriangleQuery = "side 100";
    private const string SquareQuery = "side 10 side 10";
    private const string NestedQuery =
        "[[side 15 side 920], [side 30 side 30], [side 45], [side 200 side 200], [side 1000 side 20], [side 50], [side 75 side 75], [side 300], [side 90 side 150], [side 45 side 45], [side 400 side 500], [side 500], [side 600 side 600], [side 70], [side 800 side 800], [side 900], [side 110 side 110], [side 250 side 250], [side 330], [side 450 side 450], [side 560], [side 780 side 780], [side 60 side 60], [side 700], [side 320 side 320], [side 140 side 140], [side 230], [side 410 side 410], [side 520], [side 890 side 890], [side 90], [side 950 side 950], [side 360], [side 470 side 470], [side 580], [side 690 side 690], [side 100], [side 1000 side 1000], [side 50], [side 75 side 75], [side 300], [side 90 side 150], [side 45 side 45], [side 400 side 500], [side 600], [side 150 side 150], [side 210], [side 320 side 320], [side 430], [side 540 side 540], [side 650], [side 770 side 770], [side 120], [side 880 side 880], [side 990], [side 80 side 80], [side 220 side 220], [side 340], [side 450 side 450], [side 560], [side 670 side 670], [side 110], [side 780 side 780], [side 890], [side 130 side 130], [side 240 side 240], [side 350], [side 460 side 460], [side 570], [side 680 side 680], [side 90], [side 790 side 790], [side 1000]]";

    [Benchmark]
    public void BenchmarkNestedZeroAlloc()
    {
        var querySpan = NestedQuery.AsSpan();
        int expectedFigures = GeometricCalculator.GetFiguresCount(ref querySpan);
        Span<(ShapeType, double, double)> figures =
            stackalloc (ShapeType, double, double)[expectedFigures];
        _calculator.CalcZeroAlloc(ref querySpan, figures, expectedFigures);
    }

    [Benchmark]
    public void BenchmarkNestedTokenizer()
    {
        var querySpan = NestedQuery.AsSpan();
        var tokenSpan = GeometricCalculator.TokenizeQuery(ref querySpan, out var figureCount);
        Span<(ShapeType, double, double)> figuresBuffer =
            stackalloc (ShapeType, double, double)[figureCount];
        _calculator.Calc(ref tokenSpan, ref figuresBuffer);
    }

    [Benchmark]
    public void BenchmarkNestedNaive()
    {
        _calculator.CalcNaive(NestedQuery);
    }

    [Benchmark]
    public void BenchmarkRectangleZeroAlloc()
    {
        var querySpan = RectangleQuery.AsSpan();
        int expectedFigures = GeometricCalculator.GetFiguresCount(ref querySpan);
        Span<(ShapeType, double, double)> figures =
            stackalloc (ShapeType, double, double)[expectedFigures];
        _calculator.CalcZeroAlloc(ref querySpan, figures, expectedFigures);
    }

    [Benchmark]
    public void BenchmarkRectangleTokenizer()
    {
        var querySpan = RectangleQuery.AsSpan();
        var tokenSpan = GeometricCalculator.TokenizeQuery(ref querySpan, out var figureCount);
        Span<(ShapeType, double, double)> figuresBuffer =
            stackalloc (ShapeType, double, double)[figureCount];
        _calculator.Calc(ref tokenSpan, ref figuresBuffer);
    }

    [Benchmark]
    public void BenchmarkRectangleNaive()
    {
        _calculator.CalcNaive(RectangleQuery);
    }

    [Benchmark]
    public void BenchmarkSquareZeroAlloc()
    {
        var querySpan = SquareQuery.AsSpan();
        int expectedFigures = GeometricCalculator.GetFiguresCount(ref querySpan);
        Span<(ShapeType, double, double)> figures =
            stackalloc (ShapeType, double, double)[expectedFigures];
        _calculator.CalcZeroAlloc(ref querySpan, figures, expectedFigures);
    }

    [Benchmark]
    public void BenchmarkSquareTokenizer()
    {
        var querySpan = SquareQuery.AsSpan();
        var tokenSpan = GeometricCalculator.TokenizeQuery(ref querySpan, out var figureCount);
        Span<(ShapeType, double, double)> figuresBuffer =
            stackalloc (ShapeType, double, double)[figureCount];
        _calculator.Calc(ref tokenSpan, ref figuresBuffer);
    }

    [Benchmark]
    public void BenchmarkSquareNaive()
    {
        _calculator.CalcNaive(SquareQuery);
    }

    [Benchmark]
    public void BenchmarkTriangleZeroAlloc()
    {
        var querySpan = TriangleQuery.AsSpan();
        int expectedFigures = GeometricCalculator.GetFiguresCount(ref querySpan);
        Span<(ShapeType, double, double)> figures =
            stackalloc (ShapeType, double, double)[expectedFigures];
        _calculator.CalcZeroAlloc(ref querySpan, figures, expectedFigures);
    }

    [Benchmark]
    public void BenchmarkTriangleTokenizer()
    {
        var querySpan = TriangleQuery.AsSpan();
        var tokenSpan = GeometricCalculator.TokenizeQuery(ref querySpan, out var figureCount);
        Span<(ShapeType, double, double)> figuresBuffer =
            stackalloc (ShapeType, double, double)[figureCount];
        _calculator.Calc(ref tokenSpan, ref figuresBuffer);
    }

    [Benchmark]
    public void BenchmarkTriangleNaive()
    {
        _calculator.CalcNaive(TriangleQuery);
    }
}
