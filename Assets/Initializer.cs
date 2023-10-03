using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    private readonly List<IInitializable> _initializables = new();

    public void Initialize()
    {
        foreach (IInitializable initializable in _initializables)
        {
            initializable.Initialize();
        }
    }

    public void AddInitializable(IInitializable initializable)
    {
        _initializables.Add(initializable);
    }
}
