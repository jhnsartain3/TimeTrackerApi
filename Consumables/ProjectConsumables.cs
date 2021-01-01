using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumables.Interfaces;
using Contexts;
using Models;

namespace Consumables
{
    public interface IProjectConsumable : ISpecificUserConsumable<ProjectModel>
    {
    }

    public class ProjectConsumable : IProjectConsumable
    {
        private readonly IProjectContext _projectContext;

        public ProjectConsumable(IProjectContext projectContext)
        {
            _projectContext = projectContext;
        }

        public async Task<IEnumerable<ProjectModel>> GetAllAsync(string userId)
        {
            return (await _projectContext.GetAllAsync(userId)).ToList();
        }

        public async Task<ProjectModel> GetByIdAsync(string userId, string id)
        {
            return await _projectContext.GetByIdAsync(userId, id);
        }

        public async Task UpdateAsync(string userId, string id, ProjectModel model)
        {
            await _projectContext.UpdateAsync(userId, id, model);
        }

        public async Task CreateAsync(string userId, ProjectModel model)
        {
            await _projectContext.CreateAsync(userId, model);
        }

        public async Task DeleteAsync(string userId, string id)
        {
            await _projectContext.DeleteAsync(userId, id);
        }
    }
}