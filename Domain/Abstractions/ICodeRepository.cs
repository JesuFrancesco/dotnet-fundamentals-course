using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface ICodeRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByCodeAsync(string code);
        Task<bool> ExistsWithCodeAsync(string code);
    }
}
