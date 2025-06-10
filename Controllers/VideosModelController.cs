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
    public class VideosModelController : ControllerBase
    {
        private readonly TecnusDBContext _context;

        public VideosModelController(TecnusDBContext context)
        {
            _context = context;
        }

        // GET: api/VideosModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoModel>>> GetVideoModel()
        {
            return await _context.VideoModel.ToListAsync();
        }

        // GET: api/VideosModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoModel>> GetVideoModel(int id)
        {
            var videoModel = await _context.VideoModel.FindAsync(id);

            if (videoModel == null)
            {
                return NotFound();
            }

            return videoModel;
        }

        // PUT: api/VideosModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideoModel(int id, VideoModel videoModel)
        {
            if (id != videoModel.Id_Video)
            {
                return BadRequest();
            }

            _context.Entry(videoModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoModelExists(id))
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

        // POST: api/VideosModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VideoModel>> PostVideoModel(VideoModel videoModel)
        {
            _context.VideoModel.Add(videoModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideoModel", new { id = videoModel.Id_Video }, videoModel);
        }

        // DELETE: api/VideosModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoModel(int id)
        {
            var videoModel = await _context.VideoModel.FindAsync(id);
            if (videoModel == null)
            {
                return NotFound();
            }

            _context.VideoModel.Remove(videoModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VideoModelExists(int id)
        {
            return _context.VideoModel.Any(e => e.Id_Video == id);
        }
    }
}
