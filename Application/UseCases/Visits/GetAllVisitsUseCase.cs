using Domain;
using Domain.Abstractions;

namespace Application.UseCases.Visits
{
    public class GetAllVisitsUseCase
    {
        private readonly IRepository<VisitEntity, Guid> _repository;

        public GetAllVisitsUseCase(IRepository<VisitEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<VisitEntity>> ExecuteAsync()
        {
            return await _repository.FindAllAsync();
        }
    }
}
