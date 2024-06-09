using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class CardService : ICardService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;

        public CardService(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _baseUrl = configuration["WebAPI:BaseUrl"];
        }

        public async Task<IActionResult> GetCard(int id)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/card/{id}");
            if (response.IsSuccessStatusCode)
            {
                var card = await response.Content.ReadFromJsonAsync<Card>();
                if (card == null)
                {
                    return new NotFoundResult();
                }

                var model = new AddBalanceViewModel
                {
                    CardId = card.CardId,
                    CardNumber = card.CardNumber
                };
                return new OkObjectResult(model);
            }
            return new NotFoundResult();
        }

        public async Task<IActionResult> AddBalance(AddBalanceViewModel model)
        {
            var response = await _client.PostAsJsonAsync($"{_baseUrl}/api/card/addbalance", model);
            if (response.IsSuccessStatusCode)
            {
                return new RedirectToActionResult("Profile", "Home", null);
            }

            // Handle the error appropriately
            return new BadRequestObjectResult("Failed to add balance.");
        }
    }
}
