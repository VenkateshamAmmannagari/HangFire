using Hangfire;
using HangFire.Demo.Models;
using HangFire.Demo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangFire.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PilotsController : ControllerBase
    {
        public static List<Pilot> pilots = new List<Pilot>();
        private readonly IserviceManagement _iserviceManagement;

        public PilotsController(IserviceManagement iserviceManagement)
        {
            _iserviceManagement = iserviceManagement;
        }
        [HttpPost]
        public IActionResult AddPilot(Pilot pilot)
        {
            if (ModelState.IsValid)
            {
                pilots.Add(pilot);
                _iserviceManagement.InsertRecords(pilot);
                BackgroundJob.Enqueue<IserviceManagement>(x => x.SendEmail());
                return CreatedAtAction("GetPilot", new { pilot.Id }, pilot);
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetPilot(int id)
        {
            var pilots = _iserviceManagement.GetAllRecords();

            var pilot = pilots.FirstOrDefault(x => x.Id == id);
            if (pilot == null)
                return NotFound();
            BackgroundJob.Enqueue<IserviceManagement>(x => x.SyncData());
            return Ok(pilot);
        }

        
    }
}
