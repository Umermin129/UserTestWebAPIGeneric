using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IService<TEntity,TResponse,TCreate> where TEntity : class 
    {
        Task<TResponse> CreateAsync(TCreate entity);
        Task<TResponse> GetAsync(Guid id);
        Task<List<TResponse>> GetAllAsync();
    }
}
