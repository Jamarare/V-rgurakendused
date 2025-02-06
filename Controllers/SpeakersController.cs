using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpeakersController : ControllerBase
{
    private readonly DataContext _context;

    public SpeakersController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Speaker>> GetSpeakers(string? name = null)
    {
        var query = _context.Speakers!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

        return query.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<TextReader> GetSpeaker(int id)
    {
        var s = _context.Speakers!.Find(id);

        if (s == null)
        {
            return NotFound();
        }

        return Ok(s);
    }

    [HttpPut("{id}")]
    public IActionResult PutSpeaker(int id, Speaker s)
    {
        var dbs = _context.Speakers!.AsNoTracking().FirstOrDefault(x => x.Id == s.Id);
        if (id != s.Id || dbs == null)
        {
            return NotFound();
        }

        _context.Update(s);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Speaker> PostSpeaker(Speaker speaker)
    {
        if (!speaker.Email.Contains("@"))
        {
            return BadRequest("Email needs to contain an @");
        }
        var dbExercise = _context.Speakers!.Find(speaker.Id);
        if (dbExercise == null)
        {
            _context.Add(speaker);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetSpeaker), new { Id = speaker.Id }, speaker);
        }
        else
        {
            return Conflict();
        }
    }


    [HttpDelete("{id}")]
    public IActionResult DeleteSpeaker(int id)
    {
        var s = _context.Speakers!.Find(id);
        if (s == null)
        {
            return NotFound();
        }

        _context.Remove(s);
        _context.SaveChanges();

        return NoContent();
    }
}
