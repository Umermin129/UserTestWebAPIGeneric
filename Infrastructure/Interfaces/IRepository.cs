using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface  IRepository<T> where T : class
    {
        Task<T> Create(T item);
        Task<T> GetAsync(Guid id);
        Task<List<T>> GetAllAsync();
    }
}
