using System.Security.Cryptography;
using System.Text;

namespace Default.Application.Utils
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

        public static async Task<string> HashPasswordAsync(string password)
        {
            return await Task.Run(() =>
            {
                byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
                byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(password),
                    salt,
                    Iterations,
                    HashAlgorithm,
                    KeySize);

                return $"{Convert.ToHexString(salt)}:{Convert.ToHexString(hash)}";
            });
        }

        public static async Task<bool> VerifyPasswordAsync(string password, string storedHash)
        {
            return await Task.Run(() =>
            {
                var parts = storedHash.Split(':');
                if (parts.Length != 2) return false;

                byte[] salt = Convert.FromHexString(parts[0]);
                byte[] hash = Convert.FromHexString(parts[1]);

                byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(password),
                    salt,
                    Iterations,
                    HashAlgorithm,
                    KeySize);

                return CryptographicOperations.FixedTimeEquals(hash, inputHash);
            });
        }
    }
}
