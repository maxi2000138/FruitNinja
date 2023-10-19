public interface ISaveLoadService
{
    void SaveProgress<T>(T state, string key);
    T LoadProgress<T>(string key);
}
