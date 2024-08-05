
# Shape Parser Project

This project is designed to parse shape descriptions and convert them into structured data. The objective is to improve the parsing process from a naive approach to a more efficient one, ultimately eliminating unnecessary memory allocation.

## Project Overview

Given a string representing shapes and their dimensions, such as:

```plaintext
side 10 side 900 --> rectangle
side 100 --> triangle
side 10 side 10 --> square
[[side 10 side 900], [side 10]] --> rectangle, triangle
```

The goal is to parse this input into a structured format: `(ShapeType, double, double)[]`, where `ShapeType` can be `Triangle`, `Square`, or `Rectangle`.

## Approaches

The project contains three different approaches to parsing the input:

1. **CalcNaive**: 
   - This is the initial naive approach that uses tokenization to parse the input.
   - It validates and tokenizes the input, even though the input is considered error-free and non-misleading.

2. **Calc**: 
   - This approach also uses tokenization but improves on the memory allocation compared to the naive method.
   - It provides a balance between simplicity and performance, although tokenization is still used unnecessarily.

3. **CalcZeroAlloc**:
   - The most efficient approach, eliminating unnecessary memory allocation by avoiding tokenization.
   - This method directly parses the input, leveraging the assumption that the input is always correct and not misleading.

## Why Avoid Tokenization?

The tokenization approach in `CalcNaive` and `Calc` is not necessary if the initial input is guaranteed to be error-free. Thus, the `CalcZeroAlloc` approach provides the best performance by directly parsing the input without additional validation.

## Benchmarks
![image](https://github.com/user-attachments/assets/ebf8a585-0e30-4bf8-8772-79bbe5d4d8a7)


