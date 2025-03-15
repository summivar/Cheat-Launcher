namespace Cheat_Launcher.Data.SecureStorage;

public interface ISecureStorage
{
    void Save(string key, string value);
    string Load(string key);
    void Delete(string key);
}