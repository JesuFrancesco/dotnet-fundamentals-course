using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Abstractions;

namespace Application.UseCases.Persons
{
    public class GetAllPersonsUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _repository;
        public GetAllPersonsUseCase(IRepository<PersonEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PersonEntity>> ExecuteAsync()
        {
            return await _repository.FindAllAsync();
        }
    }
}
