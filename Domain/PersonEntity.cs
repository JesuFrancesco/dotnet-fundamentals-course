using System.Text.RegularExpressions;

namespace Domain
{
    public class PersonEntity
    {
        public PersonEntity(string code, string firstName, string lastName, string email, string phoneNumber)
        {
            ValidateCode(code);
            ValidateEmail(email);

            Id = Guid.NewGuid();
            Code = code.Trim().ToUpper();
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim().ToLower();
            PhoneNumber = phoneNumber.Trim();
        }

        public Guid Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email{ get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        public void UpdatePersonalInfo(string firstName, string lastName, string email, string phoneNumber) { 
            ValidateCode(Code);
            ValidateEmail(email);

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim().ToLower();
            PhoneNumber = phoneNumber.Trim();
        }


        private void ValidateCode(string code)
        {
            if(string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("El código no puede estar vacío", nameof(code));
            }

            if (code.Trim().Length < 3) {
                throw new ArgumentException("El código no puede ser de menos de 3 caracteres", nameof(code));
            }

            if (code.Trim().Length > 30)
            {
                throw new ArgumentException("El código no puede ser de más de 30 caracteres", nameof(code));
            }
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(nameof(email));
            }

            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                throw new ArgumentException("El formato del correo es inválido", nameof(email));
            }
        }

    }
}
