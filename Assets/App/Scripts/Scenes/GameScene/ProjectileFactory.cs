using UnityEngine;

public class ProjectileFactory
{
    private readonly DestroyLine _destroyLine;
    private readonly ProjectileContainer _projectileContainer;

    public ProjectileFactory(DestroyLine destroyLine, ProjectileContainer projectileContainer)
    {
        _destroyLine = destroyLine;
        _projectileContainer = projectileContainer;
    }

    public GameObject CreateFruit(Vector2 position)
    {
        GameObject fruit = GameObject.Instantiate((GameObject)Resources.Load(ResourcePathes.BaseFruitPath));
        fruit.transform.SetParent(_projectileContainer.transform);
        fruit.transform.position = position;
        _destroyLine.AddLineDestroyListener(fruit.transform);
        return fruit;
    }
}
