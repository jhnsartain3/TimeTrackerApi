﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumables;
using Models;
using Sartain_Studios_Common.Logging;
using Services.Interfaces;

namespace Services
{
    public interface IEventService : ISpecificUserDataAccess<EventModel>
    {
        Task<IEnumerable<EventModel>> GetAllByIdAsync(string userId, string id);
        Task<string> CanBeStopped(string userId, string id);
    }

    public class EventService : IEventService
    {
        private readonly IEventConsumable _eventConsumable;
        private readonly ILoggerWrapper _loggerWrapper;

        public EventService(ILoggerWrapper loggerWrapper, IEventConsumable eventConsumable)
        {
            _eventConsumable = eventConsumable;
            _loggerWrapper = loggerWrapper;
        }

        public async Task<IEnumerable<EventModel>> GetAllByIdAsync(string userId, string id)
        {
            _loggerWrapper.LogInformation($"Get All Event Model By Id: {id}", GetType().Name, nameof(GetByIdAsync), null);

            var allModels = await GetAllAsync(userId);

            return allModels
                .Where(x => x.ProjectId.Equals(id))
                .Select(x => x)
                .OrderByDescending(x => x.StartDateTime);
        }

        public async Task<string> CanBeStopped(string userId, string id)
        {
            _loggerWrapper.LogInformation($"CanBeStopped: {id}", GetType().Name, nameof(GetByIdAsync), null);

            var allModels = await GetAllAsync(userId);

            var eventModelWithNullEndDateTime = allModels
                .Where(x => x.ProjectId.Equals(id) && x.EndDateTime == null)
                .Select(x => x)
                .FirstOrDefault();

            return eventModelWithNullEndDateTime?.Id;
        }

        public async Task<IEnumerable<EventModel>> GetAllAsync(string userId)
        {
            _loggerWrapper.LogInformation("Get All Events Models", GetType().Name, nameof(GetAllAsync), null);

            return (await _eventConsumable.GetAllAsync(userId)).ToList();
        }

        public async Task<EventModel> GetByIdAsync(string userId, string id)
        {
            _loggerWrapper.LogInformation($"Get Event Model By Id: {id}", GetType().Name, nameof(GetByIdAsync), null);

            return await _eventConsumable.GetByIdAsync(userId, id);
        }

        public async Task UpdateAsync(string userId, string id, EventModel model)
        {
            _loggerWrapper.LogInformation($"Update Event Model: {id}", GetType().Name, nameof(UpdateAsync), null);

            await _eventConsumable.UpdateAsync(userId, id, model);
        }

        public async Task CreateAsync(string userId, EventModel model)
        {
            _loggerWrapper.LogInformation($"Create Event Model: {model.ProjectId}", GetType().Name, nameof(CreateAsync),
                null);

            await _eventConsumable.CreateAsync(userId, model);
        }

        public async Task DeleteAsync(string userId, string id)
        {
            _loggerWrapper.LogInformation($"Delete Event Model: {id}", GetType().Name, nameof(DeleteAsync), null);

            await _eventConsumable.DeleteAsync(userId, id);
        }
    }
}