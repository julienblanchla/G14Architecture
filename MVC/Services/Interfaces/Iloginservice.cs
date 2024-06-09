
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface Iloginservice
    {
        bool Login(string username, string password);
        Task<UserProfile> GetUserProfile();
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<UserProfile> GetUserProfileByIdAsync(int userId);
    }
}