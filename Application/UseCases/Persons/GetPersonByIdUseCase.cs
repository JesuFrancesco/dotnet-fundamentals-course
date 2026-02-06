using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Abstractions;

namespace Application.UseCases.Persons
{
    public class GetPersonByIdUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _repository;

        public GetPersonByIdUseCase(IRepository<PersonEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PersonEntity> ExecuteAsync(Guid id)
        {
            var person = await _repository.FindByIdAsync(id);

            if (person == null)
            {
                throw new InvalidOperationException($"No se pudo encontrar usuario con id {id}");
            }

            return person;
        }
    }
}
