using Business.Services.NITValidation.Interfaces;
using System.Net.Http.Json;

namespace Business.Services.NITValidation
{
    public class NitValidationService : INitValidationService
    {
        private readonly HttpClient _httpClient;

        public NitValidationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://www.datos.gov.co/resource/");
        }

        public async Task<bool> ExistsAsync(string nitBase)
        {
            var result = await _httpClient
                .GetFromJsonAsync<List<NitGovResponse>>($"c82u-588k.json?nit={nitBase}");

            return result != null && result.Count > 0;
        }
    }

    public class NitGovResponse
    {
        public string? NIT { get; set; } 
        public string? Codigo_Camara { get; set; }
        public string? Matricula { get; set; }
    }
}
