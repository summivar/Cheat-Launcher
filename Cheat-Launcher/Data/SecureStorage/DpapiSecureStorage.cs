using System.IO;
using System.Security.Cryptography;

namespace Cheat_Launcher.Data.SecureStorage;

public class DpapiSecureStorage : ISecureStorage
{
    private readonly string _storagePath;

    public DpapiSecureStorage(string storagePath)
    {
        _storagePath = storagePath;
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public void Save(string key, string value)
    {
        var filePath = GetFilePath(key);
        var encryptedData = ProtectedData.Protect(
            System.Text.Encoding.UTF8.GetBytes(value),
            null,
            DataProtectionScope.CurrentUser);
        File.WriteAllBytes(filePath, encryptedData);
    }

    public string Load(string key)
    {
        var filePath = GetFilePath(key);
        if (!File.Exists(filePath)) throw new FileNotFoundException($"No data found for key: {key}");

        var encryptedData = File.ReadAllBytes(filePath);
        var decryptedData = ProtectedData.Unprotect(
            encryptedData,
            null,
            DataProtectionScope.CurrentUser);
        return System.Text.Encoding.UTF8.GetString(decryptedData);
    }

    public void Delete(string key)
    {
        var filePath = GetFilePath(key);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private string GetFilePath(string key)
    {
        return Path.Combine(_storagePath, $"{key}.bin");
    }
}