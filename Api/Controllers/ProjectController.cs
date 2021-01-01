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
    public class ProjectController : BaseController, IUserSpecificDataAccessController<ProjectModel>
    {
        // GET: api/Project
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> GetAll(string userId)
        {
            LoggerWrapper.LogInformation("Get All Projects Models", GetType().Name, nameof(GetAll), null);

            return Ok(await ProjectService.GetAllAsync(userId));
        }

        // GET: api/Project/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectModel>> GetById(string userId, string id)
        {
            LoggerWrapper.LogInformation($"Get Project Model By Id: {id}", GetType().Name, nameof(GetById), null);

            return Ok(await ProjectService.GetByIdAsync(userId, id));
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string userId, string id, ProjectModel model)
        {
            LoggerWrapper.LogInformation($"Update Project Model: {id}", GetType().Name, nameof(Update), null);

            await ProjectService.UpdateAsync(userId, id, model);

            return NoContent();
        }

        // POST: api/Project
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ProjectModel>> Create(string userId, ProjectModel model)
        {
            LoggerWrapper.LogInformation($"Create Project Model: {model.Name}", GetType().Name, nameof(Create), null);

            await ProjectService.CreateAsync(userId, model);

            return Created(nameof(Create), new {id = model.Id});
        }

        // DELETE: api/Project/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProjectModel>> Delete(string userId, string id)
        {
            LoggerWrapper.LogInformation($"Delete Project Model: {id}", GetType().Name, nameof(Delete), null);

            await ProjectService.DeleteAsync(userId, id);

            return NoContent();
        }
    }
}