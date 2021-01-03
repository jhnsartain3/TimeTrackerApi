using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Controllers.Base;
using Api.Controllers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : BaseController, IUserSpecificDataAccessController<EventModel>
    {
        // GET: api/Event
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetAll(string userId)
        {
            LoggerWrapper.LogInformation("Get All Event Models", GetType().Name, nameof(GetAll), null);

            return Ok(await EventService.GetAllAsync(userId));
        }

        // GET: api/Event/ByProjectId/5
        [HttpGet("ByProjectId/{id}")]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetAllById(string userId, string id)
        {
            LoggerWrapper.LogInformation($"Get all event models with project id: {id}", this.GetType().Name, nameof(GetAllById), null);

            return Ok(await EventService.GetAllByIdAsync(userId, id));
        }

        // GET: api/Event/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventModel>> GetById(string userId, string id)
        {
            LoggerWrapper.LogInformation($"Get Event Model By Id: {id}", GetType().Name, nameof(GetById), null);

            return Ok(await EventService.GetByIdAsync(userId, id));
        }

        // PUT: api/Event/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string userId, string id, EventModel model)
        {
            LoggerWrapper.LogInformation($"Update Event Model: {id}", GetType().Name, nameof(Update), null);

            await EventService.UpdateAsync(userId, id, model);

            return NoContent();
        }

        // POST: api/Event
        [HttpPost]
        public async Task<ActionResult<EventModel>> Create(string userId, EventModel model)
        {
            LoggerWrapper.LogInformation($"Create Event Model: {model.Id}", GetType().Name, nameof(Create), null);

            await EventService.CreateAsync(userId, model);

            return Created(nameof(Create), new { id = model.Id });
        }

        // DELETE: api/Event/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EventModel>> Delete(string userId, string id)
        {
            LoggerWrapper.LogInformation($"Delete Event Model: {id}", GetType().Name, nameof(Delete), null);

            await EventService.DeleteAsync(userId, id);

            return NoContent();
        }

        // GET: api/Event/CanBeStopped/
        [HttpGet("CanBeStopped/{id}")]
        public async Task<ActionResult<EventModel>> CanBeStopped(string userId, string id)
        {
            LoggerWrapper.LogInformation($"CanBeStopped: {id}", GetType().Name, nameof(CanBeStopped), null);

            return Ok(new EventModel { Id = await EventService.CanBeStopped(userId, id) });
        }
    }
}