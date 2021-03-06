﻿using DatabaseInteraction;
using DatabaseInteraction.Interfaces;
using DatabaseInteraction.Models;
using Microsoft.Extensions.Configuration;
using Models;

namespace Contexts
{
    public interface IEventContext : IUserSpecificDatabaseAccess<EventModel>
    {
    }

    public class EventContext : MongoUserSpecificDatabaseAccess<EventModel>, IEventContext
    {
        public EventContext(IConfiguration configuration) : base(configuration)
        {
            var connectionModel = new ConnectionModel
            {
                ConnectionString = configuration["ConnectionStrings:TimeTrackerDatabaseServer"],
                DatabaseName = configuration["DatabaseNames:TimeTracker"],
                CollectionName =
                    configuration[$"CollectionNames:{nameof(EventContext).Substring(0, nameof(EventContext).Length - ("Context".Length))}"]
            };

            SetupConnectionAsync(connectionModel);

            Items = MongoDatabase.GetCollection<EventModel>(connectionModel.CollectionName);
        }
    }
}