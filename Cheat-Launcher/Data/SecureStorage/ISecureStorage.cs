namespace Cheat_Launcher.Data.SecureStorage;

public interface ISecureStorage
{
    void Save<T>(string key, T value);
    T? Load<T>(string key);
    void Delete(string key);
}