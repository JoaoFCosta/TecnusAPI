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
    public class CursosModelController : ControllerBase
    {
        private readonly TecnusDBContext _context;

        public CursosModelController(TecnusDBContext context)
        {
            _context = context;
        }

        // GET: api/CursosModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoModel>>> GetCursosModel()
        {
            return await _context.CursosModel.ToListAsync();
        }

        // GET: api/CursosModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CursoModel>> GetCursoModel(int id)
        {
            var cursoModel = await _context.CursosModel.FindAsync(id);

            if (cursoModel == null)
            {
                return NotFound();
            }

            return cursoModel;
        }

        // PUT: api/CursosModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCursoModel(int id, CursoModel cursoModel)
        {
            if (id != cursoModel.Id_Curso)
            {
                return BadRequest();
            }

            _context.Entry(cursoModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoModelExists(id))
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

        // POST: api/CursosModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CursoModel>> PostCursoModel(CursoModel cursoModel)
        {
            _context.CursosModel.Add(cursoModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCursoModel", new { id = cursoModel.Id_Curso }, cursoModel);
        }

        // DELETE: api/CursosModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCursoModel(int id)
        {
            var cursoModel = await _context.CursosModel.FindAsync(id);
            if (cursoModel == null)
            {
                return NotFound();
            }

            _context.CursosModel.Remove(cursoModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursoModelExists(int id)
        {
            return _context.CursosModel.Any(e => e.Id_Curso == id);
        }
    }
}
