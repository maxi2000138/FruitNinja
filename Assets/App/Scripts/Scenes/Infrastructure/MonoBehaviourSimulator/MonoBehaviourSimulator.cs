using System.Collections.Generic;

public class MonoBehaviourSimulator
{
    private readonly List<IInitializable> _initializables = new();
    private readonly List<IUpdatable> _updatables = new();

    public void InitializeAll()
    {
        foreach (IInitializable initializable in _initializables)
        {
            initializable?.Initialize();
        }
    }

    public void UpdateAll(float deltaTime)
    {
        foreach (IUpdatable updatable in _updatables)
        {
            updatable?.Update(deltaTime);
        }
    }

    public void AddInitializable(IInitializable initializable) => 
        _initializables.Add(initializable);

    public void AddUpdatable(IUpdatable updatable) => 
        _updatables.Add(updatable);
}