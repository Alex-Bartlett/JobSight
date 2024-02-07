using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTiersController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public AccountTiersController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/AccountTiers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountTier>>> GetAccountTiers()
        {
            return await _context.AccountTiers.ToListAsync();
        }

        // GET: api/AccountTiers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountTier>> GetAccountTier(int id)
        {
            var accountTier = await _context.AccountTiers.FindAsync(id);

            if (accountTier == null)
            {
                return NotFound();
            }

            return accountTier;
        }
    }
}
