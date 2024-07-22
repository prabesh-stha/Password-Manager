using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;

    public class EncryptionService
    {
        private readonly IDataProtector _dataProtector;

        public EncryptionService()
        {
            // Configure data protection
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection();
            var services = serviceCollection.BuildServiceProvider();

            var dataProtectionProvider = services.GetService<IDataProtectionProvider>();
            _dataProtector = dataProtectionProvider.CreateProtector("PasswordEncryption");
        }

        public string Encrypt(string plaintext)
        {
            return _dataProtector.Protect(plaintext);
        }

        public string Decrypt(string encryptedText)
        {
            return _dataProtector.Unprotect(encryptedText);
        }
    }