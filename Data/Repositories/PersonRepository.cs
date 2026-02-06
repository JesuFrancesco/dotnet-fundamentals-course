using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Persistence;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class PersonRepository : IRepository<PersonEntity, Guid>, ICodeRepository<PersonEntity>
    {
        private readonly ApplicationDBContext _context;

        public PersonRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _context.Persons.AddAsync(entity);
        }

        public Task DeleteAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Persons.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<PersonEntity>> FindAllAsync()
        {
            return await _context.Persons.AsNoTracking().OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToListAsync();
        }

        public async Task<PersonEntity?> FindByIdAsync(Guid id)
        {
            return await _context.Persons.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Persons.Update(entity);
            return Task.CompletedTask;
        }
        public async Task<bool> ExistsWithCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code), "El código no puede estar vacío.");
            }

            var normalizedCode = code.ToUpperInvariant();
            return await _context.Persons.AnyAsync(p => p.Code == normalizedCode);
        }

        public async Task<PersonEntity?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code), "El código no puede estar vacío.");
            }

            var normalizedCode = code.ToUpperInvariant();
            return await _context.Persons.FirstOrDefaultAsync(p => p.Code == normalizedCode);
        }
    }
}
