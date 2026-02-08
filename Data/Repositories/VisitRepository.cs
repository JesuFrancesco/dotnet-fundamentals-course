using Data.Persistence;
using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class VisitRepository : IRepository<VisitEntity, Guid>, IVisitRepository<VisitEntity>
    {
        private readonly ApplicationDBContext _context;

        public VisitRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(VisitEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _context.Visits.AddAsync(entity);
        }

        public Task DeleteAsync(VisitEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<VisitEntity>> FindAllAsync()
        {
            return await _context.Visits.Include(v => v.Person).AsNoTracking().OrderByDescending(v => v.EntryTime).ToListAsync();
        }

        public async Task<VisitEntity?> FindByIdAsync(Guid id)
        {
            return await _context.Visits.Include(v => v.Person).FirstOrDefaultAsync(v => v.Id == id);
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(VisitEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Update(entity);
            return Task.CompletedTask;
        }

        // IVisitRepo
        public async Task<VisitEntity?> GetActiveVisitByPersonCodeAsync(string personCode)
        {
            return await _context.Visits
                .Include(v => v.Person)
                .FirstOrDefaultAsync(v => v.Person != null && v.Person.Code.ToUpper() == personCode.ToUpper() && v.ExitTime == null);
        }

        public async Task<IEnumerable<VisitEntity>> GetActiveVisitsAsync()
        {
            return await _context.Visits.Where(v => v.ExitTime == null).OrderBy(v => v.EntryTime).ToListAsync();
        }


        public async Task<bool> HasActiveVisitAsync(Guid personId)
        {
            return await _context.Visits.AnyAsync(v => v.PersonId == personId && v.ExitTime == null);
        }

        public async Task<IEnumerable<VisitEntity>> GetVisitsByPersonIdAsync(Guid personId)
        {
            return await _context.Visits.Include(v => v.Person).Where(e => e.PersonId == personId).OrderByDescending(v => v.EntryTime).ToListAsync();
        }
    }
}
