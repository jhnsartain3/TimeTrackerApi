using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumables;
using Models;
using Sartain_Studios_Common.Logging;
using Services.Interfaces;

namespace Services
{
    public interface IProjectService : ISpecificUserDataAccess<ProjectModel>
    {
    }

    public class ProjectService : IProjectService
    {
        private readonly ILoggerWrapper _loggerWrapper;
        private readonly IProjectConsumable _projectConsumable;

        public ProjectService(ILoggerWrapper loggerWrapper, IProjectConsumable projectConsumable)
        {
            _projectConsumable = projectConsumable;
            _loggerWrapper = loggerWrapper;
        }

        public async Task<IEnumerable<ProjectModel>> GetAllAsync(string userId)
        {
            _loggerWrapper.LogInformation("Get All Projects Models", GetType().Name, nameof(GetAllAsync), null);

            return (await _projectConsumable.GetAllAsync(userId)).ToList();
        }

        public async Task<ProjectModel> GetByIdAsync(string userId, string id)
        {
            _loggerWrapper.LogInformation($"Get Project Model By Id: {id}", GetType().Name, nameof(GetByIdAsync), null);

            return await _projectConsumable.GetByIdAsync(userId, id);
        }

        public async Task UpdateAsync(string userId, string id, ProjectModel model)
        {
            _loggerWrapper.LogInformation($"Update Project Model: {id}", GetType().Name, nameof(UpdateAsync), null);

            await _projectConsumable.UpdateAsync(userId, id, model);
        }

        public async Task CreateAsync(string userId, ProjectModel model)
        {
            _loggerWrapper.LogInformation($"Create Project Model: {model.Name}", GetType().Name, nameof(CreateAsync),
                null);

            await _projectConsumable.CreateAsync(userId, model);
        }

        public async Task DeleteAsync(string userId, string id)
        {
            _loggerWrapper.LogInformation($"Delete Project Model: {id}", GetType().Name, nameof(DeleteAsync), null);

            await _projectConsumable.DeleteAsync(userId, id);
        }
    }
}