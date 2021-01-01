using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumables.Interfaces;
using Contexts;
using Models;

namespace Consumables
{
    public interface IEventConsumable : ISpecificUserConsumable<EventModel>
    {
    }

    public class EventConsumable : IEventConsumable
    {
        private readonly IEventContext _eventContext;

        public EventConsumable(IEventContext eventContext)
        {
            _eventContext = eventContext;
        }

        public async Task<IEnumerable<EventModel>> GetAllAsync(string userId)
        {
            return (await _eventContext.GetAllAsync(userId)).ToList();
        }

        public async Task<EventModel> GetByIdAsync(string userId, string id)
        {
            return await _eventContext.GetByIdAsync(userId, id);
        }

        public async Task UpdateAsync(string userId, string id, EventModel model)
        {
            await _eventContext.UpdateAsync(userId, id, model);
        }

        public async Task CreateAsync(string userId, EventModel model)
        {
            await _eventContext.CreateAsync(userId, model);
        }

        public async Task DeleteAsync(string userId, string id)
        {
            await _eventContext.DeleteAsync(userId, id);
        }
    }
}