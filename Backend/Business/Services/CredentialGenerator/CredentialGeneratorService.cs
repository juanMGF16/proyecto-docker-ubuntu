using System.Globalization;
using System.Text;
using Business.Services.CredentialGenerator.Interfaces;

namespace Business.Services.CredentialGenerator
{
    public class CredentialGeneratorService : ICredentialGeneratorService
    {
        public (string username, string password) GenerateCredentials(string name, string lastName, string email)
        {
            var username = GenerateUsername(name, lastName);
            var password = GenerateRandomPassword(name);

            return (username, password);
        }

        private string GenerateUsername(string name, string lastName)
        {
            var random = new Random();

            // Eliminar tildes/acentos
            string normalizedLastName = RemoveDiacritics(lastName.ToLower());
            string normalizedName = RemoveDiacritics(name.ToLower());

            // Primera letra del nombre + apellido + 4 dígitos aleatorios
            var digits = random.Next(1000, 9999);
            return $"{normalizedName[0]}{normalizedLastName}{digits}";
        }

        private string GenerateRandomPassword(string name)
        {
            var random = new Random();
            string[] symbols = { "!", "@", "#", "$", "%" };

            var capitalized = char.ToUpper(name[0]) + name.Substring(1).ToLower();
            var number = random.Next(100, 999);
            var symbol = symbols[random.Next(symbols.Length)];

            // Ejemplo: "Carlos482!"
            return $"{capitalized}{number}{symbol}";
        }

        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
