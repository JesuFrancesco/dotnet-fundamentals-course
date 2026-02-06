using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Persons;
using Domain;
using Domain.Abstractions;

namespace Application.UseCases.Persons
{
    public class UpdatePersonUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _repository;

        public UpdatePersonUseCase(IRepository<PersonEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<PersonEntity> ExecuteAsync(UpdatePersonDTO dto)
        {
            var person = await _repository.FindByIdAsync(dto.Id);

            if (person == null)
            {
                throw new InvalidOperationException($"No existe usuario con id {dto.Id}");
            }

            person.UpdatePersonalInfo(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber);

            await _repository.UpdateAsync(person);
            await _repository.SaveChangesAsync();

            return person;
        }
    }
}
