using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Refit;
using RestSharp;
using System.Net.Http.Json;

namespace DotnetBenchmark.HttpRequestsBenchmark;

[MemoryDiagnoser]
[RankColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class HttpRequests
{
    private const string BaseUrl = "https://economia.awesomeapi.com.br/";
    private const string RequestUrl = "json/daily/EUR/30";

    [Benchmark]
    public async Task HttpRequestUsing_HttpClient()
    {
        using var client = new HttpClient() { BaseAddress = new Uri(BaseUrl) };

        var response = await client.GetAsync(RequestUrl);

        var content = await response.Content.ReadFromJsonAsync<IEnumerable<EconomiaApiResponse>>();
    }

    [Benchmark]
    public async Task HttpRequestUsing_Restsharp()
    {
        using var client = new RestClient(BaseUrl);

        var response = await client.GetJsonAsync<IEnumerable<EconomiaApiResponse>>(RequestUrl);
    }

    [Benchmark]
    public async Task HttpRequestUsing_Refit()
    {
        var client = RestService.For<IEconomiaApiClient>(BaseUrl);

        var response = await client.GetEuroValue();
    }
}

public record EconomiaApiResponse (string code, string codin, string name, string high, string low, string varBid, string pctChange, string bid, string ask, string timestamp, string create_date);

public interface IEconomiaApiClient
{
    [Get("/json/daily/EUR/2")]
    public Task<IEnumerable<EconomiaApiResponse>> GetEuroValue();
}
