using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Abstractions;

namespace Application.UseCases.Persons
{
    public class GetPersonByCodeUseCase
    {
        private readonly ICodeRepository<PersonEntity> _codeRepository;

        public GetPersonByCodeUseCase(ICodeRepository<PersonEntity> codeRepository)
        {
            _codeRepository = codeRepository;
        }

        public async Task<PersonEntity> ExecuteAsync(string code)
        {
            var person = await _codeRepository.GetByCodeAsync(code);

            if (person == null)
            {
                throw new InvalidOperationException($"No se encontró persona con código {code}");
            }
            return person;
        }
    }
}
