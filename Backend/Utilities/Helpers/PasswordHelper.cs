using Microsoft.AspNetCore.Identity;

namespace Utilities.Helpers
{
    public static class PasswordHelper
    {
        private static readonly object _dummyUser = new();
        private static readonly PasswordHasher<object> _hasher = new();

        public static string Hash(string password)
            => _hasher.HashPassword(_dummyUser, password);

        public static bool Verify(string hashedPassword, string providedPassword)
            => _hasher.VerifyHashedPassword(_dummyUser, hashedPassword, providedPassword)
                == PasswordVerificationResult.Success;
    }
}