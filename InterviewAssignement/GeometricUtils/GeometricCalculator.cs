using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using InterviewAssignement.GeometricUtils;

class GeometricCalculator : IGeometricCalculator
{
    private readonly GeometricParser _parser;

    public GeometricCalculator(GeometricParser parser)
    {
        _parser = parser;
    }

    public (ShapeType, double, double)[] CalcNaive(string query)
    {
        var tokenList = TokenizeQueryNaive(query);

        int f = default;
        int s = default;
        var firstAssigned = false;

        var processedFigures = new List<(ShapeType, double, double)>();
        foreach (var item in tokenList)
        {
            if (item.TokenType == TokenType.Number)
            {
                int number = int.Parse(item.Value);

                if (!firstAssigned)
                {
                    f = number;
                    firstAssigned = true;
                }
                else
                {
                    s = number;
                }
            }
            else if (item.TokenType == TokenType.Comma)
            {
                processedFigures.Add(_parser.GetFigureData(ref f, ref s));

                f = default;
                s = default;
                firstAssigned = false;
            }
        }

        processedFigures.Add(_parser.GetFigureData(ref f, ref s));

        return processedFigures.ToArray();
    }

    private static List<Token> TokenizeQueryNaive(string query)
    {
        var tokenList = new List<Token>();
        for (int i = 0; i < query.Length; i++)
        {
            if (query[i] == '[' || query[i] == ']')
            {
                tokenList.Add(
                    new Token() { TokenType = TokenType.Bracket, Value = query[i].ToString() }
                );
            }
            else if (query[i] == 's')
            {
                var value = string.Empty;
                tokenList.Add(new Token() { TokenType = TokenType.Side, Value = "side" });
                i += 2;
            }
            else if (char.IsDigit(query[i]))
            {
                int number = 0;
                while (i < query.Length && char.IsDigit(query[i]))
                {
                    if (number == 0)
                    {
                        number = query[i] - '0';
                    }
                    else
                    {
                        number = (number * 10) + (query[i] - '0');
                    }

                    i++;
                }

                tokenList.Add(
                    new Token() { TokenType = TokenType.Number, Value = number.ToString() }
                );
            }
            else if (query[i] == ',')
            {
                tokenList.Add(
                    new Token() { TokenType = TokenType.Comma, Value = query[i].ToString() }
                );
            }
        }

        return tokenList;
    }

    public Span<(ShapeType, double, double)> Calc(
        ref readonly ReadOnlySpan<Token> tokenList,
        ref readonly Span<(ShapeType, double, double)> outputBuffer
    )
    {
        int f = default;
        int s = default;
        var firstAssigned = false;

        var processedFigures = 0;
        foreach (var item in tokenList)
        {
            if (item.TokenType == TokenType.Number)
            {
                int number = int.Parse(item.Value);

                if (!firstAssigned)
                {
                    f = number;
                    firstAssigned = true;
                }
                else
                {
                    s = number;
                }
            }
            else if (item.TokenType == TokenType.Comma)
            {
                outputBuffer[processedFigures++] = _parser.GetFigureData(ref f, ref s);

                f = default;
                s = default;
                firstAssigned = false;
            }
        }

        outputBuffer[processedFigures++] = _parser.GetFigureData(ref f, ref s);

        return outputBuffer;
    }

    public static ReadOnlySpan<Token> TokenizeQuery(
        ref readonly ReadOnlySpan<char> span,
        out int figureCount
    )
    {
        figureCount = 1;

        ref var startOne = ref MemoryMarshal.GetReference(span);
        ref var endOne = ref Unsafe.Add(ref startOne, span.Length);

        var tokenCount = 0;
        while (Unsafe.IsAddressLessThan(ref startOne, ref endOne))
        {
            if (startOne == '[' || startOne == ']')
            {
                tokenCount++;
            }
            else if (startOne == 's')
            {
                tokenCount++;
            }
            else if (char.IsDigit(startOne))
            {
                int number = 0;
                while (Unsafe.IsAddressLessThan(ref startOne, ref endOne) && char.IsDigit(startOne))
                {
                    if (number == 0)
                    {
                        number = startOne - '0';
                    }
                    else
                    {
                        number = (number * 10) + (startOne - '0');
                    }

                    startOne = ref Unsafe.Add(ref startOne, 1);
                }

                tokenCount++;
            }
            else if (startOne == ',')
            {
                tokenCount++;
            }

            startOne = ref Unsafe.Add(ref startOne, 1);
        }

        ref var start = ref MemoryMarshal.GetReference(span);
        ref var end = ref Unsafe.Add(ref start, span.Length);

        Span<Token> tokenList = new Token[tokenCount];
        var tempCount = 0;

        Span<char> numbers = stackalloc char[10];
        Span<char> bracket = stackalloc char[1];
        while (Unsafe.IsAddressLessThan(ref start, ref end))
        {
            if (start == '[' || start == ']')
            {
                tokenList[tempCount++] = new Token()
                {
                    TokenType = TokenType.Bracket,
                    Value = start.ToString()
                };
            }
            else if (start == 's')
            {
                var value = string.Empty;
                tokenList[tempCount++] = new Token() { TokenType = TokenType.Side, Value = "side" };
                start = ref Unsafe.Add(ref start, 2);
            }
            else if (char.IsDigit(start))
            {
                int number = 0;
                while (Unsafe.IsAddressLessThan(ref start, ref end) && char.IsDigit(start))
                {
                    if (number == 0)
                    {
                        number = start - '0';
                    }
                    else
                    {
                        number = (number * 10) + (start - '0');
                    }

                    start = ref Unsafe.Add(ref start, 1);
                }

                tokenList[tempCount++] = new Token()
                {
                    TokenType = TokenType.Number,
                    Value = number.ToString()
                };
            }
            else if (start == ',')
            {
                tokenList[tempCount++] = new Token()
                {
                    TokenType = TokenType.Comma,
                    Value = start.ToString()
                };

                figureCount++;
            }

            start = ref Unsafe.Add(ref start, 1);
        }

        return tokenList;
    }

    public void CalcZeroAlloc(
        ref readonly ReadOnlySpan<char> querySpan,
        Span<(ShapeType, double, double)> outputBuffer,
        int figuresCount
    )
    {
        Span<int> buffer = stackalloc int[figuresCount * 2];
        var figureList = GetSidesLength(in querySpan, buffer);

        var figureNum = 0;
        for (int i = 0; i < figureList.Length; i++)
        {
            var firstSide = figureList[i++];
            var secondSide = figureList[i];

            outputBuffer[figureNum] = _parser.GetFigureData(ref firstSide, ref secondSide);
            figureNum++;
        }
    }

    public static int GetFiguresCount(ref readonly ReadOnlySpan<char> charSpan)
    {
        ref var startOne = ref MemoryMarshal.GetReference(charSpan);
        ref var endOne = ref Unsafe.Add(ref startOne, charSpan.Length);

        var figuresCount = 1;
        while (Unsafe.IsAddressLessThan(ref startOne, ref endOne))
        {
            if (startOne == ',')
            {
                figuresCount++;
            }
            startOne = ref Unsafe.Add(ref startOne, 1);
        }

        return figuresCount;
    }

    private static ReadOnlySpan<int> GetSidesLength(
        ref readonly ReadOnlySpan<char> charSpan,
        Span<int> buffer
    )
    {
        int f = default;
        int s = default;
        var firstAssigned = false;

        ref var start = ref MemoryMarshal.GetReference(charSpan);
        ref var end = ref Unsafe.Add(ref start, charSpan.Length);

        int bufferIndex = 0;
        while (Unsafe.IsAddressLessThan(ref start, ref end))
        {
            if (start == 's')
            {
                start = ref Unsafe.Add(ref start, 2);
            }
            else if (char.IsDigit(start))
            {
                int number = 0;
                while (Unsafe.IsAddressLessThan(ref start, ref end) && char.IsDigit(start))
                {
                    if (number == 0)
                    {
                        number = start - '0';
                    }
                    else
                    {
                        number = (number * 10) + (start - '0');
                    }

                    start = ref Unsafe.Add(ref start, 1);
                }

                if (!firstAssigned)
                {
                    f = number;
                    firstAssigned = true;
                }
                else
                {
                    s = number;
                }
            }
            else if (start == ',')
            {
                buffer[bufferIndex++] = f;
                buffer[bufferIndex++] = s;

                f = default;
                s = default;
                firstAssigned = false;
            }

            start = ref Unsafe.Add(ref start, 1);
        }

        buffer[bufferIndex++] = f;
        buffer[bufferIndex++] = s;

        return buffer;
    }
}
