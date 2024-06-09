using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Database;
using Database.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrinterController : ControllerBase
    {
        private readonly G14DB _context;

        public PrinterController(G14DB context)
        {
            _context = context;
        }

        // GET: api/Printer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Printer>>> GetPrinters()
        {
            return await _context.Printers.Include(p => p.Papers).ToListAsync();
        }

        // GET: api/Printer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Printer>> GetPrinter(int id)
        {
            var printer = await _context.Printers.Include(p => p.Papers).FirstOrDefaultAsync(p => p.PrinterId == id);

            if (printer == null)
            {
                return NotFound();
            }

            return printer;
        }

        // DELETE: api/Printer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrinter(int id)
        {
            var printer = await _context.Printers.FindAsync(id);
            if (printer == null)
            {
                return NotFound();
            }

            _context.Printers.Remove(printer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Printer/5/Papers
        [HttpPost("{id}/Papers")]
        public async Task<ActionResult<Paper>> AddPaper(int id, [FromBody] PaperDto paperDto)
        {
            var printer = await _context.Printers.FindAsync(id);
            if (printer == null)
            {
                return NotFound();
            }

            var paper = new Paper
            {
                Type = paperDto.Type,
                Amount = paperDto.Amount,
                Value = paperDto.Value,
                PrinterId = id
            };

            _context.Papers.Add(paper);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrinter), new { id = printer.PrinterId }, paper);
        }

// POST: api/Printer/5/Buy
        [HttpPost("{id}/Buy")]
        public async Task<IActionResult> Buy(int id, [FromBody] BuyPaperDto buyPaperDto)
        {
            var printer = await _context.Printers.FindAsync(id);
            Console.WriteLine(printer);
            if (printer == null)
            {
                return NotFound();
            }

            var paper = await _context.Papers.FirstOrDefaultAsync(p => p.PrinterId == id);
            Console.WriteLine(paper);
            if (paper == null)
            {
                return BadRequest(new { Message = "No paper available for the printer" });
            }
            var totalCost = buyPaperDto.Amount * paper.Value;
            Console.WriteLine(totalCost);
            var user = await _context.User.Include(u => u.Card).FirstOrDefaultAsync(u => u.UserId == buyPaperDto.UserId);
            Console.WriteLine(user);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Card.Balance < totalCost)
            {
                return BadRequest(new { Message = "Insufficient balance" });
            }

            if (paper.Amount < buyPaperDto.Amount)
            {
                return BadRequest(new { Message = "Not enough paper available" });
            }

            // Deduct amount of paper and cost from printer and user respectively
            paper.Amount -= buyPaperDto.Amount;
            Console.WriteLine(paper.Amount);
            user.Card.Balance -= totalCost;
            Console.WriteLine(user.Card.Balance);

            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Successfully bought {buyPaperDto.Amount} paper for {totalCost}" });
        }
    }

    public class BuyPaperDto
    {
        public int PrinterId { get; set; }
        public int PaperId { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public decimal Value { get; set; }

    }

    public class PaperDto
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public decimal Value { get; set; }
    }
}
