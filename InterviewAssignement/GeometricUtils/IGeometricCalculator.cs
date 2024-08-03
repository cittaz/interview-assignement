using InterviewAssignement.GeometricUtils;

public interface IGeometricCalculator
{
    Span<(ShapeType, double, double)> Calc(
        ref readonly ReadOnlySpan<Token> tokenList,
        ref readonly Span<(ShapeType, double, double)> outputBuffer
    );
    (ShapeType, double, double)[] CalcNaive(string query);
    void CalcZeroAlloc(
        ref readonly ReadOnlySpan<char> querySpan,
        Span<(ShapeType, double, double)> outputBuffer,
        int figuresCount
    );
}
