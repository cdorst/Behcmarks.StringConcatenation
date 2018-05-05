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
.NET Core SDK=2.1.300-preview2-008533
  [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  Job-BDNANP : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT

LaunchCount=10  

```
|                            Method |      Mean |     Error |    StdDev |    Median | Rank |
|---------------------------------- |----------:|----------:|----------:|----------:|-----:|
|      CommaDelimiterStringAddition |  40.06 ns | 0.2581 ns | 0.9385 ns |  39.84 ns |    4 |
|       CommaDelimiterStringBuilder |  60.78 ns | 0.3063 ns | 1.0939 ns |  60.59 ns |    6 |
|        CommaDelimiterStringConcat |  39.58 ns | 0.1324 ns | 0.4780 ns |  39.54 ns |    3 |
| CommaDelimiterStringInterpolation | 143.17 ns | 0.5708 ns | 2.0532 ns | 142.92 ns |   10 |
|          CommaDelimiterStringJoin |  65.33 ns | 0.2203 ns | 0.7723 ns |  65.23 ns |    7 |
|         NoDelimiterStringAddition |  32.30 ns | 0.1614 ns | 0.5868 ns |  32.21 ns |    2 |
|          NoDelimiterStringBuilder |  56.54 ns | 0.2414 ns | 0.8808 ns |  56.47 ns |    5 |
|           NoDelimiterStringConcat |  32.13 ns | 0.0946 ns | 0.3429 ns |  32.08 ns |    1 |
|    NoDelimiterStringInterpolation | 138.37 ns | 0.5443 ns | 1.9650 ns | 137.94 ns |    9 |
|             NoDelimiterStringJoin |  69.30 ns | 0.2323 ns | 0.8326 ns |  69.20 ns |    8 |

## Conclusion

`string.Concat` had the fastest average runtime for the scenario tested.

Results indicate that there is a slight performance penalty for using the more terse, convenient `$` string interpolation syntax.

