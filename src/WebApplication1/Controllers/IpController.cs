using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/ip")]
public sealed class IpController : ControllerBase
{
    private readonly IpClient ipClient;

    public IpController(IpClient ipClient)
    {
        this.ipClient = ipClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetId()
    {
        var ipResponse = await ipClient.GetIpAddressAsync();

        return Ok(ipResponse.Ip);
    }
}

public sealed class IpClient
{
    private readonly HttpClient client;

    public IpClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<IpResponse> GetIpAddressAsync()
    {
        var response = await client.GetAsync("https://api.ipify.org/?format=json");
        response.EnsureSuccessStatusCode();
        var rawResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IpResponse>(rawResponse) ?? new IpResponse();
    }
}

public sealed class IpResponse
{
    [JsonPropertyName("ip")]
    public string? Ip { get; init; }
}
