using System.Security.Cryptography;

namespace WebApplication1_Test
{
    public static class Hasher
    {
        private const int saltSize = 128;
        private const int keySize = 256;
        private const int iterations = 100;

        public static string Hash(string value)
        {
            using (Rfc2898DeriveBytes algorithm = new Rfc2898DeriveBytes(value, saltSize, iterations, HashAlgorithmName.SHA256))
            {
                string key = Convert.ToBase64String(algorithm.GetBytes(keySize));
                string salt = Convert.ToBase64String(algorithm.Salt);

                return $"{salt}.{key}";
            }
        }

        public static bool Check(string hash, string value)
        {
            var parts = hash.Split('.', 2);

            if (parts.Length != 2)
            {
                throw new FormatException("Неверный формат, должен быть 'соль.хэш'.");
            }

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] key = Convert.FromBase64String(parts[1]);

            using (var algorithm = new Rfc2898DeriveBytes(value, salt, iterations, HashAlgorithmName.SHA256))
            {
                return algorithm.GetBytes(keySize).SequenceEqual(key);
            }
        }
    }
}
