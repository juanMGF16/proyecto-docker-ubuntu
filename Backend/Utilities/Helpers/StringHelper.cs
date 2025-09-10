using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities.Helpers
{
    public static class StringHelper
    {
        public static string Normalize(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            // Quitar tildes (acentos) usando normalización Unicode
            string normalized = input
                .Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .Aggregate("", (s, c) => s + c);

            // Quitar puntuación (comas, puntos, etc.)
            normalized = new string(normalized
                .Where(c => !char.IsPunctuation(c))
                .ToArray());

            // Quitar espacios y convertir a minúsculas
            return normalized.Trim().ToLowerInvariant().Replace(" ", "");
        }

        public static bool IsValidEmail(string email)
            => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public static bool EqualsNormalized(string a, string b)
            => Normalize(a) == Normalize(b);
    }
}