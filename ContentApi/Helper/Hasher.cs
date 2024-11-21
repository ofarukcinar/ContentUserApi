using System;
using System.Security.Cryptography;

namespace ContentApi.Helper
{
    public sealed class Hasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32; // Daha uzun hash için
        private const int HashIter = 10000;

        private readonly byte[] _salt, _hash;

        public Hasher(string password)
        {
            _salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(_salt);

            using (var rfc2898 = new Rfc2898DeriveBytes(password, _salt, HashIter, HashAlgorithmName.SHA256))
            {
                _hash = rfc2898.GetBytes(HashSize);
            }
        }

        public Hasher(byte[] hashBytes)
        {
            if (hashBytes.Length != SaltSize + HashSize)
                throw new ArgumentException("Invalid hash size.");

            _salt = new byte[SaltSize];
            _hash = new byte[HashSize];
            Array.Copy(hashBytes, 0, _salt, 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, _hash, 0, HashSize);
        }

        public Hasher(byte[] salt, byte[] hash)
        {
            if (salt.Length != SaltSize || hash.Length != HashSize)
                throw new ArgumentException("Invalid salt or hash size.");

            _salt = new byte[SaltSize];
            _hash = new byte[HashSize];
            Array.Copy(salt, 0, _salt, 0, SaltSize);
            Array.Copy(hash, 0, _hash, 0, HashSize);
        }

        public byte[] Salt => (byte[])_salt.Clone();
        public byte[] Hash => (byte[])_hash.Clone();

        public byte[] ToArray()
        {
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
            return hashBytes;
        }

        public bool Verify(string password)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(password, _salt, HashIter, HashAlgorithmName.SHA256))
            {
                var test = rfc2898.GetBytes(HashSize);
                for (var i = 0; i < HashSize; i++)
                    if (test[i] != _hash[i])
                        return false;
            }
            return true;
        }
    }
}