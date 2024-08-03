namespace InterviewAssignement.GeometricUtils;

public class GeometricParser
{
    public (ShapeType, double, double) GetFigureData(
        ref readonly int firstSide,
        ref readonly int secondSide
    )
    {
        return (firstSide, secondSide) switch
        {
            (var a, var b) when a == b => (ShapeType.Square, 4 * a, a * a),
            (_, not 0)
                => (
                    ShapeType.Rectangle,
                    2 * (firstSide + (double)secondSide!),
                    firstSide * (double)secondSide!
                ),
            (_, 0) => (ShapeType.Triangle, 3 * firstSide, Math.Sqrt(3) / 4 * firstSide * firstSide)
        };
    }
}
