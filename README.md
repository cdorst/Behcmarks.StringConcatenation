# String concatenation benchmarks
Run `./run.ps1` or `./run.sh` at the repository root to repeat the experiment

## Question

What is the most performant method of concatenating strings in .NET Core 2.1 applications?

## Variables

Five implementations of string concatenation are tested:

- `+` operator string addition
- `StringBuilder`
- `string.Concat`
- `$` string interpolation
- `string.Join`

Performance impact of using a comma-delimiter for each implementation is also observed.

## Hypothesis

Higher-level abstractions like string interpolation (`$` => `string.Format`), `StringBuilder`, and `string.Join` are expected to perform slower than lower-level functions like the `+` operator and `string.Concat` for simple scenarios like the one tested.

## Results

``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i7-7700HQ CPU 2.80GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.300-rc1-008673
  [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-NSBYSK : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT

LaunchCount=10  

```
|                            Method |      Mean |     Error |    StdDev |    Median | Rank |  Gen 0 | Allocated |
|---------------------------------- |----------:|----------:|----------:|----------:|-----:|-------:|----------:|
|      CommaDelimiterStringAddition |  41.33 ns | 0.2430 ns | 0.8710 ns |  41.24 ns |    2 | 0.0381 |     120 B |
|       CommaDelimiterStringBuilder |  62.51 ns | 0.3458 ns | 1.2962 ns |  62.16 ns |    4 | 0.0712 |     224 B |
|        CommaDelimiterStringConcat |  41.16 ns | 0.2576 ns | 0.9843 ns |  40.94 ns |    2 | 0.0381 |     120 B |
| CommaDelimiterStringInterpolation | 151.70 ns | 1.0135 ns | 4.2914 ns | 150.73 ns |    8 | 0.0379 |     120 B |
|          CommaDelimiterStringJoin |  67.47 ns | 0.5115 ns | 1.9607 ns |  67.45 ns |    5 | 0.0508 |     160 B |
|         NoDelimiterStringAddition |  34.00 ns | 0.2111 ns | 0.8041 ns |  33.88 ns |    1 | 0.0381 |     120 B |
|          NoDelimiterStringBuilder |  57.91 ns | 0.4356 ns | 1.7467 ns |  57.76 ns |    3 | 0.0712 |     224 B |
|           NoDelimiterStringConcat |  33.98 ns | 0.2113 ns | 0.7789 ns |  33.89 ns |    1 | 0.0381 |     120 B |
|    NoDelimiterStringInterpolation | 143.46 ns | 0.7099 ns | 2.5718 ns | 142.91 ns |    7 | 0.0379 |     120 B |
|             NoDelimiterStringJoin |  70.08 ns | 0.4100 ns | 1.5165 ns |  69.89 ns |    6 | 0.0508 |     160 B |

## Conclusion

`string.Concat` had the fastest average runtime for the scenario tested.

Results indicate that there is a slight performance penalty for using the more terse, convenient `$` string interpolation syntax.

