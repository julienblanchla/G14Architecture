using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

using MVC.Models;
using MVC.Services.Interfaces;

namespace MVC.Services
{
    public class loginservice : Iloginservice
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string UserIdSessionKey = "UserId";

        public loginservice(HttpClient client, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _baseUrl = configuration["WebAPI:BaseUrl"];
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Login(string username, string password)
        {
            var response = _client.PostAsJsonAsync($"{_baseUrl}/api/user/login", new { username, password }).Result;

            if (response.IsSuccessStatusCode)
            {
                var user = response.Content.ReadFromJsonAsync<UserProfile>().Result;
                if (user != null)
                {
                    _httpContextAccessor.HttpContext.Session.SetInt32(UserIdSessionKey, user.UserId);
                    return true;
                }
            }

            return false;
        }

        public async Task<UserProfile> GetUserProfile()
        {
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32(UserIdSessionKey);
            if (userId == null)
            {
                throw new Exception("User is not logged in.");
            }

            var response = await _client.GetAsync($"{_baseUrl}/api/user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserProfile>();
            }
            else
            {
                throw new HttpRequestException("Failed to load profile data.");
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/user");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var userResponse = JsonSerializer.Deserialize<UserResponse>(jsonString, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                });
                return userResponse.Values;
            }
            else
            {
                throw new HttpRequestException("Failed to load users data.");
            }
        }
        public async Task<UserProfile> GetUserProfileByIdAsync(int userId)
        {
            var response = await _client.GetAsync($"{_baseUrl}/api/user/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserProfile>();
            }
            else
            {
                throw new HttpRequestException("Failed to load profile data.");
            }
        }
    }
}
