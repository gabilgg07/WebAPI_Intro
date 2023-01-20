using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentSystem.WebApi.Models.DataContexts;
using StudentSystem.WebApi.Models.Entities;

namespace StudentSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        readonly StudentDbContext db;

        public StudentsController(StudentDbContext db)
        {
            this.db = db;
        }

        // GET: api/Students
        [HttpGet]
        public IActionResult Get()
        {
            var students = db.Students.ToList();

            return Ok(students);
        }

        // GET api/Students/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!db.Students.Any(g => g.Id == id))
                return NotFound();

            var student = db.Students
                .FirstOrDefault(g => g.Id == id);

            return Ok(student);
        }

        // POST api/Students
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Student student)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Students.Add(student);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), routeValues: new
            {
                id = student.Id
            },
            student
            );
        }

        // PUT api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Student student)
        {
            if (id != student.Id)
                ModelState.AddModelError("Data", "Xetali muraciet gondermisiniz!!!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!db.Students.Any(g => g.Id == id))
                return NotFound();


            db.Students.Update(student);
            await db.SaveChangesAsync();

            return Ok(student);
        }

        // DELETE api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
                return BadRequest();

            var entity = db.Students.SingleOrDefault(g => g.Id == id);

            if (entity == null)
                return NotFound();

            db.Students.Remove(entity);
            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}

