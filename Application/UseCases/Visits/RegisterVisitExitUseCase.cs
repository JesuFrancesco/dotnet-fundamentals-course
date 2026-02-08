using Application.DTOs.Visits;
using Domain;
using Domain.Abstractions;

namespace Application.UseCases.Visits
{
    public class RegisterVisitExitUseCase
    {
        private readonly IRepository<VisitEntity, Guid> _repository;
        private readonly IVisitRepository<VisitEntity> _visitRepository;

        public RegisterVisitExitUseCase(IRepository<VisitEntity, Guid> repository, IVisitRepository<VisitEntity> visitRepository)
        {
            _repository = repository;
            _visitRepository = visitRepository;
        }

        public async Task<VisitEntity> ExecuteAsync(RegisterExitDTO dto)
        {
            VisitEntity? visit;
            if (dto.VisitId.HasValue)
            {
                visit = await _repository.FindByIdAsync(dto.VisitId.Value);

                if (visit == null)
                {
                    throw new InvalidOperationException($"No se encontró una visita con Id {dto.VisitId}");
                }
            }
            else if (!string.IsNullOrWhiteSpace(dto.Code))
            {
                visit = await _visitRepository.GetActiveVisitByPersonCodeAsync(dto.Code);

                if (visit == null)
                {
                    throw new InvalidOperationException($"No se encontró una visita con Code {dto.Code}");
                }
            }
            else
            {
                throw new InvalidOperationException("Debe proporcionar VisitId o Code");
            }

            visit.RegisterExit(dto.ExitTime);
            await _repository.UpdateAsync(visit);
            await _repository.SaveChangesAsync();

            return visit;
        }
    }
}
