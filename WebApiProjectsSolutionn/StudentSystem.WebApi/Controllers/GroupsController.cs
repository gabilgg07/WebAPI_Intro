using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentSystem.WebApi.Models.DataContexts;
using StudentSystem.WebApi.Models.Entities;

namespace StudentSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class GroupsController : Controller
    {
        readonly StudentDbContext db;
        public GroupsController(StudentDbContext db)
        {
            this.db = db;
        }

        // GET: api/Groups
        [HttpGet]
        public IActionResult Get()
        {
            var groups = db.Groups.ToList();

            return Ok(groups);
        }

        // GET api/Groups/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var group = db.Groups
                .FirstOrDefault(g => g.Id == id);

            if (group == null)
                return NotFound();

            return Ok(group);
        }

        // POST api/Groups
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Group group)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            db.Groups.Add(group);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), routeValues: new
            {
                id = group.Id
            },
            group
            );
        }

        // PUT api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody]  Group group)
        {
            if (id != group.Id)
                ModelState.AddModelError("Data", "Xetali muraciet gondermisiniz!!!");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!db.Groups.Any(g => g.Id == id))
                return NotFound();


            db.Groups.Update(group);
            await db.SaveChangesAsync();

            return Ok(group);
        }

        // DELETE api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
                return BadRequest();

            var entity = db.Groups.SingleOrDefault(g => g.Id == id);

            if (entity == null)
                return NotFound();

            db.Groups.Remove(entity);
            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}

