using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Threading.Tasks;

namespace MVC.Services.Interfaces
{
    public interface ICardService
    {
        Task<IActionResult> GetCard(int id);
        Task<IActionResult> AddBalance(AddBalanceViewModel model);
    }
}
