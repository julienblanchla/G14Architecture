using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Database.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Services.Interfaces;
using Newtonsoft.Json;

namespace MVC.Services
{
    public class PrinterService : IPrinterService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PrinterService> _logger;

        public PrinterService(HttpClient client, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<PrinterService> logger)
        {
            _client = client;
            _baseUrl = configuration["WebAPI:BaseUrl"];
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<IEnumerable<Printer>> GetPrintersAsync()
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/api/printer");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Printer>>(jsonString);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching printers: {ex.Message}");
                throw;
            }
        }

        public async Task<Printer> GetPrinterAsync(int id)
        {
            try
            {
                var response = await _client.GetAsync($"{_baseUrl}/api/printer/{id}");
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Printer>(jsonString);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error fetching printer with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeletePrinterAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"{_baseUrl}/api/printer/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error deleting printer with ID {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<Paper> AddPaperAsync(int printerId, PaperDto paperDto)
        {
            try
            {
                var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/printer/{printerId}/papers", paperDto);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Paper>(jsonString);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error adding paper to printer with ID {printerId}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> BuyPaperAsync(int printerId, BuyPaperDto buyPaperDto)
        {
            try
            {
                var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/printer/{printerId}/buy", buyPaperDto);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error buying paper for printer with ID {printerId}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> BuyPaper(int printerId, int paperId, int userId, int amount, decimal value)
        {
            try {
                var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/printer/{printerId}/buy", new { printerId, paperId, userId, amount, value });
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error buying paper: {ex.Message}");
                throw;
            }
        }
    }
}
