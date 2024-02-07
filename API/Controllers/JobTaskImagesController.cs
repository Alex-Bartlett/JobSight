using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobSightLib.Models;
using API.Helpers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTaskImagesController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public JobTaskImagesController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/JobTaskImages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobTaskImage>>> GetTaskImages()
        {
            return await _context.JobTaskImages.ToListAsync();
        }

        // GET: api/JobTaskImages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobTaskImage>> GetJobTaskImage(int id)
        {
            var jobTaskImage = await _context.JobTaskImages.FindAsync(id);

            if (jobTaskImage == null)
            {
                return NotFound();
            }

            return jobTaskImage;
        }

        // PUT: api/JobTaskImages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobTaskImage(int id, JobTaskImage jobTaskImage)
        {
            if (id != jobTaskImage.Id)
            {
                return BadRequest();
            }

            _context.Entry(jobTaskImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTaskImageExists(id))
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

        // POST: api/JobTaskImages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<JobTaskImage>> PostJobTaskImage(JobTaskImage jobTaskImage)
        {
            _context.JobTaskImages.Add(jobTaskImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobTaskImage", new { id = jobTaskImage.Id }, jobTaskImage);
        }

        // DELETE: api/JobTaskImages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobTaskImage(int id)
        {
            var jobTaskImage = await _context.JobTaskImages.FindAsync(id);
            if (jobTaskImage == null)
            {
                return NotFound();
            }

            _context.JobTaskImages.Remove(jobTaskImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JobTaskImageExists(int id)
        {
            return _context.JobTaskImages.Any(e => e.Id == id);
        }
    }
}
