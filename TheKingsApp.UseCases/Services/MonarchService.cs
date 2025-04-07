using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TheKingsApp.Core.Entities;
using TheKingsApp.Core.Interfaces;
using TheKingsApp.UseCases.Dto;

namespace TheKingsApp.UseCases.Services
{
    public class MonarchService : IMonarchService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MonarchService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<List<Monarchs>> GetMonarchsAsync()
        {

            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, _configuration["EnglishMonarchsUrl"]);
            request.Headers.Add("accept", "application/json");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var monarchsData = JsonConvert.DeserializeObject<List<MonarchApiResponseDto>>(responseContent) ?? throw new InvalidOperationException();

            var monarchList = monarchsData.Select(m => new Monarchs
            {
                Id = m.Id,
                Name = m.Nm,
                Country = m.Cty,
                House = m.Hse,
                Years = m.Yrs,

            }).ToList();

            return monarchList;
        }
    }
}
