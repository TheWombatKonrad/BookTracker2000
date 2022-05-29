using BookTrackersApi.Authorization;
using BookTrackersApi.DatabaseContext;
using BookTrackersApi.Models.Readings;
using BookTrackersApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookTrackersApi.Controllers
{
    [Authorize]
    [Route("api/readings")]
    [ApiController]
    public class ReadingsController : ControllerBase
    {
        private IReadingService _readingService;

        public ReadingsController(IReadingService readingService)
        {
            _readingService = readingService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var readings = _readingService.GetAll();
            return Ok(readings);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetReading(int id)
        {
            var reading = _readingService.GetById(id);
            return Ok(reading);
        }

        [AllowAnonymous]
        [HttpGet("current")]
        public IActionResult GetReadingsByCurrentUser()
        {
            var reading = _readingService.GetByCurrentUser();
            return Ok(reading);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateReadingRequest model)
        {
            _readingService.Update(id, model);
            return Ok(new { message = "Reading updated successfully" });

        }

        [HttpPost]
        public IActionResult Register(RegisterReadingRequest model)
        {
            _readingService.Register(model);
            return Ok(new { message = "Reading added successfully" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _readingService.Delete(id);
            return Ok(new { message = "Reading deleted successfully" });
        }
    }
}
