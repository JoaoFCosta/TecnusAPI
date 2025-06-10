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
    public class VisualizacaoVideosModelController : ControllerBase
    {
        private readonly TecnusDBContext _context;

        public VisualizacaoVideosModelController(TecnusDBContext context)
        {
            _context = context;
        }

        // GET: api/VisualizacaoVideosModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisualizacaoVideoModel>>> GetVisualizacaoVideosModel()
        {
            return await _context.VisualizacaoVideosModel.ToListAsync();
        }

        // GET: api/VisualizacaoVideosModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VisualizacaoVideoModel>> GetVisualizacaoVideoModel(int id)
        {
            var visualizacaoVideoModel = await _context.VisualizacaoVideosModel.FindAsync(id);

            if (visualizacaoVideoModel == null)
            {
                return NotFound();
            }

            return visualizacaoVideoModel;
        }

        // PUT: api/VisualizacaoVideosModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisualizacaoVideoModel(int id, VisualizacaoVideoModel visualizacaoVideoModel)
        {
            if (id != visualizacaoVideoModel.Id_Visualizacao)
            {
                return BadRequest();
            }

            _context.Entry(visualizacaoVideoModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisualizacaoVideoModelExists(id))
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

        // POST: api/VisualizacaoVideosModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VisualizacaoVideoModel>> PostVisualizacaoVideoModel(VisualizacaoVideoModel visualizacaoVideoModel)
        {
            _context.VisualizacaoVideosModel.Add(visualizacaoVideoModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVisualizacaoVideoModel", new { id = visualizacaoVideoModel.Id_Visualizacao }, visualizacaoVideoModel);
        }

        // DELETE: api/VisualizacaoVideosModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisualizacaoVideoModel(int id)
        {
            var visualizacaoVideoModel = await _context.VisualizacaoVideosModel.FindAsync(id);
            if (visualizacaoVideoModel == null)
            {
                return NotFound();
            }

            _context.VisualizacaoVideosModel.Remove(visualizacaoVideoModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VisualizacaoVideoModelExists(int id)
        {
            return _context.VisualizacaoVideosModel.Any(e => e.Id_Visualizacao == id);
        }
    }
}
