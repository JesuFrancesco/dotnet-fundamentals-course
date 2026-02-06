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
    public class CreatePersonUseCase
    {
        private readonly IRepository<PersonEntity, Guid> _repository;
        private readonly ICodeRepository<PersonEntity> _codeRepository;

        public CreatePersonUseCase(IRepository<PersonEntity, Guid> repository, ICodeRepository<PersonEntity> codeRepository)
        {
            _repository = repository;
            _codeRepository = codeRepository;
        }

        public async Task<PersonEntity> ExecuteAsync(CreatePersonDTO dto)
        {
            if (await _codeRepository.ExistsWithCodeAsync(dto.Code)) {
                throw new InvalidOperationException($"Ya existe un usuario con código {dto.Code}");
            }

            var person = new PersonEntity(
                dto.Code, dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber
            );

            await _repository.AddAsync(person);
            await _repository.SaveChangesAsync();
            return person;
        }
    }
}
