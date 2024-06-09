using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private readonly G14DB _context;

        public CardController(G14DB context)
        {
            _context = context;
        }

        // GET: api/card
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
            return await _context.Cards.ToListAsync();
        }

        // GET: api/card/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            var card = await _context.Cards.FindAsync(id);

            if (card == null)
            {
                return NotFound();
            }

            return card;
        }

        // POST: api/card/addbalance
        [HttpPost("addbalance")]
        public async Task<IActionResult> AddBalance([FromBody] AddBalanceViewModel model)
        {
            var card = await _context.Cards.FindAsync(model.CardId);
            if (card == null)
            {
                return NotFound();
            }

            card.Balance += model.Amount;
            await _context.SaveChangesAsync();

            return Ok(card);
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.CardId == id);
        }
    }

    public class AddBalanceViewModel
    {
        public int CardId { get; set; }
        public decimal Amount { get; set; }
    }
}
