using DatabaseInteraction;
using DatabaseInteraction.Interfaces;
using DatabaseInteraction.Models;
using Microsoft.Extensions.Configuration;
using Models;

namespace Contexts
{
    public interface IProjectContext : IUserSpecificDatabaseAccess<ProjectModel>
    {
    }

    public class ProjectContext : MongoUserSpecificDatabaseAccess<ProjectModel>, IProjectContext
    {
        public ProjectContext(IConfiguration configuration) : base(configuration)
        {
            var connectionModel = new ConnectionModel
            {
                ConnectionString = configuration["ConnectionStrings:TimeTrackerDatabaseServer"],
                DatabaseName = configuration["DatabaseNames:TimeTracker"],
                CollectionName =
                    configuration[$"CollectionNames:{nameof(ProjectContext).Substring(0, nameof(ProjectContext).Length - ("Context".Length))}"]
            };

            SetupConnectionAsync(connectionModel);

            Items = MongoDatabase.GetCollection<ProjectModel>(connectionModel.CollectionName);
        }
    }
}