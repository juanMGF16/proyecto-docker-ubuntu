using Utilities.Exceptions;

namespace Utilities.Helpers
{
    public static class ValidationHelper
    {
        public static void ThrowIfNull<T>(T? obj, string paramName) where T : class
        {
            if (obj == null)
                throw new ValidationException(paramName, $"{paramName} no puede ser nulo.");
        }

        public static void ThrowIfEmpty(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException(paramName, $"{paramName} no puede estar vacío.");
        }

        public static void EnsureValidId(int id, string paramName)
        {
            if (id <= 0)
                throw new ValidationException(paramName, "El ID debe ser mayor que cero.");
        }
    }
}
