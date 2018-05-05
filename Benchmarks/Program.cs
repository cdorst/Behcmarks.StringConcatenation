using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using System;
using System.IO;
using System.Text;
using static Benchmarks.Constants;

namespace Benchmarks
{
    [SimpleJob(10)]
    [RPlotExporter, RankColumn]
    public class Tests
    {
        [Benchmark]
        public string CommaDelimiterStringAddition() => new Strings().String1 + Comma + new Strings().String2;

        [Benchmark]
        public string CommaDelimiterStringBuilder() => new StringBuilder(new Strings().String1).Append(Comma).Append(new Strings().String2).ToString();

        [Benchmark]
        public string CommaDelimiterStringConcat() => string.Concat(new Strings().String1, Comma, new Strings().String2);

        [Benchmark]
        public string CommaDelimiterStringInterpolation() => $"{new Strings().String1},{new Strings().String2}";

        [Benchmark]
        public string CommaDelimiterStringJoin() => string.Join(Comma, new Strings().String1, new Strings().String2);

        [Benchmark]
        public string NoDelimiterStringAddition() => new Strings().String1 + new Strings().String2;

        [Benchmark]
        public string NoDelimiterStringBuilder() => new StringBuilder(new Strings().String1).Append(new Strings().String2).ToString();

        [Benchmark]
        public string NoDelimiterStringConcat() => string.Concat(new Strings().String1, new Strings().String2);

        [Benchmark]
        public string NoDelimiterStringInterpolation() => $"{new Strings().String1}{new Strings().String2}";

        [Benchmark]
        public string NoDelimiterStringJoin() => string.Join(string.Empty, new Strings().String1, new Strings().String2);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Tests>();
            var dataTable = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "BenchmarkDotNet.Artifacts", "results", "Tests-report-github.md"));
            var readme = new StringBuilder()
                .Append("# String concatenation benchmarks")
                .AppendLine()
                .AppendLine("Run `./run.ps1` or `./run.sh` at the repository root to repeat the experiment")
                .AppendLine()
                .AppendLine("## Question")
                .AppendLine()
                .AppendLine("What is the most performant method of concatenating strings in .NET Core 2.1 applications?")
                .AppendLine()
                .AppendLine("## Variables")
                .AppendLine()
                .AppendLine("Five implementations of string concatenation are tested:")
                .AppendLine()
                .AppendLine("- `+` operator string addition")
                .AppendLine("- `StringBuilder`")
                .AppendLine("- `string.Concat`")
                .AppendLine("- `$` string interpolation")
                .AppendLine("- `string.Join`")
                .AppendLine()
                .AppendLine("Performance impact of using a comma-delimiter for each implementation is also observed.")
                .AppendLine()
                .AppendLine("## Hypothesis")
                .AppendLine()
                .AppendLine("Higher-level abstractions like string interpolation (`$` => `string.Format`), `StringBuilder`, and `string.Join` are expected to perform slower than lower-level functions like the `+` operator and `string.Concat` for simple scenarios like the one tested.")
                .AppendLine()
                .AppendLine("## Results")
                .AppendLine();
            foreach (var line in dataTable) readme.AppendLine(line);
            readme.AppendLine();
            // summary
            readme
                .AppendLine("## Conclusion")
                .AppendLine()
                .AppendLine("`string.Concat` had the fastest average runtime for the scenario tested.")
                .AppendLine()
                .AppendLine("Results indicate that there is a slight performance penalty for using the more terse, convenient `$` string interpolation syntax.")
                .AppendLine();
            File.WriteAllText("../README.md", readme.ToString());
        }
    }
}
