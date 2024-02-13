using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using API.Helpers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobNotesController : ControllerBase
    {
        private readonly JobSightDbContext _context;

        public JobNotesController(JobSightDbContext context)
        {
            _context = context;
        }

        // GET: api/JobNotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobNote>>> GetJobNotes()
        {
            return await _context.JobNotes.ToListAsync();
        }

        // GET: api/JobNotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobNote>> GetJobNote(int id)
        {
            var jobNote = await _context.JobNotes.FindAsync(id);

            if (jobNote == null)
            {
                return NotFound();
            }

            return jobNote;
        }

        // PUT: api/JobNotes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobNote(int id, JobNote jobNote)
        {
            if (id != jobNote.Id)
            {
                return BadRequest();
            }

            _context.Entry(jobNote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobNoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/JobNotes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobNote>> PostJobNote(JobNote jobNote)
        {
            _context.JobNotes.Add(jobNote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobNote", new { id = jobNote.Id }, jobNote);
        }

        // DELETE: api/JobNotes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobNote(int id)
        {
            var jobNote = await _context.JobNotes.FindAsync(id);
            if (jobNote == null)
            {
                return NotFound();
            }

            _context.JobNotes.Remove(jobNote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobNoteExists(int id)
        {
            return _context.JobNotes.Any(e => e.Id == id);
        }
    }
}
