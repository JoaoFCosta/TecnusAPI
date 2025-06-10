using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnusAPI.Data;
using TecnusAPI.Models;

namespace TecnusAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksModelController : ControllerBase
    {
        private readonly TecnusDBContext _context;

        public FeedbacksModelController(TecnusDBContext context)
        {
            _context = context;
        }

        // GET: api/FeedbacksModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackModel>>> GetFeedbacksModel()
        {
            return await _context.FeedbacksModel.ToListAsync();
        }

        // GET: api/FeedbacksModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackModel>> GetFeedbackModel(int id)
        {
            var feedbackModel = await _context.FeedbacksModel.FindAsync(id);

            if (feedbackModel == null)
            {
                return NotFound();
            }

            return feedbackModel;
        }

        // PUT: api/FeedbacksModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedbackModel(int id, FeedbackModel feedbackModel)
        {
            if (id != feedbackModel.Id_Feedback)
            {
                return BadRequest();
            }

            _context.Entry(feedbackModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackModelExists(id))
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

        // POST: api/FeedbacksModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FeedbackModel>> PostFeedbackModel(FeedbackModel feedbackModel)
        {
            _context.FeedbacksModel.Add(feedbackModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeedbackModel", new { id = feedbackModel.Id_Feedback }, feedbackModel);
        }

        // DELETE: api/FeedbacksModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedbackModel(int id)
        {
            var feedbackModel = await _context.FeedbacksModel.FindAsync(id);
            if (feedbackModel == null)
            {
                return NotFound();
            }

            _context.FeedbacksModel.Remove(feedbackModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeedbackModelExists(int id)
        {
            return _context.FeedbacksModel.Any(e => e.Id_Feedback == id);
        }
    }
}
