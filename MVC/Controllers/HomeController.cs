
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Services.Interfaces;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Iloginservice _loginService;
        private readonly ICardService _cardService;
        private readonly IPrinterService _printerService;

        public HomeController(ILogger<HomeController> logger, Iloginservice loginService, ICardService cardService, IPrinterService printerService)
        {
            _logger = logger;
            _loginService = loginService;
            _cardService = cardService;
            _printerService = printerService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Index()
        {
            return Login();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (_loginService.Login(username, password))
            {
                return RedirectToAction("Profile");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }
        }
        public async Task<IActionResult> Profile()
        {
            try
            {
                var userProfile = await _loginService.GetUserProfile();
                return View(userProfile);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddBalance(int cardId)
        {
            var result = await _cardService.GetCard(cardId);
            if (result is OkObjectResult okResult)
            {
                var model = okResult.Value as AddBalanceViewModel;
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddBalance(AddBalanceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _cardService.AddBalance(model);
                if (result is RedirectToActionResult)
                {
                    return result;
                }

                ModelState.AddModelError("", "Failed to add balance.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var userProfile = await _loginService.GetUserProfile();
            ViewBag.UserType = userProfile.UserType; // Pass UserType to the view
            try
            {
                var users = await _loginService.GetAllUsersAsync();
                if (users == null)
                {
                    users = new List<User>();
                }
                return View(users);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(new List<User>());
            }
        }
        public async Task<IActionResult> CheckProfile(int userId)
        {
            try
            {
                var userProfile = await _loginService.GetUserProfileByIdAsync(userId);
                return View(userProfile);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(new UserProfile());
            }
        }
        [HttpGet]
        public async Task<IActionResult> Printers()
        {
            var userProfile = await _loginService.GetUserProfile();
            ViewBag.UserType = userProfile.UserType; // Pass UserType to the view
            var printers = await _printerService.GetPrintersAsync();
            return View(printers);
        }

        [HttpGet]
        public async Task<IActionResult> PrinterDetails(int id)
        {
            var userProfile = await _loginService.GetUserProfile();
            ViewBag.UserType = userProfile.UserType; // Pass UserType to the view
            var printer = await _printerService.GetPrinterAsync(id);
            if (printer == null)
            {
                return NotFound();
            }
            return View(printer);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePrinter(int id)
        {
            var success = await _printerService.DeletePrinterAsync(id);
            if (success)
            {
                return RedirectToAction("Printers");
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult AddPaper(int printerId)
        {
            ViewBag.PrinterId = printerId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPaper(int printerId, PaperDto paperDto)
        {
            if (ModelState.IsValid)
            {
                var paper = await _printerService.AddPaperAsync(printerId, paperDto);
                return RedirectToAction("PrinterDetails", new { id = printerId });
            }
            ViewBag.PrinterId = printerId;
            return View(paperDto);
        }

        [HttpPost]
        public async Task<IActionResult> UsePrinter(int printerId)
        {
            var userProfile = await _loginService.GetUserProfile();
            ViewBag.UserType = userProfile.UserType; // Pass UserType to the view
            var printer = await _printerService.GetPrinterAsync(printerId);
            if (printer == null)
            {
                return NotFound();
            }
            return View(printer);
        }
        [HttpPost]
        public async Task<IActionResult> BuyPaper([FromBody] BuyPaperDto buyPaperDto)
        {
            try
            {
                var userProfile = await _loginService.GetUserProfile();
                if (userProfile.Card.Balance < buyPaperDto.Value)
                {
                    return Json(new { success = false, message = "Insufficient balance" });
                }
                var result = await _printerService.BuyPaper(buyPaperDto.PrinterId, buyPaperDto.PaperId, userProfile.UserId, buyPaperDto.Amount, buyPaperDto.Value);
                if (result)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to complete the purchase" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
