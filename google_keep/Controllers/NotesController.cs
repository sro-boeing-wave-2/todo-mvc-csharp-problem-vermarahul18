using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using google_keep.Models;

namespace google_keep.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteContext _context;

        public NotesController(NoteContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Note> GetNote()
        {
           return _context.Note.Include(x => x.labels).Include(x => x.checklist);
        }

        // GET: api/Notes/5
       // [HttpGet("{id}")]
        public async Task<IActionResult> GetNote([FromQuery]int id,[FromQuery] string title,[FromQuery] bool pinned)
        {
            
            if (!ModelState.IsValid)
            {
 
                return BadRequest(ModelState);
            }
            var ret = _context.Note.Include(x => x.labels).Include(x => x.checklist);
            List<Note> lmn = new List<Note>();

            if (id != 0)
            {
              return Ok(ret.Where(x => x.Id == id));
            }
            else if (title != null)
            {
                lmn.AddRange(_context.Note.Where(x => x.Title == title).ToList());
            }
            else 
            {
                lmn.AddRange(lmn.Where(x => x.Pinned == pinned).ToList());
            }
            return Ok(lmn);
          //  var note = await _context.Note.FindAsync(id);

            //if (note == null)
            //{
            //    return NotFound();
            //}

            //return Ok(note);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != note.Id)
            {
                return BadRequest();
            }

            _context.Entry(note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
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

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.Id }, note);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Note.Remove(note);
            await _context.SaveChangesAsync();

            return Ok(note);
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}