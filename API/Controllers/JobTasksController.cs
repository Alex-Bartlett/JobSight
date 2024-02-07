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
    public class JobTasksController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public JobTasksController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/JobTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobTask>>> GetTasks()
        {
            return await _context.JobTasks.ToListAsync();
        }

        // GET: api/JobTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobTask>> GetJobTask(int id)
        {
            var jobTask = await _context.JobTasks.FindAsync(id);

            if (jobTask == null)
            {
                return NotFound();
            }

            return jobTask;
        }

        // PUT: api/JobTasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobTask(int id, JobTask jobTask)
        {
            if (id != jobTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(jobTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTaskExists(id))
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

        // POST: api/JobTasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobTask>> PostJobTask(JobTask jobTask)
        {
            _context.JobTasks.Add(jobTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobTask", new { id = jobTask.Id }, jobTask);
        }

        // DELETE: api/JobTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobTask(int id)
        {
            var jobTask = await _context.JobTasks.FindAsync(id);
            if (jobTask == null)
            {
                return NotFound();
            }

            _context.JobTasks.Remove(jobTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobTaskExists(int id)
        {
            return _context.JobTasks.Any(e => e.Id == id);
        }
    }
}
