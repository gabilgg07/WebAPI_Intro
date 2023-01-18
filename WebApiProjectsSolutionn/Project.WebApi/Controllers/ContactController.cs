using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        static List<Contact> contacts = new List<Contact>();
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(contacts);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var found = contacts.FirstOrDefault(c => c.Id == id);

            if (found == null)
                return NotFound();

            return Ok(found);
        }

        // POST api/values
        [HttpPost]
        //public void Post([FromBody]string value)
        public IActionResult Post(Contact c)
        {
            contacts.Add(c);

            return Ok(c);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        public IActionResult Put(int id, Contact m)
        {
            var found = contacts.FirstOrDefault(c => c.Id == id);

            if (found == null)
                return NotFound();

            found.Name = m.Name;
            found.Phone = m.Phone;

            return Ok(found);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var found = contacts.FirstOrDefault(c => c.Id == id);

            if (found == null)
                return NotFound();

            contacts.Remove(found);

            return NoContent();

        }
    }
}

