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
    public class QuizzesModelController : ControllerBase
    {
        private readonly TecnusDBContext _context;

        public QuizzesModelController(TecnusDBContext context)
        {
            _context = context;
        }

        // GET: api/QuizzesModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizModel>>> GetQuizzesModel()
        {
            return await _context.QuizzesModel.ToListAsync();
        }

        // GET: api/QuizzesModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizModel>> GetQuizModel(int id)
        {
            var quizModel = await _context.QuizzesModel.FindAsync(id);

            if (quizModel == null)
            {
                return NotFound();
            }

            return quizModel;
        }

        // PUT: api/QuizzesModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuizModel(int id, QuizModel quizModel)
        {
            if (id != quizModel.Id_Quiz)
            {
                return BadRequest();
            }

            _context.Entry(quizModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizModelExists(id))
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

        // POST: api/QuizzesModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<QuizModel>> PostQuizModel(QuizModel quizModel)
        {
            _context.QuizzesModel.Add(quizModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuizModel", new { id = quizModel.Id_Quiz }, quizModel);
        }

        // DELETE: api/QuizzesModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizModel(int id)
        {
            var quizModel = await _context.QuizzesModel.FindAsync(id);
            if (quizModel == null)
            {
                return NotFound();
            }

            _context.QuizzesModel.Remove(quizModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizModelExists(int id)
        {
            return _context.QuizzesModel.Any(e => e.Id_Quiz == id);
        }
    }
}
