using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Abstractions;

namespace Application.UseCases.Persons
{
    public class DeletePersonUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _repository;

        public DeletePersonUseCase(Domain.Abstractions.IRepository<PersonEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Guid id)
        {
            var person = await _repository.FindByIdAsync(id);
            if (person == null)
            {
                throw new ArgumentException($"No se encontró el usuario con id {id}");
            }
            await _repository.DeleteAsync(person);
            await _repository.SaveChangesAsync();
        }
    }
}
