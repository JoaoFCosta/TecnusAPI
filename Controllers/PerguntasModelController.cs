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
    public class PerguntasModelController : ControllerBase
    {
        private readonly TecnusDBContext _context;

        public PerguntasModelController(TecnusDBContext context)
        {
            _context = context;
        }

        // GET: api/PerguntasModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerguntaModel>>> GetPerguntasModel()
        {
            return await _context.PerguntasModel.ToListAsync();
        }

        // GET: api/PerguntasModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PerguntaModel>> GetPerguntaModel(int id)
        {
            var perguntaModel = await _context.PerguntasModel.FindAsync(id);

            if (perguntaModel == null)
            {
                return NotFound();
            }

            return perguntaModel;
        }

        // PUT: api/PerguntasModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerguntaModel(int id, PerguntaModel perguntaModel)
        {
            if (id != perguntaModel.Id_Pergunta)
            {
                return BadRequest();
            }

            _context.Entry(perguntaModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerguntaModelExists(id))
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

        // POST: api/PerguntasModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PerguntaModel>> PostPerguntaModel(PerguntaModel perguntaModel)
        {
            _context.PerguntasModel.Add(perguntaModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerguntaModel", new { id = perguntaModel.Id_Pergunta }, perguntaModel);
        }

        // DELETE: api/PerguntasModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerguntaModel(int id)
        {
            var perguntaModel = await _context.PerguntasModel.FindAsync(id);
            if (perguntaModel == null)
            {
                return NotFound();
            }

            _context.PerguntasModel.Remove(perguntaModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PerguntaModelExists(int id)
        {
            return _context.PerguntasModel.Any(e => e.Id_Pergunta == id);
        }
    }
}
