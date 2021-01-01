using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Interfaces
{
    public interface IUserSpecificDataAccessController<TEntity>
    {
        Task<ActionResult<IEnumerable<TEntity>>> GetAll(string userId);
        Task<ActionResult<TEntity>> GetById(string userId, string id);
        Task<IActionResult> Update(string userId, string id, TEntity model);
        Task<ActionResult<TEntity>> Create(string userId, TEntity model);
        Task<ActionResult<TEntity>> Delete(string userId, string id);
    }
}