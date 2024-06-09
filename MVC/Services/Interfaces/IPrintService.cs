using Database.Models;
using MVC.Models;

namespace MVC.Services.Interfaces
{
    public interface IPrinterService
    {
        Task<IEnumerable<Printer>> GetPrintersAsync();
        Task<Printer> GetPrinterAsync(int id);
        Task<bool> DeletePrinterAsync(int id);
        Task<Paper> AddPaperAsync(int printerId, PaperDto paperDto);
        Task<bool> BuyPaperAsync(int printerId, BuyPaperDto buyPaperDto);
        Task<bool> BuyPaper(int printerId,int paperId, int userId, int amount, decimal value);
    }
}
