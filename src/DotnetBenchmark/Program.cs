using BenchmarkDotNet.Running;
using DotnetBenchmark.HttpRequestsBenchmark;

var resultado = BenchmarkRunner.Run<HttpRequests>();