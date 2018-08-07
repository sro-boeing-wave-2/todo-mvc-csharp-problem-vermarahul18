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
        //[HttpGet]
        //public IEnumerable<Note> GetNote()
        //{
        //    return _context.Note.Include(x => x.labels).Include(x => x.checklist);
        //}

        // GET: api/Notes/5
        [HttpGet]
        public async Task<IActionResult> GetNoteByPrimitive(
              [FromQuery(Name = "Id")] int Id,
              [FromQuery(Name = "Title")] string Title,
              [FromQuery(Name = "text")] string text,
              [FromQuery(Name = "Pinned")] bool Pinned)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Note> temp = new List<Note>();
            temp = _context.Note.Include(x => x.checklist).Include(x => x.labels)
                .Where(element => element.Title == ((Title == null) ? element.Title : Title)
                      && element.text == ((text == null) ? element.text : text)
                      && element.Pinned == ((!Pinned) ? element.Pinned : Pinned)
                      && element.Id == ((Id == 0) ? element.Id : Id)).ToList();


            if (temp == null)
            {
                return NotFound();
            }
            return Ok(temp);
        }



        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Note.Include(x => x.labels).Include(x => x.checklist).ForEachAsync(x =>
            {
                if (x.Id == note.Id)
                {
                    x.Title = note.Title;
                    x.text = note.text;
                    foreach (Labels y in note.labels)
                    {
                        Labels a = x.labels.Find(z => z.Id == y.Id);
                        if (a != null)
                        {
                            a.label = y.label;
                        }
                        else
                        {
                            Labels lab= new Labels() { label= y.label };
                            x.labels.Add(lab);
                        }
                    }

                    foreach (CheckList obj in note.checklist)
                    {
                        CheckList c = x.checklist.Find(z => z.Id == obj.Id);
                        if (c != null)
                        {
                            c.Check = obj.Check;
                            c.isChecked = obj.isChecked;
                        }
                        else
                        {
                            CheckList a = new CheckList { Check = obj.Check, isChecked = obj.isChecked };
                            x.checklist.Add(a);
                        }
                    }

                }
            });
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

            //return CreatedAtAction(nameof(GetNoteByPrimitive), new
            //{
            //    note
            //});

            return Ok(note);
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

            return CreatedAtAction("GetNoteByPrimitive", new { id = note.Id }, note);
        }

        // DELETE: api/Notes/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteNote([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //   // var note = await _context.Note.FindAsync(id);
        //    var note = await _context.Note.Include(x => x.checklist).Include(x => x.labels).SingleOrDefaultAsync(x=>x.Id==id);
        //    if (note == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Note.Remove(note);
        //    await _context.SaveChangesAsync();

        //    return Ok(note);
        //}

        [HttpDelete]
        public async Task<IActionResult> DeleteNote([FromQuery(Name = "Id")] int Id,
              [FromQuery(Name = "Title")] string Title,
              [FromQuery(Name = "text")] string text,
              [FromQuery(Name = "Pinned")] bool Pinned)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Note> temp = new List<Note>();
            temp = _context.Note.Include(x => x.checklist).Include(x => x.labels)
                .Where(element => element.Title == ((Title == null) ? element.Title : Title)
                      && element.text == ((text == null) ? element.text : text)
                      && element.Pinned == ((!Pinned) ? element.Pinned : Pinned)
                      && element.Id == ((Id == 0) ? element.Id : Id)).ToList();

            if (temp == null)
            {
                return NotFound();
            }

            //_context.Note.Remove(note);
            //await _context.SaveChangesAsync();

            //return Ok(note);
            temp.ForEach(NoteDel=> _context.CheckList.RemoveRange(NoteDel.checklist));
            temp.ForEach(NoteDel => _context.Labels.RemoveRange(NoteDel.labels));
            _context.Note.RemoveRange(temp);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}