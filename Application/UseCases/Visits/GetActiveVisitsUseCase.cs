using Domain;
using Domain.Abstractions;

namespace Application.UseCases.Visits
{
    public class GetActiveVisitsUseCase
    {
        private readonly IVisitRepository<VisitEntity> _visitRepository;

        public GetActiveVisitsUseCase(IVisitRepository<VisitEntity> visitRepository)
        {
            _visitRepository = visitRepository;
        }
        public async Task<IEnumerable<VisitEntity>> ExecuteAsync()
        {
            return await _visitRepository.GetActiveVisitsAsync();
        }
    }
}
