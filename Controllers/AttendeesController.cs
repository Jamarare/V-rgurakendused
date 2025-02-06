using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendeesController : ControllerBase
{
    private readonly DataContext _context;

    public AttendeesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Attendee>> GetAttendees(string? name = null)
    {
        var query = _context.Attendees!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

        return query.ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<Attendee> GetAttendee(int id)
    {
        var a = _context.Attendees!.Find(id);

        if (a == null)
        {
            return NotFound();
        }

        return Ok(a);
    }

    [HttpPut("{id}")]
    public IActionResult PutAttendee(int id, Attendee a)
    {
        var dba = _context.Attendees!.AsNoTracking().FirstOrDefault(x => x.Id == a.Id);
        if (id != a.Id || dba == null)
        {
            return NotFound();
        }

        _context.Update(a);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult<Attendee> PostAttendees(Attendee a)
    {
        var dbExercise = _context.Attendees!.Find(a.Id);
        if (dbExercise == null)
        {
            _context.Add(a);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAttendee), new { Id = a.Id }, a);
        }
        else
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAttendee(int id)
    {
        var a = _context.Attendees!.Find(id);
        if (a == null)
        {
            return NotFound();
        }

        _context.Remove(a);
        _context.SaveChanges();

        return NoContent();
    }
}
