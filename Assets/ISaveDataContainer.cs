using System;

public interface ISaveDataContainer<T> : ISaveDataContainer where T : ISavedData
{
    T ReadData();        
}

public interface ISaveDataContainer
{
    event Action DataLoaded;
    bool IsChanged { get; }
    void Save();
    void Load();

}
