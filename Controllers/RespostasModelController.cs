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
    public class RespostasModelController : ControllerBase
    {
        private readonly TecnusDBContext _context;

        public RespostasModelController(TecnusDBContext context)
        {
            _context = context;
        }

        // GET: api/RespostasModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RespostaModel>>> GetRespostasModel()
        {
            return await _context.RespostasModel.ToListAsync();
        }

        // GET: api/RespostasModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RespostaModel>> GetRespostaModel(int id)
        {
            var respostaModel = await _context.RespostasModel.FindAsync(id);

            if (respostaModel == null)
            {
                return NotFound();
            }

            return respostaModel;
        }

        // PUT: api/RespostasModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRespostaModel(int id, RespostaModel respostaModel)
        {
            if (id != respostaModel.Id_Resposta)
            {
                return BadRequest();
            }

            _context.Entry(respostaModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RespostaModelExists(id))
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

        // POST: api/RespostasModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RespostaModel>> PostRespostaModel(RespostaModel respostaModel)
        {
            _context.RespostasModel.Add(respostaModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRespostaModel", new { id = respostaModel.Id_Resposta }, respostaModel);
        }

        // DELETE: api/RespostasModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRespostaModel(int id)
        {
            var respostaModel = await _context.RespostasModel.FindAsync(id);
            if (respostaModel == null)
            {
                return NotFound();
            }

            _context.RespostasModel.Remove(respostaModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RespostaModelExists(int id)
        {
            return _context.RespostasModel.Any(e => e.Id_Resposta == id);
        }
    }
}
